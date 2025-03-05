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
	public partial class TrackingNumberSettingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTRACKINGNUMBERSETTING = "InsertTrackingNumberSetting";
		private const string UPDATETRACKINGNUMBERSETTING = "UpdateTrackingNumberSetting";
		private const string DELETETRACKINGNUMBERSETTING = "DeleteTrackingNumberSetting";
		private const string GETTRACKINGNUMBERSETTINGBYID = "GetTrackingNumberSettingById";
		private const string GETALLTRACKINGNUMBERSETTING = "GetAllTrackingNumberSetting";
		private const string GETPAGEDTRACKINGNUMBERSETTING = "GetPagedTrackingNumberSetting";
		private const string GETTRACKINGNUMBERSETTINGMAXIMUMID = "GetTrackingNumberSettingMaximumId";
		private const string GETTRACKINGNUMBERSETTINGROWCOUNT = "GetTrackingNumberSettingRowCount";	
		private const string GETTRACKINGNUMBERSETTINGBYQUERY = "GetTrackingNumberSettingByQuery";
		#endregion
		
		#region Constructors
		public TrackingNumberSettingDataAccess(ClientContext context) : base(context) { }
		public TrackingNumberSettingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="trackingNumberSettingObject"></param>
		private void AddCommonParams(SqlCommand cmd, TrackingNumberSettingBase trackingNumberSettingObject)
		{	
			AddParameter(cmd, pGuid(TrackingNumberSettingBase.Property_CompanyId, trackingNumberSettingObject.CompanyId));
			AddParameter(cmd, pGuid(TrackingNumberSettingBase.Property_TrackingId, trackingNumberSettingObject.TrackingId));
			AddParameter(cmd, pNVarChar(TrackingNumberSettingBase.Property_TrackingNumber, 250, trackingNumberSettingObject.TrackingNumber));
			AddParameter(cmd, pBool(TrackingNumberSettingBase.Property_IsActive, trackingNumberSettingObject.IsActive));
			AddParameter(cmd, pBool(TrackingNumberSettingBase.Property_IsRecorded, trackingNumberSettingObject.IsRecorded));
			AddParameter(cmd, pBool(TrackingNumberSettingBase.Property_IsPrompt, trackingNumberSettingObject.IsPrompt));
			AddParameter(cmd, pNVarChar(TrackingNumberSettingBase.Property_Comments, trackingNumberSettingObject.Comments));
			AddParameter(cmd, pGuid(TrackingNumberSettingBase.Property_CreatedBy, trackingNumberSettingObject.CreatedBy));
			AddParameter(cmd, pGuid(TrackingNumberSettingBase.Property_LastUpdatedBy, trackingNumberSettingObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(TrackingNumberSettingBase.Property_CreatedDate, trackingNumberSettingObject.CreatedDate));
			AddParameter(cmd, pDateTime(TrackingNumberSettingBase.Property_LastUpdatedDate, trackingNumberSettingObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(TrackingNumberSettingBase.Property_ForwardingNumber, 250, trackingNumberSettingObject.ForwardingNumber));
			AddParameter(cmd, pNVarChar(TrackingNumberSettingBase.Property_SubAccountId, trackingNumberSettingObject.SubAccountId));
			AddParameter(cmd, pNVarChar(TrackingNumberSettingBase.Property_SubAccountToken, trackingNumberSettingObject.SubAccountToken));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TrackingNumberSetting
        /// </summary>
        /// <param name="trackingNumberSettingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TrackingNumberSettingBase trackingNumberSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTRACKINGNUMBERSETTING);
	
				AddParameter(cmd, pInt32Out(TrackingNumberSettingBase.Property_Id));
				AddCommonParams(cmd, trackingNumberSettingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					trackingNumberSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					trackingNumberSettingObject.Id = (Int32)GetOutParameter(cmd, TrackingNumberSettingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(trackingNumberSettingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TrackingNumberSetting
        /// </summary>
        /// <param name="trackingNumberSettingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TrackingNumberSettingBase trackingNumberSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETRACKINGNUMBERSETTING);
				
				AddParameter(cmd, pInt32(TrackingNumberSettingBase.Property_Id, trackingNumberSettingObject.Id));
				AddCommonParams(cmd, trackingNumberSettingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					trackingNumberSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(trackingNumberSettingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TrackingNumberSetting
        /// </summary>
        /// <param name="Id">Id of the TrackingNumberSetting object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETRACKINGNUMBERSETTING);	
				
				AddParameter(cmd, pInt32(TrackingNumberSettingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TrackingNumberSetting), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TrackingNumberSetting object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TrackingNumberSetting object to retrieve</param>
        /// <returns>TrackingNumberSetting object, null if not found</returns>
		public TrackingNumberSetting Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRACKINGNUMBERSETTINGBYID))
			{
				AddParameter( cmd, pInt32(TrackingNumberSettingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TrackingNumberSetting objects 
        /// </summary>
        /// <returns>A list of TrackingNumberSetting objects</returns>
		public TrackingNumberSettingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTRACKINGNUMBERSETTING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TrackingNumberSetting objects by PageRequest
        /// </summary>
        /// <returns>A list of TrackingNumberSetting objects</returns>
		public TrackingNumberSettingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTRACKINGNUMBERSETTING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TrackingNumberSettingList _TrackingNumberSettingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TrackingNumberSettingList;
			}
		}
		
		/// <summary>
        /// Retrieves all TrackingNumberSetting objects by query String
        /// </summary>
        /// <returns>A list of TrackingNumberSetting objects</returns>
		public TrackingNumberSettingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRACKINGNUMBERSETTINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TrackingNumberSetting Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TrackingNumberSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRACKINGNUMBERSETTINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TrackingNumberSetting Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TrackingNumberSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TrackingNumberSettingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRACKINGNUMBERSETTINGROWCOUNT))
			{
				SqlDataReader reader;
				_TrackingNumberSettingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TrackingNumberSettingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TrackingNumberSetting object
        /// </summary>
        /// <param name="trackingNumberSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TrackingNumberSettingBase trackingNumberSettingObject, SqlDataReader reader, int start)
		{
			
				trackingNumberSettingObject.Id = reader.GetInt32( start + 0 );			
				trackingNumberSettingObject.CompanyId = reader.GetGuid( start + 1 );			
				trackingNumberSettingObject.TrackingId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) trackingNumberSettingObject.TrackingNumber = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) trackingNumberSettingObject.IsActive = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) trackingNumberSettingObject.IsRecorded = reader.GetBoolean( start + 5 );			
				if(!reader.IsDBNull(6)) trackingNumberSettingObject.IsPrompt = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) trackingNumberSettingObject.Comments = reader.GetString( start + 7 );			
				trackingNumberSettingObject.CreatedBy = reader.GetGuid( start + 8 );			
				trackingNumberSettingObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				trackingNumberSettingObject.CreatedDate = reader.GetDateTime( start + 10 );			
				trackingNumberSettingObject.LastUpdatedDate = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) trackingNumberSettingObject.ForwardingNumber = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) trackingNumberSettingObject.SubAccountId = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) trackingNumberSettingObject.SubAccountToken = reader.GetString( start + 14 );			
			FillBaseObject(trackingNumberSettingObject, reader, (start + 15));

			
			trackingNumberSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TrackingNumberSetting object
        /// </summary>
        /// <param name="trackingNumberSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TrackingNumberSettingBase trackingNumberSettingObject, SqlDataReader reader)
		{
			FillObject(trackingNumberSettingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TrackingNumberSetting object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TrackingNumberSetting object</returns>
		private TrackingNumberSetting GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TrackingNumberSetting trackingNumberSettingObject= new TrackingNumberSetting();
					FillObject(trackingNumberSettingObject, reader);
					return trackingNumberSettingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TrackingNumberSetting objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TrackingNumberSetting objects</returns>
		private TrackingNumberSettingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TrackingNumberSetting list
			TrackingNumberSettingList list = new TrackingNumberSettingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TrackingNumberSetting trackingNumberSettingObject = new TrackingNumberSetting();
					FillObject(trackingNumberSettingObject, reader);

					list.Add(trackingNumberSettingObject);
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
