using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomerView 
	{
		public string CustomerViewName { get; set; }
        public int CustomerViewid { get; set; }
        public string LastVisitDate { get; set; }
        public string CustomerViewBussiness { get; set; }
    }
}
