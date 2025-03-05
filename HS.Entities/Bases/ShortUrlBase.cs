using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ShortUrlBase", Namespace = "http://www.piistech.com//entities")]
	public class ShortUrlBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			Code = 2,
			Url = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Code = "Code";		            
		public const string Property_Url = "Url";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _Code;	            
		private String _Url;	            
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
		public Guid CustomerId
		{	
			get{ return _CustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerId, value, _CustomerId);
				if (PropertyChanging(args))
				{
					_CustomerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Code
		{	
			get{ return _Code; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Code, value, _Code);
				if (PropertyChanging(args))
				{
					_Code = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Url
		{	
			get{ return _Url; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Url, value, _Url);
				if (PropertyChanging(args))
				{
					_Url = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ShortUrlBase Clone()
		{
			ShortUrlBase newObj = new  ShortUrlBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Code = this.Code;						
			newObj.Url = this.Url;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ShortUrlBase.Property_Id, Id);				
			info.AddValue(ShortUrlBase.Property_CustomerId, CustomerId);				
			info.AddValue(ShortUrlBase.Property_Code, Code);				
			info.AddValue(ShortUrlBase.Property_Url, Url);				
		}
		#endregion

		
	}
}
