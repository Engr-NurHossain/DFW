using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TrackingNumberRecordedList", Namespace = "http://www.hims-tech.com//list")]	
	public class TrackingNumberRecordedList : BaseCollection<TrackingNumberRecorded>
	{
		#region Constructors
	    public TrackingNumberRecordedList() : base() { }
        public TrackingNumberRecordedList(TrackingNumberRecorded[] list) : base(list) { }
        public TrackingNumberRecordedList(List<TrackingNumberRecorded> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
