using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Utilities;
using Test.View;
using Autodesk.Revit.DB.Electrical;

namespace Test.ViewModel
{
    public class ViewModelTest : ViewModelBase
    {
        private Document Doc { get; }
        private UIDocument UIdoc { get; }

        private TestView _TestView;

        public TestView TestView
        {
            get
            {
                if (_TestView == null)
                {
                    _TestView = new TestView() { DataContext = this };
                }
                return _TestView;

            }
            set
            {
                _TestView = value;
                OnPropertyChanged(nameof(TestView));
            }
        }

        //private string _Title = "Test_MVVM";

        //public string Title
        //{
        //    get { return _Title; }
        //    set
        //    {
        //        _Title = value;
        //        OnPropertyChanged(nameof(Title));
        //    }
        //}

        private string _Title = "Test_MVVM";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }



        public LambdaCommand<object> ButtonTest { get; set; }
        //public LambdaCommand<object> ButtonTransaction { get; set; }


        public ViewModelTest(UIDocument uidoc)
        {
            UIdoc = uidoc;
            Doc = uidoc.Document;
            ButtonTest = new LambdaCommand<object>(p => true, p => ButtonTestAction());
            //ButtonTransaction = new LambdaCommand<object>(p => true, p => TransactionMenhod());

        }

        private void ButtonTestAction()
        {
            //var currentView = Doc.ActiveView;

            //List<ElementId> CurrentFamilys = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
            //                                                           .OfCategory(BuiltInCategory.OST_GenericModel)
            //                                                           .Cast<FamilyInstance>()
            //                                                           .Where(x => x.Name == "Поток")
            //                                                           .Select(it => it.Id)
            //                                                           .ToList();
            //foreach (var CurFam in CurrentFamilys)
            //{
            //    Element elem = Doc.GetElement(CurFam);

            //    ElementId elID = null;
            //    foreach (Parameter Param in elem.Parameters)
            //    {
            //        if (Param.Definition.Name == "Тип Линии 1")
            //        {
            //            var temp = Param.AsElementId();
            //            elID = temp;
            //        }
            //    }

            //    foreach (Parameter Param in elem.Parameters)
            //    {

            //        if (Param.Definition.Name == "Тип линии 1")
            //        {
            //            using (Transaction transaction = new Transaction(Doc))
            //            {
            //                transaction.Start("Перезапись параметров");
            //                Param.Set(elID);
            //                transaction.Commit();
            //            }
            //        }
            //    }

                
            //}

            TaskDialog.Show("INFO", "INFO");
        }

        #region Transaction
        //private void TransactionMethod()
        //{

        //    CurveArray curves = new CurveArray();

        //    curves.Append(Arc.Create(XYZ.Zero, 10, 0, 2 * PI, XYZ.BasisX, XYZ.BasisY));

        //    curves.Append(Arc.Create(XYZ.Zero, 8, PI, 2 * PI, XYZ.BasisX, XYZ.BasisY));

        //    curves.Append(Arc.Create(new XYZ(-8, 0, 0), new XYZ(8, 0, 0), new XYZ(0, -3, 0)));

        //    curves.Append(Arc.Create(new XYZ(-5, 2, 0), 3, 0, PI, XYZ.BasisX, XYZ.BasisY));

        //    curves.Append(Arc.Create(new XYZ(5, 2, 0), 3, 0, PI, XYZ.BasisX, XYZ.BasisY));

        //    var activeView = Doc.ActiveView;

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Safety transaction");

        //        Doc.Create.NewDetailCurveArray(activeView, curves);

        //        transaction.Commit();
        //    }
        //} 
        #endregion

        #region FilteredElementCollector
        //private void FilteredElementCollectorMethod()
        //{
        //    List<CurveElement> curves = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_Lines)
        //                                                                 .Cast<CurveElement>()
        //                                                                 .Where(it => it.CurveElementType == CurveElementType.DetailCurve)
        //                                                                 .ToList();

