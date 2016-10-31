using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DomDocumentTest
{
    public class WebBrowserDirectoryCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        private WebBrowserHtmlCatalog innerCatalog;
        private FileSystemWatcher watcher;
        private Func<FileInfo, bool> filter;
        private bool throwOnRecompositionException;

        public event EventHandler<RecompositionAttemptEventArgs> RecompositionAttemptEvent;
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

        public WebBrowserDirectoryCatalog(HtmlDocument htmlDocument, DirectoryInfo directory, SearchOption searchOption, Func<FileInfo, bool> filter,bool throwOnRecompositionException=false)
        {
            this.throwOnRecompositionException = throwOnRecompositionException;
            this.filter = filter;
            var files = directory.EnumerateFiles("*", searchOption);
            var filteredFiles = files.Where(f => filter(f));
            watcher = new FileSystemWatcher(directory.FullName);
            watcher.EnableRaisingEvents = true;
            watcher.Created += Watcher_Created;
            watcher.Deleted += Watcher_Deleted;
            this.innerCatalog = new WebBrowserHtmlCatalog(htmlDocument, filteredFiles.Select(f =>
            {
                var streamReader = new StreamReader(f.FullName);
                var html = streamReader.ReadToEnd();
                streamReader.Dispose();
                return new Tuple<string, string>(f.FullName, html);
            }).ToArray());
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (filter(new FileInfo(e.FullPath)))
            {
                var streamReader = new StreamReader(e.FullPath);
                var html = streamReader.ReadToEnd();
                streamReader.Dispose();
                var newParts = innerCatalog.AddFragmentWithKey(e.FullPath, html, false);
                if (newParts.Count > 0)
                {
                    var changeArgs = new ComposablePartCatalogChangeEventArgs(newParts, Enumerable.Empty<ComposablePartDefinition>(), null);
                    AddOrDeleteChanging(changeArgs, () => innerCatalog.AddKeyedParts(e.FullPath, newParts), true);
                }
            }
        }
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (innerCatalog.KeyedParts.ContainsKey(e.FullPath))
            {
                var affectedParts = innerCatalog.KeyedParts[e.FullPath];
                var changeArgs = new ComposablePartCatalogChangeEventArgs(Enumerable.Empty<ComposablePartDefinition>(), affectedParts, null);
                AddOrDeleteChanging(changeArgs, () => innerCatalog.RemoveKeyedParts(e.FullPath),false);
            }
        }
        private void AddOrDeleteChanging(ComposablePartCatalogChangeEventArgs changeArgs,Action addDeleteAction,bool added)
        {
            RecompositionAttemptEventArgs recompositionEventArgs = new RecompositionAttemptEventArgs();
            bool changeException = false;
            ChangeRejectedException changeRejectedException = null;
            try
            {
                Changing?.Invoke(this, changeArgs);
                
            }
            catch (ChangeRejectedException exc)
            {
                recompositionEventArgs.FailureDetails = GetChangeRejectedExceptionMessage(exc);
                changeException = true;
            }
            
            addDeleteAction();
            Changed?.Invoke(this, changeArgs);

            if (!changeException)
            {
                recompositionEventArgs.Success = true;
            }else
            {
                RecompositionException(changeRejectedException);
            }
            if (added)
            {
                recompositionEventArgs.Reason = RecompositionEventReason.Added;
            }else
            {
                recompositionEventArgs.Reason = RecompositionEventReason.Deleted;
            }

            RecompositionAttemptEvent?.Invoke(this, recompositionEventArgs);
        }
        
        public override IEnumerator<ComposablePartDefinition> GetEnumerator()
        {
            return innerCatalog.GetEnumerator();
        }

        protected virtual void RecompositionException(ChangeRejectedException changeRejectedException)
        {
            if (throwOnRecompositionException)
            {
                throw changeRejectedException;
            }

        }
        private string GetChangeRejectedExceptionMessage(ChangeRejectedException changeRejectedException)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Errors:");
            foreach (var error in changeRejectedException.Errors)
            {
                stringBuilder.AppendLine("     " + error.Description);
            }

            if (changeRejectedException.RootCauses.Count > 0)
            {
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("Root causes:");
                foreach (var exception in changeRejectedException.RootCauses)
                {
                    string rootCauseMessage = null;

                    var compositionException = exception as CompositionException;
                    if (compositionException != null)
                    {
                        rootCauseMessage = compositionException.Message;
                    }
                    else
                    {
                        rootCauseMessage = exception.Message;
                    }
                    stringBuilder.AppendLine("     " + rootCauseMessage);
                }
            }

            return stringBuilder.ToString();
        }
    }
}