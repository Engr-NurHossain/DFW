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
	public partial class FileTemplateDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTFILETEMPLATE = "InsertFileTemplate";
		private const string UPDATEFILETEMPLATE = "UpdateFileTemplate";
		private const string DELETEFILETEMPLATE = "DeleteFileTemplate";
		private const string GETFILETEMPLATEBYID = "GetFileTemplateById";
		private const string GETALLFILETEMPLATE = "GetAllFileTemplate";
		private const string GETPAGEDFILETEMPLATE = "GetPagedFileTemplate";
		private const string GETFILETEMPLATEMAXIMUMID = "GetFileTemplateMaximumId";
		private const string GETFILETEMPLATEROWCOUNT = "GetFileTemplateRowCount";	
		private const string GETFILETEMPLATEBYQUERY = "GetFileTemplateByQuery";
		#endregion
		
		#region Constructors
		public FileTemplateDataAccess(ClientContext context) : base(context) { }
		public FileTemplateDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="fileTemplateObject"></param>
		private void AddCommonParams(SqlCommand cmd, FileTemplateBase fileTemplateObject)
		{	
			AddParameter(cmd, pNVarChar(FileTemplateBase.Property_FileName, 250, fileTemplateObject.FileName));
			AddParameter(cmd, pNVarChar(FileTemplateBase.Property_FileDescription, fileTemplateObject.FileDescription));
			AddParameter(cmd, pNVarChar(FileTemplateBase.Property_FileBody, fileTemplateObject.FileBody));
			AddParameter(cmd, pGuid(FileTemplateBase.Property_CreatedBy, fileTemplateObject.CreatedBy));
			AddParameter(cmd, pDateTime(FileTemplateBase.Property_CreatedDate, fileTemplateObject.CreatedDate));
			AddParameter(cmd, pGuid(FileTemplateBase.Property_LastUpdatedBy, fileTemplateObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(FileTemplateBase.Property_LastUpdatedDate, fileTemplateObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(FileTemplateBase.Property_CompanyId, fileTemplateObject.CompanyId));
			AddParameter(cmd, pBool(FileTemplateBase.Property_IsCustomerSignRequired, fileTemplateObject.IsCustomerSignRequired));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts FileTemplate
        /// </summary>
        /// <param name="fileTemplateObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(FileTemplateBase fileTemplateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTFILETEMPLATE);
	
				AddParameter(cmd, pInt32Out(FileTemplateBase.Property_Id));
				AddCommonParams(cmd, fileTemplateObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					fileTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					fileTemplateObject.Id = (Int32)GetOutParameter(cmd, FileTemplateBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(fileTemplateObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates FileTemplate
        /// </summary>
        /// <param name="fileTemplateObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(FileTemplateBase fileTemplateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEFILETEMPLATE);
				
				AddParameter(cmd, pInt32(FileTemplateBase.Property_Id, fileTemplateObject.Id));
				AddCommonParams(cmd, fileTemplateObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					fileTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(fileTemplateObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes FileTemplate
        /// </summary>
        /// <param name="Id">Id of the FileTemplate object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEFILETEMPLATE);	
				
				AddParameter(cmd, pInt32(FileTemplateBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(FileTemplate), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves FileTemplate object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the FileTemplate object to retrieve</param>
        /// <returns>FileTemplate object, null if not found</returns>
		public FileTemplate Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETFILETEMPLATEBYID))
			{
				AddParameter( cmd, pInt32(FileTemplateBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all FileTemplate objects 
        /// </summary>
        /// <returns>A list of FileTemplate objects</returns>
		public FileTemplateList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLFILETEMPLATE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all FileTemplate objects by PageRequest
        /// </summary>
        /// <returns>A list of FileTemplate objects</returns>
		public FileTemplateList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDFILETEMPLATE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				FileTemplateList _FileTemplateList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _FileTemplateList;
			}
		}
		
		/// <summary>
        /// Retrieves all FileTemplate objects by query String
        /// </summary>
        /// <returns>A list of FileTemplate objects</returns>
		public FileTemplateList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETFILETEMPLATEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get FileTemplate Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of FileTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFILETEMPLATEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get FileTemplate Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of FileTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _FileTemplateRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFILETEMPLATEROWCOUNT))
			{
				SqlDataReader reader;
				_FileTemplateRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _FileTemplateRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills FileTemplate object
        /// </summary>
        /// <param name="fileTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(FileTemplateBase fileTemplateObject, SqlDataReader reader, int start)
		{
			
				fileTemplateObject.Id = reader.GetInt32( start + 0 );			
				fileTemplateObject.FileName = reader.GetString( start + 1 );			
				fileTemplateObject.FileDescription = reader.GetString( start + 2 );			
				fileTemplateObject.FileBody = reader.GetString( start + 3 );			
				fileTemplateObject.CreatedBy = reader.GetGuid( start + 4 );			
				fileTemplateObject.CreatedDate = reader.GetDateTime( start + 5 );			
				fileTemplateObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
				fileTemplateObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
				fileTemplateObject.CompanyId = reader.GetGuid( start + 8 );			
				fileTemplateObject.IsCustomerSignRequired = reader.GetBoolean( start + 9 );			
			FillBaseObject(fileTemplateObject, reader, (start + 10));

			
			fileTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills FileTemplate object
        /// </summary>
        /// <param name="fileTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(FileTemplateBase fileTemplateObject, SqlDataReader reader)
		{
			FillObject(fileTemplateObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves FileTemplate object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>FileTemplate object</returns>
		private FileTemplate GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					FileTemplate fileTemplateObject= new FileTemplate();
					FillObject(fileTemplateObject, reader);
					return fileTemplateObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of FileTemplate objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of FileTemplate objects</returns>
		private FileTemplateList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//FileTemplate list
			FileTemplateList list = new FileTemplateList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					FileTemplate fileTemplateObject = new FileTemplate();
					FillObject(fileTemplateObject, reader);

					list.Add(fileTemplateObject);
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
