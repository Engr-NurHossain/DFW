using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SocialMediaContentList", Namespace = "http://www.hims-tech.com//list")]	
	public class SocialMediaContentList : BaseCollection<SocialMediaContent>
	{
		#region Constructors
	    public SocialMediaContentList() : base() { }
        public SocialMediaContentList(SocialMediaContent[] list) : base(list) { }
        public SocialMediaContentList(List<SocialMediaContent> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
