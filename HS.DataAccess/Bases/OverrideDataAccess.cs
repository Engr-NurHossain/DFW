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
	public partial class OverrideDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTOVERRIDE = "InsertOverride";
		private const string UPDATEOVERRIDE = "UpdateOverride";
		private const string DELETEOVERRIDE = "DeleteOverride";
		private const string GETOVERRIDEBYID = "GetOverrideById";
		private const string GETALLOVERRIDE = "GetAllOverride";
		private const string GETPAGEDOVERRIDE = "GetPagedOverride";
		private const string GETOVERRIDEMAXIMUMID = "GetOverrideMaximumId";
		private const string GETOVERRIDEROWCOUNT = "GetOverrideRowCount";	
		private const string GETOVERRIDEBYQUERY = "GetOverrideByQuery";
		#endregion
		
		#region Constructors
		public OverrideDataAccess(ClientContext context) : base(context) { }
		public OverrideDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="overrideObject"></param>
		private void AddCommonParams(SqlCommand cmd, OverrideBase overrideObject)
		{	
			AddParameter(cmd, pNVarChar(OverrideBase.Property_Name, 50, overrideObject.Name));
			AddParameter(cmd, pNVarChar(OverrideBase.Property_Timeframe, 50, overrideObject.Timeframe));
			AddParameter(cmd, pNVarChar(OverrideBase.Property_StartDayWk, 50, overrideObject.StartDayWk));
			AddParameter(cmd, pNVarChar(OverrideBase.Property_StartDayMonth, 50, overrideObject.StartDayMonth));
			AddParameter(cmd, pBool(OverrideBase.Property_IsActive, overrideObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Override
        /// </summary>
        /// <param name="overrideObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(OverrideBase overrideObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTOVERRIDE);
	
				AddParameter(cmd, pInt32Out(OverrideBase.Property_Id));
				AddCommonParams(cmd, overrideObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					overrideObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					overrideObject.Id = (Int32)GetOutParameter(cmd, OverrideBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(overrideObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Override
        /// </summary>
        /// <param name="overrideObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(OverrideBase overrideObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEOVERRIDE);
				
				AddParameter(cmd, pInt32(OverrideBase.Property_Id, overrideObject.Id));
				AddCommonParams(cmd, overrideObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					overrideObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(overrideObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Override
        /// </summary>
        /// <param name="Id">Id of the Override object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEOVERRIDE);	
				
				AddParameter(cmd, pInt32(OverrideBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Override), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Override object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Override object to retrieve</param>
        /// <returns>Override object, null if not found</returns>
		public Override Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETOVERRIDEBYID))
			{
				AddParameter( cmd, pInt32(OverrideBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Override objects 
        /// </summary>
        /// <returns>A list of Override objects</returns>
		public OverrideList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLOVERRIDE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Override objects by PageRequest
        /// </summary>
        /// <returns>A list of Override objects</returns>
		public OverrideList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDOVERRIDE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				OverrideList _OverrideList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _OverrideList;
			}
		}
		
		/// <summary>
        /// Retrieves all Override objects by query String
        /// </summary>
        /// <returns>A list of Override objects</returns>
		public OverrideList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETOVERRIDEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Override Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Override
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETOVERRIDEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Override Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Override
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _OverrideRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETOVERRIDEROWCOUNT))
			{
				SqlDataReader reader;
				_OverrideRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _OverrideRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Override object
        /// </summary>
        /// <param name="overrideObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(OverrideBase overrideObject, SqlDataReader reader, int start)
		{
			
				overrideObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) overrideObject.Name = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) overrideObject.Timeframe = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) overrideObject.StartDayWk = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) overrideObject.StartDayMonth = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) overrideObject.IsActive = reader.GetBoolean( start + 5 );			
			FillBaseObject(overrideObject, reader, (start + 6));

			
			overrideObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Override object
        /// </summary>
        /// <param name="overrideObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(OverrideBase overrideObject, SqlDataReader reader)
		{
			FillObject(overrideObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Override object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Override object</returns>
		private Override GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Override overrideObject= new Override();
					FillObject(overrideObject, reader);
					return overrideObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Override objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Override objects</returns>
		private OverrideList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Override list
			OverrideList list = new OverrideList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Override overrideObject = new Override();
					FillObject(overrideObject, reader);

					list.Add(overrideObject);
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
