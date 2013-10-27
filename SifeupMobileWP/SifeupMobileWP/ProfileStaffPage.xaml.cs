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
    public partial class ProfileStaffPage : PhoneApplicationPage
    {
        string codigo;
        StaffResponse staff;
        bool isDataLoaded = false;

        //StudentProfileViewModel StudentProfileModel = new StudentProfileViewModel();
        public ProfileStaffPage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            //DataContext = StudentProfileModel;
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
            //if (!StudentProfileModel.IsDataLoaded)
            //{
            //    StudentProfileModel.LoadData();
            //    StudentProfileModel.Items[0].LineTwo = codigo;
            //}
        }

        public void LoadResources() {
            if (codigo == null)
                return;

            if (isDataLoaded)
                return;

            API.getReply(API.getEmployeeUrl(codigo), handleReply);
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
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(StaffResponse));
            staff = dcjs.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as StaffResponse;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                isDataLoaded = true;
                this.tStudentName.Text = staff.nome;

                lbItems.Items.Add(new ItemViewModel() { LineOne = "Acronym", LineTwo = staff.sigla });
                lbItems.Items.Add(new ItemViewModel() { LineOne = "Code", LineTwo = staff.codigo });
                if (!string.IsNullOrEmpty(staff.email))
                    lbItems.Items.Add(new ItemViewModel() { LineOne = "Email", LineTwo = staff.email });
                if (!string.IsNullOrEmpty(staff.email_alt))
                    lbItems.Items.Add(new ItemViewModel() { LineOne = "Alternative Email", LineTwo = staff.email_alt });
                if (!string.IsNullOrEmpty(staff.pagina_web))
                    lbItems.Items.Add(new ItemViewModel() { LineOne = "Webpage", LineTwo = staff.pagina_web });
                if (!string.IsNullOrEmpty(staff.telefone))
                    lbItems.Items.Add(new ItemViewModel() { LineOne = "Contact", LineTwo = staff.telefone });
                if (staff.salas.Length > 0)
                    lbItems.Items.Add(new ItemViewModel() { LineOne = "Room",
                                                            LineTwo = staff.salas[0].cod_edi + " " + staff.salas[0].cod_sala});

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        for (int i = 2; i < lbItems.Items.Count; ++i)
                        {
                            if ((lbItems.Items[i] as ItemViewModel).LineOne == "Room")
                                continue;

                            TiltEffect.SetIsTiltEnabled(lbItems.ItemContainerGenerator.ContainerFromIndex(i), true);
                        }
                    }
                    catch { } // Not really important.
                });

                this.lbItems.SelectionChanged += new SelectionChangedEventHandler(sendEmailHandler);

                progressBar1.IsIndeterminate = false;
            });
        }

        private void sendEmailHandler(object sender, SelectionChangedEventArgs e)
        {
            ItemViewModel ivm = lbItems.SelectedItem as ItemViewModel;
            switch (ivm.LineOne)
            {
                case "Email":
                case "Alternative Email":
                    sendEmail(ivm.LineTwo);
                    break;
                case "Webpage":
                    try
                    {
                        new WebBrowserTask { Uri = new Uri(ivm.LineTwo) }.Show();
                    }
                    catch
                    {
                        MessageBox.Show("Unable to start the web browser.");
                    }
                    break;
                case "Contact":
                    try
                    {
                        string[] nameSplit = this.tStudentName.Text.Split(' ');
                        string name;
                        if (nameSplit.Length < 2)
                        {
                            name = ivm.LineTwo;
                        }
                        else
                        {
                            name = nameSplit[0] + " " + nameSplit[nameSplit.Length - 1];
                        }

                        new PhoneCallTask
                        {
                            DisplayName = name,
                            PhoneNumber = ivm.LineTwo.Replace(" ", "")
                        }.Show();
                    }
                    catch
                    {
                        MessageBox.Show("Unable to start the web browser.");
                    }
                    break;
                default:
                    return;
            }
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