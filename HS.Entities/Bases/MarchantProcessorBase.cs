using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MarchantProcessorBase", Namespace = "http://www.piistech.com//entities")]
	public class MarchantProcessorBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			MarchantId = 2,
			ProcessorId = 3,
			Method = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_MarchantId = "MarchantId";		            
		public const string Property_ProcessorId = "ProcessorId";		            
		public const string Property_Method = "Method";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Nullable<Int32> _MarchantId;	            
		private Nullable<Int32> _ProcessorId;	            
		private String _Method;	            
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
		public Nullable<Int32> MarchantId
		{	
			get{ return _MarchantId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MarchantId, value, _MarchantId);
				if (PropertyChanging(args))
				{
					_MarchantId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> ProcessorId
		{	
			get{ return _ProcessorId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProcessorId, value, _ProcessorId);
				if (PropertyChanging(args))
				{
					_ProcessorId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Method
		{	
			get{ return _Method; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Method, value, _Method);
				if (PropertyChanging(args))
				{
					_Method = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  MarchantProcessorBase Clone()
		{
			MarchantProcessorBase newObj = new  MarchantProcessorBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.MarchantId = this.MarchantId;						
			newObj.ProcessorId = this.ProcessorId;						
			newObj.Method = this.Method;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MarchantProcessorBase.Property_Id, Id);				
			info.AddValue(MarchantProcessorBase.Property_CompanyId, CompanyId);				
			info.AddValue(MarchantProcessorBase.Property_MarchantId, MarchantId);				
			info.AddValue(MarchantProcessorBase.Property_ProcessorId, ProcessorId);				
			info.AddValue(MarchantProcessorBase.Property_Method, Method);				
		}
		#endregion

		
	}
}
