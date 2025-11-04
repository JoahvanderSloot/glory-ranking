using System;
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

        List<string> weightClasses = new List<string>()
        {
            "None", "Heavyweight", "Light heavyweight", "Middleweight", "Welterweight", "Featherweight"
        };

        List<TextBox> searchInfo;

        public Action ResetView;

        public EditFighterView()
        {
            InitializeComponent();

            searchInfo = new List<TextBox> { fighterName, fighterWeightclass, fighterElo, fighterPeakElo };

            fighterWeightDropdown.ItemsSource = weightClasses;
            fighterWeightDropdown.Visibility = Visibility.Hidden;
            fighterWeightclass.Visibility = Visibility.Visible;

            ResetView = () =>
            {
                searchBox.Text = "";
                txtSearchPlaceholder.Visibility = Visibility.Visible;

                fighterName.Text = "Name...";
                fighterWeightclass.Text = "Weightclass...";
                fighterElo.Text = "Ranking...";
                fighterPeakElo.Text = "Peak ranking...";

                SetEditButtonsVisibility(false);

                checkRetiredOrNot.IsEnabled = false;
                checkRetiredOrNot.IsChecked = false;
                checkRetiredOrNot.Foreground = Brushes.Silver;

                foreach (var _item in searchInfo)
                {
                    _item.Foreground = Brushes.Silver;
                    _item.IsEnabled = false;
                }
            };
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtSearchPlaceholder.Visibility =
                string.IsNullOrWhiteSpace(searchBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                ResetView?.Invoke();
                return;
            }

            if (searchBox.Text.Equals(testName, StringComparison.OrdinalIgnoreCase))
            {
                LoadFighterData();
                return;
            }

            foreach (var _item in searchInfo)
            {
                _item.Foreground = Brushes.Silver;
                _item.IsEnabled = false;
            }

            fighterName.Text = "Name...";
            fighterWeightclass.Text = "Weightclass...";
            fighterElo.Text = "Ranking...";
            fighterPeakElo.Text = "Peak ranking...";

            checkRetiredOrNot.IsEnabled = false;
            checkRetiredOrNot.IsChecked = false;
            checkRetiredOrNot.Foreground = Brushes.Silver;

            SetEditButtonsVisibility(false);
        }

        private void LoadFighterData()
        {
            fighterName.Text = testName;
            fighterWeightclass.Text = testWeight;
            fighterElo.Text = "1200";
            fighterPeakElo.Text = "1350";

            foreach (var _item in searchInfo)
            {
                _item.Foreground = Brushes.Black;
                _item.IsEnabled = false;
            }

            checkRetiredOrNot.IsEnabled = true;
            checkRetiredOrNot.Foreground = Brushes.Black;

            SetEditButtonsVisibility(checkRetiredOrNot.IsChecked == true);

            fighterWeightDropdown.Visibility = Visibility.Hidden;
            fighterWeightclass.Visibility = Visibility.Visible;
        }

        private void EditName_Click(object sender, RoutedEventArgs e)
        {
            if (checkRetiredOrNot.IsChecked == false) return;

            ToggleEdit(fighterName, editNameButton, setNameButton);
        }

        private void SetName_Click(object sender, RoutedEventArgs e)
        {
            CommitEdit(fighterName, editNameButton, setNameButton);

            searchBox.TextChanged -= searchBox_TextChanged;

            testName = fighterName.Text;
            searchBox.Text = testName;
            LoadFighterData();

            searchBox.TextChanged += searchBox_TextChanged;
        }

        private void EditWeight_Click(object sender, RoutedEventArgs e)
        {
            if (checkRetiredOrNot.IsChecked == false) return;

            fighterWeightDropdown.Visibility = Visibility.Visible;
            fighterWeightclass.Visibility = Visibility.Hidden;

            fighterWeightDropdown.SelectedItem = testWeight;
            fighterWeightDropdown.IsEnabled = true;

            editWeightButton.Visibility = Visibility.Hidden;
            setWeightButton.Visibility = Visibility.Visible;
        }

        private void SetWeight_Click(object sender, RoutedEventArgs e)
        {
            if (fighterWeightDropdown.SelectedItem != null)
            {
                testWeight = fighterWeightDropdown.SelectedItem.ToString();
                fighterWeightclass.Text = testWeight;
            }

            fighterWeightDropdown.Visibility = Visibility.Hidden;
            fighterWeightclass.Visibility = Visibility.Visible;

            setWeightButton.Visibility = Visibility.Hidden;
            editWeightButton.Visibility = Visibility.Visible;
        }

        private void checkRetiredOrNot_Checked(object sender, RoutedEventArgs e)
        {
            SetEditButtonsVisibility(true);
        }

        private void checkRetiredOrNot_Unchecked(object sender, RoutedEventArgs e)
        {
            if (fighterName.IsEnabled)
            {
                testName = fighterName.Text;
                searchBox.TextChanged -= searchBox_TextChanged;
                searchBox.Text = testName;
                searchBox.TextChanged += searchBox_TextChanged;
            }

            if (fighterWeightDropdown.Visibility == Visibility.Visible)
            {
                if (fighterWeightDropdown.SelectedItem != null)
                    testWeight = fighterWeightDropdown.SelectedItem.ToString();
                else
                    testWeight = fighterWeightclass.Text;
            }
            else if (fighterWeightclass.IsEnabled)
            {
                testWeight = fighterWeightclass.Text;
            }

            CommitEdit(fighterName, editNameButton, setNameButton, true);
            CommitEdit(fighterWeightclass, editWeightButton, setWeightButton, true);

            fighterName.Text = testName;
            fighterWeightclass.Text = testWeight;

            fighterWeightDropdown.Visibility = Visibility.Hidden;
            fighterWeightclass.Visibility = Visibility.Visible;
        }

        private void ToggleEdit(TextBox textBox, Button editBtn, Button doneBtn)
        {
            textBox.IsEnabled = true;
            textBox.Focus();
            textBox.CaretIndex = textBox.Text.Length;
            editBtn.Visibility = Visibility.Hidden;
            doneBtn.Visibility = Visibility.Visible;
        }

        private void CommitEdit(TextBox textBox, Button editBtn, Button doneBtn, bool forceHide = false)
        {
            textBox.IsEnabled = false;

            if (!forceHide)
            {
                editBtn.Visibility = Visibility.Visible;
            }
            else
            {
                editBtn.Visibility = Visibility.Hidden;
            }

            doneBtn.Visibility = Visibility.Hidden;
        }

        private void SetEditButtonsVisibility(bool enabled)
        {
            editNameButton.Visibility = enabled ? Visibility.Visible : Visibility.Hidden;
            editWeightButton.Visibility = enabled ? Visibility.Visible : Visibility.Hidden;

            setNameButton.Visibility = Visibility.Hidden;
            setWeightButton.Visibility = Visibility.Hidden;
        }
    }
}
