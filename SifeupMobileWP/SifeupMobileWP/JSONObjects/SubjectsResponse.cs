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
    public class SubjectsResponse
    {
        public Subject[] inscricoes;
    }

    public class Subject
    {
        public string dis_codigo;
        public int ano_curricular;
        public string nome;
        public string name;
        public string periodo;
    }
}
