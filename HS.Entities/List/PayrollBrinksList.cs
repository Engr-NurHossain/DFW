using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollBrinksList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollBrinksList : BaseCollection<PayrollBrinks>
	{
		#region Constructors
	    public PayrollBrinksList() : base() { }
        public PayrollBrinksList(PayrollBrinks[] list) : base(list) { }
        public PayrollBrinksList(List<PayrollBrinks> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
