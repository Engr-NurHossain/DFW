using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class PanelType 
	{
		public string EquipName { get; set; }
        public Equipment Equipment { get; set; }
    }
}
