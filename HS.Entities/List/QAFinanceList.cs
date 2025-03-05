using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "QAFinanceList", Namespace = "http://www.hims-tech.com//list")]	
	public class QAFinanceList : BaseCollection<QAFinance>
	{
		#region Constructors
	    public QAFinanceList() : base() { }
        public QAFinanceList(QAFinance[] list) : base(list) { }
        public QAFinanceList(List<QAFinance> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
