using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomDocumentTest
{
    public partial class Form1 : Form
    {
        private CompositionContainer container;
        private WebBrowserDirectoryCatalog htmlCatalog;

        public Form1()
        {
            InitializeComponent();
            setText = new SetTextDelegate(SetText);
            //would get this from file.....
            var mainHtml = @"<html>
                <head></head>
                <body>
                    <div style='color:red'>This is not an import or export</div>
                    <br/>
                    <!-- both of these will be matched by a single export   -->
                    <div import='multipleExport1'></div>
                    <div import='multipleExport2'></div>
                    <br/>
                    <div import='exactlyOneAllowsRecomposition' cardinality='ExactlyOne' isRecomposable='true'></div>
                    <br/>
                    <!-- through the allowed recomposition will demonstrate the cardinality of ZeroOrOne, initially matched with one -->
                    <div import='zeroOrOneAllowRecomposition' cardinality='ZeroOrOne' isRecomposable='true'></div>
                    <br/>
                    <!-- demonstrates that recomposition when not specified will throw - initially matches none -->
                    <div import='zeroOrOneDoNotAllowRecomposition' cardinality='ZeroOrOne'></div>
                    <br/>
                    <!-- both of these will be matched by exports that also have imports, the first is an import export -->
                    <div import='exportAndImportMany'></div>
                    <div import='exportWithChildImports'></div>
                    <br/>
                    <!-- this import will have an import that has already been specified - exportAndImportMany, demonstrating -->
                    <!-- recomposition with an import that has also been recomposed.  Initially matches none -->
                    <div import='importsRecomposed' isRecomposable='true' cardinality='ZeroOrOne'></div>
                </body>
            ";

            webBrowser1.DocumentText = mainHtml;
            Application.DoEvents();
            var doc = webBrowser1.Document;
            var htmlPagePart = new HTMLComposablePartDefinition(doc.Body).CreatePart();
            DirectoryInfo partsDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            htmlCatalog = new WebBrowserDirectoryCatalog(doc, partsDirectory, SearchOption.AllDirectories, (f) =>
                 {
                     return f.Extension == ".htmlPart";
                 });
            htmlCatalog.RecompositionAttemptEvent += HtmlCatalog_RecompositionAttemptEvent;
            container = new CompositionContainer(htmlCatalog);

            CompositionBatch batch = new CompositionBatch(new ComposablePart[] { htmlPagePart }, Enumerable.Empty<ComposablePart>());

            container.Compose(batch);
            

        }
        private delegate void SetTextDelegate(string text);
        private SetTextDelegate setText;
        private void HtmlCatalog_RecompositionAttemptEvent(object sender, RecompositionAttemptEventArgs e)
        {
            string recompositionFailureEventDetails = "";
            if (!e.Success)
            {
                recompositionFailureEventDetails = e.FailureDetails;
            }


            tbRecompositionEventDetails.Invoke(setText, recompositionFailureEventDetails);
            
        }
        private void SetText(string text)
        {
            tbRecompositionEventDetails.Text = text;
        }

       
    }
}
