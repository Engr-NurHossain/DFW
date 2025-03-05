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
	public partial class TrackingNumberRecordedDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTRACKINGNUMBERRECORDED = "InsertTrackingNumberRecorded";
		private const string UPDATETRACKINGNUMBERRECORDED = "UpdateTrackingNumberRecorded";
		private const string DELETETRACKINGNUMBERRECORDED = "DeleteTrackingNumberRecorded";
		private const string GETTRACKINGNUMBERRECORDEDBYID = "GetTrackingNumberRecordedById";
		private const string GETALLTRACKINGNUMBERRECORDED = "GetAllTrackingNumberRecorded";
		private const string GETPAGEDTRACKINGNUMBERRECORDED = "GetPagedTrackingNumberRecorded";
		private const string GETTRACKINGNUMBERRECORDEDMAXIMUMID = "GetTrackingNumberRecordedMaximumId";
		private const string GETTRACKINGNUMBERRECORDEDROWCOUNT = "GetTrackingNumberRecordedRowCount";	
		private const string GETTRACKINGNUMBERRECORDEDBYQUERY = "GetTrackingNumberRecordedByQuery";
		#endregion
		
		#region Constructors
		public TrackingNumberRecordedDataAccess(ClientContext context) : base(context) { }
		public TrackingNumberRecordedDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="trackingNumberRecordedObject"></param>
		private void AddCommonParams(SqlCommand cmd, TrackingNumberRecordedBase trackingNumberRecordedObject)
		{	
			AddParameter(cmd, pGuid(TrackingNumberRecordedBase.Property_TrackingId, trackingNumberRecordedObject.TrackingId));
			AddParameter(cmd, pGuid(TrackingNumberRecordedBase.Property_CallerId, trackingNumberRecordedObject.CallerId));
			AddParameter(cmd, pGuid(TrackingNumberRecordedBase.Property_CompanyId, trackingNumberRecordedObject.CompanyId));
			AddParameter(cmd, pNVarChar(TrackingNumberRecordedBase.Property_TrackingNumber, 250, trackingNumberRecordedObject.TrackingNumber));
			AddParameter(cmd, pNVarChar(TrackingNumberRecordedBase.Property_CallerNumber, 250, trackingNumberRecordedObject.CallerNumber));
			AddParameter(cmd, pNVarChar(TrackingNumberRecordedBase.Property_Status, 150, trackingNumberRecordedObject.Status));
			AddParameter(cmd, pDateTime(TrackingNumberRecordedBase.Property_RecordDate, trackingNumberRecordedObject.RecordDate));
			AddParameter(cmd, pGuid(TrackingNumberRecordedBase.Property_CreatedBy, trackingNumberRecordedObject.CreatedBy));
			AddParameter(cmd, pGuid(TrackingNumberRecordedBase.Property_LastUpdatedBy, trackingNumberRecordedObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(TrackingNumberRecordedBase.Property_CreatedDate, trackingNumberRecordedObject.CreatedDate));
			AddParameter(cmd, pDateTime(TrackingNumberRecordedBase.Property_LastUpdatedDate, trackingNumberRecordedObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(TrackingNumberRecordedBase.Property_CustomerId, trackingNumberRecordedObject.CustomerId));
			AddParameter(cmd, pNVarChar(TrackingNumberRecordedBase.Property_RecordFile, trackingNumberRecordedObject.RecordFile));
			AddParameter(cmd, pNVarChar(TrackingNumberRecordedBase.Property_TalkTimeSeconds, trackingNumberRecordedObject.TalkTimeSeconds));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TrackingNumberRecorded
        /// </summary>
        /// <param name="trackingNumberRecordedObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TrackingNumberRecordedBase trackingNumberRecordedObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTRACKINGNUMBERRECORDED);
	
				AddParameter(cmd, pInt32Out(TrackingNumberRecordedBase.Property_Id));
				AddCommonParams(cmd, trackingNumberRecordedObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					trackingNumberRecordedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					trackingNumberRecordedObject.Id = (Int32)GetOutParameter(cmd, TrackingNumberRecordedBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(trackingNumberRecordedObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TrackingNumberRecorded
        /// </summary>
        /// <param name="trackingNumberRecordedObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TrackingNumberRecordedBase trackingNumberRecordedObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETRACKINGNUMBERRECORDED);
				
				AddParameter(cmd, pInt32(TrackingNumberRecordedBase.Property_Id, trackingNumberRecordedObject.Id));
				AddCommonParams(cmd, trackingNumberRecordedObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					trackingNumberRecordedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(trackingNumberRecordedObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TrackingNumberRecorded
        /// </summary>
        /// <param name="Id">Id of the TrackingNumberRecorded object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETRACKINGNUMBERRECORDED);	
				
				AddParameter(cmd, pInt32(TrackingNumberRecordedBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TrackingNumberRecorded), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TrackingNumberRecorded object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TrackingNumberRecorded object to retrieve</param>
        /// <returns>TrackingNumberRecorded object, null if not found</returns>
		public TrackingNumberRecorded Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRACKINGNUMBERRECORDEDBYID))
			{
				AddParameter( cmd, pInt32(TrackingNumberRecordedBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TrackingNumberRecorded objects 
        /// </summary>
        /// <returns>A list of TrackingNumberRecorded objects</returns>
		public TrackingNumberRecordedList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTRACKINGNUMBERRECORDED))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TrackingNumberRecorded objects by PageRequest
        /// </summary>
        /// <returns>A list of TrackingNumberRecorded objects</returns>
		public TrackingNumberRecordedList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTRACKINGNUMBERRECORDED))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TrackingNumberRecordedList _TrackingNumberRecordedList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TrackingNumberRecordedList;
			}
		}
		
		/// <summary>
        /// Retrieves all TrackingNumberRecorded objects by query String
        /// </summary>
        /// <returns>A list of TrackingNumberRecorded objects</returns>
		public TrackingNumberRecordedList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRACKINGNUMBERRECORDEDBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TrackingNumberRecorded Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TrackingNumberRecorded
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRACKINGNUMBERRECORDEDMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TrackingNumberRecorded Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TrackingNumberRecorded
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TrackingNumberRecordedRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRACKINGNUMBERRECORDEDROWCOUNT))
			{
				SqlDataReader reader;
				_TrackingNumberRecordedRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TrackingNumberRecordedRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TrackingNumberRecorded object
        /// </summary>
        /// <param name="trackingNumberRecordedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TrackingNumberRecordedBase trackingNumberRecordedObject, SqlDataReader reader, int start)
		{
			
				trackingNumberRecordedObject.Id = reader.GetInt32( start + 0 );			
				trackingNumberRecordedObject.TrackingId = reader.GetGuid( start + 1 );			
				trackingNumberRecordedObject.CallerId = reader.GetGuid( start + 2 );			
				trackingNumberRecordedObject.CompanyId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) trackingNumberRecordedObject.TrackingNumber = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) trackingNumberRecordedObject.CallerNumber = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) trackingNumberRecordedObject.Status = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) trackingNumberRecordedObject.RecordDate = reader.GetDateTime( start + 7 );			
				trackingNumberRecordedObject.CreatedBy = reader.GetGuid( start + 8 );			
				trackingNumberRecordedObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				trackingNumberRecordedObject.CreatedDate = reader.GetDateTime( start + 10 );			
				trackingNumberRecordedObject.LastUpdatedDate = reader.GetDateTime( start + 11 );			
				trackingNumberRecordedObject.CustomerId = reader.GetGuid( start + 12 );			
				if(!reader.IsDBNull(13)) trackingNumberRecordedObject.RecordFile = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) trackingNumberRecordedObject.TalkTimeSeconds = reader.GetString( start + 14 );			
			FillBaseObject(trackingNumberRecordedObject, reader, (start + 15));

			
			trackingNumberRecordedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TrackingNumberRecorded object
        /// </summary>
        /// <param name="trackingNumberRecordedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TrackingNumberRecordedBase trackingNumberRecordedObject, SqlDataReader reader)
		{
			FillObject(trackingNumberRecordedObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TrackingNumberRecorded object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TrackingNumberRecorded object</returns>
		private TrackingNumberRecorded GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TrackingNumberRecorded trackingNumberRecordedObject= new TrackingNumberRecorded();
					FillObject(trackingNumberRecordedObject, reader);
					return trackingNumberRecordedObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TrackingNumberRecorded objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TrackingNumberRecorded objects</returns>
		private TrackingNumberRecordedList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TrackingNumberRecorded list
			TrackingNumberRecordedList list = new TrackingNumberRecordedList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TrackingNumberRecorded trackingNumberRecordedObject = new TrackingNumberRecorded();
					FillObject(trackingNumberRecordedObject, reader);

					list.Add(trackingNumberRecordedObject);
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
