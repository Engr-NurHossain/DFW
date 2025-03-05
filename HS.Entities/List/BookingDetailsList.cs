using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BookingDetailsList", Namespace = "http://www.piistech.com//list")]	
	public class BookingDetailsList : BaseCollection<BookingDetails>
	{
		#region Constructors
	    public BookingDetailsList() : base() { }
        public BookingDetailsList(BookingDetails[] list) : base(list) { }
        public BookingDetailsList(List<BookingDetails> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

