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
	public partial class PackageDeviceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGEDEVICE = "InsertPackageDevice";
		private const string UPDATEPACKAGEDEVICE = "UpdatePackageDevice";
		private const string DELETEPACKAGEDEVICE = "DeletePackageDevice";
		private const string GETPACKAGEDEVICEBYID = "GetPackageDeviceById";
		private const string GETALLPACKAGEDEVICE = "GetAllPackageDevice";
		private const string GETPAGEDPACKAGEDEVICE = "GetPagedPackageDevice";
		private const string GETPACKAGEDEVICEMAXIMUMID = "GetPackageDeviceMaximumId";
		private const string GETPACKAGEDEVICEROWCOUNT = "GetPackageDeviceRowCount";	
		private const string GETPACKAGEDEVICEBYQUERY = "GetPackageDeviceByQuery";
		#endregion
		
		#region Constructors
		public PackageDeviceDataAccess(ClientContext context) : base(context) { }
		public PackageDeviceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageDeviceObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageDeviceBase packageDeviceObject)
		{	
			AddParameter(cmd, pGuid(PackageDeviceBase.Property_CompanyId, packageDeviceObject.CompanyId));
			AddParameter(cmd, pInt32(PackageDeviceBase.Property_PackageId, packageDeviceObject.PackageId));
			AddParameter(cmd, pGuid(PackageDeviceBase.Property_EquipmentId, packageDeviceObject.EquipmentId));
			AddParameter(cmd, pBool(PackageDeviceBase.Property_IsFree, packageDeviceObject.IsFree));
			AddParameter(cmd, pInt32(PackageDeviceBase.Property_EptNo, packageDeviceObject.EptNo));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageDevice
        /// </summary>
        /// <param name="packageDeviceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageDeviceBase packageDeviceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGEDEVICE);
	
				AddParameter(cmd, pInt32Out(PackageDeviceBase.Property_Id));
				AddCommonParams(cmd, packageDeviceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageDeviceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageDeviceObject.Id = (Int32)GetOutParameter(cmd, PackageDeviceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageDeviceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageDevice
        /// </summary>
        /// <param name="packageDeviceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageDeviceBase packageDeviceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGEDEVICE);
				
				AddParameter(cmd, pInt32(PackageDeviceBase.Property_Id, packageDeviceObject.Id));
				AddCommonParams(cmd, packageDeviceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageDeviceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageDeviceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageDevice
        /// </summary>
        /// <param name="Id">Id of the PackageDevice object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGEDEVICE);	
				
				AddParameter(cmd, pInt32(PackageDeviceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageDevice), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageDevice object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageDevice object to retrieve</param>
        /// <returns>PackageDevice object, null if not found</returns>
		public PackageDevice Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEDEVICEBYID))
			{
				AddParameter( cmd, pInt32(PackageDeviceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageDevice objects 
        /// </summary>
        /// <returns>A list of PackageDevice objects</returns>
		public PackageDeviceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGEDEVICE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageDevice objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageDevice objects</returns>
		public PackageDeviceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGEDEVICE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageDeviceList _PackageDeviceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageDeviceList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageDevice objects by query String
        /// </summary>
        /// <returns>A list of PackageDevice objects</returns>
		public PackageDeviceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEDEVICEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageDevice Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageDevice
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEDEVICEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageDevice Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageDevice
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageDeviceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEDEVICEROWCOUNT))
			{
				SqlDataReader reader;
				_PackageDeviceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageDeviceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageDevice object
        /// </summary>
        /// <param name="packageDeviceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageDeviceBase packageDeviceObject, SqlDataReader reader, int start)
		{
			
				packageDeviceObject.Id = reader.GetInt32( start + 0 );			
				packageDeviceObject.CompanyId = reader.GetGuid( start + 1 );			
				packageDeviceObject.PackageId = reader.GetInt32( start + 2 );			
				packageDeviceObject.EquipmentId = reader.GetGuid( start + 3 );			
				packageDeviceObject.IsFree = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) packageDeviceObject.EptNo = reader.GetInt32( start + 5 );			
			FillBaseObject(packageDeviceObject, reader, (start + 6));

			
			packageDeviceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageDevice object
        /// </summary>
        /// <param name="packageDeviceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageDeviceBase packageDeviceObject, SqlDataReader reader)
		{
			FillObject(packageDeviceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageDevice object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageDevice object</returns>
		private PackageDevice GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageDevice packageDeviceObject= new PackageDevice();
					FillObject(packageDeviceObject, reader);
					return packageDeviceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageDevice objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageDevice objects</returns>
		private PackageDeviceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageDevice list
			PackageDeviceList list = new PackageDeviceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageDevice packageDeviceObject = new PackageDevice();
					FillObject(packageDeviceObject, reader);

					list.Add(packageDeviceObject);
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
