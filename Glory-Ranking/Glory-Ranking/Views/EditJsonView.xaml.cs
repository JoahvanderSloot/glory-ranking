using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;

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
                OpenFileDialog _openFileDialog = new OpenFileDialog()
                {
                    DefaultExt = ".json",
                    Filter = "fighter data file (*.json)|*json",  
                };

                bool? _response = _openFileDialog.ShowDialog();

                if (_response == true)
                {
                    string _filepath = _openFileDialog.FileName;
                    if (Path.GetExtension(_filepath) != ".json")
                    {
                       MessageBox.Show("Please upload a json file", "json error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string _jsonScript = File.ReadAllText(_filepath);
                    try
                    {
                        FighterData _fighter = JsonSerializer.Deserialize<FighterData>(_jsonScript);
                        if (_fighter.Fighters == null || !_fighter.Version.Equals("fighter-data-v1") || _fighter.Fighters.Length == 0)
                        {
                            MessageBox.Show("json file does not contain fighter data", "json error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("json file was not compatible", "json error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
        }
    }
}
