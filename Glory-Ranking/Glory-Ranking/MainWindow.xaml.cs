using System.Windows;
using System.Windows.Controls;

namespace Glory_Ranking
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Load fighters from JSON when the app starts
            FighterManager.LoadFighters();

            // Refresh the leaderboard so it shows the loaded fighters
            LeaderboardTab?.RefreshLeaderboard();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                // Reset AddFightView if it exists
                AddFightTab?.ResetView?.Invoke();

                // Reset EditFighterView if it exists
                EditFighterTab?.ResetView?.Invoke();

                // Refresh LeaderboardView if it exists
                LeaderboardTab?.RefreshLeaderboard();
            }
        }
    }
}
