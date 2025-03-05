using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class LocalizeResource 
	{
		
	}
    public class LocalizeResourceViewModel
    {
        public List<LocalizeResource> LocalizeResource { set; get; }
        public int TotalCount { set; get; }

    }
}
