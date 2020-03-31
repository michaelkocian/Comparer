using Comparer.ListApp.Forms;
using Manina.Windows.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Comparer.ListApp.ImageListViewHelpers
{
    public class ClicksHelper
    {
        private readonly ImageListView imageListView;

        int size = 400;

        public ClicksHelper(ImageListView imageListView)
        {
            this.imageListView = imageListView;
            this.imageListView.KeyDown += ListView1_KeyDown;
            this.imageListView.DoubleClick += listView1_DoubleClick;
            this.imageListView.ItemClick += ImageListView_ItemClick;

            this.imageListView.ThumbnailSize = new Size(size, size * 9 / 16);
        }

        private void ImageListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Buttons == MouseButtons.Right)
            {
                ImageListViewItem item = e.Item;
                ImageListViewSubItem item2 = item.SubItems["alternative"];

                ContextMenuStrip menu = new ContextMenuStrip();


                string size = item.SubItems["filesizeFriendly"]?.Text;
                string size2 = item.SubItems["filesize2Friendly"]?.Text;

                menu.Items.Add($"Open ({size}) {item.FileName}", null, (sender, p) => OpenWithDefaultProgram(item.FileName));
                menu.Items.Add("Open folder " + item.FileName, null, (sender, p) => OpenInExplorer(item.FileName));
                menu.Items.Add("Properities", null, (sender, p) => new PropertiesForm(item).Show());

                if (!string.IsNullOrWhiteSpace(item2.Text))
                {
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add($"Open ({size2}) {item2.Text}", null, (sender, p) => OpenWithDefaultProgram(item2.Text));
                    menu.Items.Add("Open folder " + item2.Text, null, (sender, p) => OpenInExplorer(item2.Text));
                    menu.Items.Add("Properities", null, (sender, p) => new PropertiesForm(new FileInfo(item2.Text)).Show());
                }


                menu.Show(sender as Control, e.Location);
            }
        }



        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ImageListViewItem selectedITem = (sender as ImageListView).SelectedItems[0];
            string path = selectedITem?.FileName;
            if (path.EndsWith(".jpg"))
                new Comparer.FullscreenImage.Form1(path).Show();
            //new FullscreenImg(path, selectedITem).Show();
            else
                OpenWithDefaultProgram(path);

        }


        public static void OpenWithDefaultProgram(string path)
        {
            Process fileopener = new Process();
            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();
        }



        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Add:
                    size += 100;
                    imageListView.ThumbnailSize = new Size(size, size * 9 / 16);
                    break;
                case Keys.Subtract:
                    if (size <= 100)
                        break;
                    size -= 100;
                    imageListView.ThumbnailSize = new Size(size, size * 9 / 16);
                    break;

            }


        }

        private void OpenInExplorer(string path)
        {
            string args = string.Format("/e, /select, \"{0}\"", path);

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "explorer";
            info.Arguments = args;
            Process.Start(info);

        }



    }
}
