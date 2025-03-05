using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "QA1ScriptList", Namespace = "http://www.hims-tech.com//list")]	
	public class QA1ScriptList : BaseCollection<QA1Script>
	{
		#region Constructors
	    public QA1ScriptList() : base() { }
        public QA1ScriptList(QA1Script[] list) : base(list) { }
        public QA1ScriptList(List<QA1Script> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
