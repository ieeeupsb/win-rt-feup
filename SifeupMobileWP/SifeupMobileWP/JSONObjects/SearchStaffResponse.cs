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
    public class SearchStaffResponse : ErrorResponse
    {
        public int total;
        public int primeiro;
        public int tam_pagina;
        public Staff[] pessoal;
    }

    public class Staff
    {
        public int codigo;
        public string nome;
    }
}
