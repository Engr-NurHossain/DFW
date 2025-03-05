using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CompanyHolidayList", Namespace = "http://www.piistech.com//list")]	
	public class CompanyHolidayList : BaseCollection<CompanyHoliday>
	{
		#region Constructors
	    public CompanyHolidayList() : base() { }
        public CompanyHolidayList(CompanyHoliday[] list) : base(list) { }
        public CompanyHolidayList(List<CompanyHoliday> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