        //    List<CurveElement> curves = new FilteredElementCollector(Doc).OfClass(typeof(CurveElement))
        //                                                                 .Cast<CurveElement>()
        //                                                                 .Where(it => it.CurveElementType == CurveElementType.DetailCurve)
        //                                                                 .ToList();

        //    List<CurveElement> curves = new FilteredElementCollector(Doc).WherePasses(new ElementClassFilter(typeof(CurveElement)))
        //                                                                 .Cast<CurveElement>()
        //                                                                 .Where(it => it.CurveElementType == CurveElementType.DetailCurve)
        //                                                                 .ToList();

        //    List<CurveElement> curves = new FilteredElementCollector(Doc).WherePasses(new CurveElementFilter(CurveElementType.DetailCurve))
        //                                                                 .Cast<CurveElement>()
        //                                                                 .ToList();

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Delete all detail curves");

        //        foreach (var curve in curves)
        //        {
        //            Doc.Delete(curve.Id);
        //        }

        //        transaction.Commit();
        //    }
        //} 
        #endregion

        #region NewFamilyInstance
        #region Sofa
        //Размещение экземпляра на примере Дивана
        //private void NewFamilyInstanceMethodSofa()
        //{
        //    FamilySymbol couchSymbol = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
        //                                                               .OfCategory(BuiltInCategory.OST_Furniture)
        //                                                               .Cast<FamilySymbol>()
        //                                                               .First(it => it.FamilyName == "Диван-Pensi" && it.Name == "1650 мм");
        //    XYZ couchLocation = XYZ.Zero;

        //    Level level = new FilteredElementCollector(Doc).OfClass(typeof(Level)).FirstElement() as Level;

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Insert couch");

        //        if (!couchSymbol.IsActive)
        //        {
        //            couchSymbol.Activate();
        //        }

        //        Doc.Create.NewFamilyInstance(couchLocation, couchSymbol, level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

        //        transaction.Commit();
        //    }
        //}
        #endregion
        #region Door
        //Размещение двери на основе стены
        //private void NewFamilyInstanceMethodDoor()
        //{
        //    FamilySymbol doorSymbol = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
        //                                                              .OfCategory(BuiltInCategory.OST_Doors)
        //                                                              .Cast<FamilySymbol>()
        //                                                              .First(it => it.FamilyName == "Дверь-Одинарная-Панель" && it.Name == "750 x 2000 мм");

        //    IEnumerable<Element> walls = new FilteredElementCollector(Doc).OfClass(typeof(Wall)).ToElements();

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Insert doors");

        //        if (!doorSymbol.IsActive)
        //        {
        //            doorSymbol.Activate();
        //        }

        //        foreach (var wall in walls)
        //        {
        //            Curve wallCurve = (wall.Location as LocationCurve).Curve;

        //            XYZ wallCenter = wallCurve.Evaluate(0.5, true);

        //            Doc.Create.NewFamilyInstance(wallCenter, doorSymbol, wall, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
        //        }

        //        transaction.Commit();
        //    }
        //}
        #endregion
        #region CabelTry
        //Размещение перегородки кабельного лотка на основе линии
        //private void NewFamilyInstanceMethodCabelTry()
        //{
        //    FamilySymbol separatorSymbol = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
        //                                                                    .OfCategory(BuiltInCategory.OST_CableTrayFitting)
        //                                                                    .Cast<FamilySymbol>()
        //                                                                    .First(it => it.FamilyName == "VC_Перегородка лотка");

        //    List<CableTray> cableTrays = new FilteredElementCollector(Doc).OfClass(typeof(CableTray))
        //                                                                  .Cast<CableTray>()
        //                                                                  .Where(it => it.Height == UnitUtils.ConvertToInternalUnits(50, UnitTypeId.Millimeters))
        //                                                                  .ToList();

        //    Options viewSpecificOptions = new Options()
        //    {
        //        DetailLevel = ViewDetailLevel.Medium,
        //        ComputeReferences = true
        //    };

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Insert cable tray separators");

