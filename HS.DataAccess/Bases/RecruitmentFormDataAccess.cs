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
	public partial class RecruitmentFormDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRECRUITMENTFORM = "InsertRecruitmentForm";
		private const string UPDATERECRUITMENTFORM = "UpdateRecruitmentForm";
		private const string DELETERECRUITMENTFORM = "DeleteRecruitmentForm";
		private const string GETRECRUITMENTFORMBYID = "GetRecruitmentFormById";
		private const string GETALLRECRUITMENTFORM = "GetAllRecruitmentForm";
		private const string GETPAGEDRECRUITMENTFORM = "GetPagedRecruitmentForm";
		private const string GETRECRUITMENTFORMMAXIMUMID = "GetRecruitmentFormMaximumId";
		private const string GETRECRUITMENTFORMROWCOUNT = "GetRecruitmentFormRowCount";	
		private const string GETRECRUITMENTFORMBYQUERY = "GetRecruitmentFormByQuery";
		#endregion
		
		#region Constructors
		public RecruitmentFormDataAccess(ClientContext context) : base(context) { }
		public RecruitmentFormDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="recruitmentFormObject"></param>
		private void AddCommonParams(SqlCommand cmd, RecruitmentFormBase recruitmentFormObject)
		{	
			AddParameter(cmd, pGuid(RecruitmentFormBase.Property_CompanyId, recruitmentFormObject.CompanyId));
			AddParameter(cmd, pNVarChar(RecruitmentFormBase.Property_Name, 500, recruitmentFormObject.Name));
			AddParameter(cmd, pNVarChar(RecruitmentFormBase.Property_DisplayName, 500, recruitmentFormObject.DisplayName));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RecruitmentForm
        /// </summary>
        /// <param name="recruitmentFormObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RecruitmentFormBase recruitmentFormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRECRUITMENTFORM);
	
				AddParameter(cmd, pInt32Out(RecruitmentFormBase.Property_Id));
				AddCommonParams(cmd, recruitmentFormObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					recruitmentFormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					recruitmentFormObject.Id = (Int32)GetOutParameter(cmd, RecruitmentFormBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(recruitmentFormObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RecruitmentForm
        /// </summary>
        /// <param name="recruitmentFormObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RecruitmentFormBase recruitmentFormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERECRUITMENTFORM);
				
				AddParameter(cmd, pInt32(RecruitmentFormBase.Property_Id, recruitmentFormObject.Id));
				AddCommonParams(cmd, recruitmentFormObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					recruitmentFormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(recruitmentFormObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RecruitmentForm
        /// </summary>
        /// <param name="Id">Id of the RecruitmentForm object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERECRUITMENTFORM);	
				
				AddParameter(cmd, pInt32(RecruitmentFormBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RecruitmentForm), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RecruitmentForm object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RecruitmentForm object to retrieve</param>
        /// <returns>RecruitmentForm object, null if not found</returns>
		public RecruitmentForm Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTFORMBYID))
			{
				AddParameter( cmd, pInt32(RecruitmentFormBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RecruitmentForm objects 
        /// </summary>
        /// <returns>A list of RecruitmentForm objects</returns>
		public RecruitmentFormList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRECRUITMENTFORM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RecruitmentForm objects by PageRequest
        /// </summary>
        /// <returns>A list of RecruitmentForm objects</returns>
		public RecruitmentFormList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRECRUITMENTFORM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RecruitmentFormList _RecruitmentFormList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RecruitmentFormList;
			}
		}
		
		/// <summary>
        /// Retrieves all RecruitmentForm objects by query String
        /// </summary>
        /// <returns>A list of RecruitmentForm objects</returns>
		public RecruitmentFormList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTFORMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RecruitmentForm Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RecruitmentForm
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTFORMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RecruitmentForm Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RecruitmentForm
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RecruitmentFormRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTFORMROWCOUNT))
			{
				SqlDataReader reader;
				_RecruitmentFormRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RecruitmentFormRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RecruitmentForm object
        /// </summary>
        /// <param name="recruitmentFormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RecruitmentFormBase recruitmentFormObject, SqlDataReader reader, int start)
		{
			
				recruitmentFormObject.Id = reader.GetInt32( start + 0 );			
				recruitmentFormObject.CompanyId = reader.GetGuid( start + 1 );			
				recruitmentFormObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) recruitmentFormObject.DisplayName = reader.GetString( start + 3 );			
			FillBaseObject(recruitmentFormObject, reader, (start + 4));

			
			recruitmentFormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RecruitmentForm object
        /// </summary>
        /// <param name="recruitmentFormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RecruitmentFormBase recruitmentFormObject, SqlDataReader reader)
		{
			FillObject(recruitmentFormObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RecruitmentForm object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RecruitmentForm object</returns>
		private RecruitmentForm GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RecruitmentForm recruitmentFormObject= new RecruitmentForm();
					FillObject(recruitmentFormObject, reader);
					return recruitmentFormObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RecruitmentForm objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RecruitmentForm objects</returns>
		private RecruitmentFormList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RecruitmentForm list
			RecruitmentFormList list = new RecruitmentFormList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RecruitmentForm recruitmentFormObject = new RecruitmentForm();
					FillObject(recruitmentFormObject, reader);

					list.Add(recruitmentFormObject);
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
