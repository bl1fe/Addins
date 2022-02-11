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
            PushButtonData button1 = new PushButtonData("Wall Height", "Set Wall Height", path, "Commands.WallHeight");
            PushButtonData button2 = new PushButtonData("Parameter", "Change Parameter", path, "Commands.SetParameter");

            RibbonPanel panel = app.CreateRibbonPanel("Commands", "Tools for automation");
            PushButton pushButton1 = panel.AddItem(button1) as PushButton;
            PushButton pushButton2 = panel.AddItem(button2) as PushButton;

            Uri imagePath1 = new Uri(@"https://i.ibb.co/WK0LBg0/wall-icon-32.png");
            Uri imagePath2 = new Uri(@"https://i.ibb.co/xYhqjVG/ICON-ICX-icon-32.png");
            BitmapImage image1 = new BitmapImage(imagePath1);
            BitmapImage image2 = new BitmapImage(imagePath2);

            pushButton1.LargeImage = image1;
            pushButton2.LargeImage = image2;

            return Result.Succeeded;
        }

    }

}
