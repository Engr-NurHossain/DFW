using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgebaseAccessedHistoryList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgebaseAccessedHistoryList : BaseCollection<KnowledgebaseAccessedHistory>
	{
		#region Constructors
	    public KnowledgebaseAccessedHistoryList() : base() { }
        public KnowledgebaseAccessedHistoryList(KnowledgebaseAccessedHistory[] list) : base(list) { }
        public KnowledgebaseAccessedHistoryList(List<KnowledgebaseAccessedHistory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
