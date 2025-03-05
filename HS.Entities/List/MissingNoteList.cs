using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MissingNoteList", Namespace = "http://www.hims-tech.com//list")]	
	public class MissingNoteList : BaseCollection<MissingNote>
	{
		#region Constructors
	    public MissingNoteList() : base() { }
        public MissingNoteList(MissingNote[] list) : base(list) { }
        public MissingNoteList(List<MissingNote> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
