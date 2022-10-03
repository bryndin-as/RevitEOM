using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Electrical;
using Utilities;
using EditCableStream.View;
using RevitEOM.EditCableStream.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;

namespace EditCableStream.ViewModel
{
    class ViewModelEditCableStream : ViewModelBase
    {
        #region BaseProperty
        private Document Doc { get; }
        private UIDocument UIdoc { get; }

        private EditCableStreamView _EditCableStream;

        public EditCableStreamView EditCableStreamView
        {
            get
            {
                if (_EditCableStream == null)
                {
                    _EditCableStream = new EditCableStreamView() { DataContext = this };
                }
                return _EditCableStream;

            }
            set
            {
                _EditCableStream = value;
                OnPropertyChanged(nameof(EditCableStreamView));
            }
        }
        #endregion

        #region Property
        private string _Check = "Check";

        public string Check
        {
            get => _Check;
            set => Set(ref _Check, value);
        }



        private string _TextToFilter = string.Empty;

        public string TextToFilter
        {
            get => _TextToFilter;
            set => Set(ref _TextToFilter, value);
        }



        private ICollectionView _Cablecollection;

        public ICollectionView Cablecollection
        {
            get => _Cablecollection;
            set => Set(ref _Cablecollection, value);
        }

        private ObservableCollection<CableParameter> Cable;

        public ObservableCollection<CableParameter> DataCollection
        {
            get => new ObservableCollection<CableParameter>(Cable);
            set => Set(ref Cable, value);
        }
        #endregion

        #region Command
        public LambdaCommand<object> EditCableStreamTest { get; set; }
        public LambdaCommand<object> FilterCable { get; set; }
        #endregion


