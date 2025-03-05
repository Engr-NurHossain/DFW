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
	public partial class RecruitmentFormEmployeeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRECRUITMENTFORMEMPLOYEE = "InsertRecruitmentFormEmployee";
		private const string UPDATERECRUITMENTFORMEMPLOYEE = "UpdateRecruitmentFormEmployee";
		private const string DELETERECRUITMENTFORMEMPLOYEE = "DeleteRecruitmentFormEmployee";
		private const string GETRECRUITMENTFORMEMPLOYEEBYID = "GetRecruitmentFormEmployeeById";
		private const string GETALLRECRUITMENTFORMEMPLOYEE = "GetAllRecruitmentFormEmployee";
		private const string GETPAGEDRECRUITMENTFORMEMPLOYEE = "GetPagedRecruitmentFormEmployee";
		private const string GETRECRUITMENTFORMEMPLOYEEMAXIMUMID = "GetRecruitmentFormEmployeeMaximumId";
		private const string GETRECRUITMENTFORMEMPLOYEEROWCOUNT = "GetRecruitmentFormEmployeeRowCount";	
		private const string GETRECRUITMENTFORMEMPLOYEEBYQUERY = "GetRecruitmentFormEmployeeByQuery";
		#endregion
		
		#region Constructors
		public RecruitmentFormEmployeeDataAccess(ClientContext context) : base(context) { }
		public RecruitmentFormEmployeeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="recruitmentFormEmployeeObject"></param>
		private void AddCommonParams(SqlCommand cmd, RecruitmentFormEmployeeBase recruitmentFormEmployeeObject)
		{	
			AddParameter(cmd, pInt32(RecruitmentFormEmployeeBase.Property_RecruitmentFormId, recruitmentFormEmployeeObject.RecruitmentFormId));
			AddParameter(cmd, pGuid(RecruitmentFormEmployeeBase.Property_EmployeeId, recruitmentFormEmployeeObject.EmployeeId));
			AddParameter(cmd, pGuid(RecruitmentFormEmployeeBase.Property_FormId, recruitmentFormEmployeeObject.FormId));
			AddParameter(cmd, pBool(RecruitmentFormEmployeeBase.Property_IsFillUp, recruitmentFormEmployeeObject.IsFillUp));
			AddParameter(cmd, pBool(RecruitmentFormEmployeeBase.Property_IsSubmitted, recruitmentFormEmployeeObject.IsSubmitted));
			AddParameter(cmd, pDateTime(RecruitmentFormEmployeeBase.Property_FillDate, recruitmentFormEmployeeObject.FillDate));
			AddParameter(cmd, pDateTime(RecruitmentFormEmployeeBase.Property_SubmitDate, recruitmentFormEmployeeObject.SubmitDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RecruitmentFormEmployee
        /// </summary>
        /// <param name="recruitmentFormEmployeeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RecruitmentFormEmployeeBase recruitmentFormEmployeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRECRUITMENTFORMEMPLOYEE);
	
				AddParameter(cmd, pInt32Out(RecruitmentFormEmployeeBase.Property_Id));
				AddCommonParams(cmd, recruitmentFormEmployeeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					recruitmentFormEmployeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					recruitmentFormEmployeeObject.Id = (Int32)GetOutParameter(cmd, RecruitmentFormEmployeeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(recruitmentFormEmployeeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RecruitmentFormEmployee
        /// </summary>
        /// <param name="recruitmentFormEmployeeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RecruitmentFormEmployeeBase recruitmentFormEmployeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERECRUITMENTFORMEMPLOYEE);
				
				AddParameter(cmd, pInt32(RecruitmentFormEmployeeBase.Property_Id, recruitmentFormEmployeeObject.Id));
				AddCommonParams(cmd, recruitmentFormEmployeeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					recruitmentFormEmployeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(recruitmentFormEmployeeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RecruitmentFormEmployee
        /// </summary>
        /// <param name="Id">Id of the RecruitmentFormEmployee object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERECRUITMENTFORMEMPLOYEE);	
				
				AddParameter(cmd, pInt32(RecruitmentFormEmployeeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RecruitmentFormEmployee), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RecruitmentFormEmployee object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RecruitmentFormEmployee object to retrieve</param>
        /// <returns>RecruitmentFormEmployee object, null if not found</returns>
		public RecruitmentFormEmployee Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTFORMEMPLOYEEBYID))
			{
				AddParameter( cmd, pInt32(RecruitmentFormEmployeeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RecruitmentFormEmployee objects 
        /// </summary>
        /// <returns>A list of RecruitmentFormEmployee objects</returns>
		public RecruitmentFormEmployeeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRECRUITMENTFORMEMPLOYEE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RecruitmentFormEmployee objects by PageRequest
        /// </summary>
        /// <returns>A list of RecruitmentFormEmployee objects</returns>
		public RecruitmentFormEmployeeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRECRUITMENTFORMEMPLOYEE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RecruitmentFormEmployeeList _RecruitmentFormEmployeeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RecruitmentFormEmployeeList;
			}
		}
		
		/// <summary>
        /// Retrieves all RecruitmentFormEmployee objects by query String
        /// </summary>
        /// <returns>A list of RecruitmentFormEmployee objects</returns>
		public RecruitmentFormEmployeeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTFORMEMPLOYEEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RecruitmentFormEmployee Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RecruitmentFormEmployee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTFORMEMPLOYEEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RecruitmentFormEmployee Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RecruitmentFormEmployee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RecruitmentFormEmployeeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTFORMEMPLOYEEROWCOUNT))
			{
				SqlDataReader reader;
				_RecruitmentFormEmployeeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RecruitmentFormEmployeeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RecruitmentFormEmployee object
        /// </summary>
        /// <param name="recruitmentFormEmployeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RecruitmentFormEmployeeBase recruitmentFormEmployeeObject, SqlDataReader reader, int start)
		{
			
				recruitmentFormEmployeeObject.Id = reader.GetInt32( start + 0 );			
				recruitmentFormEmployeeObject.RecruitmentFormId = reader.GetInt32( start + 1 );			
				recruitmentFormEmployeeObject.EmployeeId = reader.GetGuid( start + 2 );			
				recruitmentFormEmployeeObject.FormId = reader.GetGuid( start + 3 );			
				recruitmentFormEmployeeObject.IsFillUp = reader.GetBoolean( start + 4 );			
				recruitmentFormEmployeeObject.IsSubmitted = reader.GetBoolean( start + 5 );			
				if(!reader.IsDBNull(6)) recruitmentFormEmployeeObject.FillDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) recruitmentFormEmployeeObject.SubmitDate = reader.GetDateTime( start + 7 );			
			FillBaseObject(recruitmentFormEmployeeObject, reader, (start + 8));

			
			recruitmentFormEmployeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RecruitmentFormEmployee object
        /// </summary>
        /// <param name="recruitmentFormEmployeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RecruitmentFormEmployeeBase recruitmentFormEmployeeObject, SqlDataReader reader)
		{
			FillObject(recruitmentFormEmployeeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RecruitmentFormEmployee object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RecruitmentFormEmployee object</returns>
		private RecruitmentFormEmployee GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RecruitmentFormEmployee recruitmentFormEmployeeObject= new RecruitmentFormEmployee();
					FillObject(recruitmentFormEmployeeObject, reader);
					return recruitmentFormEmployeeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RecruitmentFormEmployee objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RecruitmentFormEmployee objects</returns>
		private RecruitmentFormEmployeeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RecruitmentFormEmployee list
			RecruitmentFormEmployeeList list = new RecruitmentFormEmployeeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RecruitmentFormEmployee recruitmentFormEmployeeObject = new RecruitmentFormEmployee();
					FillObject(recruitmentFormEmployeeObject, reader);

					list.Add(recruitmentFormEmployeeObject);
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
