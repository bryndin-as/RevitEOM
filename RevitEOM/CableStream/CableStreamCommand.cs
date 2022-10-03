using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CableStream.ViewModel;


[Transaction(TransactionMode.Manual)]
class CableStreamCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uidoc = commandData.Application.ActiveUIDocument;
        var vm = new ViewModelCableStream(uidoc);

        vm.CableStreamView.ShowDialog();
        return Result.Succeeded;
    }
}

