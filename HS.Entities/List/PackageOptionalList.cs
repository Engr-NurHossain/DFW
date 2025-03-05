using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageOptionalList", Namespace = "http://www.piistech.com//list")]	
	public class PackageOptionalList : BaseCollection<PackageOptional>
	{
		#region Constructors
	    public PackageOptionalList() : base() { }
        public PackageOptionalList(PackageOptional[] list) : base(list) { }
        public PackageOptionalList(List<PackageOptional> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

