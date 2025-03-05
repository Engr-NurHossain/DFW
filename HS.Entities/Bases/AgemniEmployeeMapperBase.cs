using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AgemniEmployeeMapperBase", Namespace = "http://www.piistech.com//entities")]
	public class AgemniEmployeeMapperBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			ID = 0,
			AgID = 1,
			RMRID = 2
		}
		#endregion
	
		#region Constants
		public const string Property_ID = "ID";		            
		public const string Property_AgID = "AgID";		            
		public const string Property_RMRID = "RMRID";		            
		#endregion
		
		#region Private Data Types
		private Int32 _ID;	            
		private Nullable<Int32> _AgID;	            
		private Guid _RMRID;	            
		#endregion
		
		#region Properties		
		[DataMember]
		public Int32 ID
		{	
			get{ return _ID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ID, value, _ID);
				if (PropertyChanging(args))
				{
					_ID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AgID
		{	
			get{ return _AgID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AgID, value, _AgID);
				if (PropertyChanging(args))
				{
					_AgID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid RMRID
		{	
			get{ return _RMRID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RMRID, value, _RMRID);
				if (PropertyChanging(args))
				{
					_RMRID = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AgemniEmployeeMapperBase Clone()
		{
			AgemniEmployeeMapperBase newObj = new  AgemniEmployeeMapperBase();
			base.CloneBase(newObj);
			newObj.ID = this.ID;						
			newObj.AgID = this.AgID;						
			newObj.RMRID = this.RMRID;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AgemniEmployeeMapperBase.Property_ID, ID);				
			info.AddValue(AgemniEmployeeMapperBase.Property_AgID, AgID);				
			info.AddValue(AgemniEmployeeMapperBase.Property_RMRID, RMRID);				
		}
		#endregion

		
	}
}
