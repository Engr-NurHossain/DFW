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
	public partial class EmployeeTimeClockSupervisorDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEETIMECLOCKSUPERVISOR = "InsertEmployeeTimeClockSupervisor";
		private const string UPDATEEMPLOYEETIMECLOCKSUPERVISOR = "UpdateEmployeeTimeClockSupervisor";
		private const string DELETEEMPLOYEETIMECLOCKSUPERVISOR = "DeleteEmployeeTimeClockSupervisor";
		private const string GETEMPLOYEETIMECLOCKSUPERVISORBYID = "GetEmployeeTimeClockSupervisorById";
		private const string GETALLEMPLOYEETIMECLOCKSUPERVISOR = "GetAllEmployeeTimeClockSupervisor";
		private const string GETPAGEDEMPLOYEETIMECLOCKSUPERVISOR = "GetPagedEmployeeTimeClockSupervisor";
		private const string GETEMPLOYEETIMECLOCKSUPERVISORMAXIMUMID = "GetEmployeeTimeClockSupervisorMaximumId";
		private const string GETEMPLOYEETIMECLOCKSUPERVISORROWCOUNT = "GetEmployeeTimeClockSupervisorRowCount";	
		private const string GETEMPLOYEETIMECLOCKSUPERVISORBYQUERY = "GetEmployeeTimeClockSupervisorByQuery";
		#endregion
		
		#region Constructors
		public EmployeeTimeClockSupervisorDataAccess(ClientContext context) : base(context) { }
		public EmployeeTimeClockSupervisorDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeTimeClockSupervisorObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeTimeClockSupervisorBase employeeTimeClockSupervisorObject)
		{	
			AddParameter(cmd, pGuid(EmployeeTimeClockSupervisorBase.Property_UserId, employeeTimeClockSupervisorObject.UserId));
			AddParameter(cmd, pGuid(EmployeeTimeClockSupervisorBase.Property_SupervisorId, employeeTimeClockSupervisorObject.SupervisorId));
			AddParameter(cmd, pGuid(EmployeeTimeClockSupervisorBase.Property_CreatedBy, employeeTimeClockSupervisorObject.CreatedBy));
			AddParameter(cmd, pDateTime(EmployeeTimeClockSupervisorBase.Property_CreatedDate, employeeTimeClockSupervisorObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeTimeClockSupervisor
        /// </summary>
        /// <param name="employeeTimeClockSupervisorObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeTimeClockSupervisorBase employeeTimeClockSupervisorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEETIMECLOCKSUPERVISOR);
	
				AddParameter(cmd, pInt32Out(EmployeeTimeClockSupervisorBase.Property_Id));
				AddCommonParams(cmd, employeeTimeClockSupervisorObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeTimeClockSupervisorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeTimeClockSupervisorObject.Id = (Int32)GetOutParameter(cmd, EmployeeTimeClockSupervisorBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeTimeClockSupervisorObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeTimeClockSupervisor
        /// </summary>
        /// <param name="employeeTimeClockSupervisorObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeTimeClockSupervisorBase employeeTimeClockSupervisorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEETIMECLOCKSUPERVISOR);
				
				AddParameter(cmd, pInt32(EmployeeTimeClockSupervisorBase.Property_Id, employeeTimeClockSupervisorObject.Id));
				AddCommonParams(cmd, employeeTimeClockSupervisorObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeTimeClockSupervisorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeTimeClockSupervisorObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeTimeClockSupervisor
        /// </summary>
        /// <param name="Id">Id of the EmployeeTimeClockSupervisor object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEETIMECLOCKSUPERVISOR);	
				
				AddParameter(cmd, pInt32(EmployeeTimeClockSupervisorBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeTimeClockSupervisor), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeTimeClockSupervisor object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeTimeClockSupervisor object to retrieve</param>
        /// <returns>EmployeeTimeClockSupervisor object, null if not found</returns>
		public EmployeeTimeClockSupervisor Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEETIMECLOCKSUPERVISORBYID))
			{
				AddParameter( cmd, pInt32(EmployeeTimeClockSupervisorBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeTimeClockSupervisor objects 
        /// </summary>
        /// <returns>A list of EmployeeTimeClockSupervisor objects</returns>
		public EmployeeTimeClockSupervisorList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEETIMECLOCKSUPERVISOR))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeTimeClockSupervisor objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeTimeClockSupervisor objects</returns>
		public EmployeeTimeClockSupervisorList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEETIMECLOCKSUPERVISOR))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeTimeClockSupervisorList _EmployeeTimeClockSupervisorList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeTimeClockSupervisorList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeTimeClockSupervisor objects by query String
        /// </summary>
        /// <returns>A list of EmployeeTimeClockSupervisor objects</returns>
		public EmployeeTimeClockSupervisorList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEETIMECLOCKSUPERVISORBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeTimeClockSupervisor Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeTimeClockSupervisor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEETIMECLOCKSUPERVISORMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeTimeClockSupervisor Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeTimeClockSupervisor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeTimeClockSupervisorRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEETIMECLOCKSUPERVISORROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeTimeClockSupervisorRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeTimeClockSupervisorRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeTimeClockSupervisor object
        /// </summary>
        /// <param name="employeeTimeClockSupervisorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeTimeClockSupervisorBase employeeTimeClockSupervisorObject, SqlDataReader reader, int start)
		{
			
				employeeTimeClockSupervisorObject.Id = reader.GetInt32( start + 0 );			
				employeeTimeClockSupervisorObject.UserId = reader.GetGuid( start + 1 );			
				employeeTimeClockSupervisorObject.SupervisorId = reader.GetGuid( start + 2 );			
				employeeTimeClockSupervisorObject.CreatedBy = reader.GetGuid( start + 3 );			
				employeeTimeClockSupervisorObject.CreatedDate = reader.GetDateTime( start + 4 );			
			FillBaseObject(employeeTimeClockSupervisorObject, reader, (start + 5));

			
			employeeTimeClockSupervisorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeTimeClockSupervisor object
        /// </summary>
        /// <param name="employeeTimeClockSupervisorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeTimeClockSupervisorBase employeeTimeClockSupervisorObject, SqlDataReader reader)
		{
			FillObject(employeeTimeClockSupervisorObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeTimeClockSupervisor object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeTimeClockSupervisor object</returns>
		private EmployeeTimeClockSupervisor GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeTimeClockSupervisor employeeTimeClockSupervisorObject= new EmployeeTimeClockSupervisor();
					FillObject(employeeTimeClockSupervisorObject, reader);
					return employeeTimeClockSupervisorObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeTimeClockSupervisor objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeTimeClockSupervisor objects</returns>
		private EmployeeTimeClockSupervisorList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeTimeClockSupervisor list
			EmployeeTimeClockSupervisorList list = new EmployeeTimeClockSupervisorList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeTimeClockSupervisor employeeTimeClockSupervisorObject = new EmployeeTimeClockSupervisor();
					FillObject(employeeTimeClockSupervisorObject, reader);

					list.Add(employeeTimeClockSupervisorObject);
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
