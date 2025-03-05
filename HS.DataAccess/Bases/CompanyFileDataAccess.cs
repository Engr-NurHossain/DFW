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
	public partial class CompanyFileDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCOMPANYFILE = "InsertCompanyFile";
		private const string UPDATECOMPANYFILE = "UpdateCompanyFile";
		private const string DELETECOMPANYFILE = "DeleteCompanyFile";
		private const string GETCOMPANYFILEBYID = "GetCompanyFileById";
		private const string GETALLCOMPANYFILE = "GetAllCompanyFile";
		private const string GETPAGEDCOMPANYFILE = "GetPagedCompanyFile";
		private const string GETCOMPANYFILEMAXIMUMID = "GetCompanyFileMaximumId";
		private const string GETCOMPANYFILEROWCOUNT = "GetCompanyFileRowCount";	
		private const string GETCOMPANYFILEBYQUERY = "GetCompanyFileByQuery";
		#endregion
		
		#region Constructors
		public CompanyFileDataAccess(ClientContext context) : base(context) { }
		public CompanyFileDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="companyFileObject"></param>
		private void AddCommonParams(SqlCommand cmd, CompanyFileBase companyFileObject)
		{	
			AddParameter(cmd, pNVarChar(CompanyFileBase.Property_FileDescription, companyFileObject.FileDescription));
			AddParameter(cmd, pNVarChar(CompanyFileBase.Property_Filename, 500, companyFileObject.Filename));
			AddParameter(cmd, pNVarChar(CompanyFileBase.Property_FileFullName, 500, companyFileObject.FileFullName));
			AddParameter(cmd, pDateTime(CompanyFileBase.Property_Uploadeddate, companyFileObject.Uploadeddate));
			AddParameter(cmd, pGuid(CompanyFileBase.Property_CompanyId, companyFileObject.CompanyId));
			AddParameter(cmd, pBool(CompanyFileBase.Property_IsActive, companyFileObject.IsActive));
			AddParameter(cmd, pDouble(CompanyFileBase.Property_FileSize, companyFileObject.FileSize));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CompanyFile
        /// </summary>
        /// <param name="companyFileObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CompanyFileBase companyFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCOMPANYFILE);
	
				AddParameter(cmd, pInt32Out(CompanyFileBase.Property_Id));
				AddCommonParams(cmd, companyFileObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					companyFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					companyFileObject.Id = (Int32)GetOutParameter(cmd, CompanyFileBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(companyFileObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CompanyFile
        /// </summary>
        /// <param name="companyFileObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CompanyFileBase companyFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECOMPANYFILE);
				
				AddParameter(cmd, pInt32(CompanyFileBase.Property_Id, companyFileObject.Id));
				AddCommonParams(cmd, companyFileObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					companyFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(companyFileObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CompanyFile
        /// </summary>
        /// <param name="Id">Id of the CompanyFile object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECOMPANYFILE);	
				
				AddParameter(cmd, pInt32(CompanyFileBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CompanyFile), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CompanyFile object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CompanyFile object to retrieve</param>
        /// <returns>CompanyFile object, null if not found</returns>
		public CompanyFile Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYFILEBYID))
			{
				AddParameter( cmd, pInt32(CompanyFileBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CompanyFile objects 
        /// </summary>
        /// <returns>A list of CompanyFile objects</returns>
		public CompanyFileList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCOMPANYFILE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CompanyFile objects by PageRequest
        /// </summary>
        /// <returns>A list of CompanyFile objects</returns>
		public CompanyFileList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCOMPANYFILE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CompanyFileList _CompanyFileList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CompanyFileList;
			}
		}
		
		/// <summary>
        /// Retrieves all CompanyFile objects by query String
        /// </summary>
        /// <returns>A list of CompanyFile objects</returns>
		public CompanyFileList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYFILEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CompanyFile Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CompanyFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYFILEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CompanyFile Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CompanyFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CompanyFileRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYFILEROWCOUNT))
			{
				SqlDataReader reader;
				_CompanyFileRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CompanyFileRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CompanyFile object
        /// </summary>
        /// <param name="companyFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CompanyFileBase companyFileObject, SqlDataReader reader, int start)
		{
			
				companyFileObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) companyFileObject.FileDescription = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) companyFileObject.Filename = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) companyFileObject.FileFullName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) companyFileObject.Uploadeddate = reader.GetDateTime( start + 4 );			
				companyFileObject.CompanyId = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) companyFileObject.IsActive = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) companyFileObject.FileSize = reader.GetDouble( start + 7 );			
			FillBaseObject(companyFileObject, reader, (start + 8));

			
			companyFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CompanyFile object
        /// </summary>
        /// <param name="companyFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CompanyFileBase companyFileObject, SqlDataReader reader)
		{
			FillObject(companyFileObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CompanyFile object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CompanyFile object</returns>
		private CompanyFile GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CompanyFile companyFileObject= new CompanyFile();
					FillObject(companyFileObject, reader);
					return companyFileObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CompanyFile objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CompanyFile objects</returns>
		private CompanyFileList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CompanyFile list
			CompanyFileList list = new CompanyFileList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CompanyFile companyFileObject = new CompanyFile();
					FillObject(companyFileObject, reader);

					list.Add(companyFileObject);
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
