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
	public partial class CustomerFileDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERFILE = "InsertCustomerFile_v2";
		private const string UPDATECUSTOMERFILE = "UpdateCustomerFile_v2";
		private const string DELETECUSTOMERFILE = "DeleteCustomerFile";
		private const string GETCUSTOMERFILEBYID = "GetCustomerFileById";
		private const string GETALLCUSTOMERFILE = "GetAllCustomerFile";
		private const string GETPAGEDCUSTOMERFILE = "GetPagedCustomerFile";
		private const string GETCUSTOMERFILEMAXIMUMID = "GetCustomerFileMaximumId";
		private const string GETCUSTOMERFILEROWCOUNT = "GetCustomerFileRowCount";	
		private const string GETCUSTOMERFILEBYQUERY = "GetCustomerFileByQuery";
		#endregion
		
		#region Constructors
		public CustomerFileDataAccess(ClientContext context) : base(context) { }
		public CustomerFileDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerFileObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerFileBase customerFileObject)
		{	
			AddParameter(cmd, pNVarChar(CustomerFileBase.Property_FileDescription, customerFileObject.FileDescription));
			AddParameter(cmd, pNVarChar(CustomerFileBase.Property_Filename, 500, customerFileObject.Filename));
			AddParameter(cmd, pNVarChar(CustomerFileBase.Property_FileFullName, 500, customerFileObject.FileFullName));
			AddParameter(cmd, pDateTime(CustomerFileBase.Property_Uploadeddate, customerFileObject.Uploadeddate));
			AddParameter(cmd, pGuid(CustomerFileBase.Property_CustomerId, customerFileObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerFileBase.Property_CompanyId, customerFileObject.CompanyId));
			AddParameter(cmd, pBool(CustomerFileBase.Property_IsActive, customerFileObject.IsActive));
			AddParameter(cmd, pDouble(CustomerFileBase.Property_FileSize, customerFileObject.FileSize));
			AddParameter(cmd, pNVarChar(CustomerFileBase.Property_Tag, 50, customerFileObject.Tag));
			AddParameter(cmd, pNVarChar(CustomerFileBase.Property_InvoiceId, 50, customerFileObject.InvoiceId));
			AddParameter(cmd, pNVarChar(CustomerFileBase.Property_GeeseFileType, 50, customerFileObject.GeeseFileType));
			AddParameter(cmd, pGuid(CustomerFileBase.Property_CreatedBy, customerFileObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomerFileBase.Property_CreatedDate, customerFileObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerFileBase.Property_UpdatedBy, customerFileObject.UpdatedBy));
			AddParameter(cmd, pDateTime(CustomerFileBase.Property_UpdatedDate, customerFileObject.UpdatedDate));
			AddParameter(cmd, pGuid(CustomerFileBase.Property_FileId, customerFileObject.FileId));
            AddParameter(cmd, pNVarChar(CustomerFileBase.Property_AWSProcessStatus, customerFileObject.AWSProcessStatus));
            AddParameter(cmd, pDateTime(CustomerFileBase.Property_AWSUploadTS, customerFileObject.AWSUploadTS));
            AddParameter(cmd, pNVarChar(CustomerFileBase.Property_WMStatus, customerFileObject.WMStatus));
        }
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerFile
        /// </summary>
        /// <param name="customerFileObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerFileBase customerFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERFILE);
	
				AddParameter(cmd, pInt32Out(CustomerFileBase.Property_Id));
				AddCommonParams(cmd, customerFileObject);

                long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerFileObject.Id = (Int32)GetOutParameter(cmd, CustomerFileBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerFileObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerFile
        /// </summary>
        /// <param name="customerFileObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerFileBase customerFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERFILE);
				
				AddParameter(cmd, pInt32(CustomerFileBase.Property_Id, customerFileObject.Id));
                //cmd.Parameters.AddWithValue("WMStatus", customerFileObject.WMStatus);
                //cmd.Parameters.AddWithValue("AWSProcessStatus", customerFileObject.AWSProcessStatus);
                //cmd.Parameters.AddWithValue("AWSUploadTS", customerFileObject.AWSUploadTS);
                AddCommonParams(cmd, customerFileObject);
         
                long result = UpdateRecord(cmd);
				if (result > 0)
					customerFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerFileObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerFile
        /// </summary>
        /// <param name="Id">Id of the CustomerFile object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERFILE);	
				
				AddParameter(cmd, pInt32(CustomerFileBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerFile), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerFile object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerFile object to retrieve</param>
        /// <returns>CustomerFile object, null if not found</returns>
		public CustomerFile Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERFILEBYID))
			{
				AddParameter( cmd, pInt32(CustomerFileBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerFile objects 
        /// </summary>
        /// <returns>A list of CustomerFile objects</returns>
		public CustomerFileList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERFILE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerFile objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerFile objects</returns>
		public CustomerFileList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERFILE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerFileList _CustomerFileList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerFileList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerFile objects by query String
        /// </summary>
        /// <returns>A list of CustomerFile objects</returns>
		public CustomerFileList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERFILEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerFile Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERFILEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerFile Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerFileRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERFILEROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerFileRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerFileRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerFile object
        /// </summary>
        /// <param name="customerFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerFileBase customerFileObject, SqlDataReader reader, int start)
		{
			
				customerFileObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) customerFileObject.FileDescription = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) customerFileObject.Filename = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerFileObject.FileFullName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerFileObject.Uploadeddate = reader.GetDateTime( start + 4 );			
				customerFileObject.CustomerId = reader.GetGuid( start + 5 );			
				customerFileObject.CompanyId = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) customerFileObject.IsActive = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) customerFileObject.FileSize = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) customerFileObject.Tag = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerFileObject.InvoiceId = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerFileObject.GeeseFileType = reader.GetString( start + 11 );			
				customerFileObject.CreatedBy = reader.GetGuid( start + 12 );			
				customerFileObject.CreatedDate = reader.GetDateTime( start + 13 );			
				customerFileObject.UpdatedBy = reader.GetGuid( start + 14 );			
				customerFileObject.UpdatedDate = reader.GetDateTime( start + 15 );			
				customerFileObject.FileId = reader.GetGuid( start + 16 );
				if (!reader.IsDBNull(17)) customerFileObject.AWSProcessStatus = reader.GetString(start + 17);
				if (!reader.IsDBNull(18)) customerFileObject.AWSUploadTS = reader.GetDateTime(start + 18);
				if (!reader.IsDBNull(19)) customerFileObject.WMStatus = reader.GetString(start + 19);
            FillBaseObject(customerFileObject, reader, (start + 20));

			
			customerFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerFile object
        /// </summary>
        /// <param name="customerFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerFileBase customerFileObject, SqlDataReader reader)
		{
			FillObject(customerFileObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerFile object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerFile object</returns>
		private CustomerFile GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerFile customerFileObject= new CustomerFile();
					FillObject(customerFileObject, reader);
					return customerFileObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerFile objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerFile objects</returns>
		private CustomerFileList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerFile list
			CustomerFileList list = new CustomerFileList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerFile customerFileObject = new CustomerFile();
					FillObject(customerFileObject, reader);

					list.Add(customerFileObject);
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
