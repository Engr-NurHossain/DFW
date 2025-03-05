using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class HrDoc 
	{
        public string CreatedByName { get; set; }
        public string DocCategory { get; set; }
    }
   
}
