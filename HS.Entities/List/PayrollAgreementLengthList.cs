using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollAgreementLengthList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollAgreementLengthList : BaseCollection<PayrollAgreementLength>
	{
		#region Constructors
	    public PayrollAgreementLengthList() : base() { }
        public PayrollAgreementLengthList(PayrollAgreementLength[] list) : base(list) { }
        public PayrollAgreementLengthList(List<PayrollAgreementLength> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
