using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CourseWork.UserClass;
using System.Windows.Threading;
using System.Windows.Controls;
using CourseWork.TextClass;
using CourseWork.GameClass;

namespace CourseWork.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string TextNameValue;
        private string TextLanguageValue;
        private User userBuffer;
        private DispatcherTimer timer;
        private List<Text> EnglishTexts = new List<Text>();
        private List<Text> RussianTexts = new List<Text>();
        private List<Text> OtherTexts = new List<Text>();
        private string[] SplitArray = new string[50];
        private List<string> WordsForUser = new List<string>();
        private bool TimerFlag = false;
        private int timecounter = 60;
        private int counter = 0;
        public MainWindow(User obj)
        {
            userBuffer = obj;
            InitializeComponent();
            using (TextContext contextToGetUserTexts = new TextContext())
            {
                var TextsWordsVarRussian = from p in contextToGetUserTexts.Texts where (p.UserName == "admin") && p.TextLanguage == "Russian" select p;
                foreach (Text a in TextsWordsVarRussian)
                {
                    RussianTexts.Add(a);
                }
                var TextsWordsVarEnglish = from p in contextToGetUserTexts.Texts where (p.UserName == "admin") && p.TextLanguage == "English" select p;
                foreach (Text a in TextsWordsVarEnglish)
                {
                    EnglishTexts.Add(a);
                }
                var TextsWordsVarOther = from p in contextToGetUserTexts.Texts where (p.UserName == userBuffer.UserName) && p.TextLanguage == "Other" select p;
                foreach (Text a in TextsWordsVarOther)
                {
                    OtherTexts.Add(a);
                    OtherTextComboBox.Items.Add(a.TextName);
                }
            }
            TextChooseComboBox.SelectedIndex = 0;
            LanguageChooseComboBox.SelectedIndex = 0;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1d);
            timer.Tick += new EventHandler(TimeSetter);
        }

        private void SpacePressed(object sender, KeyEventArgs e)
        {
            if (TimerFlag == false)
            {
                timer.Start();
                LanguageChooseComboBox.IsEnabled = false;
                TextChooseComboBox.IsEnabled = false;
            }
            if (e.Key == Key.Space)
            {
                if (UserTextBox.Text.Trim(' ') == WordsForUser[0])
                {
                    WordsTextBox.Clear();
                    WordsForUser.RemoveAt(0);
                    UserTextBox.Clear();
                    counter++;
                    PointsText.Content="";
                    PointsText.Content += counter.ToString();
                    foreach (string a in WordsForUser)
                    {
                        WordsTextBox.Text += ' ' + a;
                    }
                }
            }
            if (WordsForUser.Count <= 30)
            {
                Random rnd = new Random();
                for(int i = 0; i < 20; i++)
                {
                    WordsForUser.Add(SplitArray[rnd.Next(0, SplitArray.Length)]);

                }
            }
        }

        private void TimeSetter(object sender, EventArgs e)
        {
            if (timecounter > 0)
            {
                TimeShower.Content="";
                TimeShower.Content = timecounter--.ToString();
            }
            else
            {
                using(GameContext contextToSaveGame=new GameContext())
                {
                    Game GameToSave = new Game(userBuffer.UserName, TextNameValue, TextLanguageValue, counter);
                    contextToSaveGame.Games.Add(GameToSave);
                    contextToSaveGame.SaveChanges();
                }
                counter = 0;
                timecounter = 60;
                timer.Stop();
                TimerFlag = false;
                UserTextBox.IsEnabled = false;
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePage newWindow = new ProfilePage(userBuffer);
            newWindow.Show();
            Close();
        }
        private void RatingButton_Click(object sender, RoutedEventArgs e)
        {
            RatingWindow newWindow = new RatingWindow(userBuffer);
            newWindow.Show();
            Close();
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateWindow newWindow = new CreateWindow(userBuffer);
            newWindow.Show();
            Close();
        }
        private void RestartButton_Click(object sender,RoutedEventArgs e)
        {
            TimeShower.Content = "60";
            PointsText.Content = "0";
            UserTextBox.IsEnabled = true;
            LanguageChooseComboBox.IsEnabled = true;
            TextChooseComboBox.IsEnabled = true;
            counter = 0;
            timecounter = 60;
            timer.Stop();
            TimerFlag = false;
            GenerateText();
        }

        private void TextChooseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenerateText();
        }
        private void GenerateText()
        {
            Random rnd = new Random();
            WordsForUser.Clear();
            if (LanguageChooseComboBox.SelectedIndex == 0)
            {
                SplitArray = RussianTexts[TextChooseComboBox.SelectedIndex].TextWords.Split(' ');
                TextNameValue = RussianTexts[TextChooseComboBox.SelectedIndex].TextName;
                TextLanguageValue = "Russian";
            }
            else if(LanguageChooseComboBox.SelectedIndex==1)
            {
                SplitArray = EnglishTexts[TextChooseComboBox.SelectedIndex].TextWords.Split(' ');
                TextNameValue = EnglishTexts[TextChooseComboBox.SelectedIndex].TextName;
                TextLanguageValue = "English";
            }
            for (int i = 0; i < 50; i++)
            {
                WordsForUser.Add(SplitArray[rnd.Next(0, SplitArray.Length)]);
            }
            WordsTextBox.Clear();
            foreach (string a in WordsForUser)
            {
                WordsTextBox.Text += ' ' + a;
            }
        }

        private void OtherTextComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OtherTextComboBox.SelectedIndex != 0)
            {
                Random rnd = new Random();
                WordsForUser.Clear();
                TextChooseComboBox.IsEnabled = false;
                LanguageChooseComboBox.IsEnabled = false;
                SplitArray = OtherTexts[TextChooseComboBox.SelectedIndex].TextWords.Split(' ');
                TextNameValue = OtherTexts[TextChooseComboBox.SelectedIndex].TextName;
                TextLanguageValue = "Other";
                for (int i = 0; i < 50; i++)
                {
                    WordsForUser.Add(SplitArray[rnd.Next(0, SplitArray.Length)]);
                }
                WordsTextBox.Clear();
                foreach (string a in WordsForUser)
                {
                    WordsTextBox.Text += ' ' + a;
                }
            }
            else
            {
                TextChooseComboBox.IsEnabled = true;
                LanguageChooseComboBox.IsEnabled = true;
            }
        }
    }
}
