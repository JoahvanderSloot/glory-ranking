using System.Windows.Controls;

namespace Glory_Ranking.Views
{
    public partial class LeaderboardView : UserControl
    {
        public LeaderboardView()
        {
            InitializeComponent();
        }

        private void leaderboardDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chooseDivPrev.Visibility = leaderboardDivision.SelectedItem == null ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
    }
}
