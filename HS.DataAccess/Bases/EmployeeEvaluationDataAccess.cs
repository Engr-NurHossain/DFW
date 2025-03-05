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
	public partial class EmployeeEvaluationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEEVALUATION = "InsertEmployeeEvaluation";
		private const string UPDATEEMPLOYEEEVALUATION = "UpdateEmployeeEvaluation";
		private const string DELETEEMPLOYEEEVALUATION = "DeleteEmployeeEvaluation";
		private const string GETEMPLOYEEEVALUATIONBYID = "GetEmployeeEvaluationById";
		private const string GETALLEMPLOYEEEVALUATION = "GetAllEmployeeEvaluation";
		private const string GETPAGEDEMPLOYEEEVALUATION = "GetPagedEmployeeEvaluation";
		private const string GETEMPLOYEEEVALUATIONMAXIMUMID = "GetEmployeeEvaluationMaximumId";
		private const string GETEMPLOYEEEVALUATIONROWCOUNT = "GetEmployeeEvaluationRowCount";	
		private const string GETEMPLOYEEEVALUATIONBYQUERY = "GetEmployeeEvaluationByQuery";
		#endregion
		
		#region Constructors
		public EmployeeEvaluationDataAccess(ClientContext context) : base(context) { }
		public EmployeeEvaluationDataAccess(string ConStr) : base(ConStr) { }
		public EmployeeEvaluationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeEvaluationObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeEvaluationBase employeeEvaluationObject)
		{	
			AddParameter(cmd, pGuid(EmployeeEvaluationBase.Property_UserId, employeeEvaluationObject.UserId));
			AddParameter(cmd, pDateTime(EmployeeEvaluationBase.Property_EvaluationReminderDate, employeeEvaluationObject.EvaluationReminderDate));
			AddParameter(cmd, pNVarChar(EmployeeEvaluationBase.Property_EvaluationType, 50, employeeEvaluationObject.EvaluationType));
			AddParameter(cmd, pGuid(EmployeeEvaluationBase.Property_CreatedByUid, employeeEvaluationObject.CreatedByUid));
			AddParameter(cmd, pDateTime(EmployeeEvaluationBase.Property_CreatedDate, employeeEvaluationObject.CreatedDate));
			AddParameter(cmd, pGuid(EmployeeEvaluationBase.Property_LastUpdatedByUid, employeeEvaluationObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(EmployeeEvaluationBase.Property_LastUpdatedDate, employeeEvaluationObject.LastUpdatedDate));
			AddParameter(cmd, pDateTime(EmployeeEvaluationBase.Property_LastEvaluationDate, employeeEvaluationObject.LastEvaluationDate));
			AddParameter(cmd, pDateTime(EmployeeEvaluationBase.Property_NextEvaluationDate, employeeEvaluationObject.NextEvaluationDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeEvaluation
        /// </summary>
        /// <param name="employeeEvaluationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeEvaluationBase employeeEvaluationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEEVALUATION);
	
				AddParameter(cmd, pInt32Out(EmployeeEvaluationBase.Property_Id));
				AddCommonParams(cmd, employeeEvaluationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeEvaluationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeEvaluationObject.Id = (Int32)GetOutParameter(cmd, EmployeeEvaluationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeEvaluationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeEvaluation
        /// </summary>
        /// <param name="employeeEvaluationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeEvaluationBase employeeEvaluationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEEVALUATION);
				
				AddParameter(cmd, pInt32(EmployeeEvaluationBase.Property_Id, employeeEvaluationObject.Id));
				AddCommonParams(cmd, employeeEvaluationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeEvaluationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeEvaluationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeEvaluation
        /// </summary>
        /// <param name="Id">Id of the EmployeeEvaluation object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEEVALUATION);	
				
				AddParameter(cmd, pInt32(EmployeeEvaluationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeEvaluation), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeEvaluation object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeEvaluation object to retrieve</param>
        /// <returns>EmployeeEvaluation object, null if not found</returns>
		public EmployeeEvaluation Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEEVALUATIONBYID))
			{
				AddParameter( cmd, pInt32(EmployeeEvaluationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeEvaluation objects 
        /// </summary>
        /// <returns>A list of EmployeeEvaluation objects</returns>
		public EmployeeEvaluationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEEVALUATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeEvaluation objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeEvaluation objects</returns>
		public EmployeeEvaluationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEEVALUATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeEvaluationList _EmployeeEvaluationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeEvaluationList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeEvaluation objects by query String
        /// </summary>
        /// <returns>A list of EmployeeEvaluation objects</returns>
		public EmployeeEvaluationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEEVALUATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeEvaluation Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeEvaluation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEEVALUATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeEvaluation Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeEvaluation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeEvaluationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEEVALUATIONROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeEvaluationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeEvaluationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeEvaluation object
        /// </summary>
        /// <param name="employeeEvaluationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeEvaluationBase employeeEvaluationObject, SqlDataReader reader, int start)
		{
			
				employeeEvaluationObject.Id = reader.GetInt32( start + 0 );			
				employeeEvaluationObject.UserId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) employeeEvaluationObject.EvaluationReminderDate = reader.GetDateTime( start + 2 );			
				if(!reader.IsDBNull(3)) employeeEvaluationObject.EvaluationType = reader.GetString( start + 3 );			
				employeeEvaluationObject.CreatedByUid = reader.GetGuid( start + 4 );			
				employeeEvaluationObject.CreatedDate = reader.GetDateTime( start + 5 );			
				employeeEvaluationObject.LastUpdatedByUid = reader.GetGuid( start + 6 );			
				employeeEvaluationObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) employeeEvaluationObject.LastEvaluationDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) employeeEvaluationObject.NextEvaluationDate = reader.GetDateTime( start + 9 );			
			FillBaseObject(employeeEvaluationObject, reader, (start + 10));

			
			employeeEvaluationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeEvaluation object
        /// </summary>
        /// <param name="employeeEvaluationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeEvaluationBase employeeEvaluationObject, SqlDataReader reader)
		{
			FillObject(employeeEvaluationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeEvaluation object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeEvaluation object</returns>
		private EmployeeEvaluation GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeEvaluation employeeEvaluationObject= new EmployeeEvaluation();
					FillObject(employeeEvaluationObject, reader);
					return employeeEvaluationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeEvaluation objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeEvaluation objects</returns>
		private EmployeeEvaluationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeEvaluation list
			EmployeeEvaluationList list = new EmployeeEvaluationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeEvaluation employeeEvaluationObject = new EmployeeEvaluation();
					FillObject(employeeEvaluationObject, reader);

					list.Add(employeeEvaluationObject);
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
