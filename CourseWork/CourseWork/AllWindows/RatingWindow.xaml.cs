using System.Linq;
using System.Windows;
using CourseWork.GameClass;
using CourseWork.UserClass;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CourseWork.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для RatingWindow.xaml
    /// </summary>
    public partial class RatingWindow : Window
    {
        User userBuffer;
        List<Game> GamesList = new List<Game>();
        public RatingWindow(User obj)
        {
            InitializeComponent();
            userBuffer = obj;
            TextChooseComboBox.SelectedIndex = 0;
            LanguageChooseComboBox.SelectedIndex = 0;
            Load();
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow(userBuffer);
            newWindow.Show();
            Close();
        }
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePage newWindow = new ProfilePage(userBuffer);
            newWindow.Show();
            Close();
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateWindow newWindow = new CreateWindow(userBuffer);
            newWindow.Show();
            Close();
        }
        private void Load()
        {
            using (GameContext contextToGetGames = new GameContext())
            {
                var Games = from p in contextToGetGames.Games where p.TextLanguage!="Other" orderby p.Result descending select p;
                foreach (Game a in Games)
                {
                    GamesList.Add(a);
                }
            }
            foreach (Game a in GamesList)
            {
                if (a.TextName == "100Russian" || a.TextName == "100English")
                {
                    a.TextName = "Начальный";
                }
                if (a.TextName == "300Russian" || a.TextName == "300English")
                {
                    a.TextName = "Продвинутый";
                }
                if (a.TextName == "500Russian" || a.TextName == "500English")
                {
                    a.TextName = "Мастер";
                }
                if (a.TextLanguage == "Russian")
                {
                    a.TextLanguage = "Русский";
                }
                if (a.TextLanguage == "English")
                {
                    a.TextLanguage = "Английский";
                }
            }
            GamesListBox.ItemsSource = GamesList;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Regex TextNameRegex;
            Regex TextLanguageRegex;
            List<Game> Buffer = new List<Game>();
            TextNameRegex = new Regex(TextChooseComboBox.Text);
            if (LanguageChooseComboBox.SelectedIndex == 0)
            {
                TextLanguageRegex = new Regex("");
            }
            else
            {
                TextLanguageRegex = new Regex(LanguageChooseComboBox.Text);
            }
            var LinqBuffer = from p in GamesList where TextNameRegex.IsMatch(p.TextName) && TextLanguageRegex.IsMatch(p.TextLanguage) select p;
            foreach (Game a in LinqBuffer)
            {
                Buffer.Add(a);
            }
            GamesListBox.ItemsSource = null;
            GamesListBox.ItemsSource = Buffer;
        }
    }
}
