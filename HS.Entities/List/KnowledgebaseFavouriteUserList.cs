using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "KnowledgebaseFavouriteUserList", Namespace = "http://www.piistech.com//list")]	
	public class KnowledgebaseFavouriteUserList : BaseCollection<KnowledgebaseFavouriteUser>
	{
		#region Constructors
	    public KnowledgebaseFavouriteUserList() : base() { }
        public KnowledgebaseFavouriteUserList(KnowledgebaseFavouriteUser[] list) : base(list) { }
        public KnowledgebaseFavouriteUserList(List<KnowledgebaseFavouriteUser> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
