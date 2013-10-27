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
    public partial class ExamsPage : PhoneApplicationPage
    {

        public ExamsPage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            DataContext = App.ExamsViewModel;
            this.Loaded += new RoutedEventHandler(ExamsPage_Loaded);
            LoadResources();
        }

        private void ExamsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ExamsViewModel.IsDataLoaded)
            {
                App.ExamsViewModel.LoadData();
            }
        }

        public void LoadResources()
        {
            if (API.auth == null)
                return;

            API.getReply(API.getExamsUrl(API.auth.codigo), handleReply);
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

            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(ExamsResponse));
            ExamsResponse exams = dcjs.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as ExamsResponse;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                App.ExamsViewModel.Items.Clear();
                progressBar1.IsIndeterminate = false;
                if (exams.exames.Length == 0)
                {
                    App.ExamsViewModel.Items.Add(
                        new ItemViewModel() { LineOne = "No exams" });
                    return;
                }
                foreach(Exam e in exams.exames)
                    App.ExamsViewModel.Items.Add(
                        new ItemViewModel() {
                            LineOne = e.uc_nome,
                            LineTwo = e.dia.Trim() + ", " + e.data + " @ " + e.hora_inicio + " to " + e.hora_fim,
                            LineThree = "" });
            });
        }
    }
}