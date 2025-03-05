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
	public partial class RescheduleCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESCHEDULECOMMISSION = "InsertRescheduleCommission";
		private const string UPDATERESCHEDULECOMMISSION = "UpdateRescheduleCommission";
		private const string DELETERESCHEDULECOMMISSION = "DeleteRescheduleCommission";
		private const string GETRESCHEDULECOMMISSIONBYID = "GetRescheduleCommissionById";
		private const string GETALLRESCHEDULECOMMISSION = "GetAllRescheduleCommission";
		private const string GETPAGEDRESCHEDULECOMMISSION = "GetPagedRescheduleCommission";
		private const string GETRESCHEDULECOMMISSIONMAXIMUMID = "GetRescheduleCommissionMaximumId";
		private const string GETRESCHEDULECOMMISSIONROWCOUNT = "GetRescheduleCommissionRowCount";	
		private const string GETRESCHEDULECOMMISSIONBYQUERY = "GetRescheduleCommissionByQuery";
		#endregion
		
		#region Constructors
		public RescheduleCommissionDataAccess(ClientContext context) : base(context) { }
		public RescheduleCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="rescheduleCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, RescheduleCommissionBase rescheduleCommissionObject)
		{	
			AddParameter(cmd, pGuid(RescheduleCommissionBase.Property_RescheduleCommissionId, rescheduleCommissionObject.RescheduleCommissionId));
			AddParameter(cmd, pGuid(RescheduleCommissionBase.Property_TicketId, rescheduleCommissionObject.TicketId));
			AddParameter(cmd, pGuid(RescheduleCommissionBase.Property_CustomerId, rescheduleCommissionObject.CustomerId));
			AddParameter(cmd, pGuid(RescheduleCommissionBase.Property_UserId, rescheduleCommissionObject.UserId));
			AddParameter(cmd, pDateTime(RescheduleCommissionBase.Property_CompletionDate, rescheduleCommissionObject.CompletionDate));
			AddParameter(cmd, pDouble(RescheduleCommissionBase.Property_Adjustment, rescheduleCommissionObject.Adjustment));
			AddParameter(cmd, pDouble(RescheduleCommissionBase.Property_Commission, rescheduleCommissionObject.Commission));
			AddParameter(cmd, pBool(RescheduleCommissionBase.Property_IsPaid, rescheduleCommissionObject.IsPaid));
			AddParameter(cmd, pGuid(RescheduleCommissionBase.Property_CreatedBy, rescheduleCommissionObject.CreatedBy));
			AddParameter(cmd, pDateTime(RescheduleCommissionBase.Property_CreatedDate, rescheduleCommissionObject.CreatedDate));
			AddParameter(cmd, pNVarChar(RescheduleCommissionBase.Property_Batch, 50, rescheduleCommissionObject.Batch));
			AddParameter(cmd, pNVarChar(RescheduleCommissionBase.Property_CommissionCalculation, rescheduleCommissionObject.CommissionCalculation));
			AddParameter(cmd, pDateTime(RescheduleCommissionBase.Property_PaidDate, rescheduleCommissionObject.PaidDate));
			AddParameter(cmd, pBool(RescheduleCommissionBase.Property_IsManual, rescheduleCommissionObject.IsManual));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RescheduleCommission
        /// </summary>
        /// <param name="rescheduleCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RescheduleCommissionBase rescheduleCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESCHEDULECOMMISSION);
	
				AddParameter(cmd, pInt32Out(RescheduleCommissionBase.Property_Id));
				AddCommonParams(cmd, rescheduleCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					rescheduleCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					rescheduleCommissionObject.Id = (Int32)GetOutParameter(cmd, RescheduleCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(rescheduleCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RescheduleCommission
        /// </summary>
        /// <param name="rescheduleCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RescheduleCommissionBase rescheduleCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESCHEDULECOMMISSION);
				
				AddParameter(cmd, pInt32(RescheduleCommissionBase.Property_Id, rescheduleCommissionObject.Id));
				AddCommonParams(cmd, rescheduleCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					rescheduleCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(rescheduleCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RescheduleCommission
        /// </summary>
        /// <param name="Id">Id of the RescheduleCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESCHEDULECOMMISSION);	
				
				AddParameter(cmd, pInt32(RescheduleCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RescheduleCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RescheduleCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RescheduleCommission object to retrieve</param>
        /// <returns>RescheduleCommission object, null if not found</returns>
		public RescheduleCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESCHEDULECOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(RescheduleCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RescheduleCommission objects 
        /// </summary>
        /// <returns>A list of RescheduleCommission objects</returns>
		public RescheduleCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESCHEDULECOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RescheduleCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of RescheduleCommission objects</returns>
		public RescheduleCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESCHEDULECOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RescheduleCommissionList _RescheduleCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RescheduleCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all RescheduleCommission objects by query String
        /// </summary>
        /// <returns>A list of RescheduleCommission objects</returns>
		public RescheduleCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESCHEDULECOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RescheduleCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RescheduleCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESCHEDULECOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RescheduleCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RescheduleCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RescheduleCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESCHEDULECOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_RescheduleCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RescheduleCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RescheduleCommission object
        /// </summary>
        /// <param name="rescheduleCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RescheduleCommissionBase rescheduleCommissionObject, SqlDataReader reader, int start)
		{
			
				rescheduleCommissionObject.Id = reader.GetInt32( start + 0 );			
				rescheduleCommissionObject.RescheduleCommissionId = reader.GetGuid( start + 1 );			
				rescheduleCommissionObject.TicketId = reader.GetGuid( start + 2 );			
				rescheduleCommissionObject.CustomerId = reader.GetGuid( start + 3 );			
				rescheduleCommissionObject.UserId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) rescheduleCommissionObject.CompletionDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) rescheduleCommissionObject.Adjustment = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) rescheduleCommissionObject.Commission = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) rescheduleCommissionObject.IsPaid = reader.GetBoolean( start + 8 );			
				rescheduleCommissionObject.CreatedBy = reader.GetGuid( start + 9 );			
				rescheduleCommissionObject.CreatedDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) rescheduleCommissionObject.Batch = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) rescheduleCommissionObject.CommissionCalculation = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) rescheduleCommissionObject.PaidDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) rescheduleCommissionObject.IsManual = reader.GetBoolean( start + 14 );			
			FillBaseObject(rescheduleCommissionObject, reader, (start + 15));

			
			rescheduleCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RescheduleCommission object
        /// </summary>
        /// <param name="rescheduleCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RescheduleCommissionBase rescheduleCommissionObject, SqlDataReader reader)
		{
			FillObject(rescheduleCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RescheduleCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RescheduleCommission object</returns>
		private RescheduleCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RescheduleCommission rescheduleCommissionObject= new RescheduleCommission();
					FillObject(rescheduleCommissionObject, reader);
					return rescheduleCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RescheduleCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RescheduleCommission objects</returns>
		private RescheduleCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RescheduleCommission list
			RescheduleCommissionList list = new RescheduleCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RescheduleCommission rescheduleCommissionObject = new RescheduleCommission();
					FillObject(rescheduleCommissionObject, reader);

					list.Add(rescheduleCommissionObject);
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
