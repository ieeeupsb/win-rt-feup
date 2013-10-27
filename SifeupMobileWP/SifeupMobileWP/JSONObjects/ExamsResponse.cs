using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SifeupMobileWP.JSONObjects
{
    public class ExamsResponse
    {
        public Exam[] exames;
    }

    public class Exam
    {
        public string tipo;
        public string uc;
        public string uc_nome;
        public string dia;
        public string data;
        public string hora_inicio;
        public string hora_fim;
        public string salas;
    }
}
