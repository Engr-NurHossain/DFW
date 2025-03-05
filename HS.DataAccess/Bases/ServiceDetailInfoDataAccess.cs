using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class ServiceDetailInfoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSERVICEDETAILINFO = "InsertServiceDetailInfo";
		private const string UPDATESERVICEDETAILINFO = "UpdateServiceDetailInfo";
		private const string DELETESERVICEDETAILINFO = "DeleteServiceDetailInfo";
		private const string GETSERVICEDETAILINFOBYID = "GetServiceDetailInfoById";
		private const string GETALLSERVICEDETAILINFO = "GetAllServiceDetailInfo";
		private const string GETPAGEDSERVICEDETAILINFO = "GetPagedServiceDetailInfo";
		private const string GETSERVICEDETAILINFOMAXIMUMID = "GetServiceDetailInfoMaximumId";
		private const string GETSERVICEDETAILINFOROWCOUNT = "GetServiceDetailInfoRowCount";	
		private const string GETSERVICEDETAILINFOBYQUERY = "GetServiceDetailInfoByQuery";
		#endregion
		
		#region Constructors
		public ServiceDetailInfoDataAccess(ClientContext context) : base(context) { }
		public ServiceDetailInfoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="serviceDetailInfoObject"></param>
		private void AddCommonParams(SqlCommand cmd, ServiceDetailInfoBase serviceDetailInfoObject)
		{	
			AddParameter(cmd, pGuid(ServiceDetailInfoBase.Property_ServiceInfoId, serviceDetailInfoObject.ServiceInfoId));
			AddParameter(cmd, pNVarChar(ServiceDetailInfoBase.Property_Name, 150, serviceDetailInfoObject.Name));
			AddParameter(cmd, pNVarChar(ServiceDetailInfoBase.Property_Type, 50, serviceDetailInfoObject.Type));
			AddParameter(cmd, pGuid(ServiceDetailInfoBase.Property_ServiceId, serviceDetailInfoObject.ServiceId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ServiceDetailInfo
        /// </summary>
        /// <param name="serviceDetailInfoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ServiceDetailInfoBase serviceDetailInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSERVICEDETAILINFO);
	
				AddParameter(cmd, pInt32Out(ServiceDetailInfoBase.Property_Id));
				AddCommonParams(cmd, serviceDetailInfoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					serviceDetailInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					serviceDetailInfoObject.Id = (Int32)GetOutParameter(cmd, ServiceDetailInfoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(serviceDetailInfoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ServiceDetailInfo
        /// </summary>
        /// <param name="serviceDetailInfoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ServiceDetailInfoBase serviceDetailInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESERVICEDETAILINFO);
				
				AddParameter(cmd, pInt32(ServiceDetailInfoBase.Property_Id, serviceDetailInfoObject.Id));
				AddCommonParams(cmd, serviceDetailInfoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					serviceDetailInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(serviceDetailInfoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ServiceDetailInfo
        /// </summary>
        /// <param name="Id">Id of the ServiceDetailInfo object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESERVICEDETAILINFO);	
				
				AddParameter(cmd, pInt32(ServiceDetailInfoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ServiceDetailInfo), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ServiceDetailInfo object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ServiceDetailInfo object to retrieve</param>
        /// <returns>ServiceDetailInfo object, null if not found</returns>
		public ServiceDetailInfo Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEDETAILINFOBYID))
			{
				AddParameter( cmd, pInt32(ServiceDetailInfoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ServiceDetailInfo objects 
        /// </summary>
        /// <returns>A list of ServiceDetailInfo objects</returns>
		public ServiceDetailInfoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSERVICEDETAILINFO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ServiceDetailInfo objects by PageRequest
        /// </summary>
        /// <returns>A list of ServiceDetailInfo objects</returns>
		public ServiceDetailInfoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSERVICEDETAILINFO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ServiceDetailInfoList _ServiceDetailInfoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ServiceDetailInfoList;
			}
		}
		
		/// <summary>
        /// Retrieves all ServiceDetailInfo objects by query String
        /// </summary>
        /// <returns>A list of ServiceDetailInfo objects</returns>
		public ServiceDetailInfoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEDETAILINFOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ServiceDetailInfo Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ServiceDetailInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEDETAILINFOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ServiceDetailInfo Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ServiceDetailInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ServiceDetailInfoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEDETAILINFOROWCOUNT))
			{
				SqlDataReader reader;
				_ServiceDetailInfoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ServiceDetailInfoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ServiceDetailInfo object
        /// </summary>
        /// <param name="serviceDetailInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ServiceDetailInfoBase serviceDetailInfoObject, SqlDataReader reader, int start)
		{
			
				serviceDetailInfoObject.Id = reader.GetInt32( start + 0 );			
				serviceDetailInfoObject.ServiceInfoId = reader.GetGuid( start + 1 );			
				serviceDetailInfoObject.Name = reader.GetString( start + 2 );			
				serviceDetailInfoObject.Type = reader.GetString( start + 3 );			
				serviceDetailInfoObject.ServiceId = reader.GetGuid( start + 4 );			
			FillBaseObject(serviceDetailInfoObject, reader, (start + 5));

			
			serviceDetailInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ServiceDetailInfo object
        /// </summary>
        /// <param name="serviceDetailInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ServiceDetailInfoBase serviceDetailInfoObject, SqlDataReader reader)
		{
			FillObject(serviceDetailInfoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ServiceDetailInfo object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ServiceDetailInfo object</returns>
		private ServiceDetailInfo GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ServiceDetailInfo serviceDetailInfoObject= new ServiceDetailInfo();
					FillObject(serviceDetailInfoObject, reader);
					return serviceDetailInfoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ServiceDetailInfo objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ServiceDetailInfo objects</returns>
		private ServiceDetailInfoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ServiceDetailInfo list
			ServiceDetailInfoList list = new ServiceDetailInfoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ServiceDetailInfo serviceDetailInfoObject = new ServiceDetailInfo();
					FillObject(serviceDetailInfoObject, reader);

					list.Add(serviceDetailInfoObject);
				}
				
				// Close the reader in order to receive output parameters
				// Output parameters are not available until reader is closed.
				reader.Close();
			}

			return list;
		}
		
		#endregion
	}	
}
