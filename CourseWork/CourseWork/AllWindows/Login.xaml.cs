using System.Windows;
using CourseWork.UserClass;
using System.Windows.Input;
using System;
using System.Windows.Controls;

namespace CourseWork.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        UserContext users;
        public Login()
        {
            InitializeComponent();
            users = new UserContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User buffer = users.Users.Find(LoginTextBox.Text);
            if (LoginTextBox.Text == "" || PasswordTextBox.Password == "")
            {
                ErrorTextBox.SetResourceReference(TextBox.TextProperty, "FieldsException");
                ErrorTextBox.Visibility = Visibility.Visible;
                return;
            }
            else if (buffer == null)
            {
                ErrorTextBox.SetResourceReference(TextBox.TextProperty, "UserNameNotExistsException");
                ErrorTextBox.Visibility = Visibility.Visible;
                return;
            }
            else if (buffer.UserPassword != PasswordTextBox.Password)
            {
                ErrorTextBox.SetResourceReference(TextBox.TextProperty, "PasswordException");
                ErrorTextBox.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                MainWindow newWindow = new MainWindow(buffer);
                newWindow.Show();
                Close();
            }
        }
        private void GoToRegisterForm(object sender, RoutedEventArgs e)
        {
            Register newWindow = new Register();
            newWindow.Show();
            Close();
        }
        private void EnterClicker(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(new object(), new RoutedEventArgs());
            }
        }
    }
}
