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
    public class SearchStudentResponse : ErrorResponse
    {
        public int total;
        public int primeiro;
        public int tam_pagina;
        public Aluno[] alunos;
    }

    public class Aluno
    {
        public string codigo;
        public string nome;
        public string cur_sigla;
        public string cur_nome;
        public string cur_name;
    }
}
