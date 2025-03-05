using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CancellationReasonList", Namespace = "http://www.piistech.com//list")]	
	public class CancellationReasonList : BaseCollection<CancellationReason>
	{
		#region Constructors
	    public CancellationReasonList() : base() { }
        public CancellationReasonList(CancellationReason[] list) : base(list) { }
        public CancellationReasonList(List<CancellationReason> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

