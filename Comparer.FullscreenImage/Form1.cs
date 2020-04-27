using Cyotek.Windows.Forms;
using ExifLibrary;
using System.Drawing;
using System.Windows.Forms;

namespace Comparer.FullscreenImage
{
    public partial class Form1 : Form
    {
        Button r, c;
        Image image;
        ImageBox imageBox;
        ImageFile ExifInfo;
        System.DateTime formLoaded;

        Color transparencyKey = Color.FromArgb(255, 255, 255, 255);


        public Form1(string path = @"D:\zaloha telefonu\Camera\20170903_222301.jpg")
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Text = path; //title
            this.BackColor = transparencyKey;
            this.TransparencyKey = transparencyKey;

            ExifInfo = ImageFile.FromFile(path);

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            imageBox = new MyImageBox
            {
                Width = this.Width,
                Height = this.Height,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ForeColor = transparencyKey,
                GridColor = transparencyKey,
                GridColorAlternate = transparencyKey,
            };

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


            var bitmap = new Bitmap(path);
            bitmap.ChangeColor(255, 255, 255, 254, 255, 255);
            image = bitmap;
            image.RotateFlip(type);
            imageBox.Image = image;

            c = new Button
            {
                Width = 30,
                Height = 30,
                BackColor = Color.Black,
                ForeColor = Color.FromArgb(255, 125, 0, 0),
                Text = "✖",
                FlatStyle = FlatStyle.Flat
            };

            r = new Button
            {
                Width = 30,
                Height = 30,
                BackColor = Color.Black,
                ForeColor = Color.FromArgb(255, 125, 0, 0),
                Text = "↶",
                FlatStyle = FlatStyle.Popup,
            };

            new ToolTip().SetToolTip(r, "Rotate");
            new ToolTip().SetToolTip(c, "Close");

            this.Controls.Add(c);
            this.Controls.Add(r);
            this.Controls.Add(imageBox);



            this.Load += Form1_Load;
            c.Click += C_Click;
            r.Click += R_Click;
            imageBox.BorderStyle = BorderStyle.None;

            this.Deactivate += Form1_Deactivate;
        }

        private void R_Click(object sender, System.EventArgs e)
        {
            imageBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            imageBox.Invalidate();
        }

        private void Form1_Deactivate(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void C_Click(object sender, System.EventArgs e)
        {
            if (System.DateTime.UtcNow - formLoaded > System.TimeSpan.FromMilliseconds(500))
                this.Close();
        }


        private void Form1_Load(object sender, System.EventArgs e)
        {
            int zoomfactor = (this.Height /* - 40*/) * 100 / image.Height;
            imageBox.Zoom = zoomfactor;
            formLoaded = System.DateTime.UtcNow;
            c.Location = new Point(this.Width - 35, 5);
            r.Location = new Point(this.Width - 70, 5);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            image.Dispose();
        }



        //
        //    //CLOSE ON CLICK OUTSIDE
        //    private const int WM_ACTIVATEAPP = 0x1C;
        //    protected override void WndProc(ref Message m)
        //    {
        //        if (m.Msg == WM_ACTIVATEAPP)
        //        {
        //            if (m.WParam == System.IntPtr.Zero)
        //            {
        //                BeginInvoke(new System.Action(() => { Text = "Deactivated"; }));
        //                this.Close(); //closes this window on focus lost;
        //            }
        //            else
        //                BeginInvoke(new System.Action(() => { Text = "Activated"; }));
        //        }
        //        base.WndProc(ref m);
        //    }


    }
}
