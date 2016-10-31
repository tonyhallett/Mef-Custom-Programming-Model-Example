using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;

namespace DomDocumentTest
{

    public class HTMLComposablePart : ComposablePart
    {
        private HTMLComposablePartDefinition partDefinition;
        public override string ToString()
        {
            return partDefinition.ToString();
        }
        internal HTMLComposablePart(HTMLComposablePartDefinition definition)
        {
            this.partDefinition = definition;
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
                return partDefinition.GetExportedValue();
            }
            throw new ArgumentException();
        }
        public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
        {
            if (ImportDefinitions.Contains(definition))
            {
                var htmlExports = exports.Select(e => e.Value as HtmlExport).ToList();
                partDefinition.SetImport(definition, htmlExports);
            }
        }
    }
}