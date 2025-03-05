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
	public partial class AlarmAddOnnsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTALARMADDONNS = "InsertAlarmAddOnns";
		private const string UPDATEALARMADDONNS = "UpdateAlarmAddOnns";
		private const string DELETEALARMADDONNS = "DeleteAlarmAddOnns";
		private const string GETALARMADDONNSBYID = "GetAlarmAddOnnsById";
		private const string GETALLALARMADDONNS = "GetAllAlarmAddOnns";
		private const string GETPAGEDALARMADDONNS = "GetPagedAlarmAddOnns";
		private const string GETALARMADDONNSMAXIMUMID = "GetAlarmAddOnnsMaximumId";
		private const string GETALARMADDONNSROWCOUNT = "GetAlarmAddOnnsRowCount";	
		private const string GETALARMADDONNSBYQUERY = "GetAlarmAddOnnsByQuery";
		#endregion
		
		#region Constructors
		public AlarmAddOnnsDataAccess(ClientContext context) : base(context) { }
		public AlarmAddOnnsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="alarmAddOnnsObject"></param>
		private void AddCommonParams(SqlCommand cmd, AlarmAddOnnsBase alarmAddOnnsObject)
		{	
			AddParameter(cmd, pNVarChar(AlarmAddOnnsBase.Property_Name, 100, alarmAddOnnsObject.Name));
			AddParameter(cmd, pInt32(AlarmAddOnnsBase.Property_Value, alarmAddOnnsObject.Value));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AlarmAddOnns
        /// </summary>
        /// <param name="alarmAddOnnsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AlarmAddOnnsBase alarmAddOnnsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTALARMADDONNS);
	
				AddParameter(cmd, pInt32Out(AlarmAddOnnsBase.Property_Id));
				AddCommonParams(cmd, alarmAddOnnsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					alarmAddOnnsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					alarmAddOnnsObject.Id = (Int32)GetOutParameter(cmd, AlarmAddOnnsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(alarmAddOnnsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AlarmAddOnns
        /// </summary>
        /// <param name="alarmAddOnnsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AlarmAddOnnsBase alarmAddOnnsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEALARMADDONNS);
				
				AddParameter(cmd, pInt32(AlarmAddOnnsBase.Property_Id, alarmAddOnnsObject.Id));
				AddCommonParams(cmd, alarmAddOnnsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					alarmAddOnnsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(alarmAddOnnsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AlarmAddOnns
        /// </summary>
        /// <param name="Id">Id of the AlarmAddOnns object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEALARMADDONNS);	
				
				AddParameter(cmd, pInt32(AlarmAddOnnsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AlarmAddOnns), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AlarmAddOnns object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AlarmAddOnns object to retrieve</param>
        /// <returns>AlarmAddOnns object, null if not found</returns>
		public AlarmAddOnns Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETALARMADDONNSBYID))
			{
				AddParameter( cmd, pInt32(AlarmAddOnnsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AlarmAddOnns objects 
        /// </summary>
        /// <returns>A list of AlarmAddOnns objects</returns>
		public AlarmAddOnnsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLALARMADDONNS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AlarmAddOnns objects by PageRequest
        /// </summary>
        /// <returns>A list of AlarmAddOnns objects</returns>
		public AlarmAddOnnsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDALARMADDONNS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AlarmAddOnnsList _AlarmAddOnnsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AlarmAddOnnsList;
			}
		}
		
		/// <summary>
        /// Retrieves all AlarmAddOnns objects by query String
        /// </summary>
        /// <returns>A list of AlarmAddOnns objects</returns>
		public AlarmAddOnnsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETALARMADDONNSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AlarmAddOnns Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AlarmAddOnns
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETALARMADDONNSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AlarmAddOnns Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AlarmAddOnns
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AlarmAddOnnsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETALARMADDONNSROWCOUNT))
			{
				SqlDataReader reader;
				_AlarmAddOnnsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AlarmAddOnnsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AlarmAddOnns object
        /// </summary>
        /// <param name="alarmAddOnnsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AlarmAddOnnsBase alarmAddOnnsObject, SqlDataReader reader, int start)
		{
			
				alarmAddOnnsObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) alarmAddOnnsObject.Name = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) alarmAddOnnsObject.Value = reader.GetInt32( start + 2 );			
			FillBaseObject(alarmAddOnnsObject, reader, (start + 3));

			
			alarmAddOnnsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AlarmAddOnns object
        /// </summary>
        /// <param name="alarmAddOnnsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AlarmAddOnnsBase alarmAddOnnsObject, SqlDataReader reader)
		{
			FillObject(alarmAddOnnsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AlarmAddOnns object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AlarmAddOnns object</returns>
		private AlarmAddOnns GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AlarmAddOnns alarmAddOnnsObject= new AlarmAddOnns();
					FillObject(alarmAddOnnsObject, reader);
					return alarmAddOnnsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AlarmAddOnns objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AlarmAddOnns objects</returns>
		private AlarmAddOnnsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AlarmAddOnns list
			AlarmAddOnnsList list = new AlarmAddOnnsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AlarmAddOnns alarmAddOnnsObject = new AlarmAddOnns();
					FillObject(alarmAddOnnsObject, reader);

					list.Add(alarmAddOnnsObject);
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
