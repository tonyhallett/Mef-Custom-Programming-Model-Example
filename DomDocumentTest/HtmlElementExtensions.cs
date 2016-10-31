using System.Windows.Forms;

namespace DomDocumentTest
{
    public static class HtmlElementExtensions
    {
        public static void Replace(this HtmlElement toReplace, HtmlElement replacement)
        {
            toReplace.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, replacement);
            toReplace.OuterHtml = "";
        }
        public static HtmlElement Clone(this HtmlElement toClone)
        {
            var doc = toClone.Document;
            //am I going to have an issue with parent ?
            var tempContainer = doc.CreateElement("div");
            tempContainer.InnerHtml = toClone.OuterHtml;
            var numChildren = tempContainer.Children.Count;
            var clone = tempContainer.Children[0];
            return clone;
        }

    }
}