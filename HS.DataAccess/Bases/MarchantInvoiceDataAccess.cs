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
	public partial class MarchantInvoiceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMARCHANTINVOICE = "InsertMarchantInvoice";
		private const string UPDATEMARCHANTINVOICE = "UpdateMarchantInvoice";
		private const string DELETEMARCHANTINVOICE = "DeleteMarchantInvoice";
		private const string GETMARCHANTINVOICEBYID = "GetMarchantInvoiceById";
		private const string GETALLMARCHANTINVOICE = "GetAllMarchantInvoice";
		private const string GETPAGEDMARCHANTINVOICE = "GetPagedMarchantInvoice";
		private const string GETMARCHANTINVOICEMAXIMUMID = "GetMarchantInvoiceMaximumId";
		private const string GETMARCHANTINVOICEROWCOUNT = "GetMarchantInvoiceRowCount";	
		private const string GETMARCHANTINVOICEBYQUERY = "GetMarchantInvoiceByQuery";
		#endregion
		
		#region Constructors
		public MarchantInvoiceDataAccess(ClientContext context) : base(context) { }
		public MarchantInvoiceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="marchantInvoiceObject"></param>
		private void AddCommonParams(SqlCommand cmd, MarchantInvoiceBase marchantInvoiceObject)
		{	
			AddParameter(cmd, pNVarChar(MarchantInvoiceBase.Property_InvoiceId, 15, marchantInvoiceObject.InvoiceId));
			AddParameter(cmd, pGuid(MarchantInvoiceBase.Property_InventoryId, marchantInvoiceObject.InventoryId));
			AddParameter(cmd, pGuid(MarchantInvoiceBase.Property_MarchantId, marchantInvoiceObject.MarchantId));
			AddParameter(cmd, pGuid(MarchantInvoiceBase.Property_CompanyId, marchantInvoiceObject.CompanyId));
			AddParameter(cmd, pDouble(MarchantInvoiceBase.Property_Amount, marchantInvoiceObject.Amount));
			AddParameter(cmd, pDouble(MarchantInvoiceBase.Property_Tax, marchantInvoiceObject.Tax));
			AddParameter(cmd, pNVarChar(MarchantInvoiceBase.Property_DiscountCode, 50, marchantInvoiceObject.DiscountCode));
			AddParameter(cmd, pDouble(MarchantInvoiceBase.Property_DiscountAmount, marchantInvoiceObject.DiscountAmount));
			AddParameter(cmd, pDouble(MarchantInvoiceBase.Property_TotalAmount, marchantInvoiceObject.TotalAmount));
			AddParameter(cmd, pNVarChar(MarchantInvoiceBase.Property_Status, 50, marchantInvoiceObject.Status));
			AddParameter(cmd, pDateTime(MarchantInvoiceBase.Property_CreatedDate, marchantInvoiceObject.CreatedDate));
			AddParameter(cmd, pNVarChar(MarchantInvoiceBase.Property_CreatedBy, 50, marchantInvoiceObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MarchantInvoice
        /// </summary>
        /// <param name="marchantInvoiceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MarchantInvoiceBase marchantInvoiceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMARCHANTINVOICE);
	
				AddParameter(cmd, pInt32Out(MarchantInvoiceBase.Property_Id));
				AddCommonParams(cmd, marchantInvoiceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					marchantInvoiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					marchantInvoiceObject.Id = (Int32)GetOutParameter(cmd, MarchantInvoiceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(marchantInvoiceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MarchantInvoice
        /// </summary>
        /// <param name="marchantInvoiceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MarchantInvoiceBase marchantInvoiceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMARCHANTINVOICE);
				
				AddParameter(cmd, pInt32(MarchantInvoiceBase.Property_Id, marchantInvoiceObject.Id));
				AddCommonParams(cmd, marchantInvoiceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					marchantInvoiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(marchantInvoiceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MarchantInvoice
        /// </summary>
        /// <param name="Id">Id of the MarchantInvoice object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMARCHANTINVOICE);	
				
				AddParameter(cmd, pInt32(MarchantInvoiceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MarchantInvoice), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MarchantInvoice object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MarchantInvoice object to retrieve</param>
        /// <returns>MarchantInvoice object, null if not found</returns>
		public MarchantInvoice Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTINVOICEBYID))
			{
				AddParameter( cmd, pInt32(MarchantInvoiceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MarchantInvoice objects 
        /// </summary>
        /// <returns>A list of MarchantInvoice objects</returns>
		public MarchantInvoiceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMARCHANTINVOICE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MarchantInvoice objects by PageRequest
        /// </summary>
        /// <returns>A list of MarchantInvoice objects</returns>
		public MarchantInvoiceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMARCHANTINVOICE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MarchantInvoiceList _MarchantInvoiceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MarchantInvoiceList;
			}
		}
		
		/// <summary>
        /// Retrieves all MarchantInvoice objects by query String
        /// </summary>
        /// <returns>A list of MarchantInvoice objects</returns>
		public MarchantInvoiceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTINVOICEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MarchantInvoice Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MarchantInvoice
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTINVOICEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MarchantInvoice Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MarchantInvoice
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MarchantInvoiceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTINVOICEROWCOUNT))
			{
				SqlDataReader reader;
				_MarchantInvoiceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MarchantInvoiceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MarchantInvoice object
        /// </summary>
        /// <param name="marchantInvoiceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MarchantInvoiceBase marchantInvoiceObject, SqlDataReader reader, int start)
		{
			
				marchantInvoiceObject.Id = reader.GetInt32( start + 0 );			
				marchantInvoiceObject.InvoiceId = reader.GetString( start + 1 );			
				marchantInvoiceObject.InventoryId = reader.GetGuid( start + 2 );			
				marchantInvoiceObject.MarchantId = reader.GetGuid( start + 3 );			
				marchantInvoiceObject.CompanyId = reader.GetGuid( start + 4 );			
				marchantInvoiceObject.Amount = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) marchantInvoiceObject.Tax = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) marchantInvoiceObject.DiscountCode = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) marchantInvoiceObject.DiscountAmount = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) marchantInvoiceObject.TotalAmount = reader.GetDouble( start + 9 );			
				marchantInvoiceObject.Status = reader.GetString( start + 10 );			
				marchantInvoiceObject.CreatedDate = reader.GetDateTime( start + 11 );			
				marchantInvoiceObject.CreatedBy = reader.GetString( start + 12 );			
			FillBaseObject(marchantInvoiceObject, reader, (start + 13));

			
			marchantInvoiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MarchantInvoice object
        /// </summary>
        /// <param name="marchantInvoiceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MarchantInvoiceBase marchantInvoiceObject, SqlDataReader reader)
		{
			FillObject(marchantInvoiceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MarchantInvoice object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MarchantInvoice object</returns>
		private MarchantInvoice GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MarchantInvoice marchantInvoiceObject= new MarchantInvoice();
					FillObject(marchantInvoiceObject, reader);
					return marchantInvoiceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MarchantInvoice objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MarchantInvoice objects</returns>
		private MarchantInvoiceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MarchantInvoice list
			MarchantInvoiceList list = new MarchantInvoiceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MarchantInvoice marchantInvoiceObject = new MarchantInvoice();
					FillObject(marchantInvoiceObject, reader);

					list.Add(marchantInvoiceObject);
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
