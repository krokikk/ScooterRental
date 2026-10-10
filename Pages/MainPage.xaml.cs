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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private User currentUser; 
        public MainPage(User user)
        {
            InitializeComponent();
            currentUser = user;

        }

        // переход на страницу профиля
        private void ProfileClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage(currentUser));
        }

        // переключение темы
        private bool isDayTheme = true;
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            string path;

            if (isDayTheme == true)
            {
                path = "Styles/StyleNight.xaml";
            }
            else
            {
                path = "Styles/StyleDay.xaml";
            }

            var uri = new Uri(path, UriKind.Relative);

            var resourceDict =
                Application.LoadComponent(uri) as ResourceDictionary;

            if (resourceDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Clear();

                Application.Current.Resources.MergedDictionaries.Add(resourceDict);

                if (isDayTheme == true)
                {
                    isDayTheme = false;
                }
                else
                {
                    isDayTheme = true;
                }
            }
        }

        // кнопки самокатов
        private void ChooseClick1(object sender, RoutedEventArgs e)
        {
            var scooter1 = Core.Context.Vechicles.FirstOrDefault(u => u.ID == 1);
            RentWindow window = new RentWindow(scooter1);
            window.Show();
        }

        private void ChooseClick2(object sender, RoutedEventArgs e)
        {
            var bike1 = Core.Context.Vechicles.FirstOrDefault(u => u.ID == 2);
            RentWindow window = new RentWindow(bike1);
            window.Show();
        }

        private void ChooseClick3(object sender, RoutedEventArgs e)
        {
            var scooter2 = Core.Context.Vechicles.FirstOrDefault(u => u.ID == 3);
            RentWindow window = new RentWindow(scooter2);
            window.Show();
        }

        private void ChooseClick4(object sender, RoutedEventArgs e)
        {
            var scooter3 = Core.Context.Vechicles.FirstOrDefault(u => u.ID == 4);
            RentWindow window = new RentWindow(scooter3);
            window.Show();
        }
        private void ChooseClick5(object sender, RoutedEventArgs e)
        {
            var bike2 = Core.Context.Vechicles.FirstOrDefault(u => u.ID == 6);
            RentWindow window = new RentWindow(bike2);
            window.Show();
        }
        private void ChooseClick6(object sender, RoutedEventArgs e)
        {
            var scooter4 = Core.Context.Vechicles.FirstOrDefault(u => u.ID == 5);
            RentWindow window = new RentWindow(scooter4);
            window.Show();
        }

        // выбор типа транспорта
        private void ChooseCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Core.Context.Vechicles_Models.Any(u => u.Model_name == "Электросамокат"))
            {
                Scooter1.Visibility = Visibility.Visible;
                Scooter2.Visibility = Visibility.Visible;
                Scooter3.Visibility = Visibility.Visible;
                Scooter4.Visibility = Visibility.Visible;
                Bike1.Visibility = Visibility.Hidden;
                Bike2.Visibility = Visibility.Hidden;
            }
            if (Core.Context.Vechicles_Models.Any(u => u.Model_name == "Электровелосипед"))
            {
                Scooter1.Visibility = Visibility.Hidden;
                Scooter2.Visibility = Visibility.Hidden;
                Scooter3.Visibility = Visibility.Hidden;
                Scooter4.Visibility = Visibility.Hidden;
                Bike1.Visibility = Visibility.Visible;
                Bike2.Visibility = Visibility.Hidden;
            }
        }
    }
}
