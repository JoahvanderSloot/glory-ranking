using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Glory_Ranking
{
    public partial class FullLeaderboard : Window
    {
        private int currentDivision;
        private static FullLeaderboard currentOpenWindow;

        public FullLeaderboard()
        {
            InitializeComponent();

            // Allow pressing Enter to open selected fighter
            fullLeaderboardList.KeyDown += LeaderboardBox_KeyDown;
        }

        public void OpenLeaderboard(int _weightClass)
        {
            if (currentOpenWindow != null && currentOpenWindow != this)
                currentOpenWindow.Close();

            currentOpenWindow = this;
            currentDivision = _weightClass;

            string _divisionName = _weightClass switch
            {
                0 => "Heavyweight",
                1 => "Light Heavyweight",
                2 => "Middleweight",
                3 => "Welterweight",
                4 => "Featherweight",
                5 => "All Weightclasses",
                _ => "Unknown"
            };

            divisionLabel.Content = $"Division: {_divisionName}";
            RefreshLeaderboard();
            Show();
            Activate();
        }

        private void RefreshLeaderboard()
        {
            if (fullLeaderboardList == null) return;

            fullLeaderboardList.Items.Clear();

            var _fighters = FighterManager.Fighters.AsEnumerable();

            if (currentDivision >= 0 && currentDivision < 5)
                _fighters = _fighters.Where(f => f.Division == currentDivision + 1);

            if (retiredCheckbox.IsChecked != true)
                _fighters = _fighters.Where(f => !f.Retired);

            _fighters = _fighters.OrderByDescending(f => f.Elo);

            int _rank = 1;
            foreach (var f in _fighters)
            {
                string _weightName = f.Division switch
                {
                    1 => "Heavyweight",
                    2 => "Light heavyweight",
                    3 => "Middleweight",
                    4 => "Welterweight",
                    5 => "Featherweight",
                    _ => "None"
                };

                // Show weight class only for "All Weightclasses"
                string _entry = currentDivision == 5
                    ? $"#{_rank}. {f.Name} - Elo: {f.Elo} ({_weightName})"
                    : $"#{_rank}. {f.Name} - Elo: {f.Elo}";

                fullLeaderboardList.Items.Add(_entry);
                _rank++;
            }

            if (!_fighters.Any())
                fullLeaderboardList.Items.Add("No fighters in this division yet.");
        }

        private void RetiredCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            RefreshLeaderboard();
        }

        private void LeaderboardBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenSelectedFighter();
        }

        // --- NEW: Press Enter to open selected fighter ---
        private void LeaderboardBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OpenSelectedFighter();
        }

        private void OpenSelectedFighter()
        {
            if (fullLeaderboardList.SelectedItem == null) return;

            string _selectedText = fullLeaderboardList.SelectedItem.ToString();
            int _startIndex = _selectedText.IndexOf(". ");
            int _endIndex = _selectedText.IndexOf(" - Elo:");

            if (_startIndex < 0 || _endIndex < 0 || _endIndex <= _startIndex) return;

            string _fighterName = _selectedText.Substring(_startIndex + 2, _endIndex - (_startIndex + 2));

            var _fighterWindow = new FighterWindow();
            _fighterWindow.OpenFighter(_fighterName);
            _fighterWindow.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (currentOpenWindow == this)
                currentOpenWindow = null;
        }
    }
}
