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
	public partial class VehicleRepairLogDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTVEHICLEREPAIRLOG = "InsertVehicleRepairLog";
		private const string UPDATEVEHICLEREPAIRLOG = "UpdateVehicleRepairLog";
		private const string DELETEVEHICLEREPAIRLOG = "DeleteVehicleRepairLog";
		private const string GETVEHICLEREPAIRLOGBYID = "GetVehicleRepairLogById";
		private const string GETALLVEHICLEREPAIRLOG = "GetAllVehicleRepairLog";
		private const string GETPAGEDVEHICLEREPAIRLOG = "GetPagedVehicleRepairLog";
		private const string GETVEHICLEREPAIRLOGMAXIMUMID = "GetVehicleRepairLogMaximumId";
		private const string GETVEHICLEREPAIRLOGROWCOUNT = "GetVehicleRepairLogRowCount";	
		private const string GETVEHICLEREPAIRLOGBYQUERY = "GetVehicleRepairLogByQuery";
		#endregion
		
		#region Constructors
		public VehicleRepairLogDataAccess(ClientContext context) : base(context) { }
		public VehicleRepairLogDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="vehicleRepairLogObject"></param>
		private void AddCommonParams(SqlCommand cmd, VehicleRepairLogBase vehicleRepairLogObject)
		{	
			AddParameter(cmd, pGuid(VehicleRepairLogBase.Property_VehicleId, vehicleRepairLogObject.VehicleId));
			AddParameter(cmd, pGuid(VehicleRepairLogBase.Property_Driver, vehicleRepairLogObject.Driver));
			AddParameter(cmd, pDateTime(VehicleRepairLogBase.Property_RepairDate, vehicleRepairLogObject.RepairDate));
			AddParameter(cmd, pDouble(VehicleRepairLogBase.Property_Spent, vehicleRepairLogObject.Spent));
			AddParameter(cmd, pDateTime(VehicleRepairLogBase.Property_TireRotation, vehicleRepairLogObject.TireRotation));
			AddParameter(cmd, pDateTime(VehicleRepairLogBase.Property_OilChange, vehicleRepairLogObject.OilChange));
			AddParameter(cmd, pNVarChar(VehicleRepairLogBase.Property_Note, vehicleRepairLogObject.Note));
			AddParameter(cmd, pDateTime(VehicleRepairLogBase.Property_CreatedDate, vehicleRepairLogObject.CreatedDate));
			AddParameter(cmd, pGuid(VehicleRepairLogBase.Property_CreatedByUid, vehicleRepairLogObject.CreatedByUid));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts VehicleRepairLog
        /// </summary>
        /// <param name="vehicleRepairLogObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(VehicleRepairLogBase vehicleRepairLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTVEHICLEREPAIRLOG);
	
				AddParameter(cmd, pInt32Out(VehicleRepairLogBase.Property_Id));
				AddCommonParams(cmd, vehicleRepairLogObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					vehicleRepairLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					vehicleRepairLogObject.Id = (Int32)GetOutParameter(cmd, VehicleRepairLogBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(vehicleRepairLogObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates VehicleRepairLog
        /// </summary>
        /// <param name="vehicleRepairLogObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(VehicleRepairLogBase vehicleRepairLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEVEHICLEREPAIRLOG);
				
				AddParameter(cmd, pInt32(VehicleRepairLogBase.Property_Id, vehicleRepairLogObject.Id));
				AddCommonParams(cmd, vehicleRepairLogObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					vehicleRepairLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(vehicleRepairLogObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes VehicleRepairLog
        /// </summary>
        /// <param name="Id">Id of the VehicleRepairLog object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEVEHICLEREPAIRLOG);	
				
				AddParameter(cmd, pInt32(VehicleRepairLogBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(VehicleRepairLog), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves VehicleRepairLog object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the VehicleRepairLog object to retrieve</param>
        /// <returns>VehicleRepairLog object, null if not found</returns>
		public VehicleRepairLog Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEREPAIRLOGBYID))
			{
				AddParameter( cmd, pInt32(VehicleRepairLogBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all VehicleRepairLog objects 
        /// </summary>
        /// <returns>A list of VehicleRepairLog objects</returns>
		public VehicleRepairLogList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLVEHICLEREPAIRLOG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all VehicleRepairLog objects by PageRequest
        /// </summary>
        /// <returns>A list of VehicleRepairLog objects</returns>
		public VehicleRepairLogList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDVEHICLEREPAIRLOG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				VehicleRepairLogList _VehicleRepairLogList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _VehicleRepairLogList;
			}
		}
		
		/// <summary>
        /// Retrieves all VehicleRepairLog objects by query String
        /// </summary>
        /// <returns>A list of VehicleRepairLog objects</returns>
		public VehicleRepairLogList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEREPAIRLOGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get VehicleRepairLog Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of VehicleRepairLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEREPAIRLOGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get VehicleRepairLog Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of VehicleRepairLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _VehicleRepairLogRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEREPAIRLOGROWCOUNT))
			{
				SqlDataReader reader;
				_VehicleRepairLogRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _VehicleRepairLogRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills VehicleRepairLog object
        /// </summary>
        /// <param name="vehicleRepairLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(VehicleRepairLogBase vehicleRepairLogObject, SqlDataReader reader, int start)
		{
			
				vehicleRepairLogObject.Id = reader.GetInt32( start + 0 );			
				vehicleRepairLogObject.VehicleId = reader.GetGuid( start + 1 );			
				vehicleRepairLogObject.Driver = reader.GetGuid( start + 2 );			
				vehicleRepairLogObject.RepairDate = reader.GetDateTime( start + 3 );			
				vehicleRepairLogObject.Spent = reader.GetDouble( start + 4 );			
				if(!reader.IsDBNull(5)) vehicleRepairLogObject.TireRotation = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) vehicleRepairLogObject.OilChange = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) vehicleRepairLogObject.Note = reader.GetString( start + 7 );			
				vehicleRepairLogObject.CreatedDate = reader.GetDateTime( start + 8 );			
				vehicleRepairLogObject.CreatedByUid = reader.GetGuid( start + 9 );			
			FillBaseObject(vehicleRepairLogObject, reader, (start + 10));

			
			vehicleRepairLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills VehicleRepairLog object
        /// </summary>
        /// <param name="vehicleRepairLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(VehicleRepairLogBase vehicleRepairLogObject, SqlDataReader reader)
		{
			FillObject(vehicleRepairLogObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves VehicleRepairLog object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>VehicleRepairLog object</returns>
		private VehicleRepairLog GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					VehicleRepairLog vehicleRepairLogObject= new VehicleRepairLog();
					FillObject(vehicleRepairLogObject, reader);
					return vehicleRepairLogObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of VehicleRepairLog objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of VehicleRepairLog objects</returns>
		private VehicleRepairLogList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//VehicleRepairLog list
			VehicleRepairLogList list = new VehicleRepairLogList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					VehicleRepairLog vehicleRepairLogObject = new VehicleRepairLog();
					FillObject(vehicleRepairLogObject, reader);

					list.Add(vehicleRepairLogObject);
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
