using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomSurveyUserList", Namespace = "http://www.piistech.com//list")]	
	public class CustomSurveyUserList : BaseCollection<CustomSurveyUser>
	{
		#region Constructors
	    public CustomSurveyUserList() : base() { }
        public CustomSurveyUserList(CustomSurveyUser[] list) : base(list) { }
        public CustomSurveyUserList(List<CustomSurveyUser> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

