using System.Windows.Forms;

namespace Comparer2.ListApp.Forms
{
    public partial class PropertiesForm : Form
    {
        public PropertiesForm(object fileInfo)
        {
            InitializeComponent();
            PropertyGrid p = new PropertyGrid();
            p.SelectedObject = fileInfo;
            p.Dock = DockStyle.Fill;
            this.Controls.Add(p);
        }
    }
}
