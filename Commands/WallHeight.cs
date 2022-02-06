using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;

namespace Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    class WallHeight : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            Document doc = commandData.Application.ActiveUIDocument.Document;            

            Form1 form = new Form1(doc);
            form.Text = "Команды";
            form.ShowDialog();

            TaskDialog.Show("Уведомление", "Высота стен изменена на " + form.GetMMValueWallHeight("Высота стены") + " м.");

            return Result.Succeeded;
        }
    }
}
