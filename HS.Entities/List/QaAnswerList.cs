using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "QaAnswerList", Namespace = "http://www.piistech.com//list")]	
	public class QaAnswerList : BaseCollection<QaAnswer>
	{
		#region Constructors
	    public QaAnswerList() : base() { }
        public QaAnswerList(QaAnswer[] list) : base(list) { }
        public QaAnswerList(List<QaAnswer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

