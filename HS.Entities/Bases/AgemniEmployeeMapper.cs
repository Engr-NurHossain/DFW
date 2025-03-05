using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	[Serializable]
    [DataContract(Name = "AgemniEmployeeMapper", Namespace = "http://www.piistech.com//entities")]
	public partial class AgemniEmployeeMapper : AgemniEmployeeMapperBase
	{
		#region Exernal Properties
		#endregion
		
		#region Orverride Equals
		public override bool Equals(Object obj)		
		{
			if (obj.GetType() != typeof(AgemniEmployeeMapper))
            {
                return false;
            }			
			
			 AgemniEmployeeMapper _paramObj = obj as AgemniEmployeeMapper;
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
