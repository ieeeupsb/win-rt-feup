using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Text;
using System.IO;
using System.Windows.Navigation;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using Microsoft.Phone.Tasks;
using System.Threading;

namespace SifeupMobileWP
{
    public partial class LoginPage : PhoneApplicationPage
    {
        bool appBarVisibile = true;

        public LoginPage()
        {
            InitializeComponent();

            if(!API.userSettings.Contains("username"))
                return;

            this.textBox1.Text = (string) API.userSettings["username"];
            if (!API.userSettings.Contains("password"))
                return;

            this.passwordBox1.Password = (string) API.userSettings["password"];
            button1_Click(null, null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (progressBar1.IsIndeterminate)
                return;

            this.passwordBox1.Password = "";
            CheckPasswordWatermark();
            API.userSettings.Remove("password");
            if (API.SettingLogin.RememberNothing == ((API.SettingLogin)API.userSettings["settingLogin"]))
            {
                this.tbAccountMessage.Opacity = 0;
                this.textBox1.Text = "";
            }
            else
                this.tbAccountMessage.Opacity = 1;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                if (MessageBox.Show("Please type your username.") == MessageBoxResult.OK)
                   this.textBox1.Focus();

                return;
            }
            if (this.passwordBox1.Password == "")
            {
                if (MessageBox.Show("Please type your password.") == MessageBoxResult.OK)
                {
                    if(passwordBox1.Visibility == Visibility.Visible)
                        this.passwordBox1.Focus();
                    else 
                        this.passwordTextBox.Focus();
                }

                return;
            }
            if (progressBar1.IsIndeterminate)
                return;

            progressBar1.IsIndeterminate = true;
            API.auth = null;
            App.resetViewModels();
            API.performLogin(this.textBox1.Text, this.passwordBox1.Password, doLogin);
        }

        private void UsernameGotFocus(object sender, RoutedEventArgs e)
        {
            appBarVisibile = false;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ApplicationBar.IsVisible = appBarVisibile;
            });
        }

        private void UsernameLostFocus(object sender, RoutedEventArgs e)
        {
            appBarVisibile = true;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ApplicationBar.IsVisible = appBarVisibile;
            });
        }

        private void PasswordLostFocus(object sender, RoutedEventArgs e)
        {
            appBarVisibile = true;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ApplicationBar.IsVisible = appBarVisibile;
            });
            CheckPasswordWatermark();
        }

        public void CheckPasswordWatermark()
        {
            if (passwordTextBox.Visibility == Visibility.Visible && string.IsNullOrEmpty(passwordTextBox.Text)
                || passwordBox1.Visibility == Visibility.Visible && string.IsNullOrEmpty(passwordBox1.Password))
            {
                PasswordWatermark.Opacity = 1;
                cbShowPassword_Checked(null, null);
            }
        }

        private void PasswordGotFocus(object sender, RoutedEventArgs e)
        {
            appBarVisibile = false;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ApplicationBar.IsVisible = appBarVisibile;
            });
            if (PasswordWatermark.Opacity == 0)
                return;

            PasswordWatermark.Opacity = 0;
            cbShowPassword_Checked(null, null);
        }

        private void doLogin(string json)
        {
            if (json == null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    progressBar1.IsIndeterminate = false;
                    MessageBox.Show("Unable to communicate with server.");
                });
                return;
            }
            // Process data
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(AuthenticateResponse));
            API.auth = dcjs.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as AuthenticateResponse;
            if (API.auth.erro != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    progressBar1.IsIndeterminate = false;
                    MessageBox.Show("Your credentials are invalid.");
                    this.passwordBox1.Password = "";
                    CheckPasswordWatermark();
                    API.userSettings.Remove("username");
                    API.userSettings.Remove("password");
                });
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                progressBar1.IsIndeterminate = false;
                NavigationService.Navigate(new Uri("/MainMenu.xaml", UriKind.Relative));

                API.userSettings.Remove("password");
                API.userSettings.Remove("username");

                switch ((API.SettingLogin) API.userSettings["settingLogin"])
                {
                    case API.SettingLogin.RememberEverything:
                        API.userSettings.Add("password", this.passwordBox1.Password);
                        goto case API.SettingLogin.RememberUsername;
                    case API.SettingLogin.RememberUsername:
                        API.userSettings.Add("username", this.textBox1.Text);
                        break;
                }
            });
        }

        private void cbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            switch(cbShowPassword.IsChecked) {
                case true:
                    passwordBox1.Visibility = Visibility.Collapsed;
                    passwordTextBox.Visibility = Visibility.Visible;
                    passwordTextBox.Opacity = PasswordWatermark.Opacity == 0 ? 1 : 0;
                    break;
                case false:
                    passwordBox1.Visibility = Visibility.Visible;
                    passwordTextBox.Visibility = Visibility.Collapsed;
                    passwordBox1.Opacity = PasswordWatermark.Opacity == 0 ? 1 : 0;
                    break;
                default:
                    return;
            }
        }


        public void terms(object sender, RoutedEventArgs e)
        {
            try
            {
                new WebBrowserTask { Uri = new Uri("http://sigarra.up.pt/feup_uk/web_base.gera_pagina?P_pagina=21181") }.Show();
            }
            catch
            {
                MessageBox.Show("Unable to start the web browser.");
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}