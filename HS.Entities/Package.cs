using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class Package 
	{
		public MMRRange MMRRange { get; set; }
        public double MMRMax { get; set; }
        public double MMRMin { get; set; }
    }
}
