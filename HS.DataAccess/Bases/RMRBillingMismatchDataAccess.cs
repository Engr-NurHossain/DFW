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
	public partial class RMRBillingMismatchDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRMRBILLINGMISMATCH = "InsertRMRBillingMismatch";
		private const string UPDATERMRBILLINGMISMATCH = "UpdateRMRBillingMismatch";
		private const string DELETERMRBILLINGMISMATCH = "DeleteRMRBillingMismatch";
		private const string GETRMRBILLINGMISMATCHBYID = "GetRMRBillingMismatchById";
		private const string GETALLRMRBILLINGMISMATCH = "GetAllRMRBillingMismatch";
		private const string GETPAGEDRMRBILLINGMISMATCH = "GetPagedRMRBillingMismatch";
		private const string GETRMRBILLINGMISMATCHMAXIMUMID = "GetRMRBillingMismatchMaximumId";
		private const string GETRMRBILLINGMISMATCHROWCOUNT = "GetRMRBillingMismatchRowCount";	
		private const string GETRMRBILLINGMISMATCHBYQUERY = "GetRMRBillingMismatchByQuery";
		#endregion
		
		#region Constructors
		public RMRBillingMismatchDataAccess(ClientContext context) : base(context) { }
		public RMRBillingMismatchDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="rMRBillingMismatchObject"></param>
		private void AddCommonParams(SqlCommand cmd, RMRBillingMismatchBase rMRBillingMismatchObject)
		{	
			AddParameter(cmd, pGuid(RMRBillingMismatchBase.Property_CustomerId, rMRBillingMismatchObject.CustomerId));
			AddParameter(cmd, pNVarChar(RMRBillingMismatchBase.Property_TransactionId, 150, rMRBillingMismatchObject.TransactionId));
			AddParameter(cmd, pNVarChar(RMRBillingMismatchBase.Property_InvoiceId, 50, rMRBillingMismatchObject.InvoiceId));
			AddParameter(cmd, pDouble(RMRBillingMismatchBase.Property_BillingAmount, rMRBillingMismatchObject.BillingAmount));
			AddParameter(cmd, pDouble(RMRBillingMismatchBase.Property_ChargedAmountByGateway, rMRBillingMismatchObject.ChargedAmountByGateway));
			AddParameter(cmd, pBool(RMRBillingMismatchBase.Property_IsResolved, rMRBillingMismatchObject.IsResolved));
			AddParameter(cmd, pGuid(RMRBillingMismatchBase.Property_ResolvedBy, rMRBillingMismatchObject.ResolvedBy));
			AddParameter(cmd, pDateTime(RMRBillingMismatchBase.Property_ResolvedDate, rMRBillingMismatchObject.ResolvedDate));
			AddParameter(cmd, pDateTime(RMRBillingMismatchBase.Property_CreatedDate, rMRBillingMismatchObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RMRBillingMismatch
        /// </summary>
        /// <param name="rMRBillingMismatchObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RMRBillingMismatchBase rMRBillingMismatchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRMRBILLINGMISMATCH);
	
				AddParameter(cmd, pInt32Out(RMRBillingMismatchBase.Property_Id));
				AddCommonParams(cmd, rMRBillingMismatchObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					rMRBillingMismatchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					rMRBillingMismatchObject.Id = (Int32)GetOutParameter(cmd, RMRBillingMismatchBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(rMRBillingMismatchObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RMRBillingMismatch
        /// </summary>
        /// <param name="rMRBillingMismatchObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RMRBillingMismatchBase rMRBillingMismatchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERMRBILLINGMISMATCH);
				
				AddParameter(cmd, pInt32(RMRBillingMismatchBase.Property_Id, rMRBillingMismatchObject.Id));
				AddCommonParams(cmd, rMRBillingMismatchObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					rMRBillingMismatchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(rMRBillingMismatchObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RMRBillingMismatch
        /// </summary>
        /// <param name="Id">Id of the RMRBillingMismatch object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERMRBILLINGMISMATCH);	
				
				AddParameter(cmd, pInt32(RMRBillingMismatchBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RMRBillingMismatch), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RMRBillingMismatch object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RMRBillingMismatch object to retrieve</param>
        /// <returns>RMRBillingMismatch object, null if not found</returns>
		public RMRBillingMismatch Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRMRBILLINGMISMATCHBYID))
			{
				AddParameter( cmd, pInt32(RMRBillingMismatchBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RMRBillingMismatch objects 
        /// </summary>
        /// <returns>A list of RMRBillingMismatch objects</returns>
		public RMRBillingMismatchList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRMRBILLINGMISMATCH))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RMRBillingMismatch objects by PageRequest
        /// </summary>
        /// <returns>A list of RMRBillingMismatch objects</returns>
		public RMRBillingMismatchList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRMRBILLINGMISMATCH))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RMRBillingMismatchList _RMRBillingMismatchList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RMRBillingMismatchList;
			}
		}
		
		/// <summary>
        /// Retrieves all RMRBillingMismatch objects by query String
        /// </summary>
        /// <returns>A list of RMRBillingMismatch objects</returns>
		public RMRBillingMismatchList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRMRBILLINGMISMATCHBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RMRBillingMismatch Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RMRBillingMismatch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRMRBILLINGMISMATCHMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RMRBillingMismatch Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RMRBillingMismatch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RMRBillingMismatchRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRMRBILLINGMISMATCHROWCOUNT))
			{
				SqlDataReader reader;
				_RMRBillingMismatchRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RMRBillingMismatchRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RMRBillingMismatch object
        /// </summary>
        /// <param name="rMRBillingMismatchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RMRBillingMismatchBase rMRBillingMismatchObject, SqlDataReader reader, int start)
		{
			
				rMRBillingMismatchObject.Id = reader.GetInt32( start + 0 );			
				rMRBillingMismatchObject.CustomerId = reader.GetGuid( start + 1 );			
				rMRBillingMismatchObject.TransactionId = reader.GetString( start + 2 );			
				rMRBillingMismatchObject.InvoiceId = reader.GetString( start + 3 );			
				rMRBillingMismatchObject.BillingAmount = reader.GetDouble( start + 4 );			
				rMRBillingMismatchObject.ChargedAmountByGateway = reader.GetDouble( start + 5 );			
				rMRBillingMismatchObject.IsResolved = reader.GetBoolean( start + 6 );			
				rMRBillingMismatchObject.ResolvedBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) rMRBillingMismatchObject.ResolvedDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) rMRBillingMismatchObject.CreatedDate = reader.GetDateTime( start + 9 );			
			FillBaseObject(rMRBillingMismatchObject, reader, (start + 10));

			
			rMRBillingMismatchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RMRBillingMismatch object
        /// </summary>
        /// <param name="rMRBillingMismatchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RMRBillingMismatchBase rMRBillingMismatchObject, SqlDataReader reader)
		{
			FillObject(rMRBillingMismatchObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RMRBillingMismatch object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RMRBillingMismatch object</returns>
		private RMRBillingMismatch GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RMRBillingMismatch rMRBillingMismatchObject= new RMRBillingMismatch();
					FillObject(rMRBillingMismatchObject, reader);
					return rMRBillingMismatchObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RMRBillingMismatch objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RMRBillingMismatch objects</returns>
		private RMRBillingMismatchList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RMRBillingMismatch list
			RMRBillingMismatchList list = new RMRBillingMismatchList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RMRBillingMismatch rMRBillingMismatchObject = new RMRBillingMismatch();
					FillObject(rMRBillingMismatchObject, reader);

					list.Add(rMRBillingMismatchObject);
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
