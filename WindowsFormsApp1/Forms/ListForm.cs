using Comparer2.ListApp.ImageListViewHelpers;
using Manina.Windows.Forms;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comparer2.ListApp.Forms
{
    public partial class ListForm : Form
    {

        bool scanning = false;
        bool scannedOnce = false;
        InitHelper initHelper;

        public ListForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            SetupComponents();
        }


        private void SetupComponents()
        {
            initHelper = new InitHelper(this, button1, button2);
            new DisplayRendererHelper(listView1, comboBox1);
            new DragDropHelper(listView1, this);
            new ClicksHelper(listView1);

            comboBox1.SelectedIndex = 0;
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (scanning)
            {
                scanning = false;
                return;
            }

            if (scannedOnce)
            {
                DialogResult dialogResult = MessageBox.Show(this, "Already scanned. Do you want to scan again?", "Rescan", MessageBoxButtons.OKCancel);
                if (dialogResult != DialogResult.OK)
                    return;
            }
#if DEBUG
            if (string.IsNullOrWhiteSpace(initHelper.Path1))
                initHelper.Path1 = @"D:\zaloha telefonu\Camera";
            if (string.IsNullOrWhiteSpace(initHelper.Path2))
                initHelper.Path2 = @"D:\zaloha telefonu\Camera 2019-12-26";
#endif

            if (string.IsNullOrWhiteSpace(initHelper.Path1) || string.IsNullOrWhiteSpace(initHelper.Path2))
            {
                MessageBox.Show(this, "Select folders first!");
                return;
            }


            listView1.Items.Clear();
            this.button3.Text = $"Scan";
            scanning = true;



            Action<string, int> onStatusUpdate = (ptr, value) =>
            {
                label1.Invoke(new MethodInvoker(delegate
                {
                    switch (ptr)
                    {
                        case "listing": this.button1.Text = $"{initHelper.Folder1} ({value})"; break;
                        case "comparing": this.button2.Text = $"{initHelper.Folder2} ({value})"; break;
                        case "computing": this.button3.Text = $"Scan ({value})"; break;
                    }
                }));

            };


            Comparer.Core.Core p = new Comparer.Core.Core(onStatusUpdate, initHelper.Path1, initHelper.Path2);
            Task t = Task.Run(() =>
            {
                p.Execute();

            });

            t.ContinueWith((tt) =>
            {
                label1.Invoke(new MethodInvoker(delegate
                {
                    var paths = p.GetResults();
                    label1.Text = "found: " + paths.Length;
                    PopulateListView(paths, p);
                    scannedOnce = true;
                    scanning = false;
                }));
            });


        }



        private void PopulateListView(FileInfo[] files, Comparer.Core.Core program)
        {
            listView1.Items.Clear();
            listView1.SuspendLayout();

            Random rnd = new Random(); ;

            string[] extensions = new string[] { ".pdf", ".jpg", ".png", ".bmp", ".ico", ".cur", ".emf", ".wmf", ".tif", ".tiff", ".gif", ""/*empty for all files*/ };
            string[] extensions2 = new string[] { ".jpg", ".png", ".bmp", ".tif", ".tiff", ".gif" };

            foreach (FileInfo p in files)
            {
                if (extensions.Any(e => p.Name.EndsWith(e)))
                {
                    ImageListViewItem item = new ImageListViewItem(p.FullName);

                    item.SubItems.Add("filesize", p.Length.ToString());
                    item.SubItems.Add("filesizeFriendly", p.Length.ToFriendlyFileSize());

                    if (program.Similarities.TryGetValue(p.FullName, out string val) && extensions2.Any(e => p.Name.EndsWith(e)))
                    {
                        FileInfo p2 = new FileInfo(val);
                        item.SubItems.Add("alternative", val);
                        item.SubItems.Add("filesize2", p2.Length.ToString());
                        item.SubItems.Add("filesize2Friendly", p2.Length.ToFriendlyFileSize());

                        string text = p.Length == p2.Length ? "Stejné" : (p.Length < p2.Length ? "Kvalitnější" : "Horší");
                        item.SubItems.Add("comparison", text);
                    }

                    listView1.Items.Add(item);
                }
            }


            listView1.ResumeLayout();
        }



    }
}
