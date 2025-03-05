using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PanelTypeList", Namespace = "http://www.piistech.com//list")]	
	public class PanelTypeList : BaseCollection<PanelType>
	{
		#region Constructors
	    public PanelTypeList() : base() { }
        public PanelTypeList(PanelType[] list) : base(list) { }
        public PanelTypeList(List<PanelType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

