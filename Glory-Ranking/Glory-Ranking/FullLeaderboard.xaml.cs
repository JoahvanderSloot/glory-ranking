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
        }

        // --- Open leaderboard for given weight class ---
        public void OpenLeaderboard(int _weightClass)
        {
            // Close any existing leaderboard before opening a new one
            if (currentOpenWindow != null && currentOpenWindow != this)
            {
                currentOpenWindow.Close();
            }
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

        // --- Refresh list based on division and retired checkbox ---
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

                string _entry = $"{_rank}. {f.Name} - Elo: {f.Elo} ({_weightName})";
                fullLeaderboardList.Items.Add(_entry);
                _rank++;
            }

            if (!_fighters.Any())
                fullLeaderboardList.Items.Add("No fighters in this division yet.");
        }

        // --- Update when checkbox toggled ---
        private void RetiredCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            RefreshLeaderboard();
        }

        // --- Double-click to open fighter window ---
        private void LeaderboardBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (fullLeaderboardList.SelectedItem == null) return;

            string _selectedText = fullLeaderboardList.SelectedItem.ToString();

            // Extract name between ". " and " - Elo:"
            int startIndex = _selectedText.IndexOf(". ");
            int endIndex = _selectedText.IndexOf(" - Elo:");

            if (startIndex < 0 || endIndex < 0 || endIndex <= startIndex) return;

            string _fighterName = _selectedText.Substring(startIndex + 2, endIndex - (startIndex + 2));

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
