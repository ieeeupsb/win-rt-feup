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
using SifeupMobileWP.JSONObjects;

namespace SifeupMobileWP
{
    public class AuthenticateResponse : ErrorResponse
    {
        public Boolean authenticated;

        public string tipo;
        public string codigo;
    }
}
