using System.Windows;
using Microsoft.Win32;
using CourseWork.UserClass;
using System.Windows.Media;
using System.IO;
using System.Windows.Controls;

namespace CourseWork.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        byte[] ImageData;
        public Register()
        {
            InitializeComponent();
            
        }
        private void RegisterButton_Click(object sender,RoutedEventArgs e)
        {
            try
            {
                ErrorTextBox.Foreground = Brushes.Red;
                UserContext users = new UserContext();
                User buffer = new User();
                buffer.UserName = LoginTextBox.Text;
                buffer.UserPassword = InputPasswordBox.Password;
                buffer.UserCountry = CountryComboBox.Text;
                buffer.AvatarImage = ImageData;
                if (buffer.UserName == "" || buffer.UserPassword == "")
                {
                    ErrorTextBox.SetResourceReference(TextBox.TextProperty, "FieldsException");
                    ErrorTextBox.Visibility = Visibility.Visible;
                    return;
                }
                else if (buffer.UserPassword != RepeatPasswordBox.Password)
                {
                    ErrorTextBox.SetResourceReference(TextBox.TextProperty, "PasswordNotSameException");
                    ErrorTextBox.Visibility = Visibility.Visible;
                    return;
                }
                else if (buffer.AvatarImage == null)
                {
                    ErrorTextBox.SetResourceReference(TextBox.TextProperty, "PictureException");
                    ErrorTextBox.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    users.Users.Add(buffer);
                    users.SaveChanges();
                    ErrorTextBox.Foreground = Brushes.Green;
                    ErrorTextBox.SetResourceReference(TextBox.TextProperty, "SuccesNotException");
                    ErrorTextBox.Visibility = Visibility.Visible;
                    InputPasswordBox.Password = "";
                    RepeatPasswordBox.Password = "";
                    CountryComboBox.SelectedIndex = 0;
                    LoginTextBox.Text = "";
                    ImageData = null;
                }
            }
            catch
            {
                ErrorTextBox.SetResourceReference(TextBox.TextProperty, "UserNameException");
                ErrorTextBox.Visibility = Visibility.Visible;
                return;
            }
        }
        private void ReturnButton_Click(object sender,RoutedEventArgs e)
        {
            Login newWindow = new Login();
            newWindow.Show();
            Close();
        }

        private void ChoosePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter= "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
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
            }
        }
    }
}
