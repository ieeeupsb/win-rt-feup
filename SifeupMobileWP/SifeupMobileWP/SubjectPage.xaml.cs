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
using SifeupMobileWP.JSONObjects;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace SifeupMobileWP
{
    public partial class SubjectPage : PhoneApplicationPage
    {
        string codigo, year, per;

        public SubjectPage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("codigo", out codigo);
            NavigationContext.QueryString.TryGetValue("year", out year);
            NavigationContext.QueryString.TryGetValue("per", out per);
            LoadResources();
        }
        
        public void LoadResources() {
            API.getReply(API.getSubjectDescUrl(codigo, ""+(API.secondYearOfSchoolYear()-1) + '/' + API.secondYearOfSchoolYear(), per),
                            handleReply);
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
            DataContractJsonSerializer dcjsSubject = new DataContractJsonSerializer(typeof(SubjectResponse));
            SubjectResponse subject = dcjsSubject.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as SubjectResponse;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                tbObjectives.Text = subject.objectivos;
                tbContent.Text = subject.conteudo;
                TeachersViewModel tvm = new TeachersViewModel();
                BiblioViewModel bvm = new BiblioViewModel();
                GenericViewModel svm = new GenericViewModel();

                List<string> teachers = new List<string>();
                for (int i = 0; i < subject.ds.Length; ++i)
                {
                    for (int j = 0; j < subject.ds[i].docentes.Length; ++j)
                    {
                        if (teachers.Contains(subject.ds[i].docentes[j].nome))
                            continue;

                        teachers.Add(subject.ds[i].docentes[j].nome);
                        tvm.Items.Add(new ItemViewModel()
                        {
                            extra = subject.ds[i].docentes[j].doc_codigo.ToString(),
                            LineOne = subject.ds[i].docentes[j].nome,
                        });
                    }
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        for (int i = 0; i < lbTeachers.Items.Count; ++i)
                            TiltEffect.SetIsTiltEnabled(lbTeachers.ItemContainerGenerator.ContainerFromIndex(i), true);
                    }
                    catch { } // Not really important.
                });
                lbTeachers.DataContext = tvm;

                if(subject.bibliografia != null)
                {
                    for (int i = 0; i < subject.bibliografia.Length; ++i)
                    {
                        bvm.Items.Add(new ItemViewModel()
                        {
                            LineOne = subject.bibliografia[i].tipo_descr,
                        
                            LineTwo = (subject.bibliografia[i].autores != null ?  subject.bibliografia[i].autores + "\n" : "")
                                    + (subject.bibliografia[i].titulo != null ? subject.bibliografia[i].titulo + "\n" : "")
                                    + (subject.bibliografia[i].isbn != null ? "ISBN: " + subject.bibliografia[i].isbn : ""),
                        });
                    }
                    lbBibliography.DataContext = bvm;
                }
                if (subject.software != null)
                {
                    foreach (Software s in subject.software)
                    {
                        if (s.nome == "" && s.descricao != "")
                        {
                            s.nome = s.descricao;
                            s.descricao = "";
                        }
                        svm.Items.Add(new ItemViewModel()
                        {
                            LineOne = s.nome,
                            LineTwo = s.descricao,
                        });
                    }
                }
                else
                {
                    svm.Items.Add(new ItemViewModel() { LineOne = "No data" });
                }
                lbSoftware.DataContext = svm;

                tbMetodology.Text = subject.metodologia == null ? "No data" : subject.metodologia;
                tbExam.Text = subject.cond_frequencia == null ? "No data" : subject.cond_frequencia;
                tbFinalGrade.Text = subject.for_avaliacao == null ? "No data" : subject.for_avaliacao;
                lbTeachers.SelectionChanged += new SelectionChangedEventHandler(lbTeachers_SelectionChanged);

                progressBar1.IsIndeterminate = false;
            });
        }

        void lbTeachers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbTeachers.SelectedIndex == -1)
                return;

            string codigo = ((ItemViewModel)lbTeachers.SelectedItem).extra as string;
            if (codigo == null)
                return;

            string query = "?codigo=" + codigo;
            NavigationService.Navigate(new Uri("/ProfileStaffPage.xaml" + query, UriKind.Relative));

            lbTeachers.SelectedIndex = -1;
        }

    }
}