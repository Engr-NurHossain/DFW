using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CalendarEmployeeDataMapperList", Namespace = "http://www.hims-tech.com//list")]	
	public class CalendarEmployeeDataMapperList : BaseCollection<CalendarEmployeeDataMapper>
	{
		#region Constructors
	    public CalendarEmployeeDataMapperList() : base() { }
        public CalendarEmployeeDataMapperList(CalendarEmployeeDataMapper[] list) : base(list) { }
        public CalendarEmployeeDataMapperList(List<CalendarEmployeeDataMapper> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
