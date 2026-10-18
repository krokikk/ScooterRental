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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public User currentUser;
        public AdminPage(User user)
        {
            InitializeComponent();
            currentUser = user;

            LoadUser();
            LoadReviews();
        }

        private void LoadUser()
        {
            var admin = Core.Context.User.FirstOrDefault(u => u.Login == currentUser.Login);
            DataContext = admin;

            var user = Core.Context.User.ToList();
            LBUser.ItemsSource = user;
        }

        private void LoadReviews()
        {
            var review = Core.Context.Review.ToList();
            LBReview.ItemsSource = review;
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new RegPage());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new RedactWindow();
            window.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportPage());
        }
    }
}
