using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Shapes;

namespace Sonya3laba
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> _nounsCollection;
        public ObservableCollection<string> Verbs { get; } = new ObservableCollection<string>();
        public string SelectedVerb { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            // Инициализация существительных
            _nounsCollection = new ObservableCollection<string>
            {
                "город", "река", "гора", "озеро", "остров", "страна"
            };
            NounsComboBox.ItemsSource = _nounsCollection;
            NounsComboBox.SelectedIndex = 0;

            // Добавляем глаголы
            Verbs.Add("находится");
            Verbs.Add("располагается");
            Verbs.Add("простирается");
            Verbs.Add("лежит");
            Verbs.Add("принадлежит");
            VerbsListBox.SelectedIndex = 0;

            // Установка контекста данных(для привязки)
            DataContext = this;

            // Настройка Expander
            ResultExpander.Expanded += (s, e) => UpdateDetailsText();
        }
        private void NounsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NounsComboBox.SelectedItem != null)
            {
                NounTextBox.Text = NounsComboBox.SelectedItem.ToString();
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NounTextBox.Text))
            {
                if (!_nounsCollection.Contains(NounTextBox.Text))
                {
                    _nounsCollection.Add(NounTextBox.Text);
                    NounsComboBox.SelectedItem = NounTextBox.Text;
                }
                else
                {
                    MessageBox.Show("Такое существительное уже существует!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (NounsComboBox.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(NounTextBox.Text))
            {
                if (!_nounsCollection.Contains(NounTextBox.Text))
                {
                    _nounsCollection[NounsComboBox.SelectedIndex] = NounTextBox.Text;
                }
                else
                {
                    MessageBox.Show("Такое существительное уже существует!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NounsComboBox.SelectedIndex >= 0)
            {
                var result = MessageBox.Show("Удалить выбранное существительное?", "Подтверждение",
                                           MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _nounsCollection.RemoveAt(NounsComboBox.SelectedIndex);

                    if (_nounsCollection.Count > 0)
                    {
                        NounsComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        NounTextBox.Text = "";
                    }
                }
            }
        }
        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            string adjective = GetSelectedAdjective();
            string noun = NounsComboBox.SelectedItem as string;
            string verb = SelectedVerb;

            ResultText.Text = $"{adjective} {noun} {verb}";
            UpdateDetailsText();
            ResultExpander.IsExpanded = true;
        }
        private void NounTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.A && e.Key <= Key.Z) ||          // Английские буквы
                (e.Key >= Key.Oem3 && e.Key <= Key.Oem102) ||  // Русские буквы
                e.Key == Key.Space ||                          // Пробел
                e.Key == Key.Back)                           // Backspace
            {
                e.Handled = false; // Разрешаем ввод
            }
            else
            {
                e.Handled = true;  // Запрещаем ввод
                MessageBox.Show("Можно вводить только буквы и пробелы!", "Ошибка ввода",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void UpdateDetailsText()
        {
            string noun = NounsComboBox.SelectedItem as string;
            DetailsText.Text = $"Вы выбрали: существительное '{noun}' (всего {_nounsCollection.Count} слов в списке)";
        }
        private string GetSelectedAdjective()
        {
            int i = 0;
            while (i < AdjectivesPanel.Children.Count)
            {
                if (AdjectivesPanel.Children[i] is RadioButton radioButton && radioButton.IsChecked == true)
                {
                    return radioButton.Content.ToString();
                }
                i++;
            }
            return string.Empty;
        }
    }
}