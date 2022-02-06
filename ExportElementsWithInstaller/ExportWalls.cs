using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Globalization;

namespace ExportElements
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class ExportWalls : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> _elementsDB = collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElements();
            ICollection<ElementId> _elementsIds = collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElementIds();


            List<double> _pHeight = new List<double>();


            foreach (Element e in _elementsDB)
            {
                if (e.LookupParameter("Неприсоединенная высота") != null)
                {
                    _pHeight.Add(UnitUtils.ConvertFromInternalUnits(e.LookupParameter("Неприсоединенная высота").AsDouble(), DisplayUnitType.DUT_MILLIMETERS));
                }
                else
                {
                    continue;
                }
            }

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Sheet");
            var file = new FileInfo(@"D:\Walls.xlsx");

            ws.Cells[1,1].Value = "Наименование";
            ws.Cells[1,2].Value = "ID";
            ws.Cells[1,3].Value = "Высота";

            int col = 1;
            for (int row = 2; row - 1 <= _elementsDB.Count; row++)
            {
                ws.Cells[row, col].Value = _elementsDB[row - 2].Name;
            }

            int col2 = 2;
            for (int row = 2; row - 1 <= _elementsIds.Count; row++)
            {
                ws.Cells[row, col2].Value = _elementsIds.ElementAt(row - 2);
            }

            int col3 = 3;
            for (int row = 2; row - 1 <= _elementsIds.Count; row++)
            {
                ws.Cells[row, col3].Value = _pHeight[row - 2];
            }

            package.SaveAs(new FileInfo(file.ToString()));
            TaskDialog.Show("Уведомление", "Экспортировано " + _elementsIds.Count + " элементов") ;
            return Result.Succeeded;

        }
    }
}
