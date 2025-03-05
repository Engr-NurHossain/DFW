using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgebaseWeblinkList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgebaseWeblinkList : BaseCollection<KnowledgebaseWeblink>
	{
		#region Constructors
	    public KnowledgebaseWeblinkList() : base() { }
        public KnowledgebaseWeblinkList(KnowledgebaseWeblink[] list) : base(list) { }
        public KnowledgebaseWeblinkList(List<KnowledgebaseWeblink> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
