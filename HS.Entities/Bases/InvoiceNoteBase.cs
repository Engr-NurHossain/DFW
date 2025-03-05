using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "InvoiceNoteBase", Namespace = "http://www.piistech.com//entities")]
	public class InvoiceNoteBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			InvoiceId = 2,
			Note = 3,
			AddedDate = 4,
			AddedBy = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_Note = "Note";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_AddedBy = "AddedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _InvoiceId;	            
		private String _Note;	            
		private DateTime _AddedDate;	            
		private Guid _AddedBy;	            
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
		public Int32 InvoiceId
		{	
			get{ return _InvoiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceId, value, _InvoiceId);
				if (PropertyChanging(args))
				{
					_InvoiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime AddedDate
		{	
			get{ return _AddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedDate, value, _AddedDate);
				if (PropertyChanging(args))
				{
					_AddedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AddedBy
		{	
			get{ return _AddedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedBy, value, _AddedBy);
				if (PropertyChanging(args))
				{
					_AddedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  InvoiceNoteBase Clone()
		{
			InvoiceNoteBase newObj = new  InvoiceNoteBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.Note = this.Note;						
			newObj.AddedDate = this.AddedDate;						
			newObj.AddedBy = this.AddedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(InvoiceNoteBase.Property_Id, Id);				
			info.AddValue(InvoiceNoteBase.Property_CompanyId, CompanyId);				
			info.AddValue(InvoiceNoteBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(InvoiceNoteBase.Property_Note, Note);				
			info.AddValue(InvoiceNoteBase.Property_AddedDate, AddedDate);				
			info.AddValue(InvoiceNoteBase.Property_AddedBy, AddedBy);				
		}
		#endregion

		
	}
}
