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
	public partial class UserLoginDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTUSERLOGIN = "InsertUserLogin";
		private const string UPDATEUSERLOGIN = "UpdateUserLogin";
		private const string DELETEUSERLOGIN = "DeleteUserLogin";
		private const string GETUSERLOGINBYID = "GetUserLoginById";
		private const string GETALLUSERLOGIN = "GetAllUserLogin";
		private const string GETPAGEDUSERLOGIN = "GetPagedUserLogin";
		private const string GETUSERLOGINMAXIMUMID = "GetUserLoginMaximumId";
		private const string GETUSERLOGINROWCOUNT = "GetUserLoginRowCount";	
		private const string GETUSERLOGINBYQUERY = "GetUserLoginByQuery";
		#endregion
		
		#region Constructors
		public UserLoginDataAccess(ClientContext context) : base(context) { }
		public UserLoginDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="userLoginObject"></param>
		private void AddCommonParams(SqlCommand cmd, UserLoginBase userLoginObject)
		{	
			AddParameter(cmd, pGuid(UserLoginBase.Property_UserId, userLoginObject.UserId));
			AddParameter(cmd, pNVarChar(UserLoginBase.Property_UserName, 250, userLoginObject.UserName));
			AddParameter(cmd, pNVarChar(UserLoginBase.Property_Password, 100, userLoginObject.Password));
			AddParameter(cmd, pNVarChar(UserLoginBase.Property_EmailAddress, 500, userLoginObject.EmailAddress));
			AddParameter(cmd, pBool(UserLoginBase.Property_IsActive, userLoginObject.IsActive));
			AddParameter(cmd, pBool(UserLoginBase.Property_IsDeleted, userLoginObject.IsDeleted));
			AddParameter(cmd, pNVarChar(UserLoginBase.Property_LastUpdatedBy, 250, userLoginObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(UserLoginBase.Property_LastUpdatedDate, userLoginObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(UserLoginBase.Property_CompanyId, userLoginObject.CompanyId));
			AddParameter(cmd, pDateTime(UserLoginBase.Property_PasswordResetDate, userLoginObject.PasswordResetDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts UserLogin
        /// </summary>
        /// <param name="userLoginObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(UserLoginBase userLoginObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTUSERLOGIN);
	
				AddParameter(cmd, pInt32Out(UserLoginBase.Property_Id));
				AddCommonParams(cmd, userLoginObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					userLoginObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					userLoginObject.Id = (Int32)GetOutParameter(cmd, UserLoginBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(userLoginObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates UserLogin
        /// </summary>
        /// <param name="userLoginObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(UserLoginBase userLoginObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEUSERLOGIN);
				
				AddParameter(cmd, pInt32(UserLoginBase.Property_Id, userLoginObject.Id));
				AddCommonParams(cmd, userLoginObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					userLoginObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(userLoginObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes UserLogin
        /// </summary>
        /// <param name="Id">Id of the UserLogin object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEUSERLOGIN);	
				
				AddParameter(cmd, pInt32(UserLoginBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(UserLogin), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves UserLogin object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the UserLogin object to retrieve</param>
        /// <returns>UserLogin object, null if not found</returns>
		public UserLogin Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERLOGINBYID))
			{
				AddParameter( cmd, pInt32(UserLoginBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all UserLogin objects 
        /// </summary>
        /// <returns>A list of UserLogin objects</returns>
		public UserLoginList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLUSERLOGIN))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all UserLogin objects by PageRequest
        /// </summary>
        /// <returns>A list of UserLogin objects</returns>
		public UserLoginList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDUSERLOGIN))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				UserLoginList _UserLoginList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _UserLoginList;
			}
		}
		
		/// <summary>
        /// Retrieves all UserLogin objects by query String
        /// </summary>
        /// <returns>A list of UserLogin objects</returns>
		public UserLoginList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERLOGINBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get UserLogin Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of UserLogin
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERLOGINMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get UserLogin Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of UserLogin
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _UserLoginRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERLOGINROWCOUNT))
			{
				SqlDataReader reader;
				_UserLoginRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _UserLoginRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills UserLogin object
        /// </summary>
        /// <param name="userLoginObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(UserLoginBase userLoginObject, SqlDataReader reader, int start)
		{
			
				userLoginObject.Id = reader.GetInt32( start + 0 );			
				userLoginObject.UserId = reader.GetGuid( start + 1 );			
				userLoginObject.UserName = reader.GetString( start + 2 );			
				userLoginObject.Password = reader.GetString( start + 3 );			
				userLoginObject.EmailAddress = reader.GetString( start + 4 );			
				userLoginObject.IsActive = reader.GetBoolean( start + 5 );			
				userLoginObject.IsDeleted = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) userLoginObject.LastUpdatedBy = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) userLoginObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
				userLoginObject.CompanyId = reader.GetGuid( start + 9 );			
				if(!reader.IsDBNull(10)) userLoginObject.PasswordResetDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(userLoginObject, reader, (start + 11));

			
			userLoginObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills UserLogin object
        /// </summary>
        /// <param name="userLoginObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(UserLoginBase userLoginObject, SqlDataReader reader)
		{
			FillObject(userLoginObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves UserLogin object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>UserLogin object</returns>
		private UserLogin GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					UserLogin userLoginObject= new UserLogin();
					FillObject(userLoginObject, reader);
					return userLoginObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of UserLogin objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of UserLogin objects</returns>
		private UserLoginList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//UserLogin list
			UserLoginList list = new UserLoginList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					UserLogin userLoginObject = new UserLogin();
					FillObject(userLoginObject, reader);

					list.Add(userLoginObject);
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
