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
using CableStream.View;
using Autodesk.Revit.DB.Electrical;

namespace CableStream.ViewModel
{
    class ViewModelCableStream : ViewModelBase

    {
        private Document Doc { get; }
        private UIDocument UIdoc { get; }

        private CableStreamView _CableStream;

        public CableStreamView CableStreamView
        {
            get
            {
                if (_CableStream == null)
                {
                    _CableStream = new CableStreamView() { DataContext = this };
                }
                return _CableStream;

            }
            set
            {
                _CableStream = value;
                OnPropertyChanged(nameof(CableStreamView));
            }
        }

        public LambdaCommand<object> CableStreamTest { get; set; }

        public ViewModelCableStream(UIDocument uidoc)
        {
            UIdoc = uidoc;
            Doc = uidoc.Document;
            CableStreamTest = new LambdaCommand<object>(p => true, p => CableStreamTestAction());
            //ButtonTransaction = new LambdaCommand<object>(p => true, p => TransactionMenhod());

        }

        private void CableStreamTestAction()
        {

            List<ElementId> currentFamilys = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
                                                                      .OfCategory(BuiltInCategory.OST_ElectricalFixtures)
                                                                      .Cast<FamilyInstance>()
                                                                      .Where(it => it.Symbol.FamilyName.Contains("ASML_ЭОМ_КЛ_"))
                                                                      .Select(it => it.Id)
                                                                      .ToList();

            //List<ElementId> cabelTypesList = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
            //                                                         .Cast<FamilySymbol>()
            //                                                         .Where(it => it.FamilyName == "Кабель")
            //                                                         .Select(it => it.Id)
            //                                                         .ToList();

            ElementId FamilyTapesID = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
                                                                    .Cast<FamilySymbol>()
                                                                    .Where(it => it.FamilyName == "Кабель")
                                                                    //.Where(it => it.Name == SelTypeFamily)
                                                                    .Select(it => it.Id)
                                                                    .FirstOrDefault();

            Element FamilyTapesElem = Doc.GetElement(FamilyTapesID);

            foreach (var currentFamily in currentFamilys)
            {
                Element currentFamilyElem = Doc.GetElement(currentFamily);
                FamilyInstance Inst = currentFamilyElem as FamilyInstance;
                Family curfamily = Inst.Symbol.Family;


                if (curfamily.Name.Contains("ОДН"))
                {
                    foreach (Parameter familyParameter in currentFamilyElem.Parameters)
                    {
                        if (familyParameter.Definition.Name.Contains("_Тип КЛ"))
                        {
                            ElementId elemId = familyParameter.AsElementId();
                            Element elem = Doc.GetElement(elemId);


                            if (elem.Name == FamilyTapesElem.Name)
                            {
                                ElementId el = FamilyTapesElem.Id;

                                using (Transaction transaction = new Transaction(Doc))
                                {
                                    transaction.Start("Перезапись параметров");
                                    familyParameter.Set(el);
                                    transaction.Commit();
                                }
                            }


                            //if(elem.Name == )

                        }

                    }
                }

            }

            try
            {
                TaskDialog.Show("INFO", "Test");
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.ToString());
            }

        }

    }
}
