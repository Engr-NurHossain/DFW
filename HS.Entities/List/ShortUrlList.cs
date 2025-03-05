using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ShortUrlList", Namespace = "http://www.piistech.com//list")]	
	public class ShortUrlList : BaseCollection<ShortUrl>
	{
		#region Constructors
	    public ShortUrlList() : base() { }
        public ShortUrlList(ShortUrl[] list) : base(list) { }
        public ShortUrlList(List<ShortUrl> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

