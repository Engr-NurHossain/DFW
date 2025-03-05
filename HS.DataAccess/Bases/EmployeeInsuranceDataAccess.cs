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
	public partial class EmployeeInsuranceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEINSURANCE = "InsertEmployeeInsurance";
		private const string UPDATEEMPLOYEEINSURANCE = "UpdateEmployeeInsurance";
		private const string DELETEEMPLOYEEINSURANCE = "DeleteEmployeeInsurance";
		private const string GETEMPLOYEEINSURANCEBYID = "GetEmployeeInsuranceById";
		private const string GETALLEMPLOYEEINSURANCE = "GetAllEmployeeInsurance";
		private const string GETPAGEDEMPLOYEEINSURANCE = "GetPagedEmployeeInsurance";
		private const string GETEMPLOYEEINSURANCEMAXIMUMID = "GetEmployeeInsuranceMaximumId";
		private const string GETEMPLOYEEINSURANCEROWCOUNT = "GetEmployeeInsuranceRowCount";	
		private const string GETEMPLOYEEINSURANCEBYQUERY = "GetEmployeeInsuranceByQuery";
		#endregion
		
		#region Constructors
		public EmployeeInsuranceDataAccess(ClientContext context) : base(context) { }
		public EmployeeInsuranceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeInsuranceObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeInsuranceBase employeeInsuranceObject)
		{	
			AddParameter(cmd, pGuid(EmployeeInsuranceBase.Property_UserId, employeeInsuranceObject.UserId));
			AddParameter(cmd, pDateTime(EmployeeInsuranceBase.Property_EligibleFrom, employeeInsuranceObject.EligibleFrom));
			AddParameter(cmd, pNVarChar(EmployeeInsuranceBase.Property_Type, 50, employeeInsuranceObject.Type));
			AddParameter(cmd, pNVarChar(EmployeeInsuranceBase.Property_Subtype, 50, employeeInsuranceObject.Subtype));
			AddParameter(cmd, pDouble(EmployeeInsuranceBase.Property_InsuranceRate, employeeInsuranceObject.InsuranceRate));
			AddParameter(cmd, pNVarChar(EmployeeInsuranceBase.Property_RateType, 50, employeeInsuranceObject.RateType));
			AddParameter(cmd, pBool(EmployeeInsuranceBase.Property_IsActive, employeeInsuranceObject.IsActive));
			AddParameter(cmd, pGuid(EmployeeInsuranceBase.Property_CreatedByUid, employeeInsuranceObject.CreatedByUid));
			AddParameter(cmd, pGuid(EmployeeInsuranceBase.Property_LastUpdatedByUid, employeeInsuranceObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(EmployeeInsuranceBase.Property_LastUpdatedDate, employeeInsuranceObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeInsurance
        /// </summary>
        /// <param name="employeeInsuranceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeInsuranceBase employeeInsuranceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEINSURANCE);
	
				AddParameter(cmd, pInt32Out(EmployeeInsuranceBase.Property_Id));
				AddCommonParams(cmd, employeeInsuranceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeInsuranceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeInsuranceObject.Id = (Int32)GetOutParameter(cmd, EmployeeInsuranceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeInsuranceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeInsurance
        /// </summary>
        /// <param name="employeeInsuranceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeInsuranceBase employeeInsuranceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEINSURANCE);
				
				AddParameter(cmd, pInt32(EmployeeInsuranceBase.Property_Id, employeeInsuranceObject.Id));
				AddCommonParams(cmd, employeeInsuranceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeInsuranceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeInsuranceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeInsurance
        /// </summary>
        /// <param name="Id">Id of the EmployeeInsurance object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEINSURANCE);	
				
				AddParameter(cmd, pInt32(EmployeeInsuranceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeInsurance), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeInsurance object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeInsurance object to retrieve</param>
        /// <returns>EmployeeInsurance object, null if not found</returns>
		public EmployeeInsurance Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEINSURANCEBYID))
			{
				AddParameter( cmd, pInt32(EmployeeInsuranceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeInsurance objects 
        /// </summary>
        /// <returns>A list of EmployeeInsurance objects</returns>
		public EmployeeInsuranceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEINSURANCE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeInsurance objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeInsurance objects</returns>
		public EmployeeInsuranceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEINSURANCE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeInsuranceList _EmployeeInsuranceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeInsuranceList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeInsurance objects by query String
        /// </summary>
        /// <returns>A list of EmployeeInsurance objects</returns>
		public EmployeeInsuranceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEINSURANCEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeInsurance Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeInsurance
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEINSURANCEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeInsurance Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeInsurance
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeInsuranceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEINSURANCEROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeInsuranceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeInsuranceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeInsurance object
        /// </summary>
        /// <param name="employeeInsuranceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeInsuranceBase employeeInsuranceObject, SqlDataReader reader, int start)
		{
			
				employeeInsuranceObject.Id = reader.GetInt32( start + 0 );			
				employeeInsuranceObject.UserId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) employeeInsuranceObject.EligibleFrom = reader.GetDateTime( start + 2 );			
				if(!reader.IsDBNull(3)) employeeInsuranceObject.Type = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) employeeInsuranceObject.Subtype = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) employeeInsuranceObject.InsuranceRate = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) employeeInsuranceObject.RateType = reader.GetString( start + 6 );			
				employeeInsuranceObject.IsActive = reader.GetBoolean( start + 7 );			
				employeeInsuranceObject.CreatedByUid = reader.GetGuid( start + 8 );			
				employeeInsuranceObject.LastUpdatedByUid = reader.GetGuid( start + 9 );			
				employeeInsuranceObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(employeeInsuranceObject, reader, (start + 11));

			
			employeeInsuranceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeInsurance object
        /// </summary>
        /// <param name="employeeInsuranceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeInsuranceBase employeeInsuranceObject, SqlDataReader reader)
		{
			FillObject(employeeInsuranceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeInsurance object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeInsurance object</returns>
		private EmployeeInsurance GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeInsurance employeeInsuranceObject= new EmployeeInsurance();
					FillObject(employeeInsuranceObject, reader);
					return employeeInsuranceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeInsurance objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeInsurance objects</returns>
		private EmployeeInsuranceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeInsurance list
			EmployeeInsuranceList list = new EmployeeInsuranceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeInsurance employeeInsuranceObject = new EmployeeInsurance();
					FillObject(employeeInsuranceObject, reader);

					list.Add(employeeInsuranceObject);
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
