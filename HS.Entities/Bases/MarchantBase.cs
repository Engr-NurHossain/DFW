﻿using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MarchantBase", Namespace = "http://www.piistech.com//entities")]
	public class MarchantBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			OrderBy = 3,
			IsActive = 4,
			IsDefault = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_IsDefault = "IsDefault";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private Nullable<Int32> _OrderBy;	            
		private Nullable<Boolean> _IsActive;	            
		private Nullable<Boolean> _IsDefault;	            
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
		public Nullable<Int32> OrderBy
		{	
			get{ return _OrderBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderBy, value, _OrderBy);
				if (PropertyChanging(args))
				{
					_OrderBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDefault
		{	
			get{ return _IsDefault; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDefault, value, _IsDefault);
				if (PropertyChanging(args))
				{
					_IsDefault = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  MarchantBase Clone()
		{
			MarchantBase newObj = new  MarchantBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.OrderBy = this.OrderBy;						
			newObj.IsActive = this.IsActive;						
			newObj.IsDefault = this.IsDefault;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MarchantBase.Property_Id, Id);				
			info.AddValue(MarchantBase.Property_CompanyId, CompanyId);				
			info.AddValue(MarchantBase.Property_Name, Name);				
			info.AddValue(MarchantBase.Property_OrderBy, OrderBy);				
			info.AddValue(MarchantBase.Property_IsActive, IsActive);				
			info.AddValue(MarchantBase.Property_IsDefault, IsDefault);				
		}
		#endregion

		
	}
}
