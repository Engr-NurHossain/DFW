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
	public partial class SupplierBillDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSUPPLIERBILL = "InsertSupplierBill";
		private const string UPDATESUPPLIERBILL = "UpdateSupplierBill";
		private const string DELETESUPPLIERBILL = "DeleteSupplierBill";
		private const string GETSUPPLIERBILLBYID = "GetSupplierBillById";
		private const string GETALLSUPPLIERBILL = "GetAllSupplierBill";
		private const string GETPAGEDSUPPLIERBILL = "GetPagedSupplierBill";
		private const string GETSUPPLIERBILLMAXIMUMID = "GetSupplierBillMaximumId";
		private const string GETSUPPLIERBILLROWCOUNT = "GetSupplierBillRowCount";	
		private const string GETSUPPLIERBILLBYQUERY = "GetSupplierBillByQuery";
		#endregion
		
		#region Constructors
		public SupplierBillDataAccess(ClientContext context) : base(context) { }
		public SupplierBillDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="supplierBillObject"></param>
		private void AddCommonParams(SqlCommand cmd, SupplierBillBase supplierBillObject)
		{	
			AddParameter(cmd, pNVarChar(SupplierBillBase.Property_BillNo, 50, supplierBillObject.BillNo));
			AddParameter(cmd, pGuid(SupplierBillBase.Property_CompanyId, supplierBillObject.CompanyId));
			AddParameter(cmd, pInt32(SupplierBillBase.Property_SuplierId, supplierBillObject.SuplierId));
			AddParameter(cmd, pNVarChar(SupplierBillBase.Property_Type, 50, supplierBillObject.Type));
			AddParameter(cmd, pDouble(SupplierBillBase.Property_Amount, supplierBillObject.Amount));
			AddParameter(cmd, pNVarChar(SupplierBillBase.Property_PaymentMethod, 50, supplierBillObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(SupplierBillBase.Property_PaymentStatus, 50, supplierBillObject.PaymentStatus));
			AddParameter(cmd, pDateTime(SupplierBillBase.Property_PaymentDate, supplierBillObject.PaymentDate));
			AddParameter(cmd, pDateTime(SupplierBillBase.Property_PaymentDueDate, supplierBillObject.PaymentDueDate));
			AddParameter(cmd, pNVarChar(SupplierBillBase.Property_Notes, supplierBillObject.Notes));
			AddParameter(cmd, pNVarChar(SupplierBillBase.Property_UpdatedBy, 50, supplierBillObject.UpdatedBy));
			AddParameter(cmd, pDateTime(SupplierBillBase.Property_UpdatedDate, supplierBillObject.UpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SupplierBill
        /// </summary>
        /// <param name="supplierBillObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SupplierBillBase supplierBillObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSUPPLIERBILL);
	
				AddParameter(cmd, pInt32Out(SupplierBillBase.Property_Id));
				AddCommonParams(cmd, supplierBillObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					supplierBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					supplierBillObject.Id = (Int32)GetOutParameter(cmd, SupplierBillBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(supplierBillObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SupplierBill
        /// </summary>
        /// <param name="supplierBillObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SupplierBillBase supplierBillObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESUPPLIERBILL);
				
				AddParameter(cmd, pInt32(SupplierBillBase.Property_Id, supplierBillObject.Id));
				AddCommonParams(cmd, supplierBillObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					supplierBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(supplierBillObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SupplierBill
        /// </summary>
        /// <param name="Id">Id of the SupplierBill object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESUPPLIERBILL);	
				
				AddParameter(cmd, pInt32(SupplierBillBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SupplierBill), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SupplierBill object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SupplierBill object to retrieve</param>
        /// <returns>SupplierBill object, null if not found</returns>
		public SupplierBill Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBILLBYID))
			{
				AddParameter( cmd, pInt32(SupplierBillBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SupplierBill objects 
        /// </summary>
        /// <returns>A list of SupplierBill objects</returns>
		public SupplierBillList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSUPPLIERBILL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SupplierBill objects by PageRequest
        /// </summary>
        /// <returns>A list of SupplierBill objects</returns>
		public SupplierBillList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSUPPLIERBILL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SupplierBillList _SupplierBillList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SupplierBillList;
			}
		}
		
		/// <summary>
        /// Retrieves all SupplierBill objects by query String
        /// </summary>
        /// <returns>A list of SupplierBill objects</returns>
		public SupplierBillList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBILLBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SupplierBill Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SupplierBill
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBILLMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SupplierBill Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SupplierBill
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SupplierBillRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBILLROWCOUNT))
			{
				SqlDataReader reader;
				_SupplierBillRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SupplierBillRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SupplierBill object
        /// </summary>
        /// <param name="supplierBillObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SupplierBillBase supplierBillObject, SqlDataReader reader, int start)
		{
			
				supplierBillObject.Id = reader.GetInt32( start + 0 );			
				supplierBillObject.BillNo = reader.GetString( start + 1 );			
				supplierBillObject.CompanyId = reader.GetGuid( start + 2 );			
				supplierBillObject.SuplierId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) supplierBillObject.Type = reader.GetString( start + 4 );			
				supplierBillObject.Amount = reader.GetDouble( start + 5 );			
				supplierBillObject.PaymentMethod = reader.GetString( start + 6 );			
				supplierBillObject.PaymentStatus = reader.GetString( start + 7 );			
				supplierBillObject.PaymentDate = reader.GetDateTime( start + 8 );			
				supplierBillObject.PaymentDueDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) supplierBillObject.Notes = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) supplierBillObject.UpdatedBy = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) supplierBillObject.UpdatedDate = reader.GetDateTime( start + 12 );			
			FillBaseObject(supplierBillObject, reader, (start + 13));

			
			supplierBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SupplierBill object
        /// </summary>
        /// <param name="supplierBillObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SupplierBillBase supplierBillObject, SqlDataReader reader)
		{
			FillObject(supplierBillObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SupplierBill object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SupplierBill object</returns>
		private SupplierBill GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SupplierBill supplierBillObject= new SupplierBill();
					FillObject(supplierBillObject, reader);
					return supplierBillObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SupplierBill objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SupplierBill objects</returns>
		private SupplierBillList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SupplierBill list
			SupplierBillList list = new SupplierBillList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SupplierBill supplierBillObject = new SupplierBill();
					FillObject(supplierBillObject, reader);

					list.Add(supplierBillObject);
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
