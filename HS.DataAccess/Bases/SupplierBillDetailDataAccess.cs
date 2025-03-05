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
	public partial class SupplierBillDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSUPPLIERBILLDETAIL = "InsertSupplierBillDetail";
		private const string UPDATESUPPLIERBILLDETAIL = "UpdateSupplierBillDetail";
		private const string DELETESUPPLIERBILLDETAIL = "DeleteSupplierBillDetail";
		private const string GETSUPPLIERBILLDETAILBYID = "GetSupplierBillDetailById";
		private const string GETALLSUPPLIERBILLDETAIL = "GetAllSupplierBillDetail";
		private const string GETPAGEDSUPPLIERBILLDETAIL = "GetPagedSupplierBillDetail";
		private const string GETSUPPLIERBILLDETAILMAXIMUMID = "GetSupplierBillDetailMaximumId";
		private const string GETSUPPLIERBILLDETAILROWCOUNT = "GetSupplierBillDetailRowCount";	
		private const string GETSUPPLIERBILLDETAILBYQUERY = "GetSupplierBillDetailByQuery";
		#endregion
		
		#region Constructors
		public SupplierBillDetailDataAccess(ClientContext context) : base(context) { }
		public SupplierBillDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="supplierBillDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, SupplierBillDetailBase supplierBillDetailObject)
		{	
			AddParameter(cmd, pInt32(SupplierBillDetailBase.Property_SupplierBillId, supplierBillDetailObject.SupplierBillId));
			AddParameter(cmd, pInt32(SupplierBillDetailBase.Property_EquipmentId, supplierBillDetailObject.EquipmentId));
			AddParameter(cmd, pInt32(SupplierBillDetailBase.Property_AccoutTypeId, supplierBillDetailObject.AccoutTypeId));
			AddParameter(cmd, pNVarChar(SupplierBillDetailBase.Property_Dscription, supplierBillDetailObject.Dscription));
			AddParameter(cmd, pInt32(SupplierBillDetailBase.Property_Quantity, supplierBillDetailObject.Quantity));
			AddParameter(cmd, pDouble(SupplierBillDetailBase.Property_Rate, supplierBillDetailObject.Rate));
			AddParameter(cmd, pDouble(SupplierBillDetailBase.Property_Amount, supplierBillDetailObject.Amount));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SupplierBillDetail
        /// </summary>
        /// <param name="supplierBillDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SupplierBillDetailBase supplierBillDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSUPPLIERBILLDETAIL);
	
				AddParameter(cmd, pInt32Out(SupplierBillDetailBase.Property_Id));
				AddCommonParams(cmd, supplierBillDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					supplierBillDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					supplierBillDetailObject.Id = (Int32)GetOutParameter(cmd, SupplierBillDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(supplierBillDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SupplierBillDetail
        /// </summary>
        /// <param name="supplierBillDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SupplierBillDetailBase supplierBillDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESUPPLIERBILLDETAIL);
				
				AddParameter(cmd, pInt32(SupplierBillDetailBase.Property_Id, supplierBillDetailObject.Id));
				AddCommonParams(cmd, supplierBillDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					supplierBillDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(supplierBillDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SupplierBillDetail
        /// </summary>
        /// <param name="Id">Id of the SupplierBillDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESUPPLIERBILLDETAIL);	
				
				AddParameter(cmd, pInt32(SupplierBillDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SupplierBillDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SupplierBillDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SupplierBillDetail object to retrieve</param>
        /// <returns>SupplierBillDetail object, null if not found</returns>
		public SupplierBillDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBILLDETAILBYID))
			{
				AddParameter( cmd, pInt32(SupplierBillDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SupplierBillDetail objects 
        /// </summary>
        /// <returns>A list of SupplierBillDetail objects</returns>
		public SupplierBillDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSUPPLIERBILLDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SupplierBillDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of SupplierBillDetail objects</returns>
		public SupplierBillDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSUPPLIERBILLDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SupplierBillDetailList _SupplierBillDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SupplierBillDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all SupplierBillDetail objects by query String
        /// </summary>
        /// <returns>A list of SupplierBillDetail objects</returns>
		public SupplierBillDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBILLDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SupplierBillDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SupplierBillDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBILLDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SupplierBillDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SupplierBillDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SupplierBillDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBILLDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_SupplierBillDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SupplierBillDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SupplierBillDetail object
        /// </summary>
        /// <param name="supplierBillDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SupplierBillDetailBase supplierBillDetailObject, SqlDataReader reader, int start)
		{
			
				supplierBillDetailObject.Id = reader.GetInt32( start + 0 );			
				supplierBillDetailObject.SupplierBillId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) supplierBillDetailObject.EquipmentId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) supplierBillDetailObject.AccoutTypeId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) supplierBillDetailObject.Dscription = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) supplierBillDetailObject.Quantity = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) supplierBillDetailObject.Rate = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) supplierBillDetailObject.Amount = reader.GetDouble( start + 7 );			
			FillBaseObject(supplierBillDetailObject, reader, (start + 8));

			
			supplierBillDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SupplierBillDetail object
        /// </summary>
        /// <param name="supplierBillDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SupplierBillDetailBase supplierBillDetailObject, SqlDataReader reader)
		{
			FillObject(supplierBillDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SupplierBillDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SupplierBillDetail object</returns>
		private SupplierBillDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SupplierBillDetail supplierBillDetailObject= new SupplierBillDetail();
					FillObject(supplierBillDetailObject, reader);
					return supplierBillDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SupplierBillDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SupplierBillDetail objects</returns>
		private SupplierBillDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SupplierBillDetail list
			SupplierBillDetailList list = new SupplierBillDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SupplierBillDetail supplierBillDetailObject = new SupplierBillDetail();
					FillObject(supplierBillDetailObject, reader);

					list.Add(supplierBillDetailObject);
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
