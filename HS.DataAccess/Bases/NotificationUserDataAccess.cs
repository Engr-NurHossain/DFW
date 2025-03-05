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
	public partial class NotificationUserDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTNOTIFICATIONUSER = "InsertNotificationUser";
		private const string UPDATENOTIFICATIONUSER = "UpdateNotificationUser";
		private const string DELETENOTIFICATIONUSER = "DeleteNotificationUser";
		private const string GETNOTIFICATIONUSERBYID = "GetNotificationUserById";
		private const string GETALLNOTIFICATIONUSER = "GetAllNotificationUser";
		private const string GETPAGEDNOTIFICATIONUSER = "GetPagedNotificationUser";
		private const string GETNOTIFICATIONUSERMAXIMUMID = "GetNotificationUserMaximumId";
		private const string GETNOTIFICATIONUSERROWCOUNT = "GetNotificationUserRowCount";	
		private const string GETNOTIFICATIONUSERBYQUERY = "GetNotificationUserByQuery";
		#endregion
		
		#region Constructors
		public NotificationUserDataAccess(ClientContext context) : base(context) { }
		public NotificationUserDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        
        #endregion

        #region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="notificationUserObject"></param>
        private void AddCommonParams(SqlCommand cmd, NotificationUserBase notificationUserObject)
		{	
			AddParameter(cmd, pGuid(NotificationUserBase.Property_NotificationId, notificationUserObject.NotificationId));
			AddParameter(cmd, pGuid(NotificationUserBase.Property_NotificationPerson, notificationUserObject.NotificationPerson));
			AddParameter(cmd, pBool(NotificationUserBase.Property_IsRead, notificationUserObject.IsRead));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts NotificationUser
        /// </summary>
        /// <param name="notificationUserObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(NotificationUserBase notificationUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTNOTIFICATIONUSER);
	
				AddParameter(cmd, pInt32Out(NotificationUserBase.Property_Id));
				AddCommonParams(cmd, notificationUserObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					notificationUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					notificationUserObject.Id = (Int32)GetOutParameter(cmd, NotificationUserBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(notificationUserObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates NotificationUser
        /// </summary>
        /// <param name="notificationUserObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(NotificationUserBase notificationUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATENOTIFICATIONUSER);
				
				AddParameter(cmd, pInt32(NotificationUserBase.Property_Id, notificationUserObject.Id));
				AddCommonParams(cmd, notificationUserObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					notificationUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(notificationUserObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes NotificationUser
        /// </summary>
        /// <param name="Id">Id of the NotificationUser object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETENOTIFICATIONUSER);	
				
				AddParameter(cmd, pInt32(NotificationUserBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(NotificationUser), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves NotificationUser object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the NotificationUser object to retrieve</param>
        /// <returns>NotificationUser object, null if not found</returns>
		public NotificationUser Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETNOTIFICATIONUSERBYID))
			{
				AddParameter( cmd, pInt32(NotificationUserBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all NotificationUser objects 
        /// </summary>
        /// <returns>A list of NotificationUser objects</returns>
		public NotificationUserList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLNOTIFICATIONUSER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all NotificationUser objects by PageRequest
        /// </summary>
        /// <returns>A list of NotificationUser objects</returns>
		public NotificationUserList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDNOTIFICATIONUSER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				NotificationUserList _NotificationUserList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _NotificationUserList;
			}
		}
		
		/// <summary>
        /// Retrieves all NotificationUser objects by query String
        /// </summary>
        /// <returns>A list of NotificationUser objects</returns>
		public NotificationUserList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETNOTIFICATIONUSERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get NotificationUser Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of NotificationUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETNOTIFICATIONUSERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get NotificationUser Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of NotificationUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _NotificationUserRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETNOTIFICATIONUSERROWCOUNT))
			{
				SqlDataReader reader;
				_NotificationUserRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _NotificationUserRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills NotificationUser object
        /// </summary>
        /// <param name="notificationUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(NotificationUserBase notificationUserObject, SqlDataReader reader, int start)
		{
			
				notificationUserObject.Id = reader.GetInt32( start + 0 );			
				notificationUserObject.NotificationId = reader.GetGuid( start + 1 );			
				notificationUserObject.NotificationPerson = reader.GetGuid( start + 2 );			
				notificationUserObject.IsRead = reader.GetBoolean( start + 3 );			
			FillBaseObject(notificationUserObject, reader, (start + 4));

			
			notificationUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills NotificationUser object
        /// </summary>
        /// <param name="notificationUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(NotificationUserBase notificationUserObject, SqlDataReader reader)
		{
			FillObject(notificationUserObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves NotificationUser object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>NotificationUser object</returns>
		private NotificationUser GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					NotificationUser notificationUserObject= new NotificationUser();
					FillObject(notificationUserObject, reader);
					return notificationUserObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of NotificationUser objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of NotificationUser objects</returns>
		private NotificationUserList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//NotificationUser list
			NotificationUserList list = new NotificationUserList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					NotificationUser notificationUserObject = new NotificationUser();
					FillObject(notificationUserObject, reader);

					list.Add(notificationUserObject);
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
