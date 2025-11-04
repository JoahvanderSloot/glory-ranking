using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Glory_Ranking.Views
{
    public partial class EditJsonView : UserControl
    {
        private readonly string defaultSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fighters.json");

        public EditJsonView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == downloadJsonButton)
            {
                DownloadJson();
            }
            else if (sender == uploadJsonButton)
            {
                UploadJson();
            }
            else if (sender == resetJsonButton)
            {
                ResetJson();
            }
        }

        private void DownloadJson()
        {
            try
            {
                SaveFileDialog _saveDialog = new SaveFileDialog
                {
                    FileName = "fighters.json",
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
                };

                if (_saveDialog.ShowDialog() == true)
                {
                    FighterManager.SaveToFile();
                    File.Copy(defaultSavePath, _saveDialog.FileName, true);
                    MessageBox.Show("JSON file downloaded successfully.", "Download Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception _ex)
            {
                MessageBox.Show($"Failed to download JSON: {_ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UploadJson()
        {
            try
            {
                OpenFileDialog _openDialog = new OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
                };

                if (_openDialog.ShowDialog() == true)
                {
                    File.Copy(_openDialog.FileName, defaultSavePath, true);
                    FighterManager.LoadFromFile();
                    MessageBox.Show("JSON file uploaded successfully.", "Upload Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception _ex)
            {
                MessageBox.Show($"Failed to upload JSON: {_ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetJson()
        {
            var _result = MessageBox.Show(
                "Are you sure you want to reset all fighters? This will permanently delete all saved data.",
                "Confirm Reset",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (_result == MessageBoxResult.Yes)
            {
                try
                {
                    FighterManager.ResetFighters();
                    MessageBox.Show("All fighters have been cleared and reset.", "Reset Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception _ex)
                {
                    MessageBox.Show($"Failed to reset fighters: {_ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
