using System.Windows;
using CourseWork.UserClass;
using CourseWork.GameClass;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace CourseWork.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Window
    {
        User userBuffer;
        UserContext users;
        List<Game> GamesList = new List<Game>();
         
        public ProfilePage(User obj)
        {
            InitializeComponent();
            userBuffer = obj;
            Load();
        }

        //Смена пароля
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (InputOldPasswordBox.Password == userBuffer.UserPassword)
            {
                if (InputNewPasswordBox.Password == RepeatNewPasswordBox.Password)
                {
                    using (users = new UserContext())
                    {
                        User bufferToChangePassword = users.Users.Find(userBuffer.UserName);
                        bufferToChangePassword.UserPassword = RepeatNewPasswordBox.Password;
                        users.SaveChanges();
                        Load();
                        MessageBox.Show("Пароль успешно изменен!");
                    }
                }
                else
                {
                    MessageBox.Show("Подтвердите новый пароль!");
                }
            }
            else
            {
                MessageBox.Show("Введён неверный пароль!");
            }
        }
        //Фильтры для игр пользователя
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Regex TextNameRegex;
            Regex TextLanguageRegex;
            List<Game> Buffer = new List<Game>();
            if (TextChooseComboBox.SelectedIndex == 0)
            {
                TextNameRegex = new Regex("");
            }
            else
            {
                TextNameRegex = new Regex(TextChooseComboBox.Text);
            }
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

        //Установки при загрузке окна
        private void Load()
        {
            using(UserContext contextToGetUser=new UserContext())
            {
                userBuffer = contextToGetUser.Users.Find(userBuffer.UserName);
            }
            UserNameTextBox.Content = userBuffer.UserName;
            UserCountryTextBox.Content = userBuffer.UserCountry;
            using (GameContext contextToGetGames = new GameContext())
            {
                var Games = from p in contextToGetGames.Games where p.UserName == userBuffer.UserName select p;
                foreach (Game a in Games)
                {
                    GamesList.Add(a);
                }
            }
            foreach(Game a in GamesList)
            {
                if(a.TextName=="100Russian" || a.TextName == "100English")
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
            foreach (string a in (from p in GamesList select p.TextName).Distinct())
            {
                TextChooseComboBox.Items.Add(a);
            }
            try
            {
                Stream StremObj = new MemoryStream(userBuffer.AvatarImage);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = StremObj;
                bitmapImage.EndInit();
                Avatar.Source = null;
                Avatar.Source = bitmapImage;
            }
            catch { }
            GamesListBox.ItemsSource = GamesList;
        }

        //Смена картинки
        private void ChangePicture_Click(object sender, RoutedEventArgs e)
        {
            byte[] ImageData;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                MessageBox.Show(path);
                byte[] imageBytesBuffer;
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    imageBytesBuffer = new byte[fs.Length];
                    fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
                }
                ImageData = imageBytesBuffer;
                using (users = new UserContext())
                {
                    User bufferToChangePicture=users.Users.Find(userBuffer.UserName);
                    bufferToChangePicture.AvatarImage = ImageData;
                    users.SaveChanges();
                }
            }
            Load();

        }
        //Сортировка
        private void Sort_Click(object sender,RoutedEventArgs e)
        {
            List<Game> GameBuffer = new List<Game>();
            Button ButtonBuffer = (Button)sender;
            MessageBox.Show(ButtonBuffer.Name);
            switch (ButtonBuffer.Name)
            {
                case "ID":
                    var SelectionBufferID = from p in GamesList orderby p.GameID descending select p;
                    foreach (Game a in SelectionBufferID)
                    {
                        GameBuffer.Add(a);
                    }
                    break;
                case "Text":
                    var SelectionBufferText = from p in GamesList orderby p.GameID ascending select p;
                    foreach (Game a in SelectionBufferText)
                    {
                        GameBuffer.Add(a);
                    }
                    break;
                case "Language":
                    var SelectionBufferLanguage = from p in GamesList orderby p.GameID ascending select p;
                    foreach (Game a in SelectionBufferLanguage)
                    {
                        GameBuffer.Add(a);
                    }
                    break;
                case "Result":
                    var SelectionBufferResult = from p in GamesList orderby p.GameID ascending select p;
                    foreach (Game a in SelectionBufferResult)
                    {
                        GameBuffer.Add(a);
                    }
                    break;
                default:
                    break;
            }
            GamesListBox.ItemsSource = GameBuffer;
        }

        //Переходы между окнами
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow(userBuffer);
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
    }
}
