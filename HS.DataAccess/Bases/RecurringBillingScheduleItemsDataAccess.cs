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
	public partial class RecurringBillingScheduleItemsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRECURRINGBILLINGSCHEDULEITEMS = "InsertRecurringBillingScheduleItems";
		private const string UPDATERECURRINGBILLINGSCHEDULEITEMS = "UpdateRecurringBillingScheduleItems";
		private const string DELETERECURRINGBILLINGSCHEDULEITEMS = "DeleteRecurringBillingScheduleItems";
		private const string GETRECURRINGBILLINGSCHEDULEITEMSBYID = "GetRecurringBillingScheduleItemsById";
		private const string GETALLRECURRINGBILLINGSCHEDULEITEMS = "GetAllRecurringBillingScheduleItems";
		private const string GETPAGEDRECURRINGBILLINGSCHEDULEITEMS = "GetPagedRecurringBillingScheduleItems";
		private const string GETRECURRINGBILLINGSCHEDULEITEMSMAXIMUMID = "GetRecurringBillingScheduleItemsMaximumId";
		private const string GETRECURRINGBILLINGSCHEDULEITEMSROWCOUNT = "GetRecurringBillingScheduleItemsRowCount";	
		private const string GETRECURRINGBILLINGSCHEDULEITEMSBYQUERY = "GetRecurringBillingScheduleItemsByQuery";
		#endregion
		
		#region Constructors
		public RecurringBillingScheduleItemsDataAccess(ClientContext context) : base(context) { }
		public RecurringBillingScheduleItemsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }		
		#endregion

		#region AddCommonParams Method
		/// <summary>
		/// Add common parameters before calling a procedure
		/// </summary>
		/// <param name="cmd">command object, where parameters will be added</param>
		/// <param name="recurringBillingScheduleItemsObject"></param>
		private void AddCommonParams(SqlCommand cmd, RecurringBillingScheduleItemsBase recurringBillingScheduleItemsObject)
		{	
			AddParameter(cmd, pGuid(RecurringBillingScheduleItemsBase.Property_ScheduleId, recurringBillingScheduleItemsObject.ScheduleId));
			AddParameter(cmd, pNVarChar(RecurringBillingScheduleItemsBase.Property_ProductName, 200, recurringBillingScheduleItemsObject.ProductName));
			AddParameter(cmd, pNVarChar(RecurringBillingScheduleItemsBase.Property_Description, 500, recurringBillingScheduleItemsObject.Description));
			AddParameter(cmd, pInt32(RecurringBillingScheduleItemsBase.Property_Qty, recurringBillingScheduleItemsObject.Qty));
			AddParameter(cmd, pDouble(RecurringBillingScheduleItemsBase.Property_Rate, recurringBillingScheduleItemsObject.Rate));
			AddParameter(cmd, pDouble(RecurringBillingScheduleItemsBase.Property_Amount, recurringBillingScheduleItemsObject.Amount));
			AddParameter(cmd, pBool(RecurringBillingScheduleItemsBase.Property_IsTaxable, recurringBillingScheduleItemsObject.IsTaxable));
			AddParameter(cmd, pGuid(RecurringBillingScheduleItemsBase.Property_AddedBy, recurringBillingScheduleItemsObject.AddedBy));
			AddParameter(cmd, pDateTime(RecurringBillingScheduleItemsBase.Property_AddedDate, recurringBillingScheduleItemsObject.AddedDate));
			AddParameter(cmd, pDateTime(RecurringBillingScheduleItemsBase.Property_EffectiveDate, recurringBillingScheduleItemsObject.EffectiveDate));
			AddParameter(cmd, pDateTime(RecurringBillingScheduleItemsBase.Property_CycleStartDate, recurringBillingScheduleItemsObject.CycleStartDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RecurringBillingScheduleItems
        /// </summary>
        /// <param name="recurringBillingScheduleItemsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RecurringBillingScheduleItemsBase recurringBillingScheduleItemsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRECURRINGBILLINGSCHEDULEITEMS);
	
				AddParameter(cmd, pInt32Out(RecurringBillingScheduleItemsBase.Property_Id));
				AddCommonParams(cmd, recurringBillingScheduleItemsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					recurringBillingScheduleItemsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					recurringBillingScheduleItemsObject.Id = (Int32)GetOutParameter(cmd, RecurringBillingScheduleItemsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(recurringBillingScheduleItemsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RecurringBillingScheduleItems
        /// </summary>
        /// <param name="recurringBillingScheduleItemsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RecurringBillingScheduleItemsBase recurringBillingScheduleItemsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERECURRINGBILLINGSCHEDULEITEMS);
				
				AddParameter(cmd, pInt32(RecurringBillingScheduleItemsBase.Property_Id, recurringBillingScheduleItemsObject.Id));
				AddCommonParams(cmd, recurringBillingScheduleItemsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					recurringBillingScheduleItemsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(recurringBillingScheduleItemsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RecurringBillingScheduleItems
        /// </summary>
        /// <param name="Id">Id of the RecurringBillingScheduleItems object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERECURRINGBILLINGSCHEDULEITEMS);	
				
				AddParameter(cmd, pInt32(RecurringBillingScheduleItemsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RecurringBillingScheduleItems), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RecurringBillingScheduleItems object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RecurringBillingScheduleItems object to retrieve</param>
        /// <returns>RecurringBillingScheduleItems object, null if not found</returns>
		public RecurringBillingScheduleItems Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECURRINGBILLINGSCHEDULEITEMSBYID))
			{
				AddParameter( cmd, pInt32(RecurringBillingScheduleItemsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RecurringBillingScheduleItems objects 
        /// </summary>
        /// <returns>A list of RecurringBillingScheduleItems objects</returns>
		public RecurringBillingScheduleItemsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRECURRINGBILLINGSCHEDULEITEMS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RecurringBillingScheduleItems objects by PageRequest
        /// </summary>
        /// <returns>A list of RecurringBillingScheduleItems objects</returns>
		public RecurringBillingScheduleItemsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRECURRINGBILLINGSCHEDULEITEMS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RecurringBillingScheduleItemsList _RecurringBillingScheduleItemsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RecurringBillingScheduleItemsList;
			}
		}
		
		/// <summary>
        /// Retrieves all RecurringBillingScheduleItems objects by query String
        /// </summary>
        /// <returns>A list of RecurringBillingScheduleItems objects</returns>
		public RecurringBillingScheduleItemsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECURRINGBILLINGSCHEDULEITEMSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RecurringBillingScheduleItems Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RecurringBillingScheduleItems
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECURRINGBILLINGSCHEDULEITEMSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RecurringBillingScheduleItems Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RecurringBillingScheduleItems
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RecurringBillingScheduleItemsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECURRINGBILLINGSCHEDULEITEMSROWCOUNT))
			{
				SqlDataReader reader;
				_RecurringBillingScheduleItemsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RecurringBillingScheduleItemsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RecurringBillingScheduleItems object
        /// </summary>
        /// <param name="recurringBillingScheduleItemsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RecurringBillingScheduleItemsBase recurringBillingScheduleItemsObject, SqlDataReader reader, int start)
		{
			
				recurringBillingScheduleItemsObject.Id = reader.GetInt32( start + 0 );			
				recurringBillingScheduleItemsObject.ScheduleId = reader.GetGuid( start + 1 );			
				recurringBillingScheduleItemsObject.ProductName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) recurringBillingScheduleItemsObject.Description = reader.GetString( start + 3 );			
				recurringBillingScheduleItemsObject.Qty = reader.GetInt32( start + 4 );			
				recurringBillingScheduleItemsObject.Rate = reader.GetDouble( start + 5 );			
				recurringBillingScheduleItemsObject.Amount = reader.GetDouble( start + 6 );			
				recurringBillingScheduleItemsObject.IsTaxable = reader.GetBoolean( start + 7 );			
				recurringBillingScheduleItemsObject.AddedBy = reader.GetGuid( start + 8 );			
				recurringBillingScheduleItemsObject.AddedDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) recurringBillingScheduleItemsObject.EffectiveDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) recurringBillingScheduleItemsObject.CycleStartDate = reader.GetDateTime( start + 11 );			
			FillBaseObject(recurringBillingScheduleItemsObject, reader, (start + 12));

			
			recurringBillingScheduleItemsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RecurringBillingScheduleItems object
        /// </summary>
        /// <param name="recurringBillingScheduleItemsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RecurringBillingScheduleItemsBase recurringBillingScheduleItemsObject, SqlDataReader reader)
		{
			FillObject(recurringBillingScheduleItemsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RecurringBillingScheduleItems object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RecurringBillingScheduleItems object</returns>
		private RecurringBillingScheduleItems GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RecurringBillingScheduleItems recurringBillingScheduleItemsObject= new RecurringBillingScheduleItems();
					FillObject(recurringBillingScheduleItemsObject, reader);
					return recurringBillingScheduleItemsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RecurringBillingScheduleItems objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RecurringBillingScheduleItems objects</returns>
		private RecurringBillingScheduleItemsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RecurringBillingScheduleItems list
			RecurringBillingScheduleItemsList list = new RecurringBillingScheduleItemsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RecurringBillingScheduleItems recurringBillingScheduleItemsObject = new RecurringBillingScheduleItems();
					FillObject(recurringBillingScheduleItemsObject, reader);

					list.Add(recurringBillingScheduleItemsObject);
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
