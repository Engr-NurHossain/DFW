﻿using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SalesComissionList", Namespace = "http://www.piistech.com//list")]	
	public class SalesComissionList : BaseCollection<SalesComission>
	{
		#region Constructors
	    public SalesComissionList() : base() { }
        public SalesComissionList(SalesComission[] list) : base(list) { }
        public SalesComissionList(List<SalesComission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

