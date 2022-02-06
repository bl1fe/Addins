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

namespace Addins.ApartmentColor
{

    [Transaction(TransactionMode.Manual)]

    public class SetColor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;           

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Rooms);

            #region Lists

            IList<Element> apts = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            List<Element> apts_1bRoom = new List<Element>();

            List<KeyValuePair<Element, string>> apts_1bRoom_AptNumStr = new List<KeyValuePair<Element, string>>();

            List<KeyValuePair<Element, int>> apts_1bRoom_AptNumInt = new List<KeyValuePair<Element, int>>();


            #endregion

            

            #region 1-bedroom apartments
        

            foreach (Element el in apts)
            {

                if (el.LookupParameter("Amount of rooms").AsString() == "1к")
                {
                    apts_1bRoom.Add(el);
                }
            }

            foreach (Element el in apts_1bRoom)
            {
                apts_1bRoom_AptNumStr.Add(new KeyValuePair<Element, string>(el, el.LookupParameter("Room number").AsString()));
            }

            foreach (KeyValuePair<Element, string> kvp in apts_1bRoom_AptNumStr)
            {
                apts_1bRoom_AptNumInt.Add(new KeyValuePair<Element, int>(kvp.Key, Convert.ToInt32(kvp.Value)));
            }
            #endregion


            Tools tools = new Tools();
            var apts_1bRoom_AptNumInt_Close = tools.Get(apts_1bRoom_AptNumInt);
            
            using (Transaction t = new Transaction(doc, "Change Parameter"))
            {
                t.Start();

                foreach (KeyValuePair<Element, int> kvp in apts_1bRoom_AptNumInt_Close)

                {
                    string pAmountOfRooms = kvp.Key.LookupParameter("Amount of rooms").AsString();
                    kvp.Key.LookupParameter("Index").Set(pAmountOfRooms + ".Полутон");
                }

                

                t.Commit();
            }


            return Result.Succeeded;
        }
    }
}


