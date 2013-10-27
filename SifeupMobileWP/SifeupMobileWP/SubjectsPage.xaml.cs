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
using SifeupMobileWP.JSONObjects;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace SifeupMobileWP
{
    public partial class SubjectsPage : PhoneApplicationPage
    {
        public SubjectsPage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            DataContext = App.SubjectsViewModel;
            this.Loaded += new RoutedEventHandler(SubjectsPage_Loaded);
            LoadResources();
        }

        private void SubjectsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.SubjectsViewModel.IsDataLoaded)
            {
                App.SubjectsViewModel.LoadData();
            }
        }

        public void LoadResources()
        {
            if (API.auth == null)
                return;

            API.getReply(API.getSubjectsUrl(API.auth.codigo, "" + (API.secondYearOfSchoolYear() - 1)), handleReply);
        }

        public void handleReply(string json)
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

            DataContractJsonSerializer dcjsSubject = new DataContractJsonSerializer(typeof(SubjectsResponse));
            SubjectsResponse subjects = dcjsSubject.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as SubjectsResponse;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                App.SubjectsViewModel.Items.Clear();
                foreach (Subject s in subjects.inscricoes)
                    App.SubjectsViewModel.Items.Add(
                        new ItemViewModel() {
                            extra = new string[] { s.dis_codigo, "" + s.ano_curricular, s.periodo },
                            LineOne = s.name == "" ? s.nome : s.name,
                            LineTwo = s.dis_codigo,
                            LineThree = "Year " + s.ano_curricular + " - Semester " + s.periodo.TrimStart('S')});
          
                progressBar1.IsIndeterminate = false;
            });
        }

        private void lbItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbItems.SelectedIndex == -1)
                return;

            string[] extra = (string[]) ((ItemViewModel) lbItems.SelectedItem).extra;
            if (extra == null)
                return;

            string query = "?codigo=" + extra[0] + "&year=" + extra[1] + "&per=" + extra[2];
            NavigationService.Navigate(new Uri("/SubjectPage.xaml" + query, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            lbItems.SelectedIndex = -1;
        }
    }
}