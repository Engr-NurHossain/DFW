using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "FileTemplateList", Namespace = "http://www.piistech.com//list")]	
	public class FileTemplateList : BaseCollection<FileTemplate>
	{
		#region Constructors
	    public FileTemplateList() : base() { }
        public FileTemplateList(FileTemplate[] list) : base(list) { }
        public FileTemplateList(List<FileTemplate> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

