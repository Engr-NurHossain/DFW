using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentFileList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentFileList : BaseCollection<EquipmentFile>
	{
		#region Constructors
	    public EquipmentFileList() : base() { }
        public EquipmentFileList(EquipmentFile[] list) : base(list) { }
        public EquipmentFileList(List<EquipmentFile> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

