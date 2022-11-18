using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CourseWork.UserClass;
using CourseWork.TextClass;

namespace CourseWork.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        User userBuffer;
        public CreateWindow(User obj)
        {
            InitializeComponent();
            userBuffer = obj;
        }

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
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePage newWindow = new ProfilePage(userBuffer);
            newWindow.Show();
            Close();
        }

        private void CreateTextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextContext texts = new TextContext();
                Text buffer = new Text();
                buffer.UserName = userBuffer.UserName;
                buffer.TextLanguage = "Other";
                buffer.TextName = TextNameTextBox.Text;
                buffer.TextWords = WordsTextBox.Text;
                texts.Texts.Add(buffer);
                texts.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
