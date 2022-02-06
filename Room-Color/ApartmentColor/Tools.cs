using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Addins.ApartmentColor
{
    class Tools
    {        
            public IEnumerable<KeyValuePair<Element, int>> Get(IEnumerable<KeyValuePair<Element, int>> listIEnumerable)
            {
                var list = listIEnumerable.ToList();
                var lstOrder = list.Select(s => s.Value).Distinct().OrderBy(q => q).ToList();
                var finishLstOrder = new List<int>();

                for (var i = 0; i < lstOrder.Count; i++)
                {
                    if (lstOrder.Count - 1 <= i) continue;
                    if (lstOrder[i + 1] - lstOrder[i] == 1)
                    {
                        finishLstOrder.Add(lstOrder[i]);
                        finishLstOrder.Add(lstOrder[i + 1]);
                    }
                }

                var result = list.Join(finishLstOrder.Distinct(), l => l.Value, f => f, (l, f) => l);

                return result;
            }   
    }
}



    
