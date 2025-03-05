using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeNoteList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeNoteList : BaseCollection<EmployeeNote>
	{
		#region Constructors
	    public EmployeeNoteList() : base() { }
        public EmployeeNoteList(EmployeeNote[] list) : base(list) { }
        public EmployeeNoteList(List<EmployeeNote> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

