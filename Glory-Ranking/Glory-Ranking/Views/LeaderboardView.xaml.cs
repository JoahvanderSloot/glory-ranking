using System.Windows.Controls;
using System.Windows.Input;

namespace Glory_Ranking.Views
{
    public partial class LeaderboardView : UserControl
    {
        FighterWindow fighterWindow = new FighterWindow();

        public LeaderboardView()
        {
            InitializeComponent();

            retiredCheckbox.Checked += RetiredCheckbox_Changed;
            retiredCheckbox.Unchecked += RetiredCheckbox_Changed;

            leaderboardBox.MouseDoubleClick += LeaderboardBox_MouseDoubleClick;

            RefreshLeaderboard();
        }

        private void LeaderboardBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (leaderboardBox.SelectedItem == null) return;

            string selectedText = leaderboardBox.SelectedItem.ToString();
            int index = selectedText.IndexOf(" - Elo:");
            if (index < 0) return;

            string fighterName = selectedText.Substring(0, index);

            var fighterWindow = new FighterWindow();
            fighterWindow.OpenFighter(fighterName);
            fighterWindow.Show();
        }

        private void RetiredCheckbox_Changed(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshLeaderboard();
        }

        private void leaderboardDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int? _division = null;

            if (leaderboardDivision.SelectedIndex >= 0 && leaderboardDivision.SelectedIndex < 5)
            {
                // 0-4 = Heavyweight to Featherweight
                _division = leaderboardDivision.SelectedIndex + 1;
            }
            else if (leaderboardDivision.SelectedIndex == 5)
            {
                // "All weightclasses" selected
                _division = null;
            }

            RefreshLeaderboard(_division);
        }

        public void RefreshLeaderboard(int? _division = null)
        {
            if (leaderboardBox == null) return;

            leaderboardBox.Items.Clear();

            bool _showRetired = retiredCheckbox.IsChecked == true;

            var _topFighters = FighterManager.Fighters.AsEnumerable();

            if (_division.HasValue)
            {
                _topFighters = _topFighters.Where(f => f.Division == _division.Value);
            }

            if (!_showRetired)
            {
                _topFighters = _topFighters.Where(f => !f.Retired);
            }

            _topFighters = _topFighters.OrderByDescending(f => f.Elo).Take(10);

            foreach (var _f in _topFighters)
            {
                string _weight = _f.Division switch
                {
                    1 => "Heavyweight",
                    2 => "Light heavyweight",
                    3 => "Middleweight",
                    4 => "Welterweight",
                    5 => "Featherweight",
                    _ => "None"
                };

                string _display = _division.HasValue
                    ? $"{_f.Name} - Elo: {_f.Elo}"
                    : $"{_f.Name} - Elo: {_f.Elo} ({_weight})";

                leaderboardBox.Items.Add(_display);
            }
        }
    }
}
