using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace HTMLComposition
{

    public class HTMLComposablePartDefinition : ComposablePartDefinition
    {
        private const string rootIdentifier = "Root";
        private string toString;
        public override string ToString()
        {
            return toString;
        }

        private HtmlElement export;
        private List<ExportDefinition> exportDefinitions;
        private List<HTMLImportDefinition> importDefinitions;
        private Dictionary<string, object> metadata;


        internal static HTMLComposablePart CreateRootPart(HtmlElement rootElement)
        {
            var partDefinition = new HTMLComposablePartDefinition(rootElement);
            partDefinition.toString = rootIdentifier;
            return partDefinition.CreateRootPart();
        }


        public HTMLComposablePartDefinition(HtmlElement export)
        {
            metadata = new Dictionary<string, object>();
            metadata.Add("System.ComponentModel.Composition.CreationPolicy", CreationPolicy.NonShared);
            this.export = export;
            SetToStringForNonRoot();
            exportDefinitions = HTMLExportDefinition.CreateExports(export).ToList();
            importDefinitions = HTMLImportDefinition.CreateImports(export).ToList();
        }
        private void SetToStringForNonRoot()
        {
            var domNode = export.DomElement as IHTMLDOMNode;
            var attributes = (domNode.attributes as IHTMLAttributeCollection).Cast<IHTMLDOMAttribute>().Where(a => a.specified);
            StringBuilder sb = new StringBuilder();
            foreach(IHTMLDOMAttribute attribute in attributes)
            {
                sb.Append(attribute.nodeName + "=" + attribute.nodeValue + " ");
            }
            toString = sb.ToString().Trim();
        }

        public override IDictionary<string, object> Metadata
        {
            get
            {
                return metadata;
            }
        }
        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get
            {
                return exportDefinitions;
            }
        }
        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get
            {
                return importDefinitions;
            }
        }
        public override ComposablePart CreatePart()
        {
            var clonedExport = export.Clone();
            return CreateHTMLComposablePart(clonedExport);
            
        }

        private HTMLComposablePart CreateRootPart()
        {
            return CreateHTMLComposablePart(export);
        }
        private HTMLComposablePart CreateHTMLComposablePart(HtmlElement root)
        {
            HTMLImportDefinition.SetImports(root, importDefinitions);
            return new HTMLComposablePart(this, root);
        }
        
    }
}