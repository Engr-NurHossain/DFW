using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class RecruitmentDocForm 
	{
		public bool IsPdf { set; get; }
        public string FormFor { set; get; }
	}
}
