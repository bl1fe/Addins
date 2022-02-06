using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Creation;

namespace CubeA
{
    class Tools
    {
        public PlanarFace findFace(Extrusion pBox, XYZ normal)
        {
            Options op = new Options();
            op.ComputeReferences = true;
            GeometryElement geomElem = pBox.get_Geometry(op);

            foreach (GeometryObject geomObj in geomElem)
            {
                if (geomObj is Solid)
                {
                    Solid pSolid = geomObj as Solid;
                    FaceArray faces = pSolid.Faces;
                    foreach (Face pFace in faces)
                    {
                        PlanarFace pPlanarFace = pFace as PlanarFace;
                        if ((pPlanarFace != null) && pPlanarFace.FaceNormal.IsAlmostEqualTo(normal))
                        {
                            return pPlanarFace;
                        }
                    }
                }
            }
            return null;
        }

       
    }
}
