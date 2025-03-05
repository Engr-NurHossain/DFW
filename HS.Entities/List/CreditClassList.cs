using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CreditClassList", Namespace = "http://www.piistech.com//list")]	
	public class CreditClassList : BaseCollection<CreditClass>
	{
		#region Constructors
	    public CreditClassList() : base() { }
        public CreditClassList(CreditClass[] list) : base(list) { }
        public CreditClassList(List<CreditClass> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

