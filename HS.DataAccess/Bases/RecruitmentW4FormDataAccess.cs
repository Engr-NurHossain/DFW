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
	public partial class RecruitmentW4FormDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRECRUITMENTW4FORM = "InsertRecruitmentW4Form";
		private const string UPDATERECRUITMENTW4FORM = "UpdateRecruitmentW4Form";
		private const string DELETERECRUITMENTW4FORM = "DeleteRecruitmentW4Form";
		private const string GETRECRUITMENTW4FORMBYID = "GetRecruitmentW4FormById";
		private const string GETALLRECRUITMENTW4FORM = "GetAllRecruitmentW4Form";
		private const string GETPAGEDRECRUITMENTW4FORM = "GetPagedRecruitmentW4Form";
		private const string GETRECRUITMENTW4FORMMAXIMUMID = "GetRecruitmentW4FormMaximumId";
		private const string GETRECRUITMENTW4FORMROWCOUNT = "GetRecruitmentW4FormRowCount";	
		private const string GETRECRUITMENTW4FORMBYQUERY = "GetRecruitmentW4FormByQuery";
		#endregion
		
		#region Constructors
		public RecruitmentW4FormDataAccess(ClientContext context) : base(context) { }
		public RecruitmentW4FormDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="recruitmentW4FormObject"></param>
		private void AddCommonParams(SqlCommand cmd, RecruitmentW4FormBase recruitmentW4FormObject)
		{	
			AddParameter(cmd, pGuid(RecruitmentW4FormBase.Property_FormId, recruitmentW4FormObject.FormId));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AllowanceWorksheetA, 50, recruitmentW4FormObject.AllowanceWorksheetA));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AllowanceWorksheetB, 50, recruitmentW4FormObject.AllowanceWorksheetB));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AllowanceWorksheetC, 50, recruitmentW4FormObject.AllowanceWorksheetC));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AllowanceWorksheetD, 50, recruitmentW4FormObject.AllowanceWorksheetD));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AllowanceWorksheetE, 50, recruitmentW4FormObject.AllowanceWorksheetE));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AllowanceWorksheetF, 50, recruitmentW4FormObject.AllowanceWorksheetF));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AllowanceWorksheetG, 50, recruitmentW4FormObject.AllowanceWorksheetG));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AllowanceWorksheetH, 50, recruitmentW4FormObject.AllowanceWorksheetH));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_FirstName, 50, recruitmentW4FormObject.FirstName));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_MiddleInitial, 50, recruitmentW4FormObject.MiddleInitial));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_LastName, 50, recruitmentW4FormObject.LastName));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_SSN, 50, recruitmentW4FormObject.SSN));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_Address, 250, recruitmentW4FormObject.Address));
			AddParameter(cmd, pBool(RecruitmentW4FormBase.Property_Single, recruitmentW4FormObject.Single));
			AddParameter(cmd, pBool(RecruitmentW4FormBase.Property_Married, recruitmentW4FormObject.Married));
			AddParameter(cmd, pBool(RecruitmentW4FormBase.Property_MarriadButSeparated, recruitmentW4FormObject.MarriadButSeparated));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_City, 50, recruitmentW4FormObject.City));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_State, 50, recruitmentW4FormObject.State));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_Zipcode, 50, recruitmentW4FormObject.Zipcode));
			AddParameter(cmd, pBool(RecruitmentW4FormBase.Property_ReplaceSSNCard4, recruitmentW4FormObject.ReplaceSSNCard4));
			AddParameter(cmd, pInt32(RecruitmentW4FormBase.Property_TotalAllowance5, recruitmentW4FormObject.TotalAllowance5));
			AddParameter(cmd, pDouble(RecruitmentW4FormBase.Property_AdditionalAmount6, recruitmentW4FormObject.AdditionalAmount6));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_NoTaxLiability7, 50, recruitmentW4FormObject.NoTaxLiability7));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_Signature, 550, recruitmentW4FormObject.Signature));
			AddParameter(cmd, pDateTime(RecruitmentW4FormBase.Property_SingatureDate, recruitmentW4FormObject.SingatureDate));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_EmployernameAndAddress, 500, recruitmentW4FormObject.EmployernameAndAddress));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_OfficeCode, 50, recruitmentW4FormObject.OfficeCode));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_EmployerEIN, 50, recruitmentW4FormObject.EmployerEIN));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet1, 50, recruitmentW4FormObject.AdjustWorkSheet1));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet2, 50, recruitmentW4FormObject.AdjustWorkSheet2));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet3, 50, recruitmentW4FormObject.AdjustWorkSheet3));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet4, 50, recruitmentW4FormObject.AdjustWorkSheet4));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet5, 50, recruitmentW4FormObject.AdjustWorkSheet5));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet6, 50, recruitmentW4FormObject.AdjustWorkSheet6));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet7, 50, recruitmentW4FormObject.AdjustWorkSheet7));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet8, 50, recruitmentW4FormObject.AdjustWorkSheet8));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet9, 50, recruitmentW4FormObject.AdjustWorkSheet9));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_AdjustWorkSheet10, 50, recruitmentW4FormObject.AdjustWorkSheet10));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet1, 50, recruitmentW4FormObject.JobWroksheet1));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet2, 50, recruitmentW4FormObject.JobWroksheet2));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet3, 50, recruitmentW4FormObject.JobWroksheet3));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet4, 50, recruitmentW4FormObject.JobWroksheet4));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet5, 50, recruitmentW4FormObject.JobWroksheet5));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet6, 50, recruitmentW4FormObject.JobWroksheet6));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet7, 50, recruitmentW4FormObject.JobWroksheet7));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet8, 50, recruitmentW4FormObject.JobWroksheet8));
			AddParameter(cmd, pNVarChar(RecruitmentW4FormBase.Property_JobWroksheet9, 50, recruitmentW4FormObject.JobWroksheet9));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RecruitmentW4Form
        /// </summary>
        /// <param name="recruitmentW4FormObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RecruitmentW4FormBase recruitmentW4FormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRECRUITMENTW4FORM);
	
				AddParameter(cmd, pInt32Out(RecruitmentW4FormBase.Property_Id));
				AddCommonParams(cmd, recruitmentW4FormObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					recruitmentW4FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					recruitmentW4FormObject.Id = (Int32)GetOutParameter(cmd, RecruitmentW4FormBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(recruitmentW4FormObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RecruitmentW4Form
        /// </summary>
        /// <param name="recruitmentW4FormObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RecruitmentW4FormBase recruitmentW4FormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERECRUITMENTW4FORM);
				
				AddParameter(cmd, pInt32(RecruitmentW4FormBase.Property_Id, recruitmentW4FormObject.Id));
				AddCommonParams(cmd, recruitmentW4FormObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					recruitmentW4FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(recruitmentW4FormObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RecruitmentW4Form
        /// </summary>
        /// <param name="Id">Id of the RecruitmentW4Form object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERECRUITMENTW4FORM);	
				
				AddParameter(cmd, pInt32(RecruitmentW4FormBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RecruitmentW4Form), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RecruitmentW4Form object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RecruitmentW4Form object to retrieve</param>
        /// <returns>RecruitmentW4Form object, null if not found</returns>
		public RecruitmentW4Form Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTW4FORMBYID))
			{
				AddParameter( cmd, pInt32(RecruitmentW4FormBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RecruitmentW4Form objects 
        /// </summary>
        /// <returns>A list of RecruitmentW4Form objects</returns>
		public RecruitmentW4FormList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRECRUITMENTW4FORM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RecruitmentW4Form objects by PageRequest
        /// </summary>
        /// <returns>A list of RecruitmentW4Form objects</returns>
		public RecruitmentW4FormList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRECRUITMENTW4FORM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RecruitmentW4FormList _RecruitmentW4FormList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RecruitmentW4FormList;
			}
		}
		
		/// <summary>
        /// Retrieves all RecruitmentW4Form objects by query String
        /// </summary>
        /// <returns>A list of RecruitmentW4Form objects</returns>
		public RecruitmentW4FormList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTW4FORMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RecruitmentW4Form Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RecruitmentW4Form
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTW4FORMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RecruitmentW4Form Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RecruitmentW4Form
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RecruitmentW4FormRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTW4FORMROWCOUNT))
			{
				SqlDataReader reader;
				_RecruitmentW4FormRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RecruitmentW4FormRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RecruitmentW4Form object
        /// </summary>
        /// <param name="recruitmentW4FormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RecruitmentW4FormBase recruitmentW4FormObject, SqlDataReader reader, int start)
		{
			
				recruitmentW4FormObject.Id = reader.GetInt32( start + 0 );			
				recruitmentW4FormObject.FormId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) recruitmentW4FormObject.AllowanceWorksheetA = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) recruitmentW4FormObject.AllowanceWorksheetB = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) recruitmentW4FormObject.AllowanceWorksheetC = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) recruitmentW4FormObject.AllowanceWorksheetD = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) recruitmentW4FormObject.AllowanceWorksheetE = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) recruitmentW4FormObject.AllowanceWorksheetF = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) recruitmentW4FormObject.AllowanceWorksheetG = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) recruitmentW4FormObject.AllowanceWorksheetH = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) recruitmentW4FormObject.FirstName = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) recruitmentW4FormObject.MiddleInitial = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) recruitmentW4FormObject.LastName = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) recruitmentW4FormObject.SSN = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) recruitmentW4FormObject.Address = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) recruitmentW4FormObject.Single = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) recruitmentW4FormObject.Married = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) recruitmentW4FormObject.MarriadButSeparated = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) recruitmentW4FormObject.City = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) recruitmentW4FormObject.State = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) recruitmentW4FormObject.Zipcode = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) recruitmentW4FormObject.ReplaceSSNCard4 = reader.GetBoolean( start + 21 );			
				if(!reader.IsDBNull(22)) recruitmentW4FormObject.TotalAllowance5 = reader.GetInt32( start + 22 );			
				if(!reader.IsDBNull(23)) recruitmentW4FormObject.AdditionalAmount6 = reader.GetDouble( start + 23 );			
				if(!reader.IsDBNull(24)) recruitmentW4FormObject.NoTaxLiability7 = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) recruitmentW4FormObject.Signature = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) recruitmentW4FormObject.SingatureDate = reader.GetDateTime( start + 26 );			
				if(!reader.IsDBNull(27)) recruitmentW4FormObject.EmployernameAndAddress = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) recruitmentW4FormObject.OfficeCode = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) recruitmentW4FormObject.EmployerEIN = reader.GetString( start + 29 );			
				if(!reader.IsDBNull(30)) recruitmentW4FormObject.AdjustWorkSheet1 = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) recruitmentW4FormObject.AdjustWorkSheet2 = reader.GetString( start + 31 );			
				if(!reader.IsDBNull(32)) recruitmentW4FormObject.AdjustWorkSheet3 = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) recruitmentW4FormObject.AdjustWorkSheet4 = reader.GetString( start + 33 );			
				if(!reader.IsDBNull(34)) recruitmentW4FormObject.AdjustWorkSheet5 = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) recruitmentW4FormObject.AdjustWorkSheet6 = reader.GetString( start + 35 );			
				if(!reader.IsDBNull(36)) recruitmentW4FormObject.AdjustWorkSheet7 = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) recruitmentW4FormObject.AdjustWorkSheet8 = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) recruitmentW4FormObject.AdjustWorkSheet9 = reader.GetString( start + 38 );			
				if(!reader.IsDBNull(39)) recruitmentW4FormObject.AdjustWorkSheet10 = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) recruitmentW4FormObject.JobWroksheet1 = reader.GetString( start + 40 );			
				if(!reader.IsDBNull(41)) recruitmentW4FormObject.JobWroksheet2 = reader.GetString( start + 41 );			
				if(!reader.IsDBNull(42)) recruitmentW4FormObject.JobWroksheet3 = reader.GetString( start + 42 );			
				if(!reader.IsDBNull(43)) recruitmentW4FormObject.JobWroksheet4 = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) recruitmentW4FormObject.JobWroksheet5 = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) recruitmentW4FormObject.JobWroksheet6 = reader.GetString( start + 45 );			
				if(!reader.IsDBNull(46)) recruitmentW4FormObject.JobWroksheet7 = reader.GetString( start + 46 );			
				if(!reader.IsDBNull(47)) recruitmentW4FormObject.JobWroksheet8 = reader.GetString( start + 47 );			
				if(!reader.IsDBNull(48)) recruitmentW4FormObject.JobWroksheet9 = reader.GetString( start + 48 );			
			FillBaseObject(recruitmentW4FormObject, reader, (start + 49));

			
			recruitmentW4FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RecruitmentW4Form object
        /// </summary>
        /// <param name="recruitmentW4FormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RecruitmentW4FormBase recruitmentW4FormObject, SqlDataReader reader)
		{
			FillObject(recruitmentW4FormObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RecruitmentW4Form object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RecruitmentW4Form object</returns>
		private RecruitmentW4Form GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RecruitmentW4Form recruitmentW4FormObject= new RecruitmentW4Form();
					FillObject(recruitmentW4FormObject, reader);
					return recruitmentW4FormObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RecruitmentW4Form objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RecruitmentW4Form objects</returns>
		private RecruitmentW4FormList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RecruitmentW4Form list
			RecruitmentW4FormList list = new RecruitmentW4FormList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RecruitmentW4Form recruitmentW4FormObject = new RecruitmentW4Form();
					FillObject(recruitmentW4FormObject, reader);

					list.Add(recruitmentW4FormObject);
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
