using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TaskNoteList", Namespace = "http://www.hims-tech.com//list")]	
	public class TaskNoteList : BaseCollection<TaskNote>
	{
		#region Constructors
	    public TaskNoteList() : base() { }
        public TaskNoteList(TaskNote[] list) : base(list) { }
        public TaskNoteList(List<TaskNote> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
