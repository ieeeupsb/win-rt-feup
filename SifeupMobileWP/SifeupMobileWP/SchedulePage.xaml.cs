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
    public partial class SchedulePage : PhoneApplicationPage
    {
        string codigo;
        DateTime begin, end;
        ScheduleResponse schedule;
        ScheduleViewModel[] ScheduleModels = new ScheduleViewModel[5];
        ItemViewModel ivmNoClasses = new ItemViewModel() { LineOne="No classes" };

        public SchedulePage()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            DataContext = ScheduleModels[0];
            this.Loaded += new RoutedEventHandler(SchedulePage_Loaded);

            begin = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            end = begin.AddDays(4);
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("codigo", out codigo);
            LoadResources();
        }

        // Load data for the ViewModel Items
        private void SchedulePage_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ScheduleModels.Length; ++ i)
            {
                if (ScheduleModels[i] == null)
                    ScheduleModels[i] = new ScheduleViewModel();

                if (!ScheduleModels[i].IsDataLoaded)
                    ScheduleModels[i].LoadData();
            }
        }
        
        public void LoadResources() {
            string scheduleUrl = API.getScheduleUrl(codigo, begin.ToString("yyyyMMdd"), end.ToString("yyyyMMdd"));
            API.getReply(scheduleUrl, handleReply);

            int index = ((int)DateTime.Now.DayOfWeek) - 1;
            if (index == -1 || index == 5)
                return;

            pSchedule.SelectedIndex = index;
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
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(ScheduleResponse));
            schedule = dcjs.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json))) as ScheduleResponse;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                for (int i = 0; i < schedule.horario.Length; ++i)
                {
                    ScheduleModels[schedule.horario[i].dia - 2].Items.Add(
                        new ItemViewModel()
                        {
                            LineOne = schedule.horario[i].cad_sigla,
                            LineTwo = timeToString(schedule.horario[i].hora_inicio, schedule.horario[i].aula_duracao),
                            LineThree = ""
                        });
                }

                // Verificar se os dias tem aulas:
                foreach(ScheduleViewModel svm in ScheduleModels) {
                    if (svm.Items.Count == 0)
                        svm.Items.Add(ivmNoClasses);
                }

                monday.DataContext = ScheduleModels[0];
                tuesday.DataContext = ScheduleModels[1];
                wednesday.DataContext = ScheduleModels[2];
                thursday.DataContext = ScheduleModels[3];
                friday.DataContext = ScheduleModels[4];

                progressBar1.IsIndeterminate = false;
            });

        }

        private String timeToString(int time, double durationHours)
        {
            int hour, minutes;
            hour = time / 3600;
            minutes = (time - hour * 3600) / 60;
            string from = hour + ":" + (minutes < 10 ? "0" + minutes : "" + minutes);

            time += (int)(durationHours * 3600d);
            hour = time / 3600;
            minutes = (time - hour * 3600) / 60;
            string to = hour + ":" + (minutes < 10 ? "0" + minutes : "" + minutes);

            return from + " until " + to;
        }

    }
}