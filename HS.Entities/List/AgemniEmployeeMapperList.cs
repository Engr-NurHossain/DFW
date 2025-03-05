using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AgemniEmployeeMapperList", Namespace = "http://www.piistech.com//list")]	
	public class AgemniEmployeeMapperList : BaseCollection<AgemniEmployeeMapper>
	{
		#region Constructors
	    public AgemniEmployeeMapperList() : base() { }
        public AgemniEmployeeMapperList(AgemniEmployeeMapper[] list) : base(list) { }
        public AgemniEmployeeMapperList(List<AgemniEmployeeMapper> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

