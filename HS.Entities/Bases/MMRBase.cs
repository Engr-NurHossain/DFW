using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MMRBase", Namespace = "http://www.piistech.com//entities")]
	public class MMRBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			Value = 3,
			IsActivve = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_Value = "Value";		            
		public const string Property_IsActivve = "IsActivve";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private Nullable<Double> _Value;	            
		private Nullable<Boolean> _IsActivve;	            
		#endregion
		
		#region Properties		
		[DataMember]
		public Int32 Id
		{	
			get{ return _Id; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Id, value, _Id);
				if (PropertyChanging(args))
				{
					_Id = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CompanyId
		{	
			get{ return _CompanyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyId, value, _CompanyId);
				if (PropertyChanging(args))
				{
					_CompanyId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Value
		{	
			get{ return _Value; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Value, value, _Value);
				if (PropertyChanging(args))
				{
					_Value = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActivve
		{	
			get{ return _IsActivve; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActivve, value, _IsActivve);
				if (PropertyChanging(args))
				{
					_IsActivve = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  MMRBase Clone()
		{
			MMRBase newObj = new  MMRBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.Value = this.Value;						
			newObj.IsActivve = this.IsActivve;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MMRBase.Property_Id, Id);				
			info.AddValue(MMRBase.Property_CompanyId, CompanyId);				
			info.AddValue(MMRBase.Property_Name, Name);				
			info.AddValue(MMRBase.Property_Value, Value);				
			info.AddValue(MMRBase.Property_IsActivve, IsActivve);				
		}
		#endregion

		
	}
}
