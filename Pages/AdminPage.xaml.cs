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
            var btn = sender as Button;
            var user = btn.DataContext as User;
            User curUser = Core.Context.User.FirstOrDefault(u => u.ID == user.ID);

            Window window = new RedactWindow(curUser);
            window.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportPage());
        }

        private bool exitConfirmed = false;
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitConfirmed)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить пользователя?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                var btn = sender as Button;
                var user = btn.DataContext as User;
                User delUser = Core.Context.User.FirstOrDefault(u => u.ID == user.ID);
                if (delUser != null)
                {
                    if (delUser.RoleID == 2)
                    {
                        var del = Core.Context.Review.Where(u => u.UserID == user.ID).ToList();
                        foreach (var item in del)
                        {
                            Core.Context.Review.Remove(item);
                        }
                        var del2 = Core.Context.Rentals.Where(u => u.UserID == user.ID).ToList();
                        foreach (var item in del2)
                        {
                            Core.Context.Rentals.Remove(item);
                        }

                        Core.Context.User.Remove(delUser);
                        Core.Context.SaveChanges();
                        LoadUser();
                    }
                    if (delUser.RoleID == 3)
                    {
                        var del = Core.Context.Maintenance.Where(u => u.UserID == user.ID).ToList();
                        foreach (var item in del)
                        {
                            Core.Context.Maintenance.Remove(item);
                        }

                        Core.Context.User.Remove(delUser);
                        Core.Context.SaveChanges();
                        LoadUser();
                    }
                }
            }
            else
            {
                return;
            }

        }
    }
}
