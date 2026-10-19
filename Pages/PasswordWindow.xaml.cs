using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Shapes;

namespace ScooterRental.Pages
{
    /// <summary>
    /// Логика взаимодействия для PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private User currentUser;
        public PasswordWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordPB.Password) || string.IsNullOrEmpty(NewPasswordPB.Password) || string.IsNullOrEmpty(ConfirmPasswordPB.Password))
            {
                MessageBox.Show("Все поля обязательны к заполнению!");
                return;
            }

            if (PasswordPB.Password.Length >= 6)
            {
                bool en = true;
                bool number = false;

                for (int i = 0; i < PasswordPB.Password.Length; i++)
                {
                    if (PasswordPB.Password[i] >= '0' && PasswordPB.Password[i] <= '9') number =
                    true;
                    else if (!((PasswordPB.Password[i] >= 'A' && PasswordPB.Password[i] <=
                    'Z') || (PasswordPB.Password[i] >= 'a' && PasswordPB.Password[i] <= 'z')))
                        en = false;
                }

                if (!en)
                {
                    MessageBox.Show("Используйте только английскую раскладку!");
                    return;
                }

                else if (!number)
                {
                    MessageBox.Show("Добавьте хотя бы одну цифру!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Пароль слишком короткий, должно быть минимум 6 символов!");
                return;
            }

            if (NewPasswordPB.Password != ConfirmPasswordPB.Password)
            {
                MessageBox.Show("Новый пароль не совпадает с подтверждением");
                NewPasswordPB.Clear();
                ConfirmPasswordPB.Clear();
                return;
            }

            string hashedPass = GetHash(PasswordPB.Password);
            var user = Core.Context.User.FirstOrDefault(u => u.Login == currentUser.Login && u.Password == hashedPass);

            if (user == null)
            {
                MessageBox.Show("Текущий пароль неверный!");

                return;
            }

            user.Password = GetHash(NewPasswordPB.Password);
            Core.Context.SaveChanges();
            MessageBox.Show("Пароль успешно изменен!");
            Close();

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        public static string GetHash(String password)
        {
            using (var hash = SHA1.Create())
            {
                return
                string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x =>
                x.ToString("X2")));
            }
        }
    }
}
