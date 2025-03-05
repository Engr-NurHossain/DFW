using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BookingExtraItemList", Namespace = "http://www.piistech.com//list")]	
	public class BookingExtraItemList : BaseCollection<BookingExtraItem>
	{
		#region Constructors
	    public BookingExtraItemList() : base() { }
        public BookingExtraItemList(BookingExtraItem[] list) : base(list) { }
        public BookingExtraItemList(List<BookingExtraItem> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

