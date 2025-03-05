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
	public partial class EmployeeOccurencesDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEOCCURENCES = "InsertEmployeeOccurences";
		private const string UPDATEEMPLOYEEOCCURENCES = "UpdateEmployeeOccurences";
		private const string DELETEEMPLOYEEOCCURENCES = "DeleteEmployeeOccurences";
		private const string GETEMPLOYEEOCCURENCESBYID = "GetEmployeeOccurencesById";
		private const string GETALLEMPLOYEEOCCURENCES = "GetAllEmployeeOccurences";
		private const string GETPAGEDEMPLOYEEOCCURENCES = "GetPagedEmployeeOccurences";
		private const string GETEMPLOYEEOCCURENCESMAXIMUMID = "GetEmployeeOccurencesMaximumId";
		private const string GETEMPLOYEEOCCURENCESROWCOUNT = "GetEmployeeOccurencesRowCount";	
		private const string GETEMPLOYEEOCCURENCESBYQUERY = "GetEmployeeOccurencesByQuery";
		#endregion
		
		#region Constructors
		public EmployeeOccurencesDataAccess(ClientContext context) : base(context) { }
		public EmployeeOccurencesDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeOccurencesObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeOccurencesBase employeeOccurencesObject)
		{	
			AddParameter(cmd, pGuid(EmployeeOccurencesBase.Property_UserId, employeeOccurencesObject.UserId));
			AddParameter(cmd, pDateTime(EmployeeOccurencesBase.Property_OccurenceDate, employeeOccurencesObject.OccurenceDate));
			AddParameter(cmd, pDouble(EmployeeOccurencesBase.Property_Amount, employeeOccurencesObject.Amount));
			AddParameter(cmd, pNVarChar(EmployeeOccurencesBase.Property_Notes, employeeOccurencesObject.Notes));
			AddParameter(cmd, pGuid(EmployeeOccurencesBase.Property_CreatedByUid, employeeOccurencesObject.CreatedByUid));
			AddParameter(cmd, pDateTime(EmployeeOccurencesBase.Property_CreatedDate, employeeOccurencesObject.CreatedDate));
			AddParameter(cmd, pGuid(EmployeeOccurencesBase.Property_LastUpdatedByUid, employeeOccurencesObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(EmployeeOccurencesBase.Property_LastUpdatedDate, employeeOccurencesObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeOccurences
        /// </summary>
        /// <param name="employeeOccurencesObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeOccurencesBase employeeOccurencesObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEOCCURENCES);
	
				AddParameter(cmd, pInt32Out(EmployeeOccurencesBase.Property_Id));
				AddCommonParams(cmd, employeeOccurencesObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeOccurencesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeOccurencesObject.Id = (Int32)GetOutParameter(cmd, EmployeeOccurencesBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeOccurencesObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeOccurences
        /// </summary>
        /// <param name="employeeOccurencesObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeOccurencesBase employeeOccurencesObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEOCCURENCES);
				
				AddParameter(cmd, pInt32(EmployeeOccurencesBase.Property_Id, employeeOccurencesObject.Id));
				AddCommonParams(cmd, employeeOccurencesObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeOccurencesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeOccurencesObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeOccurences
        /// </summary>
        /// <param name="Id">Id of the EmployeeOccurences object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEOCCURENCES);	
				
				AddParameter(cmd, pInt32(EmployeeOccurencesBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeOccurences), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeOccurences object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeOccurences object to retrieve</param>
        /// <returns>EmployeeOccurences object, null if not found</returns>
		public EmployeeOccurences Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEOCCURENCESBYID))
			{
				AddParameter( cmd, pInt32(EmployeeOccurencesBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeOccurences objects 
        /// </summary>
        /// <returns>A list of EmployeeOccurences objects</returns>
		public EmployeeOccurencesList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEOCCURENCES))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeOccurences objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeOccurences objects</returns>
		public EmployeeOccurencesList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEOCCURENCES))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeOccurencesList _EmployeeOccurencesList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeOccurencesList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeOccurences objects by query String
        /// </summary>
        /// <returns>A list of EmployeeOccurences objects</returns>
		public EmployeeOccurencesList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEOCCURENCESBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeOccurences Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeOccurences
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEOCCURENCESMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeOccurences Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeOccurences
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeOccurencesRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEOCCURENCESROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeOccurencesRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeOccurencesRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeOccurences object
        /// </summary>
        /// <param name="employeeOccurencesObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeOccurencesBase employeeOccurencesObject, SqlDataReader reader, int start)
		{
			
				employeeOccurencesObject.Id = reader.GetInt32( start + 0 );			
				employeeOccurencesObject.UserId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) employeeOccurencesObject.OccurenceDate = reader.GetDateTime( start + 2 );			
				if(!reader.IsDBNull(3)) employeeOccurencesObject.Amount = reader.GetDouble( start + 3 );			
				if(!reader.IsDBNull(4)) employeeOccurencesObject.Notes = reader.GetString( start + 4 );			
				employeeOccurencesObject.CreatedByUid = reader.GetGuid( start + 5 );			
				employeeOccurencesObject.CreatedDate = reader.GetDateTime( start + 6 );			
				employeeOccurencesObject.LastUpdatedByUid = reader.GetGuid( start + 7 );			
				employeeOccurencesObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
			FillBaseObject(employeeOccurencesObject, reader, (start + 9));

			
			employeeOccurencesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeOccurences object
        /// </summary>
        /// <param name="employeeOccurencesObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeOccurencesBase employeeOccurencesObject, SqlDataReader reader)
		{
			FillObject(employeeOccurencesObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeOccurences object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeOccurences object</returns>
		private EmployeeOccurences GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeOccurences employeeOccurencesObject= new EmployeeOccurences();
					FillObject(employeeOccurencesObject, reader);
					return employeeOccurencesObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeOccurences objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeOccurences objects</returns>
		private EmployeeOccurencesList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeOccurences list
			EmployeeOccurencesList list = new EmployeeOccurencesList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeOccurences employeeOccurencesObject = new EmployeeOccurences();
					FillObject(employeeOccurencesObject, reader);

					list.Add(employeeOccurencesObject);
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
