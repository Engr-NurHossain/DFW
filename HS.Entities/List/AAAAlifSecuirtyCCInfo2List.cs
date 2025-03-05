using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AAAAlifSecuirtyCCInfo2List", Namespace = "http://www.piistech.com//list")]	
	public class AAAAlifSecuirtyCCInfo2List : BaseCollection<AAAAlifSecuirtyCCInfo2>
	{
		#region Constructors
	    public AAAAlifSecuirtyCCInfo2List() : base() { }
        public AAAAlifSecuirtyCCInfo2List(AAAAlifSecuirtyCCInfo2[] list) : base(list) { }
        public AAAAlifSecuirtyCCInfo2List(List<AAAAlifSecuirtyCCInfo2> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
