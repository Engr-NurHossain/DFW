using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "QaQuestionList", Namespace = "http://www.piistech.com//list")]	
	public class QaQuestionList : BaseCollection<QaQuestion>
	{
		#region Constructors
	    public QaQuestionList() : base() { }
        public QaQuestionList(QaQuestion[] list) : base(list) { }
        public QaQuestionList(List<QaQuestion> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

