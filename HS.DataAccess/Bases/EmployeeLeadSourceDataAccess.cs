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
	public partial class EmployeeLeadSourceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEELEADSOURCE = "InsertEmployeeLeadSource";
		private const string UPDATEEMPLOYEELEADSOURCE = "UpdateEmployeeLeadSource";
		private const string DELETEEMPLOYEELEADSOURCE = "DeleteEmployeeLeadSource";
		private const string GETEMPLOYEELEADSOURCEBYID = "GetEmployeeLeadSourceById";
		private const string GETALLEMPLOYEELEADSOURCE = "GetAllEmployeeLeadSource";
		private const string GETPAGEDEMPLOYEELEADSOURCE = "GetPagedEmployeeLeadSource";
		private const string GETEMPLOYEELEADSOURCEMAXIMUMID = "GetEmployeeLeadSourceMaximumId";
		private const string GETEMPLOYEELEADSOURCEROWCOUNT = "GetEmployeeLeadSourceRowCount";	
		private const string GETEMPLOYEELEADSOURCEBYQUERY = "GetEmployeeLeadSourceByQuery";
		#endregion
		
		#region Constructors
		public EmployeeLeadSourceDataAccess(ClientContext context) : base(context) { }
		public EmployeeLeadSourceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeLeadSourceObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeLeadSourceBase employeeLeadSourceObject)
		{	
			AddParameter(cmd, pGuid(EmployeeLeadSourceBase.Property_EmployeeId, employeeLeadSourceObject.EmployeeId));
			AddParameter(cmd, pNVarChar(EmployeeLeadSourceBase.Property_LeadSource, 50, employeeLeadSourceObject.LeadSource));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeLeadSource
        /// </summary>
        /// <param name="employeeLeadSourceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeLeadSourceBase employeeLeadSourceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEELEADSOURCE);
	
				AddParameter(cmd, pInt32Out(EmployeeLeadSourceBase.Property_Id));
				AddCommonParams(cmd, employeeLeadSourceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeLeadSourceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeLeadSourceObject.Id = (Int32)GetOutParameter(cmd, EmployeeLeadSourceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeLeadSourceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeLeadSource
        /// </summary>
        /// <param name="employeeLeadSourceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeLeadSourceBase employeeLeadSourceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEELEADSOURCE);
				
				AddParameter(cmd, pInt32(EmployeeLeadSourceBase.Property_Id, employeeLeadSourceObject.Id));
				AddCommonParams(cmd, employeeLeadSourceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeLeadSourceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeLeadSourceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeLeadSource
        /// </summary>
        /// <param name="Id">Id of the EmployeeLeadSource object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEELEADSOURCE);	
				
				AddParameter(cmd, pInt32(EmployeeLeadSourceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeLeadSource), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeLeadSource object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeLeadSource object to retrieve</param>
        /// <returns>EmployeeLeadSource object, null if not found</returns>
		public EmployeeLeadSource Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEELEADSOURCEBYID))
			{
				AddParameter( cmd, pInt32(EmployeeLeadSourceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeLeadSource objects 
        /// </summary>
        /// <returns>A list of EmployeeLeadSource objects</returns>
		public EmployeeLeadSourceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEELEADSOURCE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeLeadSource objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeLeadSource objects</returns>
		public EmployeeLeadSourceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEELEADSOURCE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeLeadSourceList _EmployeeLeadSourceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeLeadSourceList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeLeadSource objects by query String
        /// </summary>
        /// <returns>A list of EmployeeLeadSource objects</returns>
		public EmployeeLeadSourceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEELEADSOURCEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeLeadSource Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeLeadSource
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEELEADSOURCEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeLeadSource Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeLeadSource
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeLeadSourceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEELEADSOURCEROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeLeadSourceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeLeadSourceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeLeadSource object
        /// </summary>
        /// <param name="employeeLeadSourceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeLeadSourceBase employeeLeadSourceObject, SqlDataReader reader, int start)
		{
			
				employeeLeadSourceObject.Id = reader.GetInt32( start + 0 );			
				employeeLeadSourceObject.EmployeeId = reader.GetGuid( start + 1 );			
				employeeLeadSourceObject.LeadSource = reader.GetString( start + 2 );			
			FillBaseObject(employeeLeadSourceObject, reader, (start + 3));

			
			employeeLeadSourceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeLeadSource object
        /// </summary>
        /// <param name="employeeLeadSourceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeLeadSourceBase employeeLeadSourceObject, SqlDataReader reader)
		{
			FillObject(employeeLeadSourceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeLeadSource object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeLeadSource object</returns>
		private EmployeeLeadSource GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeLeadSource employeeLeadSourceObject= new EmployeeLeadSource();
					FillObject(employeeLeadSourceObject, reader);
					return employeeLeadSourceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeLeadSource objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeLeadSource objects</returns>
		private EmployeeLeadSourceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeLeadSource list
			EmployeeLeadSourceList list = new EmployeeLeadSourceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeLeadSource employeeLeadSourceObject = new EmployeeLeadSource();
					FillObject(employeeLeadSourceObject, reader);

					list.Add(employeeLeadSourceObject);
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
