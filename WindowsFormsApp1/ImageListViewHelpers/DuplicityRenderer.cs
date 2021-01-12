using Manina.Windows.Forms;
using Manina.Windows.Forms.ImageListViewRenderers;
using System.Collections.Generic;
using System.Drawing;

namespace Comparer2.ListApp.ImageListViewHelpers
{
    public class DuplicityRenderer : DefaultRenderer
    {

        Brush b = Brushes.Red;
        Font f = new Font("Arial", 14);
        Dictionary<string, Image> cache = new Dictionary<string, Image>();



        public override void DrawItem(Graphics g, ImageListViewItem item, ItemState state, Rectangle bounds)
        {
            base.DrawItem(g, item, state, bounds);

            var alternativeImg = item.SubItems["alternative"]?.Text;
            if (string.IsNullOrWhiteSpace(alternativeImg))
                return;
            if (!cache.TryGetValue(alternativeImg, out Image icon))
            {
                using (Bitmap original = (Bitmap)Image.FromFile(alternativeImg))
                {
                    icon = new Bitmap(original, new Size(original.Width / 10, original.Height / 10));
                    cache.Add(alternativeImg, icon);
                }
            }
            if (icon != null)
            {
                string text = item.SubItems["comparison"]?.Text;

                Rectangle newRect = new Rectangle(bounds.X + bounds.Width * 2 / 3, bounds.Y, bounds.Width / 3, bounds.Height * 5 / 6);
                g.SetClip(newRect);

                float ratio = (float)icon.Width / (float)icon.Height;

                Rectangle imageRect = new Rectangle(newRect.X, newRect.Y + 20, (int)((float)((float)(newRect.Height - 20)) * ratio), newRect.Height - 20);
                g.DrawImage(icon, imageRect);
                g.DrawString(text, f, b, newRect);
            }
        }

    }
}
