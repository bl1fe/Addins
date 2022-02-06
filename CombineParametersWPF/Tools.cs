using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace CombineParameters
{
    public static class Tools
    {
        public static Categories GetCategories(Document doc)
        {
            return doc.Settings.Categories;
        }
        public static List<string> GetParameters(Document doc, List<Autodesk.Revit.DB.Category> selectedCategories) 
        {
            List<ElementId> cats = new List<ElementId>();

            ElementId Category1 = Tools.GetCategories(doc).get_Item("Walls").Id;
            ElementId Category2 = Tools.GetCategories(doc).get_Item("Windows").Id;

            cats.Add(Category1);
            cats.Add(Category2);

            List<ElementId> elid = new List<ElementId>();

            ParameterFilterUtilities.GetFilterableParametersInCommon(doc, cats);
          
            return new List<string>(); 
        }
    }
}
