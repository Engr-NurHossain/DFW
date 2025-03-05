using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SmartSystemTypeList", Namespace = "http://www.piistech.com//list")]	
	public class SmartSystemTypeList : BaseCollection<SmartSystemType>
	{
		#region Constructors
	    public SmartSystemTypeList() : base() { }
        public SmartSystemTypeList(SmartSystemType[] list) : base(list) { }
        public SmartSystemTypeList(List<SmartSystemType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

