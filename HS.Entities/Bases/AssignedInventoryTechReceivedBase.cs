using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AssignedInventoryTechReceivedBase", Namespace = "http://www.piistech.com//entities")]
	public class AssignedInventoryTechReceivedBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TechnicianId = 1,
			EquipmentId = 2,
			Quantity = 3,
			IsReceived = 4,
			ReceivedDate = 5,
			ReceivedBy = 6,
			CreatedDate = 7,
			CreatedBy = 8,
			IsApprove = 9,
			IsDecline = 10,
			ReqSrc = 11,
            ClosedBy = 12,
        }
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TechnicianId = "TechnicianId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_IsReceived = "IsReceived";		            
		public const string Property_ReceivedDate = "ReceivedDate";		            
		public const string Property_ReceivedBy = "ReceivedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_IsApprove = "IsApprove";		            
		public const string Property_IsDecline = "IsDecline";
		public const string Property_ReqSrc = "ReqSrc";
        public const string Property_ClosedBy = "ClosedBy";


        #endregion

        #region Private Data Types
        private Int32 _Id;	            
		private Guid _TechnicianId;	            
		private Guid _EquipmentId;	            
		private Int32 _Quantity;	            
		private Boolean _IsReceived;	            
		private Nullable<DateTime> _ReceivedDate;	            
		private Nullable<Guid> _ReceivedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Nullable<Boolean> _IsApprove;
		private Nullable<Boolean> _IsDecline;
		private String _ReqSrc;
        private Guid _ClosedBy;
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
		public Guid TechnicianId
		{	
			get{ return _TechnicianId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechnicianId, value, _TechnicianId);
				if (PropertyChanging(args))
				{
					_TechnicianId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid EquipmentId
		{	
			get{ return _EquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentId, value, _EquipmentId);
				if (PropertyChanging(args))
				{
					_EquipmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Quantity
		{	
			get{ return _Quantity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Quantity, value, _Quantity);
				if (PropertyChanging(args))
				{
					_Quantity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsReceived
		{	
			get{ return _IsReceived; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsReceived, value, _IsReceived);
				if (PropertyChanging(args))
				{
					_IsReceived = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReceivedDate
		{	
			get{ return _ReceivedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReceivedDate, value, _ReceivedDate);
				if (PropertyChanging(args))
				{
					_ReceivedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Guid> ReceivedBy
		{	
			get{ return _ReceivedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReceivedBy, value, _ReceivedBy);
				if (PropertyChanging(args))
				{
					_ReceivedBy = value;
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
		public Nullable<Boolean> IsApprove
		{	
			get{ return _IsApprove; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsApprove, value, _IsApprove);
				if (PropertyChanging(args))
				{
					_IsApprove = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDecline
		{	
			get{ return _IsDecline; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDecline, value, _IsDecline);
				if (PropertyChanging(args))
				{
					_IsDecline = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReqSrc
		{
			get { return _ReqSrc; }
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReqSrc, value, _ReqSrc);
				if (PropertyChanging(args))
				{
					_ReqSrc = value;
					PropertyChanged(args);
				}
			}
		}

        [DataMember]
        public Guid ClosedBy
        {
            get { return _ClosedBy; }
            set
            {
                PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClosedBy, value, _ClosedBy);
                if (PropertyChanging(args))
                {
                    _ClosedBy = value;
                    PropertyChanged(args);
                }
            }
        }

        #endregion

        #region Cloning Base Objects
        public AssignedInventoryTechReceivedBase Clone()
		{
			AssignedInventoryTechReceivedBase newObj = new  AssignedInventoryTechReceivedBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TechnicianId = this.TechnicianId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.Quantity = this.Quantity;						
			newObj.IsReceived = this.IsReceived;						
			newObj.ReceivedDate = this.ReceivedDate;						
			newObj.ReceivedBy = this.ReceivedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.IsApprove = this.IsApprove;
			newObj.IsDecline = this.IsDecline;
			newObj.ReqSrc = this.ReqSrc;
            newObj.ClosedBy = this.ClosedBy;



            return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AssignedInventoryTechReceivedBase.Property_Id, Id);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_TechnicianId, TechnicianId);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_Quantity, Quantity);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_IsReceived, IsReceived);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_ReceivedDate, ReceivedDate);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_ReceivedBy, ReceivedBy);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_IsApprove, IsApprove);				
			info.AddValue(AssignedInventoryTechReceivedBase.Property_IsDecline, IsDecline);
			info.AddValue(AssignedInventoryTechReceivedBase.Property_ReqSrc, ReqSrc);
            info.AddValue(AssignedInventoryTechReceivedBase.Property_ClosedBy, ClosedBy); 


        }
        #endregion


    }
}
