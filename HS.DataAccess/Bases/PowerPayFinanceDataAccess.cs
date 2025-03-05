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
	public partial class PowerPayFinanceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPOWERPAYFINANCE = "InsertPowerPayFinance";
		private const string UPDATEPOWERPAYFINANCE = "UpdatePowerPayFinance";
		private const string DELETEPOWERPAYFINANCE = "DeletePowerPayFinance";
		private const string GETPOWERPAYFINANCEBYID = "GetPowerPayFinanceById";
		private const string GETALLPOWERPAYFINANCE = "GetAllPowerPayFinance";
		private const string GETPAGEDPOWERPAYFINANCE = "GetPagedPowerPayFinance";
		private const string GETPOWERPAYFINANCEMAXIMUMID = "GetPowerPayFinanceMaximumId";
		private const string GETPOWERPAYFINANCEROWCOUNT = "GetPowerPayFinanceRowCount";	
		private const string GETPOWERPAYFINANCEBYQUERY = "GetPowerPayFinanceByQuery";
		#endregion
		
		#region Constructors
		public PowerPayFinanceDataAccess(ClientContext context) : base(context) { }
		public PowerPayFinanceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="powerPayFinanceObject"></param>
		private void AddCommonParams(SqlCommand cmd, PowerPayFinanceBase powerPayFinanceObject)
		{	
			AddParameter(cmd, pGuid(PowerPayFinanceBase.Property_CustomerId, powerPayFinanceObject.CustomerId));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_Firstname, 50, powerPayFinanceObject.Firstname));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_Lastname, 50, powerPayFinanceObject.Lastname));
			AddParameter(cmd, pDateTime(PowerPayFinanceBase.Property_DOB, powerPayFinanceObject.DOB));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_Phone, 50, powerPayFinanceObject.Phone));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_Email, 50, powerPayFinanceObject.Email));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_SSN, 50, powerPayFinanceObject.SSN));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_USCitizen, 50, powerPayFinanceObject.USCitizen));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_PPAddressPB, 50, powerPayFinanceObject.PPAddressPB));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_AddressStreetNumber, 50, powerPayFinanceObject.AddressStreetNumber));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_AddressStreetName, 50, powerPayFinanceObject.AddressStreetName));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_AddressStreetType, 50, powerPayFinanceObject.AddressStreetType));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_POBox, 50, powerPayFinanceObject.POBox));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_City, 50, powerPayFinanceObject.City));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_State, 50, powerPayFinanceObject.State));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_ZipCode, 50, powerPayFinanceObject.ZipCode));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_AddressHouseType, 50, powerPayFinanceObject.AddressHouseType));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_ActiveMilitary, 50, powerPayFinanceObject.ActiveMilitary));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_DriversLicense, 50, powerPayFinanceObject.DriversLicense));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_DriversLicenseState, 50, powerPayFinanceObject.DriversLicenseState));
			AddParameter(cmd, pDouble(PowerPayFinanceBase.Property_AnnualIncome, powerPayFinanceObject.AnnualIncome));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_IncomeFrequency, 50, powerPayFinanceObject.IncomeFrequency));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_EmployerOccupation, 50, powerPayFinanceObject.EmployerOccupation));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_EmployerName, 50, powerPayFinanceObject.EmployerName));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_EmployerZip, 50, powerPayFinanceObject.EmployerZip));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_EmploymentType, 50, powerPayFinanceObject.EmploymentType));
			AddParameter(cmd, pInt32(PowerPayFinanceBase.Property_EmployerYears, powerPayFinanceObject.EmployerYears));
			AddParameter(cmd, pDouble(PowerPayFinanceBase.Property_RequestedLoanAmount, powerPayFinanceObject.RequestedLoanAmount));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_ith_email, 50, powerPayFinanceObject.ith_email));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_IHRMobileNumber, 50, powerPayFinanceObject.IHRMobileNumber));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoFirstname, 50, powerPayFinanceObject.CoFirstname));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoLastname, 50, powerPayFinanceObject.CoLastname));
			AddParameter(cmd, pDateTime(PowerPayFinanceBase.Property_CoDOB, powerPayFinanceObject.CoDOB));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoPhone, 50, powerPayFinanceObject.CoPhone));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoEmail, 50, powerPayFinanceObject.CoEmail));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoSSN, 50, powerPayFinanceObject.CoSSN));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoUSCitizen, 50, powerPayFinanceObject.CoUSCitizen));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoPPAddressPB, 50, powerPayFinanceObject.CoPPAddressPB));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoAddressStreetNumber, 50, powerPayFinanceObject.CoAddressStreetNumber));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoAddressStreetName, 50, powerPayFinanceObject.CoAddressStreetName));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoAddressStreetType, 50, powerPayFinanceObject.CoAddressStreetType));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoPOBox, 50, powerPayFinanceObject.CoPOBox));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoCity, 50, powerPayFinanceObject.CoCity));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoState, 50, powerPayFinanceObject.CoState));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoZipCode, 50, powerPayFinanceObject.CoZipCode));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoAddressHouseType, 50, powerPayFinanceObject.CoAddressHouseType));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoActiveMilitary, 50, powerPayFinanceObject.CoActiveMilitary));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoDriversLicense, 50, powerPayFinanceObject.CoDriversLicense));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoDriversLicenseState, 50, powerPayFinanceObject.CoDriversLicenseState));
			AddParameter(cmd, pDouble(PowerPayFinanceBase.Property_CoAnnualIncome, powerPayFinanceObject.CoAnnualIncome));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoIncomeFrequency, 50, powerPayFinanceObject.CoIncomeFrequency));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoEmployerOccupation, 50, powerPayFinanceObject.CoEmployerOccupation));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoEmployerName, 50, powerPayFinanceObject.CoEmployerName));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoEmployerZip, 50, powerPayFinanceObject.CoEmployerZip));
			AddParameter(cmd, pNVarChar(PowerPayFinanceBase.Property_CoEmploymentType, 50, powerPayFinanceObject.CoEmploymentType));
			AddParameter(cmd, pInt32(PowerPayFinanceBase.Property_CoEmployerYears, powerPayFinanceObject.CoEmployerYears));
			AddParameter(cmd, pBool(PowerPayFinanceBase.Property_IsApply, powerPayFinanceObject.IsApply));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PowerPayFinance
        /// </summary>
        /// <param name="powerPayFinanceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PowerPayFinanceBase powerPayFinanceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPOWERPAYFINANCE);
	
				AddParameter(cmd, pInt32Out(PowerPayFinanceBase.Property_Id));
				AddCommonParams(cmd, powerPayFinanceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					powerPayFinanceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					powerPayFinanceObject.Id = (Int32)GetOutParameter(cmd, PowerPayFinanceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(powerPayFinanceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PowerPayFinance
        /// </summary>
        /// <param name="powerPayFinanceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PowerPayFinanceBase powerPayFinanceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPOWERPAYFINANCE);
				
				AddParameter(cmd, pInt32(PowerPayFinanceBase.Property_Id, powerPayFinanceObject.Id));
				AddCommonParams(cmd, powerPayFinanceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					powerPayFinanceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(powerPayFinanceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PowerPayFinance
        /// </summary>
        /// <param name="Id">Id of the PowerPayFinance object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPOWERPAYFINANCE);	
				
				AddParameter(cmd, pInt32(PowerPayFinanceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PowerPayFinance), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PowerPayFinance object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PowerPayFinance object to retrieve</param>
        /// <returns>PowerPayFinance object, null if not found</returns>
		public PowerPayFinance Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPOWERPAYFINANCEBYID))
			{
				AddParameter( cmd, pInt32(PowerPayFinanceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PowerPayFinance objects 
        /// </summary>
        /// <returns>A list of PowerPayFinance objects</returns>
		public PowerPayFinanceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPOWERPAYFINANCE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PowerPayFinance objects by PageRequest
        /// </summary>
        /// <returns>A list of PowerPayFinance objects</returns>
		public PowerPayFinanceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPOWERPAYFINANCE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PowerPayFinanceList _PowerPayFinanceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PowerPayFinanceList;
			}
		}
		
		/// <summary>
        /// Retrieves all PowerPayFinance objects by query String
        /// </summary>
        /// <returns>A list of PowerPayFinance objects</returns>
		public PowerPayFinanceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPOWERPAYFINANCEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PowerPayFinance Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PowerPayFinance
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPOWERPAYFINANCEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PowerPayFinance Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PowerPayFinance
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PowerPayFinanceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPOWERPAYFINANCEROWCOUNT))
			{
				SqlDataReader reader;
				_PowerPayFinanceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PowerPayFinanceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PowerPayFinance object
        /// </summary>
        /// <param name="powerPayFinanceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PowerPayFinanceBase powerPayFinanceObject, SqlDataReader reader, int start)
		{
			
				powerPayFinanceObject.Id = reader.GetInt32( start + 0 );			
				powerPayFinanceObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) powerPayFinanceObject.Firstname = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) powerPayFinanceObject.Lastname = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) powerPayFinanceObject.DOB = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) powerPayFinanceObject.Phone = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) powerPayFinanceObject.Email = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) powerPayFinanceObject.SSN = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) powerPayFinanceObject.USCitizen = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) powerPayFinanceObject.PPAddressPB = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) powerPayFinanceObject.AddressStreetNumber = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) powerPayFinanceObject.AddressStreetName = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) powerPayFinanceObject.AddressStreetType = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) powerPayFinanceObject.POBox = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) powerPayFinanceObject.City = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) powerPayFinanceObject.State = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) powerPayFinanceObject.ZipCode = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) powerPayFinanceObject.AddressHouseType = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) powerPayFinanceObject.ActiveMilitary = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) powerPayFinanceObject.DriversLicense = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) powerPayFinanceObject.DriversLicenseState = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) powerPayFinanceObject.AnnualIncome = reader.GetDouble( start + 21 );			
				if(!reader.IsDBNull(22)) powerPayFinanceObject.IncomeFrequency = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) powerPayFinanceObject.EmployerOccupation = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) powerPayFinanceObject.EmployerName = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) powerPayFinanceObject.EmployerZip = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) powerPayFinanceObject.EmploymentType = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) powerPayFinanceObject.EmployerYears = reader.GetInt32( start + 27 );			
				if(!reader.IsDBNull(28)) powerPayFinanceObject.RequestedLoanAmount = reader.GetDouble( start + 28 );			
				if(!reader.IsDBNull(29)) powerPayFinanceObject.ith_email = reader.GetString( start + 29 );			
				if(!reader.IsDBNull(30)) powerPayFinanceObject.IHRMobileNumber = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) powerPayFinanceObject.CoFirstname = reader.GetString( start + 31 );			
				if(!reader.IsDBNull(32)) powerPayFinanceObject.CoLastname = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) powerPayFinanceObject.CoDOB = reader.GetDateTime( start + 33 );			
				if(!reader.IsDBNull(34)) powerPayFinanceObject.CoPhone = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) powerPayFinanceObject.CoEmail = reader.GetString( start + 35 );			
				if(!reader.IsDBNull(36)) powerPayFinanceObject.CoSSN = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) powerPayFinanceObject.CoUSCitizen = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) powerPayFinanceObject.CoPPAddressPB = reader.GetString( start + 38 );			
				if(!reader.IsDBNull(39)) powerPayFinanceObject.CoAddressStreetNumber = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) powerPayFinanceObject.CoAddressStreetName = reader.GetString( start + 40 );			
				if(!reader.IsDBNull(41)) powerPayFinanceObject.CoAddressStreetType = reader.GetString( start + 41 );			
				if(!reader.IsDBNull(42)) powerPayFinanceObject.CoPOBox = reader.GetString( start + 42 );			
				if(!reader.IsDBNull(43)) powerPayFinanceObject.CoCity = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) powerPayFinanceObject.CoState = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) powerPayFinanceObject.CoZipCode = reader.GetString( start + 45 );			
				if(!reader.IsDBNull(46)) powerPayFinanceObject.CoAddressHouseType = reader.GetString( start + 46 );			
				if(!reader.IsDBNull(47)) powerPayFinanceObject.CoActiveMilitary = reader.GetString( start + 47 );			
				if(!reader.IsDBNull(48)) powerPayFinanceObject.CoDriversLicense = reader.GetString( start + 48 );			
				if(!reader.IsDBNull(49)) powerPayFinanceObject.CoDriversLicenseState = reader.GetString( start + 49 );			
				if(!reader.IsDBNull(50)) powerPayFinanceObject.CoAnnualIncome = reader.GetDouble( start + 50 );			
				if(!reader.IsDBNull(51)) powerPayFinanceObject.CoIncomeFrequency = reader.GetString( start + 51 );			
				if(!reader.IsDBNull(52)) powerPayFinanceObject.CoEmployerOccupation = reader.GetString( start + 52 );			
				if(!reader.IsDBNull(53)) powerPayFinanceObject.CoEmployerName = reader.GetString( start + 53 );			
				if(!reader.IsDBNull(54)) powerPayFinanceObject.CoEmployerZip = reader.GetString( start + 54 );			
				if(!reader.IsDBNull(55)) powerPayFinanceObject.CoEmploymentType = reader.GetString( start + 55 );			
				if(!reader.IsDBNull(56)) powerPayFinanceObject.CoEmployerYears = reader.GetInt32( start + 56 );			
				if(!reader.IsDBNull(57)) powerPayFinanceObject.IsApply = reader.GetBoolean( start + 57 );			
			FillBaseObject(powerPayFinanceObject, reader, (start + 58));

			
			powerPayFinanceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PowerPayFinance object
        /// </summary>
        /// <param name="powerPayFinanceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PowerPayFinanceBase powerPayFinanceObject, SqlDataReader reader)
		{
			FillObject(powerPayFinanceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PowerPayFinance object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PowerPayFinance object</returns>
		private PowerPayFinance GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PowerPayFinance powerPayFinanceObject= new PowerPayFinance();
					FillObject(powerPayFinanceObject, reader);
					return powerPayFinanceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PowerPayFinance objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PowerPayFinance objects</returns>
		private PowerPayFinanceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PowerPayFinance list
			PowerPayFinanceList list = new PowerPayFinanceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PowerPayFinance powerPayFinanceObject = new PowerPayFinance();
					FillObject(powerPayFinanceObject, reader);

					list.Add(powerPayFinanceObject);
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
