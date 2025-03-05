using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ErrorLogList", Namespace = "http://www.piistech.com//list")]	
	public class ErrorLogList : BaseCollection<ErrorLog>
	{
		#region Constructors
	    public ErrorLogList() : base() { }
        public ErrorLogList(ErrorLog[] list) : base(list) { }
        public ErrorLogList(List<ErrorLog> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

