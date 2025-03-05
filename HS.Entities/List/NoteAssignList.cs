using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "NoteAssignList", Namespace = "http://www.piistech.com//list")]	
	public class NoteAssignList : BaseCollection<NoteAssign>
	{
		#region Constructors
	    public NoteAssignList() : base() { }
        public NoteAssignList(NoteAssign[] list) : base(list) { }
        public NoteAssignList(List<NoteAssign> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

