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


        #region Старые свойства
        //private List<ElementId> _CabelTypesFR;

        //public List<ElementId> CabelTypesFR
        //{
        //    get => _CabelTypesFR;
        //    set => Set(ref _CabelTypesFR, value);
        //}

        //private List<ElementId> _CabelTypes;

        //public List<ElementId> CabelTypes
        //{
        //    get => _CabelTypes;
        //    set => Set(ref _CabelTypes, value);
        //}

        //private List<Parameter> _ListGroupNumberParam;

        //public List<Parameter> ListGroupNumberParam
        //{
        //    get => _ListGroupNumberParam;
        //    set => Set(ref _ListGroupNumberParam, value);
        //} 
        #endregion

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


            #region Старое получение
            //List<ElementId> CabelTypesList = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
            //                                                         .Cast<FamilySymbol>()
            //                                                         .Where(it => it.FamilyName == "Кабель_Графика")
            //                                                         .Select(it => it.Id)
            //                                                         .ToList();

            //CabelTypes = CabelTypesList;

            //List<ElementId> CabelTypesFRList = new FilteredElementCollector(Doc).OfClass(typeof(FamilySymbol))
            //                                                          .Cast<FamilySymbol>()
            //                                                          .Where(it => it.FamilyName == "Кабель_FR_Графика")
            //                                                          .Select(it => it.Id)
            //                                                          .ToList();

            //CabelTypesFR = CabelTypesFRList; 
            #endregion

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

            #region Старое
            //List<Parameter> ViewDataCollectionGroupNumberParamList = new List<Parameter>();
            //List<string> ViewDataCollectionType = new List<string>();
            //List<string> ViewDataCollectionTypeFR = new List<string>();


            //foreach (var FamType in CabelTypes)
            //{
            //    Element elem = Doc.GetElement(FamType);
            //    ViewDataCollectionType.Add(elem.Name);
            //}
            //ViewDataCollectionType.Sort();
            //ListCaleTypeName = ViewDataCollectionType.Distinct().ToList();


            //foreach (var FamTypeFR in CabelTypesFR)
            //{
            //    Element elem = Doc.GetElement(FamTypeFR);
            //    ViewDataCollectionTypeFR.Add(elem.Name);
            //}
            //ViewDataCollectionTypeFR.Sort();
            //ListCaleTypeFRName = ViewDataCollectionTypeFR.Distinct().ToList(); 

            //foreach (var CurFam in FamilyInstanseOnView)
            //{
            //    Element elem = Doc.GetElement(CurFam);
            //    foreach (Parameter ParamGroupNumber in elem.Parameters)
            //    {
            //        if (ParamGroupNumber.Definition.Name.Contains("_Номер группы"))
            //        {
            //            ViewDataCollectionGroupNumberParamList.Add(ParamGroupNumber);
            //        }
            //    }
            //}
            //ListGroupNumberParam = ViewDataCollectionGroupNumberParamList;
            #endregion

            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Поток_ОДН", "Г_Поток_HF", ListCableTypeGraphics); //ListCaleTypeName
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Поток_ОДН", "Г_Поток_LS", ListCableTypeGraphics);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Линия_ОДН", "Г_Линия_HF", ListCableTypeGraphics);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Линия_ОДН", "Г_Линия_LS", ListCableTypeGraphics);

            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Поток_ОКЛ", "Г_Поток_FRHF", ListCableTypeGraphics); //ListCaleTypeFRName
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Поток_ОКЛ", "Г_Поток_FRLS", ListCableTypeGraphics);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Линия_ОКЛ", "Г_Линия_FRHF", ListCableTypeGraphics);
            CreateCableStream(ViewDataCollection, "ASML_ЭОМ_КЛ_Г_Линия_ОКЛ", "Г_Линия_FRLS", ListCableTypeGraphics);

            //DataCollection = ViewDataCollection;
            Cablecollection = CollectionViewSource.GetDefaultView(DataCollection);

            EditCableType = new LambdaCommand<object>(p => true, p => EditCableTypeAction());
            EditCableGroupNumber = new LambdaCommand<object>(p => true, p => EditCableGroupNumberAction());
            SetGroupNumber = new LambdaCommand<object>(p => true, p => SetGroupNumberAction());
            SetCabelType = new LambdaCommand<object>(p => true, p => SetCabelTypeAction());
            FilterCable = new LambdaCommand<object>(p => true, p => Filter());
        }


        private void CreateCableStream(ObservableCollection<CableParameter> ViewDataCollection, string FamilyName, string FamilyTypeName, List<string> ListCaleType)
        {
            foreach (var CurFam in FamilyInstanseOnView)
            {
                Element elem = Doc.GetElement(CurFam);
                FamilyInstance Inst = elem as FamilyInstance;
                Family Fam = Inst.Symbol.Family;
                if (Fam.Name == FamilyName) //"ASML_ЭОМ_КЛ_Г_Поток_ОКЛ"
                {
                    List<Parameter> FamilyParamGroupNumber = new List<Parameter>();
                    if(Inst.Name == FamilyTypeName) //"Г_Поток_FRHF"
                    {
                        foreach (Parameter FamilyParam in elem.Parameters)
                        {
                            if (FamilyParam.Definition.Name.Contains("_Номер группы"))
                            {
                                FamilyParamGroupNumber.Add(FamilyParam);
                            }
                        }
                    }

                    if (Inst.Name == FamilyTypeName) //"Г_Поток_FRHF"
                    {
                       
                        foreach (Parameter FamilyParam in elem.Parameters)
                        {
                            

                            if (FamilyParam.Definition.Name.Contains("_Тип КЛ"))
                            {
                                string par = FamilyParam.Definition.Name;
                                string[] parsplit = par.Split('_');
                                string parprefics = parsplit[0];


                                var Elem = FamilyParam.Element;
                                string CableLenghtSegment = string.Empty;

                                foreach (Parameter ParamLenght in Elem.Parameters)
                                {
                                    if (ParamLenght.Definition.Name == "Длина")
                                    {
                                        var ParamLenghtSegment = ParamLenght.AsValueString();
                                        CableLenghtSegment = ParamLenghtSegment;
                                    }
                                }

                                string paramname = string.Empty;

                                foreach (Parameter ParamPanel in Elem.Parameters)
                                {
                                    if (ParamPanel.Definition.Name.Contains("_Имя панели"))
                                    {
                                        string parampan = ParamPanel.Definition.Name;
                                        string[] parampansplit = parampan.Split('_');
                                        string parampanprefics = parampansplit[0];

                                        if (parprefics == parampanprefics)
                                        {
                                            var ParamPanelName = ParamPanel.AsString();
                                            paramname = ParamPanelName;
                                        }

                                    }
                                }






                                var ElemIdType = FamilyParam.AsElementId();
                                Element ElemType = Doc.GetElement(ElemIdType);
                                string GroupNumber = string.Empty;

                                foreach (var GroupNumberParam in FamilyParamGroupNumber) 
                                {
                                    if (GroupNumberParam.AsString() != string.Empty)
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
                                }

                                if (GroupNumber != string.Empty)
                                {
                                    ViewDataCollection.Add(new CableParameter()
                                    {
                                        Id = elem.Id.IntegerValue,
                                        NumberKL = FamilyParam.Definition.Name,
                                        CableType = ListCaleType,
                                        SelecteditemCableType = ElemType.Name,
                                        GroupNumber = GroupNumber,
                                        CableLength = CableLenghtSegment,
                                        PanelName = paramname
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
                string NumberKL = datacoll.NumberKL;
                string[] NumberKLSplit = NumberKL.Split('_');
                string PreficsNumber = NumberKLSplit[0];

                Element elem = Doc.GetElement(new ElementId(datacoll.Id));
                ParameterSet ParamNames = elem.Parameters;

                foreach (Parameter ParamName in ParamNames)
                {

                    if (ParamName.Definition.Name == $"{PreficsNumber}_Номер группы") 
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

