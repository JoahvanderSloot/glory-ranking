using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Glory_Ranking
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string _name = ((TabItem)((TabControl)sender).SelectedItem).Name;
            if(_name == "leaderboard")
            {
                
            }
            else if(_name == "fighter")
            {
                
            }
            else if(_name == "fight")
            {
                
            }
            else if(_name == "json")
            {
                
            }
        }
    }
}