        //        if (!separatorSymbol.IsActive)
        //        {
        //            separatorSymbol.Activate();
        //        }

        //        foreach (var cableTray in cableTrays)
        //        {
        //            Solid cableTrayGeometry = cableTray.get_Geometry(viewSpecificOptions).First(it => it is Solid) as Solid;

        //            Face cableTrayTop = cableTrayGeometry.Faces.get_Item(3);

        //            Curve cableTrayLine = (cableTray.Location as LocationCurve).Curve;

        //            XYZ cableTrayLineStartPoint = cableTrayLine.GetEndPoint(0),
        //                cableTrayLineEndPoint = cableTrayLine.GetEndPoint(1),
        //                separatorStartPoint = cableTrayTop.Project(cableTrayLineStartPoint).XYZPoint,
        //                separatorEndPoint = cableTrayTop.Project(cableTrayLineEndPoint).XYZPoint;

        //            Line separatorLine = Line.CreateBound(separatorStartPoint, separatorEndPoint);

        //            Doc.Create.NewFamilyInstance(cableTrayTop, separatorLine, separatorSymbol);
        //        }

        //        transaction.Commit();
        //    }
        //}  
        #endregion
        #endregion

        #region Location
        //private void LocationMethodWWallLenght()
        //{
        //    Wall wall = new FilteredElementCollector(Doc).OfClass(typeof(Wall))
        //                                                 .First() as Wall;

        //    LocationCurve wallLocation = wall.Location as LocationCurve;

        //    Curve locationCurve = wallLocation.Curve;

        //    double wallLength = UnitUtils.ConvertFromInternalUnits(locationCurve.Length, UnitTypeId.Meters);

        //    TaskDialog.Show("Результаты анализа", $"Длина стены: {wallLength:f2} м");

        //    XYZ wallTranslation = new XYZ(0, -3, 0);

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Move wall");

        //        wallLocation.Move(wallTranslation);

        //        transaction.Commit();
        //    }
        //}
        //private void LocationMethodСoordinates()
        //{
        //    FamilyInstance couch = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                            .OfCategory(BuiltInCategory.OST_Furniture)
        //                                                            .First() as FamilyInstance;

        //    LocationPoint couchLocation = couch.Location as LocationPoint;

        //    XYZ couchLocationPoint = couchLocation.Point;

        //    TaskDialog.Show("Результаты анализа", $"Координаты дивана (футы): {couchLocationPoint}");

        //    Line rotationAxis = Line.CreateBound(XYZ.Zero, XYZ.Zero + new XYZ(0, 0, 1));

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Rotate couch");

        //        couchLocation.Rotate(rotationAxis, System.Math.PI / 4);

        //        transaction.Commit();
        //    }
        //} 
        #endregion

        #region ElementTransformUtils
        //private void ElementTransformUtilsMethodCopy()
        //{
        //    FamilyInstance couch = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                            .OfCategory(BuiltInCategory.OST_Furniture)
        //                                                            .Cast<FamilyInstance>()
        //                                                            .Last(it => it.Symbol.FamilyName == "Диван-Pensi" && it.Symbol.Name == "1650 мм");

        //    XYZ translation = new XYZ(-10, 10, 0);

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Copy couch");

        //        ElementTransformUtils.CopyElement(Doc, couch.Id, translation);

        //        transaction.Commit();
        //    }
        //}
        //private void ElementTransformUtilsMethodMirror()
        //{
        //    FamilyInstance couch = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                            .OfCategory(BuiltInCategory.OST_Furniture)
        //                                                            .Cast<FamilyInstance>()
        //                                                            .Last(it => it.Symbol.FamilyName == "Диван-Pensi" && it.Symbol.Name == "1650 мм");

        //    Plane reflectionPlane = Plane.CreateByNormalAndOrigin(XYZ.BasisX, XYZ.Zero);

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Mirror couch");

        //        ElementTransformUtils.MirrorElement(Doc, couch.Id, reflectionPlane);

