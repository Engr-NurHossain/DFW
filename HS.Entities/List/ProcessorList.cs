using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ProcessorList", Namespace = "http://www.piistech.com//list")]	
	public class ProcessorList : BaseCollection<Processor>
	{
		#region Constructors
	    public ProcessorList() : base() { }
        public ProcessorList(Processor[] list) : base(list) { }
        public ProcessorList(List<Processor> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

