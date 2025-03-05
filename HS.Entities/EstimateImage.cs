using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EstimateImage 
	{
		public string StrUploadedDate { get; set; }
		public string Extension { get; set; }
	}
}
