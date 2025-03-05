using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MissingNoteBase", Namespace = "http://www.hims-tech.com//entities")]
	public class MissingNoteBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerID = 1,
			NoteHtml = 2,
			NoteText = 3,
			NoteType = 4,
			CreatedDate = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerID = "CustomerID";		            
		public const string Property_NoteHtml = "NoteHtml";		            
		public const string Property_NoteText = "NoteText";		            
		public const string Property_NoteType = "NoteType";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Nullable<Int32> _CustomerID;	            
		private String _NoteHtml;	            
		private String _NoteText;	            
		private String _NoteType;	            
		private Nullable<DateTime> _CreatedDate;	            
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
		public Nullable<Int32> CustomerID
		{	
			get{ return _CustomerID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerID, value, _CustomerID);
				if (PropertyChanging(args))
				{
					_CustomerID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoteHtml
		{	
			get{ return _NoteHtml; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoteHtml, value, _NoteHtml);
				if (PropertyChanging(args))
				{
					_NoteHtml = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoteText
		{	
			get{ return _NoteText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoteText, value, _NoteText);
				if (PropertyChanging(args))
				{
					_NoteText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoteType
		{	
			get{ return _NoteType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoteType, value, _NoteType);
				if (PropertyChanging(args))
				{
					_NoteType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CreatedDate
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

		#endregion
		
		#region Cloning Base Objects
		public  MissingNoteBase Clone()
		{
			MissingNoteBase newObj = new  MissingNoteBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerID = this.CustomerID;						
			newObj.NoteHtml = this.NoteHtml;						
			newObj.NoteText = this.NoteText;						
			newObj.NoteType = this.NoteType;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MissingNoteBase.Property_Id, Id);				
			info.AddValue(MissingNoteBase.Property_CustomerID, CustomerID);				
			info.AddValue(MissingNoteBase.Property_NoteHtml, NoteHtml);				
			info.AddValue(MissingNoteBase.Property_NoteText, NoteText);				
			info.AddValue(MissingNoteBase.Property_NoteType, NoteType);				
			info.AddValue(MissingNoteBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
