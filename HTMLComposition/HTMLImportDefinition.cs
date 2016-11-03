using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace HTMLComposition
{
    public class HTMLImportDefinition : ImportDefinition
    {
        private const string importAttributeName = "import";
        private const string isRecomposableName = "isRecomposable";
        private const string cardinalityName = "cardinality";
        private const string importTagTypeName = "importTagType";
        private const ImportCardinality defaultCardinality = ImportCardinality.ExactlyOne;
        private Dictionary<HtmlElement, HtmlElement> exportImports = new Dictionary<HtmlElement, HtmlElement>();
        private string toString;

        internal static void SetImports(HtmlElement clonedExport, List<HTMLImportDefinition> importDefinitions)
        {
            if (importDefinitions.Count == 1)
            {
                importDefinitions[0].exportImports.Add(clonedExport, clonedExport);
            }else
            {
                int importIndex = 0;
                WalkImports(clonedExport, (e) =>
                {
                    importDefinitions[importIndex].exportImports.Add(clonedExport, e);
                    importIndex++;
                });
            }
        }
        internal static IEnumerable<HTMLImportDefinition> CreateImports(HtmlElement exportElement)
        {
            List<HTMLImportDefinition> importDefinitions = null;
            var exportImportAttributeValue = exportElement.GetAttribute(importAttributeName);
            if (exportImportAttributeValue != "")
            {
                importDefinitions= new List<HTMLImportDefinition> { CreateImportDefinition(exportElement) };
            }
            else
            {
                
                importDefinitions = new List<HTMLImportDefinition>();
                Action<HtmlElement> createImportDefinitionCallback = (e) =>
                {
                    importDefinitions.Add(CreateImportDefinition(e));
                };
                WalkImports(exportElement, createImportDefinitionCallback);
            }
            return importDefinitions;
        }
        private static void WalkImports(HtmlElement element,Action<HtmlElement> importElementCallback)
        {
            var children = element.Children.OfType<HtmlElement>();
            foreach(var child in children)
            {
                if (IsImport(child))
                {
                    importElementCallback(child);
                }else
                {
                    WalkImports(child,importElementCallback);
                }
            }
        }
        private static bool IsImport(HtmlElement element)
        {
            var importAttributeValue = element.GetAttribute(importAttributeName);
            return  importAttributeValue != "";
        }
        private static HTMLImportDefinition CreateImportDefinition(HtmlElement importElement)
        {
            string importAttributeValue = importElement.GetAttribute(importAttributeName);
            bool isRecomposable = false;

            var isRecomposableAttributeValue = importElement.GetAttribute(isRecomposableName);
            if (isRecomposableAttributeValue != "")
            {
                isRecomposable = true;
            }

            ImportCardinality importCardinality = defaultCardinality;

            var importCardinalityAttributeValue = importElement.GetAttribute(cardinalityName);
            if (!String.IsNullOrEmpty(importCardinalityAttributeValue))
            {
                var parsed = Enum.TryParse(importCardinalityAttributeValue, out importCardinality);
            }

            string contractName = importAttributeValue;


            string htmlTagType = "Any";
            bool htmlNodeAny = true;
            var importTagTypeAttributeValue = importElement.GetAttribute(importTagTypeName);

            var importTagType = importElement.TagName;
            if (importTagType == "DIV")
            {
                if (importTagTypeAttributeValue != "")
                {
                    htmlNodeAny = false;
                    htmlTagType = importTagTypeAttributeValue.ToUpper();
                }
            }
            else
            {
                htmlNodeAny = false;
                if (importTagTypeAttributeValue != "")
                {
                    htmlTagType = importTagTypeAttributeValue.ToUpper();
                }
                else
                {
                    htmlTagType = importTagType;
                }

            }


            Expression<Func<ExportDefinition, bool>> constraint = (ed) => ((ed is HTMLExportDefinition) ? ((HTMLExportDefinition)ed).ContractName == contractName && (htmlNodeAny == true ? htmlNodeAny : ((HTMLExportDefinition)ed).HtmlNodeType == htmlTagType) : false);


            string toString = "ContractName: " + contractName + ", TagType: " + (htmlNodeAny ? "Any" : htmlTagType) + ", Cardinality: " + importCardinality.ToString() + ", allows recomposition: " + isRecomposable.ToString();

            return new HTMLImportDefinition(toString, constraint, contractName, importCardinality, isRecomposable, false);
            

        } 
        
        
        private HTMLImportDefinition(string toString, Expression<Func<ExportDefinition, bool>> constraint, string contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite) : base(constraint, contractName, cardinality, isRecomposable, isPrerequisite)
        {
            this.toString = toString;
        }

        public override string ToString()
        {
            return toString;
        }
        
        internal void SetImports(HtmlElement exportedElement, IEnumerable<Export> exports)
        {
            var importElement = exportImports[exportedElement];
            importElement.InnerHtml = "";
            foreach (var export in exports)
            {
                var exportedHtmlElement = export.Value as HtmlElement;
                importElement.AppendChild(exportedHtmlElement);
            }
        }
        
    }
}