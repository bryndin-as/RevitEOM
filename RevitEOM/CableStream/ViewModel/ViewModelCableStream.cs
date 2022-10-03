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
            //var currentView = Doc.ActiveView;

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
