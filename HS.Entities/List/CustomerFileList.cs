using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerFileList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerFileList : BaseCollection<CustomerFile>
	{
		#region Constructors
	    public CustomerFileList() : base() { }
        public CustomerFileList(CustomerFile[] list) : base(list) { }
        public CustomerFileList(List<CustomerFile> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

