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
using System.Runtime.Serialization.Json;
using SifeupMobileWP.JSONObjects;
using System.IO;
using System.Text;
using System.Windows.Navigation;
using System.Threading;

namespace SifeupMobileWP
{
    public partial class SearchPage : PhoneApplicationPage
    {
        private string lastStudentSearch;
        private string lastStaffSearch;

        public SearchPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void tbSearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key != Key.Enter && e.Key != Key.Space) || (e.Key == Key.Space && !(bool) API.userSettings["settingInstantSearch"]))
                return;

            string search = tbSearchInput.Text.Trim();
            if (search.Length < 5 || search == lastStudentSearch)
                return;

            pbSearch.IsIndeterminate = true;

            lastStudentSearch = search;
            API.getReply(API.getStudentsSearchUrl(search, 1), handleSearchResult);
        }

        public void handleSearchResult(string json)
        {
            if (json == null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    pbSearch.IsIndeterminate = false;
                    MessageBox.Show("Unable to communicate with server.");
                });
                return;
            }
            DataContractJsonSerializer dcjsSearch = new DataContractJsonSerializer(typeof(SearchStudentResponse));
            SearchStudentResponse search = dcjsSearch.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as SearchStudentResponse;
            if (search.erro != null)
            {
                API.handleError(search);
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                pbSearch.IsIndeterminate = false;
                lbSearchResults.Items.Clear();
                if (search.alunos.Length == 0)
                {
                    lbSearchResults.Items.Add(new ItemViewModel() { LineOne = "Search yielded no results." });
                    //TiltEffect.SetIsTiltEnabled(lbSearchResults.ItemContainerGenerator.ContainerFromIndex(0), false);
                    return;
                }
                foreach (Aluno aluno in search.alunos)
                {
                    lbSearchResults.Items.Add(new ItemViewModel() { LineOne = aluno.nome, LineTwo = aluno.cur_nome, LineThree = aluno.codigo });
                }
            });
        }

        private void lbSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSearchResults.SelectedIndex == -1)
                return;

            string nome = ((ItemViewModel)lbSearchResults.SelectedItem).LineOne;
            string curso = ((ItemViewModel)lbSearchResults.SelectedItem).LineTwo;
            string codigo = ((ItemViewModel)lbSearchResults.SelectedItem).LineThree;
            if (nome == null || codigo == null)
                return;

            if (curso == null)
                curso = "";

            string query = "?codigo=" + codigo + "&nome=" + nome + "&curso=" + curso;
            NavigationService.Navigate(new Uri("/ProfilePage.xaml" + query, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            lbSearchResults.SelectedIndex = -1;
        }

        private void tbSearchStaffInput_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key != Key.Enter && e.Key != Key.Space) || (e.Key == Key.Space && !(bool)API.userSettings["settingInstantSearch"]))
                return;

            string search = tbSearchStaffInput.Text.Trim();
            if (search.Length < 5 || search == lastStaffSearch)
                return;

            pbSearch.IsIndeterminate = true;

            lastStaffSearch = search;
            API.getReply("http://paginas.fe.up.pt/~ei10139/feupextensions/pessoal_pesquisa.php?pv_nome=" + search, handleStaffSearchResult);
        }

        public void handleStaffSearchResult(string json)
        {
            if (json == null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    pbSearch.IsIndeterminate = false;
                    MessageBox.Show("Unable to communicate with server.");
                });
                return;
            }
            DataContractJsonSerializer dcjsSearch = new DataContractJsonSerializer(typeof(SearchStaffResponse));
            SearchStaffResponse search = dcjsSearch.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as SearchStaffResponse;
            if (search.erro != null)
            {
                API.handleError(search);
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                pbSearch.IsIndeterminate = false;
                lbSearchStaffResults.Items.Clear();
                if (search.pessoal.Length == 0)
                {
                    lbSearchStaffResults.Items.Add(new ItemViewModel() { LineOne = "Search yielded no results." });
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    //{ // maybe set the rest to true...
                    //    try
                    //    {
                    //        Thread.Sleep(50);
                    //        TiltEffect.SetIsTiltEnabled(lbSearchStaffResults.ItemContainerGenerator.ContainerFromIndex(0), false);
                    //    }
                    //    catch { } // Not really important.
                    //});
                    return;
                }
                else if (search.pessoal.Length == 1)
                {
                    NavigationService.Navigate(new Uri("/ProfileStaffPage.xaml?codigo=" + search.pessoal[0].codigo, UriKind.Relative));
                    return;
                }
                foreach (Staff staff in search.pessoal)
                {
                    lbSearchStaffResults.Items.Add(new ItemViewModel() { LineOne = staff.nome, LineThree = ""+staff.codigo });
                }
            });
        }

        private void lbSearchInputResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSearchStaffResults.SelectedIndex == -1)
                return;

            string codigo = ((ItemViewModel)lbSearchStaffResults.SelectedItem).LineThree;
            if (codigo == null)
                return;

            string query = "?codigo=" + codigo;
            NavigationService.Navigate(new Uri("/ProfileStaffPage.xaml" + query, UriKind.Relative));

            lbSearchResults.SelectedIndex = -1;
        }
    }
}