        //        transaction.Commit();
        //    }
        //}
        //private void ElementTransformUtilsMethodMove()
        //{
        //    List<ElementId> couchIds = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                               .OfCategory(BuiltInCategory.OST_Furniture)
        //                                                               .Cast<FamilyInstance>()
        //                                                               .Where(it => it.Symbol.FamilyName == "Диван-Pensi" && it.Symbol.Name == "1650 мм")
        //                                                               .Select(it => it.Id)
        //                                                               .ToList();

        //    XYZ translation = new XYZ(10, 10, 0);

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Move couches");

        //        ElementTransformUtils.MoveElements(Doc, couchIds, translation);

        //        transaction.Commit();
        //    }
        //}
        //private void ElementTransformUtilsMethodRotate()
        //{
        //    List<ElementId> couchIds = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                                .OfCategory(BuiltInCategory.OST_Furniture)
        //                                                                .Cast<FamilyInstance>()
        //                                                                .Where(it => it.Symbol.FamilyName == "Диван-Pensi" && it.Symbol.Name == "1650 мм")
        //                                                                .Select(it => it.Id)
        //                                                                .ToList();

        //    Line rotationAxis = Line.CreateBound(XYZ.Zero, XYZ.Zero + new XYZ(0, 0, 1));

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Rotate couches");

        //        ElementTransformUtils.RotateElements(Doc, couchIds, rotationAxis, Math.PI / 4);

        //        transaction.Commit();
        //    }
        //} 
        #endregion

        #region AdaptiveComponentInstanceUtils
        //private void AdaptiveComponentInstanceUtilsMenhod()
        //{
        //    List<FamilyInstance> supports = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                                     .OfCategory(BuiltInCategory.OST_GenericModel)
        //                                                                     .Cast<FamilyInstance>()
        //                                                                     .Where(it => it.Symbol.FamilyName == "VC_Опора")
        //                                                                     .ToList();

        //    FamilySymbol wireSymbol = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
        //                                                               .OfCategory(BuiltInCategory.OST_GenericModel)
        //                                                               .Cast<FamilySymbol>()
        //                                                               .First(it => it.FamilyName == "VC_Провод");


        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Insert adaptive component");

        //        if (!wireSymbol.IsActive)
        //        {
        //            wireSymbol.Activate();
        //        }

        //        for (int supportIndex = 0; supportIndex < supports.Count - 1; supportIndex++)
        //        {
        //            FamilyInstance currentSupport = supports[supportIndex],
        //                           nextSupport = supports[supportIndex + 1];

        //            List<FamilyInstance> currentConnectors = currentSupport.GetSubComponentIds()
        //                                                                   .Select(it => Doc.GetElement(it) as FamilyInstance)
        //                                                                   .Where(it => it.Symbol.FamilyName == "VC_Коннектор провода")
        //                                                                   .ToList();

        //            List<FamilyInstance> nextConnectors = nextSupport.GetSubComponentIds()
        //                                                             .Select(it => Doc.GetElement(it) as FamilyInstance)
        //                                                             .Where(it => it.Symbol.FamilyName == "VC_Коннектор провода")
        //                                                             .ToList();

        //            XYZ currentA = (currentConnectors.First(it => it.Symbol.Name == "A").Location as LocationPoint).Point,
        //                currentB = (currentConnectors.First(it => it.Symbol.Name == "B").Location as LocationPoint).Point,
        //                currentC = (currentConnectors.First(it => it.Symbol.Name == "C").Location as LocationPoint).Point,
        //                nextA = (nextConnectors.First(it => it.Symbol.Name == "A").Location as LocationPoint).Point,
        //                nextB = (nextConnectors.First(it => it.Symbol.Name == "B").Location as LocationPoint).Point,
        //                nextC = (nextConnectors.First(it => it.Symbol.Name == "C").Location as LocationPoint).Point;

        //            FamilyInstance wireA = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(Doc, wireSymbol),
        //                           wireB = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(Doc, wireSymbol),
        //                           wireC = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(Doc, wireSymbol);

