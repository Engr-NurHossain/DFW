using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "DocumentLibraryWeblinkList", Namespace = "http://www.piistech.com//list")]	
	public class DocumentLibraryWeblinkList : BaseCollection<DocumentLibraryWeblink>
	{
		#region Constructors
	    public DocumentLibraryWeblinkList() : base() { }
        public DocumentLibraryWeblinkList(DocumentLibraryWeblink[] list) : base(list) { }
        public DocumentLibraryWeblinkList(List<DocumentLibraryWeblink> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
