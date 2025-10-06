using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Glory_Ranking
{
    public partial class MainWindow : Window
    {
        //Leaderboard UI elements
        ListBox leaderboard;
        ComboBox divisionBox;
        TextBox chooseDivPreset;

        public MainWindow()
        {
            InitializeComponent();

            leaderboard = leaderboardBox;
            divisionBox = leaderboardDivision;
            chooseDivPreset = ChooseDivPrev;
        }

        private void leaderboardDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chooseDivPreset.Visibility = divisionBox.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
        }
    }
}