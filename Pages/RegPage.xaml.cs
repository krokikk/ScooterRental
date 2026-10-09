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
            if (string.IsNullOrEmpty(LoginTB.Text) || string.IsNullOrEmpty(PasswordPB.Password) || string.IsNullOrEmpty(Password2PB.Password) || string.IsNullOrEmpty(AgeDP.Text) ||
                string.IsNullOrEmpty(FioTB.Text) || string.IsNullOrEmpty(NumberTB.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            if (PasswordPB.Password != Password2PB.Password)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

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
                Password = PasswordPB.Password,
                Age = AgeDP.DisplayDate,
                Phone_number = NumberTB.Text,
            };

            Core.Context.User.Add(currentUser);
            Core.Context.SaveChanges();

            NavigationService.Navigate(new MainPage(currentUser));

        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
