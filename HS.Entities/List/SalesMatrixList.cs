using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SalesMatrixList", Namespace = "http://www.piistech.com//list")]	
	public class SalesMatrixList : BaseCollection<SalesMatrix>
	{
		#region Constructors
	    public SalesMatrixList() : base() { }
        public SalesMatrixList(SalesMatrix[] list) : base(list) { }
        public SalesMatrixList(List<SalesMatrix> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

