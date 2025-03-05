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
	public partial class UserActivityDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTUSERACTIVITY = "InsertUserActivity";
		private const string UPDATEUSERACTIVITY = "UpdateUserActivity";
		private const string DELETEUSERACTIVITY = "DeleteUserActivity";
		private const string GETUSERACTIVITYBYID = "GetUserActivityById";
		private const string GETALLUSERACTIVITY = "GetAllUserActivity";
		private const string GETPAGEDUSERACTIVITY = "GetPagedUserActivity";
		private const string GETUSERACTIVITYMAXIMUMID = "GetUserActivityMaximumId";
		private const string GETUSERACTIVITYROWCOUNT = "GetUserActivityRowCount";	
		private const string GETUSERACTIVITYBYQUERY = "GetUserActivityByQuery";
		#endregion
		
		#region Constructors
		public UserActivityDataAccess(ClientContext context) : base(context) { }
		public UserActivityDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="userActivityObject"></param>
		private void AddCommonParams(SqlCommand cmd, UserActivityBase userActivityObject)
		{	
			AddParameter(cmd, pGuid(UserActivityBase.Property_ActivityId, userActivityObject.ActivityId));
			AddParameter(cmd, pNVarChar(UserActivityBase.Property_PageUrl, userActivityObject.PageUrl));
			AddParameter(cmd, pNVarChar(UserActivityBase.Property_ReferrerUrl, userActivityObject.ReferrerUrl));
			AddParameter(cmd, pNVarChar(UserActivityBase.Property_Action, 250, userActivityObject.Action));
			AddParameter(cmd, pNVarChar(UserActivityBase.Property_ActionDisplyText, userActivityObject.ActionDisplyText));
			AddParameter(cmd, pNVarChar(UserActivityBase.Property_UserIp, 100, userActivityObject.UserIp));
			AddParameter(cmd, pNVarChar(UserActivityBase.Property_UserAgent, userActivityObject.UserAgent));
			AddParameter(cmd, pNVarChar(UserActivityBase.Property_UserName, 100, userActivityObject.UserName));
			AddParameter(cmd, pGuid(UserActivityBase.Property_UserId, userActivityObject.UserId));
			AddParameter(cmd, pDateTime(UserActivityBase.Property_StatsDate, userActivityObject.StatsDate));
			AddParameter(cmd, pBool(UserActivityBase.Property_IsARB, userActivityObject.IsARB));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts UserActivity
        /// </summary>
        /// <param name="userActivityObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(UserActivityBase userActivityObject)
		{
			long result = 0;
            try
			{
				SqlCommand cmd = GetSPCommand(INSERTUSERACTIVITY);
	
				AddParameter(cmd, pInt32Out(UserActivityBase.Property_Id));
				AddCommonParams(cmd, userActivityObject);

                result = InsertRecord(cmd);
				if (result > 0)
				{
					userActivityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					userActivityObject.Id = (Int32)GetOutParameter(cmd, UserActivityBase.Property_Id);
				}
			}
			catch(SqlException x)
			{
				logger.Error(x);
			}
            return result;
        }
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates UserActivity
        /// </summary>
        /// <param name="userActivityObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(UserActivityBase userActivityObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEUSERACTIVITY);
				
				AddParameter(cmd, pInt32(UserActivityBase.Property_Id, userActivityObject.Id));
				AddCommonParams(cmd, userActivityObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					userActivityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(userActivityObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes UserActivity
        /// </summary>
        /// <param name="Id">Id of the UserActivity object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEUSERACTIVITY);	
				
				AddParameter(cmd, pInt32(UserActivityBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(UserActivity), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves UserActivity object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the UserActivity object to retrieve</param>
        /// <returns>UserActivity object, null if not found</returns>
		public UserActivity Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERACTIVITYBYID))
			{
				AddParameter( cmd, pInt32(UserActivityBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all UserActivity objects 
        /// </summary>
        /// <returns>A list of UserActivity objects</returns>
		public UserActivityList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLUSERACTIVITY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all UserActivity objects by PageRequest
        /// </summary>
        /// <returns>A list of UserActivity objects</returns>
		public UserActivityList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDUSERACTIVITY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				UserActivityList _UserActivityList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _UserActivityList;
			}
		}
		
		/// <summary>
        /// Retrieves all UserActivity objects by query String
        /// </summary>
        /// <returns>A list of UserActivity objects</returns>
		public UserActivityList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETUSERACTIVITYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get UserActivity Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of UserActivity
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERACTIVITYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get UserActivity Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of UserActivity
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _UserActivityRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETUSERACTIVITYROWCOUNT))
			{
				SqlDataReader reader;
				_UserActivityRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _UserActivityRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills UserActivity object
        /// </summary>
        /// <param name="userActivityObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(UserActivityBase userActivityObject, SqlDataReader reader, int start)
		{
			
				userActivityObject.Id = reader.GetInt32( start + 0 );			
				userActivityObject.ActivityId = reader.GetGuid( start + 1 );			
				userActivityObject.PageUrl = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) userActivityObject.ReferrerUrl = reader.GetString( start + 3 );			
				userActivityObject.Action = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) userActivityObject.ActionDisplyText = reader.GetString( start + 5 );			
				userActivityObject.UserIp = reader.GetString( start + 6 );			
				userActivityObject.UserAgent = reader.GetString( start + 7 );			
				userActivityObject.UserName = reader.GetString( start + 8 );			
				userActivityObject.UserId = reader.GetGuid( start + 9 );			
				userActivityObject.StatsDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) userActivityObject.IsARB = reader.GetBoolean( start + 11 );			
			FillBaseObject(userActivityObject, reader, (start + 12));

			
			userActivityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills UserActivity object
        /// </summary>
        /// <param name="userActivityObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(UserActivityBase userActivityObject, SqlDataReader reader)
		{
			FillObject(userActivityObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves UserActivity object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>UserActivity object</returns>
		private UserActivity GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					UserActivity userActivityObject= new UserActivity();
					FillObject(userActivityObject, reader);
					return userActivityObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of UserActivity objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of UserActivity objects</returns>
		private UserActivityList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//UserActivity list
			UserActivityList list = new UserActivityList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					UserActivity userActivityObject = new UserActivity();
					FillObject(userActivityObject, reader);

					list.Add(userActivityObject);
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
