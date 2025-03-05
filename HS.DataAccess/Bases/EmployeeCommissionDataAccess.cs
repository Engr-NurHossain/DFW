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
	public partial class EmployeeCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEECOMMISSION = "InsertEmployeeCommission";
		private const string UPDATEEMPLOYEECOMMISSION = "UpdateEmployeeCommission";
		private const string DELETEEMPLOYEECOMMISSION = "DeleteEmployeeCommission";
		private const string GETEMPLOYEECOMMISSIONBYID = "GetEmployeeCommissionById";
		private const string GETALLEMPLOYEECOMMISSION = "GetAllEmployeeCommission";
		private const string GETPAGEDEMPLOYEECOMMISSION = "GetPagedEmployeeCommission";
		private const string GETEMPLOYEECOMMISSIONMAXIMUMID = "GetEmployeeCommissionMaximumId";
		private const string GETEMPLOYEECOMMISSIONROWCOUNT = "GetEmployeeCommissionRowCount";	
		private const string GETEMPLOYEECOMMISSIONBYQUERY = "GetEmployeeCommissionByQuery";
		#endregion
		
		#region Constructors
		public EmployeeCommissionDataAccess(ClientContext context) : base(context) { }
		public EmployeeCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeCommissionBase employeeCommissionObject)
		{	
			AddParameter(cmd, pGuid(EmployeeCommissionBase.Property_CompanyId, employeeCommissionObject.CompanyId));
			AddParameter(cmd, pGuid(EmployeeCommissionBase.Property_EmployeeCommissionId, employeeCommissionObject.EmployeeCommissionId));
			AddParameter(cmd, pGuid(EmployeeCommissionBase.Property_UserId, employeeCommissionObject.UserId));
			AddParameter(cmd, pGuid(EmployeeCommissionBase.Property_CustomerId, employeeCommissionObject.CustomerId));
			AddParameter(cmd, pDouble(EmployeeCommissionBase.Property_Amount, employeeCommissionObject.Amount));
			AddParameter(cmd, pDateTime(EmployeeCommissionBase.Property_CreatedDate, employeeCommissionObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeCommission
        /// </summary>
        /// <param name="employeeCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeCommissionBase employeeCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEECOMMISSION);
	
				AddParameter(cmd, pInt32Out(EmployeeCommissionBase.Property_Id));
				AddCommonParams(cmd, employeeCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeCommissionObject.Id = (Int32)GetOutParameter(cmd, EmployeeCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeCommission
        /// </summary>
        /// <param name="employeeCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeCommissionBase employeeCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEECOMMISSION);
				
				AddParameter(cmd, pInt32(EmployeeCommissionBase.Property_Id, employeeCommissionObject.Id));
				AddCommonParams(cmd, employeeCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeCommission
        /// </summary>
        /// <param name="Id">Id of the EmployeeCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEECOMMISSION);	
				
				AddParameter(cmd, pInt32(EmployeeCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeCommission object to retrieve</param>
        /// <returns>EmployeeCommission object, null if not found</returns>
		public EmployeeCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEECOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(EmployeeCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeCommission objects 
        /// </summary>
        /// <returns>A list of EmployeeCommission objects</returns>
		public EmployeeCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEECOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeCommission objects</returns>
		public EmployeeCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEECOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeCommissionList _EmployeeCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeCommission objects by query String
        /// </summary>
        /// <returns>A list of EmployeeCommission objects</returns>
		public EmployeeCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEECOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEECOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEECOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeCommission object
        /// </summary>
        /// <param name="employeeCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeCommissionBase employeeCommissionObject, SqlDataReader reader, int start)
		{
			
				employeeCommissionObject.Id = reader.GetInt32( start + 0 );			
				employeeCommissionObject.CompanyId = reader.GetGuid( start + 1 );			
				employeeCommissionObject.EmployeeCommissionId = reader.GetGuid( start + 2 );			
				employeeCommissionObject.UserId = reader.GetGuid( start + 3 );			
				employeeCommissionObject.CustomerId = reader.GetGuid( start + 4 );			
				employeeCommissionObject.Amount = reader.GetDouble( start + 5 );			
				employeeCommissionObject.CreatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(employeeCommissionObject, reader, (start + 7));

			
			employeeCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeCommission object
        /// </summary>
        /// <param name="employeeCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeCommissionBase employeeCommissionObject, SqlDataReader reader)
		{
			FillObject(employeeCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeCommission object</returns>
		private EmployeeCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeCommission employeeCommissionObject= new EmployeeCommission();
					FillObject(employeeCommissionObject, reader);
					return employeeCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeCommission objects</returns>
		private EmployeeCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeCommission list
			EmployeeCommissionList list = new EmployeeCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeCommission employeeCommissionObject = new EmployeeCommission();
					FillObject(employeeCommissionObject, reader);

					list.Add(employeeCommissionObject);
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
