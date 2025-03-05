using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AdjustmentSchemeList", Namespace = "http://www.piistech.com//list")]	
	public class AdjustmentSchemeList : BaseCollection<AdjustmentScheme>
	{
		#region Constructors
	    public AdjustmentSchemeList() : base() { }
        public AdjustmentSchemeList(AdjustmentScheme[] list) : base(list) { }
        public AdjustmentSchemeList(List<AdjustmentScheme> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

