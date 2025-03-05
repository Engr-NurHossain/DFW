using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MarchantList", Namespace = "http://www.piistech.com//list")]	
	public class MarchantList : BaseCollection<Marchant>
	{
		#region Constructors
	    public MarchantList() : base() { }
        public MarchantList(Marchant[] list) : base(list) { }
        public MarchantList(List<Marchant> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

