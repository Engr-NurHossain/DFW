using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeeOccurences 
	{
		private string _StrOccurenceDate { set; get; }
        public string StrOccurenceDate { get { return _StrOccurenceDate; } set
            {
                this.OccurenceDate = value.ToDateTime();
            } }

    }
}
