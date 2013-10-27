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
    public partial class PrintQuotaPage : PhoneApplicationPage
    {
        public PrintQuotaPage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            LoadResources();
        }

        public void LoadResources()
        {
            if (API.auth == null)
                return;

            API.getReply(API.getPrintingUrl(API.auth.codigo), handleReply);
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
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(PrintingQuotaResponse));
            PrintingQuotaResponse quota = dcjs.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as PrintingQuotaResponse;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                tbPrinting.Text = "Printing Quota: € " + quota.saldo;
                if(quota.saldo >= .03d)
                    tbMaxAllowed.Text = "You can print up to " + ((int) (quota.saldo / 0.03d)) + " black A4 pages.";
                else
                    tbMaxAllowed.Text = "You can't print anything.";

                progressBar1.IsIndeterminate = false;
            });
        }

        private void bGen_Click(object sender, RoutedEventArgs e)
        {
            int value;
            try
            {
                value = Int32.Parse(tbValue.Text);
                if (value < 1)
                    throw new Exception();
            }
            catch
            {
                if(MessageBox.Show("Invalid amount inserted.") != MessageBoxResult.OK)
                    return;

                tbValue.Text = "5";
                tbValue.SelectAll();
                tbValue.Focus();
                return;
            }
            if (progressBar1.IsIndeterminate)
                return;

            progressBar1.IsIndeterminate = true;
            API.getReply(API.getPrintingRefUrl(tbValue.Text), handleRefReply);
        }

        public void handleRefReply(string json)
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

            try
            {
                json = json.Replace("Data Limite\" : ", "dataLimite\" : \"").Insert(json.Length - 2, "\"");
                DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(PrintingRefResponse));
                PrintingRefResponse pref = dcjs.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as PrintingRefResponse;

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    progressBar1.IsIndeterminate = false;
                    MessageBox.Show("Referencia: " + pref.Referencia
                                + "\nEntidade: " + pref.Entidade
                                + "\nValor: € " + pref.Valor
                                + "\nData Limite: " + pref.dataLimite);
                });
            }
            catch
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    progressBar1.IsIndeterminate = false;
                    MessageBox.Show("Invalid answer from server. Try again later.");
                });
            }
        }
    }
}