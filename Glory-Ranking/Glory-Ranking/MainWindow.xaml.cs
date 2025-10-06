using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Glory_Ranking
{
    public partial class MainWindow : Window
    {
        string testName = "Joah";

        //Leaderboard UI elements
        ListBox leaderboard;
        ComboBox divisionBox;
        TextBox chooseDivPreset;
        //Edit Fighter UI elements
        TextBox searchFighter;
        Label searchPrev;
        List<TextBlock> searchInfo = new List<TextBlock>();

        public MainWindow()
        {
            InitializeComponent();

            leaderboard = leaderboardBox;
            divisionBox = leaderboardDivision;
            chooseDivPreset = chooseDivPrev;

            searchFighter = searchBox;
            searchPrev = txtSearchPlaceholder;

            searchInfo.Add(fighterName);
            searchInfo.Add(fighterWeightclass);
            searchInfo.Add(fighterElo);
            searchInfo.Add(fighterPeakElo);
        }

        private void leaderboardDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chooseDivPreset.Visibility = divisionBox.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchPrev.Visibility = searchFighter.Text == "" ? Visibility.Visible : Visibility.Hidden;

            if(searchFighter.Text == testName)
            {
                foreach (var item in searchInfo)
                {
                    item.Foreground = Brushes.Black;
                }
                searchInfo[0].Text = "Joahdhsajdhsajkdhkjashdkjsahdkjashdjkashkjdhsajkdhskajhdkjsa";
                searchInfo[1].Text = "Welterweight";
                searchInfo[2].Text = 2000.ToString();
                searchInfo[3].Text = 2100.ToString();
            }
            else
            {
                foreach(var item in searchInfo)
                {
                    item.Foreground = Brushes.Silver;
                }
                searchInfo[0].Text = "Name...";
                searchInfo[1].Text = "Weightclass...";
                searchInfo[2].Text = "Ranking...";
                searchInfo[3].Text = "Peak ranking...";
            }
        }
    }
}