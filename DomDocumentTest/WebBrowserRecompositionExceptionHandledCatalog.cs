using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Forms;

namespace DomDocumentTest
{
    //could just have a boolean for allow recomposition exceptions
    public class WebBrowserRecompositionExceptionHandledCatalog : WebBrowserDirectoryCatalog
    {
        public WebBrowserRecompositionExceptionHandledCatalog(HtmlDocument htmlDocument, DirectoryInfo directory, SearchOption searchOption, Func<FileInfo, bool> filter) : base(htmlDocument, directory, searchOption, filter) { }
        protected override void RecompositionException(ChangeRejectedException changeRejectedException)
        {
            var st = "";
        }
    }
}