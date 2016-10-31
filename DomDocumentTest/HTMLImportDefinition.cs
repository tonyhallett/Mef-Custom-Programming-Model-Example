using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace DomDocumentTest
{
    public class HTMLImportDefinition : ImportDefinition
    {
        public HtmlElement ImportElement { get; private set; }
        
        public event EventHandler ImportsChanged;
        public override string ToString()
        {
            return toString;
        }
        private string toString;
        public HTMLImportDefinition(string toString, HtmlElement importElement, Expression<Func<ExportDefinition, bool>> constraint, string contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite) : base(constraint, contractName, cardinality, isRecomposable, isPrerequisite)
        {
            this.toString = toString;
            ImportElement = importElement;
        }
        private List<HtmlExport> exportsForImport;
        private List<IHTMLDOMNode> clonedImports;
        public void SetImport(List<HtmlExport> htmlExports)
        {
            if (exportsForImport != null)
            {
                foreach(var exportForImport in exportsForImport)
                {
                    exportForImport.ElementChanged -= HtmlExport_ElementChanged;
                }
            }
            exportsForImport = new List<HtmlExport>(htmlExports);
            foreach (var htmlExport in htmlExports)
            {
                htmlExport.ElementChanged += HtmlExport_ElementChanged;
            }
            SetImports();   
        }
        
        private void SetImports()
        {
            clonedImports = new List<IHTMLDOMNode>();

            ImportElement.InnerHtml = "";
            var domImportElement = (IHTMLDOMNode)ImportElement.DomElement;
            
            foreach (var htmlExport in exportsForImport)
            {
                var exportForImportClone = CloneElement(htmlExport.Element);
                clonedImports.Add(exportForImportClone);
                domImportElement.appendChild(exportForImportClone);
            }
        }

        private void HtmlExport_ElementChanged(object sender, EventArgs e)
        {
            var htmlExport = sender as HtmlExport;
            var previousIndex = exportsForImport.IndexOf(htmlExport);
            var previousImport = clonedImports[previousIndex];
            var newImport = CloneElement(htmlExport.Element);
            previousImport.replaceNode(newImport);
            clonedImports[previousIndex] = newImport;
            ImportsChanged?.Invoke(this, EventArgs.Empty);
        }
        private IHTMLDOMNode CloneElement(HtmlElement elementToClone)
        {
            IHTMLDOMNode clone = null;
            var domNode = (IHTMLDOMNode)elementToClone.DomElement;
            clone = domNode.cloneNode(true);
          
            return clone;
        }
    }
}