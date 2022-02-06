using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using OfficeOpenXml;
using OfficeOpenXml.Table;


namespace ReminderParameter
{
    class RP : IExternalDBApplication
    {

        public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            application.DocumentSynchronizingWithCentral -= SyncReminder;
            return ExternalDBApplicationResult.Succeeded;
        }

        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            application.DocumentSynchronizingWithCentral += new EventHandler<DocumentSynchronizingWithCentralEventArgs>(SyncReminder);
            return ExternalDBApplicationResult.Succeeded;
        }

        public void SyncReminder(object sender, DocumentSynchronizingWithCentralEventArgs args)
        {

            Document doc = args.Document;

            WorksetTable worksetTable = doc.GetWorksetTable();
            WorksetId currentWorksetId = worksetTable.GetActiveWorksetId();
            Workset currentWorkset = worksetTable.GetWorkset(currentWorksetId);
            string currentWorksetName = currentWorkset.Name;

            Categories categories = doc.Settings.Categories;

            SortedList<string, Category> categoriesSortedList = new SortedList<string, Category>();

            foreach (Category c in categories)

            {
                categoriesSortedList.Add(c.Name, c);
            }

            List<Category> prjcats = new List<Category>();


            foreach (var item in categoriesSortedList)
            {
                prjcats.Add(item.Value);
            }

            List<ElementId> prjcatsId = new List<ElementId>();

            foreach (var item in prjcats)
            {
                prjcatsId.Add(item.Id);
            }

            ElementMulticategoryFilter filter = new ElementMulticategoryFilter(prjcatsId);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> allElements = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            List<string> _Names = new List<string>();
            List<Autodesk.Revit.DB.ElementId> _Ids = new List<ElementId>();
            List<string> _Categories = new List<string>();

            foreach (Element el in allElements  )
            {
                if (el.LookupParameter("Марка") == null)
                {
                    continue;
                }

                else if (el.LookupParameter("Марка").AsString() == "" || el.LookupParameter("Марка").AsString() == null)
                {

                    _Names.Add(el.Name);
                    _Ids.Add(el.Id);
                    _Categories.Add(el.Category.Name);

                }
            }

            if (_Names.Count > 0)
            {

                string file = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Параметр " + "(" + args.Document.Title + ")" + ".xlsx";
                FileInfo fileFI = new FileInfo(file);
                ExcelPackage exp = new ExcelPackage();


                ExcelWorksheet ws = exp.Workbook.Worksheets.Add("Sheet");

                ws.Cells[1, 1].Value = "Name";
                ws.Cells[1, 2].Value = "ID";
                ws.Cells[1, 3].Value = "Category";


                int col = 1;
                for (int row = 2; row - 1 <= _Names.Count; row++)
                {
                    ws.Cells[row, col].Value = _Names[row - 2];
                }


                int col2 = 2;
                for (int row = 2; row - 1 <= _Ids.Count; row++)
                {
                    ws.Cells[row, col2].Value = _Ids.ElementAt(row - 2);
                }

                int col3 = 3;
                for (int row = 2; row - 1 <= _Categories.Count; row++)
                {
                    ws.Cells[row, col3].Value = _Categories.ElementAt(row - 2);
                }

                exp.SaveAs(fileFI);
                TaskDialog.Show("Уведомление",
                    "Текущий рабочий набор: " + currentWorksetName + System.Environment.NewLine +
                     _Names.Count.ToString() + " элемента(ов) с незаполненным параметром. "
                    + System.Environment.NewLine + "Файл находится на рабочем столе.");

            }

            if (_Names.Count == 0)
            {
                TaskDialog.Show("Уведомление", System.Environment.NewLine + "Элементов с незаполненным параметром нет.");
            }


        }

    }
}
