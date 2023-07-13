using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaPlayerDemo.Dialogs
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {

        public string PlaylistName
        {
            get { return txtValue.Text; }
        }

        public InputDialog(string q, string defAns = "")
        {
            InitializeComponent();
            tbQuestion.Text = q;
            txtValue.Text = defAns;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtValue.Focus();
            txtValue.SelectAll();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}
