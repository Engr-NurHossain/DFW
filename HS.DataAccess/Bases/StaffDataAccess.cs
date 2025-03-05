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
	public partial class StaffDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSTAFF = "InsertStaff";
		private const string UPDATESTAFF = "UpdateStaff";
		private const string DELETESTAFF = "DeleteStaff";
		private const string GETSTAFFBYID = "GetStaffById";
		private const string GETALLSTAFF = "GetAllStaff";
		private const string GETPAGEDSTAFF = "GetPagedStaff";
		private const string GETSTAFFMAXIMUMID = "GetStaffMaximumId";
		private const string GETSTAFFROWCOUNT = "GetStaffRowCount";	
		private const string GETSTAFFBYQUERY = "GetStaffByQuery";
        #endregion

        #region Constructors
        public StaffDataAccess() { }
		public StaffDataAccess(ClientContext context) : base(context) { }
		public StaffDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="staffObject"></param>
		private void AddCommonParams(SqlCommand cmd, StaffBase staffObject)
		{	
			AddParameter(cmd, pNVarChar(StaffBase.Property_UserName, 250, staffObject.UserName));
			AddParameter(cmd, pBool(StaffBase.Property_IsActive, staffObject.IsActive));
			AddParameter(cmd, pNVarChar(StaffBase.Property_LastUpdatedBy, 50, staffObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(StaffBase.Property_LastUpdatedDate, staffObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Staff
        /// </summary>
        /// <param name="staffObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(StaffBase staffObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSTAFF);
	
				AddParameter(cmd, pInt32Out(StaffBase.Property_Id));
				AddCommonParams(cmd, staffObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					staffObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					staffObject.Id = (Int32)GetOutParameter(cmd, StaffBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(staffObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Staff
        /// </summary>
        /// <param name="staffObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(StaffBase staffObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESTAFF);
				
				AddParameter(cmd, pInt32(StaffBase.Property_Id, staffObject.Id));
				AddCommonParams(cmd, staffObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					staffObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(staffObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Staff
        /// </summary>
        /// <param name="Id">Id of the Staff object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESTAFF);	
				
				AddParameter(cmd, pInt32(StaffBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Staff), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Staff object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Staff object to retrieve</param>
        /// <returns>Staff object, null if not found</returns>
		public Staff Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSTAFFBYID))
			{
				AddParameter( cmd, pInt32(StaffBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Staff objects 
        /// </summary>
        /// <returns>A list of Staff objects</returns>
		public StaffList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSTAFF))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Staff objects by PageRequest
        /// </summary>
        /// <returns>A list of Staff objects</returns>
		public StaffList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSTAFF))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				StaffList _StaffList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _StaffList;
			}
		}
		
		/// <summary>
        /// Retrieves all Staff objects by query String
        /// </summary>
        /// <returns>A list of Staff objects</returns>
		public StaffList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSTAFFBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Staff Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Staff
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSTAFFMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Staff Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Staff
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _StaffRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSTAFFROWCOUNT))
			{
				SqlDataReader reader;
				_StaffRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _StaffRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Staff object
        /// </summary>
        /// <param name="staffObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(StaffBase staffObject, SqlDataReader reader, int start)
		{
			
				staffObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) staffObject.UserName = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) staffObject.IsActive = reader.GetBoolean( start + 2 );			
				if(!reader.IsDBNull(3)) staffObject.LastUpdatedBy = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) staffObject.LastUpdatedDate = reader.GetDateTime( start + 4 );			
			FillBaseObject(staffObject, reader, (start + 5));

			
			staffObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Staff object
        /// </summary>
        /// <param name="staffObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(StaffBase staffObject, SqlDataReader reader)
		{
			FillObject(staffObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Staff object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Staff object</returns>
		private Staff GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Staff staffObject= new Staff();
					FillObject(staffObject, reader);
					return staffObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Staff objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Staff objects</returns>
		private StaffList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Staff list
			StaffList list = new StaffList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Staff staffObject = new Staff();
					FillObject(staffObject, reader);

					list.Add(staffObject);
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
