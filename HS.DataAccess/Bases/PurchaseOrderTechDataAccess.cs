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
	public partial class PurchaseOrderTechDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPURCHASEORDERTECH = "InsertPurchaseOrderTech";
		private const string UPDATEPURCHASEORDERTECH = "UpdatePurchaseOrderTech";
		private const string DELETEPURCHASEORDERTECH = "DeletePurchaseOrderTech";
		private const string GETPURCHASEORDERTECHBYID = "GetPurchaseOrderTechById";
		private const string GETALLPURCHASEORDERTECH = "GetAllPurchaseOrderTech";
		private const string GETPAGEDPURCHASEORDERTECH = "GetPagedPurchaseOrderTech";
		private const string GETPURCHASEORDERTECHMAXIMUMID = "GetPurchaseOrderTechMaximumId";
		private const string GETPURCHASEORDERTECHROWCOUNT = "GetPurchaseOrderTechRowCount";	
		private const string GETPURCHASEORDERTECHBYQUERY = "GetPurchaseOrderTechByQuery";
		#endregion
		
		#region Constructors
		public PurchaseOrderTechDataAccess(ClientContext context) : base(context) { }
		public PurchaseOrderTechDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="purchaseOrderTechObject"></param>
		private void AddCommonParams(SqlCommand cmd, PurchaseOrderTechBase purchaseOrderTechObject)
		{	
			AddParameter(cmd, pGuid(PurchaseOrderTechBase.Property_CompanyId, purchaseOrderTechObject.CompanyId));
			AddParameter(cmd, pGuid(PurchaseOrderTechBase.Property_TechnicianId, purchaseOrderTechObject.TechnicianId));
			AddParameter(cmd, pNVarChar(PurchaseOrderTechBase.Property_DemandOrderId, 50, purchaseOrderTechObject.DemandOrderId));
			AddParameter(cmd, pNVarChar(PurchaseOrderTechBase.Property_Status, 50, purchaseOrderTechObject.Status));
			AddParameter(cmd, pNVarChar(PurchaseOrderTechBase.Property_Location, 50, purchaseOrderTechObject.Location));
			AddParameter(cmd, pBool(PurchaseOrderTechBase.Property_IsReceived, purchaseOrderTechObject.IsReceived));
			AddParameter(cmd, pGuid(PurchaseOrderTechBase.Property_CreatedByUid, purchaseOrderTechObject.CreatedByUid));
			AddParameter(cmd, pDateTime(PurchaseOrderTechBase.Property_LastUpdatedDate, purchaseOrderTechObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(PurchaseOrderTechBase.Property_LastUpdatedByUid, purchaseOrderTechObject.LastUpdatedByUid));
			AddParameter(cmd, pNVarChar(PurchaseOrderTechBase.Property_Description, purchaseOrderTechObject.Description));
			AddParameter(cmd, pDateTime(PurchaseOrderTechBase.Property_CreatedDate, purchaseOrderTechObject.CreatedDate));
			AddParameter(cmd, pBool(PurchaseOrderTechBase.Property_IsBulkPO, purchaseOrderTechObject.IsBulkPO));
			AddParameter(cmd, pInt32(PurchaseOrderTechBase.Property_TicketId, purchaseOrderTechObject.TicketId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PurchaseOrderTech
        /// </summary>
        /// <param name="purchaseOrderTechObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PurchaseOrderTechBase purchaseOrderTechObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPURCHASEORDERTECH);
	
				AddParameter(cmd, pInt32Out(PurchaseOrderTechBase.Property_Id));
				AddCommonParams(cmd, purchaseOrderTechObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					purchaseOrderTechObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					purchaseOrderTechObject.Id = (Int32)GetOutParameter(cmd, PurchaseOrderTechBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(purchaseOrderTechObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PurchaseOrderTech
        /// </summary>
        /// <param name="purchaseOrderTechObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PurchaseOrderTechBase purchaseOrderTechObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPURCHASEORDERTECH);
				
				AddParameter(cmd, pInt32(PurchaseOrderTechBase.Property_Id, purchaseOrderTechObject.Id));
				AddCommonParams(cmd, purchaseOrderTechObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					purchaseOrderTechObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(purchaseOrderTechObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PurchaseOrderTech
        /// </summary>
        /// <param name="Id">Id of the PurchaseOrderTech object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPURCHASEORDERTECH);	
				
				AddParameter(cmd, pInt32(PurchaseOrderTechBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PurchaseOrderTech), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PurchaseOrderTech object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PurchaseOrderTech object to retrieve</param>
        /// <returns>PurchaseOrderTech object, null if not found</returns>
		public PurchaseOrderTech Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERTECHBYID))
			{
				AddParameter( cmd, pInt32(PurchaseOrderTechBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PurchaseOrderTech objects 
        /// </summary>
        /// <returns>A list of PurchaseOrderTech objects</returns>
		public PurchaseOrderTechList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPURCHASEORDERTECH))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PurchaseOrderTech objects by PageRequest
        /// </summary>
        /// <returns>A list of PurchaseOrderTech objects</returns>
		public PurchaseOrderTechList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPURCHASEORDERTECH))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PurchaseOrderTechList _PurchaseOrderTechList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PurchaseOrderTechList;
			}
		}
		
		/// <summary>
        /// Retrieves all PurchaseOrderTech objects by query String
        /// </summary>
        /// <returns>A list of PurchaseOrderTech objects</returns>
		public PurchaseOrderTechList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERTECHBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PurchaseOrderTech Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PurchaseOrderTech
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERTECHMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PurchaseOrderTech Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PurchaseOrderTech
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PurchaseOrderTechRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERTECHROWCOUNT))
			{
				SqlDataReader reader;
				_PurchaseOrderTechRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PurchaseOrderTechRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PurchaseOrderTech object
        /// </summary>
        /// <param name="purchaseOrderTechObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PurchaseOrderTechBase purchaseOrderTechObject, SqlDataReader reader, int start)
		{
			
				purchaseOrderTechObject.Id = reader.GetInt32( start + 0 );			
				purchaseOrderTechObject.CompanyId = reader.GetGuid( start + 1 );			
				purchaseOrderTechObject.TechnicianId = reader.GetGuid( start + 2 );			
				purchaseOrderTechObject.DemandOrderId = reader.GetString( start + 3 );			
				purchaseOrderTechObject.Status = reader.GetString( start + 4 );			
				purchaseOrderTechObject.Location = reader.GetString( start + 5 );			
				purchaseOrderTechObject.IsReceived = reader.GetBoolean( start + 6 );			
				purchaseOrderTechObject.CreatedByUid = reader.GetGuid( start + 7 );			
				purchaseOrderTechObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
				purchaseOrderTechObject.LastUpdatedByUid = reader.GetGuid( start + 9 );			
				if(!reader.IsDBNull(10)) purchaseOrderTechObject.Description = reader.GetString( start + 10 );			
				purchaseOrderTechObject.CreatedDate = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) purchaseOrderTechObject.IsBulkPO = reader.GetBoolean( start + 12 );			
				if(!reader.IsDBNull(13)) purchaseOrderTechObject.TicketId = reader.GetInt32( start + 13 );			
			FillBaseObject(purchaseOrderTechObject, reader, (start + 14));

			
			purchaseOrderTechObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PurchaseOrderTech object
        /// </summary>
        /// <param name="purchaseOrderTechObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PurchaseOrderTechBase purchaseOrderTechObject, SqlDataReader reader)
		{
			FillObject(purchaseOrderTechObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PurchaseOrderTech object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PurchaseOrderTech object</returns>
		private PurchaseOrderTech GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PurchaseOrderTech purchaseOrderTechObject= new PurchaseOrderTech();
					FillObject(purchaseOrderTechObject, reader);
					return purchaseOrderTechObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PurchaseOrderTech objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PurchaseOrderTech objects</returns>
		private PurchaseOrderTechList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PurchaseOrderTech list
			PurchaseOrderTechList list = new PurchaseOrderTechList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PurchaseOrderTech purchaseOrderTechObject = new PurchaseOrderTech();
					FillObject(purchaseOrderTechObject, reader);

					list.Add(purchaseOrderTechObject);
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
