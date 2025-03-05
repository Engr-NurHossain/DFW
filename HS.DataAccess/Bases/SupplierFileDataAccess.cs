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
	public partial class SupplierFileDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSUPPLIERFILE = "InsertSupplierFile";
		private const string UPDATESUPPLIERFILE = "UpdateSupplierFile";
		private const string DELETESUPPLIERFILE = "DeleteSupplierFile";
		private const string GETSUPPLIERFILEBYID = "GetSupplierFileById";
		private const string GETALLSUPPLIERFILE = "GetAllSupplierFile";
		private const string GETPAGEDSUPPLIERFILE = "GetPagedSupplierFile";
		private const string GETSUPPLIERFILEMAXIMUMID = "GetSupplierFileMaximumId";
		private const string GETSUPPLIERFILEROWCOUNT = "GetSupplierFileRowCount";	
		private const string GETSUPPLIERFILEBYQUERY = "GetSupplierFileByQuery";
		#endregion
		
		#region Constructors
		public SupplierFileDataAccess(ClientContext context) : base(context) { }
		public SupplierFileDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="supplierFileObject"></param>
		private void AddCommonParams(SqlCommand cmd, SupplierFileBase supplierFileObject)
		{	
			AddParameter(cmd, pNVarChar(SupplierFileBase.Property_FileDescription, supplierFileObject.FileDescription));
			AddParameter(cmd, pNVarChar(SupplierFileBase.Property_Filename, 500, supplierFileObject.Filename));
			AddParameter(cmd, pNVarChar(SupplierFileBase.Property_FileFullName, 500, supplierFileObject.FileFullName));
			AddParameter(cmd, pDateTime(SupplierFileBase.Property_Uploadeddate, supplierFileObject.Uploadeddate));
			AddParameter(cmd, pGuid(SupplierFileBase.Property_SupplierId, supplierFileObject.SupplierId));
			AddParameter(cmd, pGuid(SupplierFileBase.Property_CompanyId, supplierFileObject.CompanyId));
			AddParameter(cmd, pBool(SupplierFileBase.Property_IsActive, supplierFileObject.IsActive));
			AddParameter(cmd, pDouble(SupplierFileBase.Property_FileSize, supplierFileObject.FileSize));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SupplierFile
        /// </summary>
        /// <param name="supplierFileObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SupplierFileBase supplierFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSUPPLIERFILE);
	
				AddParameter(cmd, pInt32Out(SupplierFileBase.Property_Id));
				AddCommonParams(cmd, supplierFileObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					supplierFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					supplierFileObject.Id = (Int32)GetOutParameter(cmd, SupplierFileBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(supplierFileObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SupplierFile
        /// </summary>
        /// <param name="supplierFileObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SupplierFileBase supplierFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESUPPLIERFILE);
				
				AddParameter(cmd, pInt32(SupplierFileBase.Property_Id, supplierFileObject.Id));
				AddCommonParams(cmd, supplierFileObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					supplierFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(supplierFileObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SupplierFile
        /// </summary>
        /// <param name="Id">Id of the SupplierFile object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESUPPLIERFILE);	
				
				AddParameter(cmd, pInt32(SupplierFileBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SupplierFile), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SupplierFile object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SupplierFile object to retrieve</param>
        /// <returns>SupplierFile object, null if not found</returns>
		public SupplierFile Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERFILEBYID))
			{
				AddParameter( cmd, pInt32(SupplierFileBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SupplierFile objects 
        /// </summary>
        /// <returns>A list of SupplierFile objects</returns>
		public SupplierFileList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSUPPLIERFILE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SupplierFile objects by PageRequest
        /// </summary>
        /// <returns>A list of SupplierFile objects</returns>
		public SupplierFileList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSUPPLIERFILE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SupplierFileList _SupplierFileList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SupplierFileList;
			}
		}
		
		/// <summary>
        /// Retrieves all SupplierFile objects by query String
        /// </summary>
        /// <returns>A list of SupplierFile objects</returns>
		public SupplierFileList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERFILEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SupplierFile Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SupplierFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERFILEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SupplierFile Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SupplierFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SupplierFileRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERFILEROWCOUNT))
			{
				SqlDataReader reader;
				_SupplierFileRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SupplierFileRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SupplierFile object
        /// </summary>
        /// <param name="supplierFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SupplierFileBase supplierFileObject, SqlDataReader reader, int start)
		{
			
				supplierFileObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) supplierFileObject.FileDescription = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) supplierFileObject.Filename = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) supplierFileObject.FileFullName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) supplierFileObject.Uploadeddate = reader.GetDateTime( start + 4 );			
				supplierFileObject.SupplierId = reader.GetGuid( start + 5 );			
				supplierFileObject.CompanyId = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) supplierFileObject.IsActive = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) supplierFileObject.FileSize = reader.GetDouble( start + 8 );			
			FillBaseObject(supplierFileObject, reader, (start + 9));

			
			supplierFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SupplierFile object
        /// </summary>
        /// <param name="supplierFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SupplierFileBase supplierFileObject, SqlDataReader reader)
		{
			FillObject(supplierFileObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SupplierFile object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SupplierFile object</returns>
		private SupplierFile GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SupplierFile supplierFileObject= new SupplierFile();
					FillObject(supplierFileObject, reader);
					return supplierFileObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SupplierFile objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SupplierFile objects</returns>
		private SupplierFileList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SupplierFile list
			SupplierFileList list = new SupplierFileList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SupplierFile supplierFileObject = new SupplierFile();
					FillObject(supplierFileObject, reader);

					list.Add(supplierFileObject);
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
