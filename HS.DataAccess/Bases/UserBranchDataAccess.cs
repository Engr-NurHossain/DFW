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
	public partial class UserBranchDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTUSERBRANCH = "InsertUserBranch";
		private const string UPDATEUSERBRANCH = "UpdateUserBranch";
		private const string DELETEUSERBRANCH = "DeleteUserBranch";
		private const string GETUSERBRANCHBYID = "GetUserBranchById";
		private const string GETALLUSERBRANCH = "GetAllUserBranch";
		private const string GETPAGEDUSERBRANCH = "GetPagedUserBranch";
		private const string GETUSERBRANCHMAXIMUMID = "GetUserBranchMaximumId";
		private const string GETUSERBRANCHROWCOUNT = "GetUserBranchRowCount";	
		private const string GETUSERBRANCHBYQUERY = "GetUserBranchByQuery";
		#endregion
		
		#region Constructors
		public UserBranchDataAccess(ClientContext context) : base(context) { }
		public UserBranchDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="userBranchObject"></param>
		private void AddCommonParams(SqlCommand cmd, UserBranchBase userBranchObject)
		{	
			AddParameter(cmd, pGuid(UserBranchBase.Property_CompanyId, userBranchObject.CompanyId));
			AddParameter(cmd, pGuid(UserBranchBase.Property_UserId, userBranchObject.UserId));
			AddParameter(cmd, pInt32(UserBranchBase.Property_BranchId, userBranchObject.BranchId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts UserBranch
        /// </summary>
        /// <param name="userBranchObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(UserBranchBase userBranchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTUSERBRANCH);
	
				AddParameter(cmd, pInt32Out(UserBranchBase.Property_Id));
				AddCommonParams(cmd, userBranchObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					userBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					userBranchObject.Id = (Int32)GetOutParameter(cmd, UserBranchBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(userBranchObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates UserBranch
        /// </summary>
        /// <param name="userBranchObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(UserBranchBase userBranchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEUSERBRANCH);
				
				AddParameter(cmd, pInt32(UserBranchBase.Property_Id, userBranchObject.Id));
				AddCommonParams(cmd, userBranchObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					userBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(userBranchObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes UserBranch
        /// </summary>
        /// <param name="Id">Id of the UserBranch object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEUSERBRANCH);	
				
				AddParameter(cmd, pInt32(UserBranchBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(UserBranch), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves UserBranch object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the UserBranch object to retrieve</param>
        /// <returns>UserBranch object, null if not found</returns>
		public UserBranch Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERBRANCHBYID))
			{
				AddParameter( cmd, pInt32(UserBranchBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all UserBranch objects 
        /// </summary>
        /// <returns>A list of UserBranch objects</returns>
		public UserBranchList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLUSERBRANCH))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all UserBranch objects by PageRequest
        /// </summary>
        /// <returns>A list of UserBranch objects</returns>
		public UserBranchList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDUSERBRANCH))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				UserBranchList _UserBranchList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _UserBranchList;
			}
		}
		
		/// <summary>
        /// Retrieves all UserBranch objects by query String
        /// </summary>
        /// <returns>A list of UserBranch objects</returns>
		public UserBranchList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERBRANCHBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get UserBranch Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of UserBranch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERBRANCHMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get UserBranch Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of UserBranch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _UserBranchRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERBRANCHROWCOUNT))
			{
				SqlDataReader reader;
				_UserBranchRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _UserBranchRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills UserBranch object
        /// </summary>
        /// <param name="userBranchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(UserBranchBase userBranchObject, SqlDataReader reader, int start)
		{
			
				userBranchObject.Id = reader.GetInt32( start + 0 );			
				userBranchObject.CompanyId = reader.GetGuid( start + 1 );			
				userBranchObject.UserId = reader.GetGuid( start + 2 );			
				userBranchObject.BranchId = reader.GetInt32( start + 3 );			
			FillBaseObject(userBranchObject, reader, (start + 4));

			
			userBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills UserBranch object
        /// </summary>
        /// <param name="userBranchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(UserBranchBase userBranchObject, SqlDataReader reader)
		{
			FillObject(userBranchObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves UserBranch object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>UserBranch object</returns>
		private UserBranch GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					UserBranch userBranchObject= new UserBranch();
					FillObject(userBranchObject, reader);
					return userBranchObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of UserBranch objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of UserBranch objects</returns>
		private UserBranchList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//UserBranch list
			UserBranchList list = new UserBranchList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					UserBranch userBranchObject = new UserBranch();
					FillObject(userBranchObject, reader);

					list.Add(userBranchObject);
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
