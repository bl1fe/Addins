using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Commands
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        
        Document revitDoc { get; set; }
        public double value = 0;
        
        public Form1(Document doc)
        {
            InitializeComponent();

            this.revitDoc = doc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            value = UnitUtils.ConvertToInternalUnits(Convert.ToDouble(textBox1.Text), DisplayUnitType.DUT_MILLIMETERS);
            FilteredElementCollector collector = new FilteredElementCollector(revitDoc);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);

            IList<Element> AllWalls = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();            

            foreach (Element el in AllWalls)

            {
                using (Transaction t = new Transaction(revitDoc, "Change Parameter"))
                {
                    t.Start();
                  
                    el.LookupParameter("Неприсоединенная высота").Set(value);

                    t.Commit();
                }
            }            

        }               

        public string GetMMValueWallHeight(string s)

        {
         s = UnitUtils.ConvertFromInternalUnits(value, DisplayUnitType.DUT_METERS).ToString();
         return s;   
        }

        
    }
}
