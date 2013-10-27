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

namespace SifeupMobileWP
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public string[] saveCredentialsSettings = { "remember username and password", "remember username", "don't remember anything"};
        public SettingsPage()
        {
            InitializeComponent();

            this.lpSaveCredentials.ItemsSource = saveCredentialsSettings;
            reset();
        }

        private void reset()
        {
            this.lpSaveCredentials.SelectedIndex = ((API.SettingLogin) API.userSettings["settingLogin"]).GetHashCode();
            if ((bool)API.userSettings["settingInstantSearch"])
            {
                this.cbInstantSearch.IsChecked = true;
                cbInstantSearch_Checked(null, null);
            }
            else
            {
                this.cbInstantSearch.IsChecked = false;
                cbInstantSearch_Unchecked(null, null);
            }
        }

        private void save(object sender, EventArgs e)
        {
            API.userSettings["settingInstantSearch"] = cbInstantSearch.IsChecked ?? true;

            switch (lpSaveCredentials.SelectedIndex)
            {
                case 0:
                    API.userSettings["settingLogin"] = API.SettingLogin.RememberEverything;
                    break;
                case 1:
                    API.userSettings["settingLogin"] = API.SettingLogin.RememberUsername;
                    API.userSettings.Remove("password");
                    break;
                case 2:
                    API.userSettings["settingLogin"] = API.SettingLogin.RememberNothing;
                    API.userSettings.Remove("password");
                    API.userSettings.Remove("username");
                    break;
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            reset();
        }

        private void cbInstantSearch_Checked(object sender, RoutedEventArgs e)
        {
            cbInstantSearch.Content = "On";
        }

        private void cbInstantSearch_Unchecked(object sender, RoutedEventArgs e)
        {
            cbInstantSearch.Content = "Off";
        }
    }
}