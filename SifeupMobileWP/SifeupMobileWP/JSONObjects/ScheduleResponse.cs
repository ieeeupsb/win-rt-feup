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
using System.Collections.Generic;

namespace SifeupMobileWP.JSONObjects
{
    public class ScheduleResponse
    {
        public Dia[] horario;
    }

    public class Dia
    {
        public int dia;
        public int hora_inicio;
        public string cad_sigla;
        public string tipo;
        public double aula_duracao;
        public string doc_sigla;
        public string cad_codigo;
        public string turma_sigla;
        public string sala_cod;
        public string edi_cod;
        public string bloco;
        public string doc_codigo; // Sometimes it is ''
        public string doc_nome;
        public string periodo;
    }
}