        public ViewModelEditCableStream(UIDocument uidoc)
        {
            UIdoc = uidoc;
            Doc = uidoc.Document;

            List<ElementId> CurrentFamilys = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
                                                                      .OfCategory(BuiltInCategory.OST_ElectricalFixtures)
                                                                      .Cast<FamilyInstance>()
                                                                      .Where(it => it.Name.Contains("Г_Поток_FR")) //Г_Поток_FRLS
                                                                      .Select(it => it.Id)
                                                                      .ToList();

            List<ElementId> CabelTypesFR = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
                                                                      .Cast<FamilySymbol>()
                                                                      .Where(it => it.FamilyName == "Кабель_FR_Графика") // Кабель_Графика_FRLS_ОснЛиния
                                                                      .Select(it => it.Id)
                                                                      .ToList();

            ObservableCollection<CableParameter> ViewDataCollection = new ObservableCollection<CableParameter>();
            List<string> ViewDataCollectionType = new List<string>();
            List<Parameter> ViewDataCollectionGroupNumberParam = new List<Parameter>();
            //List<string> ViewDataCollectionGroupNumberValue = new List<string>();

            foreach (var FamTypeFR in CabelTypesFR)
            {
                Element elem = Doc.GetElement(FamTypeFR);
                ViewDataCollectionType.Add(elem.Name);
            }
            ViewDataCollectionType.Sort();
            List<string> ListCaleType = ViewDataCollectionType.Distinct().ToList();


            foreach (var CurFam in CurrentFamilys)
            {
                Element elem = Doc.GetElement(CurFam);
                foreach (Parameter ParamGroupNumber in elem.Parameters)
                {
                    if (ParamGroupNumber.Definition.Name.Contains("_Номер группы"))
                    {
                        ViewDataCollectionGroupNumberParam.Add(ParamGroupNumber);
                    }
                }
            }
            #region Old
            //провекра
            //foreach (var CurFam in CurrentFamilys)
            //{
            //    Element elem = Doc.GetElement(CurFam);
            //    foreach (Parameter ParamNameGroupNumber in elem.Parameters)
            //    {
            //        if (ParamNameGroupNumber.Definition.Name.Contains("_Номер группы"))
            //        {
            //            if (ParamNameGroupNumber.AsString() != "")
            //            {
            //                ViewDataCollectionGroupNumberValue.Add(ParamNameGroupNumber.AsString());
            //            }
            //        }
            //    }
            //}
            //ViewDataCollectionGroupNumberValue.Sort(); 
            #endregion

            foreach (var CurFam in CurrentFamilys)
            {
                Element elem = Doc.GetElement(CurFam);
                FamilyInstance Inst = elem as FamilyInstance;
                Family Fam = Inst.Symbol.Family;



                if (Fam.Name == "ASML_ЭОМ_КЛ_Г_Поток_ОКЛ") //  && Inst.Name == "Г_Поток_FRHF"
                {
                    if (Inst.Name == "Г_Поток_FRHF")
                    {
                        foreach (Parameter FamilyParam in elem.Parameters)
                        {

                            if (FamilyParam.Definition.Name.Contains("_Тип КЛ"))
                            {
                                #region Lenght
                                var ElemLen = FamilyParam.Element;
                                string CableLenghtSegment = string.Empty;

                                foreach (Parameter ParamLenght in ElemLen.Parameters)
                                {
                                    if (ParamLenght.Definition.Name == "Длина")
                                    {
                                        var ParamLenghtSegment = ParamLenght.AsValueString();
                                        CableLenghtSegment = ParamLenghtSegment;
                                    }
                                }

                                var ElemIdType = FamilyParam.AsElementId();
                                Element ElemType = Doc.GetElement(ElemIdType);
                                #endregion

                                if (ElemType.Name != "Нет FR")
                                {
                                    string GroupNumber = string.Empty;

                                    foreach (var GroupNumberParam in ViewDataCollectionGroupNumberParam)
                                    {

                                        if (GroupNumberParam.Element.Name == "Г_Поток_FRHF")
                                        {
                                            var GroupNumberName = GroupNumberParam.Definition.Name;
                                            string[] GroupNumberNamePrefix = GroupNumberName.Split(new char[] { '_' });
                                            var TypeKLNamePrefix = FamilyParam.Definition.Name.Split('_');

                                            if (TypeKLNamePrefix[0].Equals(GroupNumberNamePrefix[0]))
                                            {
                                                GroupNumber = GroupNumberParam.AsString();
                                            }
                                        }


                                    }
                                    ViewDataCollection.Add(new CableParameter()
                                    {
                                        Id = elem.Id.IntegerValue,
                                        NumberKL = FamilyParam.Definition.Name,
                                        CableType = ListCaleType,
                                        SelecteditemCableType = ElemType.Name,
                                        GroupNumber = GroupNumber,
                                        CableLength = CableLenghtSegment

                                    });
                                }
                            }
                        }

                    }
                    if (Inst.Name == "Г_Поток_FRLS")
                    {
                        foreach (Parameter FamilyParam in elem.Parameters)
                        {

                            if (FamilyParam.Definition.Name.Contains("_Тип КЛ"))
                            {
                                #region Lenght
                                var ElemLen = FamilyParam.Element;
                                string CableLenghtSegment = string.Empty;

                                foreach (Parameter ParamLenght in ElemLen.Parameters)
                                {
                                    if (ParamLenght.Definition.Name == "Длина")
                                    {
                                        var ParamLenghtSegment = ParamLenght.AsValueString();
                                        CableLenghtSegment = ParamLenghtSegment;
                                    }
                                }

                                var ElemIdType = FamilyParam.AsElementId();
                                Element ElemType = Doc.GetElement(ElemIdType);
                                #endregion

                                if (ElemType.Name != "Нет FR")
                                {
                                    string GroupNumber = string.Empty;

                                    foreach (var GroupNumberParam in ViewDataCollectionGroupNumberParam)
                                    {

                                        if (GroupNumberParam.Element.Name == "Г_Поток_FRLS")
                                        {
                                            var GroupNumberName = GroupNumberParam.Definition.Name;
                                            string[] GroupNumberNamePrefix = GroupNumberName.Split(new char[] { '_' });
                                            var TypeKLNamePrefix = FamilyParam.Definition.Name.Split('_');

                                            if (TypeKLNamePrefix[0].Equals(GroupNumberNamePrefix[0]))
                                            {
                                                GroupNumber = GroupNumberParam.AsString();
                                            }
                                        }


                                    }
                                    ViewDataCollection.Add(new CableParameter()
                                    {
                                        Id = elem.Id.IntegerValue,
                                        NumberKL = FamilyParam.Definition.Name,
                                        CableType = ListCaleType,
                                        SelecteditemCableType = ElemType.Name,
                                        GroupNumber = GroupNumber,
                                        CableLength = CableLenghtSegment

                                    });
                                }
                            }
                        }
                    }
                }

            }
            DataCollection = ViewDataCollection;
            Cablecollection = CollectionViewSource.GetDefaultView(DataCollection);

            EditCableStreamTest = new LambdaCommand<object>(p => true, p => EditCableStreamTestAction());
            FilterCable = new LambdaCommand<object>(p => true, p => Filter());
        }


        private bool Filter()
        {
            Cablecollection.Filter = FilterGroupNumber;
            return true;
        }

        private bool FilterGroupNumber(object obj)
        {
            if (!string.IsNullOrEmpty(TextToFilter))
            {
                var Cab = obj as CableParameter;
                return Cab != null && Cab.GroupNumber.Contains(TextToFilter);
            }
            return true;
        }

        private void EditCableStreamTestAction()
        {
            foreach (var datacoll in DataCollection)
            {
                var SelTypeFamily = datacoll.SelecteditemCableType;
                var group = datacoll.NumberKL;

                Element elem = Doc.GetElement(new ElementId(datacoll.Id));
                ParameterSet ps = elem.Parameters;

                foreach (Parameter p in ps)
                {
                    if (p.Definition.Name == group)
                    {
                        ElementId FamilyTapesID = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
                                                                     .Cast<FamilySymbol>()
                                                                     .Where(it => it.FamilyName == "Кабель_FR_Графика")
                                                                     .Where(it => it.Name == SelTypeFamily)
                                                                     .Select(it => it.Id)
                                                                     .FirstOrDefault();
                        using (Transaction transaction = new Transaction(Doc))
                        {
                            transaction.Start("Перезапись параметров");
                            p.Set(FamilyTapesID);
                            transaction.Commit();
                        }
                    }
                }
            }
            TaskDialog.Show("INFO", "Готово");
        }
    }
}