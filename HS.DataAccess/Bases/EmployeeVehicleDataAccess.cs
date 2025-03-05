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
	public partial class EmployeeVehicleDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEVEHICLE = "InsertEmployeeVehicle";
		private const string UPDATEEMPLOYEEVEHICLE = "UpdateEmployeeVehicle";
		private const string DELETEEMPLOYEEVEHICLE = "DeleteEmployeeVehicle";
		private const string GETEMPLOYEEVEHICLEBYID = "GetEmployeeVehicleById";
		private const string GETALLEMPLOYEEVEHICLE = "GetAllEmployeeVehicle";
		private const string GETPAGEDEMPLOYEEVEHICLE = "GetPagedEmployeeVehicle";
		private const string GETEMPLOYEEVEHICLEMAXIMUMID = "GetEmployeeVehicleMaximumId";
		private const string GETEMPLOYEEVEHICLEROWCOUNT = "GetEmployeeVehicleRowCount";	
		private const string GETEMPLOYEEVEHICLEBYQUERY = "GetEmployeeVehicleByQuery";
		#endregion
		
		#region Constructors
		public EmployeeVehicleDataAccess(ClientContext context) : base(context) { }
		public EmployeeVehicleDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeVehicleObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeVehicleBase employeeVehicleObject)
		{	
			AddParameter(cmd, pGuid(EmployeeVehicleBase.Property_CompanyId, employeeVehicleObject.CompanyId));
			AddParameter(cmd, pGuid(EmployeeVehicleBase.Property_EmployeeId, employeeVehicleObject.EmployeeId));
			AddParameter(cmd, pGuid(EmployeeVehicleBase.Property_VehicleId, employeeVehicleObject.VehicleId));
			AddParameter(cmd, pGuid(EmployeeVehicleBase.Property_AddedBy, employeeVehicleObject.AddedBy));
			AddParameter(cmd, pDateTime(EmployeeVehicleBase.Property_AddedDate, employeeVehicleObject.AddedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeVehicle
        /// </summary>
        /// <param name="employeeVehicleObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeVehicleBase employeeVehicleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEVEHICLE);
	
				AddParameter(cmd, pInt32Out(EmployeeVehicleBase.Property_Id));
				AddCommonParams(cmd, employeeVehicleObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeVehicleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeVehicleObject.Id = (Int32)GetOutParameter(cmd, EmployeeVehicleBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeVehicleObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeVehicle
        /// </summary>
        /// <param name="employeeVehicleObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeVehicleBase employeeVehicleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEVEHICLE);
				
				AddParameter(cmd, pInt32(EmployeeVehicleBase.Property_Id, employeeVehicleObject.Id));
				AddCommonParams(cmd, employeeVehicleObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeVehicleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeVehicleObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeVehicle
        /// </summary>
        /// <param name="Id">Id of the EmployeeVehicle object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEVEHICLE);	
				
				AddParameter(cmd, pInt32(EmployeeVehicleBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeVehicle), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeVehicle object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeVehicle object to retrieve</param>
        /// <returns>EmployeeVehicle object, null if not found</returns>
		public EmployeeVehicle Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEVEHICLEBYID))
			{
				AddParameter( cmd, pInt32(EmployeeVehicleBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeVehicle objects 
        /// </summary>
        /// <returns>A list of EmployeeVehicle objects</returns>
		public EmployeeVehicleList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEVEHICLE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeVehicle objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeVehicle objects</returns>
		public EmployeeVehicleList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEVEHICLE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeVehicleList _EmployeeVehicleList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeVehicleList;
			}
		}
        
        /// <summary>
        /// Retrieves all EmployeeVehicle objects by query String
        /// </summary>
        /// <returns>A list of EmployeeVehicle objects</returns>
        public EmployeeVehicleList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEVEHICLEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeVehicle Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeVehicle
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEVEHICLEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeVehicle Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeVehicle
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeVehicleRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEVEHICLEROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeVehicleRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeVehicleRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeVehicle object
        /// </summary>
        /// <param name="employeeVehicleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeVehicleBase employeeVehicleObject, SqlDataReader reader, int start)
		{
			
				employeeVehicleObject.Id = reader.GetInt32( start + 0 );			
				employeeVehicleObject.CompanyId = reader.GetGuid( start + 1 );			
				employeeVehicleObject.EmployeeId = reader.GetGuid( start + 2 );			
				employeeVehicleObject.VehicleId = reader.GetGuid( start + 3 );			
				employeeVehicleObject.AddedBy = reader.GetGuid( start + 4 );			
				employeeVehicleObject.AddedDate = reader.GetDateTime( start + 5 );			
			FillBaseObject(employeeVehicleObject, reader, (start + 6));

			
			employeeVehicleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeVehicle object
        /// </summary>
        /// <param name="employeeVehicleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeVehicleBase employeeVehicleObject, SqlDataReader reader)
		{
			FillObject(employeeVehicleObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeVehicle object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeVehicle object</returns>
		private EmployeeVehicle GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeVehicle employeeVehicleObject= new EmployeeVehicle();
					FillObject(employeeVehicleObject, reader);
					return employeeVehicleObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeVehicle objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeVehicle objects</returns>
		private EmployeeVehicleList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeVehicle list
			EmployeeVehicleList list = new EmployeeVehicleList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeVehicle employeeVehicleObject = new EmployeeVehicle();
					FillObject(employeeVehicleObject, reader);

					list.Add(employeeVehicleObject);
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
