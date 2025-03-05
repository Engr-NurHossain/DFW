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
	public partial class RecruitmentW9FormDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRECRUITMENTW9FORM = "InsertRecruitmentW9Form";
		private const string UPDATERECRUITMENTW9FORM = "UpdateRecruitmentW9Form";
		private const string DELETERECRUITMENTW9FORM = "DeleteRecruitmentW9Form";
		private const string GETRECRUITMENTW9FORMBYID = "GetRecruitmentW9FormById";
		private const string GETALLRECRUITMENTW9FORM = "GetAllRecruitmentW9Form";
		private const string GETPAGEDRECRUITMENTW9FORM = "GetPagedRecruitmentW9Form";
		private const string GETRECRUITMENTW9FORMMAXIMUMID = "GetRecruitmentW9FormMaximumId";
		private const string GETRECRUITMENTW9FORMROWCOUNT = "GetRecruitmentW9FormRowCount";	
		private const string GETRECRUITMENTW9FORMBYQUERY = "GetRecruitmentW9FormByQuery";
		#endregion
		
		#region Constructors
		public RecruitmentW9FormDataAccess(ClientContext context) : base(context) { }
		public RecruitmentW9FormDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="recruitmentW9FormObject"></param>
		private void AddCommonParams(SqlCommand cmd, RecruitmentW9FormBase recruitmentW9FormObject)
		{	
			AddParameter(cmd, pGuid(RecruitmentW9FormBase.Property_FormId, recruitmentW9FormObject.FormId));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_Name, 250, recruitmentW9FormObject.Name));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_BusinessName, 250, recruitmentW9FormObject.BusinessName));
			AddParameter(cmd, pBool(RecruitmentW9FormBase.Property_Individual, recruitmentW9FormObject.Individual));
			AddParameter(cmd, pBool(RecruitmentW9FormBase.Property_CCorporation, recruitmentW9FormObject.CCorporation));
			AddParameter(cmd, pBool(RecruitmentW9FormBase.Property_SCorporation, recruitmentW9FormObject.SCorporation));
			AddParameter(cmd, pBool(RecruitmentW9FormBase.Property_Partnership, recruitmentW9FormObject.Partnership));
			AddParameter(cmd, pBool(RecruitmentW9FormBase.Property_Trust, recruitmentW9FormObject.Trust));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_ExemptPayeeCode, 50, recruitmentW9FormObject.ExemptPayeeCode));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_FATCAReportingCode, 50, recruitmentW9FormObject.FATCAReportingCode));
			AddParameter(cmd, pBool(RecruitmentW9FormBase.Property_LimitedLiability, recruitmentW9FormObject.LimitedLiability));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_TaxClassification, 50, recruitmentW9FormObject.TaxClassification));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_Other, 250, recruitmentW9FormObject.Other));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_Address, 250, recruitmentW9FormObject.Address));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_City, 50, recruitmentW9FormObject.City));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_State, 50, recruitmentW9FormObject.State));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_Zipcode, 50, recruitmentW9FormObject.Zipcode));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_AccountNumber, 250, recruitmentW9FormObject.AccountNumber));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_SSN, 50, recruitmentW9FormObject.SSN));
			AddParameter(cmd, pNVarChar(RecruitmentW9FormBase.Property_EIN, 50, recruitmentW9FormObject.EIN));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RecruitmentW9Form
        /// </summary>
        /// <param name="recruitmentW9FormObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RecruitmentW9FormBase recruitmentW9FormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRECRUITMENTW9FORM);
	
				AddParameter(cmd, pInt32(RecruitmentW9FormBase.Property_Id, recruitmentW9FormObject.Id));
				AddCommonParams(cmd, recruitmentW9FormObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					recruitmentW9FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(recruitmentW9FormObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RecruitmentW9Form
        /// </summary>
        /// <param name="recruitmentW9FormObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RecruitmentW9FormBase recruitmentW9FormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERECRUITMENTW9FORM);
				
				AddParameter(cmd, pInt32(RecruitmentW9FormBase.Property_Id, recruitmentW9FormObject.Id));
				AddCommonParams(cmd, recruitmentW9FormObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					recruitmentW9FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(recruitmentW9FormObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RecruitmentW9Form
        /// </summary>
        /// <param name="Id">Id of the RecruitmentW9Form object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERECRUITMENTW9FORM);	
				
				AddParameter(cmd, pInt32(RecruitmentW9FormBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RecruitmentW9Form), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RecruitmentW9Form object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RecruitmentW9Form object to retrieve</param>
        /// <returns>RecruitmentW9Form object, null if not found</returns>
		public RecruitmentW9Form Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTW9FORMBYID))
			{
				AddParameter( cmd, pInt32(RecruitmentW9FormBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RecruitmentW9Form objects 
        /// </summary>
        /// <returns>A list of RecruitmentW9Form objects</returns>
		public RecruitmentW9FormList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRECRUITMENTW9FORM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RecruitmentW9Form objects by PageRequest
        /// </summary>
        /// <returns>A list of RecruitmentW9Form objects</returns>
		public RecruitmentW9FormList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRECRUITMENTW9FORM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RecruitmentW9FormList _RecruitmentW9FormList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RecruitmentW9FormList;
			}
		}
		
		/// <summary>
        /// Retrieves all RecruitmentW9Form objects by query String
        /// </summary>
        /// <returns>A list of RecruitmentW9Form objects</returns>
		public RecruitmentW9FormList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTW9FORMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RecruitmentW9Form Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RecruitmentW9Form
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTW9FORMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RecruitmentW9Form Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RecruitmentW9Form
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RecruitmentW9FormRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTW9FORMROWCOUNT))
			{
				SqlDataReader reader;
				_RecruitmentW9FormRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RecruitmentW9FormRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RecruitmentW9Form object
        /// </summary>
        /// <param name="recruitmentW9FormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RecruitmentW9FormBase recruitmentW9FormObject, SqlDataReader reader, int start)
		{
			
				recruitmentW9FormObject.Id = reader.GetInt32( start + 0 );			
				recruitmentW9FormObject.FormId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) recruitmentW9FormObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) recruitmentW9FormObject.BusinessName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) recruitmentW9FormObject.Individual = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) recruitmentW9FormObject.CCorporation = reader.GetBoolean( start + 5 );			
				if(!reader.IsDBNull(6)) recruitmentW9FormObject.SCorporation = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) recruitmentW9FormObject.Partnership = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) recruitmentW9FormObject.Trust = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) recruitmentW9FormObject.ExemptPayeeCode = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) recruitmentW9FormObject.FATCAReportingCode = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) recruitmentW9FormObject.LimitedLiability = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) recruitmentW9FormObject.TaxClassification = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) recruitmentW9FormObject.Other = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) recruitmentW9FormObject.Address = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) recruitmentW9FormObject.City = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) recruitmentW9FormObject.State = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) recruitmentW9FormObject.Zipcode = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) recruitmentW9FormObject.AccountNumber = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) recruitmentW9FormObject.SSN = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) recruitmentW9FormObject.EIN = reader.GetString( start + 20 );			
			FillBaseObject(recruitmentW9FormObject, reader, (start + 21));

			
			recruitmentW9FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RecruitmentW9Form object
        /// </summary>
        /// <param name="recruitmentW9FormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RecruitmentW9FormBase recruitmentW9FormObject, SqlDataReader reader)
		{
			FillObject(recruitmentW9FormObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RecruitmentW9Form object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RecruitmentW9Form object</returns>
		private RecruitmentW9Form GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RecruitmentW9Form recruitmentW9FormObject= new RecruitmentW9Form();
					FillObject(recruitmentW9FormObject, reader);
					return recruitmentW9FormObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RecruitmentW9Form objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RecruitmentW9Form objects</returns>
		private RecruitmentW9FormList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RecruitmentW9Form list
			RecruitmentW9FormList list = new RecruitmentW9FormList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RecruitmentW9Form recruitmentW9FormObject = new RecruitmentW9Form();
					FillObject(recruitmentW9FormObject, reader);

					list.Add(recruitmentW9FormObject);
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
