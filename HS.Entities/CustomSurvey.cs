using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class CustomSurvey 
	{
        public string CreatedByName { get; set; }
        public int QuesCount { get; set; }
    }
    public class CustomSurveyViewModel
    {
        public CustomSurveyUser CustomSurveyUser { set; get; }
        public CustomSurvey CustomSurvey { set; get; }
        public Customer _customer { get; set; }
        public List<CustomSurveyQuestion> SurveyQuestions { set; get; }
        public List<CustomSurveyAnswer> SurveyAnswers { set; get; }
        public List<CustomSurveyUserAnswers> UserAnswers { set; get; }

     
    }
}
