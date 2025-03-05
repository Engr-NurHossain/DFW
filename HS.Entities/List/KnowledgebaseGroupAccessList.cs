using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgebaseGroupAccessList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgebaseGroupAccessList : BaseCollection<KnowledgebaseGroupAccess>
	{
		#region Constructors
	    public KnowledgebaseGroupAccessList() : base() { }
        public KnowledgebaseGroupAccessList(KnowledgebaseGroupAccess[] list) : base(list) { }
        public KnowledgebaseGroupAccessList(List<KnowledgebaseGroupAccess> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
