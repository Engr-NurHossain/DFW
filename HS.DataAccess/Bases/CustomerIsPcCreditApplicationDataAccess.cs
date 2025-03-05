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
	public partial class CustomerIsPcCreditApplicationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERISPCCREDITAPPLICATION = "InsertCustomerIsPcCreditApplication";
		private const string UPDATECUSTOMERISPCCREDITAPPLICATION = "UpdateCustomerIsPcCreditApplication";
		private const string DELETECUSTOMERISPCCREDITAPPLICATION = "DeleteCustomerIsPcCreditApplication";
		private const string GETCUSTOMERISPCCREDITAPPLICATIONBYID = "GetCustomerIsPcCreditApplicationById";
		private const string GETALLCUSTOMERISPCCREDITAPPLICATION = "GetAllCustomerIsPcCreditApplication";
		private const string GETPAGEDCUSTOMERISPCCREDITAPPLICATION = "GetPagedCustomerIsPcCreditApplication";
		private const string GETCUSTOMERISPCCREDITAPPLICATIONMAXIMUMID = "GetCustomerIsPcCreditApplicationMaximumId";
		private const string GETCUSTOMERISPCCREDITAPPLICATIONROWCOUNT = "GetCustomerIsPcCreditApplicationRowCount";	
		private const string GETCUSTOMERISPCCREDITAPPLICATIONBYQUERY = "GetCustomerIsPcCreditApplicationByQuery";
		#endregion
		
		#region Constructors
		public CustomerIsPcCreditApplicationDataAccess(ClientContext context) : base(context) { }
		public CustomerIsPcCreditApplicationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerIsPcCreditApplicationObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerIsPcCreditApplicationBase customerIsPcCreditApplicationObject)
		{	
			AddParameter(cmd, pGuid(CustomerIsPcCreditApplicationBase.Property_CustomerId, customerIsPcCreditApplicationObject.CustomerId));
			AddParameter(cmd, pInt32(CustomerIsPcCreditApplicationBase.Property_MerchantId, customerIsPcCreditApplicationObject.MerchantId));
			AddParameter(cmd, pDouble(CustomerIsPcCreditApplicationBase.Property_AmountRequested, customerIsPcCreditApplicationObject.AmountRequested));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_OptionCode, 50, customerIsPcCreditApplicationObject.OptionCode));
			AddParameter(cmd, pBool(CustomerIsPcCreditApplicationBase.Property_ProdSecuritySystem, customerIsPcCreditApplicationObject.ProdSecuritySystem));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ProdMiscDescription, 200, customerIsPcCreditApplicationObject.ProdMiscDescription));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantLastName, 50, customerIsPcCreditApplicationObject.ApplicantLastName));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantFirstName, 50, customerIsPcCreditApplicationObject.ApplicantFirstName));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantMiddleName, 50, customerIsPcCreditApplicationObject.ApplicantMiddleName));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantNameSuffix, 50, customerIsPcCreditApplicationObject.ApplicantNameSuffix));
			AddParameter(cmd, pDateTime(CustomerIsPcCreditApplicationBase.Property_ApplicantDOB, customerIsPcCreditApplicationObject.ApplicantDOB));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantSSN, 50, customerIsPcCreditApplicationObject.ApplicantSSN));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantHomePhone, 50, customerIsPcCreditApplicationObject.ApplicantHomePhone));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantCellPhone, 50, customerIsPcCreditApplicationObject.ApplicantCellPhone));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantEmailAddress, 50, customerIsPcCreditApplicationObject.ApplicantEmailAddress));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantDriversLicense, 50, customerIsPcCreditApplicationObject.ApplicantDriversLicense));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantStreet, 50, customerIsPcCreditApplicationObject.ApplicantStreet));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantCity, 50, customerIsPcCreditApplicationObject.ApplicantCity));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantState, 50, customerIsPcCreditApplicationObject.ApplicantState));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantZipCode, 50, customerIsPcCreditApplicationObject.ApplicantZipCode));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantCountry, 50, customerIsPcCreditApplicationObject.ApplicantCountry));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantYearsAtAddress, 50, customerIsPcCreditApplicationObject.ApplicantYearsAtAddress));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantMonthsAtAddress, 50, customerIsPcCreditApplicationObject.ApplicantMonthsAtAddress));
			AddParameter(cmd, pDouble(CustomerIsPcCreditApplicationBase.Property_MortgagePayment, customerIsPcCreditApplicationObject.MortgagePayment));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_MortgageHolder, 50, customerIsPcCreditApplicationObject.MortgageHolder));
			AddParameter(cmd, pDouble(CustomerIsPcCreditApplicationBase.Property_MortgageBalance, customerIsPcCreditApplicationObject.MortgageBalance));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_BankName, 50, customerIsPcCreditApplicationObject.BankName));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_BankAcctType, 50, customerIsPcCreditApplicationObject.BankAcctType));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevStreet, 50, customerIsPcCreditApplicationObject.ApplicantPrevStreet));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevCity, 50, customerIsPcCreditApplicationObject.ApplicantPrevCity));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevState, 50, customerIsPcCreditApplicationObject.ApplicantPrevState));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevZipCode, 50, customerIsPcCreditApplicationObject.ApplicantPrevZipCode));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevCountry, 50, customerIsPcCreditApplicationObject.ApplicantPrevCountry));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevYearsAtAddress, 50, customerIsPcCreditApplicationObject.ApplicantPrevYearsAtAddress));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevMonthsAtAddress, 50, customerIsPcCreditApplicationObject.ApplicantPrevMonthsAtAddress));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantEmployer, 50, customerIsPcCreditApplicationObject.ApplicantEmployer));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantYearsEmployed, 50, customerIsPcCreditApplicationObject.ApplicantYearsEmployed));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantMonthsEmployed, 50, customerIsPcCreditApplicationObject.ApplicantMonthsEmployed));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantEmployerPhone, 50, customerIsPcCreditApplicationObject.ApplicantEmployerPhone));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantPosition, 50, customerIsPcCreditApplicationObject.ApplicantPosition));
			AddParameter(cmd, pDouble(CustomerIsPcCreditApplicationBase.Property_ApplicantGrossAnnualIncome, customerIsPcCreditApplicationObject.ApplicantGrossAnnualIncome));
			AddParameter(cmd, pDouble(CustomerIsPcCreditApplicationBase.Property_ApplicantOtherIncome, customerIsPcCreditApplicationObject.ApplicantOtherIncome));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_ApplicantOtherIncomeSource, 50, customerIsPcCreditApplicationObject.ApplicantOtherIncomeSource));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_SelectedAssignedUserId, 50, customerIsPcCreditApplicationObject.SelectedAssignedUserId));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantLastName, 50, customerIsPcCreditApplicationObject.CoapplicantLastName));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantFirstName, 50, customerIsPcCreditApplicationObject.CoapplicantFirstName));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantMiddleName, 50, customerIsPcCreditApplicationObject.CoapplicantMiddleName));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantNameSuffix, 50, customerIsPcCreditApplicationObject.CoapplicantNameSuffix));
			AddParameter(cmd, pDateTime(CustomerIsPcCreditApplicationBase.Property_CoapplicantDOB, customerIsPcCreditApplicationObject.CoapplicantDOB));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantSSN, 50, customerIsPcCreditApplicationObject.CoapplicantSSN));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantHomePhone, 50, customerIsPcCreditApplicationObject.CoapplicantHomePhone));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CpapplicantCellPhone, 50, customerIsPcCreditApplicationObject.CpapplicantCellPhone));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantDriversLicense, 50, customerIsPcCreditApplicationObject.CoapplicantDriversLicense));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantEmailAddress, 50, customerIsPcCreditApplicationObject.CoapplicantEmailAddress));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantStreet, 50, customerIsPcCreditApplicationObject.CoapplicantStreet));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantCity, 50, customerIsPcCreditApplicationObject.CoapplicantCity));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantState, 50, customerIsPcCreditApplicationObject.CoapplicantState));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantZipCode, 50, customerIsPcCreditApplicationObject.CoapplicantZipCode));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantCountry, 50, customerIsPcCreditApplicationObject.CoapplicantCountry));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantYearsAtAddress, 50, customerIsPcCreditApplicationObject.CoapplicantYearsAtAddress));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantMonthsAtAddress, 50, customerIsPcCreditApplicationObject.CoapplicantMonthsAtAddress));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantEmployer, 50, customerIsPcCreditApplicationObject.CoapplicantEmployer));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantYearsEmployed, 50, customerIsPcCreditApplicationObject.CoapplicantYearsEmployed));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantMonthsEmployed, 50, customerIsPcCreditApplicationObject.CoapplicantMonthsEmployed));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantEmployerPhone, 50, customerIsPcCreditApplicationObject.CoapplicantEmployerPhone));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantPosition, 50, customerIsPcCreditApplicationObject.CoapplicantPosition));
			AddParameter(cmd, pDouble(CustomerIsPcCreditApplicationBase.Property_CoapplicantGrossAnnualIncome, customerIsPcCreditApplicationObject.CoapplicantGrossAnnualIncome));
			AddParameter(cmd, pDouble(CustomerIsPcCreditApplicationBase.Property_CoapplicantOtherIncome, customerIsPcCreditApplicationObject.CoapplicantOtherIncome));
			AddParameter(cmd, pNVarChar(CustomerIsPcCreditApplicationBase.Property_CoapplicantOtherIncomeSource, 50, customerIsPcCreditApplicationObject.CoapplicantOtherIncomeSource));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerIsPcCreditApplication
        /// </summary>
        /// <param name="customerIsPcCreditApplicationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerIsPcCreditApplicationBase customerIsPcCreditApplicationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERISPCCREDITAPPLICATION);
	
				AddParameter(cmd, pInt32Out(CustomerIsPcCreditApplicationBase.Property_Id));
				AddCommonParams(cmd, customerIsPcCreditApplicationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerIsPcCreditApplicationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerIsPcCreditApplicationObject.Id = (Int32)GetOutParameter(cmd, CustomerIsPcCreditApplicationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerIsPcCreditApplicationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerIsPcCreditApplication
        /// </summary>
        /// <param name="customerIsPcCreditApplicationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerIsPcCreditApplicationBase customerIsPcCreditApplicationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERISPCCREDITAPPLICATION);
				
				AddParameter(cmd, pInt32(CustomerIsPcCreditApplicationBase.Property_Id, customerIsPcCreditApplicationObject.Id));
				AddCommonParams(cmd, customerIsPcCreditApplicationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerIsPcCreditApplicationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerIsPcCreditApplicationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerIsPcCreditApplication
        /// </summary>
        /// <param name="Id">Id of the CustomerIsPcCreditApplication object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERISPCCREDITAPPLICATION);	
				
				AddParameter(cmd, pInt32(CustomerIsPcCreditApplicationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerIsPcCreditApplication), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerIsPcCreditApplication object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerIsPcCreditApplication object to retrieve</param>
        /// <returns>CustomerIsPcCreditApplication object, null if not found</returns>
		public CustomerIsPcCreditApplication Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERISPCCREDITAPPLICATIONBYID))
			{
				AddParameter( cmd, pInt32(CustomerIsPcCreditApplicationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerIsPcCreditApplication objects 
        /// </summary>
        /// <returns>A list of CustomerIsPcCreditApplication objects</returns>
		public CustomerIsPcCreditApplicationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERISPCCREDITAPPLICATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerIsPcCreditApplication objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerIsPcCreditApplication objects</returns>
		public CustomerIsPcCreditApplicationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERISPCCREDITAPPLICATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerIsPcCreditApplicationList _CustomerIsPcCreditApplicationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerIsPcCreditApplicationList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerIsPcCreditApplication objects by query String
        /// </summary>
        /// <returns>A list of CustomerIsPcCreditApplication objects</returns>
		public CustomerIsPcCreditApplicationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERISPCCREDITAPPLICATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerIsPcCreditApplication Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerIsPcCreditApplication
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERISPCCREDITAPPLICATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerIsPcCreditApplication Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerIsPcCreditApplication
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerIsPcCreditApplicationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERISPCCREDITAPPLICATIONROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerIsPcCreditApplicationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerIsPcCreditApplicationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerIsPcCreditApplication object
        /// </summary>
        /// <param name="customerIsPcCreditApplicationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerIsPcCreditApplicationBase customerIsPcCreditApplicationObject, SqlDataReader reader, int start)
		{
			
				customerIsPcCreditApplicationObject.Id = reader.GetInt32( start + 0 );			
				customerIsPcCreditApplicationObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerIsPcCreditApplicationObject.MerchantId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) customerIsPcCreditApplicationObject.AmountRequested = reader.GetDouble( start + 3 );			
				if(!reader.IsDBNull(4)) customerIsPcCreditApplicationObject.OptionCode = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerIsPcCreditApplicationObject.ProdSecuritySystem = reader.GetBoolean( start + 5 );			
				if(!reader.IsDBNull(6)) customerIsPcCreditApplicationObject.ProdMiscDescription = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerIsPcCreditApplicationObject.ApplicantLastName = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerIsPcCreditApplicationObject.ApplicantFirstName = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerIsPcCreditApplicationObject.ApplicantMiddleName = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerIsPcCreditApplicationObject.ApplicantNameSuffix = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerIsPcCreditApplicationObject.ApplicantDOB = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) customerIsPcCreditApplicationObject.ApplicantSSN = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerIsPcCreditApplicationObject.ApplicantHomePhone = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) customerIsPcCreditApplicationObject.ApplicantCellPhone = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) customerIsPcCreditApplicationObject.ApplicantEmailAddress = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) customerIsPcCreditApplicationObject.ApplicantDriversLicense = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) customerIsPcCreditApplicationObject.ApplicantStreet = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) customerIsPcCreditApplicationObject.ApplicantCity = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) customerIsPcCreditApplicationObject.ApplicantState = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) customerIsPcCreditApplicationObject.ApplicantZipCode = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) customerIsPcCreditApplicationObject.ApplicantCountry = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) customerIsPcCreditApplicationObject.ApplicantYearsAtAddress = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) customerIsPcCreditApplicationObject.ApplicantMonthsAtAddress = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) customerIsPcCreditApplicationObject.MortgagePayment = reader.GetDouble( start + 24 );			
				if(!reader.IsDBNull(25)) customerIsPcCreditApplicationObject.MortgageHolder = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) customerIsPcCreditApplicationObject.MortgageBalance = reader.GetDouble( start + 26 );			
				if(!reader.IsDBNull(27)) customerIsPcCreditApplicationObject.BankName = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) customerIsPcCreditApplicationObject.BankAcctType = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) customerIsPcCreditApplicationObject.ApplicantPrevStreet = reader.GetString( start + 29 );			
				if(!reader.IsDBNull(30)) customerIsPcCreditApplicationObject.ApplicantPrevCity = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) customerIsPcCreditApplicationObject.ApplicantPrevState = reader.GetString( start + 31 );			
				if(!reader.IsDBNull(32)) customerIsPcCreditApplicationObject.ApplicantPrevZipCode = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) customerIsPcCreditApplicationObject.ApplicantPrevCountry = reader.GetString( start + 33 );			
				if(!reader.IsDBNull(34)) customerIsPcCreditApplicationObject.ApplicantPrevYearsAtAddress = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) customerIsPcCreditApplicationObject.ApplicantPrevMonthsAtAddress = reader.GetString( start + 35 );			
				if(!reader.IsDBNull(36)) customerIsPcCreditApplicationObject.ApplicantEmployer = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) customerIsPcCreditApplicationObject.ApplicantYearsEmployed = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) customerIsPcCreditApplicationObject.ApplicantMonthsEmployed = reader.GetString( start + 38 );			
				if(!reader.IsDBNull(39)) customerIsPcCreditApplicationObject.ApplicantEmployerPhone = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) customerIsPcCreditApplicationObject.ApplicantPosition = reader.GetString( start + 40 );			
				if(!reader.IsDBNull(41)) customerIsPcCreditApplicationObject.ApplicantGrossAnnualIncome = reader.GetDouble( start + 41 );			
				if(!reader.IsDBNull(42)) customerIsPcCreditApplicationObject.ApplicantOtherIncome = reader.GetDouble( start + 42 );			
				if(!reader.IsDBNull(43)) customerIsPcCreditApplicationObject.ApplicantOtherIncomeSource = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) customerIsPcCreditApplicationObject.SelectedAssignedUserId = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) customerIsPcCreditApplicationObject.CoapplicantLastName = reader.GetString( start + 45 );			
				if(!reader.IsDBNull(46)) customerIsPcCreditApplicationObject.CoapplicantFirstName = reader.GetString( start + 46 );			
				if(!reader.IsDBNull(47)) customerIsPcCreditApplicationObject.CoapplicantMiddleName = reader.GetString( start + 47 );			
				if(!reader.IsDBNull(48)) customerIsPcCreditApplicationObject.CoapplicantNameSuffix = reader.GetString( start + 48 );			
				if(!reader.IsDBNull(49)) customerIsPcCreditApplicationObject.CoapplicantDOB = reader.GetDateTime( start + 49 );			
				if(!reader.IsDBNull(50)) customerIsPcCreditApplicationObject.CoapplicantSSN = reader.GetString( start + 50 );			
				if(!reader.IsDBNull(51)) customerIsPcCreditApplicationObject.CoapplicantHomePhone = reader.GetString( start + 51 );			
				if(!reader.IsDBNull(52)) customerIsPcCreditApplicationObject.CpapplicantCellPhone = reader.GetString( start + 52 );			
				if(!reader.IsDBNull(53)) customerIsPcCreditApplicationObject.CoapplicantDriversLicense = reader.GetString( start + 53 );			
				if(!reader.IsDBNull(54)) customerIsPcCreditApplicationObject.CoapplicantEmailAddress = reader.GetString( start + 54 );			
				if(!reader.IsDBNull(55)) customerIsPcCreditApplicationObject.CoapplicantStreet = reader.GetString( start + 55 );			
				if(!reader.IsDBNull(56)) customerIsPcCreditApplicationObject.CoapplicantCity = reader.GetString( start + 56 );			
				if(!reader.IsDBNull(57)) customerIsPcCreditApplicationObject.CoapplicantState = reader.GetString( start + 57 );			
				if(!reader.IsDBNull(58)) customerIsPcCreditApplicationObject.CoapplicantZipCode = reader.GetString( start + 58 );			
				if(!reader.IsDBNull(59)) customerIsPcCreditApplicationObject.CoapplicantCountry = reader.GetString( start + 59 );			
				if(!reader.IsDBNull(60)) customerIsPcCreditApplicationObject.CoapplicantYearsAtAddress = reader.GetString( start + 60 );			
				if(!reader.IsDBNull(61)) customerIsPcCreditApplicationObject.CoapplicantMonthsAtAddress = reader.GetString( start + 61 );			
				if(!reader.IsDBNull(62)) customerIsPcCreditApplicationObject.CoapplicantEmployer = reader.GetString( start + 62 );			
				if(!reader.IsDBNull(63)) customerIsPcCreditApplicationObject.CoapplicantYearsEmployed = reader.GetString( start + 63 );			
				if(!reader.IsDBNull(64)) customerIsPcCreditApplicationObject.CoapplicantMonthsEmployed = reader.GetString( start + 64 );			
				if(!reader.IsDBNull(65)) customerIsPcCreditApplicationObject.CoapplicantEmployerPhone = reader.GetString( start + 65 );			
				if(!reader.IsDBNull(66)) customerIsPcCreditApplicationObject.CoapplicantPosition = reader.GetString( start + 66 );			
				if(!reader.IsDBNull(67)) customerIsPcCreditApplicationObject.CoapplicantGrossAnnualIncome = reader.GetDouble( start + 67 );			
				if(!reader.IsDBNull(68)) customerIsPcCreditApplicationObject.CoapplicantOtherIncome = reader.GetDouble( start + 68 );			
				if(!reader.IsDBNull(69)) customerIsPcCreditApplicationObject.CoapplicantOtherIncomeSource = reader.GetString( start + 69 );			
			FillBaseObject(customerIsPcCreditApplicationObject, reader, (start + 70));

			
			customerIsPcCreditApplicationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerIsPcCreditApplication object
        /// </summary>
        /// <param name="customerIsPcCreditApplicationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerIsPcCreditApplicationBase customerIsPcCreditApplicationObject, SqlDataReader reader)
		{
			FillObject(customerIsPcCreditApplicationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerIsPcCreditApplication object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerIsPcCreditApplication object</returns>
		private CustomerIsPcCreditApplication GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerIsPcCreditApplication customerIsPcCreditApplicationObject= new CustomerIsPcCreditApplication();
					FillObject(customerIsPcCreditApplicationObject, reader);
					return customerIsPcCreditApplicationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerIsPcCreditApplication objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerIsPcCreditApplication objects</returns>
		private CustomerIsPcCreditApplicationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerIsPcCreditApplication list
			CustomerIsPcCreditApplicationList list = new CustomerIsPcCreditApplicationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerIsPcCreditApplication customerIsPcCreditApplicationObject = new CustomerIsPcCreditApplication();
					FillObject(customerIsPcCreditApplicationObject, reader);

					list.Add(customerIsPcCreditApplicationObject);
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
