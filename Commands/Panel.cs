using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Commands
{
    class Panel : IExternalApplication
    {


        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication app)
        {
            //Create Ribbon Element
            app.CreateRibbonTab("Commands");
            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("Высота стен", "Изменить высоту стен", path, "Commands.WallHeight");
            RibbonPanel panel = app.CreateRibbonPanel("Commands", "Стены");
            PushButton pushButton = panel.AddItem(button) as PushButton;

            Uri imagePath = new Uri(@"");
            BitmapImage image = new BitmapImage(imagePath);

            pushButton.LargeImage = image;

            return Result.Succeeded;
        }

    }

}
