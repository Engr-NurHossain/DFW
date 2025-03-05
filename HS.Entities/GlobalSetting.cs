using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class GlobalSetting 
	{
        //public List<GlobalSetting> globalsettings { get; set; }
		public bool Techval { get; set; }
        public List<int> mulselectId { get; set; }

    }
}
