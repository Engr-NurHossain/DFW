using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CompanyFileList", Namespace = "http://www.piistech.com//list")]	
	public class CompanyFileList : BaseCollection<CompanyFile>
	{
		#region Constructors
	    public CompanyFileList() : base() { }
        public CompanyFileList(CompanyFile[] list) : base(list) { }
        public CompanyFileList(List<CompanyFile> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

