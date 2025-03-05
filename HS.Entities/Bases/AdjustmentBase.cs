using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AdjustmentBase", Namespace = "http://www.piistech.com//entities")]
	public class AdjustmentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ComissionSessionId = 1,
			AdjustSchemeId = 2,
			Description = 3,
			Conduit = 4,
			Amount = 5,
			Multiple = 6,
			AppliedTo = 7,
			StartDate = 8,
			EndDate = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ComissionSessionId = "ComissionSessionId";		            
		public const string Property_AdjustSchemeId = "AdjustSchemeId";		            
		public const string Property_Description = "Description";		            
		public const string Property_Conduit = "Conduit";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Multiple = "Multiple";		            
		public const string Property_AppliedTo = "AppliedTo";		            
		public const string Property_StartDate = "StartDate";		            
		public const string Property_EndDate = "EndDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Nullable<Int32> _ComissionSessionId;	            
		private Nullable<Int32> _AdjustSchemeId;	            
		private String _Description;	            
		private String _Conduit;	            
		private Nullable<Double> _Amount;	            
		private Nullable<Double> _Multiple;	            
		private String _AppliedTo;	            
		private Nullable<DateTime> _StartDate;	            
		private Nullable<DateTime> _EndDate;	            
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
		public Nullable<Int32> ComissionSessionId
		{	
			get{ return _ComissionSessionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ComissionSessionId, value, _ComissionSessionId);
				if (PropertyChanging(args))
				{
					_ComissionSessionId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AdjustSchemeId
		{	
			get{ return _AdjustSchemeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustSchemeId, value, _AdjustSchemeId);
				if (PropertyChanging(args))
				{
					_AdjustSchemeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Description
		{	
			get{ return _Description; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Description, value, _Description);
				if (PropertyChanging(args))
				{
					_Description = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Conduit
		{	
			get{ return _Conduit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Conduit, value, _Conduit);
				if (PropertyChanging(args))
				{
					_Conduit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Multiple
		{	
			get{ return _Multiple; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Multiple, value, _Multiple);
				if (PropertyChanging(args))
				{
					_Multiple = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AppliedTo
		{	
			get{ return _AppliedTo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppliedTo, value, _AppliedTo);
				if (PropertyChanging(args))
				{
					_AppliedTo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> StartDate
		{	
			get{ return _StartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartDate, value, _StartDate);
				if (PropertyChanging(args))
				{
					_StartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EndDate
		{	
			get{ return _EndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndDate, value, _EndDate);
				if (PropertyChanging(args))
				{
					_EndDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AdjustmentBase Clone()
		{
			AdjustmentBase newObj = new  AdjustmentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ComissionSessionId = this.ComissionSessionId;						
			newObj.AdjustSchemeId = this.AdjustSchemeId;						
			newObj.Description = this.Description;						
			newObj.Conduit = this.Conduit;						
			newObj.Amount = this.Amount;						
			newObj.Multiple = this.Multiple;						
			newObj.AppliedTo = this.AppliedTo;						
			newObj.StartDate = this.StartDate;						
			newObj.EndDate = this.EndDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AdjustmentBase.Property_Id, Id);				
			info.AddValue(AdjustmentBase.Property_ComissionSessionId, ComissionSessionId);				
			info.AddValue(AdjustmentBase.Property_AdjustSchemeId, AdjustSchemeId);				
			info.AddValue(AdjustmentBase.Property_Description, Description);				
			info.AddValue(AdjustmentBase.Property_Conduit, Conduit);				
			info.AddValue(AdjustmentBase.Property_Amount, Amount);				
			info.AddValue(AdjustmentBase.Property_Multiple, Multiple);				
			info.AddValue(AdjustmentBase.Property_AppliedTo, AppliedTo);				
			info.AddValue(AdjustmentBase.Property_StartDate, StartDate);				
			info.AddValue(AdjustmentBase.Property_EndDate, EndDate);				
		}
		#endregion

		
	}
}
