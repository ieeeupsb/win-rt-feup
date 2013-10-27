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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace SifeupMobileWP
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        string codigo;
        StudentResponse student;
        StudentProfileViewModel StudentProfileModel = new StudentProfileViewModel();
        public ProfilePage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            DataContext = StudentProfileModel;
            this.Loaded += new RoutedEventHandler(ProfilePage_Loaded);
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("codigo", out codigo);
            PageTitle.Text = codigo == API.auth.codigo ? "your profile"
                                                       : "profile";
            LoadResources();
        }

        // Load data for the ViewModel Items
        private void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!StudentProfileModel.IsDataLoaded)
            {
                StudentProfileModel.LoadData();
                StudentProfileModel.Items[0].LineTwo = codigo;
            }
        }

        public void LoadResources() {
            if (codigo == null)
                return;

            if (StudentProfileModel.IsDataLoaded)
                return;

            API.getReply(API.getStudentUrl(codigo), handleReply);
            iStudent.Source = new BitmapImage(new Uri(API.getPersonPicUrl(codigo), UriKind.Absolute));
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
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(StudentResponse));
            student = dcjs.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as StudentResponse;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.tStudentName.Text = student.nome;

                StudentProfileModel.Items[0].LineTwo = student.codigo;
                StudentProfileModel.Items[1].LineTwo = student.email;
                StudentProfileModel.Items[2].LineTwo = student.email_alternativo;
                StudentProfileModel.Items[3].LineTwo = student.curso_nome;
                StudentProfileModel.Items[4].LineTwo = student.estado;
                StudentProfileModel.Items[5].LineTwo = student.ano_curricular.ToString();

                TiltEffect.SetIsTiltEnabled(lbItems.ItemContainerGenerator.ContainerFromIndex(1), true);
                TiltEffect.SetIsTiltEnabled(lbItems.ItemContainerGenerator.ContainerFromIndex(2), true);

                if (student.email_alternativo == null)
                    StudentProfileModel.Items.RemoveAt(2);

                this.lbItems.SelectionChanged += new SelectionChangedEventHandler(sendEmailHandler);


                progressBar1.IsIndeterminate = false;
            });
        }

        private void sendEmailHandler(object sender, SelectionChangedEventArgs e)
        {
            if (lbItems.SelectedIndex == 1)
                sendEmail(((ItemViewModel)lbItems.SelectedItem).LineTwo);
            
            if(lbItems.SelectedIndex != 2)
                return;

            if(((ItemViewModel)lbItems.SelectedItem).LineOne != "Alternative Email")
                return;

            sendEmail(((ItemViewModel)lbItems.SelectedItem).LineTwo);
        }

        private void sendEmail(string to)
        {
            if (to == null)
                return;
            try
            {
                new EmailComposeTask() { To = to }.Show();
            }
            catch {}
        }
    }
}