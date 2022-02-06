using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace CombineParameters 
{
    class ExternalApplication : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication app)
        {
            app.CreateRibbonTab("Combine Project");
            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("QWERTY", "Click to combine parameters", path, "CombineParameters.CombineParameters" );
            RibbonPanel panel = app.CreateRibbonPanel("Combining", "Combining parameters");
            PushButton pushButton = panel.AddItem(button) as PushButton;
      
            return Result.Succeeded;
        }
    }
}
