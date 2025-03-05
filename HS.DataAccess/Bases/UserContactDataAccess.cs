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
	public partial class UserContactDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTUSERCONTACT = "InsertUserContact";
		private const string UPDATEUSERCONTACT = "UpdateUserContact";
		private const string DELETEUSERCONTACT = "DeleteUserContact";
		private const string GETUSERCONTACTBYID = "GetUserContactById";
		private const string GETALLUSERCONTACT = "GetAllUserContact";
		private const string GETPAGEDUSERCONTACT = "GetPagedUserContact";
		private const string GETUSERCONTACTMAXIMUMID = "GetUserContactMaximumId";
		private const string GETUSERCONTACTROWCOUNT = "GetUserContactRowCount";	
		private const string GETUSERCONTACTBYQUERY = "GetUserContactByQuery";
		#endregion
		
		#region Constructors
		public UserContactDataAccess(ClientContext context) : base(context) { }
		public UserContactDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="userContactObject"></param>
		private void AddCommonParams(SqlCommand cmd, UserContactBase userContactObject)
		{	
			AddParameter(cmd, pGuid(UserContactBase.Property_UserId, userContactObject.UserId));
			AddParameter(cmd, pGuid(UserContactBase.Property_ContactId, userContactObject.ContactId));
			AddParameter(cmd, pNVarChar(UserContactBase.Property_UserType, 50, userContactObject.UserType));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts UserContact
        /// </summary>
        /// <param name="userContactObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(UserContactBase userContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTUSERCONTACT);
	
				AddParameter(cmd, pInt32Out(UserContactBase.Property_Id));
				AddCommonParams(cmd, userContactObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					userContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					userContactObject.Id = (Int32)GetOutParameter(cmd, UserContactBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(userContactObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates UserContact
        /// </summary>
        /// <param name="userContactObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(UserContactBase userContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEUSERCONTACT);
				
				AddParameter(cmd, pInt32(UserContactBase.Property_Id, userContactObject.Id));
				AddCommonParams(cmd, userContactObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					userContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(userContactObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes UserContact
        /// </summary>
        /// <param name="Id">Id of the UserContact object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEUSERCONTACT);	
				
				AddParameter(cmd, pInt32(UserContactBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(UserContact), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves UserContact object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the UserContact object to retrieve</param>
        /// <returns>UserContact object, null if not found</returns>
		public UserContact Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERCONTACTBYID))
			{
				AddParameter( cmd, pInt32(UserContactBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all UserContact objects 
        /// </summary>
        /// <returns>A list of UserContact objects</returns>
		public UserContactList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLUSERCONTACT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all UserContact objects by PageRequest
        /// </summary>
        /// <returns>A list of UserContact objects</returns>
		public UserContactList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDUSERCONTACT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				UserContactList _UserContactList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _UserContactList;
			}
		}
		
		/// <summary>
        /// Retrieves all UserContact objects by query String
        /// </summary>
        /// <returns>A list of UserContact objects</returns>
		public UserContactList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERCONTACTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get UserContact Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of UserContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERCONTACTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get UserContact Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of UserContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _UserContactRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERCONTACTROWCOUNT))
			{
				SqlDataReader reader;
				_UserContactRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _UserContactRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills UserContact object
        /// </summary>
        /// <param name="userContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(UserContactBase userContactObject, SqlDataReader reader, int start)
		{
			
				userContactObject.Id = reader.GetInt32( start + 0 );			
				userContactObject.UserId = reader.GetGuid( start + 1 );			
				userContactObject.ContactId = reader.GetGuid( start + 2 );			
				userContactObject.UserType = reader.GetString( start + 3 );			
			FillBaseObject(userContactObject, reader, (start + 4));

			
			userContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills UserContact object
        /// </summary>
        /// <param name="userContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(UserContactBase userContactObject, SqlDataReader reader)
		{
			FillObject(userContactObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves UserContact object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>UserContact object</returns>
		private UserContact GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					UserContact userContactObject= new UserContact();
					FillObject(userContactObject, reader);
					return userContactObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of UserContact objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of UserContact objects</returns>
		private UserContactList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//UserContact list
			UserContactList list = new UserContactList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					UserContact userContactObject = new UserContact();
					FillObject(userContactObject, reader);

					list.Add(userContactObject);
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
