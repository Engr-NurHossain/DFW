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
	public partial class OverrideRangeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTOVERRIDERANGE = "InsertOverrideRange";
		private const string UPDATEOVERRIDERANGE = "UpdateOverrideRange";
		private const string DELETEOVERRIDERANGE = "DeleteOverrideRange";
		private const string GETOVERRIDERANGEBYID = "GetOverrideRangeById";
		private const string GETALLOVERRIDERANGE = "GetAllOverrideRange";
		private const string GETPAGEDOVERRIDERANGE = "GetPagedOverrideRange";
		private const string GETOVERRIDERANGEMAXIMUMID = "GetOverrideRangeMaximumId";
		private const string GETOVERRIDERANGEROWCOUNT = "GetOverrideRangeRowCount";	
		private const string GETOVERRIDERANGEBYQUERY = "GetOverrideRangeByQuery";
		#endregion
		
		#region Constructors
		public OverrideRangeDataAccess(ClientContext context) : base(context) { }
		public OverrideRangeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="overrideRangeObject"></param>
		private void AddCommonParams(SqlCommand cmd, OverrideRangeBase overrideRangeObject)
		{	
			AddParameter(cmd, pInt32(OverrideRangeBase.Property_OverrideId, overrideRangeObject.OverrideId));
			AddParameter(cmd, pInt32(OverrideRangeBase.Property_RangeStart, overrideRangeObject.RangeStart));
			AddParameter(cmd, pInt32(OverrideRangeBase.Property_RangeEnd, overrideRangeObject.RangeEnd));
			AddParameter(cmd, pDouble(OverrideRangeBase.Property_Amount, overrideRangeObject.Amount));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts OverrideRange
        /// </summary>
        /// <param name="overrideRangeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(OverrideRangeBase overrideRangeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTOVERRIDERANGE);
	
				AddParameter(cmd, pInt32Out(OverrideRangeBase.Property_Id));
				AddCommonParams(cmd, overrideRangeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					overrideRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					overrideRangeObject.Id = (Int32)GetOutParameter(cmd, OverrideRangeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(overrideRangeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates OverrideRange
        /// </summary>
        /// <param name="overrideRangeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(OverrideRangeBase overrideRangeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEOVERRIDERANGE);
				
				AddParameter(cmd, pInt32(OverrideRangeBase.Property_Id, overrideRangeObject.Id));
				AddCommonParams(cmd, overrideRangeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					overrideRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(overrideRangeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes OverrideRange
        /// </summary>
        /// <param name="Id">Id of the OverrideRange object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEOVERRIDERANGE);	
				
				AddParameter(cmd, pInt32(OverrideRangeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(OverrideRange), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves OverrideRange object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the OverrideRange object to retrieve</param>
        /// <returns>OverrideRange object, null if not found</returns>
		public OverrideRange Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETOVERRIDERANGEBYID))
			{
				AddParameter( cmd, pInt32(OverrideRangeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all OverrideRange objects 
        /// </summary>
        /// <returns>A list of OverrideRange objects</returns>
		public OverrideRangeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLOVERRIDERANGE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all OverrideRange objects by PageRequest
        /// </summary>
        /// <returns>A list of OverrideRange objects</returns>
		public OverrideRangeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDOVERRIDERANGE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				OverrideRangeList _OverrideRangeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _OverrideRangeList;
			}
		}
		
		/// <summary>
        /// Retrieves all OverrideRange objects by query String
        /// </summary>
        /// <returns>A list of OverrideRange objects</returns>
		public OverrideRangeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETOVERRIDERANGEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get OverrideRange Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of OverrideRange
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETOVERRIDERANGEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get OverrideRange Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of OverrideRange
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _OverrideRangeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETOVERRIDERANGEROWCOUNT))
			{
				SqlDataReader reader;
				_OverrideRangeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _OverrideRangeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills OverrideRange object
        /// </summary>
        /// <param name="overrideRangeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(OverrideRangeBase overrideRangeObject, SqlDataReader reader, int start)
		{
			
				overrideRangeObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) overrideRangeObject.OverrideId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) overrideRangeObject.RangeStart = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) overrideRangeObject.RangeEnd = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) overrideRangeObject.Amount = reader.GetDouble( start + 4 );			
			FillBaseObject(overrideRangeObject, reader, (start + 5));

			
			overrideRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills OverrideRange object
        /// </summary>
        /// <param name="overrideRangeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(OverrideRangeBase overrideRangeObject, SqlDataReader reader)
		{
			FillObject(overrideRangeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves OverrideRange object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>OverrideRange object</returns>
		private OverrideRange GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					OverrideRange overrideRangeObject= new OverrideRange();
					FillObject(overrideRangeObject, reader);
					return overrideRangeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of OverrideRange objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of OverrideRange objects</returns>
		private OverrideRangeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//OverrideRange list
			OverrideRangeList list = new OverrideRangeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					OverrideRange overrideRangeObject = new OverrideRange();
					FillObject(overrideRangeObject, reader);

					list.Add(overrideRangeObject);
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
