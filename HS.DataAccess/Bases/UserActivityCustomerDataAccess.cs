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
	public partial class UserActivityCustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTUSERACTIVITYCUSTOMER = "InsertUserActivityCustomer";
		private const string UPDATEUSERACTIVITYCUSTOMER = "UpdateUserActivityCustomer";
		private const string DELETEUSERACTIVITYCUSTOMER = "DeleteUserActivityCustomer";
		private const string GETUSERACTIVITYCUSTOMERBYID = "GetUserActivityCustomerById";
		private const string GETALLUSERACTIVITYCUSTOMER = "GetAllUserActivityCustomer";
		private const string GETPAGEDUSERACTIVITYCUSTOMER = "GetPagedUserActivityCustomer";
		private const string GETUSERACTIVITYCUSTOMERMAXIMUMID = "GetUserActivityCustomerMaximumId";
		private const string GETUSERACTIVITYCUSTOMERROWCOUNT = "GetUserActivityCustomerRowCount";	
		private const string GETUSERACTIVITYCUSTOMERBYQUERY = "GetUserActivityCustomerByQuery";
		#endregion
		
		#region Constructors
		public UserActivityCustomerDataAccess(ClientContext context) : base(context) { }
		public UserActivityCustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="userActivityCustomerObject"></param>
		private void AddCommonParams(SqlCommand cmd, UserActivityCustomerBase userActivityCustomerObject)
		{	
			AddParameter(cmd, pGuid(UserActivityCustomerBase.Property_CustomerId, userActivityCustomerObject.CustomerId));
			AddParameter(cmd, pGuid(UserActivityCustomerBase.Property_ActivityId, userActivityCustomerObject.ActivityId));
			AddParameter(cmd, pNVarChar(UserActivityCustomerBase.Property_RefId, 100, userActivityCustomerObject.RefId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts UserActivityCustomer
        /// </summary>
        /// <param name="userActivityCustomerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(UserActivityCustomerBase userActivityCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTUSERACTIVITYCUSTOMER);
	
				AddParameter(cmd, pInt32Out(UserActivityCustomerBase.Property_Id));
				AddCommonParams(cmd, userActivityCustomerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					userActivityCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					userActivityCustomerObject.Id = (Int32)GetOutParameter(cmd, UserActivityCustomerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(userActivityCustomerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates UserActivityCustomer
        /// </summary>
        /// <param name="userActivityCustomerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(UserActivityCustomerBase userActivityCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEUSERACTIVITYCUSTOMER);
				
				AddParameter(cmd, pInt32(UserActivityCustomerBase.Property_Id, userActivityCustomerObject.Id));
				AddCommonParams(cmd, userActivityCustomerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					userActivityCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(userActivityCustomerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes UserActivityCustomer
        /// </summary>
        /// <param name="Id">Id of the UserActivityCustomer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEUSERACTIVITYCUSTOMER);	
				
				AddParameter(cmd, pInt32(UserActivityCustomerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(UserActivityCustomer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves UserActivityCustomer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the UserActivityCustomer object to retrieve</param>
        /// <returns>UserActivityCustomer object, null if not found</returns>
		public UserActivityCustomer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERACTIVITYCUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(UserActivityCustomerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all UserActivityCustomer objects 
        /// </summary>
        /// <returns>A list of UserActivityCustomer objects</returns>
		public UserActivityCustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLUSERACTIVITYCUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all UserActivityCustomer objects by PageRequest
        /// </summary>
        /// <returns>A list of UserActivityCustomer objects</returns>
		public UserActivityCustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDUSERACTIVITYCUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				UserActivityCustomerList _UserActivityCustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _UserActivityCustomerList;
			}
		}
		
		/// <summary>
        /// Retrieves all UserActivityCustomer objects by query String
        /// </summary>
        /// <returns>A list of UserActivityCustomer objects</returns>
		public UserActivityCustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERACTIVITYCUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get UserActivityCustomer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of UserActivityCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERACTIVITYCUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get UserActivityCustomer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of UserActivityCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _UserActivityCustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERACTIVITYCUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_UserActivityCustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _UserActivityCustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills UserActivityCustomer object
        /// </summary>
        /// <param name="userActivityCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(UserActivityCustomerBase userActivityCustomerObject, SqlDataReader reader, int start)
		{
			
				userActivityCustomerObject.Id = reader.GetInt32( start + 0 );			
				userActivityCustomerObject.CustomerId = reader.GetGuid( start + 1 );			
				userActivityCustomerObject.ActivityId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) userActivityCustomerObject.RefId = reader.GetString( start + 3 );			
			FillBaseObject(userActivityCustomerObject, reader, (start + 4));

			
			userActivityCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills UserActivityCustomer object
        /// </summary>
        /// <param name="userActivityCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(UserActivityCustomerBase userActivityCustomerObject, SqlDataReader reader)
		{
			FillObject(userActivityCustomerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves UserActivityCustomer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>UserActivityCustomer object</returns>
		private UserActivityCustomer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					UserActivityCustomer userActivityCustomerObject= new UserActivityCustomer();
					FillObject(userActivityCustomerObject, reader);
					return userActivityCustomerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of UserActivityCustomer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of UserActivityCustomer objects</returns>
		private UserActivityCustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//UserActivityCustomer list
			UserActivityCustomerList list = new UserActivityCustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					UserActivityCustomer userActivityCustomerObject = new UserActivityCustomer();
					FillObject(userActivityCustomerObject, reader);

					list.Add(userActivityCustomerObject);
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
