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
	public partial class EmployeeRouteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEROUTE = "InsertEmployeeRoute";
		private const string UPDATEEMPLOYEEROUTE = "UpdateEmployeeRoute";
		private const string DELETEEMPLOYEEROUTE = "DeleteEmployeeRoute";
		private const string GETEMPLOYEEROUTEBYID = "GetEmployeeRouteById";
		private const string GETALLEMPLOYEEROUTE = "GetAllEmployeeRoute";
		private const string GETPAGEDEMPLOYEEROUTE = "GetPagedEmployeeRoute";
		private const string GETEMPLOYEEROUTEMAXIMUMID = "GetEmployeeRouteMaximumId";
		private const string GETEMPLOYEEROUTEROWCOUNT = "GetEmployeeRouteRowCount";	
		private const string GETEMPLOYEEROUTEBYQUERY = "GetEmployeeRouteByQuery";
		#endregion
		
		#region Constructors
		public EmployeeRouteDataAccess(ClientContext context) : base(context) { }
		public EmployeeRouteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeRouteObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeRouteBase employeeRouteObject)
		{	
			AddParameter(cmd, pGuid(EmployeeRouteBase.Property_RouteId, employeeRouteObject.RouteId));
			AddParameter(cmd, pGuid(EmployeeRouteBase.Property_UserId, employeeRouteObject.UserId));
			AddParameter(cmd, pGuid(EmployeeRouteBase.Property_CreatedBy, employeeRouteObject.CreatedBy));
			AddParameter(cmd, pDateTime(EmployeeRouteBase.Property_CreatedDate, employeeRouteObject.CreatedDate));
			AddParameter(cmd, pGuid(EmployeeRouteBase.Property_UpdatedBy, employeeRouteObject.UpdatedBy));
			AddParameter(cmd, pDateTime(EmployeeRouteBase.Property_UpdatedDate, employeeRouteObject.UpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeRoute
        /// </summary>
        /// <param name="employeeRouteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeRouteBase employeeRouteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEROUTE);
	
				AddParameter(cmd, pInt32Out(EmployeeRouteBase.Property_Id));
				AddCommonParams(cmd, employeeRouteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeRouteObject.Id = (Int32)GetOutParameter(cmd, EmployeeRouteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeRouteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeRoute
        /// </summary>
        /// <param name="employeeRouteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeRouteBase employeeRouteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEROUTE);
				
				AddParameter(cmd, pInt32(EmployeeRouteBase.Property_Id, employeeRouteObject.Id));
				AddCommonParams(cmd, employeeRouteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeRouteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeRoute
        /// </summary>
        /// <param name="Id">Id of the EmployeeRoute object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEROUTE);	
				
				AddParameter(cmd, pInt32(EmployeeRouteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeRoute), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeRoute object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeRoute object to retrieve</param>
        /// <returns>EmployeeRoute object, null if not found</returns>
		public EmployeeRoute Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEROUTEBYID))
			{
				AddParameter( cmd, pInt32(EmployeeRouteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeRoute objects 
        /// </summary>
        /// <returns>A list of EmployeeRoute objects</returns>
		public EmployeeRouteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEROUTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeRoute objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeRoute objects</returns>
		public EmployeeRouteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEROUTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeRouteList _EmployeeRouteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeRouteList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeRoute objects by query String
        /// </summary>
        /// <returns>A list of EmployeeRoute objects</returns>
		public EmployeeRouteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEROUTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeRoute Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeRoute
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEROUTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeRoute Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeRoute
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeRouteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEROUTEROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeRouteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeRouteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeRoute object
        /// </summary>
        /// <param name="employeeRouteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeRouteBase employeeRouteObject, SqlDataReader reader, int start)
		{
			
				employeeRouteObject.Id = reader.GetInt32( start + 0 );			
				employeeRouteObject.RouteId = reader.GetGuid( start + 1 );			
				employeeRouteObject.UserId = reader.GetGuid( start + 2 );			
				employeeRouteObject.CreatedBy = reader.GetGuid( start + 3 );			
				employeeRouteObject.CreatedDate = reader.GetDateTime( start + 4 );			
				employeeRouteObject.UpdatedBy = reader.GetGuid( start + 5 );			
				employeeRouteObject.UpdatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(employeeRouteObject, reader, (start + 7));

			
			employeeRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeRoute object
        /// </summary>
        /// <param name="employeeRouteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeRouteBase employeeRouteObject, SqlDataReader reader)
		{
			FillObject(employeeRouteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeRoute object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeRoute object</returns>
		private EmployeeRoute GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeRoute employeeRouteObject= new EmployeeRoute();
					FillObject(employeeRouteObject, reader);
					return employeeRouteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeRoute objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeRoute objects</returns>
		private EmployeeRouteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeRoute list
			EmployeeRouteList list = new EmployeeRouteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeRoute employeeRouteObject = new EmployeeRoute();
					FillObject(employeeRouteObject, reader);

					list.Add(employeeRouteObject);
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
