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
	public partial class AlarmCustomerTerminationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTALARMCUSTOMERTERMINATION = "InsertAlarmCustomerTermination";
		private const string UPDATEALARMCUSTOMERTERMINATION = "UpdateAlarmCustomerTermination";
		private const string DELETEALARMCUSTOMERTERMINATION = "DeleteAlarmCustomerTermination";
		private const string GETALARMCUSTOMERTERMINATIONBYID = "GetAlarmCustomerTerminationById";
		private const string GETALLALARMCUSTOMERTERMINATION = "GetAllAlarmCustomerTermination";
		private const string GETPAGEDALARMCUSTOMERTERMINATION = "GetPagedAlarmCustomerTermination";
		private const string GETALARMCUSTOMERTERMINATIONMAXIMUMID = "GetAlarmCustomerTerminationMaximumId";
		private const string GETALARMCUSTOMERTERMINATIONROWCOUNT = "GetAlarmCustomerTerminationRowCount";	
		private const string GETALARMCUSTOMERTERMINATIONBYQUERY = "GetAlarmCustomerTerminationByQuery";
		#endregion
		
		#region Constructors
		public AlarmCustomerTerminationDataAccess(ClientContext context) : base(context) { }
		public AlarmCustomerTerminationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="alarmCustomerTerminationObject"></param>
		private void AddCommonParams(SqlCommand cmd, AlarmCustomerTerminationBase alarmCustomerTerminationObject)
		{	
			AddParameter(cmd, pGuid(AlarmCustomerTerminationBase.Property_CustomerId, alarmCustomerTerminationObject.CustomerId));
			AddParameter(cmd, pInt32(AlarmCustomerTerminationBase.Property_AlarmId, alarmCustomerTerminationObject.AlarmId));
			AddParameter(cmd, pDateTime(AlarmCustomerTerminationBase.Property_TerminationDate, alarmCustomerTerminationObject.TerminationDate));
			AddParameter(cmd, pNVarChar(AlarmCustomerTerminationBase.Property_TerminationReason, alarmCustomerTerminationObject.TerminationReason));
			AddParameter(cmd, pGuid(AlarmCustomerTerminationBase.Property_CreatedBy, alarmCustomerTerminationObject.CreatedBy));
			AddParameter(cmd, pDateTime(AlarmCustomerTerminationBase.Property_CreatedDate, alarmCustomerTerminationObject.CreatedDate));
			AddParameter(cmd, pBool(AlarmCustomerTerminationBase.Property_IsMsgSend, alarmCustomerTerminationObject.IsMsgSend));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AlarmCustomerTermination
        /// </summary>
        /// <param name="alarmCustomerTerminationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AlarmCustomerTerminationBase alarmCustomerTerminationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTALARMCUSTOMERTERMINATION);
	
				AddParameter(cmd, pInt32Out(AlarmCustomerTerminationBase.Property_Id));
				AddCommonParams(cmd, alarmCustomerTerminationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					alarmCustomerTerminationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					alarmCustomerTerminationObject.Id = (Int32)GetOutParameter(cmd, AlarmCustomerTerminationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(alarmCustomerTerminationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AlarmCustomerTermination
        /// </summary>
        /// <param name="alarmCustomerTerminationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AlarmCustomerTerminationBase alarmCustomerTerminationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEALARMCUSTOMERTERMINATION);
				
				AddParameter(cmd, pInt32(AlarmCustomerTerminationBase.Property_Id, alarmCustomerTerminationObject.Id));
				AddCommonParams(cmd, alarmCustomerTerminationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					alarmCustomerTerminationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(alarmCustomerTerminationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AlarmCustomerTermination
        /// </summary>
        /// <param name="Id">Id of the AlarmCustomerTermination object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEALARMCUSTOMERTERMINATION);	
				
				AddParameter(cmd, pInt32(AlarmCustomerTerminationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AlarmCustomerTermination), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AlarmCustomerTermination object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AlarmCustomerTermination object to retrieve</param>
        /// <returns>AlarmCustomerTermination object, null if not found</returns>
		public AlarmCustomerTermination Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETALARMCUSTOMERTERMINATIONBYID))
			{
				AddParameter( cmd, pInt32(AlarmCustomerTerminationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AlarmCustomerTermination objects 
        /// </summary>
        /// <returns>A list of AlarmCustomerTermination objects</returns>
		public AlarmCustomerTerminationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLALARMCUSTOMERTERMINATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AlarmCustomerTermination objects by PageRequest
        /// </summary>
        /// <returns>A list of AlarmCustomerTermination objects</returns>
		public AlarmCustomerTerminationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDALARMCUSTOMERTERMINATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AlarmCustomerTerminationList _AlarmCustomerTerminationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AlarmCustomerTerminationList;
			}
		}
		
		/// <summary>
        /// Retrieves all AlarmCustomerTermination objects by query String
        /// </summary>
        /// <returns>A list of AlarmCustomerTermination objects</returns>
		public AlarmCustomerTerminationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETALARMCUSTOMERTERMINATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AlarmCustomerTermination Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AlarmCustomerTermination
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETALARMCUSTOMERTERMINATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AlarmCustomerTermination Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AlarmCustomerTermination
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AlarmCustomerTerminationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETALARMCUSTOMERTERMINATIONROWCOUNT))
			{
				SqlDataReader reader;
				_AlarmCustomerTerminationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AlarmCustomerTerminationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AlarmCustomerTermination object
        /// </summary>
        /// <param name="alarmCustomerTerminationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AlarmCustomerTerminationBase alarmCustomerTerminationObject, SqlDataReader reader, int start)
		{
			
				alarmCustomerTerminationObject.Id = reader.GetInt32( start + 0 );			
				alarmCustomerTerminationObject.CustomerId = reader.GetGuid( start + 1 );			
				alarmCustomerTerminationObject.AlarmId = reader.GetInt32( start + 2 );			
				alarmCustomerTerminationObject.TerminationDate = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) alarmCustomerTerminationObject.TerminationReason = reader.GetString( start + 4 );			
				alarmCustomerTerminationObject.CreatedBy = reader.GetGuid( start + 5 );			
				alarmCustomerTerminationObject.CreatedDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) alarmCustomerTerminationObject.IsMsgSend = reader.GetBoolean( start + 7 );			
			FillBaseObject(alarmCustomerTerminationObject, reader, (start + 8));

			
			alarmCustomerTerminationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AlarmCustomerTermination object
        /// </summary>
        /// <param name="alarmCustomerTerminationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AlarmCustomerTerminationBase alarmCustomerTerminationObject, SqlDataReader reader)
		{
			FillObject(alarmCustomerTerminationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AlarmCustomerTermination object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AlarmCustomerTermination object</returns>
		private AlarmCustomerTermination GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AlarmCustomerTermination alarmCustomerTerminationObject= new AlarmCustomerTermination();
					FillObject(alarmCustomerTerminationObject, reader);
					return alarmCustomerTerminationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AlarmCustomerTermination objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AlarmCustomerTermination objects</returns>
		private AlarmCustomerTerminationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AlarmCustomerTermination list
			AlarmCustomerTerminationList list = new AlarmCustomerTerminationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AlarmCustomerTermination alarmCustomerTerminationObject = new AlarmCustomerTermination();
					FillObject(alarmCustomerTerminationObject, reader);

					list.Add(alarmCustomerTerminationObject);
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
