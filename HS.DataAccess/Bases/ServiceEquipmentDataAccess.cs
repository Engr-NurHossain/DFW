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
	public partial class ServiceEquipmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSERVICEEQUIPMENT = "InsertServiceEquipment";
		private const string UPDATESERVICEEQUIPMENT = "UpdateServiceEquipment";
		private const string DELETESERVICEEQUIPMENT = "DeleteServiceEquipment";
		private const string GETSERVICEEQUIPMENTBYID = "GetServiceEquipmentById";
		private const string GETALLSERVICEEQUIPMENT = "GetAllServiceEquipment";
		private const string GETPAGEDSERVICEEQUIPMENT = "GetPagedServiceEquipment";
		private const string GETSERVICEEQUIPMENTMAXIMUMID = "GetServiceEquipmentMaximumId";
		private const string GETSERVICEEQUIPMENTROWCOUNT = "GetServiceEquipmentRowCount";	
		private const string GETSERVICEEQUIPMENTBYQUERY = "GetServiceEquipmentByQuery";
		#endregion
		
		#region Constructors
		public ServiceEquipmentDataAccess(ClientContext context) : base(context) { }
		public ServiceEquipmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="serviceEquipmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, ServiceEquipmentBase serviceEquipmentObject)
		{	
			AddParameter(cmd, pGuid(ServiceEquipmentBase.Property_CompanyId, serviceEquipmentObject.CompanyId));
			AddParameter(cmd, pGuid(ServiceEquipmentBase.Property_ServiceId, serviceEquipmentObject.ServiceId));
			AddParameter(cmd, pGuid(ServiceEquipmentBase.Property_EquipmentId, serviceEquipmentObject.EquipmentId));
			AddParameter(cmd, pInt32(ServiceEquipmentBase.Property_Quantity, serviceEquipmentObject.Quantity));
			AddParameter(cmd, pDouble(ServiceEquipmentBase.Property_RetailPrice, serviceEquipmentObject.RetailPrice));
			AddParameter(cmd, pGuid(ServiceEquipmentBase.Property_CreatedBy, serviceEquipmentObject.CreatedBy));
			AddParameter(cmd, pDateTime(ServiceEquipmentBase.Property_CreatedDate, serviceEquipmentObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ServiceEquipment
        /// </summary>
        /// <param name="serviceEquipmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ServiceEquipmentBase serviceEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSERVICEEQUIPMENT);
	
				AddParameter(cmd, pInt32Out(ServiceEquipmentBase.Property_Id));
				AddCommonParams(cmd, serviceEquipmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					serviceEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					serviceEquipmentObject.Id = (Int32)GetOutParameter(cmd, ServiceEquipmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(serviceEquipmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ServiceEquipment
        /// </summary>
        /// <param name="serviceEquipmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ServiceEquipmentBase serviceEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESERVICEEQUIPMENT);
				
				AddParameter(cmd, pInt32(ServiceEquipmentBase.Property_Id, serviceEquipmentObject.Id));
				AddCommonParams(cmd, serviceEquipmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					serviceEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(serviceEquipmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ServiceEquipment
        /// </summary>
        /// <param name="Id">Id of the ServiceEquipment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESERVICEEQUIPMENT);	
				
				AddParameter(cmd, pInt32(ServiceEquipmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ServiceEquipment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ServiceEquipment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ServiceEquipment object to retrieve</param>
        /// <returns>ServiceEquipment object, null if not found</returns>
		public ServiceEquipment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEEQUIPMENTBYID))
			{
				AddParameter( cmd, pInt32(ServiceEquipmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ServiceEquipment objects 
        /// </summary>
        /// <returns>A list of ServiceEquipment objects</returns>
		public ServiceEquipmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSERVICEEQUIPMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ServiceEquipment objects by PageRequest
        /// </summary>
        /// <returns>A list of ServiceEquipment objects</returns>
		public ServiceEquipmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSERVICEEQUIPMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ServiceEquipmentList _ServiceEquipmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ServiceEquipmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all ServiceEquipment objects by query String
        /// </summary>
        /// <returns>A list of ServiceEquipment objects</returns>
		public ServiceEquipmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEEQUIPMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ServiceEquipment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ServiceEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEEQUIPMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ServiceEquipment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ServiceEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ServiceEquipmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEEQUIPMENTROWCOUNT))
			{
				SqlDataReader reader;
				_ServiceEquipmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ServiceEquipmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ServiceEquipment object
        /// </summary>
        /// <param name="serviceEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ServiceEquipmentBase serviceEquipmentObject, SqlDataReader reader, int start)
		{
			
				serviceEquipmentObject.Id = reader.GetInt32( start + 0 );			
				serviceEquipmentObject.CompanyId = reader.GetGuid( start + 1 );			
				serviceEquipmentObject.ServiceId = reader.GetGuid( start + 2 );			
				serviceEquipmentObject.EquipmentId = reader.GetGuid( start + 3 );			
				serviceEquipmentObject.Quantity = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) serviceEquipmentObject.RetailPrice = reader.GetDouble( start + 5 );			
				serviceEquipmentObject.CreatedBy = reader.GetGuid( start + 6 );			
				serviceEquipmentObject.CreatedDate = reader.GetDateTime( start + 7 );			
			FillBaseObject(serviceEquipmentObject, reader, (start + 8));

			
			serviceEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ServiceEquipment object
        /// </summary>
        /// <param name="serviceEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ServiceEquipmentBase serviceEquipmentObject, SqlDataReader reader)
		{
			FillObject(serviceEquipmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ServiceEquipment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ServiceEquipment object</returns>
		private ServiceEquipment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ServiceEquipment serviceEquipmentObject= new ServiceEquipment();
					FillObject(serviceEquipmentObject, reader);
					return serviceEquipmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ServiceEquipment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ServiceEquipment objects</returns>
		private ServiceEquipmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ServiceEquipment list
			ServiceEquipmentList list = new ServiceEquipmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ServiceEquipment serviceEquipmentObject = new ServiceEquipment();
					FillObject(serviceEquipmentObject, reader);

					list.Add(serviceEquipmentObject);
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
