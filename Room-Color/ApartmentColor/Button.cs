using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;


namespace Addins.ApartmentColor
{
    class Button : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication app)
        {            
            app.CreateRibbonTab("Plugins");
            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("Apartment Color", "Apartment Color", path, "Addins.ApartmentColor.SetColor");
            RibbonPanel panel = app.CreateRibbonPanel("Plugins", "Tools for automation");
            PushButton pushButton = panel.AddItem(button) as PushButton;           
            Uri imagePath = new Uri(@"https://i.ibb.co/xYhqjVG/ICON-ICX-icon-32.png");
        
            BitmapImage image = new BitmapImage(imagePath);

            pushButton.LargeImage = image;


            return Result.Succeeded;
        }

    }
}
