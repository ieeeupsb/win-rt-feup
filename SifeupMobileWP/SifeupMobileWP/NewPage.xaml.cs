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
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;

namespace SifeupMobileWP
{
    public partial class NewPage : PhoneApplicationPage
    {
        string title, link, date, desc;
        Uri infoUri;
        public NewPage()
        {
            InitializeComponent();
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("title", out title);
            NavigationContext.QueryString.TryGetValue("link", out link);
            NavigationContext.QueryString.TryGetValue("date", out date);
            NavigationContext.QueryString.TryGetValue("desc", out desc);
            LoadResources();
        }

        private void LoadResources() {
            this.tbTitle.Text = title;
            this.tbDate.Text = date;
            this.tbNewsDesc.Text = desc.TrimStart('\n');
            try
            {
                this.infoUri = new Uri(link);
            }
            catch { }
        }

        public void moreInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                new WebBrowserTask { Uri = infoUri }.Show();
            } catch {
                MessageBox.Show("Unable to start the web browser.");
            }
        }
    }
}