using System;
using System.Windows.Forms;

namespace DomDocumentTest
{
    public class HtmlExport
    {
        public event EventHandler ElementChanged;
        public HtmlExport(HtmlElement element)
        {
            this.Element = element;
        }
        public void RaiseElementChanged()
        {
             ElementChanged?.Invoke(this, EventArgs.Empty);  
        }
        public HtmlElement Element { get; private set; }
    }
}