using System;
using System.Windows;
using System.Windows.Controls;

namespace Glory_Ranking.Views
{
    public partial class AddFightView : UserControl
    {
        public Action ResetView;

        public AddFightView()
        {
            InitializeComponent();

            ResetView = () =>
            {
                newFighterNameInput.Clear();
                winnerTextInput.Clear();
                loserTextInput.Clear();

                newFighterNameOverlay.Visibility = Visibility.Visible;
                winnerTextOverlay.Visibility = Visibility.Visible;
                loserTextOverlay.Visibility = Visibility.Visible;

                fighterAddedInfoText.Text = string.Empty;
                newFightOutputLabel.Text = string.Empty;

                weightClassDropdown.SelectedIndex = -1;
                weightClassPlaceholder.Visibility = Visibility.Visible;

                AddFighterExpander.IsExpanded = false;
                AddFightExpander.IsExpanded = false;
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == submitNewFighterButton)
            {
                var _fighterName = newFighterNameInput.Text.Trim();
                if (string.IsNullOrWhiteSpace(_fighterName))
                {
                    fighterAddedInfoText.Text = "Please enter a fighter name.";
                    fighterAddedInfoText.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                // Check if fighter already exists
                if (FighterManager.GetFighter(_fighterName) != null)
                {
                    fighterAddedInfoText.Text = $"{_fighterName} already exists!";
                    fighterAddedInfoText.Foreground = System.Windows.Media.Brushes.OrangeRed;
                    return;
                }

                // Get selected division
                int division = weightClassDropdown.SelectedIndex + 1;
                if (weightClassDropdown.SelectedIndex == -1)
                {
                    FighterManager.AddFighter(_fighterName, 6);
                    fighterAddedInfoText.Text = $"{_fighterName} added without weightclass";
                }
                else
                {
                    FighterManager.AddFighter(_fighterName, division);
                    fighterAddedInfoText.Text = $"{_fighterName} added successfully!";
                }

                fighterAddedInfoText.Foreground = System.Windows.Media.Brushes.Blue;

                // Reset inputs
                newFighterNameInput.Clear();
                newFighterNameOverlay.Visibility = System.Windows.Visibility.Visible;
                weightClassDropdown.SelectedIndex = -1;
                weightClassPlaceholder.Visibility = System.Windows.Visibility.Visible;
            }
            else if (sender == submitFightButton)
            {
                var _winner = winnerTextInput.Text.Trim();
                var _loser = loserTextInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(_winner) || string.IsNullOrWhiteSpace(_loser))
                {
                    newFightOutputLabel.Text = "Both winner and loser names are required.";
                    newFightOutputLabel.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                if (_winner == _loser)
                {
                    newFightOutputLabel.Text = "Winner and loser cannot be the same.";
                    newFightOutputLabel.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                // Record the fight in backend
                FighterManager.RecordFight(_winner, _loser);

                newFightOutputLabel.Text = $"Fight recorded: {_winner} defeated {_loser}.";
                newFightOutputLabel.Foreground = System.Windows.Media.Brushes.Blue;

                winnerTextInput.Clear();
                loserTextInput.Clear();
                winnerTextOverlay.Visibility = System.Windows.Visibility.Visible;
                loserTextOverlay.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void newFighterNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            newFighterNameOverlay.Visibility = string.IsNullOrWhiteSpace(newFighterNameInput.Text)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void fightInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == winnerTextInput)
            {
                winnerTextOverlay.Visibility = string.IsNullOrWhiteSpace(winnerTextInput.Text)
                    ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == loserTextInput)
            {
                loserTextOverlay.Visibility = string.IsNullOrWhiteSpace(loserTextInput.Text)
                    ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void AddFighterExpander_Expanded(object sender, RoutedEventArgs e)
        {
            AddFightExpander.IsExpanded = false;
        }

        private void AddFightExpander_Expanded(object sender, RoutedEventArgs e)
        {
            AddFighterExpander.IsExpanded = false;
        }

        private void WeightClassDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Show or hide placeholder based on selection
            if (weightClassDropdown.SelectedItem is ComboBoxItem _selected)
            {
                weightClassPlaceholder.Visibility = Visibility.Hidden;
                string _selectedWeight = _selected.Content.ToString();
                Console.WriteLine($"Selected weightclass: {_selectedWeight}");
            }
            else
            {
                weightClassPlaceholder.Visibility = Visibility.Visible;
            }
        }
    }
}
