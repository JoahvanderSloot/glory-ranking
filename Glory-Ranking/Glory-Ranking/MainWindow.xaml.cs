using System.Diagnostics;
using System.Drawing;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Glory_Ranking
{
    public partial class MainWindow : Window
    {
        string testName = "Joah";
        string testWeight = "Heavyweight";

        //BackendData
        List<string> weightClasses = new List<string>() { "Heavyweight", "Light heavyweight", "Middelweight", "Welterweight", "Featherweight", "None" };

        //Leaderboard UI elements
        ListBox leaderboard;
        ComboBox divisionBox;
        TextBox chooseDivPreset;
        //Edit Fighter UI elements
        TextBox searchFighter;
        Label searchPrev;
        CheckBox active;
        Button editName;
        Image editNameIMG;
        Button setName;
        Image setNameIMG;
        Button editWeight;
        Image editWeightIMG;
        Button setWeight;
        Image setWeightIMG;
        List<TextBox> searchInfo = new List<TextBox>();
        //Add Fighter UI elements
        Expander newFighterWeightclass;
        TextBox newFighterName;
        Label newFighterTextOverlay;
        Button SubmitNewFighter;
        TextBlock figterAddedInfo;
        List<RadioButton> weightOptions = new List<RadioButton>();
        //Add Fight UI elements
        TextBox winner;
        TextBox loser;
        Label winnerOverlay;
        Label loserOverlay;
        TextBlock newFightOutput;
        Button addFight;

        public MainWindow()
        {
            InitializeComponent();

            //Assign leaderboard elements
            leaderboard = leaderboardBox;
            divisionBox = leaderboardDivision;
            chooseDivPreset = chooseDivPrev;

            //Assign search fighter elements
            searchFighter = searchBox;
            searchPrev = txtSearchPlaceholder;
            active = checkRetiredOrNot;

            //Assign edit fighter elements
            editName = editNameButton;
            editNameIMG = editNameButtonIMG;
            setName = setNameButton;
            setNameIMG = setNameButtonIMG;
            editWeight = editWeightButton;
            editWeightIMG = editWeightButtonIMG;
            setWeight = setWeightButton;
            setWeightIMG = setWeightButtonIMG;

            //Assign search info elements
            searchInfo.AddRange(new[] { fighterName, fighterWeightclass, fighterElo, fighterPeakElo });

            //Assign add fighter elements
            newFighterWeightclass = newFighterWeight;
            newFighterName = newFighterNameInput;
            newFighterTextOverlay = newFighterNameOverlay;
            SubmitNewFighter = submitNewFighterButton;
            figterAddedInfo = fighterAddedInfoText;
            weightOptions.AddRange(new[] { weightHeavy, weightLightHeavy, weightMiddle, weightWelter, weightFeather, weightNone });

            //Assign add fight elements
            winner = winnerTextInput;
            loser = loserTextInput;
            winnerOverlay = winnerTextOverlay;
            loserOverlay = loserTextOverlay;
            newFightOutput = newFightOutputLabel;
            addFight = submitFightButton;
        }

        private void leaderboardDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chooseDivPreset.Visibility = divisionBox.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchPrev.Visibility = searchFighter.Text == "" ? Visibility.Visible : Visibility.Hidden;

            if (searchFighter.Text == testName)
            {
                foreach (var item in searchInfo)
                {
                    item.Foreground = Brushes.Black;
                }
                editName.Visibility = Visibility.Visible;
                editWeight.Visibility = Visibility.Visible;
                active.IsEnabled = true;
                active.Foreground = Brushes.Black;
                active.IsChecked = true;
                searchInfo[0].Text = testName;
                searchInfo[1].Text = testWeight;
                searchInfo[2].Text = 2000.ToString();
                searchInfo[3].Text = 2100.ToString();
            }
            else
            {
                foreach (var item in searchInfo)
                {
                    item.Foreground = Brushes.Silver;
                    item.IsEnabled = false;
                }
                editName.Visibility = Visibility.Hidden;
                editWeight.Visibility = Visibility.Hidden;
                setName.Visibility = Visibility.Hidden;
                setWeight.Visibility = Visibility.Hidden;
                active.IsEnabled = false;
                active.Foreground = Brushes.Silver;
                active.IsChecked = false;
                searchInfo[0].Text = "Name...";
                searchInfo[1].Text = "Weightclass...";
                searchInfo[2].Text = "Ranking...";
                searchInfo[3].Text = "Peak ranking...";
            }

            editWeightIMG.Visibility = editWeight.Visibility;
            editNameIMG.Visibility = editName.Visibility;
            setNameIMG.Visibility = setName.Visibility;
            setWeightIMG.Visibility = setWeight.Visibility;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;
            if (_button.Name == "editNameButton")
            {
                searchInfo[0].IsEnabled = true;
                editName.Visibility = Visibility.Hidden;
                setName.Visibility = Visibility.Visible;
            }
            else if (_button.Name == "setNameButton")
            {
                testName = searchInfo[0].Text;
                searchInfo[0].IsEnabled = false;
                searchFighter.Text = testName;
                setName.Visibility = Visibility.Hidden;
                editName.Visibility = Visibility.Visible;
            }
            if (_button.Name == "editWeightButton")
            {
                searchInfo[1].IsEnabled = true;
                editWeight.Visibility = Visibility.Hidden;
                setWeight.Visibility = Visibility.Visible;
            }
            else if (_button.Name == "setWeightButton")
            {
                foreach (var weight in weightClasses)
                {
                    if (searchInfo[1].Text == weight)
                    {
                        searchInfo[1].IsEnabled = false;
                        setWeight.Visibility = Visibility.Hidden;
                        editWeight.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //Input valid weight class
                    }
                }
            }

            editWeightIMG.Visibility = editName.Visibility;
            editNameIMG.Visibility = editWeight.Visibility;
            setNameIMG.Visibility = setName.Visibility;
            setWeightIMG.Visibility = setWeight.Visibility;

            if (_button.Name == "submitNewFighterButton")
            {
                for (int i = 0; i < weightOptions.Count; i++)
                {
                    if (weightOptions[i].IsChecked == true && newFighterName.Text != "")
                    {
                        if (newFighterName.Text != "ExistingFighter")
                        {
                            fighterAddedInfoText.Foreground = Brushes.Blue;
                            figterAddedInfo.Text = "New fighter added: " + newFighterName.Text;
                            testName = newFighterName.Text;
                            testWeight = weightClasses[i];
                            weightOptions[i].IsChecked = false;
                            newFighterWeightclass.Header = "Weightclass";
                            newFighterName.Text = "";
                            newFighterTextOverlay.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            fighterAddedInfoText.Foreground = Brushes.Orange;
                            figterAddedInfo.Text = "Fighter " + newFighterName.Text + "already exists";
                        }
                    }
                }
            }
            else if (_button.Name == "submitFightButton")
            {
                newFightOutput.Foreground = Brushes.Blue;
                newFightOutput.Text = "Fight added " + winner.Text + " defeated " + loser.Text;
                addFight.IsEnabled = false;
                winner.Text = "";
                loser.Text = "";
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton _button = (RadioButton)sender;
            foreach (var weight in weightOptions)
            {
                if (_button.Name == weight.Name)
                {
                    newFighterWeightclass.Header = weight.Content;
                    newFighterWeightclass.IsExpanded = false;
                    if (newFighterName.Text != "")
                    {
                        SubmitNewFighter.IsEnabled = true;
                    }
                }
            }
        }

        private void newFighterNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (newFighterName.Text == "")
            {
                newFighterTextOverlay.Visibility = Visibility.Visible;
                SubmitNewFighter.IsEnabled = false;
            }
            else
            {
                newFighterTextOverlay.Visibility = Visibility.Hidden;
                foreach (var weight in weightOptions)
                {
                    if (weight.IsChecked == true)
                    {
                        SubmitNewFighter.IsEnabled = true;
                    }
                }
            }
        }

        private void fightInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Name == "winnerTextInput")
            {
                if (winner.Text != "")
                {
                    winnerOverlay.Visibility = Visibility.Hidden;
                }
                else
                {
                    winnerOverlay.Visibility = Visibility.Visible;
                }
            }
            else if (textBox.Name == "loserTextInput")
            {
                if (loser.Text != "")
                {
                    loserOverlay.Visibility = Visibility.Hidden;
                }
                else
                {
                    loserOverlay.Visibility = Visibility.Visible;
                }
            }

            if (loser.Text != "" && winner.Text != "")
            {
                if (loser.Text == winner.Text)
                {
                    newFightOutput.Foreground = Brushes.Orange;
                    newFightOutput.Text = "Winner and loser cant be the same fighter";
                    addFight.IsEnabled = false;
                }
                else
                {
                    newFightOutput.Text = "";
                    addFight.IsEnabled = true;
                }
            }
            else
            {
                addFight.IsEnabled = false;
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Reset add page
            newFighterWeightclass.IsExpanded = false;
            newFighterWeightclass.Header = "Weightclass";
            newFighterName.Text = "";
            newFighterTextOverlay.Visibility = Visibility.Visible;
            figterAddedInfo.Text = null;
            foreach (var weight in weightOptions)
            {
                weight.IsChecked = false;
            }
            winner.Text = "";
            loser.Text = "";
            newFightOutput.Text = null;


            //Reset search page
            searchFighter.Text = "";
            editName.Visibility = Visibility.Hidden;
            editWeight.Visibility = Visibility.Hidden;
            editWeightIMG.Visibility = editWeight.Visibility;
            editNameIMG.Visibility = editName.Visibility;
            setName.Visibility = Visibility.Hidden;
            setWeight.Visibility = Visibility.Hidden;
            setNameIMG.Visibility = setName.Visibility;
            setWeightIMG.Visibility = setWeight.Visibility;
            active.IsEnabled = false;
            active.Foreground = Brushes.Silver;
            active.IsChecked = false;
            searchInfo[0].Text = "Name...";
            searchInfo[1].Text = "Weightclass...";
            searchInfo[2].Text = "Ranking...";
            searchInfo[3].Text = "Peak ranking...";
        }
    }
}