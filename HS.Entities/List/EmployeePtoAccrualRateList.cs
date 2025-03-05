using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeePtoAccrualRateList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeePtoAccrualRateList : BaseCollection<EmployeePtoAccrualRate>
	{
		#region Constructors
	    public EmployeePtoAccrualRateList() : base() { }
        public EmployeePtoAccrualRateList(EmployeePtoAccrualRate[] list) : base(list) { }
        public EmployeePtoAccrualRateList(List<EmployeePtoAccrualRate> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
