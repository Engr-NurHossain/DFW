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
	public partial class CustomerVaultDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERVAULT = "InsertCustomerVault";
		private const string UPDATECUSTOMERVAULT = "UpdateCustomerVault";
		private const string DELETECUSTOMERVAULT = "DeleteCustomerVault";
		private const string GETCUSTOMERVAULTBYID = "GetCustomerVaultById";
		private const string GETALLCUSTOMERVAULT = "GetAllCustomerVault";
		private const string GETPAGEDCUSTOMERVAULT = "GetPagedCustomerVault";
		private const string GETCUSTOMERVAULTMAXIMUMID = "GetCustomerVaultMaximumId";
		private const string GETCUSTOMERVAULTROWCOUNT = "GetCustomerVaultRowCount";	
		private const string GETCUSTOMERVAULTBYQUERY = "GetCustomerVaultByQuery";
		#endregion
		
		#region Constructors
		public CustomerVaultDataAccess(ClientContext context) : base(context) { }
		public CustomerVaultDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerVaultObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerVaultBase customerVaultObject)
		{	
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_CustomerAccountId, 500, customerVaultObject.CustomerAccountId));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_MonitoringID, 500, customerVaultObject.MonitoringID));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_CustomerName, 500, customerVaultObject.CustomerName));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Address1, 500, customerVaultObject.Address1));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Zip, 500, customerVaultObject.Zip));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Company, 500, customerVaultObject.Company));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_City, 500, customerVaultObject.City));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_LongCity, 500, customerVaultObject.LongCity));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_County, 500, customerVaultObject.County));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_State, 500, customerVaultObject.State));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Phone1, 500, customerVaultObject.Phone1));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Email, 500, customerVaultObject.Email));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Area, 500, customerVaultObject.Area));
			AddParameter(cmd, pDateTime(CustomerVaultBase.Property_SaleDate, customerVaultObject.SaleDate));
			AddParameter(cmd, pDateTime(CustomerVaultBase.Property_InstallDate, customerVaultObject.InstallDate));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Install, 500, customerVaultObject.Install));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_TimeRange, 500, customerVaultObject.TimeRange));
			AddParameter(cmd, pDateTime(CustomerVaultBase.Property_ShortSaleDate, customerVaultObject.ShortSaleDate));
			AddParameter(cmd, pDateTime(CustomerVaultBase.Property_ShortInstallDate, customerVaultObject.ShortInstallDate));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Season, 500, customerVaultObject.Season));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_CustomerAccountIdName, 500, customerVaultObject.CustomerAccountIdName));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_AccountHolderID, 500, customerVaultObject.AccountHolderID));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_MonitoringCompanyID, 500, customerVaultObject.MonitoringCompanyID));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_MonitoringCompany, 500, customerVaultObject.MonitoringCompany));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_PanelTypeNameOLD, 500, customerVaultObject.PanelTypeNameOLD));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_SystemName, 500, customerVaultObject.SystemName));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_SystemPackageName, 500, customerVaultObject.SystemPackageName));
			AddParameter(cmd, pInt32(CustomerVaultBase.Property_CreditScore, customerVaultObject.CreditScore));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_StatusID, 500, customerVaultObject.StatusID));
			AddParameter(cmd, pDouble(CustomerVaultBase.Property_ActivationFeeAmount, customerVaultObject.ActivationFeeAmount));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_AccountHolder1, 500, customerVaultObject.AccountHolder1));
			AddParameter(cmd, pDouble(CustomerVaultBase.Property_MonthlyMonitoringBaseRate, customerVaultObject.MonthlyMonitoringBaseRate));
			AddParameter(cmd, pDouble(CustomerVaultBase.Property_SystemServicesTotal, customerVaultObject.SystemServicesTotal));
			AddParameter(cmd, pDouble(CustomerVaultBase.Property_MonthlyMonitoringRate, customerVaultObject.MonthlyMonitoringRate));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_SalesRep, 500, customerVaultObject.SalesRep));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Technician, 500, customerVaultObject.Technician));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_FriendsFamilyRep, 500, customerVaultObject.FriendsFamilyRep));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_Status, 500, customerVaultObject.Status));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_EquipmentStatus, 500, customerVaultObject.EquipmentStatus));
			AddParameter(cmd, pBool(CustomerVaultBase.Property_isAccountOnline, customerVaultObject.isAccountOnline));
			AddParameter(cmd, pBool(CustomerVaultBase.Property_isAccountInService, customerVaultObject.isAccountInService));
			AddParameter(cmd, pInt32(CustomerVaultBase.Property_CaseCount, customerVaultObject.CaseCount));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_PointsUsedSales, 500, customerVaultObject.PointsUsedSales));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_LeadSource, 500, customerVaultObject.LeadSource));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_LeadSourceId, 500, customerVaultObject.LeadSourceId));
			AddParameter(cmd, pBool(CustomerVaultBase.Property_isRepAccountHold, customerVaultObject.isRepAccountHold));
			AddParameter(cmd, pBool(CustomerVaultBase.Property_isTechAccountHold, customerVaultObject.isTechAccountHold));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_DateEntered, 500, customerVaultObject.DateEntered));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_QA, 500, customerVaultObject.QA));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_QA2, 500, customerVaultObject.QA2));
			AddParameter(cmd, pDateTime(CustomerVaultBase.Property_QA1Date, customerVaultObject.QA1Date));
			AddParameter(cmd, pDouble(CustomerVaultBase.Property_QualityScore, customerVaultObject.QualityScore));
			AddParameter(cmd, pDateTime(CustomerVaultBase.Property_PreScreenDate, customerVaultObject.PreScreenDate));
			AddParameter(cmd, pDateTime(CustomerVaultBase.Property_FullReportDate, customerVaultObject.FullReportDate));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_ContractTerm, 500, customerVaultObject.ContractTerm));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_BillingMethod, 500, customerVaultObject.BillingMethod));
			AddParameter(cmd, pBool(CustomerVaultBase.Property_Takeover, customerVaultObject.Takeover));
			AddParameter(cmd, pDateTime(CustomerVaultBase.Property_DOB, customerVaultObject.DOB));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_ContractId, 500, customerVaultObject.ContractId));
			AddParameter(cmd, pNVarChar(CustomerVaultBase.Property_TransactionID, 500, customerVaultObject.TransactionID));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerVault
        /// </summary>
        /// <param name="customerVaultObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerVaultBase customerVaultObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERVAULT);
	
				AddParameter(cmd, pInt32Out(CustomerVaultBase.Property_Id));
				AddCommonParams(cmd, customerVaultObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerVaultObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerVaultObject.Id = (Int32)GetOutParameter(cmd, CustomerVaultBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerVaultObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerVault
        /// </summary>
        /// <param name="customerVaultObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerVaultBase customerVaultObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERVAULT);
				
				AddParameter(cmd, pInt32(CustomerVaultBase.Property_Id, customerVaultObject.Id));
				AddCommonParams(cmd, customerVaultObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerVaultObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerVaultObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerVault
        /// </summary>
        /// <param name="Id">Id of the CustomerVault object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERVAULT);	
				
				AddParameter(cmd, pInt32(CustomerVaultBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerVault), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerVault object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerVault object to retrieve</param>
        /// <returns>CustomerVault object, null if not found</returns>
		public CustomerVault Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERVAULTBYID))
			{
				AddParameter( cmd, pInt32(CustomerVaultBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerVault objects 
        /// </summary>
        /// <returns>A list of CustomerVault objects</returns>
		public CustomerVaultList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERVAULT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerVault objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerVault objects</returns>
		public CustomerVaultList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERVAULT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerVaultList _CustomerVaultList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerVaultList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerVault objects by query String
        /// </summary>
        /// <returns>A list of CustomerVault objects</returns>
		public CustomerVaultList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERVAULTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerVault Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerVault
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERVAULTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerVault Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerVault
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerVaultRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERVAULTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerVaultRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerVaultRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerVault object
        /// </summary>
        /// <param name="customerVaultObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerVaultBase customerVaultObject, SqlDataReader reader, int start)
		{
			
				customerVaultObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) customerVaultObject.CustomerAccountId = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) customerVaultObject.MonitoringID = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerVaultObject.CustomerName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerVaultObject.Address1 = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerVaultObject.Zip = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerVaultObject.Company = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerVaultObject.City = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerVaultObject.LongCity = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerVaultObject.County = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerVaultObject.State = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerVaultObject.Phone1 = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerVaultObject.Email = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerVaultObject.Area = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) customerVaultObject.SaleDate = reader.GetDateTime( start + 14 );			
				if(!reader.IsDBNull(15)) customerVaultObject.InstallDate = reader.GetDateTime( start + 15 );			
				if(!reader.IsDBNull(16)) customerVaultObject.Install = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) customerVaultObject.TimeRange = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) customerVaultObject.ShortSaleDate = reader.GetDateTime( start + 18 );			
				if(!reader.IsDBNull(19)) customerVaultObject.ShortInstallDate = reader.GetDateTime( start + 19 );			
				if(!reader.IsDBNull(20)) customerVaultObject.Season = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) customerVaultObject.CustomerAccountIdName = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) customerVaultObject.AccountHolderID = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) customerVaultObject.MonitoringCompanyID = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) customerVaultObject.MonitoringCompany = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) customerVaultObject.PanelTypeNameOLD = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) customerVaultObject.SystemName = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) customerVaultObject.SystemPackageName = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) customerVaultObject.CreditScore = reader.GetInt32( start + 28 );			
				if(!reader.IsDBNull(29)) customerVaultObject.StatusID = reader.GetString( start + 29 );			
				if(!reader.IsDBNull(30)) customerVaultObject.ActivationFeeAmount = reader.GetDouble( start + 30 );			
				if(!reader.IsDBNull(31)) customerVaultObject.AccountHolder1 = reader.GetString( start + 31 );			
				if(!reader.IsDBNull(32)) customerVaultObject.MonthlyMonitoringBaseRate = reader.GetDouble( start + 32 );			
				if(!reader.IsDBNull(33)) customerVaultObject.SystemServicesTotal = reader.GetDouble( start + 33 );			
				if(!reader.IsDBNull(34)) customerVaultObject.MonthlyMonitoringRate = reader.GetDouble( start + 34 );			
				if(!reader.IsDBNull(35)) customerVaultObject.SalesRep = reader.GetString( start + 35 );			
				if(!reader.IsDBNull(36)) customerVaultObject.Technician = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) customerVaultObject.FriendsFamilyRep = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) customerVaultObject.Status = reader.GetString( start + 38 );			
				if(!reader.IsDBNull(39)) customerVaultObject.EquipmentStatus = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) customerVaultObject.isAccountOnline = reader.GetBoolean( start + 40 );			
				if(!reader.IsDBNull(41)) customerVaultObject.isAccountInService = reader.GetBoolean( start + 41 );			
				if(!reader.IsDBNull(42)) customerVaultObject.CaseCount = reader.GetInt32( start + 42 );			
				if(!reader.IsDBNull(43)) customerVaultObject.PointsUsedSales = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) customerVaultObject.LeadSource = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) customerVaultObject.LeadSourceId = reader.GetString( start + 45 );			
				if(!reader.IsDBNull(46)) customerVaultObject.isRepAccountHold = reader.GetBoolean( start + 46 );			
				if(!reader.IsDBNull(47)) customerVaultObject.isTechAccountHold = reader.GetBoolean( start + 47 );			
				if(!reader.IsDBNull(48)) customerVaultObject.DateEntered = reader.GetString( start + 48 );			
				if(!reader.IsDBNull(49)) customerVaultObject.QA = reader.GetString( start + 49 );			
				if(!reader.IsDBNull(50)) customerVaultObject.QA2 = reader.GetString( start + 50 );			
				if(!reader.IsDBNull(51)) customerVaultObject.QA1Date = reader.GetDateTime( start + 51 );			
				if(!reader.IsDBNull(52)) customerVaultObject.QualityScore = reader.GetDouble( start + 52 );			
				if(!reader.IsDBNull(53)) customerVaultObject.PreScreenDate = reader.GetDateTime( start + 53 );			
				if(!reader.IsDBNull(54)) customerVaultObject.FullReportDate = reader.GetDateTime( start + 54 );			
				if(!reader.IsDBNull(55)) customerVaultObject.ContractTerm = reader.GetString( start + 55 );			
				if(!reader.IsDBNull(56)) customerVaultObject.BillingMethod = reader.GetString( start + 56 );			
				if(!reader.IsDBNull(57)) customerVaultObject.Takeover = reader.GetBoolean( start + 57 );			
				if(!reader.IsDBNull(58)) customerVaultObject.DOB = reader.GetDateTime( start + 58 );			
				if(!reader.IsDBNull(59)) customerVaultObject.ContractId = reader.GetString( start + 59 );			
				if(!reader.IsDBNull(60)) customerVaultObject.TransactionID = reader.GetString( start + 60 );			
			FillBaseObject(customerVaultObject, reader, (start + 61));

			
			customerVaultObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerVault object
        /// </summary>
        /// <param name="customerVaultObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerVaultBase customerVaultObject, SqlDataReader reader)
		{
			FillObject(customerVaultObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerVault object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerVault object</returns>
		private CustomerVault GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerVault customerVaultObject= new CustomerVault();
					FillObject(customerVaultObject, reader);
					return customerVaultObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerVault objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerVault objects</returns>
		private CustomerVaultList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerVault list
			CustomerVaultList list = new CustomerVaultList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerVault customerVaultObject = new CustomerVault();
					FillObject(customerVaultObject, reader);

					list.Add(customerVaultObject);
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
