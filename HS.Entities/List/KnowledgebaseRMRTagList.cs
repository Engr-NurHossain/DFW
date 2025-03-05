using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgebaseRMRTagList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgebaseRMRTagList : BaseCollection<KnowledgebaseRMRTag>
	{
		#region Constructors
	    public KnowledgebaseRMRTagList() : base() { }
        public KnowledgebaseRMRTagList(KnowledgebaseRMRTag[] list) : base(list) { }
        public KnowledgebaseRMRTagList(List<KnowledgebaseRMRTag> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
