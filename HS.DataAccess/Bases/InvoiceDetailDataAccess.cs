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
	public partial class InvoiceDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTINVOICEDETAIL = "InsertInvoiceDetail";
		private const string UPDATEINVOICEDETAIL = "UpdateInvoiceDetail";
		private const string DELETEINVOICEDETAIL = "DeleteInvoiceDetail";
		private const string GETINVOICEDETAILBYID = "GetInvoiceDetailById";
		private const string GETALLINVOICEDETAIL = "GetAllInvoiceDetail";
		private const string GETPAGEDINVOICEDETAIL = "GetPagedInvoiceDetail";
		private const string GETINVOICEDETAILMAXIMUMID = "GetInvoiceDetailMaximumId";
		private const string GETINVOICEDETAILROWCOUNT = "GetInvoiceDetailRowCount";	
		private const string GETINVOICEDETAILBYQUERY = "GetInvoiceDetailByQuery";
		#endregion
		
		#region Constructors
		public InvoiceDetailDataAccess(ClientContext context) : base(context) { }
		public InvoiceDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="invoiceDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, InvoiceDetailBase invoiceDetailObject)
		{	
			AddParameter(cmd, pNVarChar(InvoiceDetailBase.Property_InvoiceId, 15, invoiceDetailObject.InvoiceId));
			AddParameter(cmd, pGuid(InvoiceDetailBase.Property_InventoryId, invoiceDetailObject.InventoryId));
			AddParameter(cmd, pGuid(InvoiceDetailBase.Property_EquipmentId, invoiceDetailObject.EquipmentId));
			AddParameter(cmd, pNVarChar(InvoiceDetailBase.Property_EquipName, 500, invoiceDetailObject.EquipName));
			AddParameter(cmd, pNVarChar(InvoiceDetailBase.Property_EquipDetail, invoiceDetailObject.EquipDetail));
			AddParameter(cmd, pGuid(InvoiceDetailBase.Property_CompanyId, invoiceDetailObject.CompanyId));
			AddParameter(cmd, pInt32(InvoiceDetailBase.Property_BundleId, invoiceDetailObject.BundleId));
			AddParameter(cmd, pInt32(InvoiceDetailBase.Property_Quantity, invoiceDetailObject.Quantity));
			AddParameter(cmd, pDouble(InvoiceDetailBase.Property_UnitPrice, invoiceDetailObject.UnitPrice));
			AddParameter(cmd, pDouble(InvoiceDetailBase.Property_TotalPrice, invoiceDetailObject.TotalPrice));
			AddParameter(cmd, pDateTime(InvoiceDetailBase.Property_CreatedDate, invoiceDetailObject.CreatedDate));
			AddParameter(cmd, pNVarChar(InvoiceDetailBase.Property_CreatedBy, 50, invoiceDetailObject.CreatedBy));
			AddParameter(cmd, pBool(InvoiceDetailBase.Property_Taxable, invoiceDetailObject.Taxable));
			AddParameter(cmd, pDouble(InvoiceDetailBase.Property_DiscountAmount, invoiceDetailObject.DiscountAmount));
			AddParameter(cmd, pDouble(InvoiceDetailBase.Property_DiscountPercent, invoiceDetailObject.DiscountPercent));
			AddParameter(cmd, pNVarChar(InvoiceDetailBase.Property_DiscountType, 150, invoiceDetailObject.DiscountType));
			AddParameter(cmd, pNVarChar(InvoiceDetailBase.Property_EquipCategory, 100, invoiceDetailObject.EquipCategory));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts InvoiceDetail
        /// </summary>
        /// <param name="invoiceDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(InvoiceDetailBase invoiceDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTINVOICEDETAIL);
	
				AddParameter(cmd, pInt32Out(InvoiceDetailBase.Property_Id));
				AddCommonParams(cmd, invoiceDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					invoiceDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					invoiceDetailObject.Id = (Int32)GetOutParameter(cmd, InvoiceDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(invoiceDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates InvoiceDetail
        /// </summary>
        /// <param name="invoiceDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(InvoiceDetailBase invoiceDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEINVOICEDETAIL);
				
				AddParameter(cmd, pInt32(InvoiceDetailBase.Property_Id, invoiceDetailObject.Id));
				AddCommonParams(cmd, invoiceDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					invoiceDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(invoiceDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes InvoiceDetail
        /// </summary>
        /// <param name="Id">Id of the InvoiceDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEINVOICEDETAIL);	
				
				AddParameter(cmd, pInt32(InvoiceDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(InvoiceDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves InvoiceDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the InvoiceDetail object to retrieve</param>
        /// <returns>InvoiceDetail object, null if not found</returns>
		public InvoiceDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVOICEDETAILBYID))
			{
				AddParameter( cmd, pInt32(InvoiceDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all InvoiceDetail objects 
        /// </summary>
        /// <returns>A list of InvoiceDetail objects</returns>
		public InvoiceDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLINVOICEDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all InvoiceDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of InvoiceDetail objects</returns>
		public InvoiceDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDINVOICEDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				InvoiceDetailList _InvoiceDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _InvoiceDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all InvoiceDetail objects by query String
        /// </summary>
        /// <returns>A list of InvoiceDetail objects</returns>
		public InvoiceDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVOICEDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get InvoiceDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of InvoiceDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVOICEDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get InvoiceDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of InvoiceDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _InvoiceDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVOICEDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_InvoiceDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _InvoiceDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills InvoiceDetail object
        /// </summary>
        /// <param name="invoiceDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(InvoiceDetailBase invoiceDetailObject, SqlDataReader reader, int start)
		{
			
				invoiceDetailObject.Id = reader.GetInt32( start + 0 );			
				invoiceDetailObject.InvoiceId = reader.GetString( start + 1 );			
				invoiceDetailObject.InventoryId = reader.GetGuid( start + 2 );			
				invoiceDetailObject.EquipmentId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) invoiceDetailObject.EquipName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) invoiceDetailObject.EquipDetail = reader.GetString( start + 5 );			
				invoiceDetailObject.CompanyId = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) invoiceDetailObject.BundleId = reader.GetInt32( start + 7 );			
				if(!reader.IsDBNull(8)) invoiceDetailObject.Quantity = reader.GetInt32( start + 8 );			
				if(!reader.IsDBNull(9)) invoiceDetailObject.UnitPrice = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) invoiceDetailObject.TotalPrice = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) invoiceDetailObject.CreatedDate = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) invoiceDetailObject.CreatedBy = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) invoiceDetailObject.Taxable = reader.GetBoolean( start + 13 );			
				if(!reader.IsDBNull(14)) invoiceDetailObject.DiscountAmount = reader.GetDouble( start + 14 );			
				if(!reader.IsDBNull(15)) invoiceDetailObject.DiscountPercent = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) invoiceDetailObject.DiscountType = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) invoiceDetailObject.EquipCategory = reader.GetString( start + 17 );			
			FillBaseObject(invoiceDetailObject, reader, (start + 18));

			
			invoiceDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills InvoiceDetail object
        /// </summary>
        /// <param name="invoiceDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(InvoiceDetailBase invoiceDetailObject, SqlDataReader reader)
		{
			FillObject(invoiceDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves InvoiceDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>InvoiceDetail object</returns>
		private InvoiceDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					InvoiceDetail invoiceDetailObject= new InvoiceDetail();
					FillObject(invoiceDetailObject, reader);
					return invoiceDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of InvoiceDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of InvoiceDetail objects</returns>
		private InvoiceDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//InvoiceDetail list
			InvoiceDetailList list = new InvoiceDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					InvoiceDetail invoiceDetailObject = new InvoiceDetail();
					FillObject(invoiceDetailObject, reader);

					list.Add(invoiceDetailObject);
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
