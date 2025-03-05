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
	public partial class SmartPackageEquipmentServiceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSMARTPACKAGEEQUIPMENTSERVICE = "InsertSmartPackageEquipmentService";
		private const string UPDATESMARTPACKAGEEQUIPMENTSERVICE = "UpdateSmartPackageEquipmentService";
		private const string DELETESMARTPACKAGEEQUIPMENTSERVICE = "DeleteSmartPackageEquipmentService";
		private const string GETSMARTPACKAGEEQUIPMENTSERVICEBYID = "GetSmartPackageEquipmentServiceById";
		private const string GETALLSMARTPACKAGEEQUIPMENTSERVICE = "GetAllSmartPackageEquipmentService";
		private const string GETPAGEDSMARTPACKAGEEQUIPMENTSERVICE = "GetPagedSmartPackageEquipmentService";
		private const string GETSMARTPACKAGEEQUIPMENTSERVICEMAXIMUMID = "GetSmartPackageEquipmentServiceMaximumId";
		private const string GETSMARTPACKAGEEQUIPMENTSERVICEROWCOUNT = "GetSmartPackageEquipmentServiceRowCount";	
		private const string GETSMARTPACKAGEEQUIPMENTSERVICEBYQUERY = "GetSmartPackageEquipmentServiceByQuery";
		#endregion
		
		#region Constructors
		public SmartPackageEquipmentServiceDataAccess(ClientContext context) : base(context) { }
		public SmartPackageEquipmentServiceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="smartPackageEquipmentServiceObject"></param>
		private void AddCommonParams(SqlCommand cmd, SmartPackageEquipmentServiceBase smartPackageEquipmentServiceObject)
		{	
			AddParameter(cmd, pGuid(SmartPackageEquipmentServiceBase.Property_CompanyId, smartPackageEquipmentServiceObject.CompanyId));
			AddParameter(cmd, pGuid(SmartPackageEquipmentServiceBase.Property_PackageId, smartPackageEquipmentServiceObject.PackageId));
			AddParameter(cmd, pGuid(SmartPackageEquipmentServiceBase.Property_EquipmentId, smartPackageEquipmentServiceObject.EquipmentId));
			AddParameter(cmd, pBool(SmartPackageEquipmentServiceBase.Property_IsFree, smartPackageEquipmentServiceObject.IsFree));
			AddParameter(cmd, pInt32(SmartPackageEquipmentServiceBase.Property_EptNo, smartPackageEquipmentServiceObject.EptNo));
			AddParameter(cmd, pNVarChar(SmartPackageEquipmentServiceBase.Property_Type, 50, smartPackageEquipmentServiceObject.Type));
			AddParameter(cmd, pDouble(SmartPackageEquipmentServiceBase.Property_Price, smartPackageEquipmentServiceObject.Price));
			AddParameter(cmd, pBool(SmartPackageEquipmentServiceBase.Property_Status, smartPackageEquipmentServiceObject.Status));
			AddParameter(cmd, pGuid(SmartPackageEquipmentServiceBase.Property_LastUpdatedBy, smartPackageEquipmentServiceObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(SmartPackageEquipmentServiceBase.Property_LastUpdatedDate, smartPackageEquipmentServiceObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(SmartPackageEquipmentServiceBase.Property_SmartPackageEquipmentServiceId, smartPackageEquipmentServiceObject.SmartPackageEquipmentServiceId));
			AddParameter(cmd, pDouble(SmartPackageEquipmentServiceBase.Property_OriginalPrice, smartPackageEquipmentServiceObject.OriginalPrice));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SmartPackageEquipmentService
        /// </summary>
        /// <param name="smartPackageEquipmentServiceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SmartPackageEquipmentServiceBase smartPackageEquipmentServiceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSMARTPACKAGEEQUIPMENTSERVICE);
	
				AddParameter(cmd, pInt32Out(SmartPackageEquipmentServiceBase.Property_Id));
				AddCommonParams(cmd, smartPackageEquipmentServiceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					smartPackageEquipmentServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					smartPackageEquipmentServiceObject.Id = (Int32)GetOutParameter(cmd, SmartPackageEquipmentServiceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(smartPackageEquipmentServiceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SmartPackageEquipmentService
        /// </summary>
        /// <param name="smartPackageEquipmentServiceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SmartPackageEquipmentServiceBase smartPackageEquipmentServiceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESMARTPACKAGEEQUIPMENTSERVICE);
				
				AddParameter(cmd, pInt32(SmartPackageEquipmentServiceBase.Property_Id, smartPackageEquipmentServiceObject.Id));
				AddCommonParams(cmd, smartPackageEquipmentServiceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					smartPackageEquipmentServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(smartPackageEquipmentServiceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SmartPackageEquipmentService
        /// </summary>
        /// <param name="Id">Id of the SmartPackageEquipmentService object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESMARTPACKAGEEQUIPMENTSERVICE);	
				
				AddParameter(cmd, pInt32(SmartPackageEquipmentServiceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SmartPackageEquipmentService), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SmartPackageEquipmentService object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SmartPackageEquipmentService object to retrieve</param>
        /// <returns>SmartPackageEquipmentService object, null if not found</returns>
		public SmartPackageEquipmentService Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEEQUIPMENTSERVICEBYID))
			{
				AddParameter( cmd, pInt32(SmartPackageEquipmentServiceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SmartPackageEquipmentService objects 
        /// </summary>
        /// <returns>A list of SmartPackageEquipmentService objects</returns>
		public SmartPackageEquipmentServiceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSMARTPACKAGEEQUIPMENTSERVICE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SmartPackageEquipmentService objects by PageRequest
        /// </summary>
        /// <returns>A list of SmartPackageEquipmentService objects</returns>
		public SmartPackageEquipmentServiceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSMARTPACKAGEEQUIPMENTSERVICE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SmartPackageEquipmentServiceList _SmartPackageEquipmentServiceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SmartPackageEquipmentServiceList;
			}
		}
		
		/// <summary>
        /// Retrieves all SmartPackageEquipmentService objects by query String
        /// </summary>
        /// <returns>A list of SmartPackageEquipmentService objects</returns>
		public SmartPackageEquipmentServiceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEEQUIPMENTSERVICEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SmartPackageEquipmentService Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SmartPackageEquipmentService
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEEQUIPMENTSERVICEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SmartPackageEquipmentService Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SmartPackageEquipmentService
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SmartPackageEquipmentServiceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEEQUIPMENTSERVICEROWCOUNT))
			{
				SqlDataReader reader;
				_SmartPackageEquipmentServiceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SmartPackageEquipmentServiceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SmartPackageEquipmentService object
        /// </summary>
        /// <param name="smartPackageEquipmentServiceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SmartPackageEquipmentServiceBase smartPackageEquipmentServiceObject, SqlDataReader reader, int start)
		{
			
				smartPackageEquipmentServiceObject.Id = reader.GetInt32( start + 0 );			
				smartPackageEquipmentServiceObject.CompanyId = reader.GetGuid( start + 1 );			
				smartPackageEquipmentServiceObject.PackageId = reader.GetGuid( start + 2 );			
				smartPackageEquipmentServiceObject.EquipmentId = reader.GetGuid( start + 3 );			
				smartPackageEquipmentServiceObject.IsFree = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) smartPackageEquipmentServiceObject.EptNo = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) smartPackageEquipmentServiceObject.Type = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) smartPackageEquipmentServiceObject.Price = reader.GetDouble( start + 7 );			
				smartPackageEquipmentServiceObject.Status = reader.GetBoolean( start + 8 );			
				smartPackageEquipmentServiceObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				smartPackageEquipmentServiceObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
				smartPackageEquipmentServiceObject.SmartPackageEquipmentServiceId = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) smartPackageEquipmentServiceObject.OriginalPrice = reader.GetDouble( start + 12 );			
			FillBaseObject(smartPackageEquipmentServiceObject, reader, (start + 13));

			
			smartPackageEquipmentServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SmartPackageEquipmentService object
        /// </summary>
        /// <param name="smartPackageEquipmentServiceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SmartPackageEquipmentServiceBase smartPackageEquipmentServiceObject, SqlDataReader reader)
		{
			FillObject(smartPackageEquipmentServiceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SmartPackageEquipmentService object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SmartPackageEquipmentService object</returns>
		private SmartPackageEquipmentService GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SmartPackageEquipmentService smartPackageEquipmentServiceObject= new SmartPackageEquipmentService();
					FillObject(smartPackageEquipmentServiceObject, reader);
					return smartPackageEquipmentServiceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SmartPackageEquipmentService objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SmartPackageEquipmentService objects</returns>
		private SmartPackageEquipmentServiceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SmartPackageEquipmentService list
			SmartPackageEquipmentServiceList list = new SmartPackageEquipmentServiceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SmartPackageEquipmentService smartPackageEquipmentServiceObject = new SmartPackageEquipmentService();
					FillObject(smartPackageEquipmentServiceObject, reader);

					list.Add(smartPackageEquipmentServiceObject);
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
