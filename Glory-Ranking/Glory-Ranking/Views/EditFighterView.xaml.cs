using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Glory_Ranking.Views
{
    public partial class EditFighterView : UserControl
    {
        private List<Fighter> allFighters = new List<Fighter>();

        private Fighter? currentFighter;
        private string originalName = "";

        private readonly List<string> weightClasses = new List<string>()
        {
            "None", "Heavyweight", "Light heavyweight", "Middleweight", "Welterweight", "Featherweight"
        };

        private readonly List<TextBox> searchInfo;

        public Action ResetView;

        public EditFighterView()
        {
            InitializeComponent();

            allFighters = FighterManager.GetAllFighters(); // Make sure FighterManager has this method returning List<Fighter>

            searchInfo = new List<TextBox> { fighterName, fighterWeightclass, fighterElo, fighterPeakElo };

            fighterWeightDropdown.ItemsSource = weightClasses;
            fighterWeightDropdown.Visibility = Visibility.Hidden;
            fighterWeightclass.Visibility = Visibility.Visible;

            ResetView = () =>
            {
                searchBox.TextChanged -= searchBox_TextChanged;
                searchBox.Text = "";
                searchBox.TextChanged += searchBox_TextChanged;

                fighterName.Text = "Name...";
                fighterWeightclass.Text = "Weightclass...";
                fighterElo.Text = "Ranking...";
                fighterPeakElo.Text = "Peak ranking...";

                SetEditButtonsVisibility(false);

                checkRetiredOrNot.IsEnabled = false;
                SetRetiredCheckbox(false);

                foreach (var _item in searchInfo)
                {
                    _item.Foreground = Brushes.Silver;
                    _item.IsEnabled = false;
                }

                fighterWeightDropdown.Visibility = Visibility.Hidden;
                fighterWeightclass.Visibility = Visibility.Visible;

                currentFighter = null;
                originalName = "";
            };

            ResetView?.Invoke();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtSearchPlaceholder.Visibility =
                string.IsNullOrWhiteSpace(searchBox.Text) ? Visibility.Visible : Visibility.Collapsed;

            string _input = searchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(_input))
            {
                suggestionPopup.IsOpen = false;
                ResetView?.Invoke(); // Clear everything if search is empty
                return;
            }

            // Always get fresh fighter list
            var allFighters = FighterManager.GetAllFighters();

            // Get up to 5 suggestions containing the input
            var _suggestions = allFighters
                .Where(f => f.Name.Contains(_input, StringComparison.OrdinalIgnoreCase))
                .Select(f => f.Name)
                .Take(5)
                .ToList();

            if (_suggestions.Any())
            {
                suggestionBox.ItemsSource = _suggestions;
                suggestionBox.SelectedIndex = 0;
                suggestionPopup.IsOpen = true;

                // 🔹 New: auto-load if exact match and only 1 suggestion
                if (_suggestions.Count == 1 &&
                    string.Equals(_input, _suggestions[0], StringComparison.OrdinalIgnoreCase))
                {
                    SelectSuggestion(_suggestions[0]);
                    return; // exit early, no need to show the popup
                }
            }
            else
            {
                suggestionPopup.IsOpen = false;
            }

            // 🔹 If input no longer matches the loaded fighter, reset view
            if (currentFighter != null &&
                !string.Equals(_input, currentFighter.Name, StringComparison.OrdinalIgnoreCase))
            {
                currentFighter = null;
                ResetViewProperties();
            }
        }

        // Helper to reset only the fighter info fields (not the search box)
        private void ResetViewProperties()
        {
            fighterName.Text = "Name...";
            fighterWeightclass.Text = "Weightclass...";
            fighterElo.Text = "Ranking...";
            fighterPeakElo.Text = "Peak ranking...";

            foreach (var _item in searchInfo)
            {
                _item.Foreground = Brushes.Silver;
                _item.IsEnabled = false;
            }

            SetEditButtonsVisibility(false);
            checkRetiredOrNot.IsEnabled = false;
            SetRetiredCheckbox(false);

            fighterWeightDropdown.Visibility = Visibility.Hidden;
            fighterWeightclass.Visibility = Visibility.Visible;
        }

        private void LoadFighterData(Fighter _fighter)
        {
            currentFighter = _fighter;
            originalName = _fighter.Name;

            fighterName.Text = _fighter.Name;
            fighterWeightclass.Text = GetWeightClassName(_fighter.Division);
            fighterElo.Text = _fighter.Elo.ToString();
            fighterPeakElo.Text = _fighter.PeakElo.ToString();

            foreach (var _item in searchInfo)
                _item.Foreground = Brushes.Black;

            fighterName.IsEnabled = false;
            fighterWeightclass.IsEnabled = false;
            fighterElo.IsEnabled = false;
            fighterPeakElo.IsEnabled = false;

            checkRetiredOrNot.IsEnabled = true;
            SetRetiredCheckbox(_fighter.Retired);

            bool _canEdit = !_fighter.Retired;
            SetEditButtonsVisibility(_canEdit);

            fighterWeightDropdown.Visibility = Visibility.Hidden;
            fighterWeightclass.Visibility = Visibility.Visible;
            fighterWeightDropdown.SelectedItem = GetWeightClassName(_fighter.Division);
        }

        private void SetRetiredCheckbox(bool _retired)
        {
            // Temporarily detach events
            checkRetiredOrNot.Checked -= checkRetiredOrNot_Checked;
            checkRetiredOrNot.Unchecked -= checkRetiredOrNot_Unchecked;

            checkRetiredOrNot.IsChecked = _retired;
            checkRetiredOrNot.Foreground = Brushes.Black;

            // Reattach events
            checkRetiredOrNot.Checked += checkRetiredOrNot_Checked;
            checkRetiredOrNot.Unchecked += checkRetiredOrNot_Unchecked;
        }

        private string GetWeightClassName(int _division)
        {
            return _division switch
            {
                1 => "Heavyweight",
                2 => "Light heavyweight",
                3 => "Middleweight",
                4 => "Welterweight",
                5 => "Featherweight",
                _ => "None"
            };
        }

        private int GetDivisionFromName(string _name)
        {
            return _name switch
            {
                "Heavyweight" => 1,
                "Light heavyweight" => 2,
                "Middleweight" => 3,
                "Welterweight" => 4,
                "Featherweight" => 5,
                _ => 6
            };
        }

        private void EditName_Click(object sender, RoutedEventArgs e)
        {
            if (currentFighter == null || currentFighter.Retired) return;
            ToggleEdit(fighterName, editNameButton, setNameButton);
        }

        private void SetName_Click(object sender, RoutedEventArgs e)
        {
            if (currentFighter == null) return;

            CommitEdit(fighterName, editNameButton, setNameButton);
            currentFighter.Name = fighterName.Text.Trim();
            FighterManager.UpdateFighter(currentFighter);

            searchBox.TextChanged -= searchBox_TextChanged;
            searchBox.Text = currentFighter.Name;
            searchBox.TextChanged += searchBox_TextChanged;
        }

        private void EditWeight_Click(object sender, RoutedEventArgs e)
        {
            if (currentFighter == null || currentFighter.Retired) return;

            fighterWeightDropdown.Visibility = Visibility.Visible;
            fighterWeightclass.Visibility = Visibility.Hidden;

            fighterWeightDropdown.SelectedItem = fighterWeightclass.Text;
            fighterWeightDropdown.IsEnabled = true;

            editWeightButton.Visibility = Visibility.Hidden;
            setWeightButton.Visibility = Visibility.Visible;
        }

        private void SetWeight_Click(object sender, RoutedEventArgs e)
        {
            if (currentFighter == null) return;

            if (fighterWeightDropdown.SelectedItem != null)
            {
                string _selectedWeight = fighterWeightDropdown.SelectedItem.ToString();
                fighterWeightclass.Text = _selectedWeight;
                currentFighter.Division = GetDivisionFromName(_selectedWeight);
                FighterManager.UpdateFighter(currentFighter);
            }

            CommitWeightEdit();
        }

        private void checkRetiredOrNot_Checked(object sender, RoutedEventArgs e)
        {
            if (currentFighter == null) return;

            SavePendingEdits();

            currentFighter.Retired = true;
            FighterManager.UpdateFighter(currentFighter);

            SetEditButtonsVisibility(false);
        }

        private void checkRetiredOrNot_Unchecked(object sender, RoutedEventArgs e)
        {
            if (currentFighter == null) return;

            SavePendingEdits();

            currentFighter.Retired = false;
            FighterManager.UpdateFighter(currentFighter);

            SetEditButtonsVisibility(true);
        }

        private void SavePendingEdits()
        {
            if (currentFighter == null) return;

            if (fighterName.IsEnabled)
            {
                currentFighter.Name = fighterName.Text.Trim();
                FighterManager.UpdateFighter(currentFighter);

                searchBox.TextChanged -= searchBox_TextChanged;
                searchBox.Text = currentFighter.Name;
                searchBox.TextChanged += searchBox_TextChanged;

                CommitEdit(fighterName, editNameButton, setNameButton);
            }

            if (fighterWeightDropdown.Visibility == Visibility.Visible)
            {
                if (fighterWeightDropdown.SelectedItem != null)
                {
                    string _selectedWeight = fighterWeightDropdown.SelectedItem.ToString();
                    fighterWeightclass.Text = _selectedWeight;
                    currentFighter.Division = GetDivisionFromName(_selectedWeight);
                    FighterManager.UpdateFighter(currentFighter);
                }

                fighterWeightDropdown.Visibility = Visibility.Hidden;
                fighterWeightclass.Visibility = Visibility.Visible;
                setWeightButton.Visibility = Visibility.Hidden;
                editWeightButton.Visibility = Visibility.Visible;
            }
        }

        private void ToggleEdit(TextBox _textBox, Button _editBtn, Button _doneBtn)
        {
            _textBox.IsEnabled = true;
            _textBox.Focus();
            _textBox.CaretIndex = _textBox.Text.Length;
            _editBtn.Visibility = Visibility.Hidden;
            _doneBtn.Visibility = Visibility.Visible;
        }

        private void CommitEdit(TextBox _textBox, Button _editBtn, Button _doneBtn, bool _forceHide = false)
        {
            _textBox.IsEnabled = false;
            _editBtn.Visibility = _forceHide ? Visibility.Hidden : Visibility.Visible;
            _doneBtn.Visibility = Visibility.Hidden;
        }

        private void CommitWeightEdit()
        {
            fighterWeightDropdown.IsEnabled = false;
            fighterWeightDropdown.Visibility = Visibility.Hidden;
            fighterWeightclass.Visibility = Visibility.Visible;

            setWeightButton.Visibility = Visibility.Hidden;
            editWeightButton.Visibility = Visibility.Visible;
        }

        private void SetEditButtonsVisibility(bool _enabled)
        {
            editNameButton.Visibility = _enabled ? Visibility.Visible : Visibility.Hidden;
            editWeightButton.Visibility = _enabled ? Visibility.Visible : Visibility.Hidden;

            setNameButton.Visibility = Visibility.Hidden;
            setWeightButton.Visibility = Visibility.Hidden;
        }

        private void MouseHoverEnter(object sender, MouseEventArgs e)
        {
            Button _sendButton = sender as Button;
            if (_sendButton.RenderTransform is ScaleTransform _st)
            {
                _st.ScaleX = 1.1;
                _st.ScaleY = 1.1;
            }
        }

        private void MouseHoverExit(object sender, MouseEventArgs e)
        {
            Button _sendButton = sender as Button;
            if (_sendButton.RenderTransform is ScaleTransform _st)
            {
                _st.ScaleX = 1.0;
                _st.ScaleY = 1.0;
            }
        }

        private void suggestionBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (suggestionBox.SelectedItem != null)
            {
                SelectSuggestion(suggestionBox.SelectedItem.ToString());
            }
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!suggestionPopup.IsOpen)
                return;

            if (e.Key == Key.Down)
            {
                suggestionBox.Focus();
                suggestionBox.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (suggestionBox.SelectedItem != null)
                {
                    SelectSuggestion(suggestionBox.SelectedItem.ToString());
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Escape)
            {
                suggestionPopup.IsOpen = false;
                e.Handled = true;
            }
        }

        private void SelectSuggestion(string name)
        {
            suggestionPopup.IsOpen = false;

            searchBox.TextChanged -= searchBox_TextChanged;
            searchBox.Text = name;
            searchBox.TextChanged += searchBox_TextChanged;

            var fighter = FighterManager.GetFighter(name);
            if (fighter != null)
            {
                LoadFighterData(fighter);
            }

            Keyboard.ClearFocus();
        }

        private void Box_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && suggestionBox.SelectedItem != null)
            {
                SelectSuggestion(suggestionBox.SelectedItem.ToString());
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                suggestionPopup.IsOpen = false;
                searchBox.Focus();          
                e.Handled = true;
            }
        }
    }
}
