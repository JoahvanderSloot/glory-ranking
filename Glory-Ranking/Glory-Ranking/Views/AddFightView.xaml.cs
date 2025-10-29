using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Glory_Ranking.Views
{
    public partial class AddFightView : UserControl
    {
        string testName = "Joah";
        string testWeight = "Heavyweight";

        List<string> weightClasses = new List<string>() { "None", "Heavyweight", "Light heavyweight", "Middelweight", "Welterweight", "Featherweight" };
        List<RadioButton> weightOptions;

        public Action ResetView;

        public AddFightView()
        {
            InitializeComponent();
            weightOptions = new List<RadioButton>() { weightNone, weightHeavy, weightLightHeavy, weightMiddle, weightWelter, weightFeather };

            ResetView = () =>
            {
                newFighterWeight.IsExpanded = false;
                newFighterWeight.Header = "Weightclass";
                newFighterNameInput.Text = "";
                newFighterNameOverlay.Visibility = Visibility.Visible;
                fighterAddedInfoText.Text = "";
                foreach (var weight in weightOptions)
                    weight.IsChecked = false;

                winnerTextInput.Text = "";
                loserTextInput.Text = "";
                winnerTextOverlay.Visibility = Visibility.Visible;
                loserTextOverlay.Visibility = Visibility.Visible;
                newFightOutputLabel.Text = "";
                submitFightButton.IsEnabled = false;
                submitNewFighterButton.IsEnabled = false;
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;

            if (_button.Name == "submitNewFighterButton")
            {
                for (int i = 0; i < weightOptions.Count; i++)
                {
                    if (weightOptions[i].IsChecked == true && newFighterNameInput.Text != "")
                    {
                        if (newFighterNameInput.Text != "ExistingFighter")
                        {
                            fighterAddedInfoText.Foreground = Brushes.Blue;
                            fighterAddedInfoText.Text = "New fighter added: " + newFighterNameInput.Text;
                            testName = newFighterNameInput.Text;
                            testWeight = weightClasses[i];
                            weightOptions[i].IsChecked = false;
                            newFighterWeight.Header = "Weightclass";
                            newFighterNameInput.Text = "";
                            newFighterNameOverlay.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            fighterAddedInfoText.Foreground = Brushes.Orange;
                            fighterAddedInfoText.Text = "Fighter " + newFighterNameInput.Text + " already exists";
                        }
                    }
                }
            }
            else if (_button.Name == "submitFightButton")
            {
                newFightOutputLabel.Foreground = Brushes.Blue;
                newFightOutputLabel.Text = "Fight added " + winnerTextInput.Text + " defeated " + loserTextInput.Text;
                submitFightButton.IsEnabled = false;
                winnerTextInput.Text = "";
                loserTextInput.Text = "";
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton _button = (RadioButton)sender;
            foreach (var weight in weightOptions)
            {
                if (_button == weight)
                {
                    newFighterWeight.Header = weight.Content;
                    newFighterWeight.IsExpanded = false;
                    if (!string.IsNullOrEmpty(newFighterNameInput.Text))
                        submitNewFighterButton.IsEnabled = true;
                }
            }
        }

        private void newFighterNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(newFighterNameInput.Text))
            {
                newFighterNameOverlay.Visibility = Visibility.Visible;
                submitNewFighterButton.IsEnabled = false;
            }
            else
            {
                newFighterNameOverlay.Visibility = Visibility.Hidden;
                foreach (var weight in weightOptions)
                {
                    if (weight.IsChecked == true)
                        submitNewFighterButton.IsEnabled = true;
                }
            }
        }

        private void fightInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(winnerTextInput.Text))
                winnerTextOverlay.Visibility = Visibility.Hidden;
            else
                winnerTextOverlay.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(loserTextInput.Text))
                loserTextOverlay.Visibility = Visibility.Hidden;
            else
                loserTextOverlay.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty(winnerTextInput.Text) && !string.IsNullOrEmpty(loserTextInput.Text))
            {
                if (winnerTextInput.Text == loserTextInput.Text)
                {
                    newFightOutputLabel.Foreground = Brushes.Orange;
                    newFightOutputLabel.Text = "Winner and loser can't be the same fighter";
                    submitFightButton.IsEnabled = false;
                }
                else
                {
                    newFightOutputLabel.Text = "";
                    submitFightButton.IsEnabled = true;
                }
            }
            else
            {
                submitFightButton.IsEnabled = false;
            }
        }

    }
}
