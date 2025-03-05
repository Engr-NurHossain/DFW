using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AgreementAnswerList", Namespace = "http://www.piistech.com//list")]	
	public class AgreementAnswerList : BaseCollection<AgreementAnswer>
	{
		#region Constructors
	    public AgreementAnswerList() : base() { }
        public AgreementAnswerList(AgreementAnswer[] list) : base(list) { }
        public AgreementAnswerList(List<AgreementAnswer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

