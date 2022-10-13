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


        private string _TextToCurGroupNamber = string.Empty;

        public string TextToCurGroupNamber
        {
            get => _TextToCurGroupNamber;
            set => Set(ref _TextToCurGroupNamber, value);
        }

        private string _TextToNewGroupNumber = string.Empty;

        public string TextToNewGroupNumber
        {
            get => _TextToNewGroupNumber;
            set => Set(ref _TextToNewGroupNumber, value);
        }


        private List<ElementId> _FamilyInstanseOnView;

        public List<ElementId> FamilyInstanseOnView
        {
            get => _FamilyInstanseOnView;
            set => Set(ref _FamilyInstanseOnView, value);
        }


        private List<ElementId> _CabelTypesFR;

        public List<ElementId> CabelTypesFR
        {
            get => _CabelTypesFR;
            set => Set(ref _CabelTypesFR, value);
        }

        private List<ElementId> _CabelTypes;

        public List<ElementId> CabelTypes
        {
            get => _CabelTypes;
            set => Set(ref _CabelTypes, value);
        }

        private List<string> _ListCaleTypeFRName;

        public List<string> ListCaleTypeFRName
        {
            get => _ListCaleTypeFRName;
            set => Set(ref _ListCaleTypeFRName, value);
        }


        private List<string> _ListCaleTypeName;

        public List<string> ListCaleTypeName
        {
            get => _ListCaleTypeName;
            set => Set(ref _ListCaleTypeName, value);
        }


        private List<string> _ListCableTypeGraphics;

        public List<string> ListCableTypeGraphics
        {
            get => _ListCableTypeGraphics;
            set => Set(ref _ListCableTypeGraphics, value);
        }



        private string _SelecteditemCableTypeGraphics;

        public string SelecteditemCableTypeGraphics
        {
            get => _SelecteditemCableTypeGraphics;
            set => Set(ref _SelecteditemCableTypeGraphics, value);
        }







        private List<Parameter> _ListGroupNumberParam;

        public List<Parameter> ListGroupNumberParam
        {
            get => _ListGroupNumberParam;
            set => Set(ref _ListGroupNumberParam, value);
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
        public LambdaCommand<object> EditCableType { get; set; }
        public LambdaCommand<object> EditCableGroupNumber { get; set; }
        public LambdaCommand<object> SetGroupNumber { get; set; }
        public LambdaCommand<object> SetCabelType { get; set; }
        public LambdaCommand<object> FilterCable { get; set; }
        #endregion


        public ViewModelEditCableStream(UIDocument uidoc)
        {
            UIdoc = uidoc;
            Doc = uidoc.Document;

            List<ElementId> CurrentFamilys = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance))
                                                                      .OfCategory(BuiltInCategory.OST_ElectricalFixtures)
                                                                      .Cast<FamilyInstance>()
                                                                      .Where(it => it.Symbol.FamilyName.Contains("ASML_ЭОМ_КЛ_"))
                                                                      .Select(it => it.Id)
                                                                      .ToList();
            FamilyInstanseOnView = CurrentFamilys;


            List<ElementId> CabelTypesList = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
                                                                     .Cast<FamilySymbol>()
                                                                     .Where(it => it.FamilyName == "Кабель_Графика")
                                                                     .Select(it => it.Id)
                                                                     .ToList();

            CabelTypes = CabelTypesList;

            List<ElementId> CabelTypesFRList = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
                                                                      .Cast<FamilySymbol>()
                                                                      .Where(it => it.FamilyName == "Кабель_FR_Графика")
                                                                      .Select(it => it.Id)
                                                                      .ToList();

            CabelTypesFR = CabelTypesFRList;

            List<string> CableTypeGraphics = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
                                                                    .Cast<FamilySymbol>()
                                                                    .Where(it => it.FamilyName.Contains("Графика"))
                                                                    .Select(it => it.Name)
                                                                    .ToList();

            ListCableTypeGraphics = CableTypeGraphics;



            foreach (var item in ListCableTypeGraphics)
            {
                if (item == "Нет")
                {
                    SelecteditemCableTypeGraphics = item;
                }
                if (item == "Нет FR")
                {
                    SelecteditemCableTypeGraphics = item;
                }

            }

            ObservableCollection<CableParameter> ViewDataCollection = new ObservableCollection<CableParameter>();
            List<Parameter> ViewDataCollectionGroupNumberParamList = new List<Parameter>();

            List<string> ViewDataCollectionType = new List<string>();
            List<string> ViewDataCollectionTypeFR = new List<string>();


            foreach (var FamType in CabelTypes)
            {
                Element elem = Doc.GetElement(FamType);
                ViewDataCollectionType.Add(elem.Name);
            }
            ViewDataCollectionType.Sort();
            ListCaleTypeName = ViewDataCollectionType.Distinct().ToList();


            foreach (var FamTypeFR in CabelTypesFR)
            {
                Element elem = Doc.GetElement(FamTypeFR);
                ViewDataCollectionTypeFR.Add(elem.Name);
            }
            ViewDataCollectionTypeFR.Sort();
            ListCaleTypeFRName = ViewDataCollectionTypeFR.Distinct().ToList();


            foreach (var CurFam in FamilyInstanseOnView)
            {
                Element elem = Doc.GetElement(CurFam);
                foreach (Parameter ParamGroupNumber in elem.Parameters)
                {
                    if (ParamGroupNumber.Definition.Name.Contains("_Номер группы"))
                    {
                        ViewDataCollectionGroupNumberParamList.Add(ParamGroupNumber);
                    }
                }
            }
            ListGroupNumberParam = ViewDataCollectionGroupNumberParamList;



            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Поток_ОДН", "Г_Поток_HF", "Нет", ListCaleTypeName);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Поток_ОДН", "Г_Поток_LS", "Нет", ListCaleTypeName);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Линия_ОДН", "Г_Линия_HF", "Нет", ListCaleTypeName);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Линия_ОДН", "Г_Линия_LS", "Нет", ListCaleTypeName);

            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Поток_ОКЛ", "Г_Поток_FRHF", "Нет FR", ListCaleTypeFRName);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Поток_ОКЛ", "Г_Поток_FRLS", "Нет FR", ListCaleTypeFRName);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Линия_ОКЛ", "Г_Линия_FRHF", "Нет FR", ListCaleTypeFRName);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Линия_ОКЛ", "Г_Линия_FRLS", "Нет FR", ListCaleTypeFRName);

            //DataCollection = ViewDataCollection;
            Cablecollection = CollectionViewSource.GetDefaultView(DataCollection);

            EditCableType = new LambdaCommand<object>(p => true, p => EditCableTypeAction());
            EditCableGroupNumber = new LambdaCommand<object>(p => true, p => EditCableGroupNumberAction());
            SetGroupNumber = new LambdaCommand<object>(p => true, p => SetGroupNumberAction());
            SetCabelType = new LambdaCommand<object>(p => true, p => SetCabelTypeAction());
            FilterCable = new LambdaCommand<object>(p => true, p => Filter());
        }

        private void CreateCableStream(ObservableCollection<CableParameter> ViewDataCollection, string FamilyName, string FamilyTypeName, string IsCableTypeNotName, List<string> ListCaleType)
        {
            foreach (var CurFam in FamilyInstanseOnView)
            {
                Element elem = Doc.GetElement(CurFam);
                FamilyInstance Inst = elem as FamilyInstance;
                Family Fam = Inst.Symbol.Family;
                if (Fam.Name == FamilyName) //"ASML_ЭОМ_КЛ_Г_Поток_ОКЛ"
                {
                    if (Inst.Name == FamilyTypeName) //"Г_Поток_FRHF"
                    {
                        foreach (Parameter FamilyParam in elem.Parameters)
                        {

                            if (FamilyParam.Definition.Name.Contains("_Тип КЛ"))
                            {

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

                                if (ElemType.Name != IsCableTypeNotName) //"Нет FR"
                                {
                                    string GroupNumber = string.Empty;

                                    foreach (var GroupNumberParam in ListGroupNumberParam)
                                    {
                                        if (GroupNumberParam.Element.Name == FamilyTypeName) //"Г_Поток_FRHF"
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
                                    DataCollection = ViewDataCollection;
                                }
                            }
                        }
                    }
                }
            }
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

        private void EditCableTypeAction()
        {
            foreach (var datacoll in DataCollection)
            {
                string SelTypeFamily = datacoll.SelecteditemCableType;
                string NumberKL = datacoll.NumberKL;

                Element elem = Doc.GetElement(new ElementId(datacoll.Id));
                ParameterSet ParamNames = elem.Parameters;

                foreach (Parameter ParamName in ParamNames)
                {
                    if (ParamName.Definition.Name == NumberKL)
                    {
                        ElementId FamilyTapesID = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
                                                                     .Cast<FamilySymbol>()
                                                                     .Where(it => it.FamilyName.Contains("Графика"))
                                                                     .Where(it => it.Name == SelTypeFamily)
                                                                     .Select(it => it.Id)
                                                                     .FirstOrDefault();

                        using (Transaction transaction = new Transaction(Doc))
                        {
                            transaction.Start("Перезапись параметров");
                            ParamName.Set(FamilyTapesID);
                            transaction.Commit();
                        }
                    }
                }
            }
            TaskDialog.Show("INFO", "Тип кабеля изменен");
        }

        private void EditCableGroupNumberAction()
        {
            foreach (var datacoll in DataCollection)
            {
                string CurGroupNumber = datacoll.GroupNumber;

                Element elem = Doc.GetElement(new ElementId(datacoll.Id));
                ParameterSet ParamNames = elem.Parameters;

                foreach (Parameter ParamName in ParamNames)
                {
                    foreach (var GroupNumberParam in ListGroupNumberParam)
                    {

                        if (ParamName.Definition.Name == GroupNumberParam.Definition.Name)
                        {
                            using (Transaction transaction = new Transaction(Doc))
                            {
                                transaction.Start("Перезапись параметров");
                                ParamName.Set(CurGroupNumber);
                                transaction.Commit();
                            }
                        }
                    }
                }
            }
            TaskDialog.Show("INFO", "Номер группы изменен");
        }

        private void SetGroupNumberAction()
        {
            foreach (var datacoll in DataCollection)
            {
                if (TextToCurGroupNamber == datacoll.GroupNumber)
                {
                    datacoll.GroupNumber = TextToNewGroupNumber;
                }
            }
            Cablecollection.Refresh();
        }

        private void SetCabelTypeAction()
        {
            foreach (var datacoll in DataCollection)
            {
                if (TextToCurGroupNamber == datacoll.GroupNumber)
                {
                    datacoll.SelecteditemCableType = SelecteditemCableTypeGraphics;
                }
            }
            Cablecollection.Refresh();
        }
    }
}

