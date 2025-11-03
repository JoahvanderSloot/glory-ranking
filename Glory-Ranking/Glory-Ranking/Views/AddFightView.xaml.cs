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
            ResetView = () => { };
        }

        // Called when any weight class RadioButton is selected
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb)
            {
                // Optional: you can handle logic here like setting a selected weight variable
                // string selectedWeight = rb.Content.ToString();
            }
        }

        // Handles both "Add fighter" and "Add fight" button clicks
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

                fighterAddedInfoText.Text = $"{fighterName} added successfully!";
                fighterAddedInfoText.Foreground = System.Windows.Media.Brushes.Blue;

                newFighterNameInput.Clear();
                newFighterNameOverlay.Visibility = Visibility.Visible;
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

                newFightOutputLabel.Text = $"Fight recorded: {winner} defeated {loser}.";
                newFightOutputLabel.Foreground = System.Windows.Media.Brushes.Blue;

                winnerTextInput.Clear();
                loserTextInput.Clear();
                winnerTextOverlay.Visibility = Visibility.Visible;
                loserTextOverlay.Visibility = Visibility.Visible;
            }
        }

        // Overlay control for fighter name input
        private void newFighterNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            newFighterNameOverlay.Visibility = string.IsNullOrWhiteSpace(newFighterNameInput.Text)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        // Overlay control for fight input boxes
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
            if (weightClassDropdown.SelectedItem is ComboBoxItem selected)
            {
                string selectedWeight = selected.Content.ToString();
                // You can store this value or use it as needed
                Console.WriteLine($"Selected weightclass: {selectedWeight}");
            }
        }

    }
}
