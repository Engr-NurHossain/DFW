using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollSingleItemSettingsList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollSingleItemSettingsList : BaseCollection<PayrollSingleItemSettings>
	{
		#region Constructors
	    public PayrollSingleItemSettingsList() : base() { }
        public PayrollSingleItemSettingsList(PayrollSingleItemSettings[] list) : base(list) { }
        public PayrollSingleItemSettingsList(List<PayrollSingleItemSettings> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
