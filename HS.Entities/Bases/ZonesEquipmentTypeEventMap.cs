using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	[Serializable]
    [DataContract(Name = "ZonesEquipmentTypeEventMap", Namespace = "http://www.hims-tech.com//entities")]
	public partial class ZonesEquipmentTypeEventMap : ZonesEquipmentTypeEventMapBase
	{
		#region Exernal Properties
		#endregion
		
		#region Orverride Equals
		public override bool Equals(Object obj)		
		{
			if (obj.GetType() != typeof(ZonesEquipmentTypeEventMap))
            {
                return false;
            }			
			
			 ZonesEquipmentTypeEventMap _paramObj = obj as ZonesEquipmentTypeEventMap;
            if (_paramObj != null)
            {			
                return (_paramObj.ID == this.ID && _paramObj.CustomPropertyMatch(this));
            }
            else
            {
                return base.Equals(obj);
            }
		}
		#endregion
		
		#region Orverride HashCode
		 public override int GetHashCode()
        {
            return base.ID.GetHashCode();
        }
		#endregion		
	}
}
