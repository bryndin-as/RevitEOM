using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Test.ViewModel;



[Transaction(TransactionMode.Manual)]
public class TestCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uidoc = commandData.Application.ActiveUIDocument;
        var vm = new ViewModelTest(uidoc);

        vm.TestView.ShowDialog();
        return Result.Succeeded;
    }
}









