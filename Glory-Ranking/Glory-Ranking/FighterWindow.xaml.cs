using System.Windows;
using System.Windows.Controls;

namespace Glory_Ranking
{
    public partial class FighterWindow : Window
    {
        public FighterWindow()
        {
            InitializeComponent();
        }

        public void OpenFighter(string _fighterName)
        {
            var _fighter = FighterManager.GetFighter(_fighterName);

            if (_fighter == null)
            {
                MessageBox.Show($"Fighter '{_fighterName}' not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Fill in labels
            name.Content = $"Name: {_fighter.Name}";
            weight.Content = $"Weightclass: {GetWeightClassName(_fighter.Division)}";
            elo.Content = $"Elo: {_fighter.Elo}";
            peakElo.Content = $"Peak Elo: {_fighter.PeakElo}";
            retired.Content = $"Retired: {(_fighter.Retired ? "Yes" : "No")}";
            wins.Content = $"Wins: {_fighter.Wins}";
            losses.Content = $"Losses: {_fighter.Losses}";
            biggestEloGain.Content = $"Biggest Elo Gain: {_fighter.BiggestEloGain}";
            biggestEloLoss.Content = $"Biggest Elo Loss: {_fighter.BiggestEloLoss}";

            this.Show();
            this.Activate();
        }

        private string GetWeightClassName(int _division)
        {
            return _division switch
            {
                1 => "Heavyweight",
                2 => "Light heavyweight",
                3 => "Middleweight",
                4 => "Welterweight",
                5 => "Featherweight",
                _ => "None"
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Content.ToString())) return;

            string _fighterName = name.Content.ToString().Replace("Name: ", "").Trim();

            if (Application.Current.MainWindow is MainWindow _mainWindow)
            {
                if (_mainWindow.mainTabControl != null)
                {
                    _mainWindow.mainTabControl.SelectedIndex = 1;

                    _mainWindow.Dispatcher.InvokeAsync(() =>
                    {
                        _mainWindow.EditFighterTab.searchBox.Text = _fighterName;
                    });
                }
            }

            this.Hide();
        }
    }
}