        //            List<ReferencePoint> wireAPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireA)
        //                                                                                      .Select(it => Doc.GetElement(it) as ReferencePoint)
        //                                                                                      .ToList();

        //            List<ReferencePoint> wireBPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireB)
        //                                                                                      .Select(it => Doc.GetElement(it) as ReferencePoint)
        //                                                                                      .ToList();

        //            List<ReferencePoint> wireCPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireC)
        //                                                                                      .Select(it => Doc.GetElement(it) as ReferencePoint)
        //                                                                                      .ToList();

        //            XYZ currentTranslationA = currentA - wireAPlacementPoints[0].Position,
        //                currentTranslationB = currentB - wireBPlacementPoints[0].Position,
        //                currentTranslationC = currentC - wireCPlacementPoints[0].Position,
        //                nextTranslationA = nextA - wireAPlacementPoints[1].Position,
        //                nextTranslationB = nextB - wireBPlacementPoints[1].Position,
        //                nextTranslationC = nextC - wireCPlacementPoints[1].Position;

        //            wireAPlacementPoints[0].Location.Move(currentTranslationA);

        //            wireAPlacementPoints[1].Location.Move(nextTranslationA);

        //            wireBPlacementPoints[0].Location.Move(currentTranslationB);

        //            wireBPlacementPoints[1].Location.Move(nextTranslationB);

        //            wireCPlacementPoints[0].Location.Move(currentTranslationC);

        //            wireCPlacementPoints[1].Location.Move(nextTranslationC);
        //        }

        //        transaction.Commit();
        //    }
        //} 
        #endregion

        #region Transform
        //private void TransformMethodTranslation()
        //{
        //    FamilyInstance adaptiveInstance = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                                      .Cast<FamilyInstance>()
        //                                                                      .First(it => AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance(it));

        //    Transform translation = Transform.CreateTranslation(new XYZ(0, 0, 5));

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Translate adaptive component");

        //        AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, translation, true);

        //        transaction.Commit();
        //    }
        //}
        //private void TransformMethodRotation()
        //{
        //    FamilyInstance adaptiveInstance = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                                      .Cast<FamilyInstance>()
        //                                                                      .First(it => AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance(it));

        //    Transform rotation = Transform.CreateRotation(XYZ.BasisZ, PI / 4);

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Translate adaptive component");

        //        AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, rotation, true);

        //        transaction.Commit();
        //    }
        //}
        //private void TransformMethodComplex()
        //{
        //    FamilyInstance adaptiveInstance = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                                       .Cast<FamilyInstance>()
        //                                                                       .First(it => AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance(it));

        //    Transform translation = Transform.CreateTranslation(new XYZ(0, 0, 5)),
        //              rotation = Transform.CreateRotation(XYZ.BasisZ, PI / 4),
        //              complex = translation * rotation;

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Translate adaptive component");

        //        AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, complex, true);

        //        transaction.Commit();
        //    }
        //} 
        #endregion

        #region Parameters
        //private void ParametersMethod()
        //{
        //    List<CableTray> cableTrays = new FilteredElementCollector(Doc).OfClass(typeof(CableTray))
        //                                                                 .Cast<CableTray>()
        //                                                                 .ToList();

        //    List<FamilyInstance> separators = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                                       .OfCategory(BuiltInCategory.OST_CableTrayFitting)
        //                                                                       .Cast<FamilyInstance>()
        //                                                                       .Where(it => it.Symbol.FamilyName == "VC_Универсальная перегородка лотка")
        //                                                                       .ToList();

        //    List<FamilyInstance> covers = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
        //                                                                   .OfCategory(BuiltInCategory.OST_CableTrayFitting)
        //                                                                   .Cast<FamilyInstance>()
        //                                                                   .Where(it => it.Symbol.FamilyName == "VC_Универсальная крышка лотка")
        //                                                                   .ToList();

        //    FamilySymbol separatorSymbol = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
        //                                                                    .OfCategory(BuiltInCategory.OST_CableTrayFitting)
        //                                                                    .Cast<FamilySymbol>()
        //                                                                    .First(it => it.FamilyName == "VC_Универсальная перегородка лотка");

