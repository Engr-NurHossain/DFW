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
	public partial class EmployeeComputerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEECOMPUTER = "InsertEmployeeComputer";
		private const string UPDATEEMPLOYEECOMPUTER = "UpdateEmployeeComputer";
		private const string DELETEEMPLOYEECOMPUTER = "DeleteEmployeeComputer";
		private const string GETEMPLOYEECOMPUTERBYID = "GetEmployeeComputerById";
		private const string GETALLEMPLOYEECOMPUTER = "GetAllEmployeeComputer";
		private const string GETPAGEDEMPLOYEECOMPUTER = "GetPagedEmployeeComputer";
		private const string GETEMPLOYEECOMPUTERMAXIMUMID = "GetEmployeeComputerMaximumId";
		private const string GETEMPLOYEECOMPUTERROWCOUNT = "GetEmployeeComputerRowCount";	
		private const string GETEMPLOYEECOMPUTERBYQUERY = "GetEmployeeComputerByQuery";
		#endregion
		
		#region Constructors
		public EmployeeComputerDataAccess(ClientContext context) : base(context) { }
		public EmployeeComputerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeComputerObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeComputerBase employeeComputerObject)
		{	
			AddParameter(cmd, pGuid(EmployeeComputerBase.Property_CompanyId, employeeComputerObject.CompanyId));
			AddParameter(cmd, pGuid(EmployeeComputerBase.Property_UserId, employeeComputerObject.UserId));
			AddParameter(cmd, pNVarChar(EmployeeComputerBase.Property_ComputerName, 50, employeeComputerObject.ComputerName));
			AddParameter(cmd, pNVarChar(EmployeeComputerBase.Property_ComputerPassword, 50, employeeComputerObject.ComputerPassword));
			AddParameter(cmd, pGuid(EmployeeComputerBase.Property_CreatedBy, employeeComputerObject.CreatedBy));
			AddParameter(cmd, pGuid(EmployeeComputerBase.Property_LastUpdatedBy, employeeComputerObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(EmployeeComputerBase.Property_LastUpdatedDate, employeeComputerObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeComputer
        /// </summary>
        /// <param name="employeeComputerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeComputerBase employeeComputerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEECOMPUTER);
	
				AddParameter(cmd, pInt32Out(EmployeeComputerBase.Property_Id));
				AddCommonParams(cmd, employeeComputerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeComputerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeComputerObject.Id = (Int32)GetOutParameter(cmd, EmployeeComputerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeComputerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeComputer
        /// </summary>
        /// <param name="employeeComputerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeComputerBase employeeComputerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEECOMPUTER);
				
				AddParameter(cmd, pInt32(EmployeeComputerBase.Property_Id, employeeComputerObject.Id));
				AddCommonParams(cmd, employeeComputerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeComputerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeComputerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeComputer
        /// </summary>
        /// <param name="Id">Id of the EmployeeComputer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEECOMPUTER);	
				
				AddParameter(cmd, pInt32(EmployeeComputerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeComputer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeComputer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeComputer object to retrieve</param>
        /// <returns>EmployeeComputer object, null if not found</returns>
		public EmployeeComputer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEECOMPUTERBYID))
			{
				AddParameter( cmd, pInt32(EmployeeComputerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeComputer objects 
        /// </summary>
        /// <returns>A list of EmployeeComputer objects</returns>
		public EmployeeComputerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEECOMPUTER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeComputer objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeComputer objects</returns>
		public EmployeeComputerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEECOMPUTER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeComputerList _EmployeeComputerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeComputerList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeComputer objects by query String
        /// </summary>
        /// <returns>A list of EmployeeComputer objects</returns>
		public EmployeeComputerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEECOMPUTERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeComputer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeComputer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEECOMPUTERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeComputer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeComputer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeComputerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEECOMPUTERROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeComputerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeComputerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeComputer object
        /// </summary>
        /// <param name="employeeComputerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeComputerBase employeeComputerObject, SqlDataReader reader, int start)
		{
			
				employeeComputerObject.Id = reader.GetInt32( start + 0 );			
				employeeComputerObject.CompanyId = reader.GetGuid( start + 1 );			
				employeeComputerObject.UserId = reader.GetGuid( start + 2 );			
				employeeComputerObject.ComputerName = reader.GetString( start + 3 );			
				employeeComputerObject.ComputerPassword = reader.GetString( start + 4 );			
				employeeComputerObject.CreatedBy = reader.GetGuid( start + 5 );			
				employeeComputerObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
				employeeComputerObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
			FillBaseObject(employeeComputerObject, reader, (start + 8));

			
			employeeComputerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeComputer object
        /// </summary>
        /// <param name="employeeComputerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeComputerBase employeeComputerObject, SqlDataReader reader)
		{
			FillObject(employeeComputerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeComputer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeComputer object</returns>
		private EmployeeComputer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeComputer employeeComputerObject= new EmployeeComputer();
					FillObject(employeeComputerObject, reader);
					return employeeComputerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeComputer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeComputer objects</returns>
		private EmployeeComputerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeComputer list
			EmployeeComputerList list = new EmployeeComputerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeComputer employeeComputerObject = new EmployeeComputer();
					FillObject(employeeComputerObject, reader);

					list.Add(employeeComputerObject);
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
