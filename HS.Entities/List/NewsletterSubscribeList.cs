using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "NewsletterSubscribeList", Namespace = "http://www.piistech.com//list")]	
	public class NewsletterSubscribeList : BaseCollection<NewsletterSubscribe>
	{
		#region Constructors
	    public NewsletterSubscribeList() : base() { }
        public NewsletterSubscribeList(NewsletterSubscribe[] list) : base(list) { }
        public NewsletterSubscribeList(List<NewsletterSubscribe> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

