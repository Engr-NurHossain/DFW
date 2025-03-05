using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerContactTrackList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerContactTrackList : BaseCollection<CustomerContactTrack>
	{
		#region Constructors
	    public CustomerContactTrackList() : base() { }
        public CustomerContactTrackList(CustomerContactTrack[] list) : base(list) { }
        public CustomerContactTrackList(List<CustomerContactTrack> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

