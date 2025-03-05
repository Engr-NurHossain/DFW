using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomerAdditionalContact 
	{
       
    }
    public partial class CustomerSystemInformation
    {
        public Int32 Id { get; set; }
        public string Warranty { get; set; }
        public string Keypad { get; set; }
        public string FrontEnd { get; set; }
    }
}
