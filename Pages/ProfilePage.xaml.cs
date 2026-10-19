using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using ScooterRental.Classes;


namespace ScooterRental.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private User currentUser;

        public ProfilePage(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadUser();

        }

        private void LoadUser()
        {
            var user = Core.Context.User.FirstOrDefault(u => u.Login == currentUser.Login);
            DataContext = user;

            TripsLB.ItemsSource = Core.Context.Rentals.Include("Vechicles").Where(u => u.ID == currentUser.ID).ToList();
        }

        private void ButtonSub_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFine_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonLeave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show( "Вы действительно хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
               NavigationService.Navigate(new RegPage());
            }
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

       
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            Themes.ChangeTheme();
        }

        private void PassButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new PasswordWindow(currentUser);
            window.Show();
        }
    }
}
