using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;

namespace CombineParameters
{
    [Transaction(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]

    public class CombineParameters : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements)

        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            Categories categories = Tools.GetCategories(doc);
            
            SortedList<string, Category> myCategories = new SortedList<string, Category>();

            foreach (Category c in categories)
            {
                myCategories.Add(c.Name, c);
            }

            UserWindow UserWindow = new UserWindow(myCategories);
            UserWindow.Show();

            return Result.Succeeded;
        }
    }
}

