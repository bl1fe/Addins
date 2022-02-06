using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using System.IO;
//using CsvHelper;
using System.Globalization;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Table;

namespace ExportElements
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class ExportRoomsTest : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            //COLLECTOR
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> _elementsDB = collector.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
            ICollection<ElementId> _elementsIds = collector.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElementIds();

            List<double> _pArea = new List<double>();
            List<double> _pVolume = new List<double>();
            List<string> _pRoomNumber = new List<string>();


            foreach (Element e in _elementsDB)
            {
                _pArea.Add(UnitUtils.ConvertFromInternalUnits(e.LookupParameter("Площадь").AsDouble(), DisplayUnitType.DUT_SQUARE_METERS));

            }

            foreach (Element e in _elementsDB)
            {
                _pVolume.Add(UnitUtils.ConvertFromInternalUnits(e.LookupParameter("Объем").AsDouble(), DisplayUnitType.DUT_CUBIC_METERS));

            }


            foreach (Element e in _elementsDB)
            {
                _pRoomNumber.Add(e.LookupParameter("Номер").AsString());

            }

            /////////////////////////////////////////////////////////////////////////////////EXCEL

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Sheet");

            //WHERE TO SAVE
            var file = new FileInfo(@"D:\Rooms.xlsx");


            using (ExcelRange Rng = ws.Cells["A1:E2"])
            {
                //Indirectly access ExcelTableCollection class
                ExcelTable table = ws.Tables.Add(Rng, "Table");


                //Set Columns position & name
                table.Columns[0].Name = "Наименование помещения";
                table.Columns[1].Name = "ID";
                table.Columns[2].Name = "Площадь";
                table.Columns[3].Name = "Объем";
                table.Columns[4].Name = "Номер комнаты";

            }

            int col = 1;
            for (int row = 2; row <= _elementsDB.Count; row++)
            {
                ws.Cells[row, col].Value = _elementsDB[row - 2].Name;
            }

            int col2 = 2;
            for (int row = 2; row <= _elementsIds.Count; row++)
            {
                ws.Cells[row, col2].Value = _elementsIds.ElementAt(row - 2);
            }

            int col3 = 3;
            for (int row = 2; row <= _pArea.Count; row++)
            {
                ws.Cells[row, col3].Value = _pArea[row - 2];
            }

            int col4 = 4;
            for (int row = 2; row <= _pVolume.Count; row++)
            {
                ws.Cells[row, col4].Value = _pVolume[row - 2];
            }

            int col5 = 5;
            for (int row = 2; row <= _pRoomNumber.Count; row++)
            {
                ws.Cells[row, col5].Value = _pRoomNumber[row - 2];
            }


            package.SaveAs(new FileInfo(file.ToString()));
            return Result.Succeeded;

        }
    }
}














