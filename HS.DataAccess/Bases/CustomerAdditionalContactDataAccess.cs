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
	public partial class CustomerAdditionalContactDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERADDITIONALCONTACT = "InsertCustomerAdditionalContact";
		private const string UPDATECUSTOMERADDITIONALCONTACT = "UpdateCustomerAdditionalContact";
		private const string DELETECUSTOMERADDITIONALCONTACT = "DeleteCustomerAdditionalContact";
		private const string GETCUSTOMERADDITIONALCONTACTBYID = "GetCustomerAdditionalContactById";
		private const string GETALLCUSTOMERADDITIONALCONTACT = "GetAllCustomerAdditionalContact";
		private const string GETPAGEDCUSTOMERADDITIONALCONTACT = "GetPagedCustomerAdditionalContact";
		private const string GETCUSTOMERADDITIONALCONTACTMAXIMUMID = "GetCustomerAdditionalContactMaximumId";
		private const string GETCUSTOMERADDITIONALCONTACTROWCOUNT = "GetCustomerAdditionalContactRowCount";	
		private const string GETCUSTOMERADDITIONALCONTACTBYQUERY = "GetCustomerAdditionalContactByQuery";
		#endregion
		
		#region Constructors
		public CustomerAdditionalContactDataAccess(ClientContext context) : base(context) { }
		public CustomerAdditionalContactDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAdditionalContactObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAdditionalContactBase customerAdditionalContactObject)
		{	
			AddParameter(cmd, pGuid(CustomerAdditionalContactBase.Property_CustomerId, customerAdditionalContactObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_CrossSteet, customerAdditionalContactObject.CrossSteet));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_FirstName, 50, customerAdditionalContactObject.FirstName));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_LastName, 50, customerAdditionalContactObject.LastName));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_RelationShip, 50, customerAdditionalContactObject.RelationShip));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_Email, 50, customerAdditionalContactObject.Email));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_Phone, 50, customerAdditionalContactObject.Phone));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_PhoneType, 50, customerAdditionalContactObject.PhoneType));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_AltFirstName, 100, customerAdditionalContactObject.AltFirstName));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_AltLastName, 100, customerAdditionalContactObject.AltLastName));
			AddParameter(cmd, pDateTime(CustomerAdditionalContactBase.Property_DOB, customerAdditionalContactObject.DOB));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_SSN, 100, customerAdditionalContactObject.SSN));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_ExternalID, 100, customerAdditionalContactObject.ExternalID));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_BillingContact, 100, customerAdditionalContactObject.BillingContact));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_BillingPhone, 100, customerAdditionalContactObject.BillingPhone));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_BillingEmail, 100, customerAdditionalContactObject.BillingEmail));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_BillingAddress, 100, customerAdditionalContactObject.BillingAddress));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_BillingZipCode, 100, customerAdditionalContactObject.BillingZipCode));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_BillingCity, 100, customerAdditionalContactObject.BillingCity));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_BillingState, 100, customerAdditionalContactObject.BillingState));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_Phone2, 100, customerAdditionalContactObject.Phone2));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_Phone2Type, 100, customerAdditionalContactObject.Phone2Type));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_Phone3, 100, customerAdditionalContactObject.Phone3));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_Phone3Type, 100, customerAdditionalContactObject.Phone3Type));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_CorpLegalEntityName, 100, customerAdditionalContactObject.CorpLegalEntityName));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_CorpAddress, 100, customerAdditionalContactObject.CorpAddress));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_CorpZipCode, 100, customerAdditionalContactObject.CorpZipCode));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_CorpCity, 100, customerAdditionalContactObject.CorpCity));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_CorpState, 100, customerAdditionalContactObject.CorpState));
			AddParameter(cmd, pBool(CustomerAdditionalContactBase.Property_PointContact, customerAdditionalContactObject.PointContact));
			AddParameter(cmd, pBool(CustomerAdditionalContactBase.Property_AlternateContact, customerAdditionalContactObject.AlternateContact));
			AddParameter(cmd, pBool(CustomerAdditionalContactBase.Property_AuthorizedUser, customerAdditionalContactObject.AuthorizedUser));
			AddParameter(cmd, pBool(CustomerAdditionalContactBase.Property_IsEmergencyContact, customerAdditionalContactObject.IsEmergencyContact));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_CreditScore, 50, customerAdditionalContactObject.CreditScore));
			AddParameter(cmd, pBool(CustomerAdditionalContactBase.Property_IsCreditUsed, customerAdditionalContactObject.IsCreditUsed));
			AddParameter(cmd, pBool(CustomerAdditionalContactBase.Property_IsSigningUsed, customerAdditionalContactObject.IsSigningUsed));
			AddParameter(cmd, pNVarChar(CustomerAdditionalContactBase.Property_ReportPdfLink, 400, customerAdditionalContactObject.ReportPdfLink));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAdditionalContact
        /// </summary>
        /// <param name="customerAdditionalContactObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAdditionalContactBase customerAdditionalContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERADDITIONALCONTACT);
	
				AddParameter(cmd, pInt32Out(CustomerAdditionalContactBase.Property_Id));
				AddCommonParams(cmd, customerAdditionalContactObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAdditionalContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAdditionalContactObject.Id = (Int32)GetOutParameter(cmd, CustomerAdditionalContactBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAdditionalContactObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAdditionalContact
        /// </summary>
        /// <param name="customerAdditionalContactObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAdditionalContactBase customerAdditionalContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERADDITIONALCONTACT);
				
				AddParameter(cmd, pInt32(CustomerAdditionalContactBase.Property_Id, customerAdditionalContactObject.Id));
				AddCommonParams(cmd, customerAdditionalContactObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAdditionalContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAdditionalContactObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAdditionalContact
        /// </summary>
        /// <param name="Id">Id of the CustomerAdditionalContact object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERADDITIONALCONTACT);	
				
				AddParameter(cmd, pInt32(CustomerAdditionalContactBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAdditionalContact), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAdditionalContact object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAdditionalContact object to retrieve</param>
        /// <returns>CustomerAdditionalContact object, null if not found</returns>
		public CustomerAdditionalContact Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDITIONALCONTACTBYID))
			{
				AddParameter( cmd, pInt32(CustomerAdditionalContactBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAdditionalContact objects 
        /// </summary>
        /// <returns>A list of CustomerAdditionalContact objects</returns>
		public CustomerAdditionalContactList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERADDITIONALCONTACT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAdditionalContact objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAdditionalContact objects</returns>
		public CustomerAdditionalContactList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERADDITIONALCONTACT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAdditionalContactList _CustomerAdditionalContactList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAdditionalContactList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAdditionalContact objects by query String
        /// </summary>
        /// <returns>A list of CustomerAdditionalContact objects</returns>
		public CustomerAdditionalContactList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDITIONALCONTACTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAdditionalContact Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAdditionalContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDITIONALCONTACTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAdditionalContact Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAdditionalContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAdditionalContactRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDITIONALCONTACTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAdditionalContactRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAdditionalContactRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAdditionalContact object
        /// </summary>
        /// <param name="customerAdditionalContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAdditionalContactBase customerAdditionalContactObject, SqlDataReader reader, int start)
		{
			
				customerAdditionalContactObject.Id = reader.GetInt32( start + 0 );			
				customerAdditionalContactObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerAdditionalContactObject.CrossSteet = reader.GetString( start + 2 );			
				customerAdditionalContactObject.FirstName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerAdditionalContactObject.LastName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerAdditionalContactObject.RelationShip = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerAdditionalContactObject.Email = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerAdditionalContactObject.Phone = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerAdditionalContactObject.PhoneType = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerAdditionalContactObject.AltFirstName = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerAdditionalContactObject.AltLastName = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerAdditionalContactObject.DOB = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) customerAdditionalContactObject.SSN = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerAdditionalContactObject.ExternalID = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) customerAdditionalContactObject.BillingContact = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) customerAdditionalContactObject.BillingPhone = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) customerAdditionalContactObject.BillingEmail = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) customerAdditionalContactObject.BillingAddress = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) customerAdditionalContactObject.BillingZipCode = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) customerAdditionalContactObject.BillingCity = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) customerAdditionalContactObject.BillingState = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) customerAdditionalContactObject.Phone2 = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) customerAdditionalContactObject.Phone2Type = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) customerAdditionalContactObject.Phone3 = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) customerAdditionalContactObject.Phone3Type = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) customerAdditionalContactObject.CorpLegalEntityName = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) customerAdditionalContactObject.CorpAddress = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) customerAdditionalContactObject.CorpZipCode = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) customerAdditionalContactObject.CorpCity = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) customerAdditionalContactObject.CorpState = reader.GetString( start + 29 );			
				if(!reader.IsDBNull(30)) customerAdditionalContactObject.PointContact = reader.GetBoolean( start + 30 );			
				if(!reader.IsDBNull(31)) customerAdditionalContactObject.AlternateContact = reader.GetBoolean( start + 31 );			
				if(!reader.IsDBNull(32)) customerAdditionalContactObject.AuthorizedUser = reader.GetBoolean( start + 32 );			
				if(!reader.IsDBNull(33)) customerAdditionalContactObject.IsEmergencyContact = reader.GetBoolean( start + 33 );			
				if(!reader.IsDBNull(34)) customerAdditionalContactObject.CreditScore = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) customerAdditionalContactObject.IsCreditUsed = reader.GetBoolean( start + 35 );			
				if(!reader.IsDBNull(36)) customerAdditionalContactObject.IsSigningUsed = reader.GetBoolean( start + 36 );			
				if(!reader.IsDBNull(37)) customerAdditionalContactObject.ReportPdfLink = reader.GetString( start + 37 );			
			FillBaseObject(customerAdditionalContactObject, reader, (start + 38));

			
			customerAdditionalContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAdditionalContact object
        /// </summary>
        /// <param name="customerAdditionalContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAdditionalContactBase customerAdditionalContactObject, SqlDataReader reader)
		{
			FillObject(customerAdditionalContactObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAdditionalContact object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAdditionalContact object</returns>
		private CustomerAdditionalContact GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAdditionalContact customerAdditionalContactObject= new CustomerAdditionalContact();
					FillObject(customerAdditionalContactObject, reader);
					return customerAdditionalContactObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAdditionalContact objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAdditionalContact objects</returns>
		private CustomerAdditionalContactList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAdditionalContact list
			CustomerAdditionalContactList list = new CustomerAdditionalContactList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAdditionalContact customerAdditionalContactObject = new CustomerAdditionalContact();
					FillObject(customerAdditionalContactObject, reader);

					list.Add(customerAdditionalContactObject);
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
