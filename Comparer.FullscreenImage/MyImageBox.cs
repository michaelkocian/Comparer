using Cyotek.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace Comparer.FullscreenImage
{
    public class MyImageBox : ImageBox
    {


        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.FillRectangle(Brushes.Transparent, pevent.ClipRectangle);
        }

    }
}
