using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Manufacturer 
	{
        public int SystemId { get; set; }
    }
    public partial class ManufacturerModelWithPaging
    {
        public List<Manufacturer> Manufacturerlist { get; set; }

        public int TotalCount { get; set; }

        public int ManufacturerCount { get; set; }
    }
}
