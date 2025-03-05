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
	public partial class EmployeeWriteUpDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEWRITEUP = "InsertEmployeeWriteUp";
		private const string UPDATEEMPLOYEEWRITEUP = "UpdateEmployeeWriteUp";
		private const string DELETEEMPLOYEEWRITEUP = "DeleteEmployeeWriteUp";
		private const string GETEMPLOYEEWRITEUPBYID = "GetEmployeeWriteUpById";
		private const string GETALLEMPLOYEEWRITEUP = "GetAllEmployeeWriteUp";
		private const string GETPAGEDEMPLOYEEWRITEUP = "GetPagedEmployeeWriteUp";
		private const string GETEMPLOYEEWRITEUPMAXIMUMID = "GetEmployeeWriteUpMaximumId";
		private const string GETEMPLOYEEWRITEUPROWCOUNT = "GetEmployeeWriteUpRowCount";	
		private const string GETEMPLOYEEWRITEUPBYQUERY = "GetEmployeeWriteUpByQuery";
		#endregion
		
		#region Constructors
		public EmployeeWriteUpDataAccess(ClientContext context) : base(context) { }
		public EmployeeWriteUpDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeWriteUpObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeWriteUpBase employeeWriteUpObject)
		{	
			AddParameter(cmd, pGuid(EmployeeWriteUpBase.Property_WriteupId, employeeWriteUpObject.WriteupId));
			AddParameter(cmd, pGuid(EmployeeWriteUpBase.Property_UserId, employeeWriteUpObject.UserId));
			AddParameter(cmd, pGuid(EmployeeWriteUpBase.Property_Supervisor, employeeWriteUpObject.Supervisor));
			AddParameter(cmd, pDateTime(EmployeeWriteUpBase.Property_WriteupDate, employeeWriteUpObject.WriteupDate));
			AddParameter(cmd, pNVarChar(EmployeeWriteUpBase.Property_Category, 50, employeeWriteUpObject.Category));
			AddParameter(cmd, pNVarChar(EmployeeWriteUpBase.Property_Description, employeeWriteUpObject.Description));
			AddParameter(cmd, pGuid(EmployeeWriteUpBase.Property_CreatedBy, employeeWriteUpObject.CreatedBy));
			AddParameter(cmd, pDateTime(EmployeeWriteUpBase.Property_CreatedDate, employeeWriteUpObject.CreatedDate));
			AddParameter(cmd, pNVarChar(EmployeeWriteUpBase.Property_FileName, 150, employeeWriteUpObject.FileName));
			AddParameter(cmd, pNVarChar(EmployeeWriteUpBase.Property_FilePath, employeeWriteUpObject.FilePath));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeWriteUp
        /// </summary>
        /// <param name="employeeWriteUpObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeWriteUpBase employeeWriteUpObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEWRITEUP);
	
				AddParameter(cmd, pInt32Out(EmployeeWriteUpBase.Property_Id));
				AddCommonParams(cmd, employeeWriteUpObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeWriteUpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeWriteUpObject.Id = (Int32)GetOutParameter(cmd, EmployeeWriteUpBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeWriteUpObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeWriteUp
        /// </summary>
        /// <param name="employeeWriteUpObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeWriteUpBase employeeWriteUpObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEWRITEUP);
				
				AddParameter(cmd, pInt32(EmployeeWriteUpBase.Property_Id, employeeWriteUpObject.Id));
				AddCommonParams(cmd, employeeWriteUpObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeWriteUpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeWriteUpObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeWriteUp
        /// </summary>
        /// <param name="Id">Id of the EmployeeWriteUp object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEWRITEUP);	
				
				AddParameter(cmd, pInt32(EmployeeWriteUpBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeWriteUp), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeWriteUp object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeWriteUp object to retrieve</param>
        /// <returns>EmployeeWriteUp object, null if not found</returns>
		public EmployeeWriteUp Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEWRITEUPBYID))
			{
				AddParameter( cmd, pInt32(EmployeeWriteUpBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeWriteUp objects 
        /// </summary>
        /// <returns>A list of EmployeeWriteUp objects</returns>
		public EmployeeWriteUpList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEWRITEUP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeWriteUp objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeWriteUp objects</returns>
		public EmployeeWriteUpList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEWRITEUP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeWriteUpList _EmployeeWriteUpList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeWriteUpList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeWriteUp objects by query String
        /// </summary>
        /// <returns>A list of EmployeeWriteUp objects</returns>
		public EmployeeWriteUpList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEWRITEUPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeWriteUp Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeWriteUp
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEWRITEUPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeWriteUp Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeWriteUp
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeWriteUpRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEWRITEUPROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeWriteUpRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeWriteUpRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeWriteUp object
        /// </summary>
        /// <param name="employeeWriteUpObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeWriteUpBase employeeWriteUpObject, SqlDataReader reader, int start)
		{
			
				employeeWriteUpObject.Id = reader.GetInt32( start + 0 );			
				employeeWriteUpObject.WriteupId = reader.GetGuid( start + 1 );			
				employeeWriteUpObject.UserId = reader.GetGuid( start + 2 );			
				employeeWriteUpObject.Supervisor = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) employeeWriteUpObject.WriteupDate = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) employeeWriteUpObject.Category = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) employeeWriteUpObject.Description = reader.GetString( start + 6 );			
				employeeWriteUpObject.CreatedBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) employeeWriteUpObject.CreatedDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) employeeWriteUpObject.FileName = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) employeeWriteUpObject.FilePath = reader.GetString( start + 10 );			
			FillBaseObject(employeeWriteUpObject, reader, (start + 11));

			
			employeeWriteUpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeWriteUp object
        /// </summary>
        /// <param name="employeeWriteUpObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeWriteUpBase employeeWriteUpObject, SqlDataReader reader)
		{
			FillObject(employeeWriteUpObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeWriteUp object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeWriteUp object</returns>
		private EmployeeWriteUp GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeWriteUp employeeWriteUpObject= new EmployeeWriteUp();
					FillObject(employeeWriteUpObject, reader);
					return employeeWriteUpObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeWriteUp objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeWriteUp objects</returns>
		private EmployeeWriteUpList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeWriteUp list
			EmployeeWriteUpList list = new EmployeeWriteUpList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeWriteUp employeeWriteUpObject = new EmployeeWriteUp();
					FillObject(employeeWriteUpObject, reader);

					list.Add(employeeWriteUpObject);
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
