using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerNoteList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerNoteList : BaseCollection<CustomerNote>
	{
		#region Constructors
	    public CustomerNoteList() : base() { }
        public CustomerNoteList(CustomerNote[] list) : base(list) { }
        public CustomerNoteList(List<CustomerNote> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

