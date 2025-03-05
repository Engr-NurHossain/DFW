using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AgreementQuestionList", Namespace = "http://www.piistech.com//list")]	
	public class AgreementQuestionList : BaseCollection<AgreementQuestion>
	{
		#region Constructors
	    public AgreementQuestionList() : base() { }
        public AgreementQuestionList(AgreementQuestion[] list) : base(list) { }
        public AgreementQuestionList(List<AgreementQuestion> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

