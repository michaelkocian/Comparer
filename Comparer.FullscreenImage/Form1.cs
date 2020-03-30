using Cyotek.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace Comparer.FullscreenImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            var imbox = new ImageBox();
            imbox.Size = new System.Drawing.Size(500, 500);
            imbox.Image = Bitmap.FromFile(@"D:\zaloha telefonu\Camera\20170903_222301.jpg");
            imbox.Dock = DockStyle.Fill;
            this.Controls.Add(imbox);


        }

    }
}
