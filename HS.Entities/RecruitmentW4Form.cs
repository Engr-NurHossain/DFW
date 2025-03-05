using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class RecruitmentW4Form 
	{
		public bool IsPdf { set; get; }
        public string Ipadd { get; set; }
        public string UserAg { get; set; }
        public string Tstamp { get; set; }
    }
}
