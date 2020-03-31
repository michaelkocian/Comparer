using Cyotek.Windows.Forms;
using ExifLibrary;
using System.Drawing;
using System.Windows.Forms;

namespace Comparer.FullscreenImage
{
    public partial class Form1 : Form
    {
        Button b;
        Image image;
        ImageBox imageBox;
        ImageFile ExifInfo;
        System.DateTime formLoaded;




        public Form1(string path = @"D:\zaloha telefonu\Camera\20170903_222301.jpg")
        {
            InitializeComponent();
            this.Text = path; //title

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.TransparencyKey = BackColor;

            ExifInfo = ImageFile.FromFile(path);

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            imageBox = new MyImageBox();

            imageBox.Width = this.Width;
            imageBox.Height = this.Height;
            imageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));


            var orientation = ExifInfo.Properties.Get<ExifEnumProperty<ExifLibrary.Orientation>>(ExifTag.Orientation);

            RotateFlipType type = RotateFlipType.RotateNoneFlipNone;
            switch (orientation.Value)
            {
                case ExifLibrary.Orientation.MirroredVertically:
                    type = RotateFlipType.RotateNoneFlipY;
                    break;
                case ExifLibrary.Orientation.Rotated180:
                    type = RotateFlipType.Rotate180FlipNone;
                    break;
                case ExifLibrary.Orientation.MirroredHorizontally:
                    type = RotateFlipType.RotateNoneFlipX;
                    break;
                case ExifLibrary.Orientation.RotatedLeftAndMirroredVertically:
                    type = RotateFlipType.Rotate270FlipY;
                    break;
                case ExifLibrary.Orientation.RotatedRight:
                    type = RotateFlipType.Rotate90FlipNone;
                    break;
                case ExifLibrary.Orientation.RotatedLeft:
                    type = RotateFlipType.Rotate270FlipNone;
                    break;
                case ExifLibrary.Orientation.RotatedRightAndMirroredVertically:
                    type = RotateFlipType.Rotate90FlipY;
                    break;
            }

            imageBox.Margin = new Padding(0);
            this.Padding = new Padding(0);
            // imageBox.BackColor = Color.Transparent;
            imageBox.ForeColor = Color.Transparent;
            imageBox.GridColor = Color.Transparent;
            imageBox.GridColorAlternate = Color.Transparent;


            image = Bitmap.FromFile(path);
            image.RotateFlip(type);
            imageBox.Image = image;

            b = new Button();
            b.Width = 30;
            b.Height = 30;
            b.BackColor = Color.Black;
            b.ForeColor = Color.FromArgb(255, 125, 0, 0);
            b.Text = "X";
            b.FlatStyle = FlatStyle.Flat;
            this.Controls.Add(b);
            this.Controls.Add(imageBox);



            this.Load += Form1_Load;
            //imageBox.Click += ImageBox_Click;
            b.Click += ImageBox_Click;
            imageBox.BorderStyle = BorderStyle.None;

        }

        private void ImageBox_Click(object sender, System.EventArgs e)
        {
            if (System.DateTime.UtcNow - formLoaded > System.TimeSpan.FromMilliseconds(500))
                this.Close();
        }



        private void Form1_Load(object sender, System.EventArgs e)
        {
            int zoomfactor = (this.Height /* - 40*/) * 100 / image.Height;
            imageBox.Zoom = zoomfactor;
            formLoaded = System.DateTime.UtcNow;
            b.Location = new Point(this.Width - 35, 5);
        }



        private const int WM_ACTIVATEAPP = 0x1C;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_ACTIVATEAPP)
            {
                if (m.WParam == System.IntPtr.Zero)
                {
                    BeginInvoke(new System.Action(() => { Text = "Deactivated"; }));
                    this.Close(); //closes this window on focus lost;
                }
                else
                    BeginInvoke(new System.Action(() => { Text = "Activated"; }));
            }
            base.WndProc(ref m);
        }


    }
}
