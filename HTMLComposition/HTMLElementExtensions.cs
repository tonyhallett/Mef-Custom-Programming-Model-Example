using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTMLComposition
{
    public static class HTMLElementExtensions
    {
        public static HtmlElement Clone(this HtmlElement element)
        {
            var document = element.Document;
            var container = document.CreateElement("div");
            container.InnerHtml = element.OuterHtml;
            return container.FirstChild;
        }
    }
}
