using Manina.Windows.Forms;
using System;
using System.Windows.Forms;

namespace Comparer.ListApp.ImageListViewHelpers
{
    public class DragDropHelper
    {

        bool deleteFiles = false;
        private readonly ImageListView listView1;
        Form form;

        public DragDropHelper(ImageListView listView1, Form form)
        {
            this.form = form;
            this.listView1 = listView1;
            //listView1.AllowDrop = true;
            listView1.AllowDrag = true;
            listView1.QueryContinueDrag += ListView1_QueryContinueDrag;
            listView1.DragEnter += ListView1_DragEnter;
            listView1.DragLeave += ListView1_DragLeave;
        }


        private void ListView1_DragLeave(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("drag leave");

            deleteFiles = true;
        }

        private void ListView1_DragEnter(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("drag enter " + e.Effect);
            deleteFiles = false;
        }

        private void ListView1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

            if (e.Action == DragAction.Drop)
            {
                System.Drawing.Rectangle rect = form.ClientRectangle;

                rect.X -= 5;
                rect.Y -= 35;
                rect.Width += 10;
                rect.Height += 40;
                if (rect.Contains(form.PointToClient(Control.MousePosition)))
                    return;

                System.Diagnostics.Debug.WriteLine("drag continue " + e.Action);
                if (deleteFiles)
                {

                    listView1.SuspendLayout();
                    foreach (var item in listView1.SelectedItems)
                    {
                        listView1.Items.Remove(item);
                    }
                    listView1.ResumeLayout();
                }
                deleteFiles = false;
            }
        }



    }
}
