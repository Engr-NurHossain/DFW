using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgebaseRMRTagMapList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgebaseRMRTagMapList : BaseCollection<KnowledgebaseRMRTagMap>
	{
		#region Constructors
	    public KnowledgebaseRMRTagMapList() : base() { }
        public KnowledgebaseRMRTagMapList(KnowledgebaseRMRTagMap[] list) : base(list) { }
        public KnowledgebaseRMRTagMapList(List<KnowledgebaseRMRTagMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
