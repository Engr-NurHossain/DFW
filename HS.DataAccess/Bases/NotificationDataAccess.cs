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
	public partial class NotificationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTNOTIFICATION = "InsertNotification";
		private const string UPDATENOTIFICATION = "UpdateNotification";
		private const string DELETENOTIFICATION = "DeleteNotification";
		private const string GETNOTIFICATIONBYID = "GetNotificationById";
		private const string GETALLNOTIFICATION = "GetAllNotification";
		private const string GETPAGEDNOTIFICATION = "GetPagedNotification";
		private const string GETNOTIFICATIONMAXIMUMID = "GetNotificationMaximumId";
		private const string GETNOTIFICATIONROWCOUNT = "GetNotificationRowCount";	
		private const string GETNOTIFICATIONBYQUERY = "GetNotificationByQuery";
		#endregion
		
		#region Constructors
		public NotificationDataAccess(ClientContext context) : base(context) { }
		public NotificationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="notificationObject"></param>
		private void AddCommonParams(SqlCommand cmd, NotificationBase notificationObject)
		{	
			AddParameter(cmd, pGuid(NotificationBase.Property_CompanyId, notificationObject.CompanyId));
			AddParameter(cmd, pGuid(NotificationBase.Property_NotificationId, notificationObject.NotificationId));
			AddParameter(cmd, pGuid(NotificationBase.Property_Who, notificationObject.Who));
			AddParameter(cmd, pNVarChar(NotificationBase.Property_What, 500, notificationObject.What));
			AddParameter(cmd, pDateTime(NotificationBase.Property_CreatedDate, notificationObject.CreatedDate));
			AddParameter(cmd, pNVarChar(NotificationBase.Property_Type, 50, notificationObject.Type));
			AddParameter(cmd, pNVarChar(NotificationBase.Property_NotificationUrl, 250, notificationObject.NotificationUrl));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Notification
        /// </summary>
        /// <param name="notificationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(NotificationBase notificationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTNOTIFICATION);
	
				AddParameter(cmd, pInt32Out(NotificationBase.Property_Id));
				AddCommonParams(cmd, notificationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					notificationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					notificationObject.Id = (Int32)GetOutParameter(cmd, NotificationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(notificationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Notification
        /// </summary>
        /// <param name="notificationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(NotificationBase notificationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATENOTIFICATION);
				
				AddParameter(cmd, pInt32(NotificationBase.Property_Id, notificationObject.Id));
				AddCommonParams(cmd, notificationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					notificationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(notificationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Notification
        /// </summary>
        /// <param name="Id">Id of the Notification object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETENOTIFICATION);	
				
				AddParameter(cmd, pInt32(NotificationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Notification), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Notification object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Notification object to retrieve</param>
        /// <returns>Notification object, null if not found</returns>
		public Notification Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETNOTIFICATIONBYID))
			{
				AddParameter( cmd, pInt32(NotificationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Notification objects 
        /// </summary>
        /// <returns>A list of Notification objects</returns>
		public NotificationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLNOTIFICATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Notification objects by PageRequest
        /// </summary>
        /// <returns>A list of Notification objects</returns>
		public NotificationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDNOTIFICATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				NotificationList _NotificationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _NotificationList;
			}
		}
		
		/// <summary>
        /// Retrieves all Notification objects by query String
        /// </summary>
        /// <returns>A list of Notification objects</returns>
		public NotificationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETNOTIFICATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Notification Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Notification
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETNOTIFICATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Notification Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Notification
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _NotificationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETNOTIFICATIONROWCOUNT))
			{
				SqlDataReader reader;
				_NotificationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _NotificationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Notification object
        /// </summary>
        /// <param name="notificationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(NotificationBase notificationObject, SqlDataReader reader, int start)
		{
			
				notificationObject.Id = reader.GetInt32( start + 0 );			
				notificationObject.CompanyId = reader.GetGuid( start + 1 );			
				notificationObject.NotificationId = reader.GetGuid( start + 2 );			
				notificationObject.Who = reader.GetGuid( start + 3 );			
				notificationObject.What = reader.GetString( start + 4 );			
				notificationObject.CreatedDate = reader.GetDateTime( start + 5 );			
				notificationObject.Type = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) notificationObject.NotificationUrl = reader.GetString( start + 7 );			
			FillBaseObject(notificationObject, reader, (start + 8));

			
			notificationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Notification object
        /// </summary>
        /// <param name="notificationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(NotificationBase notificationObject, SqlDataReader reader)
		{
			FillObject(notificationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Notification object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Notification object</returns>
		private Notification GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Notification notificationObject= new Notification();
					FillObject(notificationObject, reader);
					return notificationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Notification objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Notification objects</returns>
		private NotificationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Notification list
			NotificationList list = new NotificationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Notification notificationObject = new Notification();
					FillObject(notificationObject, reader);

					list.Add(notificationObject);
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
