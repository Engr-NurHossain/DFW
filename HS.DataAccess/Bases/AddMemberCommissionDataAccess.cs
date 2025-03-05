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
	public partial class AddMemberCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTADDMEMBERCOMMISSION = "InsertAddMemberCommission";
		private const string UPDATEADDMEMBERCOMMISSION = "UpdateAddMemberCommission";
		private const string DELETEADDMEMBERCOMMISSION = "DeleteAddMemberCommission";
		private const string GETADDMEMBERCOMMISSIONBYID = "GetAddMemberCommissionById";
		private const string GETALLADDMEMBERCOMMISSION = "GetAllAddMemberCommission";
		private const string GETPAGEDADDMEMBERCOMMISSION = "GetPagedAddMemberCommission";
		private const string GETADDMEMBERCOMMISSIONMAXIMUMID = "GetAddMemberCommissionMaximumId";
		private const string GETADDMEMBERCOMMISSIONROWCOUNT = "GetAddMemberCommissionRowCount";	
		private const string GETADDMEMBERCOMMISSIONBYQUERY = "GetAddMemberCommissionByQuery";
		#endregion
		
		#region Constructors
		public AddMemberCommissionDataAccess(ClientContext context) : base(context) { }
		public AddMemberCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="addMemberCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, AddMemberCommissionBase addMemberCommissionObject)
		{	
			AddParameter(cmd, pGuid(AddMemberCommissionBase.Property_AddMemberCommissionId, addMemberCommissionObject.AddMemberCommissionId));
			AddParameter(cmd, pGuid(AddMemberCommissionBase.Property_TicketId, addMemberCommissionObject.TicketId));
			AddParameter(cmd, pGuid(AddMemberCommissionBase.Property_CustomerId, addMemberCommissionObject.CustomerId));
			AddParameter(cmd, pGuid(AddMemberCommissionBase.Property_UserId, addMemberCommissionObject.UserId));
			AddParameter(cmd, pDateTime(AddMemberCommissionBase.Property_CompletionDate, addMemberCommissionObject.CompletionDate));
			AddParameter(cmd, pDouble(AddMemberCommissionBase.Property_Adjustment, addMemberCommissionObject.Adjustment));
			AddParameter(cmd, pDouble(AddMemberCommissionBase.Property_Commission, addMemberCommissionObject.Commission));
			AddParameter(cmd, pBool(AddMemberCommissionBase.Property_IsPaid, addMemberCommissionObject.IsPaid));
			AddParameter(cmd, pGuid(AddMemberCommissionBase.Property_CreatedBy, addMemberCommissionObject.CreatedBy));
			AddParameter(cmd, pDateTime(AddMemberCommissionBase.Property_CreatedDate, addMemberCommissionObject.CreatedDate));
			AddParameter(cmd, pNVarChar(AddMemberCommissionBase.Property_Batch, 50, addMemberCommissionObject.Batch));
			AddParameter(cmd, pNVarChar(AddMemberCommissionBase.Property_CommissionCalculation, addMemberCommissionObject.CommissionCalculation));
			AddParameter(cmd, pDateTime(AddMemberCommissionBase.Property_PaidDate, addMemberCommissionObject.PaidDate));
			AddParameter(cmd, pBool(AddMemberCommissionBase.Property_IsManual, addMemberCommissionObject.IsManual));
			AddParameter(cmd, pDouble(AddMemberCommissionBase.Property_OriginalPoint, addMemberCommissionObject.OriginalPoint));
			AddParameter(cmd, pDouble(AddMemberCommissionBase.Property_AdjustablePoint, addMemberCommissionObject.AdjustablePoint));
			AddParameter(cmd, pDouble(AddMemberCommissionBase.Property_TotalPoint, addMemberCommissionObject.TotalPoint));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AddMemberCommission
        /// </summary>
        /// <param name="addMemberCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AddMemberCommissionBase addMemberCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTADDMEMBERCOMMISSION);
	
				AddParameter(cmd, pInt32Out(AddMemberCommissionBase.Property_Id));
				AddCommonParams(cmd, addMemberCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					addMemberCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					addMemberCommissionObject.Id = (Int32)GetOutParameter(cmd, AddMemberCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(addMemberCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AddMemberCommission
        /// </summary>
        /// <param name="addMemberCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AddMemberCommissionBase addMemberCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEADDMEMBERCOMMISSION);
				
				AddParameter(cmd, pInt32(AddMemberCommissionBase.Property_Id, addMemberCommissionObject.Id));
				AddCommonParams(cmd, addMemberCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					addMemberCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(addMemberCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AddMemberCommission
        /// </summary>
        /// <param name="Id">Id of the AddMemberCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEADDMEMBERCOMMISSION);	
				
				AddParameter(cmd, pInt32(AddMemberCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AddMemberCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AddMemberCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AddMemberCommission object to retrieve</param>
        /// <returns>AddMemberCommission object, null if not found</returns>
		public AddMemberCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETADDMEMBERCOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(AddMemberCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AddMemberCommission objects 
        /// </summary>
        /// <returns>A list of AddMemberCommission objects</returns>
		public AddMemberCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLADDMEMBERCOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AddMemberCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of AddMemberCommission objects</returns>
		public AddMemberCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDADDMEMBERCOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AddMemberCommissionList _AddMemberCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AddMemberCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all AddMemberCommission objects by query String
        /// </summary>
        /// <returns>A list of AddMemberCommission objects</returns>
		public AddMemberCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETADDMEMBERCOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AddMemberCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AddMemberCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADDMEMBERCOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AddMemberCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AddMemberCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AddMemberCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADDMEMBERCOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_AddMemberCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AddMemberCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AddMemberCommission object
        /// </summary>
        /// <param name="addMemberCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AddMemberCommissionBase addMemberCommissionObject, SqlDataReader reader, int start)
		{
			
				addMemberCommissionObject.Id = reader.GetInt32( start + 0 );			
				addMemberCommissionObject.AddMemberCommissionId = reader.GetGuid( start + 1 );			
				addMemberCommissionObject.TicketId = reader.GetGuid( start + 2 );			
				addMemberCommissionObject.CustomerId = reader.GetGuid( start + 3 );			
				addMemberCommissionObject.UserId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) addMemberCommissionObject.CompletionDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) addMemberCommissionObject.Adjustment = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) addMemberCommissionObject.Commission = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) addMemberCommissionObject.IsPaid = reader.GetBoolean( start + 8 );			
				addMemberCommissionObject.CreatedBy = reader.GetGuid( start + 9 );			
				addMemberCommissionObject.CreatedDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) addMemberCommissionObject.Batch = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) addMemberCommissionObject.CommissionCalculation = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) addMemberCommissionObject.PaidDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) addMemberCommissionObject.IsManual = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) addMemberCommissionObject.OriginalPoint = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) addMemberCommissionObject.AdjustablePoint = reader.GetDouble( start + 16 );			
				if(!reader.IsDBNull(17)) addMemberCommissionObject.TotalPoint = reader.GetDouble( start + 17 );			
			FillBaseObject(addMemberCommissionObject, reader, (start + 18));

			
			addMemberCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AddMemberCommission object
        /// </summary>
        /// <param name="addMemberCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AddMemberCommissionBase addMemberCommissionObject, SqlDataReader reader)
		{
			FillObject(addMemberCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AddMemberCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AddMemberCommission object</returns>
		private AddMemberCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AddMemberCommission addMemberCommissionObject= new AddMemberCommission();
					FillObject(addMemberCommissionObject, reader);
					return addMemberCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AddMemberCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AddMemberCommission objects</returns>
		private AddMemberCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AddMemberCommission list
			AddMemberCommissionList list = new AddMemberCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AddMemberCommission addMemberCommissionObject = new AddMemberCommission();
					FillObject(addMemberCommissionObject, reader);

					list.Add(addMemberCommissionObject);
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
