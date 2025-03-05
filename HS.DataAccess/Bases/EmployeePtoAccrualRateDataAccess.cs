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
	public partial class EmployeePtoAccrualRateDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEPTOACCRUALRATE = "InsertEmployeePtoAccrualRate";
		private const string UPDATEEMPLOYEEPTOACCRUALRATE = "UpdateEmployeePtoAccrualRate";
		private const string DELETEEMPLOYEEPTOACCRUALRATE = "DeleteEmployeePtoAccrualRate";
		private const string GETEMPLOYEEPTOACCRUALRATEBYID = "GetEmployeePtoAccrualRateById";
		private const string GETALLEMPLOYEEPTOACCRUALRATE = "GetAllEmployeePtoAccrualRate";
		private const string GETPAGEDEMPLOYEEPTOACCRUALRATE = "GetPagedEmployeePtoAccrualRate";
		private const string GETEMPLOYEEPTOACCRUALRATEMAXIMUMID = "GetEmployeePtoAccrualRateMaximumId";
		private const string GETEMPLOYEEPTOACCRUALRATEROWCOUNT = "GetEmployeePtoAccrualRateRowCount";	
		private const string GETEMPLOYEEPTOACCRUALRATEBYQUERY = "GetEmployeePtoAccrualRateByQuery";
		#endregion
		
		#region Constructors
		public EmployeePtoAccrualRateDataAccess(ClientContext context) : base(context) { }
		public EmployeePtoAccrualRateDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeePtoAccrualRateObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeePtoAccrualRateBase employeePtoAccrualRateObject)
		{	
			AddParameter(cmd, pGuid(EmployeePtoAccrualRateBase.Property_CompanyId, employeePtoAccrualRateObject.CompanyId));
			AddParameter(cmd, pNVarChar(EmployeePtoAccrualRateBase.Property_PayType, 250, employeePtoAccrualRateObject.PayType));
			AddParameter(cmd, pInt32(EmployeePtoAccrualRateBase.Property_MinimumDay, employeePtoAccrualRateObject.MinimumDay));
			AddParameter(cmd, pInt32(EmployeePtoAccrualRateBase.Property_MaximumDay, employeePtoAccrualRateObject.MaximumDay));
			AddParameter(cmd, pDouble(EmployeePtoAccrualRateBase.Property_AccrualRate, employeePtoAccrualRateObject.AccrualRate));
			AddParameter(cmd, pDouble(EmployeePtoAccrualRateBase.Property_PtoHours, employeePtoAccrualRateObject.PtoHours));
			AddParameter(cmd, pGuid(EmployeePtoAccrualRateBase.Property_CreatedBy, employeePtoAccrualRateObject.CreatedBy));
			AddParameter(cmd, pDateTime(EmployeePtoAccrualRateBase.Property_CreatedDate, employeePtoAccrualRateObject.CreatedDate));
			AddParameter(cmd, pGuid(EmployeePtoAccrualRateBase.Property_LastUpdatedBy, employeePtoAccrualRateObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(EmployeePtoAccrualRateBase.Property_LastUpdatedDate, employeePtoAccrualRateObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeePtoAccrualRate
        /// </summary>
        /// <param name="employeePtoAccrualRateObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeePtoAccrualRateBase employeePtoAccrualRateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEPTOACCRUALRATE);
	
				AddParameter(cmd, pInt32Out(EmployeePtoAccrualRateBase.Property_Id));
				AddCommonParams(cmd, employeePtoAccrualRateObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeePtoAccrualRateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeePtoAccrualRateObject.Id = (Int32)GetOutParameter(cmd, EmployeePtoAccrualRateBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeePtoAccrualRateObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeePtoAccrualRate
        /// </summary>
        /// <param name="employeePtoAccrualRateObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeePtoAccrualRateBase employeePtoAccrualRateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEPTOACCRUALRATE);
				
				AddParameter(cmd, pInt32(EmployeePtoAccrualRateBase.Property_Id, employeePtoAccrualRateObject.Id));
				AddCommonParams(cmd, employeePtoAccrualRateObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeePtoAccrualRateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeePtoAccrualRateObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeePtoAccrualRate
        /// </summary>
        /// <param name="Id">Id of the EmployeePtoAccrualRate object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEPTOACCRUALRATE);	
				
				AddParameter(cmd, pInt32(EmployeePtoAccrualRateBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeePtoAccrualRate), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeePtoAccrualRate object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeePtoAccrualRate object to retrieve</param>
        /// <returns>EmployeePtoAccrualRate object, null if not found</returns>
		public EmployeePtoAccrualRate Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEPTOACCRUALRATEBYID))
			{
				AddParameter( cmd, pInt32(EmployeePtoAccrualRateBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeePtoAccrualRate objects 
        /// </summary>
        /// <returns>A list of EmployeePtoAccrualRate objects</returns>
		public EmployeePtoAccrualRateList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEPTOACCRUALRATE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeePtoAccrualRate objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeePtoAccrualRate objects</returns>
		public EmployeePtoAccrualRateList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEPTOACCRUALRATE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeePtoAccrualRateList _EmployeePtoAccrualRateList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeePtoAccrualRateList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeePtoAccrualRate objects by query String
        /// </summary>
        /// <returns>A list of EmployeePtoAccrualRate objects</returns>
		public EmployeePtoAccrualRateList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEPTOACCRUALRATEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeePtoAccrualRate Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeePtoAccrualRate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEPTOACCRUALRATEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeePtoAccrualRate Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeePtoAccrualRate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeePtoAccrualRateRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEPTOACCRUALRATEROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeePtoAccrualRateRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeePtoAccrualRateRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeePtoAccrualRate object
        /// </summary>
        /// <param name="employeePtoAccrualRateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeePtoAccrualRateBase employeePtoAccrualRateObject, SqlDataReader reader, int start)
		{
			
				employeePtoAccrualRateObject.Id = reader.GetInt32( start + 0 );			
				employeePtoAccrualRateObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) employeePtoAccrualRateObject.PayType = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) employeePtoAccrualRateObject.MinimumDay = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) employeePtoAccrualRateObject.MaximumDay = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) employeePtoAccrualRateObject.AccrualRate = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) employeePtoAccrualRateObject.PtoHours = reader.GetDouble( start + 6 );			
				employeePtoAccrualRateObject.CreatedBy = reader.GetGuid( start + 7 );			
				employeePtoAccrualRateObject.CreatedDate = reader.GetDateTime( start + 8 );			
				employeePtoAccrualRateObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				employeePtoAccrualRateObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(employeePtoAccrualRateObject, reader, (start + 11));

			
			employeePtoAccrualRateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeePtoAccrualRate object
        /// </summary>
        /// <param name="employeePtoAccrualRateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeePtoAccrualRateBase employeePtoAccrualRateObject, SqlDataReader reader)
		{
			FillObject(employeePtoAccrualRateObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeePtoAccrualRate object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeePtoAccrualRate object</returns>
		private EmployeePtoAccrualRate GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeePtoAccrualRate employeePtoAccrualRateObject= new EmployeePtoAccrualRate();
					FillObject(employeePtoAccrualRateObject, reader);
					return employeePtoAccrualRateObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeePtoAccrualRate objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeePtoAccrualRate objects</returns>
		private EmployeePtoAccrualRateList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeePtoAccrualRate list
			EmployeePtoAccrualRateList list = new EmployeePtoAccrualRateList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeePtoAccrualRate employeePtoAccrualRateObject = new EmployeePtoAccrualRate();
					FillObject(employeePtoAccrualRateObject, reader);

					list.Add(employeePtoAccrualRateObject);
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