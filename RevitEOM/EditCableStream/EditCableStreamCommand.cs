using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EditCableStream.ViewModel;




[Transaction(TransactionMode.Manual)]
class EditCableStreamCommand : IExternalCommand
{
    public static string commandVersion = "1.0.0.0";
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uidoc = commandData.Application.ActiveUIDocument;
        var vm = new ViewModelEditCableStream(uidoc);

        vm.EditCableStreamView.ShowDialog();
        //vm.EditCableStreamView.Show();
        return Result.Succeeded;
        
        
    }

   
}

