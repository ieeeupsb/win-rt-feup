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
    public class CanteenResponse
    {
        public Canteen[] cantinas;
    }

    public class Canteen
    {
        public int codigo;
        public string descricao;
        public string horario;
        public CanteenMenu[] ementas;
    }

    public class CanteenMenu
    {
        public string estado;
        public string data;
        public Meal[] pratos;
    }

    public class Meal
    {
        public string estado;
        public string descricao;
        public int tipo;
        public string tipo_descr;
    }
}
