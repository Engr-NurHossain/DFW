using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ResturantReviewBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ResturantReviewBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			Email = 3,
			Comments = 4,
			Rating = 5,
			CreatedBy = 6,
			CreatedDate = 7,
			IsActive = 8,
			Reply = 9,
			ReviewFor = 10,
			ReplyBy = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_Email = "Email";		            
		public const string Property_Comments = "Comments";		            
		public const string Property_Rating = "Rating";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_Reply = "Reply";		            
		public const string Property_ReviewFor = "ReviewFor";		            
		public const string Property_ReplyBy = "ReplyBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private String _Email;	            
		private String _Comments;	            
		private Nullable<Double> _Rating;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsActive;	            
		private String _Reply;	            
		private String _ReviewFor;	            
		private Guid _ReplyBy;	            
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
		public String Email
		{	
			get{ return _Email; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Email, value, _Email);
				if (PropertyChanging(args))
				{
					_Email = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Comments
		{	
			get{ return _Comments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Comments, value, _Comments);
				if (PropertyChanging(args))
				{
					_Comments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Rating
		{	
			get{ return _Rating; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Rating, value, _Rating);
				if (PropertyChanging(args))
				{
					_Rating = value;
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
		public String Reply
		{	
			get{ return _Reply; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Reply, value, _Reply);
				if (PropertyChanging(args))
				{
					_Reply = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReviewFor
		{	
			get{ return _ReviewFor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReviewFor, value, _ReviewFor);
				if (PropertyChanging(args))
				{
					_ReviewFor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ReplyBy
		{	
			get{ return _ReplyBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReplyBy, value, _ReplyBy);
				if (PropertyChanging(args))
				{
					_ReplyBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ResturantReviewBase Clone()
		{
			ResturantReviewBase newObj = new  ResturantReviewBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.Email = this.Email;						
			newObj.Comments = this.Comments;						
			newObj.Rating = this.Rating;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsActive = this.IsActive;						
			newObj.Reply = this.Reply;						
			newObj.ReviewFor = this.ReviewFor;						
			newObj.ReplyBy = this.ReplyBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ResturantReviewBase.Property_Id, Id);				
			info.AddValue(ResturantReviewBase.Property_CompanyId, CompanyId);				
			info.AddValue(ResturantReviewBase.Property_Name, Name);				
			info.AddValue(ResturantReviewBase.Property_Email, Email);				
			info.AddValue(ResturantReviewBase.Property_Comments, Comments);				
			info.AddValue(ResturantReviewBase.Property_Rating, Rating);				
			info.AddValue(ResturantReviewBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ResturantReviewBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(ResturantReviewBase.Property_IsActive, IsActive);				
			info.AddValue(ResturantReviewBase.Property_Reply, Reply);				
			info.AddValue(ResturantReviewBase.Property_ReviewFor, ReviewFor);				
			info.AddValue(ResturantReviewBase.Property_ReplyBy, ReplyBy);				
		}
		#endregion

		
	}
}
