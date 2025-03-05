using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class WebsiteLocation 
	{
		public List<string> AvailableDays { get; set; }
		public bool CloseType { get; set; }
		public bool NewLocation { get; set; }
		public List<string> ListNeighborhood { get; set; }
		public string Neighbarhood { get; set; }
	}
}
