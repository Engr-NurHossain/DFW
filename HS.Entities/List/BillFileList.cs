using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BillFileList", Namespace = "http://www.piistech.com//list")]	
	public class BillFileList : BaseCollection<BillFile>
	{
		#region Constructors
	    public BillFileList() : base() { }
        public BillFileList(BillFile[] list) : base(list) { }
        public BillFileList(List<BillFile> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