        //    FamilySymbol coverSymbol = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
        //                                                                .OfCategory(BuiltInCategory.OST_CableTrayFitting)
        //                                                                .Cast<FamilySymbol>()
        //                                                                .First(it => it.FamilyName == "VC_Универсальная крышка лотка");

        //    Options viewSpecificOptions = new Options()
        //    {
        //        DetailLevel = ViewDetailLevel.Medium,
        //        ComputeReferences = true
        //    };

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Create cable tray separators & covers");

        //        separators.ForEach(it => Doc.Delete(it.Id));

        //        covers.ForEach(it => Doc.Delete(it.Id));

        //        if (!separatorSymbol.IsActive) separatorSymbol.Activate();

        //        if (!coverSymbol.IsActive) coverSymbol.Activate();

        //        foreach (var cableTray in cableTrays)
        //        {
        //            bool isSeparatorNeeded = cableTray.LookupParameter("VC_Установить перегородку")?.AsInteger() == 1,
        //                 isCoverNeeded = cableTray.LookupParameter("VC_Установить крышку")?.AsInteger() == 1;

        //            if (isSeparatorNeeded || isCoverNeeded)
        //            {
        //                double height = cableTray.get_Parameter(BuiltInParameter.RBS_CABLETRAY_HEIGHT_PARAM).AsDouble(),
        //                       width = cableTray.get_Parameter(BuiltInParameter.RBS_CABLETRAY_WIDTH_PARAM).AsDouble();

        //                Solid cableTrayGeometry = cableTray.get_Geometry(viewSpecificOptions).First(it => it is Solid) as Solid;

        //                Face cableTrayTop = cableTrayGeometry.Faces.get_Item(3);

        //                Curve cableTrayLine = (cableTray.Location as LocationCurve).Curve;

        //                XYZ cableTrayLineStartPoint = cableTrayLine.GetEndPoint(0),
        //                    cableTrayLineEndPoint = cableTrayLine.GetEndPoint(1),
        //                    separatorStartPoint = cableTrayTop.Project(cableTrayLineStartPoint).XYZPoint,
        //                    separatorEndPoint = cableTrayTop.Project(cableTrayLineEndPoint).XYZPoint;

        //                Line partLine = Line.CreateBound(separatorStartPoint, separatorEndPoint);

        //                if (isSeparatorNeeded)
        //                {
        //                    FamilyInstance separator = Doc.Create.NewFamilyInstance(cableTrayTop, partLine, separatorSymbol);

        //                    separator.LookupParameter("Высота лотка")?.Set(height);
        //                }

        //                if (isCoverNeeded)
        //                {
        //                    FamilyInstance cover = Doc.Create.NewFamilyInstance(cableTrayTop, partLine, coverSymbol);

        //                    cover.LookupParameter("Ширина лотка")?.Set(width);
        //                }
        //            }
        //        }

        //        transaction.Commit();
        //    }
        //} 
        #endregion

        #region PickPoint
        //private void PickPointMethod()
        //{
        //    var activeView = Doc.ActiveView;

        //    if (!(activeView is ViewPlan))
        //    {
        //        TaskDialog errorDialog = new TaskDialog("Ошибка")
        //        {
        //            MainInstruction = "Данная команда предназначена только для работы на планах",
        //            VerificationText = "Дополнительное окно",
        //            CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No,
        //            DefaultButton = TaskDialogResult.Yes,
        //            FooterText = "<a href=\"https://bim.vc/edu/courses\">" + "Получить дополнительные сведения</a>"
        //        };

        //        errorDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Открыть первый попавшийся план");

        //        errorDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Завершить работу команды");

        //        TaskDialogResult dialogResult = errorDialog.Show();

        //        if (errorDialog.WasVerificationChecked())
        //        {
        //            TaskDialog.Show("Дополнительное окно", "Привет!)");
        //        }

