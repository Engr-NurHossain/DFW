using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomSurveyQuestionList", Namespace = "http://www.piistech.com//list")]	
	public class CustomSurveyQuestionList : BaseCollection<CustomSurveyQuestion>
	{
		#region Constructors
	    public CustomSurveyQuestionList() : base() { }
        public CustomSurveyQuestionList(CustomSurveyQuestion[] list) : base(list) { }
        public CustomSurveyQuestionList(List<CustomSurveyQuestion> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

