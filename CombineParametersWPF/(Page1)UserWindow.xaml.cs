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
   
    public partial class UserWindow : Window
    {
        SortedList<string, Category> _categories;

        public UserWindow(SortedList<string, Category> elements)
        {
            InitializeComponent();

            _categories = elements;

            foreach (KeyValuePair<string, Category> cat in elements)
            {
                CheckBox cb = new CheckBox();
                cb.Content = cat.Key;
                stackPanel.Children.Add(cb);
            }
        }

        public void Next(object sender, RoutedEventArgs e)
        {
           
            UserWindowParameters UWP = new UserWindowParameters();
            UWP.Show();
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {

            this.Close();

        }

    }
}

