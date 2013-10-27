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
using SifeupMobileWP.ViewModels;

namespace SifeupMobileWP
{
    public class LunchMenuTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Date
        {
            get;
            set;
        }

        public DataTemplate Lunch
        {
            get;
            set;
        }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ItemViewModel ivm = item as ItemViewModel;

            if (ivm.LineTwo == null)
                return Date;

            return Lunch;
        }
    }
}
