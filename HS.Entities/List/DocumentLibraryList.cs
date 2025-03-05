using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "DocumentLibraryList", Namespace = "http://www.hims-tech.com//list")]	
	public class DocumentLibraryList : BaseCollection<DocumentLibrary>
	{
		#region Constructors
	    public DocumentLibraryList() : base() { }
        public DocumentLibraryList(DocumentLibrary[] list) : base(list) { }
        public DocumentLibraryList(List<DocumentLibrary> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
