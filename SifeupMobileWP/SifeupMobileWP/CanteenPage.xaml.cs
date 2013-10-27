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
    public partial class CanteenPage : PhoneApplicationPage
    {
        readonly GenericViewModel[] GenericModels = new GenericViewModel[3];

        public CanteenPage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            DataContext = GenericModels[0];
            this.Loaded += new RoutedEventHandler(CanteenPage_Loaded);

            LoadResources();
        }

        // Load data for the ViewModel Items
        private void CanteenPage_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GenericModels.Length; ++i)
            {
                if (GenericModels[i] == null)
                    GenericModels[i] = new GenericViewModel();

                if (!GenericModels[i].IsDataLoaded)
                    GenericModels[i].LoadData();
            }
        }
        
        public void LoadResources() {
            API.getReply(API.getCanteensUrl(), handleReply);
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
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(CanteenResponse));
            CanteenResponse canteen = dcjs.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as CanteenResponse;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                for (int i = 0; i < canteen.cantinas.Length - 1; ++i)
                {
                    foreach (CanteenMenu cm in canteen.cantinas[i].ementas)
                    {
                        string lastDate = "";
                        foreach (Meal m in cm.pratos)
                        {
                            if (m.descricao == null)
                                continue;

                            if (m.descricao.Length < 5) // Discard incorrect values
                                continue;

                            if (lastDate != cm.data)
                            {
                                lastDate = cm.data;
                                DateTime dt;
                                if(DateTime.TryParseExact(cm.data, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
                                {
                                    if (DateTime.Today.Equals(dt))
                                        lastDate = "Today";
                                    else if(DateTime.Today.AddDays(1).Equals(dt))
                                        lastDate = "Tomorrow";
                                }
                                GenericModels[i].Items.Add(
                                    new ItemViewModel() {
                                        LineOne = lastDate
                                    });
                                lastDate = cm.data;
                            }

                            GenericModels[i].Items.Add(
                                new ItemViewModel()
                                {
                                    LineOne = m.tipo_descr,
                                    LineTwo = m.descricao,
                                    LineThree = ""
                                });
                        }
                    }
                }

                lbCanteen.ItemsSource = GenericModels[0].Items;
                //lbCanteen.DataContext = GenericModels[0];
                lbGrill.DataContext = GenericModels[1];
                lbRestaurant.DataContext = GenericModels[2];

                progressBar1.IsIndeterminate = false;
            });
        }
    }
}