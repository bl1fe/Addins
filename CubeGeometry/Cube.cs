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
    [Transaction(TransactionMode.Manual)]
    public class Cube : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData,ref string message,ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;
            View rlView = FindElement(doc, typeof(View), "Опорный уровень") as View;

            double h = 250;
            double hm = UnitUtils.ConvertToInternalUnits(h, DisplayUnitType.DUT_MILLIMETERS);
            double extrusionHeight = 500;
            double extrusionHeightM = UnitUtils.ConvertToInternalUnits(extrusionHeight, DisplayUnitType.DUT_MILLIMETERS);

            XYZ ptr = new XYZ(hm, hm, 0);
            XYZ pbr = new XYZ(hm, -hm, 0);
            XYZ pbl = new XYZ(-hm, -hm, 0);
            XYZ ptl = new XYZ(-hm, hm, 0);

            XYZ p1zt = new XYZ(-hm, hm, extrusionHeightM);
            XYZ p2zt = new XYZ(hm, hm, extrusionHeightM);

            XYZ p1zb = new XYZ(-hm, hm, 0);
            XYZ p2zb = new XYZ(hm, hm, 0);

            Line lr = Line.CreateBound(ptr, pbr);
            Line lb = Line.CreateBound(pbr, pbl);
            Line ll = Line.CreateBound(pbl, ptl);
            Line lt = Line.CreateBound(ptl, ptr);

            CurveArray curveArray = new CurveArray();
            curveArray.Append(lr);
            curveArray.Append(lb);
            curveArray.Append(ll);
            curveArray.Append(lt);

            CurveArrArray curveArrArray = new CurveArrArray();
            curveArrArray.Append(curveArray);

            
            XYZ cutVec = new XYZ(0, 0, 1);
            XYZ cutVecZ = new XYZ(0, 1, 0);

            Element FindElement(Autodesk.Revit.DB.Document docum, Type targetType, string targetName)
            {
            return new FilteredElementCollector(doc).OfClass(targetType).First<Element>(e => e.Name.Equals(targetName));
            }

            using (Transaction tx = new Transaction(doc, "Geometry"))
            {
                tx.Start();

                SketchPlane sketch = FindElement(doc, typeof(SketchPlane), "Опорный уровень") as SketchPlane;
                
                //Extrusion element
                Extrusion box = doc.FamilyCreate.NewExtrusion(true, curveArrArray, sketch, extrusionHeightM);
                //Creating reference planes
                ReferencePlane refpr = doc.FamilyCreate.NewReferencePlane(ptr, pbr, cutVec, rlView);
                ReferencePlane refpl = doc.FamilyCreate.NewReferencePlane(ptl, pbl, cutVec, rlView);
                ReferencePlane refpt = doc.FamilyCreate.NewReferencePlane(ptl, ptr, cutVec, rlView);
                ReferencePlane refpb = doc.FamilyCreate.NewReferencePlane(pbl, pbr, cutVec, rlView);
                ReferencePlane refp1z = doc.FamilyCreate.NewReferencePlane(p1zt, p2zt, cutVecZ, rlView);
                ReferencePlane refp2z = doc.FamilyCreate.NewReferencePlane(p1zb, p2zb, cutVecZ, rlView);
                refp2z.Pinned = true;
                doc.Regenerate();                               
                //Lines for dimensions
                Line lineDimTop = Line.CreateBound(new XYZ(ptl.X, ptl.Y + 1, ptl.Z), new XYZ(ptr.X, ptr.Y + 1, pbl.Z));
                ReferenceArray refat = new ReferenceArray();
                refat.Append(refpr.GetReference());
                refat.Append(refpl.GetReference());

                Line lineDimLeft = Line.CreateBound(new XYZ(pbl.X - 1, pbl.Y, pbl.Z), new XYZ(ptl.X - 1, ptl.Y, ptl.Z));
                ReferenceArray refal = new ReferenceArray();
                refal.Append(refpt.GetReference());
                refal.Append(refpb.GetReference());          

                Line lineDimV = Line.CreateBound(new XYZ(p1zt.X - 0.5, p1zt.Y, p1zt.Z), new XYZ(p1zb.X-0.5, p1zb.Y, p1zb.Z));
                ReferenceArray refaZ = new ReferenceArray();
                refaZ.Append(refp1z.GetReference());
                refaZ.Append(refp2z.GetReference()); 

                
                //Dimensions
                var dimTop = doc.FamilyCreate.NewLinearDimension(rlView, lineDimTop, refat);
                FamilyParameter paramDimWidth = doc.FamilyManager.AddParameter("w", BuiltInParameterGroup.PG_CONSTRAINTS, ParameterType.Length, false);
                dimTop.FamilyLabel = paramDimWidth;

                var dimLeft = doc.FamilyCreate.NewLinearDimension(rlView, lineDimLeft, refal);
                dimLeft.FamilyLabel = paramDimWidth;

                
                View fView = FindElement(doc, typeof(View), "Назад") as View;
                var dimV = doc.FamilyCreate.NewLinearDimension(fView, lineDimV, refaZ);                
                FamilyParameter paramDimV = doc.FamilyManager.AddParameter("Extrusion End", BuiltInParameterGroup.PG_CONSTRAINTS, ParameterType.Length, false);             
                dimV.FamilyLabel = paramDimV;
                                

                //Dimensions locked  

                ReferenceArray refaEqX = new ReferenceArray();                
                ReferencePlane refpEqX = doc.FamilyCreate.NewReferencePlane(new XYZ(0, 0, 0), new XYZ(1, 0, 0), cutVec, rlView);
                refpEqX.Pinned = true;
                doc.Regenerate();

                refaEqX.Append(refpb.GetReference());
                refaEqX.Append(refpEqX.GetReference());               
                refaEqX.Append(refpt.GetReference());

                Line lineDimEqLeft = Line.CreateBound(new XYZ(ptl.X-0.5, ptl.Y , ptl.Z), new XYZ(pbl.X-0.5, pbl.Y , pbl.Z));

                var dimEqLeft = doc.FamilyCreate.NewLinearDimension(rlView, lineDimEqLeft, refaEqX);           

                DimensionSegmentArray dimSegArrDimLeft = dimEqLeft.Segments;
                dimEqLeft.AreSegmentsEqual = true;


                ReferenceArray refaEqY = new ReferenceArray();
                ReferencePlane refpEqY = doc.FamilyCreate.NewReferencePlane(new XYZ(0, 0, 0), new XYZ(0, 1, 0), cutVec, rlView);
                refpEqY.Pinned = true;
                doc.Regenerate();             

                refaEqY.Append(refpl.GetReference());
                refaEqY.Append(refpEqY.GetReference());
                refaEqY.Append(refpr.GetReference());
               
                Line lineDimEqTop = Line.CreateBound(new XYZ(ptl.X , ptl.Y+0.5 , ptl.Z), new XYZ(ptr.X , ptr.Y +0.5, ptr.Z));

                var dimEqTop = doc.FamilyCreate.NewLinearDimension(rlView, lineDimEqTop, refaEqY);

                DimensionSegmentArray dimSegArrDimTop = dimEqTop.Segments;
                dimEqTop.AreSegmentsEqual = true;

                //Locking Reference Planes
                Tools tools = new Tools();
                
                PlanarFace faceUp = tools.findFace(box, new XYZ(0.0, 0.0, 1.0));
                doc.FamilyCreate.NewAlignment(fView, faceUp.Reference, refp1z.GetReference());

                PlanarFace faceRight = tools.findFace(box, new XYZ(1.0, 0.0, 0.0));
                doc.FamilyCreate.NewAlignment(rlView, refpr.GetReference(), faceRight.Reference);

                PlanarFace faceFront = tools.findFace(box, new XYZ(0.0, -1.0, 0.0));
                doc.FamilyCreate.NewAlignment(rlView, refpb.GetReference(), faceFront.Reference);

                PlanarFace faceLeft = tools.findFace(box, new XYZ(-1.0, 0.0, 0.0));
                doc.FamilyCreate.NewAlignment(rlView, refpl.GetReference(), faceLeft.Reference);


                tx.Commit();                
            }

            return Result.Succeeded;
        }


    }



}
