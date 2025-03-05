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
	public partial class InvoiceNoteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTINVOICENOTE = "InsertInvoiceNote";
		private const string UPDATEINVOICENOTE = "UpdateInvoiceNote";
		private const string DELETEINVOICENOTE = "DeleteInvoiceNote";
		private const string GETINVOICENOTEBYID = "GetInvoiceNoteById";
		private const string GETALLINVOICENOTE = "GetAllInvoiceNote";
		private const string GETPAGEDINVOICENOTE = "GetPagedInvoiceNote";
		private const string GETINVOICENOTEMAXIMUMID = "GetInvoiceNoteMaximumId";
		private const string GETINVOICENOTEROWCOUNT = "GetInvoiceNoteRowCount";	
		private const string GETINVOICENOTEBYQUERY = "GetInvoiceNoteByQuery";
		#endregion
		
		#region Constructors
		public InvoiceNoteDataAccess(ClientContext context) : base(context) { }
		public InvoiceNoteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="invoiceNoteObject"></param>
		private void AddCommonParams(SqlCommand cmd, InvoiceNoteBase invoiceNoteObject)
		{	
			AddParameter(cmd, pGuid(InvoiceNoteBase.Property_CompanyId, invoiceNoteObject.CompanyId));
			AddParameter(cmd, pInt32(InvoiceNoteBase.Property_InvoiceId, invoiceNoteObject.InvoiceId));
			AddParameter(cmd, pNVarChar(InvoiceNoteBase.Property_Note, invoiceNoteObject.Note));
			AddParameter(cmd, pDateTime(InvoiceNoteBase.Property_AddedDate, invoiceNoteObject.AddedDate));
			AddParameter(cmd, pGuid(InvoiceNoteBase.Property_AddedBy, invoiceNoteObject.AddedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts InvoiceNote
        /// </summary>
        /// <param name="invoiceNoteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(InvoiceNoteBase invoiceNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTINVOICENOTE);
	
				AddParameter(cmd, pInt32Out(InvoiceNoteBase.Property_Id));
				AddCommonParams(cmd, invoiceNoteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					invoiceNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					invoiceNoteObject.Id = (Int32)GetOutParameter(cmd, InvoiceNoteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(invoiceNoteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates InvoiceNote
        /// </summary>
        /// <param name="invoiceNoteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(InvoiceNoteBase invoiceNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEINVOICENOTE);
				
				AddParameter(cmd, pInt32(InvoiceNoteBase.Property_Id, invoiceNoteObject.Id));
				AddCommonParams(cmd, invoiceNoteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					invoiceNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(invoiceNoteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes InvoiceNote
        /// </summary>
        /// <param name="Id">Id of the InvoiceNote object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEINVOICENOTE);	
				
				AddParameter(cmd, pInt32(InvoiceNoteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(InvoiceNote), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves InvoiceNote object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the InvoiceNote object to retrieve</param>
        /// <returns>InvoiceNote object, null if not found</returns>
		public InvoiceNote Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVOICENOTEBYID))
			{
				AddParameter( cmd, pInt32(InvoiceNoteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all InvoiceNote objects 
        /// </summary>
        /// <returns>A list of InvoiceNote objects</returns>
		public InvoiceNoteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLINVOICENOTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all InvoiceNote objects by PageRequest
        /// </summary>
        /// <returns>A list of InvoiceNote objects</returns>
		public InvoiceNoteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDINVOICENOTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				InvoiceNoteList _InvoiceNoteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _InvoiceNoteList;
			}
		}
		
		/// <summary>
        /// Retrieves all InvoiceNote objects by query String
        /// </summary>
        /// <returns>A list of InvoiceNote objects</returns>
		public InvoiceNoteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVOICENOTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get InvoiceNote Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of InvoiceNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVOICENOTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get InvoiceNote Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of InvoiceNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _InvoiceNoteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVOICENOTEROWCOUNT))
			{
				SqlDataReader reader;
				_InvoiceNoteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _InvoiceNoteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills InvoiceNote object
        /// </summary>
        /// <param name="invoiceNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(InvoiceNoteBase invoiceNoteObject, SqlDataReader reader, int start)
		{
			
				invoiceNoteObject.Id = reader.GetInt32( start + 0 );			
				invoiceNoteObject.CompanyId = reader.GetGuid( start + 1 );			
				invoiceNoteObject.InvoiceId = reader.GetInt32( start + 2 );			
				invoiceNoteObject.Note = reader.GetString( start + 3 );			
				invoiceNoteObject.AddedDate = reader.GetDateTime( start + 4 );			
				invoiceNoteObject.AddedBy = reader.GetGuid( start + 5 );			
			FillBaseObject(invoiceNoteObject, reader, (start + 6));

			
			invoiceNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills InvoiceNote object
        /// </summary>
        /// <param name="invoiceNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(InvoiceNoteBase invoiceNoteObject, SqlDataReader reader)
		{
			FillObject(invoiceNoteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves InvoiceNote object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>InvoiceNote object</returns>
		private InvoiceNote GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					InvoiceNote invoiceNoteObject= new InvoiceNote();
					FillObject(invoiceNoteObject, reader);
					return invoiceNoteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of InvoiceNote objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of InvoiceNote objects</returns>
		private InvoiceNoteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//InvoiceNote list
			InvoiceNoteList list = new InvoiceNoteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					InvoiceNote invoiceNoteObject = new InvoiceNote();
					FillObject(invoiceNoteObject, reader);

					list.Add(invoiceNoteObject);
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
