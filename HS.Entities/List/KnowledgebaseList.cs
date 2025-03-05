using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgebaseList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgebaseList : BaseCollection<Knowledgebase>
	{
		#region Constructors
	    public KnowledgebaseList() : base() { }
        public KnowledgebaseList(Knowledgebase[] list) : base(list) { }
        public KnowledgebaseList(List<Knowledgebase> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
