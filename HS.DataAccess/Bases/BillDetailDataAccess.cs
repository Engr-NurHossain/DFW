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
	public partial class BillDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBILLDETAIL = "InsertBillDetail";
		private const string UPDATEBILLDETAIL = "UpdateBillDetail";
		private const string DELETEBILLDETAIL = "DeleteBillDetail";
		private const string GETBILLDETAILBYID = "GetBillDetailById";
		private const string GETALLBILLDETAIL = "GetAllBillDetail";
		private const string GETPAGEDBILLDETAIL = "GetPagedBillDetail";
		private const string GETBILLDETAILMAXIMUMID = "GetBillDetailMaximumId";
		private const string GETBILLDETAILROWCOUNT = "GetBillDetailRowCount";	
		private const string GETBILLDETAILBYQUERY = "GetBillDetailByQuery";
		#endregion
		
		#region Constructors
		public BillDetailDataAccess(ClientContext context) : base(context) { }
		public BillDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="billDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, BillDetailBase billDetailObject)
		{	
			AddParameter(cmd, pInt32(BillDetailBase.Property_CustomerBillId, billDetailObject.CustomerBillId));
			AddParameter(cmd, pGuid(BillDetailBase.Property_EquipmentId, billDetailObject.EquipmentId));
			AddParameter(cmd, pInt32(BillDetailBase.Property_AccoutTypeId, billDetailObject.AccoutTypeId));
			AddParameter(cmd, pNVarChar(BillDetailBase.Property_Dscription, billDetailObject.Dscription));
			AddParameter(cmd, pInt32(BillDetailBase.Property_Quantity, billDetailObject.Quantity));
			AddParameter(cmd, pDouble(BillDetailBase.Property_Rate, billDetailObject.Rate));
			AddParameter(cmd, pDouble(BillDetailBase.Property_Amount, billDetailObject.Amount));
			AddParameter(cmd, pNVarChar(BillDetailBase.Property_ItemName, 200, billDetailObject.ItemName));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BillDetail
        /// </summary>
        /// <param name="billDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BillDetailBase billDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBILLDETAIL);
	
				AddParameter(cmd, pInt32Out(BillDetailBase.Property_Id));
				AddCommonParams(cmd, billDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					billDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					billDetailObject.Id = (Int32)GetOutParameter(cmd, BillDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(billDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BillDetail
        /// </summary>
        /// <param name="billDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BillDetailBase billDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBILLDETAIL);
				
				AddParameter(cmd, pInt32(BillDetailBase.Property_Id, billDetailObject.Id));
				AddCommonParams(cmd, billDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					billDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(billDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BillDetail
        /// </summary>
        /// <param name="Id">Id of the BillDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBILLDETAIL);	
				
				AddParameter(cmd, pInt32(BillDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BillDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BillDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BillDetail object to retrieve</param>
        /// <returns>BillDetail object, null if not found</returns>
		public BillDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLDETAILBYID))
			{
				AddParameter( cmd, pInt32(BillDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BillDetail objects 
        /// </summary>
        /// <returns>A list of BillDetail objects</returns>
		public BillDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBILLDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BillDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of BillDetail objects</returns>
		public BillDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBILLDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BillDetailList _BillDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BillDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all BillDetail objects by query String
        /// </summary>
        /// <returns>A list of BillDetail objects</returns>
		public BillDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BillDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BillDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BillDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BillDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BillDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_BillDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BillDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BillDetail object
        /// </summary>
        /// <param name="billDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BillDetailBase billDetailObject, SqlDataReader reader, int start)
		{
			
				billDetailObject.Id = reader.GetInt32( start + 0 );			
				billDetailObject.CustomerBillId = reader.GetInt32( start + 1 );			
				billDetailObject.EquipmentId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) billDetailObject.AccoutTypeId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) billDetailObject.Dscription = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) billDetailObject.Quantity = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) billDetailObject.Rate = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) billDetailObject.Amount = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) billDetailObject.ItemName = reader.GetString( start + 8 );			
			FillBaseObject(billDetailObject, reader, (start + 9));

			
			billDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BillDetail object
        /// </summary>
        /// <param name="billDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BillDetailBase billDetailObject, SqlDataReader reader)
		{
			FillObject(billDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BillDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BillDetail object</returns>
		private BillDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BillDetail billDetailObject= new BillDetail();
					FillObject(billDetailObject, reader);
					return billDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BillDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BillDetail objects</returns>
		private BillDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BillDetail list
			BillDetailList list = new BillDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BillDetail billDetailObject = new BillDetail();
					FillObject(billDetailObject, reader);

					list.Add(billDetailObject);
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
