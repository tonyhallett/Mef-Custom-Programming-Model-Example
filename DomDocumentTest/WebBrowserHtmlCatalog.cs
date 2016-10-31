
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows.Forms;

namespace DomDocumentTest
{
    public class WebBrowserHtmlCatalog : ComposablePartCatalog
    {
        private const string exportAttributeName = "export";
        private HtmlDocument doc;
        

        public List<ComposablePartDefinition> htmlParts = new List<ComposablePartDefinition>();
        private Dictionary<string, List<HTMLComposablePartDefinition>> keyedParts = new Dictionary<string, List<HTMLComposablePartDefinition>>();
        public Dictionary<string, List<HTMLComposablePartDefinition>> KeyedParts
        {
            get
            {
                return keyedParts;
            }
        }

        public WebBrowserHtmlCatalog(System.Windows.Forms.HtmlDocument doc, params string[] fragments)
        {
            foreach (var fragment in fragments)
            {
                var container = doc.CreateElement("div");
                container.InnerHtml = fragment;
                var root = container.Children[0];
                var exports = root.Children.OfType<System.Windows.Forms.HtmlElement>().Where(e => !String.IsNullOrEmpty(e.GetAttribute(exportAttributeName)));
                if (exports.Any())
                {
                    foreach (var export in exports)
                    {
                        htmlParts.Add(new HTMLComposablePartDefinition(export));
                    }
                }
            }
        }
        public WebBrowserHtmlCatalog(System.Windows.Forms.HtmlDocument doc, params Tuple<string, string>[] fragmentAndKeys)
        {
            this.doc = doc;
            foreach (var fragmentAndKey in fragmentAndKeys)
            {
                AddFragmentWithKey(fragmentAndKey.Item1, fragmentAndKey.Item2);
            }
        }
        
        public void AddKeyedParts(string key, List<HTMLComposablePartDefinition> parts)
        {
            htmlParts.AddRange(parts);
            keyedParts.Add(key, parts);
        }
        public List<HTMLComposablePartDefinition> AddFragmentWithKey(string key, string fragment, bool addToParts = true)
        {
            var container = doc.CreateElement("div");
            container.InnerHtml = fragment;
            var root = container.Children[0];
            var exports = root.Children.OfType<System.Windows.Forms.HtmlElement>().Where(e => !String.IsNullOrEmpty(e.GetAttribute(exportAttributeName)));
            List<HTMLComposablePartDefinition> parts = new List<HTMLComposablePartDefinition>();
            if (exports.Any())
            {
                foreach (var export in exports)
                {

                    var part = new HTMLComposablePartDefinition(export);

                    parts.Add(part);
                    if (addToParts)
                    {
                        htmlParts.Add(part);
                    }
                }
                if (addToParts)
                {
                    keyedParts.Add(key, parts);
                }

            }
            return parts;
        }
        
        
        public void RemoveParts(IEnumerable<ComposablePartDefinition> partsToRemove)
        {
            foreach (var part in partsToRemove)
            {
                htmlParts.Remove(part);
            }
        }
        public void RemoveKeyedParts(string key)
        {
            var parts = KeyedParts[key];
            KeyedParts.Remove(key);
            foreach (var part in parts)
            {
                htmlParts.Remove(part);
            }
        }
        public override IEnumerator<ComposablePartDefinition> GetEnumerator()
        {
            return htmlParts.GetEnumerator();
        }
    }
}