using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class UserPermission 
	{
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
    }
}
