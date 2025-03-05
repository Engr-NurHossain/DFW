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
	public partial class AdjustmentFundingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTADJUSTMENTFUNDING = "InsertAdjustmentFunding";
		private const string UPDATEADJUSTMENTFUNDING = "UpdateAdjustmentFunding";
		private const string DELETEADJUSTMENTFUNDING = "DeleteAdjustmentFunding";
		private const string GETADJUSTMENTFUNDINGBYID = "GetAdjustmentFundingById";
		private const string GETALLADJUSTMENTFUNDING = "GetAllAdjustmentFunding";
		private const string GETPAGEDADJUSTMENTFUNDING = "GetPagedAdjustmentFunding";
		private const string GETADJUSTMENTFUNDINGMAXIMUMID = "GetAdjustmentFundingMaximumId";
		private const string GETADJUSTMENTFUNDINGROWCOUNT = "GetAdjustmentFundingRowCount";	
		private const string GETADJUSTMENTFUNDINGBYQUERY = "GetAdjustmentFundingByQuery";
		#endregion
		
		#region Constructors
		public AdjustmentFundingDataAccess(ClientContext context) : base(context) { }
		public AdjustmentFundingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="adjustmentFundingObject"></param>
		private void AddCommonParams(SqlCommand cmd, AdjustmentFundingBase adjustmentFundingObject)
		{	
			AddParameter(cmd, pGuid(AdjustmentFundingBase.Property_AdjustmentId, adjustmentFundingObject.AdjustmentId));
			AddParameter(cmd, pGuid(AdjustmentFundingBase.Property_UserId, adjustmentFundingObject.UserId));
			AddParameter(cmd, pNVarChar(AdjustmentFundingBase.Property_Reason, adjustmentFundingObject.Reason));
			AddParameter(cmd, pDouble(AdjustmentFundingBase.Property_Amount, adjustmentFundingObject.Amount));
			AddParameter(cmd, pDateTime(AdjustmentFundingBase.Property_Date, adjustmentFundingObject.Date));
			AddParameter(cmd, pBool(AdjustmentFundingBase.Property_IsPaid, adjustmentFundingObject.IsPaid));
			AddParameter(cmd, pInt32(AdjustmentFundingBase.Property_Batch, adjustmentFundingObject.Batch));
			AddParameter(cmd, pDateTime(AdjustmentFundingBase.Property_PaidDate, adjustmentFundingObject.PaidDate));
			AddParameter(cmd, pDateTime(AdjustmentFundingBase.Property_CreatedDate, adjustmentFundingObject.CreatedDate));
			AddParameter(cmd, pGuid(AdjustmentFundingBase.Property_CreatedBy, adjustmentFundingObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AdjustmentFunding
        /// </summary>
        /// <param name="adjustmentFundingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AdjustmentFundingBase adjustmentFundingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTADJUSTMENTFUNDING);
	
				AddParameter(cmd, pInt32Out(AdjustmentFundingBase.Property_Id));
				AddCommonParams(cmd, adjustmentFundingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					adjustmentFundingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					adjustmentFundingObject.Id = (Int32)GetOutParameter(cmd, AdjustmentFundingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(adjustmentFundingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AdjustmentFunding
        /// </summary>
        /// <param name="adjustmentFundingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AdjustmentFundingBase adjustmentFundingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEADJUSTMENTFUNDING);
				
				AddParameter(cmd, pInt32(AdjustmentFundingBase.Property_Id, adjustmentFundingObject.Id));
				AddCommonParams(cmd, adjustmentFundingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					adjustmentFundingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(adjustmentFundingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AdjustmentFunding
        /// </summary>
        /// <param name="Id">Id of the AdjustmentFunding object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEADJUSTMENTFUNDING);	
				
				AddParameter(cmd, pInt32(AdjustmentFundingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AdjustmentFunding), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AdjustmentFunding object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AdjustmentFunding object to retrieve</param>
        /// <returns>AdjustmentFunding object, null if not found</returns>
		public AdjustmentFunding Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTFUNDINGBYID))
			{
				AddParameter( cmd, pInt32(AdjustmentFundingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AdjustmentFunding objects 
        /// </summary>
        /// <returns>A list of AdjustmentFunding objects</returns>
		public AdjustmentFundingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLADJUSTMENTFUNDING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AdjustmentFunding objects by PageRequest
        /// </summary>
        /// <returns>A list of AdjustmentFunding objects</returns>
		public AdjustmentFundingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDADJUSTMENTFUNDING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AdjustmentFundingList _AdjustmentFundingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AdjustmentFundingList;
			}
		}
		
		/// <summary>
        /// Retrieves all AdjustmentFunding objects by query String
        /// </summary>
        /// <returns>A list of AdjustmentFunding objects</returns>
		public AdjustmentFundingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTFUNDINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AdjustmentFunding Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AdjustmentFunding
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTFUNDINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AdjustmentFunding Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AdjustmentFunding
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AdjustmentFundingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTFUNDINGROWCOUNT))
			{
				SqlDataReader reader;
				_AdjustmentFundingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AdjustmentFundingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AdjustmentFunding object
        /// </summary>
        /// <param name="adjustmentFundingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AdjustmentFundingBase adjustmentFundingObject, SqlDataReader reader, int start)
		{
			
				adjustmentFundingObject.Id = reader.GetInt32( start + 0 );			
				adjustmentFundingObject.AdjustmentId = reader.GetGuid( start + 1 );			
				adjustmentFundingObject.UserId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) adjustmentFundingObject.Reason = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) adjustmentFundingObject.Amount = reader.GetDouble( start + 4 );			
				if(!reader.IsDBNull(5)) adjustmentFundingObject.Date = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) adjustmentFundingObject.IsPaid = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) adjustmentFundingObject.Batch = reader.GetInt32( start + 7 );			
				if(!reader.IsDBNull(8)) adjustmentFundingObject.PaidDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) adjustmentFundingObject.CreatedDate = reader.GetDateTime( start + 9 );			
				adjustmentFundingObject.CreatedBy = reader.GetGuid( start + 10 );			
			FillBaseObject(adjustmentFundingObject, reader, (start + 11));

			
			adjustmentFundingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AdjustmentFunding object
        /// </summary>
        /// <param name="adjustmentFundingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AdjustmentFundingBase adjustmentFundingObject, SqlDataReader reader)
		{
			FillObject(adjustmentFundingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AdjustmentFunding object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AdjustmentFunding object</returns>
		private AdjustmentFunding GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AdjustmentFunding adjustmentFundingObject= new AdjustmentFunding();
					FillObject(adjustmentFundingObject, reader);
					return adjustmentFundingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AdjustmentFunding objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AdjustmentFunding objects</returns>
		private AdjustmentFundingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AdjustmentFunding list
			AdjustmentFundingList list = new AdjustmentFundingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AdjustmentFunding adjustmentFundingObject = new AdjustmentFunding();
					FillObject(adjustmentFundingObject, reader);

					list.Add(adjustmentFundingObject);
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
