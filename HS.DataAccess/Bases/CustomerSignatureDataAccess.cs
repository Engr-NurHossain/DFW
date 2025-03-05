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
	public partial class CustomerSignatureDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERSIGNATURE = "InsertCustomerSignature";
		private const string UPDATECUSTOMERSIGNATURE = "UpdateCustomerSignature";
		private const string DELETECUSTOMERSIGNATURE = "DeleteCustomerSignature";
		private const string GETCUSTOMERSIGNATUREBYID = "GetCustomerSignatureById";
		private const string GETALLCUSTOMERSIGNATURE = "GetAllCustomerSignature";
		private const string GETPAGEDCUSTOMERSIGNATURE = "GetPagedCustomerSignature";
		private const string GETCUSTOMERSIGNATUREMAXIMUMID = "GetCustomerSignatureMaximumId";
		private const string GETCUSTOMERSIGNATUREROWCOUNT = "GetCustomerSignatureRowCount";	
		private const string GETCUSTOMERSIGNATUREBYQUERY = "GetCustomerSignatureByQuery";
		#endregion
		
		#region Constructors
		public CustomerSignatureDataAccess(ClientContext context) : base(context) { }
		public CustomerSignatureDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerSignatureObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerSignatureBase customerSignatureObject)
		{	
			AddParameter(cmd, pGuid(CustomerSignatureBase.Property_CustomerId, customerSignatureObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerSignatureBase.Property_ReferenceIdGuid, customerSignatureObject.ReferenceIdGuid));
			AddParameter(cmd, pNVarChar(CustomerSignatureBase.Property_ReferenceIdnvarchar, 200, customerSignatureObject.ReferenceIdnvarchar));
			AddParameter(cmd, pNVarChar(CustomerSignatureBase.Property_Type, 50, customerSignatureObject.Type));
			AddParameter(cmd, pNVarChar(CustomerSignatureBase.Property_Signature, 500, customerSignatureObject.Signature));
			AddParameter(cmd, pDateTime(CustomerSignatureBase.Property_CreatedDate, customerSignatureObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerSignatureBase.Property_CreatedBy, customerSignatureObject.CreatedBy));
		}
        #endregion

        #region Insert Method
        /// <summary>
        /// Inserts CustomerSignature
        /// </summary>
        /// <param name="customerSignatureObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
        public long Insert(CustomerSignatureBase customerSignatureObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSIGNATURE);
	
				AddParameter(cmd, pInt32Out(CustomerSignatureBase.Property_Id));
				AddCommonParams(cmd, customerSignatureObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerSignatureObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerSignatureObject.Id = (Int32)GetOutParameter(cmd, CustomerSignatureBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerSignatureObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerSignature
        /// </summary>
        /// <param name="customerSignatureObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerSignatureBase customerSignatureObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERSIGNATURE);
				
				AddParameter(cmd, pInt32(CustomerSignatureBase.Property_Id, customerSignatureObject.Id));
				AddCommonParams(cmd, customerSignatureObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerSignatureObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerSignatureObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerSignature
        /// </summary>
        /// <param name="Id">Id of the CustomerSignature object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERSIGNATURE);	
				
				AddParameter(cmd, pInt32(CustomerSignatureBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerSignature), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerSignature object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerSignature object to retrieve</param>
        /// <returns>CustomerSignature object, null if not found</returns>
		public CustomerSignature Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSIGNATUREBYID))
			{
				AddParameter( cmd, pInt32(CustomerSignatureBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerSignature objects 
        /// </summary>
        /// <returns>A list of CustomerSignature objects</returns>
		public CustomerSignatureList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERSIGNATURE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerSignature objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerSignature objects</returns>
		public CustomerSignatureList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERSIGNATURE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerSignatureList _CustomerSignatureList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerSignatureList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerSignature objects by query String
        /// </summary>
        /// <returns>A list of CustomerSignature objects</returns>
		public CustomerSignatureList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSIGNATUREBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerSignature Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerSignature
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSIGNATUREMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerSignature Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerSignature
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerSignatureRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSIGNATUREROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerSignatureRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerSignatureRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerSignature object
        /// </summary>
        /// <param name="customerSignatureObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerSignatureBase customerSignatureObject, SqlDataReader reader, int start)
		{
			
				customerSignatureObject.Id = reader.GetInt32( start + 0 );			
				customerSignatureObject.CustomerId = reader.GetGuid( start + 1 );			
				customerSignatureObject.ReferenceIdGuid = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerSignatureObject.ReferenceIdnvarchar = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerSignatureObject.Type = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerSignatureObject.Signature = reader.GetString( start + 5 );			
				customerSignatureObject.CreatedDate = reader.GetDateTime( start + 6 );			
				customerSignatureObject.CreatedBy = reader.GetGuid( start + 7 );			
			FillBaseObject(customerSignatureObject, reader, (start + 8));

			
			customerSignatureObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerSignature object
        /// </summary>
        /// <param name="customerSignatureObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerSignatureBase customerSignatureObject, SqlDataReader reader)
		{
			FillObject(customerSignatureObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerSignature object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerSignature object</returns>
		private CustomerSignature GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerSignature customerSignatureObject= new CustomerSignature();
					FillObject(customerSignatureObject, reader);
					return customerSignatureObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerSignature objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerSignature objects</returns>
		private CustomerSignatureList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerSignature list
			CustomerSignatureList list = new CustomerSignatureList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerSignature customerSignatureObject = new CustomerSignature();
					FillObject(customerSignatureObject, reader);

					list.Add(customerSignatureObject);
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
