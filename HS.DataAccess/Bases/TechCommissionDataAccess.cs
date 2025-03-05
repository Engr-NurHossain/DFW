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
	public partial class TechCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTECHCOMMISSION = "InsertTechCommission";
		private const string UPDATETECHCOMMISSION = "UpdateTechCommission";
		private const string DELETETECHCOMMISSION = "DeleteTechCommission";
		private const string GETTECHCOMMISSIONBYID = "GetTechCommissionById";
		private const string GETALLTECHCOMMISSION = "GetAllTechCommission";
		private const string GETPAGEDTECHCOMMISSION = "GetPagedTechCommission";
		private const string GETTECHCOMMISSIONMAXIMUMID = "GetTechCommissionMaximumId";
		private const string GETTECHCOMMISSIONROWCOUNT = "GetTechCommissionRowCount";	
		private const string GETTECHCOMMISSIONBYQUERY = "GetTechCommissionByQuery";
		#endregion
		
		#region Constructors
		public TechCommissionDataAccess(ClientContext context) : base(context) { }
		public TechCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="techCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, TechCommissionBase techCommissionObject)
		{	
			AddParameter(cmd, pGuid(TechCommissionBase.Property_TechCommissionId, techCommissionObject.TechCommissionId));
			AddParameter(cmd, pGuid(TechCommissionBase.Property_TicketId, techCommissionObject.TicketId));
			AddParameter(cmd, pGuid(TechCommissionBase.Property_CustomerId, techCommissionObject.CustomerId));
			AddParameter(cmd, pGuid(TechCommissionBase.Property_UserId, techCommissionObject.UserId));
			AddParameter(cmd, pDateTime(TechCommissionBase.Property_CompletionDate, techCommissionObject.CompletionDate));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_BaseRMR, techCommissionObject.BaseRMR));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_BaseRMRCommission, techCommissionObject.BaseRMRCommission));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_AddedRMR, techCommissionObject.AddedRMR));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_AddedRMRCommission, techCommissionObject.AddedRMRCommission));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_TotalCommission, techCommissionObject.TotalCommission));
			AddParameter(cmd, pBool(TechCommissionBase.Property_IsPaid, techCommissionObject.IsPaid));
			AddParameter(cmd, pGuid(TechCommissionBase.Property_CreatedBy, techCommissionObject.CreatedBy));
			AddParameter(cmd, pDateTime(TechCommissionBase.Property_CreatedDate, techCommissionObject.CreatedDate));
			AddParameter(cmd, pNVarChar(TechCommissionBase.Property_Batch, 50, techCommissionObject.Batch));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_Adjustment, techCommissionObject.Adjustment));
			AddParameter(cmd, pNVarChar(TechCommissionBase.Property_BaseRMRCommissionCalculation, techCommissionObject.BaseRMRCommissionCalculation));
			AddParameter(cmd, pNVarChar(TechCommissionBase.Property_AddedRMRCommissionCalculation, techCommissionObject.AddedRMRCommissionCalculation));
			AddParameter(cmd, pDateTime(TechCommissionBase.Property_PaidDate, techCommissionObject.PaidDate));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_OriginalPoint, techCommissionObject.OriginalPoint));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_AdjustablePoint, techCommissionObject.AdjustablePoint));
			AddParameter(cmd, pDouble(TechCommissionBase.Property_TotalPoint, techCommissionObject.TotalPoint));
			AddParameter(cmd, pBool(TechCommissionBase.Property_IsSealed, techCommissionObject.IsSealed));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TechCommission
        /// </summary>
        /// <param name="techCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TechCommissionBase techCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTECHCOMMISSION);
	
				AddParameter(cmd, pInt32Out(TechCommissionBase.Property_Id));
				AddCommonParams(cmd, techCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					techCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					techCommissionObject.Id = (Int32)GetOutParameter(cmd, TechCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(techCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TechCommission
        /// </summary>
        /// <param name="techCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TechCommissionBase techCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETECHCOMMISSION);
				
				AddParameter(cmd, pInt32(TechCommissionBase.Property_Id, techCommissionObject.Id));
				AddCommonParams(cmd, techCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					techCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(techCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TechCommission
        /// </summary>
        /// <param name="Id">Id of the TechCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETECHCOMMISSION);	
				
				AddParameter(cmd, pInt32(TechCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TechCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TechCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TechCommission object to retrieve</param>
        /// <returns>TechCommission object, null if not found</returns>
		public TechCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTECHCOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(TechCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TechCommission objects 
        /// </summary>
        /// <returns>A list of TechCommission objects</returns>
		public TechCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTECHCOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TechCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of TechCommission objects</returns>
		public TechCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTECHCOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TechCommissionList _TechCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TechCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all TechCommission objects by query String
        /// </summary>
        /// <returns>A list of TechCommission objects</returns>
		public TechCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTECHCOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TechCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TechCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTECHCOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TechCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TechCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TechCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTECHCOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_TechCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TechCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TechCommission object
        /// </summary>
        /// <param name="techCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TechCommissionBase techCommissionObject, SqlDataReader reader, int start)
		{
			
				techCommissionObject.Id = reader.GetInt32( start + 0 );			
				techCommissionObject.TechCommissionId = reader.GetGuid( start + 1 );			
				techCommissionObject.TicketId = reader.GetGuid( start + 2 );			
				techCommissionObject.CustomerId = reader.GetGuid( start + 3 );			
				techCommissionObject.UserId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) techCommissionObject.CompletionDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) techCommissionObject.BaseRMR = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) techCommissionObject.BaseRMRCommission = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) techCommissionObject.AddedRMR = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) techCommissionObject.AddedRMRCommission = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) techCommissionObject.TotalCommission = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) techCommissionObject.IsPaid = reader.GetBoolean( start + 11 );			
				techCommissionObject.CreatedBy = reader.GetGuid( start + 12 );			
				techCommissionObject.CreatedDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) techCommissionObject.Batch = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) techCommissionObject.Adjustment = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) techCommissionObject.BaseRMRCommissionCalculation = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) techCommissionObject.AddedRMRCommissionCalculation = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) techCommissionObject.PaidDate = reader.GetDateTime( start + 18 );			
				if(!reader.IsDBNull(19)) techCommissionObject.OriginalPoint = reader.GetDouble( start + 19 );			
				if(!reader.IsDBNull(20)) techCommissionObject.AdjustablePoint = reader.GetDouble( start + 20 );			
				if(!reader.IsDBNull(21)) techCommissionObject.TotalPoint = reader.GetDouble( start + 21 );			
				if(!reader.IsDBNull(22)) techCommissionObject.IsSealed = reader.GetBoolean( start + 22 );			
			FillBaseObject(techCommissionObject, reader, (start + 23));

			
			techCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TechCommission object
        /// </summary>
        /// <param name="techCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TechCommissionBase techCommissionObject, SqlDataReader reader)
		{
			FillObject(techCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TechCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TechCommission object</returns>
		private TechCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TechCommission techCommissionObject= new TechCommission();
					FillObject(techCommissionObject, reader);
					return techCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TechCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TechCommission objects</returns>
		private TechCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TechCommission list
			TechCommissionList list = new TechCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TechCommission techCommissionObject = new TechCommission();
					FillObject(techCommissionObject, reader);

					list.Add(techCommissionObject);
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
