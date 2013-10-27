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
    public partial class PersonalPage : PhoneApplicationPage
    {
        public PersonalPage()
        {
            InitializeComponent();
        
            // Set the data context of the listbox control to the sample data
            DataContext = App.PersonalAreaViewModel;
            this.Loaded += new RoutedEventHandler(PersonalPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void PersonalPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.PersonalAreaViewModel.IsDataLoaded)
            {
                App.PersonalAreaViewModel.LoadData();
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbItems.SelectedIndex == -1)
                return;

            switch (lbItems.SelectedIndex)
            {
                case 0:
                    NavigationService.Navigate(new Uri("/ProfilePage.xaml?codigo=" + API.auth.codigo, UriKind.Relative));
                    break;
                case 1:
                    NavigationService.Navigate(new Uri("/SchedulePage.xaml?codigo=" + API.auth.codigo, UriKind.Relative));
                    break;
                case 2:
                    NavigationService.Navigate(new Uri("/ExamsPage.xaml", UriKind.Relative));
                    break;
                case 3:
                    NavigationService.Navigate(new Uri("/SubjectsPage.xaml", UriKind.Relative));
                    break;
                case 4:
                    NavigationService.Navigate(new Uri("/CanteenPage.xaml", UriKind.Relative));
                    break;
            }

            // Reset selected index to -1 (no selection)
            lbItems.SelectedIndex = -1;
        }
    }
}