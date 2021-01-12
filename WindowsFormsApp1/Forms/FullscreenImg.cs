using System;
using System.Drawing;
using System.Windows.Forms;

namespace Comparer2.ListApp.Forms
{
    public partial class FullscreenImg : Form
    {

        DateTime created = DateTime.UtcNow;

        Image Image;

        public FullscreenImg(string imgPAth, Manina.Windows.Forms.ImageListViewItem selectedITem)
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = Image = Image.FromFile(imgPAth);
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
        }


        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Select();
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            pictureBox1.Width += e.Delta;
            pictureBox1.Height += e.Delta;
            pictureBox1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (DateTime.UtcNow - created > TimeSpan.FromMilliseconds(500))
                Close();
        }

        private void pictureBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Close();
        }

        private void FullscreenImg_KeyPress(object sender, KeyPressEventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.RotateFlip((RotateFlipType.Rotate90FlipNone));
            pictureBox1.Refresh();
        }
    }
}
