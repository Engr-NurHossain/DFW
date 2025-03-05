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
	public partial class EmployeeReviewDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEREVIEW = "InsertEmployeeReview";
		private const string UPDATEEMPLOYEEREVIEW = "UpdateEmployeeReview";
		private const string DELETEEMPLOYEEREVIEW = "DeleteEmployeeReview";
		private const string GETEMPLOYEEREVIEWBYID = "GetEmployeeReviewById";
		private const string GETALLEMPLOYEEREVIEW = "GetAllEmployeeReview";
		private const string GETPAGEDEMPLOYEEREVIEW = "GetPagedEmployeeReview";
		private const string GETEMPLOYEEREVIEWMAXIMUMID = "GetEmployeeReviewMaximumId";
		private const string GETEMPLOYEEREVIEWROWCOUNT = "GetEmployeeReviewRowCount";	
		private const string GETEMPLOYEEREVIEWBYQUERY = "GetEmployeeReviewByQuery";
		#endregion
		
		#region Constructors
		public EmployeeReviewDataAccess(ClientContext context) : base(context) { }
		public EmployeeReviewDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeReviewObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeReviewBase employeeReviewObject)
		{	
			AddParameter(cmd, pGuid(EmployeeReviewBase.Property_ReviewId, employeeReviewObject.ReviewId));
			AddParameter(cmd, pGuid(EmployeeReviewBase.Property_CompanyId, employeeReviewObject.CompanyId));
			AddParameter(cmd, pGuid(EmployeeReviewBase.Property_UserId, employeeReviewObject.UserId));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_EmpName, 150, employeeReviewObject.EmpName));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_JobTitle, 150, employeeReviewObject.JobTitle));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_Department, 100, employeeReviewObject.Department));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_ReviewPeriod, 500, employeeReviewObject.ReviewPeriod));
			AddParameter(cmd, pDateTime(EmployeeReviewBase.Property_ReviewDate, employeeReviewObject.ReviewDate));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_Manager, 150, employeeReviewObject.Manager));
			AddParameter(cmd, pDateTime(EmployeeReviewBase.Property_NextReview, employeeReviewObject.NextReview));
			AddParameter(cmd, pInt32(EmployeeReviewBase.Property_JobKnowledge, employeeReviewObject.JobKnowledge));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_JobKnowledgeComments, 500, employeeReviewObject.JobKnowledgeComments));
			AddParameter(cmd, pInt32(EmployeeReviewBase.Property_WorkQuality, employeeReviewObject.WorkQuality));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_WorkQualityComments, 500, employeeReviewObject.WorkQualityComments));
			AddParameter(cmd, pInt32(EmployeeReviewBase.Property_Attendance, employeeReviewObject.Attendance));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_AttendanceComments, 500, employeeReviewObject.AttendanceComments));
			AddParameter(cmd, pInt32(EmployeeReviewBase.Property_Initiative, employeeReviewObject.Initiative));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_InitiativeComments, 500, employeeReviewObject.InitiativeComments));
			AddParameter(cmd, pInt32(EmployeeReviewBase.Property_CommunicationSkills, employeeReviewObject.CommunicationSkills));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_CommunicationSkillsComments, 500, employeeReviewObject.CommunicationSkillsComments));
			AddParameter(cmd, pInt32(EmployeeReviewBase.Property_Dependability, employeeReviewObject.Dependability));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_DependabilityComments, 500, employeeReviewObject.DependabilityComments));
			AddParameter(cmd, pInt32(EmployeeReviewBase.Property_OverallRating, employeeReviewObject.OverallRating));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_AdditionalComments, employeeReviewObject.AdditionalComments));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_Goals, employeeReviewObject.Goals));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_EmpSignature, 250, employeeReviewObject.EmpSignature));
			AddParameter(cmd, pDateTime(EmployeeReviewBase.Property_EmpSignatureDate, employeeReviewObject.EmpSignatureDate));
			AddParameter(cmd, pNVarChar(EmployeeReviewBase.Property_ManagerSignature, 250, employeeReviewObject.ManagerSignature));
			AddParameter(cmd, pDateTime(EmployeeReviewBase.Property_ManagerSignatureDate, employeeReviewObject.ManagerSignatureDate));
			AddParameter(cmd, pGuid(EmployeeReviewBase.Property_CreatedBy, employeeReviewObject.CreatedBy));
			AddParameter(cmd, pDateTime(EmployeeReviewBase.Property_CreatedDate, employeeReviewObject.CreatedDate));
			AddParameter(cmd, pGuid(EmployeeReviewBase.Property_ReviewedBy, employeeReviewObject.ReviewedBy));
			AddParameter(cmd, pDateTime(EmployeeReviewBase.Property_ReviewedDate, employeeReviewObject.ReviewedDate));
			AddParameter(cmd, pGuid(EmployeeReviewBase.Property_LastUpdatedBy, employeeReviewObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(EmployeeReviewBase.Property_LastUpdatedDate, employeeReviewObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeReview
        /// </summary>
        /// <param name="employeeReviewObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeReviewBase employeeReviewObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEREVIEW);
	
				AddParameter(cmd, pInt32Out(EmployeeReviewBase.Property_Id));
				AddCommonParams(cmd, employeeReviewObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeReviewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeReviewObject.Id = (Int32)GetOutParameter(cmd, EmployeeReviewBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeReviewObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeReview
        /// </summary>
        /// <param name="employeeReviewObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeReviewBase employeeReviewObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEREVIEW);
				
				AddParameter(cmd, pInt32(EmployeeReviewBase.Property_Id, employeeReviewObject.Id));
				AddCommonParams(cmd, employeeReviewObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeReviewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeReviewObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeReview
        /// </summary>
        /// <param name="Id">Id of the EmployeeReview object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEREVIEW);	
				
				AddParameter(cmd, pInt32(EmployeeReviewBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeReview), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeReview object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeReview object to retrieve</param>
        /// <returns>EmployeeReview object, null if not found</returns>
		public EmployeeReview Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEREVIEWBYID))
			{
				AddParameter( cmd, pInt32(EmployeeReviewBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeReview objects 
        /// </summary>
        /// <returns>A list of EmployeeReview objects</returns>
		public EmployeeReviewList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEREVIEW))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeReview objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeReview objects</returns>
		public EmployeeReviewList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEREVIEW))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeReviewList _EmployeeReviewList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeReviewList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeReview objects by query String
        /// </summary>
        /// <returns>A list of EmployeeReview objects</returns>
		public EmployeeReviewList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEREVIEWBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeReview Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeReview
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEREVIEWMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeReview Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeReview
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeReviewRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEREVIEWROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeReviewRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeReviewRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeReview object
        /// </summary>
        /// <param name="employeeReviewObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeReviewBase employeeReviewObject, SqlDataReader reader, int start)
		{
			
				employeeReviewObject.Id = reader.GetInt32( start + 0 );			
				employeeReviewObject.ReviewId = reader.GetGuid( start + 1 );			
				employeeReviewObject.CompanyId = reader.GetGuid( start + 2 );			
				employeeReviewObject.UserId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) employeeReviewObject.EmpName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) employeeReviewObject.JobTitle = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) employeeReviewObject.Department = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) employeeReviewObject.ReviewPeriod = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) employeeReviewObject.ReviewDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) employeeReviewObject.Manager = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) employeeReviewObject.NextReview = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) employeeReviewObject.JobKnowledge = reader.GetInt32( start + 11 );			
				if(!reader.IsDBNull(12)) employeeReviewObject.JobKnowledgeComments = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) employeeReviewObject.WorkQuality = reader.GetInt32( start + 13 );			
				if(!reader.IsDBNull(14)) employeeReviewObject.WorkQualityComments = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) employeeReviewObject.Attendance = reader.GetInt32( start + 15 );			
				if(!reader.IsDBNull(16)) employeeReviewObject.AttendanceComments = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) employeeReviewObject.Initiative = reader.GetInt32( start + 17 );			
				if(!reader.IsDBNull(18)) employeeReviewObject.InitiativeComments = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) employeeReviewObject.CommunicationSkills = reader.GetInt32( start + 19 );			
				if(!reader.IsDBNull(20)) employeeReviewObject.CommunicationSkillsComments = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) employeeReviewObject.Dependability = reader.GetInt32( start + 21 );			
				if(!reader.IsDBNull(22)) employeeReviewObject.DependabilityComments = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) employeeReviewObject.OverallRating = reader.GetInt32( start + 23 );			
				if(!reader.IsDBNull(24)) employeeReviewObject.AdditionalComments = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) employeeReviewObject.Goals = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) employeeReviewObject.EmpSignature = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) employeeReviewObject.EmpSignatureDate = reader.GetDateTime( start + 27 );			
				if(!reader.IsDBNull(28)) employeeReviewObject.ManagerSignature = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) employeeReviewObject.ManagerSignatureDate = reader.GetDateTime( start + 29 );			
				employeeReviewObject.CreatedBy = reader.GetGuid( start + 30 );			
				if(!reader.IsDBNull(31)) employeeReviewObject.CreatedDate = reader.GetDateTime( start + 31 );			
				employeeReviewObject.ReviewedBy = reader.GetGuid( start + 32 );			
				if(!reader.IsDBNull(33)) employeeReviewObject.ReviewedDate = reader.GetDateTime( start + 33 );			
				employeeReviewObject.LastUpdatedBy = reader.GetGuid( start + 34 );			
				if(!reader.IsDBNull(35)) employeeReviewObject.LastUpdatedDate = reader.GetDateTime( start + 35 );			
			FillBaseObject(employeeReviewObject, reader, (start + 36));

			
			employeeReviewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeReview object
        /// </summary>
        /// <param name="employeeReviewObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeReviewBase employeeReviewObject, SqlDataReader reader)
		{
			FillObject(employeeReviewObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeReview object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeReview object</returns>
		private EmployeeReview GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeReview employeeReviewObject= new EmployeeReview();
					FillObject(employeeReviewObject, reader);
					return employeeReviewObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeReview objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeReview objects</returns>
		private EmployeeReviewList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeReview list
			EmployeeReviewList list = new EmployeeReviewList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeReview employeeReviewObject = new EmployeeReview();
					FillObject(employeeReviewObject, reader);

					list.Add(employeeReviewObject);
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
