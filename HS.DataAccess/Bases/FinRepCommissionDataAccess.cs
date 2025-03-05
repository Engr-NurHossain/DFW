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
	public partial class FinRepCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTFINREPCOMMISSION = "InsertFinRepCommission";
		private const string UPDATEFINREPCOMMISSION = "UpdateFinRepCommission";
		private const string DELETEFINREPCOMMISSION = "DeleteFinRepCommission";
		private const string GETFINREPCOMMISSIONBYID = "GetFinRepCommissionById";
		private const string GETALLFINREPCOMMISSION = "GetAllFinRepCommission";
		private const string GETPAGEDFINREPCOMMISSION = "GetPagedFinRepCommission";
		private const string GETFINREPCOMMISSIONMAXIMUMID = "GetFinRepCommissionMaximumId";
		private const string GETFINREPCOMMISSIONROWCOUNT = "GetFinRepCommissionRowCount";	
		private const string GETFINREPCOMMISSIONBYQUERY = "GetFinRepCommissionByQuery";
		#endregion
		
		#region Constructors
		public FinRepCommissionDataAccess(ClientContext context) : base(context) { }
		public FinRepCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="finRepCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, FinRepCommissionBase finRepCommissionObject)
		{	
			AddParameter(cmd, pGuid(FinRepCommissionBase.Property_FinRepCommissionId, finRepCommissionObject.FinRepCommissionId));
			AddParameter(cmd, pGuid(FinRepCommissionBase.Property_TicketId, finRepCommissionObject.TicketId));
			AddParameter(cmd, pGuid(FinRepCommissionBase.Property_CustomerId, finRepCommissionObject.CustomerId));
			AddParameter(cmd, pGuid(FinRepCommissionBase.Property_UserId, finRepCommissionObject.UserId));
			AddParameter(cmd, pDateTime(FinRepCommissionBase.Property_CompletionDate, finRepCommissionObject.CompletionDate));
			AddParameter(cmd, pDouble(FinRepCommissionBase.Property_Adjustment, finRepCommissionObject.Adjustment));
			AddParameter(cmd, pDouble(FinRepCommissionBase.Property_Commission, finRepCommissionObject.Commission));
			AddParameter(cmd, pBool(FinRepCommissionBase.Property_IsPaid, finRepCommissionObject.IsPaid));
			AddParameter(cmd, pGuid(FinRepCommissionBase.Property_CreatedBy, finRepCommissionObject.CreatedBy));
			AddParameter(cmd, pDateTime(FinRepCommissionBase.Property_CreatedDate, finRepCommissionObject.CreatedDate));
			AddParameter(cmd, pNVarChar(FinRepCommissionBase.Property_Batch, 50, finRepCommissionObject.Batch));
			AddParameter(cmd, pNVarChar(FinRepCommissionBase.Property_CommissionCalculation, finRepCommissionObject.CommissionCalculation));
			AddParameter(cmd, pDateTime(FinRepCommissionBase.Property_PaidDate, finRepCommissionObject.PaidDate));
			AddParameter(cmd, pBool(FinRepCommissionBase.Property_IsManual, finRepCommissionObject.IsManual));
			AddParameter(cmd, pDouble(FinRepCommissionBase.Property_OriginalPoint, finRepCommissionObject.OriginalPoint));
			AddParameter(cmd, pDouble(FinRepCommissionBase.Property_AdjustablePoint, finRepCommissionObject.AdjustablePoint));
			AddParameter(cmd, pDouble(FinRepCommissionBase.Property_TotalPoint, finRepCommissionObject.TotalPoint));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts FinRepCommission
        /// </summary>
        /// <param name="finRepCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(FinRepCommissionBase finRepCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTFINREPCOMMISSION);
	
				AddParameter(cmd, pInt32Out(FinRepCommissionBase.Property_Id));
				AddCommonParams(cmd, finRepCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					finRepCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					finRepCommissionObject.Id = (Int32)GetOutParameter(cmd, FinRepCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(finRepCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates FinRepCommission
        /// </summary>
        /// <param name="finRepCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(FinRepCommissionBase finRepCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEFINREPCOMMISSION);
				
				AddParameter(cmd, pInt32(FinRepCommissionBase.Property_Id, finRepCommissionObject.Id));
				AddCommonParams(cmd, finRepCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					finRepCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(finRepCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes FinRepCommission
        /// </summary>
        /// <param name="Id">Id of the FinRepCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEFINREPCOMMISSION);	
				
				AddParameter(cmd, pInt32(FinRepCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(FinRepCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves FinRepCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the FinRepCommission object to retrieve</param>
        /// <returns>FinRepCommission object, null if not found</returns>
		public FinRepCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETFINREPCOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(FinRepCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all FinRepCommission objects 
        /// </summary>
        /// <returns>A list of FinRepCommission objects</returns>
		public FinRepCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLFINREPCOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all FinRepCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of FinRepCommission objects</returns>
		public FinRepCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDFINREPCOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				FinRepCommissionList _FinRepCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _FinRepCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all FinRepCommission objects by query String
        /// </summary>
        /// <returns>A list of FinRepCommission objects</returns>
		public FinRepCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETFINREPCOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get FinRepCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of FinRepCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFINREPCOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get FinRepCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of FinRepCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _FinRepCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFINREPCOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_FinRepCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _FinRepCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills FinRepCommission object
        /// </summary>
        /// <param name="finRepCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(FinRepCommissionBase finRepCommissionObject, SqlDataReader reader, int start)
		{
			
				finRepCommissionObject.Id = reader.GetInt32( start + 0 );			
				finRepCommissionObject.FinRepCommissionId = reader.GetGuid( start + 1 );			
				finRepCommissionObject.TicketId = reader.GetGuid( start + 2 );			
				finRepCommissionObject.CustomerId = reader.GetGuid( start + 3 );			
				finRepCommissionObject.UserId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) finRepCommissionObject.CompletionDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) finRepCommissionObject.Adjustment = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) finRepCommissionObject.Commission = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) finRepCommissionObject.IsPaid = reader.GetBoolean( start + 8 );			
				finRepCommissionObject.CreatedBy = reader.GetGuid( start + 9 );			
				finRepCommissionObject.CreatedDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) finRepCommissionObject.Batch = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) finRepCommissionObject.CommissionCalculation = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) finRepCommissionObject.PaidDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) finRepCommissionObject.IsManual = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) finRepCommissionObject.OriginalPoint = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) finRepCommissionObject.AdjustablePoint = reader.GetDouble( start + 16 );			
				if(!reader.IsDBNull(17)) finRepCommissionObject.TotalPoint = reader.GetDouble( start + 17 );			
			FillBaseObject(finRepCommissionObject, reader, (start + 18));

			
			finRepCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills FinRepCommission object
        /// </summary>
        /// <param name="finRepCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(FinRepCommissionBase finRepCommissionObject, SqlDataReader reader)
		{
			FillObject(finRepCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves FinRepCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>FinRepCommission object</returns>
		private FinRepCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					FinRepCommission finRepCommissionObject= new FinRepCommission();
					FillObject(finRepCommissionObject, reader);
					return finRepCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of FinRepCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of FinRepCommission objects</returns>
		private FinRepCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//FinRepCommission list
			FinRepCommissionList list = new FinRepCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					FinRepCommission finRepCommissionObject = new FinRepCommission();
					FillObject(finRepCommissionObject, reader);

					list.Add(finRepCommissionObject);
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
