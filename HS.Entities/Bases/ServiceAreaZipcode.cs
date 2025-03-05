using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
    [Serializable]
    [DataContract(Name = "ServiceAreaZipcode", Namespace = "http://www.piistech.com//entities")]
    public partial class ServiceAreaZipcode : ServiceAreaZipcodeBase
    {
        #region Exernal Properties
        #endregion

        #region Orverride Equals
        public override bool Equals(Object obj)
        {
            if (obj.GetType() != typeof(AccountHolder))
            {
                return false;
            }

            AccountHolder _paramObj = obj as AccountHolder;
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


