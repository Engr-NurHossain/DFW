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
	public partial class EmployeeNoteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEENOTE = "InsertEmployeeNote";
		private const string UPDATEEMPLOYEENOTE = "UpdateEmployeeNote";
		private const string DELETEEMPLOYEENOTE = "DeleteEmployeeNote";
		private const string GETEMPLOYEENOTEBYID = "GetEmployeeNoteById";
		private const string GETALLEMPLOYEENOTE = "GetAllEmployeeNote";
		private const string GETPAGEDEMPLOYEENOTE = "GetPagedEmployeeNote";
		private const string GETEMPLOYEENOTEMAXIMUMID = "GetEmployeeNoteMaximumId";
		private const string GETEMPLOYEENOTEROWCOUNT = "GetEmployeeNoteRowCount";	
		private const string GETEMPLOYEENOTEBYQUERY = "GetEmployeeNoteByQuery";
		#endregion
		
		#region Constructors
		public EmployeeNoteDataAccess(ClientContext context) : base(context) { }
		public EmployeeNoteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeNoteObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeNoteBase employeeNoteObject)
		{	
			AddParameter(cmd, pNVarChar(EmployeeNoteBase.Property_Notes, employeeNoteObject.Notes));
			AddParameter(cmd, pDateTime(EmployeeNoteBase.Property_ReminderDate, employeeNoteObject.ReminderDate));
			AddParameter(cmd, pGuid(EmployeeNoteBase.Property_EmployeeId, employeeNoteObject.EmployeeId));
			AddParameter(cmd, pGuid(EmployeeNoteBase.Property_CompanyId, employeeNoteObject.CompanyId));
			AddParameter(cmd, pDateTime(EmployeeNoteBase.Property_CreatedDate, employeeNoteObject.CreatedDate));
			AddParameter(cmd, pBool(EmployeeNoteBase.Property_IsEmail, employeeNoteObject.IsEmail));
			AddParameter(cmd, pBool(EmployeeNoteBase.Property_IsText, employeeNoteObject.IsText));
			AddParameter(cmd, pBool(EmployeeNoteBase.Property_IsShedule, employeeNoteObject.IsShedule));
			AddParameter(cmd, pBool(EmployeeNoteBase.Property_IsFollowUp, employeeNoteObject.IsFollowUp));
			AddParameter(cmd, pBool(EmployeeNoteBase.Property_IsActive, employeeNoteObject.IsActive));
			AddParameter(cmd, pNVarChar(EmployeeNoteBase.Property_CreatedBy, 50, employeeNoteObject.CreatedBy));
			AddParameter(cmd, pBool(EmployeeNoteBase.Property_IsClose, employeeNoteObject.IsClose));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeNote
        /// </summary>
        /// <param name="employeeNoteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeNoteBase employeeNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEENOTE);
	
				AddParameter(cmd, pInt32Out(EmployeeNoteBase.Property_Id));
				AddCommonParams(cmd, employeeNoteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeNoteObject.Id = (Int32)GetOutParameter(cmd, EmployeeNoteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeNoteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeNote
        /// </summary>
        /// <param name="employeeNoteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeNoteBase employeeNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEENOTE);
				
				AddParameter(cmd, pInt32(EmployeeNoteBase.Property_Id, employeeNoteObject.Id));
				AddCommonParams(cmd, employeeNoteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeNoteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeNote
        /// </summary>
        /// <param name="Id">Id of the EmployeeNote object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEENOTE);	
				
				AddParameter(cmd, pInt32(EmployeeNoteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeNote), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeNote object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeNote object to retrieve</param>
        /// <returns>EmployeeNote object, null if not found</returns>
		public EmployeeNote Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEENOTEBYID))
			{
				AddParameter( cmd, pInt32(EmployeeNoteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeNote objects 
        /// </summary>
        /// <returns>A list of EmployeeNote objects</returns>
		public EmployeeNoteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEENOTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeNote objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeNote objects</returns>
		public EmployeeNoteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEENOTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeNoteList _EmployeeNoteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeNoteList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeNote objects by query String
        /// </summary>
        /// <returns>A list of EmployeeNote objects</returns>
		public EmployeeNoteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEENOTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeNote Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEENOTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeNote Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeNoteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEENOTEROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeNoteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeNoteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeNote object
        /// </summary>
        /// <param name="employeeNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeNoteBase employeeNoteObject, SqlDataReader reader, int start)
		{
			
				employeeNoteObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) employeeNoteObject.Notes = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) employeeNoteObject.ReminderDate = reader.GetDateTime( start + 2 );			
				employeeNoteObject.EmployeeId = reader.GetGuid( start + 3 );			
				employeeNoteObject.CompanyId = reader.GetGuid( start + 4 );			
				employeeNoteObject.CreatedDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) employeeNoteObject.IsEmail = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) employeeNoteObject.IsText = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) employeeNoteObject.IsShedule = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) employeeNoteObject.IsFollowUp = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) employeeNoteObject.IsActive = reader.GetBoolean( start + 10 );			
				if(!reader.IsDBNull(11)) employeeNoteObject.CreatedBy = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) employeeNoteObject.IsClose = reader.GetBoolean( start + 12 );			
			FillBaseObject(employeeNoteObject, reader, (start + 13));

			
			employeeNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeNote object
        /// </summary>
        /// <param name="employeeNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeNoteBase employeeNoteObject, SqlDataReader reader)
		{
			FillObject(employeeNoteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeNote object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeNote object</returns>
		private EmployeeNote GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeNote employeeNoteObject= new EmployeeNote();
					FillObject(employeeNoteObject, reader);
					return employeeNoteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeNote objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeNote objects</returns>
		private EmployeeNoteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeNote list
			EmployeeNoteList list = new EmployeeNoteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeNote employeeNoteObject = new EmployeeNote();
					FillObject(employeeNoteObject, reader);

					list.Add(employeeNoteObject);
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
