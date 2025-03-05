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
	public partial class CustomerCreditCheckDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERCREDITCHECK = "InsertCustomerCreditCheck";
		private const string UPDATECUSTOMERCREDITCHECK = "UpdateCustomerCreditCheck";
		private const string DELETECUSTOMERCREDITCHECK = "DeleteCustomerCreditCheck";
		private const string GETCUSTOMERCREDITCHECKBYID = "GetCustomerCreditCheckById";
		private const string GETALLCUSTOMERCREDITCHECK = "GetAllCustomerCreditCheck";
		private const string GETPAGEDCUSTOMERCREDITCHECK = "GetPagedCustomerCreditCheck";
		private const string GETCUSTOMERCREDITCHECKMAXIMUMID = "GetCustomerCreditCheckMaximumId";
		private const string GETCUSTOMERCREDITCHECKROWCOUNT = "GetCustomerCreditCheckRowCount";	
		private const string GETCUSTOMERCREDITCHECKBYQUERY = "GetCustomerCreditCheckByQuery";
		#endregion
		
		#region Constructors
		public CustomerCreditCheckDataAccess(ClientContext context) : base(context) { }
		public CustomerCreditCheckDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerCreditCheckObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerCreditCheckBase customerCreditCheckObject)
		{	
			AddParameter(cmd, pGuid(CustomerCreditCheckBase.Property_CustomerId, customerCreditCheckObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_FirstName, 50, customerCreditCheckObject.FirstName));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_LastName, 50, customerCreditCheckObject.LastName));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_CreditAddress, customerCreditCheckObject.CreditAddress));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_CreditZipCode, 50, customerCreditCheckObject.CreditZipCode));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_CreditCity, 50, customerCreditCheckObject.CreditCity));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_CreditState, 50, customerCreditCheckObject.CreditState));
			AddParameter(cmd, pDateTime(CustomerCreditCheckBase.Property_DateOfBirth, customerCreditCheckObject.DateOfBirth));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_SocialSecurityNumber, 50, customerCreditCheckObject.SocialSecurityNumber));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_CreditBureau, 50, customerCreditCheckObject.CreditBureau));
			AddParameter(cmd, pDateTime(CustomerCreditCheckBase.Property_CreditCheckDate, customerCreditCheckObject.CreditCheckDate));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_CustomerName, 50, customerCreditCheckObject.CustomerName));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_Score, 50, customerCreditCheckObject.Score));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_ProviderCreditRating, 50, customerCreditCheckObject.ProviderCreditRating));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_Hit, 50, customerCreditCheckObject.Hit));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_RepontPdfName, 50, customerCreditCheckObject.RepontPdfName));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_ReportPdfLink, customerCreditCheckObject.ReportPdfLink));
			AddParameter(cmd, pGuid(CustomerCreditCheckBase.Property_CreatedBy, customerCreditCheckObject.CreatedBy));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_TransectionId, 100, customerCreditCheckObject.TransectionId));
			AddParameter(cmd, pGuid(CustomerCreditCheckBase.Property_CompanyId, customerCreditCheckObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerCreditCheckBase.Property_CreditCheckDesc, 100, customerCreditCheckObject.CreditCheckDesc));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerCreditCheck
        /// </summary>
        /// <param name="customerCreditCheckObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerCreditCheckBase customerCreditCheckObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERCREDITCHECK);
	
				AddParameter(cmd, pInt32Out(CustomerCreditCheckBase.Property_Id));
				AddCommonParams(cmd, customerCreditCheckObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerCreditCheckObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerCreditCheckObject.Id = (Int32)GetOutParameter(cmd, CustomerCreditCheckBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerCreditCheckObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerCreditCheck
        /// </summary>
        /// <param name="customerCreditCheckObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerCreditCheckBase customerCreditCheckObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERCREDITCHECK);
				
				AddParameter(cmd, pInt32(CustomerCreditCheckBase.Property_Id, customerCreditCheckObject.Id));
				AddCommonParams(cmd, customerCreditCheckObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerCreditCheckObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerCreditCheckObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerCreditCheck
        /// </summary>
        /// <param name="Id">Id of the CustomerCreditCheck object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERCREDITCHECK);	
				
				AddParameter(cmd, pInt32(CustomerCreditCheckBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerCreditCheck), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerCreditCheck object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerCreditCheck object to retrieve</param>
        /// <returns>CustomerCreditCheck object, null if not found</returns>
		public CustomerCreditCheck Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCREDITCHECKBYID))
			{
				AddParameter( cmd, pInt32(CustomerCreditCheckBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerCreditCheck objects 
        /// </summary>
        /// <returns>A list of CustomerCreditCheck objects</returns>
		public CustomerCreditCheckList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERCREDITCHECK))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerCreditCheck objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerCreditCheck objects</returns>
		public CustomerCreditCheckList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERCREDITCHECK))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerCreditCheckList _CustomerCreditCheckList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerCreditCheckList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerCreditCheck objects by query String
        /// </summary>
        /// <returns>A list of CustomerCreditCheck objects</returns>
		public CustomerCreditCheckList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCREDITCHECKBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerCreditCheck Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerCreditCheck
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCREDITCHECKMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerCreditCheck Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerCreditCheck
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerCreditCheckRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCREDITCHECKROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerCreditCheckRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerCreditCheckRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerCreditCheck object
        /// </summary>
        /// <param name="customerCreditCheckObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerCreditCheckBase customerCreditCheckObject, SqlDataReader reader, int start)
		{
			
				customerCreditCheckObject.Id = reader.GetInt32( start + 0 );			
				customerCreditCheckObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerCreditCheckObject.FirstName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerCreditCheckObject.LastName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerCreditCheckObject.CreditAddress = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerCreditCheckObject.CreditZipCode = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerCreditCheckObject.CreditCity = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerCreditCheckObject.CreditState = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerCreditCheckObject.DateOfBirth = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) customerCreditCheckObject.SocialSecurityNumber = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerCreditCheckObject.CreditBureau = reader.GetString( start + 10 );			
				customerCreditCheckObject.CreditCheckDate = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) customerCreditCheckObject.CustomerName = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerCreditCheckObject.Score = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) customerCreditCheckObject.ProviderCreditRating = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) customerCreditCheckObject.Hit = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) customerCreditCheckObject.RepontPdfName = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) customerCreditCheckObject.ReportPdfLink = reader.GetString( start + 17 );			
				customerCreditCheckObject.CreatedBy = reader.GetGuid( start + 18 );			
				if(!reader.IsDBNull(19)) customerCreditCheckObject.TransectionId = reader.GetString( start + 19 );			
				customerCreditCheckObject.CompanyId = reader.GetGuid( start + 20 );			
				if(!reader.IsDBNull(21)) customerCreditCheckObject.CreditCheckDesc = reader.GetString( start + 21 );			
			FillBaseObject(customerCreditCheckObject, reader, (start + 22));

			
			customerCreditCheckObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerCreditCheck object
        /// </summary>
        /// <param name="customerCreditCheckObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerCreditCheckBase customerCreditCheckObject, SqlDataReader reader)
		{
			FillObject(customerCreditCheckObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerCreditCheck object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerCreditCheck object</returns>
		private CustomerCreditCheck GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerCreditCheck customerCreditCheckObject= new CustomerCreditCheck();
					FillObject(customerCreditCheckObject, reader);
					return customerCreditCheckObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerCreditCheck objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerCreditCheck objects</returns>
		private CustomerCreditCheckList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerCreditCheck list
			CustomerCreditCheckList list = new CustomerCreditCheckList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerCreditCheck customerCreditCheckObject = new CustomerCreditCheck();
					FillObject(customerCreditCheckObject, reader);

					list.Add(customerCreditCheckObject);
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
