using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class PayrollDetail 
	{
		
	}
    public class PayrollDetailModel
    {
        public List<PayrollDetail> PayrollDetailList { set; get; }
        public Count TotalCount { set; get; }
    }
}
