using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.JSON
{
    public class CanteenFEUP
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string horario { get; set; }
        public CanteenMenu[] ementas{ get; set; }
    }

    public class CanteenMenu
    {
        public string estado { get; set; }
        public string data { get; set; }
        public Meal[] pratos{ get; set; }
    }

    public class Meal
    {
        public string estado { get; set; }
        public string descricao{ get; set; }
        public int tipo { get; set; }
        public string tipo_descr { get; set; }
    }
}
