using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "QA2ScriptList", Namespace = "http://www.hims-tech.com//list")]	
	public class QA2ScriptList : BaseCollection<QA2Script>
	{
		#region Constructors
	    public QA2ScriptList() : base() { }
        public QA2ScriptList(QA2Script[] list) : base(list) { }
        public QA2ScriptList(List<QA2Script> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
