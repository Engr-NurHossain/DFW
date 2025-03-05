using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AAAAlifSecuirtyCCInfo2Base", Namespace = "http://www.piistech.com//entities")]
	public class AAAAlifSecuirtyCCInfo2Base : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			NameOnCard = 1,
			CardNumber = 2,
			ExpDate = 3,
			SecurityCode = 4,
			CustomerId = 5,
			FirstName = 6,
			LastName = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_NameOnCard = "NameOnCard";		            
		public const string Property_CardNumber = "CardNumber";		            
		public const string Property_ExpDate = "ExpDate";		            
		public const string Property_SecurityCode = "SecurityCode";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _NameOnCard;	            
		private String _CardNumber;	            
		private String _ExpDate;	            
		private String _SecurityCode;	            
		private Nullable<Int32> _CustomerId;	            
		private String _FirstName;	            
		private String _LastName;	            
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
		public String NameOnCard
		{	
			get{ return _NameOnCard; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NameOnCard, value, _NameOnCard);
				if (PropertyChanging(args))
				{
					_NameOnCard = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CardNumber
		{	
			get{ return _CardNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardNumber, value, _CardNumber);
				if (PropertyChanging(args))
				{
					_CardNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExpDate
		{	
			get{ return _ExpDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpDate, value, _ExpDate);
				if (PropertyChanging(args))
				{
					_ExpDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SecurityCode
		{	
			get{ return _SecurityCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecurityCode, value, _SecurityCode);
				if (PropertyChanging(args))
				{
					_SecurityCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CustomerId
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
		public String FirstName
		{	
			get{ return _FirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstName, value, _FirstName);
				if (PropertyChanging(args))
				{
					_FirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastName
		{	
			get{ return _LastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastName, value, _LastName);
				if (PropertyChanging(args))
				{
					_LastName = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AAAAlifSecuirtyCCInfo2Base Clone()
		{
			AAAAlifSecuirtyCCInfo2Base newObj = new  AAAAlifSecuirtyCCInfo2Base();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.NameOnCard = this.NameOnCard;						
			newObj.CardNumber = this.CardNumber;						
			newObj.ExpDate = this.ExpDate;						
			newObj.SecurityCode = this.SecurityCode;						
			newObj.CustomerId = this.CustomerId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AAAAlifSecuirtyCCInfo2Base.Property_Id, Id);				
			info.AddValue(AAAAlifSecuirtyCCInfo2Base.Property_NameOnCard, NameOnCard);				
			info.AddValue(AAAAlifSecuirtyCCInfo2Base.Property_CardNumber, CardNumber);				
			info.AddValue(AAAAlifSecuirtyCCInfo2Base.Property_ExpDate, ExpDate);				
			info.AddValue(AAAAlifSecuirtyCCInfo2Base.Property_SecurityCode, SecurityCode);				
			info.AddValue(AAAAlifSecuirtyCCInfo2Base.Property_CustomerId, CustomerId);				
			info.AddValue(AAAAlifSecuirtyCCInfo2Base.Property_FirstName, FirstName);				
			info.AddValue(AAAAlifSecuirtyCCInfo2Base.Property_LastName, LastName);				
		}
		#endregion

		
	}
}
