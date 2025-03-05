using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentReturnNoteList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentReturnNoteList : BaseCollection<EquipmentReturnNote>
	{
		#region Constructors
	    public EquipmentReturnNoteList() : base() { }
        public EquipmentReturnNoteList(EquipmentReturnNote[] list) : base(list) { }
        public EquipmentReturnNoteList(List<EquipmentReturnNote> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

