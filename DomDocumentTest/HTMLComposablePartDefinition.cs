using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace DomDocumentTest
{

    public class HTMLComposablePartDefinition : ComposablePartDefinition
    {
        private const string exportAttributeName = "export";
        private const string importAttributeName = "import";
        private const string isRecomposableName = "isRecomposable";
        private const string cardinalityName = "cardinality";
        private const string importTagTypeName = "importTagType";
        private const ImportCardinality defaultCardinality = ImportCardinality.ExactlyOne;
        
        
        internal string exportIdentifier;
        
        private bool isImportExport;
        

        private List<HtmlExport> exports = new List<HtmlExport>();
        private HtmlExport htmlExport;

        public HtmlExport GetExportedValue()
        {
            //Debug.WriteLine("GetExportedValue for - " + exportIdentifier);
            //HtmlElement exportedValue = null;
            //if (isImport)
            //{
            //    exportedValue = CloneExport();

            //    if (isImportExport)
            //    {
            //        var importDefinition = (HTMLImportDefinition)this.ImportDefinitions.First();
            //        importDefinition.AddImportElement(exportedValue);
            //    }
            //}
            //else
            //{
            //    exportedValue = export.Clone();//CloneExport
            //}
            //var htmlExport = new HtmlExport(exportedValue);
            //exports.Add(htmlExport);
            //return htmlExport;
            return htmlExport;
        }
       
        public override string ToString()
        {
            return exportIdentifier;
        }
        public HTMLComposablePartDefinition(HtmlElement export)
        {
            
            this.htmlExport = new HtmlExport(export);

            string exportAttributeValue = export.GetAttribute(exportAttributeName);
            bool hasExports = !String.IsNullOrEmpty(exportAttributeValue);
            if (hasExports)
            {
                this.exportIdentifier = exportAttributeValue;
            }
            else
            {
                this.exportIdentifier = "RootPage";
            }
            Debug.WriteLine("Part definition ctor " + this.exportIdentifier);
            if (hasExports)
            {
                var exports = exportAttributeValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string exportTagType = export.TagName;
                foreach (var exportName in exports)
                {
                    Debug.WriteLine("Creating export definition for " + exportName);
                    exportDefinitions.Add(new HTMLExportDefinition(exportTagType, exportName, new Dictionary<string, object>()));
                }
            }

            var exportImportAttributeValue = export.GetAttribute(importAttributeName);
            if (exportImportAttributeValue != "")
            {
                isImportExport = true;
                CreateImportDefinition(export, exportImportAttributeValue, isImportExport);
            }
            else
            {
                var importElements = export.All.OfType<HtmlElement>().Select(e =>
                {
                    var importAttributeValue = e.GetAttribute(importAttributeName);
                    return new { Element = e, IsImport = importAttributeValue != "", ImportAttributeValue = importAttributeValue };
                }).Where(a => a.IsImport);


                foreach (var importElement in importElements)
                {
                    CreateImportDefinition(importElement.Element, importElement.ImportAttributeValue, false);
                }
            }
        }

        public void SetImport(ImportDefinition importDefinition, List<HtmlExport> htmlExports)
        {
            var htmlImportDefinition = (HTMLImportDefinition)importDefinition;
            htmlImportDefinition.SetImport(htmlExports);
            htmlExport.RaiseElementChanged();
        }
        private void CreateImportDefinition(HtmlElement importElement, string importAttributeValue, bool isImportExport)
        {
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
            
            #region old way
            //if (isImportExport)
            //{
            //    if (importTagTypeAttributeValue != "")
            //    {
            //        htmlNodeAny = false;
            //        htmlTagType = importTagTypeAttributeValue.ToUpper();
            //    }

            //}
            //else
            //{
            //    var importTagType = importElement.TagName;
            //    if (importTagType == "DIV")
            //    {
            //        if (importTagTypeAttributeValue != "")
            //        {
            //            htmlNodeAny = false;
            //            htmlTagType = importTagTypeAttributeValue.ToUpper();
            //        }
            //    }
            //    else
            //    {
            //        htmlNodeAny = false;
            //        if (importTagTypeAttributeValue != "")
            //        {
            //            htmlTagType = importTagTypeAttributeValue.ToUpper();
            //        }
            //        else
            //        {
            //            htmlTagType = importTagType;
            //        }

            //    }
            //}
            #endregion
            
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
            Debug.WriteLine("Creating import definition - " + toString);
            HTMLImportDefinition importDefinition = new HTMLImportDefinition(toString, importElement, constraint, contractName, importCardinality, isRecomposable, false);
            importDefinition.ImportsChanged += ImportDefinition_ImportsChanged;
            importDefinitions.Add(importDefinition);

        }

        private void ImportDefinition_ImportsChanged(object sender, EventArgs e)
        {
            htmlExport.RaiseElementChanged();
        }

        private List<ExportDefinition> exportDefinitions = new List<ExportDefinition>();
        private List<HTMLImportDefinition> importDefinitions = new List<HTMLImportDefinition>();

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
            return new HTMLComposablePart(this);
        }
    }
}