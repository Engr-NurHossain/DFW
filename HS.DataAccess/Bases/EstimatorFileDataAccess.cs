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
	public partial class EstimatorFileDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTESTIMATORFILE = "InsertEstimatorFile";
		private const string UPDATEESTIMATORFILE = "UpdateEstimatorFile";
		private const string DELETEESTIMATORFILE = "DeleteEstimatorFile";
		private const string GETESTIMATORFILEBYID = "GetEstimatorFileById";
		private const string GETALLESTIMATORFILE = "GetAllEstimatorFile";
		private const string GETPAGEDESTIMATORFILE = "GetPagedEstimatorFile";
		private const string GETESTIMATORFILEMAXIMUMID = "GetEstimatorFileMaximumId";
		private const string GETESTIMATORFILEROWCOUNT = "GetEstimatorFileRowCount";	
		private const string GETESTIMATORFILEBYQUERY = "GetEstimatorFileByQuery";
		#endregion
		
		#region Constructors
		public EstimatorFileDataAccess(ClientContext context) : base(context) { }
		public EstimatorFileDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="estimatorFileObject"></param>
		private void AddCommonParams(SqlCommand cmd, EstimatorFileBase estimatorFileObject)
		{	
			AddParameter(cmd, pNVarChar(EstimatorFileBase.Property_EstimatorId, 50, estimatorFileObject.EstimatorId));
			AddParameter(cmd, pNVarChar(EstimatorFileBase.Property_FileDescription, estimatorFileObject.FileDescription));
			AddParameter(cmd, pNVarChar(EstimatorFileBase.Property_Filename, 500, estimatorFileObject.Filename));
			AddParameter(cmd, pNVarChar(EstimatorFileBase.Property_FileFullName, 500, estimatorFileObject.FileFullName));
			AddParameter(cmd, pDouble(EstimatorFileBase.Property_FileSize, estimatorFileObject.FileSize));
			AddParameter(cmd, pBool(EstimatorFileBase.Property_IsActive, estimatorFileObject.IsActive));
			AddParameter(cmd, pGuid(EstimatorFileBase.Property_CreatedBy, estimatorFileObject.CreatedBy));
			AddParameter(cmd, pDateTime(EstimatorFileBase.Property_CreatedDate, estimatorFileObject.CreatedDate));
			AddParameter(cmd, pGuid(EstimatorFileBase.Property_UpdatedBy, estimatorFileObject.UpdatedBy));
			AddParameter(cmd, pDateTime(EstimatorFileBase.Property_UpdatedDate, estimatorFileObject.UpdatedDate));
			AddParameter(cmd, pNVarChar(EstimatorFileBase.Property_EstimatorType, 250, estimatorFileObject.EstimatorType));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EstimatorFile
        /// </summary>
        /// <param name="estimatorFileObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EstimatorFileBase estimatorFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTESTIMATORFILE);
	
				AddParameter(cmd, pInt32Out(EstimatorFileBase.Property_Id));
				AddCommonParams(cmd, estimatorFileObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					estimatorFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					estimatorFileObject.Id = (Int32)GetOutParameter(cmd, EstimatorFileBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(estimatorFileObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EstimatorFile
        /// </summary>
        /// <param name="estimatorFileObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EstimatorFileBase estimatorFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEESTIMATORFILE);
				
				AddParameter(cmd, pInt32(EstimatorFileBase.Property_Id, estimatorFileObject.Id));
				AddCommonParams(cmd, estimatorFileObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					estimatorFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(estimatorFileObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EstimatorFile
        /// </summary>
        /// <param name="Id">Id of the EstimatorFile object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEESTIMATORFILE);	
				
				AddParameter(cmd, pInt32(EstimatorFileBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EstimatorFile), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EstimatorFile object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EstimatorFile object to retrieve</param>
        /// <returns>EstimatorFile object, null if not found</returns>
		public EstimatorFile Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORFILEBYID))
			{
				AddParameter( cmd, pInt32(EstimatorFileBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EstimatorFile objects 
        /// </summary>
        /// <returns>A list of EstimatorFile objects</returns>
		public EstimatorFileList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLESTIMATORFILE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EstimatorFile objects by PageRequest
        /// </summary>
        /// <returns>A list of EstimatorFile objects</returns>
		public EstimatorFileList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDESTIMATORFILE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EstimatorFileList _EstimatorFileList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EstimatorFileList;
			}
		}
		
		/// <summary>
        /// Retrieves all EstimatorFile objects by query String
        /// </summary>
        /// <returns>A list of EstimatorFile objects</returns>
		public EstimatorFileList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORFILEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EstimatorFile Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EstimatorFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORFILEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EstimatorFile Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EstimatorFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EstimatorFileRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORFILEROWCOUNT))
			{
				SqlDataReader reader;
				_EstimatorFileRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EstimatorFileRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EstimatorFile object
        /// </summary>
        /// <param name="estimatorFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EstimatorFileBase estimatorFileObject, SqlDataReader reader, int start)
		{
			
				estimatorFileObject.Id = reader.GetInt32( start + 0 );			
				estimatorFileObject.EstimatorId = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) estimatorFileObject.FileDescription = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) estimatorFileObject.Filename = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) estimatorFileObject.FileFullName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) estimatorFileObject.FileSize = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) estimatorFileObject.IsActive = reader.GetBoolean( start + 6 );			
				estimatorFileObject.CreatedBy = reader.GetGuid( start + 7 );			
				estimatorFileObject.CreatedDate = reader.GetDateTime( start + 8 );			
				estimatorFileObject.UpdatedBy = reader.GetGuid( start + 9 );			
				estimatorFileObject.UpdatedDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) estimatorFileObject.EstimatorType = reader.GetString( start + 11 );			
			FillBaseObject(estimatorFileObject, reader, (start + 12));

			
			estimatorFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EstimatorFile object
        /// </summary>
        /// <param name="estimatorFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EstimatorFileBase estimatorFileObject, SqlDataReader reader)
		{
			FillObject(estimatorFileObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EstimatorFile object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EstimatorFile object</returns>
		private EstimatorFile GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EstimatorFile estimatorFileObject= new EstimatorFile();
					FillObject(estimatorFileObject, reader);
					return estimatorFileObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EstimatorFile objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EstimatorFile objects</returns>
		private EstimatorFileList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EstimatorFile list
			EstimatorFileList list = new EstimatorFileList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EstimatorFile estimatorFileObject = new EstimatorFile();
					FillObject(estimatorFileObject, reader);

					list.Add(estimatorFileObject);
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