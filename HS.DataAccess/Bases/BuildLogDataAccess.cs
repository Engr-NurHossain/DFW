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
	public partial class BuildLogDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBUILDLOG = "InsertBuildLog";
		private const string UPDATEBUILDLOG = "UpdateBuildLog";
		private const string DELETEBUILDLOG = "DeleteBuildLog";
		private const string GETBUILDLOGBYID = "GetBuildLogById";
		private const string GETALLBUILDLOG = "GetAllBuildLog";
		private const string GETPAGEDBUILDLOG = "GetPagedBuildLog";
		private const string GETBUILDLOGMAXIMUMID = "GetBuildLogMaximumId";
		private const string GETBUILDLOGROWCOUNT = "GetBuildLogRowCount";	
		private const string GETBUILDLOGBYQUERY = "GetBuildLogByQuery";
		#endregion
		
		#region Constructors
		public BuildLogDataAccess(ClientContext context) : base(context) { }
		public BuildLogDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="buildLogObject"></param>
		private void AddCommonParams(SqlCommand cmd, BuildLogBase buildLogObject)
		{	
			AddParameter(cmd, pNVarChar(BuildLogBase.Property_Version, 50, buildLogObject.Version));
			AddParameter(cmd, pDateTime(BuildLogBase.Property_BuildDate, buildLogObject.BuildDate));
			AddParameter(cmd, pDateTime(BuildLogBase.Property_CreatedDate, buildLogObject.CreatedDate));
			AddParameter(cmd, pGuid(BuildLogBase.Property_CreatedBy, buildLogObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BuildLog
        /// </summary>
        /// <param name="buildLogObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BuildLogBase buildLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBUILDLOG);
	
				AddParameter(cmd, pInt32Out(BuildLogBase.Property_Id));
				AddCommonParams(cmd, buildLogObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					buildLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					buildLogObject.Id = (Int32)GetOutParameter(cmd, BuildLogBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(buildLogObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BuildLog
        /// </summary>
        /// <param name="buildLogObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BuildLogBase buildLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBUILDLOG);
				
				AddParameter(cmd, pInt32(BuildLogBase.Property_Id, buildLogObject.Id));
				AddCommonParams(cmd, buildLogObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					buildLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(buildLogObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BuildLog
        /// </summary>
        /// <param name="Id">Id of the BuildLog object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBUILDLOG);	
				
				AddParameter(cmd, pInt32(BuildLogBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BuildLog), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BuildLog object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BuildLog object to retrieve</param>
        /// <returns>BuildLog object, null if not found</returns>
		public BuildLog Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBUILDLOGBYID))
			{
				AddParameter( cmd, pInt32(BuildLogBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BuildLog objects 
        /// </summary>
        /// <returns>A list of BuildLog objects</returns>
		public BuildLogList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBUILDLOG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BuildLog objects by PageRequest
        /// </summary>
        /// <returns>A list of BuildLog objects</returns>
		public BuildLogList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBUILDLOG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BuildLogList _BuildLogList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BuildLogList;
			}
		}
		
		/// <summary>
        /// Retrieves all BuildLog objects by query String
        /// </summary>
        /// <returns>A list of BuildLog objects</returns>
		public BuildLogList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBUILDLOGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BuildLog Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BuildLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBUILDLOGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BuildLog Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BuildLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BuildLogRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBUILDLOGROWCOUNT))
			{
				SqlDataReader reader;
				_BuildLogRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BuildLogRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BuildLog object
        /// </summary>
        /// <param name="buildLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BuildLogBase buildLogObject, SqlDataReader reader, int start)
		{
			
				buildLogObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) buildLogObject.Version = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) buildLogObject.BuildDate = reader.GetDateTime( start + 2 );			
				if(!reader.IsDBNull(3)) buildLogObject.CreatedDate = reader.GetDateTime( start + 3 );			
				buildLogObject.CreatedBy = reader.GetGuid( start + 4 );			
			FillBaseObject(buildLogObject, reader, (start + 5));

			
			buildLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BuildLog object
        /// </summary>
        /// <param name="buildLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BuildLogBase buildLogObject, SqlDataReader reader)
		{
			FillObject(buildLogObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BuildLog object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BuildLog object</returns>
		private BuildLog GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BuildLog buildLogObject= new BuildLog();
					FillObject(buildLogObject, reader);
					return buildLogObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BuildLog objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BuildLog objects</returns>
		private BuildLogList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BuildLog list
			BuildLogList list = new BuildLogList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BuildLog buildLogObject = new BuildLog();
					FillObject(buildLogObject, reader);

					list.Add(buildLogObject);
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