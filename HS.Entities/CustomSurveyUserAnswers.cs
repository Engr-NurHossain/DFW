using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomSurveyUserAnswers 
	{
        public string UserAnswerText { set; get; }
        public string SignPath { get; set; }
	}
}
