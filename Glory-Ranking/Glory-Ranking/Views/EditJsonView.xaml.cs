using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace Glory_Ranking.Views
{
    public partial class EditJsonView : UserControl
    {
        public EditJsonView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;

            if (_button == uploadJsonButton)
            {
                OpenFileDialog _openFileDialog = new OpenFileDialog();
                bool? _response = _openFileDialog.ShowDialog();

                if (_response == true)
                {
                    string _filepath = _openFileDialog.FileName;
                    MessageBox.Show(_filepath);
                }
            }

            // Implement download/reset JSON logic here
        }
    }
}
