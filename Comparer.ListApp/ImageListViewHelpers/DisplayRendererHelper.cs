using Manina.Windows.Forms;
using System;
using System.Windows.Forms;

namespace Comparer.ListApp.ImageListViewHelpers
{
    public class DisplayRendererHelper
    {
        private readonly ImageListView imageListView;

        public DisplayRendererHelper(ImageListView imageListView, ComboBox comboBox)
        {
            this.imageListView = imageListView;
            this.imageListView.Columns.Add(ColumnType.Name);
            this.imageListView.Columns.Add(ColumnType.Dimensions);
            this.imageListView.Columns.Add(ColumnType.FileSize);
            this.imageListView.Columns.Add(ColumnType.FolderName);
            this.imageListView.Columns.Add(ColumnType.DateModified);
            this.imageListView.AllowItemReorder = false;
            comboBox.SelectedIndexChanged += cboStyle_SelectedIndexChanged;
        }



        // Change the ListView's display style.
        private void cboStyle_SelectedIndexChanged(object sender, EventArgs e)
        {

            imageListView.SetRenderer(new DuplicityRenderer());
            switch ((sender as ComboBox).Text)
            {
                case "Details":
                    imageListView.View = Manina.Windows.Forms.View.Details;
                    break;
                case "Gallery":
                    imageListView.View = Manina.Windows.Forms.View.Gallery;
                    break;
                case "Pane":
                    imageListView.View = Manina.Windows.Forms.View.Pane;
                    break;
                case "Thumbnails":
                    imageListView.View = Manina.Windows.Forms.View.Thumbnails;
                    break;
                case "HorizontalStrip":
                    imageListView.View = Manina.Windows.Forms.View.HorizontalStrip;
                    break;
                case "VerticalStrip":
                    imageListView.View = Manina.Windows.Forms.View.VerticalStrip;
                    break;

                case "AdvancedThumnails":
                    imageListView.SetRenderer(new Manina.Windows.Forms.ImageListViewRenderers.TilesRenderer());
                    break;
            }
        }
    }
}
