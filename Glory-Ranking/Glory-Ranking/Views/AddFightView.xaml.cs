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

            // Define ResetView logic (called when switching tabs)
            ResetView = () =>
            {
                // Clear all textboxes
                newFighterNameInput.Clear();
                winnerTextInput.Clear();
                loserTextInput.Clear();

                // Show overlays again
                newFighterNameOverlay.Visibility = Visibility.Visible;
                winnerTextOverlay.Visibility = Visibility.Visible;
                loserTextOverlay.Visibility = Visibility.Visible;

                // Clear labels
                fighterAddedInfoText.Text = string.Empty;
                newFightOutputLabel.Text = string.Empty;

                // Reset weight class dropdown
                weightClassDropdown.SelectedIndex = -1;
                weightClassPlaceholder.Visibility = Visibility.Visible;

                // Collapse both expanders
                AddFighterExpander.IsExpanded = false;
                AddFightExpander.IsExpanded = false;
            };
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb)
            {
                // Optional: handle selected weight
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == submitNewFighterButton)
            {
                var fighterName = newFighterNameInput.Text.Trim();
                if (string.IsNullOrWhiteSpace(fighterName))
                {
                    fighterAddedInfoText.Text = "Please enter a fighter name.";
                    fighterAddedInfoText.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                // Fighter added successfully
                fighterAddedInfoText.Text = $"{fighterName} added successfully!";
                fighterAddedInfoText.Foreground = System.Windows.Media.Brushes.Blue;

                // ✅ Clear input and reset overlay & weightclass
                newFighterNameInput.Clear();
                newFighterNameOverlay.Visibility = Visibility.Visible;
                weightClassDropdown.SelectedIndex = -1;
                weightClassPlaceholder.Visibility = Visibility.Visible;
            }
            else if (sender == submitFightButton)
            {
                var winner = winnerTextInput.Text.Trim();
                var loser = loserTextInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(winner) || string.IsNullOrWhiteSpace(loser))
                {
                    newFightOutputLabel.Text = "Both winner and loser names are required.";
                    newFightOutputLabel.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                if (winner == loser)
                {
                    newFightOutputLabel.Text = "Winner and loser cannot be the same.";
                    newFightOutputLabel.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                // ✅ Fight recorded successfully
                newFightOutputLabel.Text = $"Fight recorded: {winner} defeated {loser}.";
                newFightOutputLabel.Foreground = System.Windows.Media.Brushes.Blue;

                // Clear inputs & show overlays again
                winnerTextInput.Clear();
                loserTextInput.Clear();
                winnerTextOverlay.Visibility = Visibility.Visible;
                loserTextOverlay.Visibility = Visibility.Visible;
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
            if (weightClassDropdown.SelectedItem is ComboBoxItem selected)
            {
                weightClassPlaceholder.Visibility = Visibility.Hidden;
                string selectedWeight = selected.Content.ToString();
                Console.WriteLine($"Selected weightclass: {selectedWeight}");
            }
            else
            {
                weightClassPlaceholder.Visibility = Visibility.Visible;
            }
        }
    }
}
