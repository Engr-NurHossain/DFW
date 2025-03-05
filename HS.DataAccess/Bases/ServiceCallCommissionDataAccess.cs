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
	public partial class ServiceCallCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSERVICECALLCOMMISSION = "InsertServiceCallCommission";
		private const string UPDATESERVICECALLCOMMISSION = "UpdateServiceCallCommission";
		private const string DELETESERVICECALLCOMMISSION = "DeleteServiceCallCommission";
		private const string GETSERVICECALLCOMMISSIONBYID = "GetServiceCallCommissionById";
		private const string GETALLSERVICECALLCOMMISSION = "GetAllServiceCallCommission";
		private const string GETPAGEDSERVICECALLCOMMISSION = "GetPagedServiceCallCommission";
		private const string GETSERVICECALLCOMMISSIONMAXIMUMID = "GetServiceCallCommissionMaximumId";
		private const string GETSERVICECALLCOMMISSIONROWCOUNT = "GetServiceCallCommissionRowCount";	
		private const string GETSERVICECALLCOMMISSIONBYQUERY = "GetServiceCallCommissionByQuery";
		#endregion
		
		#region Constructors
		public ServiceCallCommissionDataAccess(ClientContext context) : base(context) { }
		public ServiceCallCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="serviceCallCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, ServiceCallCommissionBase serviceCallCommissionObject)
		{	
			AddParameter(cmd, pGuid(ServiceCallCommissionBase.Property_ServiceCallCommissionId, serviceCallCommissionObject.ServiceCallCommissionId));
			AddParameter(cmd, pGuid(ServiceCallCommissionBase.Property_TicketId, serviceCallCommissionObject.TicketId));
			AddParameter(cmd, pGuid(ServiceCallCommissionBase.Property_CustomerId, serviceCallCommissionObject.CustomerId));
			AddParameter(cmd, pGuid(ServiceCallCommissionBase.Property_UserId, serviceCallCommissionObject.UserId));
			AddParameter(cmd, pDateTime(ServiceCallCommissionBase.Property_CompletionDate, serviceCallCommissionObject.CompletionDate));
			AddParameter(cmd, pDouble(ServiceCallCommissionBase.Property_Adjustment, serviceCallCommissionObject.Adjustment));
			AddParameter(cmd, pDouble(ServiceCallCommissionBase.Property_Commission, serviceCallCommissionObject.Commission));
			AddParameter(cmd, pBool(ServiceCallCommissionBase.Property_IsPaid, serviceCallCommissionObject.IsPaid));
			AddParameter(cmd, pGuid(ServiceCallCommissionBase.Property_CreatedBy, serviceCallCommissionObject.CreatedBy));
			AddParameter(cmd, pDateTime(ServiceCallCommissionBase.Property_CreatedDate, serviceCallCommissionObject.CreatedDate));
			AddParameter(cmd, pNVarChar(ServiceCallCommissionBase.Property_Batch, 50, serviceCallCommissionObject.Batch));
			AddParameter(cmd, pNVarChar(ServiceCallCommissionBase.Property_CommissionCalculation, serviceCallCommissionObject.CommissionCalculation));
			AddParameter(cmd, pDateTime(ServiceCallCommissionBase.Property_PaidDate, serviceCallCommissionObject.PaidDate));
			AddParameter(cmd, pBool(ServiceCallCommissionBase.Property_IsManual, serviceCallCommissionObject.IsManual));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ServiceCallCommission
        /// </summary>
        /// <param name="serviceCallCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ServiceCallCommissionBase serviceCallCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSERVICECALLCOMMISSION);
	
				AddParameter(cmd, pInt32Out(ServiceCallCommissionBase.Property_Id));
				AddCommonParams(cmd, serviceCallCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					serviceCallCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					serviceCallCommissionObject.Id = (Int32)GetOutParameter(cmd, ServiceCallCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(serviceCallCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ServiceCallCommission
        /// </summary>
        /// <param name="serviceCallCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ServiceCallCommissionBase serviceCallCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESERVICECALLCOMMISSION);
				
				AddParameter(cmd, pInt32(ServiceCallCommissionBase.Property_Id, serviceCallCommissionObject.Id));
				AddCommonParams(cmd, serviceCallCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					serviceCallCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(serviceCallCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ServiceCallCommission
        /// </summary>
        /// <param name="Id">Id of the ServiceCallCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESERVICECALLCOMMISSION);	
				
				AddParameter(cmd, pInt32(ServiceCallCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ServiceCallCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ServiceCallCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ServiceCallCommission object to retrieve</param>
        /// <returns>ServiceCallCommission object, null if not found</returns>
		public ServiceCallCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICECALLCOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(ServiceCallCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ServiceCallCommission objects 
        /// </summary>
        /// <returns>A list of ServiceCallCommission objects</returns>
		public ServiceCallCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSERVICECALLCOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ServiceCallCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of ServiceCallCommission objects</returns>
		public ServiceCallCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSERVICECALLCOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ServiceCallCommissionList _ServiceCallCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ServiceCallCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all ServiceCallCommission objects by query String
        /// </summary>
        /// <returns>A list of ServiceCallCommission objects</returns>
		public ServiceCallCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICECALLCOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ServiceCallCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ServiceCallCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICECALLCOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ServiceCallCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ServiceCallCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ServiceCallCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICECALLCOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_ServiceCallCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ServiceCallCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ServiceCallCommission object
        /// </summary>
        /// <param name="serviceCallCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ServiceCallCommissionBase serviceCallCommissionObject, SqlDataReader reader, int start)
		{
			
				serviceCallCommissionObject.Id = reader.GetInt32( start + 0 );			
				serviceCallCommissionObject.ServiceCallCommissionId = reader.GetGuid( start + 1 );			
				serviceCallCommissionObject.TicketId = reader.GetGuid( start + 2 );			
				serviceCallCommissionObject.CustomerId = reader.GetGuid( start + 3 );			
				serviceCallCommissionObject.UserId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) serviceCallCommissionObject.CompletionDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) serviceCallCommissionObject.Adjustment = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) serviceCallCommissionObject.Commission = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) serviceCallCommissionObject.IsPaid = reader.GetBoolean( start + 8 );			
				serviceCallCommissionObject.CreatedBy = reader.GetGuid( start + 9 );			
				serviceCallCommissionObject.CreatedDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) serviceCallCommissionObject.Batch = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) serviceCallCommissionObject.CommissionCalculation = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) serviceCallCommissionObject.PaidDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) serviceCallCommissionObject.IsManual = reader.GetBoolean( start + 14 );			
			FillBaseObject(serviceCallCommissionObject, reader, (start + 15));

			
			serviceCallCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ServiceCallCommission object
        /// </summary>
        /// <param name="serviceCallCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ServiceCallCommissionBase serviceCallCommissionObject, SqlDataReader reader)
		{
			FillObject(serviceCallCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ServiceCallCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ServiceCallCommission object</returns>
		private ServiceCallCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ServiceCallCommission serviceCallCommissionObject= new ServiceCallCommission();
					FillObject(serviceCallCommissionObject, reader);
					return serviceCallCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ServiceCallCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ServiceCallCommission objects</returns>
		private ServiceCallCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ServiceCallCommission list
			ServiceCallCommissionList list = new ServiceCallCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ServiceCallCommission serviceCallCommissionObject = new ServiceCallCommission();
					FillObject(serviceCallCommissionObject, reader);

					list.Add(serviceCallCommissionObject);
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
