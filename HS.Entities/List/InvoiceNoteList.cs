using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "InvoiceNoteList", Namespace = "http://www.piistech.com//list")]	
	public class InvoiceNoteList : BaseCollection<InvoiceNote>
	{
		#region Constructors
	    public InvoiceNoteList() : base() { }
        public InvoiceNoteList(InvoiceNote[] list) : base(list) { }
        public InvoiceNoteList(List<InvoiceNote> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

