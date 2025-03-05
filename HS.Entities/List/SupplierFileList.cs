using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SupplierFileList", Namespace = "http://www.piistech.com//list")]	
	public class SupplierFileList : BaseCollection<SupplierFile>
	{
		#region Constructors
	    public SupplierFileList() : base() { }
        public SupplierFileList(SupplierFile[] list) : base(list) { }
        public SupplierFileList(List<SupplierFile> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

