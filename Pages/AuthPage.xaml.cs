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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private int failedAttempts = 0;
        public AuthPage()
        {
            InitializeComponent();
            /*var uri = new Uri("Styles/StyleDay.xaml", UriKind.Relative);
            ResourceDictionary dict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);*/
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTB.Text) || string.IsNullOrEmpty(PasswordPB.Password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            var currentUser = Core.Context.User.FirstOrDefault(u => u.Login == LoginTB.Text);

            if (currentUser != null)
            {
                if (currentUser.Password == GetHash(PasswordPB.Password))
                {
                    if (currentUser.RoleID == 1)
                    {
                        NavigationService.Navigate(new AdminPage());
                    }
                    else if(currentUser.RoleID == 3)
                    {
                        NavigationService.Navigate(new WorkerPage());
                    }
                    else
                    {
                        NavigationService.Navigate(new MainPage(currentUser));
                    }
                }

                else
                {
                    PasswordPB.Clear();
                    MessageBox.Show("Неверный пароль.");
                    failedAttempts++;

                    if (failedAttempts >= 3)
                    {
                        if (captcha.Visibility != Visibility.Visible)
                        {
                            CaptchaSwitch();
                        }
                        CaptchaChange();
                    }
                    return;
                }

            }
            else
            {
                MessageBox.Show("Пользователя с таким логином нет.");
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }

        public void CaptchaSwitch()
        {
            switch (captcha.Visibility)
            {
                case Visibility.Visible:
                    LoginTB.Clear();
                    PasswordPB.Clear();

                    captcha.Visibility = Visibility.Collapsed;
                    captchaInput.Visibility = Visibility.Collapsed;
                    labelCaptcha.Visibility = Visibility.Collapsed;
                    submitCaptcha.Visibility = Visibility.Collapsed;
                    BorderCapcha.Visibility = Visibility.Collapsed;

                    LoginText.Visibility = Visibility.Visible;
                    LoginTB.Visibility = Visibility.Visible;
                    PasswordText.Visibility = Visibility.Visible;
                    PasswordPB.Visibility = Visibility.Visible;
                    BorderAuth.Visibility = Visibility.Visible;

                    AuthBt.Visibility = Visibility.Visible;
                    RegBt.Visibility = Visibility.Visible;
                    RegText.Visibility = Visibility.Visible;
                    return;

                case Visibility.Collapsed:

                    captcha.Visibility = Visibility.Visible;
                    captchaInput.Visibility = Visibility.Visible;
                    labelCaptcha.Visibility = Visibility.Visible;
                    submitCaptcha.Visibility = Visibility.Visible;
                    BorderCapcha.Visibility = Visibility.Visible;


                    LoginText.Visibility = Visibility.Collapsed;
                    LoginTB.Visibility = Visibility.Collapsed;
                    PasswordText.Visibility = Visibility.Collapsed;
                    PasswordPB.Visibility = Visibility.Collapsed;
                    BorderAuth.Visibility = Visibility.Collapsed;


                    AuthBt.Visibility = Visibility.Collapsed;
                    RegBt.Visibility = Visibility.Collapsed;
                    RegText.Visibility = Visibility.Collapsed;
                    return;
            }
        }

        public void CaptchaChange()
        {
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            captcha.Text = pwd;
        }

        private void submitCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (captchaInput.Text != captcha.Text)
            {
                MessageBox.Show("Неверно введена капча", "Ошибка");
                captchaInput.Clear();
                CaptchaChange();

            }
            else
            {
                MessageBox.Show("Капча введена успешно", "Успех");
                CaptchaSwitch();
                failedAttempts = 0;
            }
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
            e.Command == ApplicationCommands.Cut ||
            e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
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
    }
}
