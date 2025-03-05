using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class GridSetting 
	{

    }

    public partial class FinalGrid
	{
		public List<GridSetting> GridSetting { get; set; }
		public List<GridSetting> GridGroupSetting { get; set; }


	}
}
