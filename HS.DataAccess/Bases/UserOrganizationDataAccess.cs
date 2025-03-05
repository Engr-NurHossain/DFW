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
	public partial class UserOrganizationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTUSERORGANIZATION = "InsertUserOrganization";
		private const string UPDATEUSERORGANIZATION = "UpdateUserOrganization";
		private const string DELETEUSERORGANIZATION = "DeleteUserOrganization";
		private const string GETUSERORGANIZATIONBYID = "GetUserOrganizationById";
		private const string GETALLUSERORGANIZATION = "GetAllUserOrganization";
		private const string GETPAGEDUSERORGANIZATION = "GetPagedUserOrganization";
		private const string GETUSERORGANIZATIONMAXIMUMID = "GetUserOrganizationMaximumId";
		private const string GETUSERORGANIZATIONROWCOUNT = "GetUserOrganizationRowCount";	
		private const string GETUSERORGANIZATIONBYQUERY = "GetUserOrganizationByQuery";
        #endregion

        #region Constructors
		public UserOrganizationDataAccess(ClientContext context) : base(context) { }
        public UserOrganizationDataAccess(string ConnectionString) : base(ConnectionString) { }
        public UserOrganizationDataAccess(ClientContext context,string ConnectionString) : base(context, ConnectionString) { }
        public UserOrganizationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="userOrganizationObject"></param>
		private void AddCommonParams(SqlCommand cmd, UserOrganizationBase userOrganizationObject)
		{	
			AddParameter(cmd, pGuid(UserOrganizationBase.Property_CompanyId, userOrganizationObject.CompanyId));
			AddParameter(cmd, pNVarChar(UserOrganizationBase.Property_UserName, 250, userOrganizationObject.UserName));
			AddParameter(cmd, pBool(UserOrganizationBase.Property_IsActive, userOrganizationObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts UserOrganization
        /// </summary>
        /// <param name="userOrganizationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(UserOrganizationBase userOrganizationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTUSERORGANIZATION);
	
				AddParameter(cmd, pInt32(UserOrganizationBase.Property_Id, userOrganizationObject.Id));
				AddCommonParams(cmd, userOrganizationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					userOrganizationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(userOrganizationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates UserOrganization
        /// </summary>
        /// <param name="userOrganizationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(UserOrganizationBase userOrganizationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEUSERORGANIZATION);
				
				AddParameter(cmd, pInt32(UserOrganizationBase.Property_Id, userOrganizationObject.Id));
				AddCommonParams(cmd, userOrganizationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					userOrganizationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(userOrganizationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes UserOrganization
        /// </summary>
        /// <param name="Id">Id of the UserOrganization object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEUSERORGANIZATION);	
				
				AddParameter(cmd, pInt32(UserOrganizationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(UserOrganization), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves UserOrganization object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the UserOrganization object to retrieve</param>
        /// <returns>UserOrganization object, null if not found</returns>
		public UserOrganization Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERORGANIZATIONBYID))
			{
				AddParameter( cmd, pInt32(UserOrganizationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all UserOrganization objects 
        /// </summary>
        /// <returns>A list of UserOrganization objects</returns>
		public UserOrganizationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLUSERORGANIZATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all UserOrganization objects by PageRequest
        /// </summary>
        /// <returns>A list of UserOrganization objects</returns>
		public UserOrganizationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDUSERORGANIZATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				UserOrganizationList _UserOrganizationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _UserOrganizationList;
			}
		}
		
		/// <summary>
        /// Retrieves all UserOrganization objects by query String
        /// </summary>
        /// <returns>A list of UserOrganization objects</returns>
		public UserOrganizationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERORGANIZATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get UserOrganization Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of UserOrganization
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERORGANIZATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get UserOrganization Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of UserOrganization
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _UserOrganizationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERORGANIZATIONROWCOUNT))
			{
				SqlDataReader reader;
				_UserOrganizationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _UserOrganizationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills UserOrganization object
        /// </summary>
        /// <param name="userOrganizationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(UserOrganizationBase userOrganizationObject, SqlDataReader reader, int start)
		{
			
				userOrganizationObject.Id = reader.GetInt32( start + 0 );			
				userOrganizationObject.CompanyId = reader.GetGuid( start + 1 );			
				userOrganizationObject.UserName = reader.GetString( start + 2 );			
				userOrganizationObject.IsActive = reader.GetBoolean( start + 3 );			
			FillBaseObject(userOrganizationObject, reader, (start + 4));

			
			userOrganizationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills UserOrganization object
        /// </summary>
        /// <param name="userOrganizationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(UserOrganizationBase userOrganizationObject, SqlDataReader reader)
		{
			FillObject(userOrganizationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves UserOrganization object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>UserOrganization object</returns>
		private UserOrganization GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					UserOrganization userOrganizationObject= new UserOrganization();
					FillObject(userOrganizationObject, reader);
					return userOrganizationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of UserOrganization objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of UserOrganization objects</returns>
		private UserOrganizationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//UserOrganization list
			UserOrganizationList list = new UserOrganizationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					UserOrganization userOrganizationObject = new UserOrganization();
					FillObject(userOrganizationObject, reader);

					list.Add(userOrganizationObject);
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
