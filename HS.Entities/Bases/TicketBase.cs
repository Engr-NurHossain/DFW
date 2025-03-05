using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TicketBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TicketId = 1,
			CompanyId = 2,
			CustomerId = 3,
			TicketType = 4,
			Subject = 5,
			Message = 6,
			CreatedBy = 7,
			CreatedDate = 8,
			CompletionDate = 9,
			Status = 10,
			Priority = 11,
			LastUpdatedBy = 12,
			LastUpdatedDate = 13,
			HasInvoice = 14,
			HasSurvey = 15,
			IsClosed = 16,
			IsAgreementTicket = 17,
			CompletedDate = 18,
			Signature = 19,
			IsDispatch = 20,
			ReferenceTicketId = 21,
			BookingId = 22,
			Reason = 23,
			RackNo = 24,
			Locations = 25,
			RescheduleTicketId = 26,
			IsImportedTicket = 27,
			TicketSignatureDate = 28,
			TechOnsiteDate = 29,
			WorkToBePerformed = 30,
			EquipmentPriceChanged = 31,
			ServicePriceChanged = 32,
			EquipmentQTYChanged = 33,
			ServiceQTYChanged = 34,
			IsPayrollClosed = 35,
			MiscName = 36,
			MiscValue = 37
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TicketType = "TicketType";		            
		public const string Property_Subject = "Subject";		            
		public const string Property_Message = "Message";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CompletionDate = "CompletionDate";		            
		public const string Property_Status = "Status";		            
		public const string Property_Priority = "Priority";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_HasInvoice = "HasInvoice";		            
		public const string Property_HasSurvey = "HasSurvey";		            
		public const string Property_IsClosed = "IsClosed";		            
		public const string Property_IsAgreementTicket = "IsAgreementTicket";		            
		public const string Property_CompletedDate = "CompletedDate";		            
		public const string Property_Signature = "Signature";		            
		public const string Property_IsDispatch = "IsDispatch";		            
		public const string Property_ReferenceTicketId = "ReferenceTicketId";		            
		public const string Property_BookingId = "BookingId";		            
		public const string Property_Reason = "Reason";		            
		public const string Property_RackNo = "RackNo";		            
		public const string Property_Locations = "Locations";		            
		public const string Property_RescheduleTicketId = "RescheduleTicketId";		            
		public const string Property_IsImportedTicket = "IsImportedTicket";		            
		public const string Property_TicketSignatureDate = "TicketSignatureDate";		            
		public const string Property_TechOnsiteDate = "TechOnsiteDate";		            
		public const string Property_WorkToBePerformed = "WorkToBePerformed";		            
		public const string Property_EquipmentPriceChanged = "EquipmentPriceChanged";		            
		public const string Property_ServicePriceChanged = "ServicePriceChanged";		            
		public const string Property_EquipmentQTYChanged = "EquipmentQTYChanged";		            
		public const string Property_ServiceQTYChanged = "ServiceQTYChanged";		            
		public const string Property_IsPayrollClosed = "IsPayrollClosed";		            
		public const string Property_MiscName = "MiscName";		            
		public const string Property_MiscValue = "MiscValue";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TicketId;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _TicketType;	            
		private String _Subject;	            
		private String _Message;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private DateTime _CompletionDate;	            
		private String _Status;	            
		private String _Priority;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Boolean> _HasInvoice;	            
		private Nullable<Boolean> _HasSurvey;	            
		private Nullable<Boolean> _IsClosed;	            
		private Nullable<Boolean> _IsAgreementTicket;	            
		private Nullable<DateTime> _CompletedDate;	            
		private String _Signature;	            
		private Nullable<Boolean> _IsDispatch;	            
		private Nullable<Int32> _ReferenceTicketId;	            
		private String _BookingId;	            
		private String _Reason;	            
		private String _RackNo;	            
		private String _Locations;	            
		private Nullable<Int32> _RescheduleTicketId;	            
		private Nullable<Boolean> _IsImportedTicket;	            
		private Nullable<DateTime> _TicketSignatureDate;	            
		private Nullable<DateTime> _TechOnsiteDate;	            
		private String _WorkToBePerformed;	            
		private Nullable<Boolean> _EquipmentPriceChanged;	            
		private Nullable<Boolean> _ServicePriceChanged;	            
		private Nullable<Boolean> _EquipmentQTYChanged;	            
		private Nullable<Boolean> _ServiceQTYChanged;	            
		private Nullable<Boolean> _IsPayrollClosed;	            
		private String _MiscName;	            
		private Nullable<Decimal> _MiscValue;	            
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
		public Guid TicketId
		{	
			get{ return _TicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketId, value, _TicketId);
				if (PropertyChanging(args))
				{
					_TicketId = value;
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
		public String TicketType
		{	
			get{ return _TicketType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketType, value, _TicketType);
				if (PropertyChanging(args))
				{
					_TicketType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Subject
		{	
			get{ return _Subject; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Subject, value, _Subject);
				if (PropertyChanging(args))
				{
					_Subject = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Message
		{	
			get{ return _Message; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Message, value, _Message);
				if (PropertyChanging(args))
				{
					_Message = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedBy
		{	
			get{ return _CreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedBy, value, _CreatedBy);
				if (PropertyChanging(args))
				{
					_CreatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CreatedDate
		{	
			get{ return _CreatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedDate, value, _CreatedDate);
				if (PropertyChanging(args))
				{
					_CreatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CompletionDate
		{	
			get{ return _CompletionDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompletionDate, value, _CompletionDate);
				if (PropertyChanging(args))
				{
					_CompletionDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Priority
		{	
			get{ return _Priority; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Priority, value, _Priority);
				if (PropertyChanging(args))
				{
					_Priority = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> HasInvoice
		{	
			get{ return _HasInvoice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HasInvoice, value, _HasInvoice);
				if (PropertyChanging(args))
				{
					_HasInvoice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> HasSurvey
		{	
			get{ return _HasSurvey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HasSurvey, value, _HasSurvey);
				if (PropertyChanging(args))
				{
					_HasSurvey = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsClosed
		{	
			get{ return _IsClosed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsClosed, value, _IsClosed);
				if (PropertyChanging(args))
				{
					_IsClosed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsAgreementTicket
		{	
			get{ return _IsAgreementTicket; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAgreementTicket, value, _IsAgreementTicket);
				if (PropertyChanging(args))
				{
					_IsAgreementTicket = value;
					PropertyChanged(args);					
				}	
			}
        }


		[DataMember]
		public Nullable<DateTime> CompletedDate
		{	
			get{ return _CompletedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompletedDate, value, _CompletedDate);
				if (PropertyChanging(args))
				{
					_CompletedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Signature
		{	
			get{ return _Signature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Signature, value, _Signature);
				if (PropertyChanging(args))
				{
					_Signature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDispatch
		{	
			get{ return _IsDispatch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDispatch, value, _IsDispatch);
				if (PropertyChanging(args))
				{
					_IsDispatch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> ReferenceTicketId
		{	
			get{ return _ReferenceTicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceTicketId, value, _ReferenceTicketId);
				if (PropertyChanging(args))
				{
					_ReferenceTicketId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BookingId
		{	
			get{ return _BookingId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BookingId, value, _BookingId);
				if (PropertyChanging(args))
				{
					_BookingId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Reason
		{	
			get{ return _Reason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Reason, value, _Reason);
				if (PropertyChanging(args))
				{
					_Reason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RackNo
		{	
			get{ return _RackNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RackNo, value, _RackNo);
				if (PropertyChanging(args))
				{
					_RackNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Locations
		{	
			get{ return _Locations; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Locations, value, _Locations);
				if (PropertyChanging(args))
				{
					_Locations = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> RescheduleTicketId
		{	
			get{ return _RescheduleTicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RescheduleTicketId, value, _RescheduleTicketId);
				if (PropertyChanging(args))
				{
					_RescheduleTicketId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsImportedTicket
		{	
			get{ return _IsImportedTicket; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsImportedTicket, value, _IsImportedTicket);
				if (PropertyChanging(args))
				{
					_IsImportedTicket = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> TicketSignatureDate
		{	
			get{ return _TicketSignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketSignatureDate, value, _TicketSignatureDate);
				if (PropertyChanging(args))
				{
					_TicketSignatureDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> TechOnsiteDate
		{	
			get{ return _TechOnsiteDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechOnsiteDate, value, _TechOnsiteDate);
				if (PropertyChanging(args))
				{
					_TechOnsiteDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String WorkToBePerformed
		{	
			get{ return _WorkToBePerformed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WorkToBePerformed, value, _WorkToBePerformed);
				if (PropertyChanging(args))
				{
					_WorkToBePerformed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> EquipmentPriceChanged
		{	
			get{ return _EquipmentPriceChanged; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentPriceChanged, value, _EquipmentPriceChanged);
				if (PropertyChanging(args))
				{
					_EquipmentPriceChanged = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> ServicePriceChanged
		{	
			get{ return _ServicePriceChanged; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServicePriceChanged, value, _ServicePriceChanged);
				if (PropertyChanging(args))
				{
					_ServicePriceChanged = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> EquipmentQTYChanged
		{	
			get{ return _EquipmentQTYChanged; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentQTYChanged, value, _EquipmentQTYChanged);
				if (PropertyChanging(args))
				{
					_EquipmentQTYChanged = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> ServiceQTYChanged
		{	
			get{ return _ServiceQTYChanged; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServiceQTYChanged, value, _ServiceQTYChanged);
				if (PropertyChanging(args))
				{
					_ServiceQTYChanged = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPayrollClosed
		{	
			get{ return _IsPayrollClosed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPayrollClosed, value, _IsPayrollClosed);
				if (PropertyChanging(args))
				{
					_IsPayrollClosed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MiscName
		{	
			get{ return _MiscName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscName, value, _MiscName);
				if (PropertyChanging(args))
				{
					_MiscName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Decimal> MiscValue
		{	
			get{ return _MiscValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscValue, value, _MiscValue);
				if (PropertyChanging(args))
				{
					_MiscValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TicketBase Clone()
		{
			TicketBase newObj = new  TicketBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TicketId = this.TicketId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TicketType = this.TicketType;						
			newObj.Subject = this.Subject;						
			newObj.Message = this.Message;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CompletionDate = this.CompletionDate;						
			newObj.Status = this.Status;						
			newObj.Priority = this.Priority;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.HasInvoice = this.HasInvoice;						
			newObj.HasSurvey = this.HasSurvey;						
			newObj.IsClosed = this.IsClosed;						
			newObj.IsAgreementTicket = this.IsAgreementTicket;						
			newObj.CompletedDate = this.CompletedDate;						
			newObj.Signature = this.Signature;						
			newObj.IsDispatch = this.IsDispatch;						
			newObj.ReferenceTicketId = this.ReferenceTicketId;						
			newObj.BookingId = this.BookingId;						
			newObj.Reason = this.Reason;						
			newObj.RackNo = this.RackNo;						
			newObj.Locations = this.Locations;						
			newObj.RescheduleTicketId = this.RescheduleTicketId;						
			newObj.IsImportedTicket = this.IsImportedTicket;						
			newObj.TicketSignatureDate = this.TicketSignatureDate;						
			newObj.TechOnsiteDate = this.TechOnsiteDate;						
			newObj.WorkToBePerformed = this.WorkToBePerformed;						
			newObj.EquipmentPriceChanged = this.EquipmentPriceChanged;						
			newObj.ServicePriceChanged = this.ServicePriceChanged;						
			newObj.EquipmentQTYChanged = this.EquipmentQTYChanged;						
			newObj.ServiceQTYChanged = this.ServiceQTYChanged;						
			newObj.IsPayrollClosed = this.IsPayrollClosed;						
			newObj.MiscName = this.MiscName;						
			newObj.MiscValue = this.MiscValue;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketBase.Property_Id, Id);				
			info.AddValue(TicketBase.Property_TicketId, TicketId);				
			info.AddValue(TicketBase.Property_CompanyId, CompanyId);				
			info.AddValue(TicketBase.Property_CustomerId, CustomerId);				
			info.AddValue(TicketBase.Property_TicketType, TicketType);				
			info.AddValue(TicketBase.Property_Subject, Subject);				
			info.AddValue(TicketBase.Property_Message, Message);				
			info.AddValue(TicketBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TicketBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(TicketBase.Property_CompletionDate, CompletionDate);				
			info.AddValue(TicketBase.Property_Status, Status);				
			info.AddValue(TicketBase.Property_Priority, Priority);				
			info.AddValue(TicketBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(TicketBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(TicketBase.Property_HasInvoice, HasInvoice);				
			info.AddValue(TicketBase.Property_HasSurvey, HasSurvey);				
			info.AddValue(TicketBase.Property_IsClosed, IsClosed);				
			info.AddValue(TicketBase.Property_IsAgreementTicket, IsAgreementTicket);				
			info.AddValue(TicketBase.Property_CompletedDate, CompletedDate);				
			info.AddValue(TicketBase.Property_Signature, Signature);				
			info.AddValue(TicketBase.Property_IsDispatch, IsDispatch);				
			info.AddValue(TicketBase.Property_ReferenceTicketId, ReferenceTicketId);				
			info.AddValue(TicketBase.Property_BookingId, BookingId);				
			info.AddValue(TicketBase.Property_Reason, Reason);				
			info.AddValue(TicketBase.Property_RackNo, RackNo);				
			info.AddValue(TicketBase.Property_Locations, Locations);				
			info.AddValue(TicketBase.Property_RescheduleTicketId, RescheduleTicketId);				
			info.AddValue(TicketBase.Property_IsImportedTicket, IsImportedTicket);				
			info.AddValue(TicketBase.Property_TicketSignatureDate, TicketSignatureDate);				
			info.AddValue(TicketBase.Property_TechOnsiteDate, TechOnsiteDate);				
			info.AddValue(TicketBase.Property_WorkToBePerformed, WorkToBePerformed);				
			info.AddValue(TicketBase.Property_EquipmentPriceChanged, EquipmentPriceChanged);				
			info.AddValue(TicketBase.Property_ServicePriceChanged, ServicePriceChanged);				
			info.AddValue(TicketBase.Property_EquipmentQTYChanged, EquipmentQTYChanged);				
			info.AddValue(TicketBase.Property_ServiceQTYChanged, ServiceQTYChanged);				
			info.AddValue(TicketBase.Property_IsPayrollClosed, IsPayrollClosed);				
			info.AddValue(TicketBase.Property_MiscName, MiscName);				
			info.AddValue(TicketBase.Property_MiscValue, MiscValue);				
		}
		#endregion

		
	}
}
