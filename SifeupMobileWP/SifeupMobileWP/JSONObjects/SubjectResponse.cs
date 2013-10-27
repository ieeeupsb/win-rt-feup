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
    public class SubjectResponse
    {
        public string codigo;
        public string nome;
        public string name;
        public string sigla;
        public string areas;
        public string ano_lectvo;
        public string periodo;
        public string pagina_web;
        public int unidade_codigo;
        public string unidade_nome;
        public string objectivos;
        public string conteudo;
        public Referencia[] bibliografia;
        public string metodologia;
        public string cond_frequencia;
        public string for_avaliacao;
        public string forma_melhoria;
        public string observacoes;
        public Class[] ds;
        public Software[] software;
    }

    public class Software
    {
        public string nome;
        public string descricao;
    }


    public class Class
    {
        public string tipo;
        public string tipo_descricao;
        public int num_turmas;
        public int num_horas;
        public Teacher[] docentes;
    }

    public class Teacher
    {
        public int doc_codigo;
        public string nome;
        public double horas;
    }

    public class Referencia
    {
        public string tipo;
        public string tipo_descr;
        public string titulo;
        public string autores;
        public string editor;
        public string link;
        public int ano;
        public string isbn;
    }
}
