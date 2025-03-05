using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	[Serializable]
    [DataContract(Name = "MenuItemCategoryURL", Namespace = "http://www.hims-tech.com//entities")]
	public partial class MenuItemCategoryURL : MenuItemCategoryURLBase
	{
		#region Exernal Properties
		#endregion
		
		#region Orverride Equals
		public override bool Equals(Object obj)		
		{
			if (obj.GetType() != typeof(MenuItemCategoryURL))
            {
                return false;
            }			
			
			 MenuItemCategoryURL _paramObj = obj as MenuItemCategoryURL;
            if (_paramObj != null)
            {			
                return (_paramObj.Id == this.Id && _paramObj.CustomPropertyMatch(this));
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
            return base.Id.GetHashCode();
        }
		#endregion		
	}
}
