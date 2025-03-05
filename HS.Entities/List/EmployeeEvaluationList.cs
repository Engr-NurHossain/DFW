using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeEvaluationList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeEvaluationList : BaseCollection<EmployeeEvaluation>
	{
		#region Constructors
	    public EmployeeEvaluationList() : base() { }
        public EmployeeEvaluationList(EmployeeEvaluation[] list) : base(list) { }
        public EmployeeEvaluationList(List<EmployeeEvaluation> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

