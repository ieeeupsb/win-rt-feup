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
    public class StaffResponse : ErrorResponse
    {
        public string codigo;
        public string nome;
        public string pagina_web;
        public string sigla;
        public string estado;
        public string email;
        public string email_alt;
        public string telefone;
        public string tele_alt;
        public string ext_tel;
        public Sala[] salas;
    }

    public class Sala
    {
        public string cod_edi;
        public int cod_sala;
    }
}
