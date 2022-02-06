using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombineParametersWinForm
{


    public partial class Form1 : System.Windows.Forms.Form
   
    {
        Document revitDoc { get; set; }

            public Form1(Document doc)
            {
                InitializeComponent();

                this.revitDoc = doc;

            # region populating combobox
            List<string> categoriesComboBox0 = new List<string>

            {
                "Оборудование",
                "Спринклеры",
                "Трубопроводная арматура"
                    
            };

            foreach (string name in categoriesComboBox0)

            {
                comboBox0.Items.Insert(0, name);
            }


                List<string> stringParameters = new List<string>
            {
                "GP_Производитель",
                "GP_Модель",
                "GP_Артикул",
                "GP_Масса",
                "GP_Высота"
               
            };
                
                foreach (string parameterName in stringParameters)
                {
                    comboBox1.Items.Insert(0, parameterName);
                    comboBox2.Items.Insert(0, parameterName);
                    comboBox3.Items.Insert(0, parameterName);
                 
                }

            }
            # endregion


        private void button1_Click(object sender, EventArgs e)
        {

            List<BuiltInCategory> revitCategoriesComboBox0 = new List<BuiltInCategory>
            {
                BuiltInCategory.OST_PipeAccessory,
                BuiltInCategory.OST_Sprinklers,
                BuiltInCategory.OST_MechanicalEquipment
            };

            FilteredElementCollector collector = new FilteredElementCollector(revitDoc);
            
            int iCat = comboBox0.SelectedIndex;

            ElementCategoryFilter filter = new ElementCategoryFilter(revitCategoriesComboBox0[iCat]);
          
            IList<Element> elementsByCategory = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            foreach (Element el in elementsByCategory)
            {
           
                Parameter parameter1 = el.LookupParameter(comboBox1.Text);
                Parameter parameter2 = el.LookupParameter(comboBox2.Text);
                Parameter parameter3 = el.LookupParameter(comboBox3.Text);  

                List<string> parameterValues = new List<string>();


                if (parameter1 != null)
                {

                    if (parameter1.StorageType.ToString() == "Double" && parameter1.ToString() != "" && parameter1.ToString() != null)

                    {
                        string parameterValue1 = parameter1.AsValueString();
                        parameterValues.Add(parameterValue1);
                    }

                    if (parameter1.StorageType.ToString() == "String" && parameter1.ToString() != "" && parameter1.ToString() != null)

                    {
                        string parameterValue1 = parameter1.AsString();
                        parameterValues.Add(parameterValue1);
                    }


                }


                if (parameter2 != null)
                {

                    if (parameter2.StorageType.ToString() == "Double" && parameter2.ToString() != "" && parameter2.ToString() != null)

                    {
                        string parameterValue2 = parameter2.AsValueString();
                        parameterValues.Add(parameterValue2);
                    }

                    if (parameter2.StorageType.ToString() == "String" && parameter2.ToString() != "" && parameter2.ToString() != null)

                    {
                        string parameterValue2 = parameter2.AsString();
                        parameterValues.Add(parameterValue2);
                    }


                }


                if (parameter3 != null)
                {
                   
                    if (parameter3.StorageType.ToString() == "Double" && parameter3.ToString() != "" && parameter3.ToString() != null)

                    {
                        string parameterValue3 = parameter3.AsValueString();
                        parameterValues.Add(parameterValue3);
                    }

                    if (parameter3.StorageType.ToString() == "String" && parameter3.ToString() != "" && parameter3.ToString() != null)

                    {
                        string parameterValue3 = parameter3.AsString();
                        parameterValues.Add(parameterValue3);
                    }

                }

            if (parameterValues.Count > 0)
                {

                    string newValue = String.Join(", ", parameterValues);

                    using (Transaction t = new Transaction(revitDoc, "Set Parameter name"))
                    {
                        t.Start();

                        el.LookupParameter("GP_Описание").Set(newValue);

                        t.Commit();
                    }

                }

            }

        }
                
    }
}
