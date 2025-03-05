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
	public partial class QA1ScriptDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTQA1SCRIPT = "InsertQA1Script";
		private const string UPDATEQA1SCRIPT = "UpdateQA1Script";
		private const string DELETEQA1SCRIPT = "DeleteQA1Script";
		private const string GETQA1SCRIPTBYID = "GetQA1ScriptById";
		private const string GETALLQA1SCRIPT = "GetAllQA1Script";
		private const string GETPAGEDQA1SCRIPT = "GetPagedQA1Script";
		private const string GETQA1SCRIPTMAXIMUMID = "GetQA1ScriptMaximumId";
		private const string GETQA1SCRIPTROWCOUNT = "GetQA1ScriptRowCount";	
		private const string GETQA1SCRIPTBYQUERY = "GetQA1ScriptByQuery";
		#endregion
		
		#region Constructors
		public QA1ScriptDataAccess(ClientContext context) : base(context) { }
		public QA1ScriptDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="qA1ScriptObject"></param>
		private void AddCommonParams(SqlCommand cmd, QA1ScriptBase qA1ScriptObject)
		{	
			AddParameter(cmd, pGuid(QA1ScriptBase.Property_CustomerId, qA1ScriptObject.CustomerId));
			AddParameter(cmd, pGuid(QA1ScriptBase.Property_CompanyId, qA1ScriptObject.CompanyId));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_FirstName, 150, qA1ScriptObject.FirstName));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_LastName, 150, qA1ScriptObject.LastName));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_NameIsCorrect, 50, qA1ScriptObject.NameIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_NameUpdateNote, 500, qA1ScriptObject.NameUpdateNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_Street, 500, qA1ScriptObject.Street));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_City, 50, qA1ScriptObject.City));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_State, 50, qA1ScriptObject.State));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_ZipCode, 50, qA1ScriptObject.ZipCode));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_AddressIsCorrect, 50, qA1ScriptObject.AddressIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_AddressUpdateNote, 500, qA1ScriptObject.AddressUpdateNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsLocationHome, 50, qA1ScriptObject.IsLocationHome));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsHomeRent, 50, qA1ScriptObject.IsHomeRent));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_HomeownerName, 100, qA1ScriptObject.HomeownerName));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_HomeownerNameUpdateNote, 500, qA1ScriptObject.HomeownerNameUpdateNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsSpeckRep, 50, qA1ScriptObject.IsSpeckRep));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsHomeownerAuthorized, 50, qA1ScriptObject.IsHomeownerAuthorized));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsOnTheDeed, 50, qA1ScriptObject.IsOnTheDeed));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsResponsibleParty, 50, qA1ScriptObject.IsResponsibleParty));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_ResponsiblePartyName, 100, qA1ScriptObject.ResponsiblePartyName));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_ReponsiblePartyNameUpdateNote, 500, qA1ScriptObject.ReponsiblePartyNameUpdateNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsSignShowingBusiness, 50, qA1ScriptObject.IsSignShowingBusiness));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_SignShowText, 100, qA1ScriptObject.SignShowText));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsOtherDocumentShowing, 50, qA1ScriptObject.IsOtherDocumentShowing));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_OtherDocumentShowingText, 100, qA1ScriptObject.OtherDocumentShowingText));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsBusinessSecretaryOfState, 50, qA1ScriptObject.IsBusinessSecretaryOfState));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_PrimaryPhone, 50, qA1ScriptObject.PrimaryPhone));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_PrimaryPhoneIsCorrect, 50, qA1ScriptObject.PrimaryPhoneIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_PrimaryPhoneUpdateNote, 500, qA1ScriptObject.PrimaryPhoneUpdateNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_EmailAddress, 50, qA1ScriptObject.EmailAddress));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_EmailIsCorrect, 50, qA1ScriptObject.EmailIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_EmailUpdateNote, 500, qA1ScriptObject.EmailUpdateNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_InstallTicketIsCorrect, 50, qA1ScriptObject.InstallTicketIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_TermAndFeeIsCorrect, 50, qA1ScriptObject.TermAndFeeIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsCurrentlyInContract, 50, qA1ScriptObject.IsCurrentlyInContract));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_CurrentlyContractText, 100, qA1ScriptObject.CurrentlyContractText));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsUnderstandAffiliateAndService, 50, qA1ScriptObject.IsUnderstandAffiliateAndService));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_UnderstandText, 100, qA1ScriptObject.UnderstandText));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsBrinksOrMonitronic, 50, qA1ScriptObject.IsBrinksOrMonitronic));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_BrinksOrMonitronicText, 100, qA1ScriptObject.BrinksOrMonitronicText));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_BrinksHowLong, 50, qA1ScriptObject.BrinksHowLong));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsPromisedByRep, 50, qA1ScriptObject.IsPromisedByRep));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_PromisedByRepText, 100, qA1ScriptObject.PromisedByRepText));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_WhatWasPromised, 100, qA1ScriptObject.WhatWasPromised));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsElectricityAvailable, 50, qA1ScriptObject.IsElectricityAvailable));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsCorrectlyWWI, 50, qA1ScriptObject.IsCorrectlyWWI));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsCorrectlyWWINoText, 50, qA1ScriptObject.IsCorrectlyWWINoText));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsCorrectlyWWIYesText, 50, qA1ScriptObject.IsCorrectlyWWIYesText));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_TicketScheduleTimeIsGood, 50, qA1ScriptObject.TicketScheduleTimeIsGood));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_DateOfBirthIsCorrect, 50, qA1ScriptObject.DateOfBirthIsCorrect));
			AddParameter(cmd, pDateTime(QA1ScriptBase.Property_DateofBirth, qA1ScriptObject.DateofBirth));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_DateOfBirthUpdateNote, 500, qA1ScriptObject.DateOfBirthUpdateNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsGenerallyResponsible, 50, qA1ScriptObject.IsGenerallyResponsible));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_Passcode, 50, qA1ScriptObject.Passcode));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_CreatedBy, 50, qA1ScriptObject.CreatedBy));
			AddParameter(cmd, pGuid(QA1ScriptBase.Property_CreatedByUid, qA1ScriptObject.CreatedByUid));
			AddParameter(cmd, pDateTime(QA1ScriptBase.Property_CreatedDate, qA1ScriptObject.CreatedDate));
			AddParameter(cmd, pGuid(QA1ScriptBase.Property_LastUpdatedByUid, qA1ScriptObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(QA1ScriptBase.Property_LastUpdatedDate, qA1ScriptObject.LastUpdatedDate));
			AddParameter(cmd, pBool(QA1ScriptBase.Property_IsCompleted, qA1ScriptObject.IsCompleted));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsSignShowingBusinessYes, 50, qA1ScriptObject.IsSignShowingBusinessYes));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_ManualNote, qA1ScriptObject.ManualNote));
			AddParameter(cmd, pGuid(QA1ScriptBase.Property_QARep, qA1ScriptObject.QARep));
			AddParameter(cmd, pGuid(QA1ScriptBase.Property_SalesRep, qA1ScriptObject.SalesRep));
			AddParameter(cmd, pDateTime(QA1ScriptBase.Property_InstallDate, qA1ScriptObject.InstallDate));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_HowQA1Initiated, 50, qA1ScriptObject.HowQA1Initiated));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsCallingToday, 50, qA1ScriptObject.IsCallingToday));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_SSNIsCorrect, 50, qA1ScriptObject.SSNIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_SSN, 50, qA1ScriptObject.SSN));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_SSNUpdateNote, 200, qA1ScriptObject.SSNUpdateNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_PermitIsRequired, 50, qA1ScriptObject.PermitIsRequired));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_BillingIsCorrect, 50, qA1ScriptObject.BillingIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_PermissionGoAhead, 50, qA1ScriptObject.PermissionGoAhead));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_PermissionGoAheadNote, 200, qA1ScriptObject.PermissionGoAheadNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_SoundIsCorrect, 50, qA1ScriptObject.SoundIsCorrect));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_SoundCorrectNote, 200, qA1ScriptObject.SoundCorrectNote));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsAssistYou, 50, qA1ScriptObject.IsAssistYou));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_IsPass, 50, qA1ScriptObject.IsPass));
			AddParameter(cmd, pNVarChar(QA1ScriptBase.Property_PassNote, 200, qA1ScriptObject.PassNote));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts QA1Script
        /// </summary>
        /// <param name="qA1ScriptObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(QA1ScriptBase qA1ScriptObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTQA1SCRIPT);
	
				AddParameter(cmd, pInt32Out(QA1ScriptBase.Property_Id));
				AddCommonParams(cmd, qA1ScriptObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					qA1ScriptObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					qA1ScriptObject.Id = (Int32)GetOutParameter(cmd, QA1ScriptBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(qA1ScriptObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates QA1Script
        /// </summary>
        /// <param name="qA1ScriptObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(QA1ScriptBase qA1ScriptObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEQA1SCRIPT);
				
				AddParameter(cmd, pInt32(QA1ScriptBase.Property_Id, qA1ScriptObject.Id));
				AddCommonParams(cmd, qA1ScriptObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					qA1ScriptObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(qA1ScriptObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes QA1Script
        /// </summary>
        /// <param name="Id">Id of the QA1Script object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEQA1SCRIPT);	
				
				AddParameter(cmd, pInt32(QA1ScriptBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(QA1Script), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves QA1Script object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the QA1Script object to retrieve</param>
        /// <returns>QA1Script object, null if not found</returns>
		public QA1Script Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETQA1SCRIPTBYID))
			{
				AddParameter( cmd, pInt32(QA1ScriptBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all QA1Script objects 
        /// </summary>
        /// <returns>A list of QA1Script objects</returns>
		public QA1ScriptList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLQA1SCRIPT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all QA1Script objects by PageRequest
        /// </summary>
        /// <returns>A list of QA1Script objects</returns>
		public QA1ScriptList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDQA1SCRIPT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				QA1ScriptList _QA1ScriptList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _QA1ScriptList;
			}
		}
		
		/// <summary>
        /// Retrieves all QA1Script objects by query String
        /// </summary>
        /// <returns>A list of QA1Script objects</returns>
		public QA1ScriptList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETQA1SCRIPTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get QA1Script Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of QA1Script
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQA1SCRIPTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get QA1Script Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of QA1Script
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _QA1ScriptRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQA1SCRIPTROWCOUNT))
			{
				SqlDataReader reader;
				_QA1ScriptRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _QA1ScriptRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills QA1Script object
        /// </summary>
        /// <param name="qA1ScriptObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(QA1ScriptBase qA1ScriptObject, SqlDataReader reader, int start)
		{
			
				qA1ScriptObject.Id = reader.GetInt32( start + 0 );			
				qA1ScriptObject.CustomerId = reader.GetGuid( start + 1 );			
				qA1ScriptObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) qA1ScriptObject.FirstName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) qA1ScriptObject.LastName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) qA1ScriptObject.NameIsCorrect = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) qA1ScriptObject.NameUpdateNote = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) qA1ScriptObject.Street = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) qA1ScriptObject.City = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) qA1ScriptObject.State = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) qA1ScriptObject.ZipCode = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) qA1ScriptObject.AddressIsCorrect = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) qA1ScriptObject.AddressUpdateNote = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) qA1ScriptObject.IsLocationHome = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) qA1ScriptObject.IsHomeRent = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) qA1ScriptObject.HomeownerName = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) qA1ScriptObject.HomeownerNameUpdateNote = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) qA1ScriptObject.IsSpeckRep = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) qA1ScriptObject.IsHomeownerAuthorized = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) qA1ScriptObject.IsOnTheDeed = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) qA1ScriptObject.IsResponsibleParty = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) qA1ScriptObject.ResponsiblePartyName = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) qA1ScriptObject.ReponsiblePartyNameUpdateNote = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) qA1ScriptObject.IsSignShowingBusiness = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) qA1ScriptObject.SignShowText = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) qA1ScriptObject.IsOtherDocumentShowing = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) qA1ScriptObject.OtherDocumentShowingText = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) qA1ScriptObject.IsBusinessSecretaryOfState = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) qA1ScriptObject.PrimaryPhone = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) qA1ScriptObject.PrimaryPhoneIsCorrect = reader.GetString( start + 29 );			
				if(!reader.IsDBNull(30)) qA1ScriptObject.PrimaryPhoneUpdateNote = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) qA1ScriptObject.EmailAddress = reader.GetString( start + 31 );			
				if(!reader.IsDBNull(32)) qA1ScriptObject.EmailIsCorrect = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) qA1ScriptObject.EmailUpdateNote = reader.GetString( start + 33 );			
				if(!reader.IsDBNull(34)) qA1ScriptObject.InstallTicketIsCorrect = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) qA1ScriptObject.TermAndFeeIsCorrect = reader.GetString( start + 35 );			
				if(!reader.IsDBNull(36)) qA1ScriptObject.IsCurrentlyInContract = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) qA1ScriptObject.CurrentlyContractText = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) qA1ScriptObject.IsUnderstandAffiliateAndService = reader.GetString( start + 38 );			
				if(!reader.IsDBNull(39)) qA1ScriptObject.UnderstandText = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) qA1ScriptObject.IsBrinksOrMonitronic = reader.GetString( start + 40 );			
				if(!reader.IsDBNull(41)) qA1ScriptObject.BrinksOrMonitronicText = reader.GetString( start + 41 );			
				if(!reader.IsDBNull(42)) qA1ScriptObject.BrinksHowLong = reader.GetString( start + 42 );			
				if(!reader.IsDBNull(43)) qA1ScriptObject.IsPromisedByRep = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) qA1ScriptObject.PromisedByRepText = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) qA1ScriptObject.WhatWasPromised = reader.GetString( start + 45 );			
				if(!reader.IsDBNull(46)) qA1ScriptObject.IsElectricityAvailable = reader.GetString( start + 46 );			
				if(!reader.IsDBNull(47)) qA1ScriptObject.IsCorrectlyWWI = reader.GetString( start + 47 );			
				if(!reader.IsDBNull(48)) qA1ScriptObject.IsCorrectlyWWINoText = reader.GetString( start + 48 );			
				if(!reader.IsDBNull(49)) qA1ScriptObject.IsCorrectlyWWIYesText = reader.GetString( start + 49 );			
				if(!reader.IsDBNull(50)) qA1ScriptObject.TicketScheduleTimeIsGood = reader.GetString( start + 50 );			
				if(!reader.IsDBNull(51)) qA1ScriptObject.DateOfBirthIsCorrect = reader.GetString( start + 51 );			
				if(!reader.IsDBNull(52)) qA1ScriptObject.DateofBirth = reader.GetDateTime( start + 52 );			
				if(!reader.IsDBNull(53)) qA1ScriptObject.DateOfBirthUpdateNote = reader.GetString( start + 53 );			
				if(!reader.IsDBNull(54)) qA1ScriptObject.IsGenerallyResponsible = reader.GetString( start + 54 );			
				if(!reader.IsDBNull(55)) qA1ScriptObject.Passcode = reader.GetString( start + 55 );			
				if(!reader.IsDBNull(56)) qA1ScriptObject.CreatedBy = reader.GetString( start + 56 );			
				qA1ScriptObject.CreatedByUid = reader.GetGuid( start + 57 );			
				qA1ScriptObject.CreatedDate = reader.GetDateTime( start + 58 );			
				qA1ScriptObject.LastUpdatedByUid = reader.GetGuid( start + 59 );			
				qA1ScriptObject.LastUpdatedDate = reader.GetDateTime( start + 60 );			
				qA1ScriptObject.IsCompleted = reader.GetBoolean( start + 61 );			
				if(!reader.IsDBNull(62)) qA1ScriptObject.IsSignShowingBusinessYes = reader.GetString( start + 62 );			
				if(!reader.IsDBNull(63)) qA1ScriptObject.ManualNote = reader.GetString( start + 63 );			
				qA1ScriptObject.QARep = reader.GetGuid( start + 64 );			
				qA1ScriptObject.SalesRep = reader.GetGuid( start + 65 );			
				qA1ScriptObject.InstallDate = reader.GetDateTime( start + 66 );			
				if(!reader.IsDBNull(67)) qA1ScriptObject.HowQA1Initiated = reader.GetString( start + 67 );			
				if(!reader.IsDBNull(68)) qA1ScriptObject.IsCallingToday = reader.GetString( start + 68 );			
				if(!reader.IsDBNull(69)) qA1ScriptObject.SSNIsCorrect = reader.GetString( start + 69 );			
				if(!reader.IsDBNull(70)) qA1ScriptObject.SSN = reader.GetString( start + 70 );			
				if(!reader.IsDBNull(71)) qA1ScriptObject.SSNUpdateNote = reader.GetString( start + 71 );			
				if(!reader.IsDBNull(72)) qA1ScriptObject.PermitIsRequired = reader.GetString( start + 72 );			
				if(!reader.IsDBNull(73)) qA1ScriptObject.BillingIsCorrect = reader.GetString( start + 73 );			
				if(!reader.IsDBNull(74)) qA1ScriptObject.PermissionGoAhead = reader.GetString( start + 74 );			
				if(!reader.IsDBNull(75)) qA1ScriptObject.PermissionGoAheadNote = reader.GetString( start + 75 );			
				if(!reader.IsDBNull(76)) qA1ScriptObject.SoundIsCorrect = reader.GetString( start + 76 );			
				if(!reader.IsDBNull(77)) qA1ScriptObject.SoundCorrectNote = reader.GetString( start + 77 );			
				if(!reader.IsDBNull(78)) qA1ScriptObject.IsAssistYou = reader.GetString( start + 78 );			
				if(!reader.IsDBNull(79)) qA1ScriptObject.IsPass = reader.GetString( start + 79 );			
				if(!reader.IsDBNull(80)) qA1ScriptObject.PassNote = reader.GetString( start + 80 );			
			FillBaseObject(qA1ScriptObject, reader, (start + 81));

			
			qA1ScriptObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills QA1Script object
        /// </summary>
        /// <param name="qA1ScriptObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(QA1ScriptBase qA1ScriptObject, SqlDataReader reader)
		{
			FillObject(qA1ScriptObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves QA1Script object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>QA1Script object</returns>
		private QA1Script GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					QA1Script qA1ScriptObject= new QA1Script();
					FillObject(qA1ScriptObject, reader);
					return qA1ScriptObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of QA1Script objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of QA1Script objects</returns>
		private QA1ScriptList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//QA1Script list
			QA1ScriptList list = new QA1ScriptList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					QA1Script qA1ScriptObject = new QA1Script();
					FillObject(qA1ScriptObject, reader);

					list.Add(qA1ScriptObject);
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
