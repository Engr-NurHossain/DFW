using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmailTextTemplateBase", Namespace = "http://www.piistech.com//entities")]
	public class EmailTextTemplateBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Type = 2,
			TextContent = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Type = "Type";		            
		public const string Property_TextContent = "TextContent";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Type;	            
		private String _TextContent;	            
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
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TextContent
		{	
			get{ return _TextContent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TextContent, value, _TextContent);
				if (PropertyChanging(args))
				{
					_TextContent = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmailTextTemplateBase Clone()
		{
			EmailTextTemplateBase newObj = new  EmailTextTemplateBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Type = this.Type;						
			newObj.TextContent = this.TextContent;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmailTextTemplateBase.Property_Id, Id);				
			info.AddValue(EmailTextTemplateBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmailTextTemplateBase.Property_Type, Type);				
			info.AddValue(EmailTextTemplateBase.Property_TextContent, TextContent);				
		}
		#endregion

		
	}
}
