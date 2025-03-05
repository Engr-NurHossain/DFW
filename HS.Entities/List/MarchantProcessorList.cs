using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MarchantProcessorList", Namespace = "http://www.piistech.com//list")]	
	public class MarchantProcessorList : BaseCollection<MarchantProcessor>
	{
		#region Constructors
	    public MarchantProcessorList() : base() { }
        public MarchantProcessorList(MarchantProcessor[] list) : base(list) { }
        public MarchantProcessorList(List<MarchantProcessor> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

