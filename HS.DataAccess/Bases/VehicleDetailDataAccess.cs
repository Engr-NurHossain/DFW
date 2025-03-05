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
	public partial class VehicleDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTVEHICLEDETAIL = "InsertVehicleDetail";
		private const string UPDATEVEHICLEDETAIL = "UpdateVehicleDetail";
		private const string DELETEVEHICLEDETAIL = "DeleteVehicleDetail";
		private const string GETVEHICLEDETAILBYID = "GetVehicleDetailById";
		private const string GETALLVEHICLEDETAIL = "GetAllVehicleDetail";
		private const string GETPAGEDVEHICLEDETAIL = "GetPagedVehicleDetail";
		private const string GETVEHICLEDETAILMAXIMUMID = "GetVehicleDetailMaximumId";
		private const string GETVEHICLEDETAILROWCOUNT = "GetVehicleDetailRowCount";	
		private const string GETVEHICLEDETAILBYQUERY = "GetVehicleDetailByQuery";
		#endregion
		
		#region Constructors
		public VehicleDetailDataAccess(ClientContext context) : base(context) { }
		public VehicleDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="vehicleDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, VehicleDetailBase vehicleDetailObject)
		{	
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_VehicleNo, 150, vehicleDetailObject.VehicleNo));
			AddParameter(cmd, pGuid(VehicleDetailBase.Property_VehicleId, vehicleDetailObject.VehicleId));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_VIN, 50, vehicleDetailObject.VIN));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_LicenseNO, 50, vehicleDetailObject.LicenseNO));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_Year, 50, vehicleDetailObject.Year));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_Make, 50, vehicleDetailObject.Make));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_Model, 50, vehicleDetailObject.Model));
			AddParameter(cmd, pGuid(VehicleDetailBase.Property_UserId, vehicleDetailObject.UserId));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_MillageData, 50, vehicleDetailObject.MillageData));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_ExpirationTag, 50, vehicleDetailObject.ExpirationTag));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_TollTag, 50, vehicleDetailObject.TollTag));
			AddParameter(cmd, pGuid(VehicleDetailBase.Property_TechnicianId, vehicleDetailObject.TechnicianId));
			AddParameter(cmd, pNVarChar(VehicleDetailBase.Property_QuickBookNo, 50, vehicleDetailObject.QuickBookNo));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts VehicleDetail
        /// </summary>
        /// <param name="vehicleDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(VehicleDetailBase vehicleDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTVEHICLEDETAIL);
	
				AddParameter(cmd, pInt32Out(VehicleDetailBase.Property_Id));
				AddCommonParams(cmd, vehicleDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					vehicleDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					vehicleDetailObject.Id = (Int32)GetOutParameter(cmd, VehicleDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(vehicleDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates VehicleDetail
        /// </summary>
        /// <param name="vehicleDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(VehicleDetailBase vehicleDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEVEHICLEDETAIL);
				
				AddParameter(cmd, pInt32(VehicleDetailBase.Property_Id, vehicleDetailObject.Id));
				AddCommonParams(cmd, vehicleDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					vehicleDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(vehicleDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes VehicleDetail
        /// </summary>
        /// <param name="Id">Id of the VehicleDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEVEHICLEDETAIL);	
				
				AddParameter(cmd, pInt32(VehicleDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(VehicleDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves VehicleDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the VehicleDetail object to retrieve</param>
        /// <returns>VehicleDetail object, null if not found</returns>
		public VehicleDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEDETAILBYID))
			{
				AddParameter( cmd, pInt32(VehicleDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all VehicleDetail objects 
        /// </summary>
        /// <returns>A list of VehicleDetail objects</returns>
		public VehicleDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLVEHICLEDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all VehicleDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of VehicleDetail objects</returns>
		public VehicleDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDVEHICLEDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				VehicleDetailList _VehicleDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _VehicleDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all VehicleDetail objects by query String
        /// </summary>
        /// <returns>A list of VehicleDetail objects</returns>
		public VehicleDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get VehicleDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of VehicleDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get VehicleDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of VehicleDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _VehicleDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETVEHICLEDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_VehicleDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _VehicleDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills VehicleDetail object
        /// </summary>
        /// <param name="vehicleDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(VehicleDetailBase vehicleDetailObject, SqlDataReader reader, int start)
		{
			
				vehicleDetailObject.Id = reader.GetInt32( start + 0 );			
				vehicleDetailObject.VehicleNo = reader.GetString( start + 1 );			
				vehicleDetailObject.VehicleId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) vehicleDetailObject.VIN = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) vehicleDetailObject.LicenseNO = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) vehicleDetailObject.Year = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) vehicleDetailObject.Make = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) vehicleDetailObject.Model = reader.GetString( start + 7 );			
				vehicleDetailObject.UserId = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) vehicleDetailObject.MillageData = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) vehicleDetailObject.ExpirationTag = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) vehicleDetailObject.TollTag = reader.GetString( start + 11 );			
				vehicleDetailObject.TechnicianId = reader.GetGuid( start + 12 );			
				if(!reader.IsDBNull(13)) vehicleDetailObject.QuickBookNo = reader.GetString( start + 13 );			
			FillBaseObject(vehicleDetailObject, reader, (start + 14));

			
			vehicleDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills VehicleDetail object
        /// </summary>
        /// <param name="vehicleDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(VehicleDetailBase vehicleDetailObject, SqlDataReader reader)
		{
			FillObject(vehicleDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves VehicleDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>VehicleDetail object</returns>
		private VehicleDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					VehicleDetail vehicleDetailObject= new VehicleDetail();
					FillObject(vehicleDetailObject, reader);
					return vehicleDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of VehicleDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of VehicleDetail objects</returns>
		private VehicleDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//VehicleDetail list
			VehicleDetailList list = new VehicleDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					VehicleDetail vehicleDetailObject = new VehicleDetail();
					FillObject(vehicleDetailObject, reader);

					list.Add(vehicleDetailObject);
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
