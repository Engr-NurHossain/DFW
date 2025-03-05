using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class NoteAssign 
	{
        public string AssignName { get; set; }
        public string[] EmployeeIdVal { get; set; }
        public string[] AssignEmployeeIdArray { get; set; }
    }
}
