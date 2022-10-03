using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace RevitEOM.EditCableStream.Model
{
    public class CableParameter 
    {
        public int Id { get; set; }
        public string NumberKL { get; set; }
        public ICollection<string> CableType { get; set; }
        public string SelecteditemCableType { get; set; }
        //public ICollection<string> GroupNumber { get; set; }
        public string GroupNumber { get; set; }
        public string CableLength { get; set; }
    }
}
