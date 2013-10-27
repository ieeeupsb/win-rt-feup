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
    public class StudentResponse
    {
        public string codigo;
        public string nome;
        public string curso_sigla;
        public string curso_nome;
        public int ano_lect_matricula;
        public string estado;
        public int ano_curricular;
        public string email;
        public string email_alternativo;

        public string erro;
        public string erro_msg;
    }
}
