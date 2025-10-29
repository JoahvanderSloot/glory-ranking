using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Glory_Ranking.Views
{
    public partial class EditFighterView : UserControl
    {
        string testName = "Joah";
        string testWeight = "Heavyweight";

        List<string> weightClasses = new List<string>() { "None", "Heavyweight", "Light heavyweight", "Middelweight", "Welterweight", "Featherweight" };
        List<TextBox> searchInfo;

        public Action ResetView;

        public EditFighterView()
        {
            InitializeComponent();

            searchInfo = new List<TextBox> { fighterName, fighterWeightclass, fighterElo, fighterPeakElo };

            ResetView = () =>
            {
                searchBox.Text = "";
                fighterName.Text = "Name...";
                fighterWeightclass.Text = "Weightclass...";
                fighterElo.Text = "Ranking...";
                fighterPeakElo.Text = "Peak ranking...";

                editNameButton.Visibility = Visibility.Hidden;
                editWeightButton.Visibility = Visibility.Hidden;
                setNameButton.Visibility = Visibility.Hidden;
                setWeightButton.Visibility = Visibility.Hidden;

                editNameButtonIMG.Visibility = editNameButton.Visibility;
                editWeightButtonIMG.Visibility = editWeightButton.Visibility;
                setNameButtonIMG.Visibility = setNameButton.Visibility;
                setWeightButtonIMG.Visibility = setWeightButton.Visibility;

                checkRetiredOrNot.IsEnabled = false;
                checkRetiredOrNot.IsChecked = false;
                checkRetiredOrNot.Foreground = Brushes.Silver;

                foreach (var item in searchInfo)
                {
                    item.Foreground = Brushes.Silver;
                    item.IsEnabled = false;
                }
            };
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtSearchPlaceholder.Visibility = string.IsNullOrEmpty(searchBox.Text) ? Visibility.Visible : Visibility.Hidden;

            if (searchBox.Text == testName)
            {
                foreach (var item in searchInfo)
                {
                    item.Foreground = Brushes.Black;
                    item.IsEnabled = true;
                }
                editNameButton.Visibility = Visibility.Visible;
                editWeightButton.Visibility = Visibility.Visible;
                checkRetiredOrNot.IsEnabled = true;
                checkRetiredOrNot.Foreground = Brushes.Black;
                checkRetiredOrNot.IsChecked = true;

                fighterName.Text = testName;
                fighterWeightclass.Text = testWeight;
                fighterElo.Text = "2000";
                fighterPeakElo.Text = "2100";
            }
            else
            {
                foreach (var item in searchInfo)
                {
                    item.Foreground = Brushes.Silver;
                    item.IsEnabled = false;
                }
                editNameButton.Visibility = Visibility.Hidden;
                editWeightButton.Visibility = Visibility.Hidden;
                setNameButton.Visibility = Visibility.Hidden;
                setWeightButton.Visibility = Visibility.Hidden;
                checkRetiredOrNot.IsEnabled = false;
                checkRetiredOrNot.Foreground = Brushes.Silver;
                checkRetiredOrNot.IsChecked = false;

                fighterName.Text = "Name...";
                fighterWeightclass.Text = "Weightclass...";
                fighterElo.Text = "Ranking...";
                fighterPeakElo.Text = "Peak ranking...";
            }

            editWeightButtonIMG.Visibility = editWeightButton.Visibility;
            editNameButtonIMG.Visibility = editNameButton.Visibility;
            setNameButtonIMG.Visibility = setNameButton.Visibility;
            setWeightButtonIMG.Visibility = setWeightButton.Visibility;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;

            if (_button == editNameButton)
            {
                fighterName.IsEnabled = true;
                editNameButton.Visibility = Visibility.Hidden;
                setNameButton.Visibility = Visibility.Visible;
            }
            else if (_button == setNameButton)
            {
                testName = fighterName.Text;
                fighterName.IsEnabled = false;
                searchBox.Text = testName;
                setNameButton.Visibility = Visibility.Hidden;
                editNameButton.Visibility = Visibility.Visible;
            }
            else if (_button == editWeightButton)
            {
                fighterWeightclass.IsEnabled = true;
                editWeightButton.Visibility = Visibility.Hidden;
            }
            else if (_button == setWeightButton)
            {
                foreach (var weight in weightClasses)
                {
                    if (fighterWeightclass.Text == weight)
                    {
                        fighterWeightclass.IsEnabled = false;
                        setWeightButton.Visibility = Visibility.Hidden;
                        editWeightButton.Visibility = Visibility.Visible;
                        break;
                    }
                }
            }

            editWeightButtonIMG.Visibility = editWeightButton.Visibility;
            editNameButtonIMG.Visibility = editNameButton.Visibility;
            setNameButtonIMG.Visibility = setNameButton.Visibility;
            setWeightButtonIMG.Visibility = setWeightButton.Visibility;
        }

        private void fighterWeightclass_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (editWeightButton.Visibility == Visibility.Hidden)
            //{
            //    foreach (var _weight in weightClasses)
            //    {
            //        if (fighterWeightclass.Text == _weight)
            //        {
            //            setWeightButton.Visibility = Visibility.Visible;
            //            return;
            //        }
            //        setWeightButton.Visibility = Visibility.Hidden;
            //    }
            //}
        }
    }
}
