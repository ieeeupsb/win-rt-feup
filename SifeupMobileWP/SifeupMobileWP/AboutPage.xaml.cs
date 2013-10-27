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
using Microsoft.Phone.Tasks;

namespace SifeupMobileWP
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void MailButton_Click(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                try {
                    new EmailComposeTask
                    {
                        To = "luis.fonseca@fe.up.pt",
                        Subject = "Support FEUP Mobile 0.3"
                    }.Show();
                }
                catch { }
            });
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new MarketplaceReviewTask().Show();
            }
            catch { }
        }
    }
}