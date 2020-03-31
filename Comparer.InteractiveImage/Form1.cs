
using System.Drawing;
using System.Windows.Forms;

namespace Comparer.InteractiveImage
{
    public partial class Form1 : Form
    {


        Image image;
        float ratio;
        int heigth = 1400;
        BufferedGraphics graphicsBuffer;

        public Form1()
        {

            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            //DoubleBuffered = true;
            image = Bitmap.FromFile(@"D:\zaloha telefonu\Camera\20170903_222301.jpg");
            ratio = (float)image.Width / (float)image.Height;


            this.MouseWheel += Form1_MouseWheel;

            using (Graphics graphics = CreateGraphics())
            {
                graphicsBuffer = BufferedGraphicsManager.Current.Allocate(graphics, new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            }

        }




        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            heigth += e.Delta;
            this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {


        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = graphicsBuffer.Graphics;
            g.DrawImage(image, 0, 0, heigth * ratio, heigth);
            graphicsBuffer.Render(e.Graphics);



        }






    }
}
