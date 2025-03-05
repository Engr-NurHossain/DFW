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
	public partial class FollowUpCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTFOLLOWUPCOMMISSION = "InsertFollowUpCommission";
		private const string UPDATEFOLLOWUPCOMMISSION = "UpdateFollowUpCommission";
		private const string DELETEFOLLOWUPCOMMISSION = "DeleteFollowUpCommission";
		private const string GETFOLLOWUPCOMMISSIONBYID = "GetFollowUpCommissionById";
		private const string GETALLFOLLOWUPCOMMISSION = "GetAllFollowUpCommission";
		private const string GETPAGEDFOLLOWUPCOMMISSION = "GetPagedFollowUpCommission";
		private const string GETFOLLOWUPCOMMISSIONMAXIMUMID = "GetFollowUpCommissionMaximumId";
		private const string GETFOLLOWUPCOMMISSIONROWCOUNT = "GetFollowUpCommissionRowCount";	
		private const string GETFOLLOWUPCOMMISSIONBYQUERY = "GetFollowUpCommissionByQuery";
		#endregion
		
		#region Constructors
		public FollowUpCommissionDataAccess(ClientContext context) : base(context) { }
		public FollowUpCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="followUpCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, FollowUpCommissionBase followUpCommissionObject)
		{	
			AddParameter(cmd, pGuid(FollowUpCommissionBase.Property_FollowUpCommissionId, followUpCommissionObject.FollowUpCommissionId));
			AddParameter(cmd, pGuid(FollowUpCommissionBase.Property_TicketId, followUpCommissionObject.TicketId));
			AddParameter(cmd, pGuid(FollowUpCommissionBase.Property_CustomerId, followUpCommissionObject.CustomerId));
			AddParameter(cmd, pGuid(FollowUpCommissionBase.Property_UserId, followUpCommissionObject.UserId));
			AddParameter(cmd, pDateTime(FollowUpCommissionBase.Property_CompletionDate, followUpCommissionObject.CompletionDate));
			AddParameter(cmd, pDouble(FollowUpCommissionBase.Property_Adjustment, followUpCommissionObject.Adjustment));
			AddParameter(cmd, pDouble(FollowUpCommissionBase.Property_Commission, followUpCommissionObject.Commission));
			AddParameter(cmd, pBool(FollowUpCommissionBase.Property_IsPaid, followUpCommissionObject.IsPaid));
			AddParameter(cmd, pGuid(FollowUpCommissionBase.Property_CreatedBy, followUpCommissionObject.CreatedBy));
			AddParameter(cmd, pDateTime(FollowUpCommissionBase.Property_CreatedDate, followUpCommissionObject.CreatedDate));
			AddParameter(cmd, pNVarChar(FollowUpCommissionBase.Property_Batch, 50, followUpCommissionObject.Batch));
			AddParameter(cmd, pNVarChar(FollowUpCommissionBase.Property_CommissionCalculation, followUpCommissionObject.CommissionCalculation));
			AddParameter(cmd, pDateTime(FollowUpCommissionBase.Property_PaidDate, followUpCommissionObject.PaidDate));
			AddParameter(cmd, pBool(FollowUpCommissionBase.Property_IsManual, followUpCommissionObject.IsManual));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts FollowUpCommission
        /// </summary>
        /// <param name="followUpCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(FollowUpCommissionBase followUpCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTFOLLOWUPCOMMISSION);
	
				AddParameter(cmd, pInt32Out(FollowUpCommissionBase.Property_Id));
				AddCommonParams(cmd, followUpCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					followUpCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					followUpCommissionObject.Id = (Int32)GetOutParameter(cmd, FollowUpCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(followUpCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates FollowUpCommission
        /// </summary>
        /// <param name="followUpCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(FollowUpCommissionBase followUpCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEFOLLOWUPCOMMISSION);
				
				AddParameter(cmd, pInt32(FollowUpCommissionBase.Property_Id, followUpCommissionObject.Id));
				AddCommonParams(cmd, followUpCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					followUpCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(followUpCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes FollowUpCommission
        /// </summary>
        /// <param name="Id">Id of the FollowUpCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEFOLLOWUPCOMMISSION);	
				
				AddParameter(cmd, pInt32(FollowUpCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(FollowUpCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves FollowUpCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the FollowUpCommission object to retrieve</param>
        /// <returns>FollowUpCommission object, null if not found</returns>
		public FollowUpCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETFOLLOWUPCOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(FollowUpCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all FollowUpCommission objects 
        /// </summary>
        /// <returns>A list of FollowUpCommission objects</returns>
		public FollowUpCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLFOLLOWUPCOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all FollowUpCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of FollowUpCommission objects</returns>
		public FollowUpCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDFOLLOWUPCOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				FollowUpCommissionList _FollowUpCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _FollowUpCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all FollowUpCommission objects by query String
        /// </summary>
        /// <returns>A list of FollowUpCommission objects</returns>
		public FollowUpCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETFOLLOWUPCOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get FollowUpCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of FollowUpCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFOLLOWUPCOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get FollowUpCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of FollowUpCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _FollowUpCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFOLLOWUPCOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_FollowUpCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _FollowUpCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills FollowUpCommission object
        /// </summary>
        /// <param name="followUpCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(FollowUpCommissionBase followUpCommissionObject, SqlDataReader reader, int start)
		{
			
				followUpCommissionObject.Id = reader.GetInt32( start + 0 );			
				followUpCommissionObject.FollowUpCommissionId = reader.GetGuid( start + 1 );			
				followUpCommissionObject.TicketId = reader.GetGuid( start + 2 );			
				followUpCommissionObject.CustomerId = reader.GetGuid( start + 3 );			
				followUpCommissionObject.UserId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) followUpCommissionObject.CompletionDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) followUpCommissionObject.Adjustment = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) followUpCommissionObject.Commission = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) followUpCommissionObject.IsPaid = reader.GetBoolean( start + 8 );			
				followUpCommissionObject.CreatedBy = reader.GetGuid( start + 9 );			
				followUpCommissionObject.CreatedDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) followUpCommissionObject.Batch = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) followUpCommissionObject.CommissionCalculation = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) followUpCommissionObject.PaidDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) followUpCommissionObject.IsManual = reader.GetBoolean( start + 14 );			
			FillBaseObject(followUpCommissionObject, reader, (start + 15));

			
			followUpCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills FollowUpCommission object
        /// </summary>
        /// <param name="followUpCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(FollowUpCommissionBase followUpCommissionObject, SqlDataReader reader)
		{
			FillObject(followUpCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves FollowUpCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>FollowUpCommission object</returns>
		private FollowUpCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					FollowUpCommission followUpCommissionObject= new FollowUpCommission();
					FillObject(followUpCommissionObject, reader);
					return followUpCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of FollowUpCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of FollowUpCommission objects</returns>
		private FollowUpCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//FollowUpCommission list
			FollowUpCommissionList list = new FollowUpCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					FollowUpCommission followUpCommissionObject = new FollowUpCommission();
					FillObject(followUpCommissionObject, reader);

					list.Add(followUpCommissionObject);
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
