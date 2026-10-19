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
using System.Windows.Shapes;

namespace ScooterRental.Pages
{
    public partial class RedactWindow : Window
    {
        private User currentUser;

        public RedactWindow(User user)
        {
            InitializeComponent();

            if (user == null)
            {
                MessageBox.Show("Пользователь не передан!");
                Close();
                return;
            }

            currentUser = Core.Context.User
                .FirstOrDefault(u => u.ID == user.ID);

            if (currentUser == null)
            {
                MessageBox.Show("Пользователь не найден в базе!");
                Close();
                return;
            }

            LoadUser();
        }

        private void LoadUser()
        {
            DataContext = currentUser;

            PhoneTB.Text = currentUser.Phone_number;

            RoleCB.ItemsSource = Core.Context.Roles.ToList();
            RoleCB.SelectedValue = currentUser.RoleID;

            SubCB.ItemsSource = Core.Context.Subscription.ToList();
            SubCB.SelectedValue = currentUser.SubID;

            IsFineCB.IsChecked = currentUser.IsFine;
            FineSumTB.Text = currentUser.FineSum.ToString();

            BlockRb.IsChecked = currentUser.UserStatus;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            currentUser.FIO = FIOTB.Text;
            currentUser.Login = LoginTB.Text;
            currentUser.Phone_number = PhoneTB.Text;

            currentUser.UserStatus = BlockRb.IsChecked != true;
            currentUser.IsFine = IsFineCB.IsChecked == true;

            if (RoleCB.SelectedValue != null)
                currentUser.RoleID = (int)RoleCB.SelectedValue;

            if (SubCB.SelectedValue != null)
                currentUser.SubID = (int)SubCB.SelectedValue;

            Core.Context.SaveChanges();

            if (decimal.TryParse(FineSumTB.Text, out decimal fineSum))
                currentUser.FineSum = fineSum;
            else
                currentUser.FineSum = 0;

            Core.Context.SaveChanges();

            MessageBox.Show("Данные пользователя сохранены!");
            Close();
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