        //        if (dialogResult == TaskDialogResult.CommandLink1 || dialogResult == TaskDialogResult.Yes)
        //        {
        //            activeView = new FilteredElementCollector(Doc).OfClass(typeof(ViewPlan)).FirstElement() as ViewPlan;

        //            UIdoc.ActiveView = activeView;
        //        }
        //        else if (dialogResult == TaskDialogResult.No)
        //        {
        //            TaskDialog.Show("Предупреждение", "Как это нет? Эээх!");

        //        }

        //    }

        //    Selection selection = UIdoc.Selection;

        //    ObjectSnapTypes snapType = ObjectSnapTypes.Centers | ObjectSnapTypes.Midpoints;

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Create polyline");

        //        XYZ lastPoint = null;

        //        while (true)
        //        {
        //            try
        //            {
        //                if (lastPoint == null)
        //                {
        //                    lastPoint = selection.PickPoint(snapType, "Укажите начальную точку (Esc - отмена)");
        //                }

        //                XYZ currentPoint = selection.PickPoint(snapType, "Укажите следующую точку (Esc - завершить)");

        //                Line line = Line.CreateBound(lastPoint, currentPoint);

        //                lastPoint = currentPoint;

        //                Doc.Create.NewDetailCurve(activeView, line);

        //                Doc.Regenerate();
        //            }
        //            catch (OperationCanceledException e)
        //            {
        //                break;
        //            }
        //        }

        //        transaction.Commit();
        //    }
        //} 
        #endregion

        #region Selection
        //private void SelectionMethod1()
        //{
        //    Selection selection = UIdoc.Selection;

        //    Reference elementRef = null;

        //    try
        //    {
        //        elementRef = selection.PickObject(ObjectType.PointOnElement, "Выберите перегородку кабельного лотка (Esc - отмена)");
        //    }
        //    catch (OperationCanceledException e)
        //    {
        //        //return Result.Cancelled;
        //    }

        //    Element selectedElement = Doc.GetElement(elementRef);

        //    TaskDialog.Show("Результаты анализа", $"Пользователь указал элемент:\n{selectedElement.Name}\n" +
        //                                          $"\nКоординаты точки:\n{elementRef.GlobalPoint}");

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Delete element");

        //        Doc.Delete(elementRef.ElementId);

        //        transaction.Commit();
        //    }
        //}
        //private void SelectionMethod2()
        //{
        //    Selection selection = UIdoc.Selection;

        //    List<Reference> elementRefs = null;

        //    try
        //    {
        //        elementRefs = selection.PickObjects(ObjectType.Element, new SeparatorSelectionFilter(), "Выберите перегородки кабельного лотка (Esc - отмена)").ToList();
        //    }
        //    catch (OperationCanceledException e)
        //    {
        //        //return Result.Cancelled;
        //    }

        //    using (Transaction transaction = new Transaction(Doc))
        //    {
        //        transaction.Start("Delete elements");

        //        elementRefs.ForEach(it => Doc.Delete(it.ElementId));

        //        transaction.Commit();
        //    }
        //}
        //private void SelectionMethod3()
        //{
        //    Selection selection = UIdoc.Selection;

        //    List<Element> selectedElements = null;

        //    try
        //    {
        //        selectedElements = selection.PickElementsByRectangle(new SeparatorSelectionFilter(),
        //                           "Выберите перегородки кабельного лотка при помощи рамки (Esc - отмена)").ToList();
        //    }
        //    catch (OperationCanceledException e)
        //    {
        //        //return Result.Cancelled;
        //    }

        //    selection.SetElementIds(selectedElements.Select(it => it.Id).ToList());
        //}

        //public class SeparatorSelectionFilter : ISelectionFilter
        //{
        //    public bool AllowElement(Element elem)
        //    {
        //        return elem is FamilyInstance instance && instance.Symbol.FamilyName == "VC_Универсальная перегородка лотка";
        //    }

        //    public bool AllowReference(Reference reference, XYZ position)
        //    {
        //        return true;
        //    }
        //} 
        #endregion
    }

}
