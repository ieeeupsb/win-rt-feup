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
using System.Xml.Linq;
using System.IO;

namespace SifeupMobileWP
{
    public partial class NewsPage : PhoneApplicationPage
    {
        public NewsPage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            DataContext = App.NewsViewModel;
            this.Loaded += new RoutedEventHandler(NewsPage_Loaded);
            LoadResources();
        }

        private void NewsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.NewsViewModel.IsDataLoaded)
            {
                App.NewsViewModel.LoadData();
            }
        }

        public void LoadResources()
        {
            API.getReply(API.getNewsUrl(), handleReply);
        }

        public void handleReply(string xml)
        {
            if (xml == null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    progressBar1.IsIndeterminate = false;
                    MessageBox.Show("Unable to communicate with server.");
                });
                return;
            }
            XDocument xmlDocument = XDocument.Load(new StringReader(xml));
            var items = xmlDocument.Descendants("item");
            var rss = xmlDocument.Descendants("rss");


            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                App.NewsViewModel.Items.Clear();
                foreach (var item in items)
                {
                    string title = item.Element("title").Value;
                    string link = item.Element("link").Value;
                    string date = item.Element("pubDate").Value;
                    string desc = item.Element("description").Value;
                    App.NewsViewModel.Items.Add(new ItemViewModel() {
                        extra = new string[] { title, link, date, desc },
                        LineOne = title,
                        LineTwo = date,
                    });
                }
                progressBar1.IsIndeterminate = false;
            });
        }

        private void lbItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbItems.SelectedIndex == -1)
                return;

            string[] extra = (string[])((ItemViewModel)lbItems.SelectedItem).extra;
            if (extra == null)
                return;

            if (extra.Length < 4)
                return;

            string query = "?title=" + extra[0] + "&link=" + extra[1] + "&date=" + extra[2] + "&desc=" + extra[3];
            NavigationService.Navigate(new Uri("/NewPage.xaml" + query, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            lbItems.SelectedIndex = -1;
        }
    }
}