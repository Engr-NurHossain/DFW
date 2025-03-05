using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class TrackingNumberRecorded 
	{
		public string CallerName { get; set; }
		public string LocationName { get; set; }
		public WebsiteLocation WebsiteLocation { get; set; }
	}
}
