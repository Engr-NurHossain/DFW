using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class LeadCorrespondence 
	{
		public string AssignName { get; set; }
        public string AssociatedType { get; set; }
        public int LeadCorresCount { get; set; }
        public string EmpName { get; set; }
    }
}
