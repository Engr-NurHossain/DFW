using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomSurveyAnswerList", Namespace = "http://www.piistech.com//list")]	
	public class CustomSurveyAnswerList : BaseCollection<CustomSurveyAnswer>
	{
		#region Constructors
	    public CustomSurveyAnswerList() : base() { }
        public CustomSurveyAnswerList(CustomSurveyAnswer[] list) : base(list) { }
        public CustomSurveyAnswerList(List<CustomSurveyAnswer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

