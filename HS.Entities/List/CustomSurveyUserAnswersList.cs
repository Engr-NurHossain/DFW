using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomSurveyUserAnswersList", Namespace = "http://www.piistech.com//list")]	
	public class CustomSurveyUserAnswersList : BaseCollection<CustomSurveyUserAnswers>
	{
		#region Constructors
	    public CustomSurveyUserAnswersList() : base() { }
        public CustomSurveyUserAnswersList(CustomSurveyUserAnswers[] list) : base(list) { }
        public CustomSurveyUserAnswersList(List<CustomSurveyUserAnswers> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

