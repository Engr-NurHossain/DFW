using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmailTemplate 
	{
        public class SurveyEmail
        {
            public string Name { set; get; }
            public string SurveyLink { set; get; }
            public string ToEmail { set; get; }
        }
        
    }
}
