using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace HTMLComposition
{

    public class HTMLComposablePart : ComposablePart
    {
        private HTMLComposablePartDefinition partDefinition;
        private HtmlElement exportedElement;

        public override string ToString()
        {
            return partDefinition.ToString();
        }
        internal HTMLComposablePart(HTMLComposablePartDefinition definition,HtmlElement exportedElement)
        {
            this.exportedElement = exportedElement;
            this.partDefinition = definition;
        }
        public static HTMLComposablePart CreateRootPart(HtmlElement rootElement)
        {
            return HTMLComposablePartDefinition.CreateRootPart(rootElement);
        }
        
        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get
            {
                return partDefinition.ExportDefinitions;
            }
        }
        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get
            {
                return partDefinition.ImportDefinitions;
            }
        }
        public override object GetExportedValue(ExportDefinition definition)
        {
            if (ExportDefinitions.Contains(definition))
            {
                return exportedElement;
            }
            throw new ArgumentException();
        }
        public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
        {
            if (ImportDefinitions.Contains(definition))
            {
                ((HTMLImportDefinition)definition).SetImports(exportedElement, exports);
            }
        }
    }
}