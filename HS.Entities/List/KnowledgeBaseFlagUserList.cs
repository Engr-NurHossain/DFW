using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgeBaseFlagUserList", Namespace = "http://www.hims-tech.com//list")]	
	public class KnowledgeBaseFlagUserList : BaseCollection<KnowledgeBaseFlagUser>
	{
		#region Constructors
	    public KnowledgeBaseFlagUserList() : base() { }
        public KnowledgeBaseFlagUserList(KnowledgeBaseFlagUser[] list) : base(list) { }
        public KnowledgeBaseFlagUserList(List<KnowledgeBaseFlagUser> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
