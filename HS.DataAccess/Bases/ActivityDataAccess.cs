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
	public partial class ActivityDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTACTIVITY = "InsertActivity";
		private const string UPDATEACTIVITY = "UpdateActivity";
		private const string DELETEACTIVITY = "DeleteActivity";
		private const string GETACTIVITYBYID = "GetActivityById";
		private const string GETALLACTIVITY = "GetAllActivity";
		private const string GETPAGEDACTIVITY = "GetPagedActivity";
		private const string GETACTIVITYMAXIMUMID = "GetActivityMaximumId";
		private const string GETACTIVITYROWCOUNT = "GetActivityRowCount";	
		private const string GETACTIVITYBYQUERY = "GetActivityByQuery";
		#endregion
		
		#region Constructors
		public ActivityDataAccess(ClientContext context) : base(context) { }
		public ActivityDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="activityObject"></param>
		private void AddCommonParams(SqlCommand cmd, ActivityBase activityObject)
		{	
			AddParameter(cmd, pGuid(ActivityBase.Property_ActivityId, activityObject.ActivityId));
			AddParameter(cmd, pNVarChar(ActivityBase.Property_ActivityType, 50, activityObject.ActivityType));
			AddParameter(cmd, pNVarChar(ActivityBase.Property_Description, activityObject.Description));
			AddParameter(cmd, pGuid(ActivityBase.Property_AssignedTo, activityObject.AssignedTo));
			AddParameter(cmd, pDateTime(ActivityBase.Property_DueDate, activityObject.DueDate));
			AddParameter(cmd, pNVarChar(ActivityBase.Property_Status, 50, activityObject.Status));
			AddParameter(cmd, pGuid(ActivityBase.Property_AssociatedWith, activityObject.AssociatedWith));
			AddParameter(cmd, pNVarChar(ActivityBase.Property_AssociatedType, 50, activityObject.AssociatedType));
			AddParameter(cmd, pNVarChar(ActivityBase.Property_Note, activityObject.Note));
			AddParameter(cmd, pGuid(ActivityBase.Property_CreatedBy, activityObject.CreatedBy));
			AddParameter(cmd, pDateTime(ActivityBase.Property_CreatedDate, activityObject.CreatedDate));
			AddParameter(cmd, pNVarChar(ActivityBase.Property_NotifyBy, 50, activityObject.NotifyBy));
			AddParameter(cmd, pNVarChar(ActivityBase.Property_Origin, 150, activityObject.Origin));
			AddParameter(cmd, pNVarChar(ActivityBase.Property_Department, 100, activityObject.Department));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Activity
        /// </summary>
        /// <param name="activityObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ActivityBase activityObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTACTIVITY);
	
				AddParameter(cmd, pInt32Out(ActivityBase.Property_Id));
				AddCommonParams(cmd, activityObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					activityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					activityObject.Id = (Int32)GetOutParameter(cmd, ActivityBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(activityObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Activity
        /// </summary>
        /// <param name="activityObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ActivityBase activityObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEACTIVITY);
				
				AddParameter(cmd, pInt32(ActivityBase.Property_Id, activityObject.Id));
				AddCommonParams(cmd, activityObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					activityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(activityObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Activity
        /// </summary>
        /// <param name="Id">Id of the Activity object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEACTIVITY);	
				
				AddParameter(cmd, pInt32(ActivityBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Activity), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Activity object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Activity object to retrieve</param>
        /// <returns>Activity object, null if not found</returns>
		public Activity Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETACTIVITYBYID))
			{
				AddParameter( cmd, pInt32(ActivityBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Activity objects 
        /// </summary>
        /// <returns>A list of Activity objects</returns>
		public ActivityList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLACTIVITY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Activity objects by PageRequest
        /// </summary>
        /// <returns>A list of Activity objects</returns>
		public ActivityList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDACTIVITY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ActivityList _ActivityList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ActivityList;
			}
		}
		
		/// <summary>
        /// Retrieves all Activity objects by query String
        /// </summary>
        /// <returns>A list of Activity objects</returns>
		public ActivityList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETACTIVITYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Activity Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Activity
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETACTIVITYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Activity Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Activity
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ActivityRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETACTIVITYROWCOUNT))
			{
				SqlDataReader reader;
				_ActivityRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ActivityRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Activity object
        /// </summary>
        /// <param name="activityObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ActivityBase activityObject, SqlDataReader reader, int start)
		{
			
				activityObject.Id = reader.GetInt32( start + 0 );			
				activityObject.ActivityId = reader.GetGuid( start + 1 );			
				activityObject.ActivityType = reader.GetString( start + 2 );			
				activityObject.Description = reader.GetString( start + 3 );			
				activityObject.AssignedTo = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) activityObject.DueDate = reader.GetDateTime( start + 5 );			
				activityObject.Status = reader.GetString( start + 6 );			
				activityObject.AssociatedWith = reader.GetGuid( start + 7 );			
				activityObject.AssociatedType = reader.GetString( start + 8 );			
				activityObject.Note = reader.GetString( start + 9 );			
				activityObject.CreatedBy = reader.GetGuid( start + 10 );			
				activityObject.CreatedDate = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) activityObject.NotifyBy = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) activityObject.Origin = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) activityObject.Department = reader.GetString( start + 14 );			
			FillBaseObject(activityObject, reader, (start + 15));

			
			activityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Activity object
        /// </summary>
        /// <param name="activityObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ActivityBase activityObject, SqlDataReader reader)
		{
			FillObject(activityObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Activity object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Activity object</returns>
		private Activity GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Activity activityObject= new Activity();
					FillObject(activityObject, reader);
					return activityObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Activity objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Activity objects</returns>
		private ActivityList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Activity list
			ActivityList list = new ActivityList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Activity activityObject = new Activity();
					FillObject(activityObject, reader);

					list.Add(activityObject);
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
