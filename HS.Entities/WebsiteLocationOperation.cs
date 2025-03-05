using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class WebsiteLocationOperation 
	{
		public string OperationStartTimeVal { get; set; }
        public string OperationEndTimeVal { get; set; }
		public string StoreOperationStartTimeVal { get; set; }
		public string StoreOperationEndTimeVal { get; set; }
	}
}
