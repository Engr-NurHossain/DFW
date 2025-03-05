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
	public partial class AlarmCustomerSelectedAddonDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTALARMCUSTOMERSELECTEDADDON = "InsertAlarmCustomerSelectedAddon";
		private const string UPDATEALARMCUSTOMERSELECTEDADDON = "UpdateAlarmCustomerSelectedAddon";
		private const string DELETEALARMCUSTOMERSELECTEDADDON = "DeleteAlarmCustomerSelectedAddon";
		private const string GETALARMCUSTOMERSELECTEDADDONBYID = "GetAlarmCustomerSelectedAddonById";
		private const string GETALLALARMCUSTOMERSELECTEDADDON = "GetAllAlarmCustomerSelectedAddon";
		private const string GETPAGEDALARMCUSTOMERSELECTEDADDON = "GetPagedAlarmCustomerSelectedAddon";
		private const string GETALARMCUSTOMERSELECTEDADDONMAXIMUMID = "GetAlarmCustomerSelectedAddonMaximumId";
		private const string GETALARMCUSTOMERSELECTEDADDONROWCOUNT = "GetAlarmCustomerSelectedAddonRowCount";	
		private const string GETALARMCUSTOMERSELECTEDADDONBYQUERY = "GetAlarmCustomerSelectedAddonByQuery";
		#endregion
		
		#region Constructors
		public AlarmCustomerSelectedAddonDataAccess(ClientContext context) : base(context) { }
		public AlarmCustomerSelectedAddonDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="alarmCustomerSelectedAddonObject"></param>
		private void AddCommonParams(SqlCommand cmd, AlarmCustomerSelectedAddonBase alarmCustomerSelectedAddonObject)
		{	
			AddParameter(cmd, pGuid(AlarmCustomerSelectedAddonBase.Property_CustomerId, alarmCustomerSelectedAddonObject.CustomerId));
			AddParameter(cmd, pNVarChar(AlarmCustomerSelectedAddonBase.Property_AddonType, 50, alarmCustomerSelectedAddonObject.AddonType));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AlarmCustomerSelectedAddon
        /// </summary>
        /// <param name="alarmCustomerSelectedAddonObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AlarmCustomerSelectedAddonBase alarmCustomerSelectedAddonObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTALARMCUSTOMERSELECTEDADDON);
	
				AddParameter(cmd, pInt32(AlarmCustomerSelectedAddonBase.Property_Id, alarmCustomerSelectedAddonObject.Id));
				AddCommonParams(cmd, alarmCustomerSelectedAddonObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					alarmCustomerSelectedAddonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(alarmCustomerSelectedAddonObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AlarmCustomerSelectedAddon
        /// </summary>
        /// <param name="alarmCustomerSelectedAddonObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AlarmCustomerSelectedAddonBase alarmCustomerSelectedAddonObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEALARMCUSTOMERSELECTEDADDON);
				
				AddParameter(cmd, pInt32(AlarmCustomerSelectedAddonBase.Property_Id, alarmCustomerSelectedAddonObject.Id));
				AddCommonParams(cmd, alarmCustomerSelectedAddonObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					alarmCustomerSelectedAddonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(alarmCustomerSelectedAddonObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AlarmCustomerSelectedAddon
        /// </summary>
        /// <param name="Id">Id of the AlarmCustomerSelectedAddon object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEALARMCUSTOMERSELECTEDADDON);	
				
				AddParameter(cmd, pInt32(AlarmCustomerSelectedAddonBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AlarmCustomerSelectedAddon), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AlarmCustomerSelectedAddon object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AlarmCustomerSelectedAddon object to retrieve</param>
        /// <returns>AlarmCustomerSelectedAddon object, null if not found</returns>
		public AlarmCustomerSelectedAddon Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETALARMCUSTOMERSELECTEDADDONBYID))
			{
				AddParameter( cmd, pInt32(AlarmCustomerSelectedAddonBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AlarmCustomerSelectedAddon objects 
        /// </summary>
        /// <returns>A list of AlarmCustomerSelectedAddon objects</returns>
		public AlarmCustomerSelectedAddonList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLALARMCUSTOMERSELECTEDADDON))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AlarmCustomerSelectedAddon objects by PageRequest
        /// </summary>
        /// <returns>A list of AlarmCustomerSelectedAddon objects</returns>
		public AlarmCustomerSelectedAddonList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDALARMCUSTOMERSELECTEDADDON))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AlarmCustomerSelectedAddonList _AlarmCustomerSelectedAddonList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AlarmCustomerSelectedAddonList;
			}
		}
		
		/// <summary>
        /// Retrieves all AlarmCustomerSelectedAddon objects by query String
        /// </summary>
        /// <returns>A list of AlarmCustomerSelectedAddon objects</returns>
		public AlarmCustomerSelectedAddonList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETALARMCUSTOMERSELECTEDADDONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AlarmCustomerSelectedAddon Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AlarmCustomerSelectedAddon
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETALARMCUSTOMERSELECTEDADDONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AlarmCustomerSelectedAddon Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AlarmCustomerSelectedAddon
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AlarmCustomerSelectedAddonRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETALARMCUSTOMERSELECTEDADDONROWCOUNT))
			{
				SqlDataReader reader;
				_AlarmCustomerSelectedAddonRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AlarmCustomerSelectedAddonRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AlarmCustomerSelectedAddon object
        /// </summary>
        /// <param name="alarmCustomerSelectedAddonObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AlarmCustomerSelectedAddonBase alarmCustomerSelectedAddonObject, SqlDataReader reader, int start)
		{
			
				alarmCustomerSelectedAddonObject.Id = reader.GetInt32( start + 0 );			
				alarmCustomerSelectedAddonObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) alarmCustomerSelectedAddonObject.AddonType = reader.GetString( start + 2 );			
			FillBaseObject(alarmCustomerSelectedAddonObject, reader, (start + 3));

			
			alarmCustomerSelectedAddonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AlarmCustomerSelectedAddon object
        /// </summary>
        /// <param name="alarmCustomerSelectedAddonObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AlarmCustomerSelectedAddonBase alarmCustomerSelectedAddonObject, SqlDataReader reader)
		{
			FillObject(alarmCustomerSelectedAddonObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AlarmCustomerSelectedAddon object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AlarmCustomerSelectedAddon object</returns>
		private AlarmCustomerSelectedAddon GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AlarmCustomerSelectedAddon alarmCustomerSelectedAddonObject= new AlarmCustomerSelectedAddon();
					FillObject(alarmCustomerSelectedAddonObject, reader);
					return alarmCustomerSelectedAddonObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AlarmCustomerSelectedAddon objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AlarmCustomerSelectedAddon objects</returns>
		private AlarmCustomerSelectedAddonList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AlarmCustomerSelectedAddon list
			AlarmCustomerSelectedAddonList list = new AlarmCustomerSelectedAddonList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AlarmCustomerSelectedAddon alarmCustomerSelectedAddonObject = new AlarmCustomerSelectedAddon();
					FillObject(alarmCustomerSelectedAddonObject, reader);

					list.Add(alarmCustomerSelectedAddonObject);
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
