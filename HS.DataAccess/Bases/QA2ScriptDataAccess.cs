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
	public partial class QA2ScriptDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTQA2SCRIPT = "InsertQA2Script";
		private const string UPDATEQA2SCRIPT = "UpdateQA2Script";
		private const string DELETEQA2SCRIPT = "DeleteQA2Script";
		private const string GETQA2SCRIPTBYID = "GetQA2ScriptById";
		private const string GETALLQA2SCRIPT = "GetAllQA2Script";
		private const string GETPAGEDQA2SCRIPT = "GetPagedQA2Script";
		private const string GETQA2SCRIPTMAXIMUMID = "GetQA2ScriptMaximumId";
		private const string GETQA2SCRIPTROWCOUNT = "GetQA2ScriptRowCount";	
		private const string GETQA2SCRIPTBYQUERY = "GetQA2ScriptByQuery";
		#endregion
		
		#region Constructors
		public QA2ScriptDataAccess(ClientContext context) : base(context) { }
		public QA2ScriptDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="qA2ScriptObject"></param>
		private void AddCommonParams(SqlCommand cmd, QA2ScriptBase qA2ScriptObject)
		{	
			AddParameter(cmd, pGuid(QA2ScriptBase.Property_CustomerId, qA2ScriptObject.CustomerId));
			AddParameter(cmd, pGuid(QA2ScriptBase.Property_CompanyId, qA2ScriptObject.CompanyId));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_Street, 500, qA2ScriptObject.Street));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_City, 50, qA2ScriptObject.City));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_State, 50, qA2ScriptObject.State));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_ZipCode, 50, qA2ScriptObject.ZipCode));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_AddressIsCorrect, 50, qA2ScriptObject.AddressIsCorrect));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_AddressUpdateNote, 500, qA2ScriptObject.AddressUpdateNote));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_PrimaryPhone, 50, qA2ScriptObject.PrimaryPhone));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_PrimaryPhoneIsCorrect, 50, qA2ScriptObject.PrimaryPhoneIsCorrect));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_PrimaryPhoneUpdateNote, 500, qA2ScriptObject.PrimaryPhoneUpdateNote));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_Passcode, 50, qA2ScriptObject.Passcode));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_PasscodeIsCorrect, 50, qA2ScriptObject.PasscodeIsCorrect));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_PassCodeUpdateNote, 500, qA2ScriptObject.PassCodeUpdateNote));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_ContractTerm, 50, qA2ScriptObject.ContractTerm));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_MonitoringFee, 50, qA2ScriptObject.MonitoringFee));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_TermAndFeeIsCorrect, 50, qA2ScriptObject.TermAndFeeIsCorrect));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_TermAndFeeUpdateNote, 500, qA2ScriptObject.TermAndFeeUpdateNote));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_IsReceiveCallOrText, 50, qA2ScriptObject.IsReceiveCallOrText));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_IsInstallThatPromised, 50, qA2ScriptObject.IsInstallThatPromised));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_IsShowSystem, 50, qA2ScriptObject.IsShowSystem));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_IsCleanUpAfterInstallation, 50, qA2ScriptObject.IsCleanUpAfterInstallation));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_ExperienceRate, 50, qA2ScriptObject.ExperienceRate));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_IsCompletelySatisfied, 50, qA2ScriptObject.IsCompletelySatisfied));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_DiscussionIsOkay, 50, qA2ScriptObject.DiscussionIsOkay));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_CreatedBy, 50, qA2ScriptObject.CreatedBy));
			AddParameter(cmd, pGuid(QA2ScriptBase.Property_CreatedByUid, qA2ScriptObject.CreatedByUid));
			AddParameter(cmd, pDateTime(QA2ScriptBase.Property_CreatedDate, qA2ScriptObject.CreatedDate));
			AddParameter(cmd, pGuid(QA2ScriptBase.Property_LastUpdatedByUid, qA2ScriptObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(QA2ScriptBase.Property_LastUpdatedDate, qA2ScriptObject.LastUpdatedDate));
			AddParameter(cmd, pBool(QA2ScriptBase.Property_IsCompleted, qA2ScriptObject.IsCompleted));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_ManualNote, qA2ScriptObject.ManualNote));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_FinanceCompletelySatisfied, 50, qA2ScriptObject.FinanceCompletelySatisfied));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_GallantFew, 50, qA2ScriptObject.GallantFew));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_TextLinkForTremendously, 50, qA2ScriptObject.TextLinkForTremendously));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_AccountOnline, 50, qA2ScriptObject.AccountOnline));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_AgreementSigned, 50, qA2ScriptObject.AgreementSigned));
			AddParameter(cmd, pNVarChar(QA2ScriptBase.Property_FirstMonthSetup, 50, qA2ScriptObject.FirstMonthSetup));
			AddParameter(cmd, pGuid(QA2ScriptBase.Property_CompletedBy, qA2ScriptObject.CompletedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts QA2Script
        /// </summary>
        /// <param name="qA2ScriptObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(QA2ScriptBase qA2ScriptObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTQA2SCRIPT);
	
				AddParameter(cmd, pInt32Out(QA2ScriptBase.Property_Id));
				AddCommonParams(cmd, qA2ScriptObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					qA2ScriptObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					qA2ScriptObject.Id = (Int32)GetOutParameter(cmd, QA2ScriptBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(qA2ScriptObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates QA2Script
        /// </summary>
        /// <param name="qA2ScriptObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(QA2ScriptBase qA2ScriptObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEQA2SCRIPT);
				
				AddParameter(cmd, pInt32(QA2ScriptBase.Property_Id, qA2ScriptObject.Id));
				AddCommonParams(cmd, qA2ScriptObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					qA2ScriptObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(qA2ScriptObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes QA2Script
        /// </summary>
        /// <param name="Id">Id of the QA2Script object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEQA2SCRIPT);	
				
				AddParameter(cmd, pInt32(QA2ScriptBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(QA2Script), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves QA2Script object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the QA2Script object to retrieve</param>
        /// <returns>QA2Script object, null if not found</returns>
		public QA2Script Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETQA2SCRIPTBYID))
			{
				AddParameter( cmd, pInt32(QA2ScriptBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all QA2Script objects 
        /// </summary>
        /// <returns>A list of QA2Script objects</returns>
		public QA2ScriptList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLQA2SCRIPT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all QA2Script objects by PageRequest
        /// </summary>
        /// <returns>A list of QA2Script objects</returns>
		public QA2ScriptList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDQA2SCRIPT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				QA2ScriptList _QA2ScriptList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _QA2ScriptList;
			}
		}
		
		/// <summary>
        /// Retrieves all QA2Script objects by query String
        /// </summary>
        /// <returns>A list of QA2Script objects</returns>
		public QA2ScriptList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETQA2SCRIPTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get QA2Script Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of QA2Script
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQA2SCRIPTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get QA2Script Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of QA2Script
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _QA2ScriptRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQA2SCRIPTROWCOUNT))
			{
				SqlDataReader reader;
				_QA2ScriptRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _QA2ScriptRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills QA2Script object
        /// </summary>
        /// <param name="qA2ScriptObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(QA2ScriptBase qA2ScriptObject, SqlDataReader reader, int start)
		{
			
				qA2ScriptObject.Id = reader.GetInt32( start + 0 );			
				qA2ScriptObject.CustomerId = reader.GetGuid( start + 1 );			
				qA2ScriptObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) qA2ScriptObject.Street = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) qA2ScriptObject.City = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) qA2ScriptObject.State = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) qA2ScriptObject.ZipCode = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) qA2ScriptObject.AddressIsCorrect = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) qA2ScriptObject.AddressUpdateNote = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) qA2ScriptObject.PrimaryPhone = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) qA2ScriptObject.PrimaryPhoneIsCorrect = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) qA2ScriptObject.PrimaryPhoneUpdateNote = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) qA2ScriptObject.Passcode = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) qA2ScriptObject.PasscodeIsCorrect = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) qA2ScriptObject.PassCodeUpdateNote = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) qA2ScriptObject.ContractTerm = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) qA2ScriptObject.MonitoringFee = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) qA2ScriptObject.TermAndFeeIsCorrect = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) qA2ScriptObject.TermAndFeeUpdateNote = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) qA2ScriptObject.IsReceiveCallOrText = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) qA2ScriptObject.IsInstallThatPromised = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) qA2ScriptObject.IsShowSystem = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) qA2ScriptObject.IsCleanUpAfterInstallation = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) qA2ScriptObject.ExperienceRate = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) qA2ScriptObject.IsCompletelySatisfied = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) qA2ScriptObject.DiscussionIsOkay = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) qA2ScriptObject.CreatedBy = reader.GetString( start + 26 );			
				qA2ScriptObject.CreatedByUid = reader.GetGuid( start + 27 );			
				qA2ScriptObject.CreatedDate = reader.GetDateTime( start + 28 );			
				qA2ScriptObject.LastUpdatedByUid = reader.GetGuid( start + 29 );			
				qA2ScriptObject.LastUpdatedDate = reader.GetDateTime( start + 30 );			
				qA2ScriptObject.IsCompleted = reader.GetBoolean( start + 31 );			
				if(!reader.IsDBNull(32)) qA2ScriptObject.ManualNote = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) qA2ScriptObject.FinanceCompletelySatisfied = reader.GetString( start + 33 );			
				if(!reader.IsDBNull(34)) qA2ScriptObject.GallantFew = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) qA2ScriptObject.TextLinkForTremendously = reader.GetString( start + 35 );			
				if(!reader.IsDBNull(36)) qA2ScriptObject.AccountOnline = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) qA2ScriptObject.AgreementSigned = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) qA2ScriptObject.FirstMonthSetup = reader.GetString( start + 38 );			
				qA2ScriptObject.CompletedBy = reader.GetGuid( start + 39 );			
			FillBaseObject(qA2ScriptObject, reader, (start + 40));

			
			qA2ScriptObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills QA2Script object
        /// </summary>
        /// <param name="qA2ScriptObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(QA2ScriptBase qA2ScriptObject, SqlDataReader reader)
		{
			FillObject(qA2ScriptObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves QA2Script object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>QA2Script object</returns>
		private QA2Script GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					QA2Script qA2ScriptObject= new QA2Script();
					FillObject(qA2ScriptObject, reader);
					return qA2ScriptObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of QA2Script objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of QA2Script objects</returns>
		private QA2ScriptList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//QA2Script list
			QA2ScriptList list = new QA2ScriptList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					QA2Script qA2ScriptObject = new QA2Script();
					FillObject(qA2ScriptObject, reader);

					list.Add(qA2ScriptObject);
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
