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
	public partial class UserCompanyDeviceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTUSERCOMPANYDEVICE = "InsertUserCompanyDevice";
		private const string UPDATEUSERCOMPANYDEVICE = "UpdateUserCompanyDevice";
		private const string DELETEUSERCOMPANYDEVICE = "DeleteUserCompanyDevice";
		private const string GETUSERCOMPANYDEVICEBYID = "GetUserCompanyDeviceById";
		private const string GETALLUSERCOMPANYDEVICE = "GetAllUserCompanyDevice";
		private const string GETPAGEDUSERCOMPANYDEVICE = "GetPagedUserCompanyDevice";
		private const string GETUSERCOMPANYDEVICEMAXIMUMID = "GetUserCompanyDeviceMaximumId";
		private const string GETUSERCOMPANYDEVICEROWCOUNT = "GetUserCompanyDeviceRowCount";	
		private const string GETUSERCOMPANYDEVICEBYQUERY = "GetUserCompanyDeviceByQuery";
        #endregion

        #region Constructors
        public UserCompanyDeviceDataAccess(string ConStr) : base(ConStr) { }
        public UserCompanyDeviceDataAccess(ClientContext context) : base(context) { }
		public UserCompanyDeviceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="userCompanyDeviceObject"></param>
		private void AddCommonParams(SqlCommand cmd, UserCompanyDeviceBase userCompanyDeviceObject)
		{	
			AddParameter(cmd, pGuid(UserCompanyDeviceBase.Property_CompanyId, userCompanyDeviceObject.CompanyId));
			AddParameter(cmd, pGuid(UserCompanyDeviceBase.Property_UserId, userCompanyDeviceObject.UserId));
			AddParameter(cmd, pNVarChar(UserCompanyDeviceBase.Property_DeviceId, userCompanyDeviceObject.DeviceId));
			AddParameter(cmd, pBool(UserCompanyDeviceBase.Property_IsActive, userCompanyDeviceObject.IsActive));
			AddParameter(cmd, pDateTime(UserCompanyDeviceBase.Property_CreatedDate, userCompanyDeviceObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts UserCompanyDevice
        /// </summary>
        /// <param name="userCompanyDeviceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(UserCompanyDeviceBase userCompanyDeviceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTUSERCOMPANYDEVICE);
	
				AddParameter(cmd, pInt32Out(UserCompanyDeviceBase.Property_Id));
				AddCommonParams(cmd, userCompanyDeviceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					userCompanyDeviceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					userCompanyDeviceObject.Id = (Int32)GetOutParameter(cmd, UserCompanyDeviceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(userCompanyDeviceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates UserCompanyDevice
        /// </summary>
        /// <param name="userCompanyDeviceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(UserCompanyDeviceBase userCompanyDeviceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEUSERCOMPANYDEVICE);
				
				AddParameter(cmd, pInt32(UserCompanyDeviceBase.Property_Id, userCompanyDeviceObject.Id));
				AddCommonParams(cmd, userCompanyDeviceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					userCompanyDeviceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(userCompanyDeviceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes UserCompanyDevice
        /// </summary>
        /// <param name="Id">Id of the UserCompanyDevice object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEUSERCOMPANYDEVICE);	
				
				AddParameter(cmd, pInt32(UserCompanyDeviceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(UserCompanyDevice), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves UserCompanyDevice object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the UserCompanyDevice object to retrieve</param>
        /// <returns>UserCompanyDevice object, null if not found</returns>
		public UserCompanyDevice Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERCOMPANYDEVICEBYID))
			{
				AddParameter( cmd, pInt32(UserCompanyDeviceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all UserCompanyDevice objects 
        /// </summary>
        /// <returns>A list of UserCompanyDevice objects</returns>
		public UserCompanyDeviceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLUSERCOMPANYDEVICE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all UserCompanyDevice objects by PageRequest
        /// </summary>
        /// <returns>A list of UserCompanyDevice objects</returns>
		public UserCompanyDeviceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDUSERCOMPANYDEVICE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				UserCompanyDeviceList _UserCompanyDeviceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _UserCompanyDeviceList;
			}
		}
		
		/// <summary>
        /// Retrieves all UserCompanyDevice objects by query String
        /// </summary>
        /// <returns>A list of UserCompanyDevice objects</returns>
		public UserCompanyDeviceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERCOMPANYDEVICEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get UserCompanyDevice Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of UserCompanyDevice
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERCOMPANYDEVICEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get UserCompanyDevice Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of UserCompanyDevice
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _UserCompanyDeviceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERCOMPANYDEVICEROWCOUNT))
			{
				SqlDataReader reader;
				_UserCompanyDeviceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _UserCompanyDeviceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills UserCompanyDevice object
        /// </summary>
        /// <param name="userCompanyDeviceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(UserCompanyDeviceBase userCompanyDeviceObject, SqlDataReader reader, int start)
		{
			
				userCompanyDeviceObject.Id = reader.GetInt32( start + 0 );			
				userCompanyDeviceObject.CompanyId = reader.GetGuid( start + 1 );			
				userCompanyDeviceObject.UserId = reader.GetGuid( start + 2 );			
				userCompanyDeviceObject.DeviceId = reader.GetString( start + 3 );			
				userCompanyDeviceObject.IsActive = reader.GetBoolean( start + 4 );			
				userCompanyDeviceObject.CreatedDate = reader.GetDateTime( start + 5 );			
			FillBaseObject(userCompanyDeviceObject, reader, (start + 6));

			
			userCompanyDeviceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills UserCompanyDevice object
        /// </summary>
        /// <param name="userCompanyDeviceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(UserCompanyDeviceBase userCompanyDeviceObject, SqlDataReader reader)
		{
			FillObject(userCompanyDeviceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves UserCompanyDevice object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>UserCompanyDevice object</returns>
		private UserCompanyDevice GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					UserCompanyDevice userCompanyDeviceObject= new UserCompanyDevice();
					FillObject(userCompanyDeviceObject, reader);
					return userCompanyDeviceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of UserCompanyDevice objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of UserCompanyDevice objects</returns>
		private UserCompanyDeviceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//UserCompanyDevice list
			UserCompanyDeviceList list = new UserCompanyDeviceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					UserCompanyDevice userCompanyDeviceObject = new UserCompanyDevice();
					FillObject(userCompanyDeviceObject, reader);

					list.Add(userCompanyDeviceObject);
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
