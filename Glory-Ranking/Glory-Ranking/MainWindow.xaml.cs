using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Glory_Ranking
{
    public partial class MainWindow : Window
    {
        //Frontend variables
        ListBox leaderboard;

        //Backend varieables

        public MainWindow()
        {
            InitializeComponent();

            leaderboard = leaderboardBox;
        }
    }
}