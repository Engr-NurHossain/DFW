using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgebaseAccountabilityList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgebaseAccountabilityList : BaseCollection<KnowledgebaseAccountability>
	{
		#region Constructors
	    public KnowledgebaseAccountabilityList() : base() { }
        public KnowledgebaseAccountabilityList(KnowledgebaseAccountability[] list) : base(list) { }
        public KnowledgebaseAccountabilityList(List<KnowledgebaseAccountability> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
