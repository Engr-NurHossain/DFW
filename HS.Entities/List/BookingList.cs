using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BookingList", Namespace = "http://www.piistech.com//list")]	
	public class BookingList : BaseCollection<Booking>
	{
		#region Constructors
	    public BookingList() : base() { }
        public BookingList(Booking[] list) : base(list) { }
        public BookingList(List<Booking> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

