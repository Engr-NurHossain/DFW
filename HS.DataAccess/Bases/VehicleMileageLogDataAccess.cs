using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class VehicleMileageLogDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTVEHICLEMILEAGELOG = "InsertVehicleMileageLog";
		private const string UPDATEVEHICLEMILEAGELOG = "UpdateVehicleMileageLog";
		private const string DELETEVEHICLEMILEAGELOG = "DeleteVehicleMileageLog";
		private const string GETVEHICLEMILEAGELOGBYID = "GetVehicleMileageLogById";
		private const string GETALLVEHICLEMILEAGELOG = "GetAllVehicleMileageLog";
		private const string GETPAGEDVEHICLEMILEAGELOG = "GetPagedVehicleMileageLog";
		private const string GETVEHICLEMILEAGELOGMAXIMUMID = "GetVehicleMileageLogMaximumId";
		private const string GETVEHICLEMILEAGELOGROWCOUNT = "GetVehicleMileageLogRowCount";	
		private const string GETVEHICLEMILEAGELOGBYQUERY = "GetVehicleMileageLogByQuery";
		#endregion
		
		#region Constructors
		public VehicleMileageLogDataAccess(ClientContext context) : base(context) { }
		public VehicleMileageLogDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        
        #endregion

        #region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="vehicleMileageLogObject"></param>
        private void AddCommonParams(SqlCommand cmd, VehicleMileageLogBase vehicleMileageLogObject)
		{	
			AddParameter(cmd, pGuid(VehicleMileageLogBase.Property_VehicleId, vehicleMileageLogObject.VehicleId));
			AddParameter(cmd, pDouble(VehicleMileageLogBase.Property_Mileage, vehicleMileageLogObject.Mileage));
			AddParameter(cmd, pBool(VehicleMileageLogBase.Property_ExteriorClean, vehicleMileageLogObject.ExteriorClean));
			AddParameter(cmd, pBool(VehicleMileageLogBase.Property_InteriorClean, vehicleMileageLogObject.InteriorClean));
			AddParameter(cmd, pBool(VehicleMileageLogBase.Property_Vaccumed, vehicleMileageLogObject.Vaccumed));
			AddParameter(cmd, pBool(VehicleMileageLogBase.Property_EquipmentOrganized, vehicleMileageLogObject.EquipmentOrganized));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts VehicleMileageLog
        /// </summary>
        /// <param name="vehicleMileageLogObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(VehicleMileageLogBase vehicleMileageLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTVEHICLEMILEAGELOG);
	
				AddParameter(cmd, pInt32Out(VehicleMileageLogBase.Property_Id));
				AddCommonParams(cmd, vehicleMileageLogObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					vehicleMileageLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					vehicleMileageLogObject.Id = (Int32)GetOutParameter(cmd, VehicleMileageLogBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(vehicleMileageLogObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates VehicleMileageLog
        /// </summary>
        /// <param name="vehicleMileageLogObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(VehicleMileageLogBase vehicleMileageLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEVEHICLEMILEAGELOG);
				
				AddParameter(cmd, pInt32(VehicleMileageLogBase.Property_Id, vehicleMileageLogObject.Id));
				AddCommonParams(cmd, vehicleMileageLogObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					vehicleMileageLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(vehicleMileageLogObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes VehicleMileageLog
        /// </summary>
        /// <param name="Id">Id of the VehicleMileageLog object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEVEHICLEMILEAGELOG);	
				
				AddParameter(cmd, pInt32(VehicleMileageLogBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(VehicleMileageLog), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves VehicleMileageLog object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the VehicleMileageLog object to retrieve</param>
        /// <returns>VehicleMileageLog object, null if not found</returns>
		public VehicleMileageLog Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEMILEAGELOGBYID))
			{
				AddParameter( cmd, pInt32(VehicleMileageLogBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all VehicleMileageLog objects 
        /// </summary>
        /// <returns>A list of VehicleMileageLog objects</returns>
		public VehicleMileageLogList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLVEHICLEMILEAGELOG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all VehicleMileageLog objects by PageRequest
        /// </summary>
        /// <returns>A list of VehicleMileageLog objects</returns>
		public VehicleMileageLogList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDVEHICLEMILEAGELOG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				VehicleMileageLogList _VehicleMileageLogList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _VehicleMileageLogList;
			}
		}
		
		/// <summary>
        /// Retrieves all VehicleMileageLog objects by query String
        /// </summary>
        /// <returns>A list of VehicleMileageLog objects</returns>
		public VehicleMileageLogList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEMILEAGELOGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get VehicleMileageLog Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of VehicleMileageLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEMILEAGELOGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get VehicleMileageLog Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of VehicleMileageLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _VehicleMileageLogRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEMILEAGELOGROWCOUNT))
			{
				SqlDataReader reader;
				_VehicleMileageLogRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _VehicleMileageLogRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills VehicleMileageLog object
        /// </summary>
        /// <param name="vehicleMileageLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(VehicleMileageLogBase vehicleMileageLogObject, SqlDataReader reader, int start)
		{
			
				vehicleMileageLogObject.Id = reader.GetInt32( start + 0 );			
				vehicleMileageLogObject.VehicleId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) vehicleMileageLogObject.Mileage = reader.GetDouble( start + 2 );			
				if(!reader.IsDBNull(3)) vehicleMileageLogObject.ExteriorClean = reader.GetBoolean( start + 3 );			
				if(!reader.IsDBNull(4)) vehicleMileageLogObject.InteriorClean = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) vehicleMileageLogObject.Vaccumed = reader.GetBoolean( start + 5 );			
				if(!reader.IsDBNull(6)) vehicleMileageLogObject.EquipmentOrganized = reader.GetBoolean( start + 6 );			
			FillBaseObject(vehicleMileageLogObject, reader, (start + 7));

			
			vehicleMileageLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills VehicleMileageLog object
        /// </summary>
        /// <param name="vehicleMileageLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(VehicleMileageLogBase vehicleMileageLogObject, SqlDataReader reader)
		{
			FillObject(vehicleMileageLogObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves VehicleMileageLog object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>VehicleMileageLog object</returns>
		private VehicleMileageLog GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					VehicleMileageLog vehicleMileageLogObject= new VehicleMileageLog();
					FillObject(vehicleMileageLogObject, reader);
					return vehicleMileageLogObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of VehicleMileageLog objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of VehicleMileageLog objects</returns>
		private VehicleMileageLogList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//VehicleMileageLog list
			VehicleMileageLogList list = new VehicleMileageLogList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					VehicleMileageLog vehicleMileageLogObject = new VehicleMileageLog();
					FillObject(vehicleMileageLogObject, reader);

					list.Add(vehicleMileageLogObject);
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
