using System;
using System.Windows.Forms;

namespace Comparer2.ListApp.ImageListViewHelpers
{
    public class InitHelper
    {
        private readonly Form form;
        ToolTip toolTip1, toolTip2;
        private string path1;
        private string path2;
        private string folder1;
        private string folder2;

        public string Path1 { get => path1; set => path1 = value; }
        public string Path2 { get => path2; set => path2 = value; }

        public string Folder1 { get => folder1; set => folder1 = value; }
        public string Folder2 { get => folder2; set => folder2 = value; }


        public InitHelper(Form form, Button button1, Button button2)
        {
            this.form = form;
            button1.Click += button1_Click;
            button2.Click += button2_Click;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            PickFolder(sender as Button, ref toolTip1, ref path1, ref folder1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PickFolder(sender as Button, ref toolTip2, ref path2, ref folder2);
        }

        private void PickFolder(Button b, ref ToolTip tooltip, ref string path, ref string folder)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.ShowDialog(form);
            if (string.IsNullOrWhiteSpace(d.SelectedPath))
                return;
            var parts = d.SelectedPath.Split('\\');
            b.Text = folder = parts[parts.Length - 1];
            tooltip?.RemoveAll();
            tooltip = new ToolTip();
            tooltip.SetToolTip(b, d.SelectedPath);
            path = d.SelectedPath;
        }
    }
}
