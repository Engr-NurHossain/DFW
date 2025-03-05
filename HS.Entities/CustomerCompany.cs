using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class CustomerCompany 
	{
		public List<AgreementQuestion> AgreementQuestion { get; set; }
        public List<AgreementAnswer> AgreementAnswer { get; set; }

		public Customer Customer { get; set; }
    }
}
