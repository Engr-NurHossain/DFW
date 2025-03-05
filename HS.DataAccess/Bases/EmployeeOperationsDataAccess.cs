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
	public partial class EmployeeOperationsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEOPERATIONS = "InsertEmployeeOperations";
		private const string UPDATEEMPLOYEEOPERATIONS = "UpdateEmployeeOperations";
		private const string DELETEEMPLOYEEOPERATIONS = "DeleteEmployeeOperations";
		private const string GETEMPLOYEEOPERATIONSBYID = "GetEmployeeOperationsById";
		private const string GETALLEMPLOYEEOPERATIONS = "GetAllEmployeeOperations";
		private const string GETPAGEDEMPLOYEEOPERATIONS = "GetPagedEmployeeOperations";
		private const string GETEMPLOYEEOPERATIONSMAXIMUMID = "GetEmployeeOperationsMaximumId";
		private const string GETEMPLOYEEOPERATIONSROWCOUNT = "GetEmployeeOperationsRowCount";	
		private const string GETEMPLOYEEOPERATIONSBYQUERY = "GetEmployeeOperationsByQuery";
		#endregion
		
		#region Constructors
		public EmployeeOperationsDataAccess(ClientContext context) : base(context) { }
		public EmployeeOperationsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeOperationsObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeOperationsBase employeeOperationsObject)
		{	
			AddParameter(cmd, pGuid(EmployeeOperationsBase.Property_EmployeeId, employeeOperationsObject.EmployeeId));
			AddParameter(cmd, pNVarChar(EmployeeOperationsBase.Property_DayName, 50, employeeOperationsObject.DayName));
			AddParameter(cmd, pNVarChar(EmployeeOperationsBase.Property_OperationStartTime, 50, employeeOperationsObject.OperationStartTime));
			AddParameter(cmd, pNVarChar(EmployeeOperationsBase.Property_OperationEndTime, 50, employeeOperationsObject.OperationEndTime));
			AddParameter(cmd, pGuid(EmployeeOperationsBase.Property_LastUpdatedBy, employeeOperationsObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(EmployeeOperationsBase.Property_UpdatedDate, employeeOperationsObject.UpdatedDate));
			AddParameter(cmd, pGuid(EmployeeOperationsBase.Property_CompanyId, employeeOperationsObject.CompanyId));
			AddParameter(cmd, pNVarChar(EmployeeOperationsBase.Property_Notes, 250, employeeOperationsObject.Notes));
			AddParameter(cmd, pBool(EmployeeOperationsBase.Property_IsDayOff, employeeOperationsObject.IsDayOff));
			AddParameter(cmd, pDateTime(EmployeeOperationsBase.Property_SelectedDate, employeeOperationsObject.SelectedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeOperations
        /// </summary>
        /// <param name="employeeOperationsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeOperationsBase employeeOperationsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEOPERATIONS);
	
				AddParameter(cmd, pInt32Out(EmployeeOperationsBase.Property_Id));
				AddCommonParams(cmd, employeeOperationsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeOperationsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeOperationsObject.Id = (Int32)GetOutParameter(cmd, EmployeeOperationsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeOperationsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeOperations
        /// </summary>
        /// <param name="employeeOperationsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeOperationsBase employeeOperationsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEOPERATIONS);
				
				AddParameter(cmd, pInt32(EmployeeOperationsBase.Property_Id, employeeOperationsObject.Id));
				AddCommonParams(cmd, employeeOperationsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeOperationsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeOperationsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeOperations
        /// </summary>
        /// <param name="Id">Id of the EmployeeOperations object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEOPERATIONS);	
				
				AddParameter(cmd, pInt32(EmployeeOperationsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeOperations), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeOperations object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeOperations object to retrieve</param>
        /// <returns>EmployeeOperations object, null if not found</returns>
		public EmployeeOperations Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEOPERATIONSBYID))
			{
				AddParameter( cmd, pInt32(EmployeeOperationsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeOperations objects 
        /// </summary>
        /// <returns>A list of EmployeeOperations objects</returns>
		public EmployeeOperationsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEOPERATIONS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeOperations objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeOperations objects</returns>
		public EmployeeOperationsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEOPERATIONS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeOperationsList _EmployeeOperationsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeOperationsList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeOperations objects by query String
        /// </summary>
        /// <returns>A list of EmployeeOperations objects</returns>
		public EmployeeOperationsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEOPERATIONSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeOperations Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeOperations
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEOPERATIONSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeOperations Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeOperations
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeOperationsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEOPERATIONSROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeOperationsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeOperationsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeOperations object
        /// </summary>
        /// <param name="employeeOperationsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeOperationsBase employeeOperationsObject, SqlDataReader reader, int start)
		{
			
				employeeOperationsObject.Id = reader.GetInt32( start + 0 );			
				employeeOperationsObject.EmployeeId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) employeeOperationsObject.DayName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) employeeOperationsObject.OperationStartTime = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) employeeOperationsObject.OperationEndTime = reader.GetString( start + 4 );			
				employeeOperationsObject.LastUpdatedBy = reader.GetGuid( start + 5 );			
				employeeOperationsObject.UpdatedDate = reader.GetDateTime( start + 6 );			
				employeeOperationsObject.CompanyId = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) employeeOperationsObject.Notes = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) employeeOperationsObject.IsDayOff = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) employeeOperationsObject.SelectedDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(employeeOperationsObject, reader, (start + 11));

			
			employeeOperationsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeOperations object
        /// </summary>
        /// <param name="employeeOperationsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeOperationsBase employeeOperationsObject, SqlDataReader reader)
		{
			FillObject(employeeOperationsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeOperations object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeOperations object</returns>
		private EmployeeOperations GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeOperations employeeOperationsObject= new EmployeeOperations();
					FillObject(employeeOperationsObject, reader);
					return employeeOperationsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeOperations objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeOperations objects</returns>
		private EmployeeOperationsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeOperations list
			EmployeeOperationsList list = new EmployeeOperationsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeOperations employeeOperationsObject = new EmployeeOperations();
					FillObject(employeeOperationsObject, reader);

					list.Add(employeeOperationsObject);
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