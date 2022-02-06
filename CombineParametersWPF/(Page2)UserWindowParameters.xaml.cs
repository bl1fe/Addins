using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;



namespace CombineParameters
{
    public partial class UserWindowParameters : Window
    {
        //List<Parameter> _parameters; 
        public UserWindowParameters()
        //public UserWindowParameters(List<Parameter> parameters)
        {
            InitializeComponent();
            //_parameters = parameters;

            // selectedParameters.ItemsSource = Tools.GetParameters();
        }
        
        private void AddParameter(object sender, RoutedEventArgs e)
        {
            // this button delete the selected parameter from "available parameter" list, and add the parameter to the "selected parameter" list. On the last position


        }

        private void DeleteParameter(object sender, RoutedEventArgs e)
        {
            // this button delete the selected parameter from "selected parameters" list, and add the parameter to the "available parameters" list. On the last position
        }

        private void UpButton(object sender, RoutedEventArgs e)
        {
            // this button modify the order of the listed parameters.
            // if use select a parameter and click this button, the position is modified. 

        }

        private void DownButton(object sender, RoutedEventArgs e)
        {
            // this button modify the order of the listed parameters.
            // if use select a parameter and click this button, the position is modified. 

        }








    }
}
