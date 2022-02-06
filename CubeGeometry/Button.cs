using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace CubeA
{
    class Button : IExternalApplication
    {


        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication app)
        {
            //Create Ribbon Element
            app.CreateRibbonTab("Cube");
            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("Cube", "Create Cube", path, "CubeA.Cube");
            RibbonPanel panel = app.CreateRibbonPanel("Cube", "Geometry");
            PushButton pushButton = panel.AddItem(button) as PushButton;
            
            Uri imagePath = new Uri(@"https://i.ibb.co/0FGhyN2/cube-32.png");
            BitmapImage image = new BitmapImage(imagePath);
            pushButton.LargeImage = image;

            return Result.Succeeded;
        }

    }

}
