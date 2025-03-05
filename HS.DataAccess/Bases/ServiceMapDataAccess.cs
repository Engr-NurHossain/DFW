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
	public partial class ServiceMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSERVICEMAP = "InsertServiceMap";
		private const string UPDATESERVICEMAP = "UpdateServiceMap";
		private const string DELETESERVICEMAP = "DeleteServiceMap";
		private const string GETSERVICEMAPBYID = "GetServiceMapById";
		private const string GETALLSERVICEMAP = "GetAllServiceMap";
		private const string GETPAGEDSERVICEMAP = "GetPagedServiceMap";
		private const string GETSERVICEMAPMAXIMUMID = "GetServiceMapMaximumId";
		private const string GETSERVICEMAPROWCOUNT = "GetServiceMapRowCount";	
		private const string GETSERVICEMAPBYQUERY = "GetServiceMapByQuery";
		#endregion
		
		#region Constructors
		public ServiceMapDataAccess(ClientContext context) : base(context) { }
		public ServiceMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="serviceMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, ServiceMapBase serviceMapObject)
		{	
			AddParameter(cmd, pGuid(ServiceMapBase.Property_CompanyId, serviceMapObject.CompanyId));
			AddParameter(cmd, pGuid(ServiceMapBase.Property_ManufacturerId, serviceMapObject.ManufacturerId));
			AddParameter(cmd, pGuid(ServiceMapBase.Property_LocationId, serviceMapObject.LocationId));
			AddParameter(cmd, pGuid(ServiceMapBase.Property_TypeId, serviceMapObject.TypeId));
			AddParameter(cmd, pGuid(ServiceMapBase.Property_ModelId, serviceMapObject.ModelId));
			AddParameter(cmd, pGuid(ServiceMapBase.Property_FinishId, serviceMapObject.FinishId));
			AddParameter(cmd, pGuid(ServiceMapBase.Property_CapacityId, serviceMapObject.CapacityId));
			AddParameter(cmd, pGuid(ServiceMapBase.Property_ServiceId, serviceMapObject.ServiceId));
			AddParameter(cmd, pGuid(ServiceMapBase.Property_EquipmentId, serviceMapObject.EquipmentId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ServiceMap
        /// </summary>
        /// <param name="serviceMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ServiceMapBase serviceMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSERVICEMAP);
	
				AddParameter(cmd, pInt32Out(ServiceMapBase.Property_Id));
				AddCommonParams(cmd, serviceMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					serviceMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					serviceMapObject.Id = (Int32)GetOutParameter(cmd, ServiceMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(serviceMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ServiceMap
        /// </summary>
        /// <param name="serviceMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ServiceMapBase serviceMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESERVICEMAP);
				
				AddParameter(cmd, pInt32(ServiceMapBase.Property_Id, serviceMapObject.Id));
				AddCommonParams(cmd, serviceMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					serviceMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(serviceMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ServiceMap
        /// </summary>
        /// <param name="Id">Id of the ServiceMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESERVICEMAP);	
				
				AddParameter(cmd, pInt32(ServiceMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ServiceMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ServiceMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ServiceMap object to retrieve</param>
        /// <returns>ServiceMap object, null if not found</returns>
		public ServiceMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEMAPBYID))
			{
				AddParameter( cmd, pInt32(ServiceMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ServiceMap objects 
        /// </summary>
        /// <returns>A list of ServiceMap objects</returns>
		public ServiceMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSERVICEMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ServiceMap objects by PageRequest
        /// </summary>
        /// <returns>A list of ServiceMap objects</returns>
		public ServiceMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSERVICEMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ServiceMapList _ServiceMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ServiceMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all ServiceMap objects by query String
        /// </summary>
        /// <returns>A list of ServiceMap objects</returns>
		public ServiceMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ServiceMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ServiceMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ServiceMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ServiceMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ServiceMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEMAPROWCOUNT))
			{
				SqlDataReader reader;
				_ServiceMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ServiceMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ServiceMap object
        /// </summary>
        /// <param name="serviceMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ServiceMapBase serviceMapObject, SqlDataReader reader, int start)
		{
			
				serviceMapObject.Id = reader.GetInt32( start + 0 );			
				serviceMapObject.CompanyId = reader.GetGuid( start + 1 );			
				serviceMapObject.ManufacturerId = reader.GetGuid( start + 2 );			
				serviceMapObject.LocationId = reader.GetGuid( start + 3 );			
				serviceMapObject.TypeId = reader.GetGuid( start + 4 );			
				serviceMapObject.ModelId = reader.GetGuid( start + 5 );			
				serviceMapObject.FinishId = reader.GetGuid( start + 6 );			
				serviceMapObject.CapacityId = reader.GetGuid( start + 7 );			
				serviceMapObject.ServiceId = reader.GetGuid( start + 8 );			
				serviceMapObject.EquipmentId = reader.GetGuid( start + 9 );			
			FillBaseObject(serviceMapObject, reader, (start + 10));

			
			serviceMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ServiceMap object
        /// </summary>
        /// <param name="serviceMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ServiceMapBase serviceMapObject, SqlDataReader reader)
		{
			FillObject(serviceMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ServiceMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ServiceMap object</returns>
		private ServiceMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ServiceMap serviceMapObject= new ServiceMap();
					FillObject(serviceMapObject, reader);
					return serviceMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ServiceMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ServiceMap objects</returns>
		private ServiceMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ServiceMap list
			ServiceMapList list = new ServiceMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ServiceMap serviceMapObject = new ServiceMap();
					FillObject(serviceMapObject, reader);

					list.Add(serviceMapObject);
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
