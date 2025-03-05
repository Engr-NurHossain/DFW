using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CityZipCodeSearchLogBase", Namespace = "http://www.piistech.com//entities")]
	public class CityZipCodeSearchLogBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AppName = 1,
			UserIP = 2,
			SearchText = 3,
			SearchDate = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AppName = "AppName";		            
		public const string Property_UserIP = "UserIP";		            
		public const string Property_SearchText = "SearchText";		            
		public const string Property_SearchDate = "SearchDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _AppName;	            
		private String _UserIP;	            
		private String _SearchText;	            
		private Nullable<DateTime> _SearchDate;	            
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
		public String AppName
		{	
			get{ return _AppName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppName, value, _AppName);
				if (PropertyChanging(args))
				{
					_AppName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserIP
		{	
			get{ return _UserIP; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserIP, value, _UserIP);
				if (PropertyChanging(args))
				{
					_UserIP = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SearchText
		{	
			get{ return _SearchText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SearchText, value, _SearchText);
				if (PropertyChanging(args))
				{
					_SearchText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SearchDate
		{	
			get{ return _SearchDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SearchDate, value, _SearchDate);
				if (PropertyChanging(args))
				{
					_SearchDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CityZipCodeSearchLogBase Clone()
		{
			CityZipCodeSearchLogBase newObj = new  CityZipCodeSearchLogBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AppName = this.AppName;						
			newObj.UserIP = this.UserIP;						
			newObj.SearchText = this.SearchText;						
			newObj.SearchDate = this.SearchDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CityZipCodeSearchLogBase.Property_Id, Id);				
			info.AddValue(CityZipCodeSearchLogBase.Property_AppName, AppName);				
			info.AddValue(CityZipCodeSearchLogBase.Property_UserIP, UserIP);				
			info.AddValue(CityZipCodeSearchLogBase.Property_SearchText, SearchText);				
			info.AddValue(CityZipCodeSearchLogBase.Property_SearchDate, SearchDate);				
		}
		#endregion

		
	}
}
