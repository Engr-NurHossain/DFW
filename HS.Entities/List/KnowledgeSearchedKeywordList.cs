using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgeSearchedKeywordList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgeSearchedKeywordList : BaseCollection<KnowledgeSearchedKeyword>
	{
		#region Constructors
	    public KnowledgeSearchedKeywordList() : base() { }
        public KnowledgeSearchedKeywordList(KnowledgeSearchedKeyword[] list) : base(list) { }
        public KnowledgeSearchedKeywordList(List<KnowledgeSearchedKeyword> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
