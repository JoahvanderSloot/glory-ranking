using System.Diagnostics;
using System.Drawing;
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
        Button setName;
        Button editWeight;
        Button setWeight;
        List<TextBox> searchInfo = new List<TextBox>();
        //Add Fighter UI elements
        Expander newFighterWeightclass;
        TextBox newFighterName;
        Label newFighterTextOverlay;
        Button SubmitNewFighter;
        TextBlock figterAddedInfo;
        List<RadioButton> weightOptions = new List<RadioButton>();

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
            editName.Visibility = Visibility.Hidden;
            setName = setNameButton;
            setName.Visibility = Visibility.Hidden;
            editWeight = editWeightButton;
            editWeight.Visibility = Visibility.Hidden;
            setWeight = setWeightButton;
            setWeight.Visibility = Visibility.Hidden;

            //Assign search info elements
            searchInfo.AddRange(new[]{ fighterName, fighterWeightclass, fighterElo, fighterPeakElo });

            //Assign add fighter elements
            newFighterWeightclass = newFighterWeight;
            newFighterName = newFighterNameInput;
            newFighterTextOverlay = newFighterNameOverlay;
            SubmitNewFighter = submitNewFighterButton;
            figterAddedInfo = fighterAddedInfoText;
            weightOptions.AddRange(new[] { weightHeavy, weightLightHeavy, weightMiddle, weightWelter, weightFeather, weightNone });
        }

        private void leaderboardDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chooseDivPreset.Visibility = divisionBox.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchPrev.Visibility = searchFighter.Text == "" ? Visibility.Visible : Visibility.Hidden;

            if(searchFighter.Text == testName)
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
                foreach(var item in searchInfo)
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
            else if(_button.Name == "setNameButton")
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
                foreach(var weight in weightClasses)
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

            if(_button.Name == "submitNewFighterButton")
            {
                for(int i = 0; i < weightOptions.Count; i++)
                {
                    if (weightOptions[i].IsChecked == true && newFighterName.Text != "")
                    {
                        if(newFighterName.Text != "ExistingFighter")
                        {
                            fighterAddedInfoText.Foreground = Brushes.Lime;
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
                            fighterAddedInfoText.Foreground = Brushes.Red;
                            figterAddedInfo.Text = "Fighter " + newFighterName.Text + "already exists";
                        }
                    }
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton _button = (RadioButton)sender;
            foreach(var weight in weightOptions)
            {
                if(_button.Name == weight.Name)
                {
                    newFighterWeightclass.Header = weight.Content;
                    newFighterWeightclass.IsExpanded = false;
                    if(newFighterName.Text != "")
                    {
                        SubmitNewFighter.IsEnabled = true;
                    }
                }
            }
        }

        private void newFighterNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(newFighterName.Text == "")
            {
                newFighterTextOverlay.Visibility = Visibility.Visible;
                SubmitNewFighter.IsEnabled = false;
            }
            else
            {
                newFighterTextOverlay.Visibility = Visibility.Hidden;
                foreach(var weight in weightOptions)
                {
                    if(weight.IsChecked == true)
                    {
                        SubmitNewFighter.IsEnabled = true;
                    }
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Reset edit page
            newFighterWeightclass.IsExpanded = false;
            newFighterWeightclass.Header = "Weightclass";
            newFighterName.Text = "";
            newFighterTextOverlay.Visibility = Visibility.Visible;
            figterAddedInfo.Text = null;
            foreach (var weight in weightOptions)
            {
                weight.IsChecked = false;
            }

            //Reset search page
            searchFighter.Text = "";
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
    }
}