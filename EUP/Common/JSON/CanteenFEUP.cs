using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.JSON
{
    public class CanteenFEUP
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
