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
	public partial class SalesCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSALESCOMMISSION = "InsertSalesCommission";
		private const string UPDATESALESCOMMISSION = "UpdateSalesCommission";
		private const string DELETESALESCOMMISSION = "DeleteSalesCommission";
		private const string GETSALESCOMMISSIONBYID = "GetSalesCommissionById";
		private const string GETALLSALESCOMMISSION = "GetAllSalesCommission";
		private const string GETPAGEDSALESCOMMISSION = "GetPagedSalesCommission";
		private const string GETSALESCOMMISSIONMAXIMUMID = "GetSalesCommissionMaximumId";
		private const string GETSALESCOMMISSIONROWCOUNT = "GetSalesCommissionRowCount";	
		private const string GETSALESCOMMISSIONBYQUERY = "GetSalesCommissionByQuery";
		#endregion
		
		#region Constructors
		public SalesCommissionDataAccess(ClientContext context) : base(context) { }
		public SalesCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="salesCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, SalesCommissionBase salesCommissionObject)
		{	
			AddParameter(cmd, pGuid(SalesCommissionBase.Property_SalesCommissionId, salesCommissionObject.SalesCommissionId));
			AddParameter(cmd, pGuid(SalesCommissionBase.Property_TicketId, salesCommissionObject.TicketId));
			AddParameter(cmd, pGuid(SalesCommissionBase.Property_CustomerId, salesCommissionObject.CustomerId));
			AddParameter(cmd, pGuid(SalesCommissionBase.Property_UserId, salesCommissionObject.UserId));
			AddParameter(cmd, pDateTime(SalesCommissionBase.Property_CompletionDate, salesCommissionObject.CompletionDate));
			AddParameter(cmd, pDouble(SalesCommissionBase.Property_RMRSold, salesCommissionObject.RMRSold));
			AddParameter(cmd, pDouble(SalesCommissionBase.Property_RMRCommission, salesCommissionObject.RMRCommission));
			AddParameter(cmd, pInt32(SalesCommissionBase.Property_NoOfEquipment, salesCommissionObject.NoOfEquipment));
			AddParameter(cmd, pDouble(SalesCommissionBase.Property_EquipmentCommission, salesCommissionObject.EquipmentCommission));
			AddParameter(cmd, pDouble(SalesCommissionBase.Property_TotalCommission, salesCommissionObject.TotalCommission));
			AddParameter(cmd, pBool(SalesCommissionBase.Property_IsPaid, salesCommissionObject.IsPaid));
			AddParameter(cmd, pGuid(SalesCommissionBase.Property_CreatedBy, salesCommissionObject.CreatedBy));
			AddParameter(cmd, pDateTime(SalesCommissionBase.Property_CreatedDate, salesCommissionObject.CreatedDate));
			AddParameter(cmd, pNVarChar(SalesCommissionBase.Property_Batch, 50, salesCommissionObject.Batch));
			AddParameter(cmd, pDouble(SalesCommissionBase.Property_Adjustment, salesCommissionObject.Adjustment));
			AddParameter(cmd, pNVarChar(SalesCommissionBase.Property_RMRCommissionCalculation, salesCommissionObject.RMRCommissionCalculation));
			AddParameter(cmd, pNVarChar(SalesCommissionBase.Property_EquipmentCommissionCalculation, salesCommissionObject.EquipmentCommissionCalculation));
			AddParameter(cmd, pDateTime(SalesCommissionBase.Property_PaidDate, salesCommissionObject.PaidDate));
			AddParameter(cmd, pBool(SalesCommissionBase.Property_IsPermanent, salesCommissionObject.IsPermanent));
			AddParameter(cmd, pDouble(SalesCommissionBase.Property_OriginalPoint, salesCommissionObject.OriginalPoint));
			AddParameter(cmd, pDouble(SalesCommissionBase.Property_AdjustablePoint, salesCommissionObject.AdjustablePoint));
			AddParameter(cmd, pDouble(SalesCommissionBase.Property_TotalPoint, salesCommissionObject.TotalPoint));
			AddParameter(cmd, pBool(SalesCommissionBase.Property_IsSealed, salesCommissionObject.IsSealed));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SalesCommission
        /// </summary>
        /// <param name="salesCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SalesCommissionBase salesCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSALESCOMMISSION);
	
				AddParameter(cmd, pInt32Out(SalesCommissionBase.Property_Id));
				AddCommonParams(cmd, salesCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					salesCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					salesCommissionObject.Id = (Int32)GetOutParameter(cmd, SalesCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(salesCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SalesCommission
        /// </summary>
        /// <param name="salesCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SalesCommissionBase salesCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESALESCOMMISSION);
				
				AddParameter(cmd, pInt32(SalesCommissionBase.Property_Id, salesCommissionObject.Id));
				AddCommonParams(cmd, salesCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					salesCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(salesCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SalesCommission
        /// </summary>
        /// <param name="Id">Id of the SalesCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESALESCOMMISSION);	
				
				AddParameter(cmd, pInt32(SalesCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SalesCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SalesCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SalesCommission object to retrieve</param>
        /// <returns>SalesCommission object, null if not found</returns>
		public SalesCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSALESCOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(SalesCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SalesCommission objects 
        /// </summary>
        /// <returns>A list of SalesCommission objects</returns>
		public SalesCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSALESCOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SalesCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of SalesCommission objects</returns>
		public SalesCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSALESCOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SalesCommissionList _SalesCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SalesCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all SalesCommission objects by query String
        /// </summary>
        /// <returns>A list of SalesCommission objects</returns>
		public SalesCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSALESCOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SalesCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SalesCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSALESCOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SalesCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SalesCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SalesCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSALESCOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_SalesCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SalesCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SalesCommission object
        /// </summary>
        /// <param name="salesCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SalesCommissionBase salesCommissionObject, SqlDataReader reader, int start)
		{
			
				salesCommissionObject.Id = reader.GetInt32( start + 0 );			
				salesCommissionObject.SalesCommissionId = reader.GetGuid( start + 1 );			
				salesCommissionObject.TicketId = reader.GetGuid( start + 2 );			
				salesCommissionObject.CustomerId = reader.GetGuid( start + 3 );			
				salesCommissionObject.UserId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) salesCommissionObject.CompletionDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) salesCommissionObject.RMRSold = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) salesCommissionObject.RMRCommission = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) salesCommissionObject.NoOfEquipment = reader.GetInt32( start + 8 );			
				if(!reader.IsDBNull(9)) salesCommissionObject.EquipmentCommission = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) salesCommissionObject.TotalCommission = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) salesCommissionObject.IsPaid = reader.GetBoolean( start + 11 );			
				salesCommissionObject.CreatedBy = reader.GetGuid( start + 12 );			
				salesCommissionObject.CreatedDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) salesCommissionObject.Batch = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) salesCommissionObject.Adjustment = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) salesCommissionObject.RMRCommissionCalculation = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) salesCommissionObject.EquipmentCommissionCalculation = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) salesCommissionObject.PaidDate = reader.GetDateTime( start + 18 );			
				if(!reader.IsDBNull(19)) salesCommissionObject.IsPermanent = reader.GetBoolean( start + 19 );			
				if(!reader.IsDBNull(20)) salesCommissionObject.OriginalPoint = reader.GetDouble( start + 20 );			
				if(!reader.IsDBNull(21)) salesCommissionObject.AdjustablePoint = reader.GetDouble( start + 21 );			
				if(!reader.IsDBNull(22)) salesCommissionObject.TotalPoint = reader.GetDouble( start + 22 );			
				if(!reader.IsDBNull(23)) salesCommissionObject.IsSealed = reader.GetBoolean( start + 23 );			
			FillBaseObject(salesCommissionObject, reader, (start + 24));

			
			salesCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SalesCommission object
        /// </summary>
        /// <param name="salesCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SalesCommissionBase salesCommissionObject, SqlDataReader reader)
		{
			FillObject(salesCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SalesCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SalesCommission object</returns>
		private SalesCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SalesCommission salesCommissionObject= new SalesCommission();
					FillObject(salesCommissionObject, reader);
					return salesCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SalesCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SalesCommission objects</returns>
		private SalesCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SalesCommission list
			SalesCommissionList list = new SalesCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SalesCommission salesCommissionObject = new SalesCommission();
					FillObject(salesCommissionObject, reader);

					list.Add(salesCommissionObject);
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
