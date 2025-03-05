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
	public partial class UserCompanyDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTUSERCOMPANY = "InsertUserCompany";
		private const string UPDATEUSERCOMPANY = "UpdateUserCompany";
		private const string DELETEUSERCOMPANY = "DeleteUserCompany";
		private const string GETUSERCOMPANYBYID = "GetUserCompanyById";
		private const string GETALLUSERCOMPANY = "GetAllUserCompany";
		private const string GETPAGEDUSERCOMPANY = "GetPagedUserCompany";
		private const string GETUSERCOMPANYMAXIMUMID = "GetUserCompanyMaximumId";
		private const string GETUSERCOMPANYROWCOUNT = "GetUserCompanyRowCount";	
		private const string GETUSERCOMPANYBYQUERY = "GetUserCompanyByQuery";
		#endregion
		
		#region Constructors
		public UserCompanyDataAccess(ClientContext context) : base(context) { }
		public UserCompanyDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="userCompanyObject"></param>
		private void AddCommonParams(SqlCommand cmd, UserCompanyBase userCompanyObject)
		{	
			AddParameter(cmd, pGuid(UserCompanyBase.Property_CompanyId, userCompanyObject.CompanyId));
			AddParameter(cmd, pGuid(UserCompanyBase.Property_UserId, userCompanyObject.UserId));
			AddParameter(cmd, pBool(UserCompanyBase.Property_IsDefault, userCompanyObject.IsDefault));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts UserCompany
        /// </summary>
        /// <param name="userCompanyObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(UserCompanyBase userCompanyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTUSERCOMPANY);
	
				AddParameter(cmd, pInt32Out(UserCompanyBase.Property_Id));
				AddCommonParams(cmd, userCompanyObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					userCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					userCompanyObject.Id = (Int32)GetOutParameter(cmd, UserCompanyBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(userCompanyObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates UserCompany
        /// </summary>
        /// <param name="userCompanyObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(UserCompanyBase userCompanyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEUSERCOMPANY);
				
				AddParameter(cmd, pInt32(UserCompanyBase.Property_Id, userCompanyObject.Id));
				AddCommonParams(cmd, userCompanyObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					userCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(userCompanyObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes UserCompany
        /// </summary>
        /// <param name="Id">Id of the UserCompany object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEUSERCOMPANY);	
				
				AddParameter(cmd, pInt32(UserCompanyBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(UserCompany), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves UserCompany object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the UserCompany object to retrieve</param>
        /// <returns>UserCompany object, null if not found</returns>
		public UserCompany Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERCOMPANYBYID))
			{
				AddParameter( cmd, pInt32(UserCompanyBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all UserCompany objects 
        /// </summary>
        /// <returns>A list of UserCompany objects</returns>
		public UserCompanyList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLUSERCOMPANY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all UserCompany objects by PageRequest
        /// </summary>
        /// <returns>A list of UserCompany objects</returns>
		public UserCompanyList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDUSERCOMPANY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				UserCompanyList _UserCompanyList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _UserCompanyList;
			}
		}
		
		/// <summary>
        /// Retrieves all UserCompany objects by query String
        /// </summary>
        /// <returns>A list of UserCompany objects</returns>
		public UserCompanyList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERCOMPANYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get UserCompany Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of UserCompany
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERCOMPANYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get UserCompany Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of UserCompany
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _UserCompanyRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERCOMPANYROWCOUNT))
			{
				SqlDataReader reader;
				_UserCompanyRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _UserCompanyRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills UserCompany object
        /// </summary>
        /// <param name="userCompanyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(UserCompanyBase userCompanyObject, SqlDataReader reader, int start)
		{
			
				userCompanyObject.Id = reader.GetInt32( start + 0 );			
				userCompanyObject.CompanyId = reader.GetGuid( start + 1 );			
				userCompanyObject.UserId = reader.GetGuid( start + 2 );			
				userCompanyObject.IsDefault = reader.GetBoolean( start + 3 );			
			FillBaseObject(userCompanyObject, reader, (start + 4));

			
			userCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills UserCompany object
        /// </summary>
        /// <param name="userCompanyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(UserCompanyBase userCompanyObject, SqlDataReader reader)
		{
			FillObject(userCompanyObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves UserCompany object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>UserCompany object</returns>
		private UserCompany GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					UserCompany userCompanyObject= new UserCompany();
					FillObject(userCompanyObject, reader);
					return userCompanyObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of UserCompany objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of UserCompany objects</returns>
		private UserCompanyList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//UserCompany list
			UserCompanyList list = new UserCompanyList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					UserCompany userCompanyObject = new UserCompany();
					FillObject(userCompanyObject, reader);

					list.Add(userCompanyObject);
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
