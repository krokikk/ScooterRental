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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();

            var uri = new Uri("Styles/StyleNight.xaml", UriKind.Relative );
            ResourceDictionary dict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }


        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            // проверка на заполненные поля
            if (string.IsNullOrEmpty(LoginTB.Text) || string.IsNullOrEmpty(PasswordPB.Password) || string.IsNullOrEmpty(Password2PB.Password) || string.IsNullOrEmpty(AgeDP.Text) ||
                string.IsNullOrEmpty(FioTB.Text) || string.IsNullOrEmpty(NumberTB.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            // проверка соответствия паролей
            if (PasswordPB.Password != Password2PB.Password)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            // проверка соответствия требованиям пароля
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
                    MessageBox.Show("Используйте только английскую расскладку!");
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


            // пользователь уже есть
            if (Core.Context.User.Any(u => u.Login == LoginTB.Text))
            {
                MessageBox.Show("Пользователь с таким логином уже существует.");
                return;
            }

            if (Core.Context.User.Any(u => u.Phone_number == NumberTB.Text))
            {
                MessageBox.Show("Пользователь с таким номером телефона уже существует.");
                return;
            }

            User currentUser = new User()
            {
                FIO = FioTB.Text,
                Login = LoginTB.Text,
                Password = GetHash(PasswordPB.Password),
                Age = AgeDP.DisplayDate,
                Phone_number = NumberTB.Text,
                RoleID = 2,
                SubID = 1,
                UserStatus = true,
                IsFine = false,
                FineSum = 0
            };

            Core.Context.User.Add(currentUser);
            Core.Context.SaveChanges();

            NavigationService.Navigate(new MainPage(currentUser));

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


        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
