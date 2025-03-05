using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class Commision 
	{
		public string TypeName { get; set; }
        public string SessionName { get; set; }
    }
}
