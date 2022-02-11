using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using System.IO;

namespace Commands
{

    [Transaction(TransactionMode.Manual)]

    public class SetParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_TelephoneDevices);

            IList<Element> telephoneDevices = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            using (Transaction t = new Transaction(doc, "Change Parameter"))
            {
                t.Start();

                foreach (Element el in telephoneDevices)

                {
                    double parameter1 = el.LookupParameter("Смещение").AsDouble();
                    el.LookupParameter("BS_Высота").Set(parameter1);
                }

                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}


