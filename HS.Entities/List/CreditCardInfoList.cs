using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CreditCardInfoList", Namespace = "http://www.piistech.com//list")]	
	public class CreditCardInfoList : BaseCollection<CreditCardInfo>
	{
		#region Constructors
	    public CreditCardInfoList() : base() { }
        public CreditCardInfoList(CreditCardInfo[] list) : base(list) { }
        public CreditCardInfoList(List<CreditCardInfo> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

