using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows.Forms;

namespace HTMLComposition
{
    public class HTMLExportDefinition : ExportDefinition
    {
        public string HtmlNodeType { get; set; }
        private const string exportAttributeName = "export";
        private HTMLExportDefinition(string htmlNodeType, string contractName, IDictionary<string, object> metadata) : base(contractName, metadata)
        {
            HtmlNodeType = htmlNodeType;
        }
        
        internal static IEnumerable<ExportDefinition> CreateExports(HtmlElement export)
        {
            string exportAttributeValue = export.GetAttribute(exportAttributeName);
            bool hasExports = !String.IsNullOrEmpty(exportAttributeValue);
            
            IEnumerable<HTMLExportDefinition> exportDefinitions = Enumerable.Empty<HTMLExportDefinition>();
            
            if (hasExports)
            {
                var exports = exportAttributeValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string exportTagType = export.TagName;
                exportDefinitions = exports.Select(e =>
                 {
                     return new HTMLExportDefinition(exportTagType, e, new Dictionary<string, object>());
                });
                
            }
            return exportDefinitions;
        }
    }
}