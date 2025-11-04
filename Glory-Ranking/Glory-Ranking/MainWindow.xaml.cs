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
            if (e.Source is TabControl)
            {
                // Reset AddFightView if it exists
                AddFightTab?.ResetView?.Invoke();

                // Reset EditFighterView if it exists
                EditFighterTab?.ResetView?.Invoke();
            }
        }
    }
}
