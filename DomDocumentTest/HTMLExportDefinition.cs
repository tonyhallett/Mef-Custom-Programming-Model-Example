using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace DomDocumentTest
{
    public class HTMLExportDefinition : ExportDefinition
    {
        public string HtmlNodeType { get; set; }
        public HTMLExportDefinition(string htmlNodeType, string contractName, IDictionary<string, object> metadata) : base(contractName, metadata)
        {
            HtmlNodeType = htmlNodeType;
        }

    }
}