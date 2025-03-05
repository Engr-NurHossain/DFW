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
	public partial class CityZipCodeSearchLogDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCITYZIPCODESEARCHLOG = "InsertCityZipCodeSearchLog";
		private const string UPDATECITYZIPCODESEARCHLOG = "UpdateCityZipCodeSearchLog";
		private const string DELETECITYZIPCODESEARCHLOG = "DeleteCityZipCodeSearchLog";
		private const string GETCITYZIPCODESEARCHLOGBYID = "GetCityZipCodeSearchLogById";
		private const string GETALLCITYZIPCODESEARCHLOG = "GetAllCityZipCodeSearchLog";
		private const string GETPAGEDCITYZIPCODESEARCHLOG = "GetPagedCityZipCodeSearchLog";
		private const string GETCITYZIPCODESEARCHLOGMAXIMUMID = "GetCityZipCodeSearchLogMaximumId";
		private const string GETCITYZIPCODESEARCHLOGROWCOUNT = "GetCityZipCodeSearchLogRowCount";	
		private const string GETCITYZIPCODESEARCHLOGBYQUERY = "GetCityZipCodeSearchLogByQuery";
		#endregion
		
		#region Constructors
		public CityZipCodeSearchLogDataAccess(ClientContext context) : base(context) { }
		public CityZipCodeSearchLogDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="cityZipCodeSearchLogObject"></param>
		private void AddCommonParams(SqlCommand cmd, CityZipCodeSearchLogBase cityZipCodeSearchLogObject)
		{	
			AddParameter(cmd, pNVarChar(CityZipCodeSearchLogBase.Property_AppName, 50, cityZipCodeSearchLogObject.AppName));
			AddParameter(cmd, pNVarChar(CityZipCodeSearchLogBase.Property_UserIP, 50, cityZipCodeSearchLogObject.UserIP));
			AddParameter(cmd, pNVarChar(CityZipCodeSearchLogBase.Property_SearchText, 50, cityZipCodeSearchLogObject.SearchText));
			AddParameter(cmd, pDateTime(CityZipCodeSearchLogBase.Property_SearchDate, cityZipCodeSearchLogObject.SearchDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CityZipCodeSearchLog
        /// </summary>
        /// <param name="cityZipCodeSearchLogObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CityZipCodeSearchLogBase cityZipCodeSearchLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCITYZIPCODESEARCHLOG);
	
				AddParameter(cmd, pInt32Out(CityZipCodeSearchLogBase.Property_Id));
				AddCommonParams(cmd, cityZipCodeSearchLogObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					cityZipCodeSearchLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					cityZipCodeSearchLogObject.Id = (Int32)GetOutParameter(cmd, CityZipCodeSearchLogBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(cityZipCodeSearchLogObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CityZipCodeSearchLog
        /// </summary>
        /// <param name="cityZipCodeSearchLogObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CityZipCodeSearchLogBase cityZipCodeSearchLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECITYZIPCODESEARCHLOG);
				
				AddParameter(cmd, pInt32(CityZipCodeSearchLogBase.Property_Id, cityZipCodeSearchLogObject.Id));
				AddCommonParams(cmd, cityZipCodeSearchLogObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					cityZipCodeSearchLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(cityZipCodeSearchLogObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CityZipCodeSearchLog
        /// </summary>
        /// <param name="Id">Id of the CityZipCodeSearchLog object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECITYZIPCODESEARCHLOG);	
				
				AddParameter(cmd, pInt32(CityZipCodeSearchLogBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CityZipCodeSearchLog), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CityZipCodeSearchLog object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CityZipCodeSearchLog object to retrieve</param>
        /// <returns>CityZipCodeSearchLog object, null if not found</returns>
		public CityZipCodeSearchLog Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCITYZIPCODESEARCHLOGBYID))
			{
				AddParameter( cmd, pInt32(CityZipCodeSearchLogBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CityZipCodeSearchLog objects 
        /// </summary>
        /// <returns>A list of CityZipCodeSearchLog objects</returns>
		public CityZipCodeSearchLogList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCITYZIPCODESEARCHLOG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CityZipCodeSearchLog objects by PageRequest
        /// </summary>
        /// <returns>A list of CityZipCodeSearchLog objects</returns>
		public CityZipCodeSearchLogList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCITYZIPCODESEARCHLOG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CityZipCodeSearchLogList _CityZipCodeSearchLogList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CityZipCodeSearchLogList;
			}
		}
		
		/// <summary>
        /// Retrieves all CityZipCodeSearchLog objects by query String
        /// </summary>
        /// <returns>A list of CityZipCodeSearchLog objects</returns>
		public CityZipCodeSearchLogList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCITYZIPCODESEARCHLOGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CityZipCodeSearchLog Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CityZipCodeSearchLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCITYZIPCODESEARCHLOGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CityZipCodeSearchLog Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CityZipCodeSearchLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CityZipCodeSearchLogRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCITYZIPCODESEARCHLOGROWCOUNT))
			{
				SqlDataReader reader;
				_CityZipCodeSearchLogRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CityZipCodeSearchLogRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CityZipCodeSearchLog object
        /// </summary>
        /// <param name="cityZipCodeSearchLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CityZipCodeSearchLogBase cityZipCodeSearchLogObject, SqlDataReader reader, int start)
		{
			
				cityZipCodeSearchLogObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) cityZipCodeSearchLogObject.AppName = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) cityZipCodeSearchLogObject.UserIP = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) cityZipCodeSearchLogObject.SearchText = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) cityZipCodeSearchLogObject.SearchDate = reader.GetDateTime( start + 4 );			
			FillBaseObject(cityZipCodeSearchLogObject, reader, (start + 5));

			
			cityZipCodeSearchLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CityZipCodeSearchLog object
        /// </summary>
        /// <param name="cityZipCodeSearchLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CityZipCodeSearchLogBase cityZipCodeSearchLogObject, SqlDataReader reader)
		{
			FillObject(cityZipCodeSearchLogObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CityZipCodeSearchLog object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CityZipCodeSearchLog object</returns>
		private CityZipCodeSearchLog GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CityZipCodeSearchLog cityZipCodeSearchLogObject= new CityZipCodeSearchLog();
					FillObject(cityZipCodeSearchLogObject, reader);
					return cityZipCodeSearchLogObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CityZipCodeSearchLog objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CityZipCodeSearchLog objects</returns>
		private CityZipCodeSearchLogList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CityZipCodeSearchLog list
			CityZipCodeSearchLogList list = new CityZipCodeSearchLogList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CityZipCodeSearchLog cityZipCodeSearchLogObject = new CityZipCodeSearchLog();
					FillObject(cityZipCodeSearchLogObject, reader);

					list.Add(cityZipCodeSearchLogObject);
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
