using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomSurveyList", Namespace = "http://www.piistech.com//list")]	
	public class CustomSurveyList : BaseCollection<CustomSurvey>
	{
		#region Constructors
	    public CustomSurveyList() : base() { }
        public CustomSurveyList(CustomSurvey[] list) : base(list) { }
        public CustomSurveyList(List<CustomSurvey> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

