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
	public partial class ResturantOrderDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTURANTORDER = "InsertResturantOrder";
		private const string UPDATERESTURANTORDER = "UpdateResturantOrder";
		private const string DELETERESTURANTORDER = "DeleteResturantOrder";
		private const string GETRESTURANTORDERBYID = "GetResturantOrderById";
		private const string GETALLRESTURANTORDER = "GetAllResturantOrder";
		private const string GETPAGEDRESTURANTORDER = "GetPagedResturantOrder";
		private const string GETRESTURANTORDERMAXIMUMID = "GetResturantOrderMaximumId";
		private const string GETRESTURANTORDERROWCOUNT = "GetResturantOrderRowCount";	
		private const string GETRESTURANTORDERBYQUERY = "GetResturantOrderByQuery";
		#endregion
		
		#region Constructors
		public ResturantOrderDataAccess(ClientContext context) : base(context) { }
		public ResturantOrderDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="resturantOrderObject"></param>
		private void AddCommonParams(SqlCommand cmd, ResturantOrderBase resturantOrderObject)
		{	
			AddParameter(cmd, pGuid(ResturantOrderBase.Property_OrderId, resturantOrderObject.OrderId));
			AddParameter(cmd, pGuid(ResturantOrderBase.Property_CustomerId, resturantOrderObject.CustomerId));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_Location, resturantOrderObject.Location));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_OrderType, 150, resturantOrderObject.OrderType));
			AddParameter(cmd, pDouble(ResturantOrderBase.Property_Amount, resturantOrderObject.Amount));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_Status, 150, resturantOrderObject.Status));
			AddParameter(cmd, pInt32(ResturantOrderBase.Property_Quantity, resturantOrderObject.Quantity));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_PickUpTime, 60, resturantOrderObject.PickUpTime));
			AddParameter(cmd, pDateTime(ResturantOrderBase.Property_CreatedDate, resturantOrderObject.CreatedDate));
			AddParameter(cmd, pDateTime(ResturantOrderBase.Property_LastUpdatedDate, resturantOrderObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_CreatedBy, 150, resturantOrderObject.CreatedBy));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_LastUpdatedBy, 150, resturantOrderObject.LastUpdatedBy));
			AddParameter(cmd, pGuid(ResturantOrderBase.Property_CompanyId, resturantOrderObject.CompanyId));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_ContactNo, 250, resturantOrderObject.ContactNo));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_SpecialInstruction, resturantOrderObject.SpecialInstruction));
			AddParameter(cmd, pDateTime(ResturantOrderBase.Property_OrderDate, resturantOrderObject.OrderDate));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_Notes, resturantOrderObject.Notes));
			AddParameter(cmd, pDateTime(ResturantOrderBase.Property_AcceptDate, resturantOrderObject.AcceptDate));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_RejectedReason, resturantOrderObject.RejectedReason));
			AddParameter(cmd, pDateTime(ResturantOrderBase.Property_RejectedDate, resturantOrderObject.RejectedDate));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_PaymentMethod, 150, resturantOrderObject.PaymentMethod));
			AddParameter(cmd, pDouble(ResturantOrderBase.Property_TaxAmount, resturantOrderObject.TaxAmount));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_RestaurantLocation, 250, resturantOrderObject.RestaurantLocation));
			AddParameter(cmd, pBool(ResturantOrderBase.Property_IsViewed, resturantOrderObject.IsViewed));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_AcceptTime, 150, resturantOrderObject.AcceptTime));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_DeliveryNotes, resturantOrderObject.DeliveryNotes));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_CardProfile, 150, resturantOrderObject.CardProfile));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_CardNumber, 250, resturantOrderObject.CardNumber));
			AddParameter(cmd, pBool(ResturantOrderBase.Property_IsDeleted, resturantOrderObject.IsDeleted));
			AddParameter(cmd, pDouble(ResturantOrderBase.Property_DiscountValue, resturantOrderObject.DiscountValue));
			AddParameter(cmd, pNVarChar(ResturantOrderBase.Property_DiscountCode, 250, resturantOrderObject.DiscountCode));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ResturantOrder
        /// </summary>
        /// <param name="resturantOrderObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ResturantOrderBase resturantOrderObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTURANTORDER);
	
				AddParameter(cmd, pInt32Out(ResturantOrderBase.Property_Id));
				AddCommonParams(cmd, resturantOrderObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					resturantOrderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					resturantOrderObject.Id = (Int32)GetOutParameter(cmd, ResturantOrderBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(resturantOrderObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ResturantOrder
        /// </summary>
        /// <param name="resturantOrderObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ResturantOrderBase resturantOrderObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTURANTORDER);
				
				AddParameter(cmd, pInt32(ResturantOrderBase.Property_Id, resturantOrderObject.Id));
				AddCommonParams(cmd, resturantOrderObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					resturantOrderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(resturantOrderObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ResturantOrder
        /// </summary>
        /// <param name="Id">Id of the ResturantOrder object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTURANTORDER);	
				
				AddParameter(cmd, pInt32(ResturantOrderBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ResturantOrder), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ResturantOrder object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ResturantOrder object to retrieve</param>
        /// <returns>ResturantOrder object, null if not found</returns>
		public ResturantOrder Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERBYID))
			{
				AddParameter( cmd, pInt32(ResturantOrderBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ResturantOrder objects 
        /// </summary>
        /// <returns>A list of ResturantOrder objects</returns>
		public ResturantOrderList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTURANTORDER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ResturantOrder objects by PageRequest
        /// </summary>
        /// <returns>A list of ResturantOrder objects</returns>
		public ResturantOrderList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTURANTORDER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ResturantOrderList _ResturantOrderList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ResturantOrderList;
			}
		}
		
		/// <summary>
        /// Retrieves all ResturantOrder objects by query String
        /// </summary>
        /// <returns>A list of ResturantOrder objects</returns>
		public ResturantOrderList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ResturantOrder Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ResturantOrder
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ResturantOrder Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ResturantOrder
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ResturantOrderRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERROWCOUNT))
			{
				SqlDataReader reader;
				_ResturantOrderRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ResturantOrderRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ResturantOrder object
        /// </summary>
        /// <param name="resturantOrderObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ResturantOrderBase resturantOrderObject, SqlDataReader reader, int start)
		{
			
				resturantOrderObject.Id = reader.GetInt32( start + 0 );			
				resturantOrderObject.OrderId = reader.GetGuid( start + 1 );			
				resturantOrderObject.CustomerId = reader.GetGuid( start + 2 );			
				resturantOrderObject.Location = reader.GetString( start + 3 );			
				resturantOrderObject.OrderType = reader.GetString( start + 4 );			
				resturantOrderObject.Amount = reader.GetDouble( start + 5 );			
				resturantOrderObject.Status = reader.GetString( start + 6 );			
				resturantOrderObject.Quantity = reader.GetInt32( start + 7 );			
				resturantOrderObject.PickUpTime = reader.GetString( start + 8 );			
				resturantOrderObject.CreatedDate = reader.GetDateTime( start + 9 );			
				resturantOrderObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
				resturantOrderObject.CreatedBy = reader.GetString( start + 11 );			
				resturantOrderObject.LastUpdatedBy = reader.GetString( start + 12 );			
				resturantOrderObject.CompanyId = reader.GetGuid( start + 13 );			
				if(!reader.IsDBNull(14)) resturantOrderObject.ContactNo = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) resturantOrderObject.SpecialInstruction = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) resturantOrderObject.OrderDate = reader.GetDateTime( start + 16 );			
				if(!reader.IsDBNull(17)) resturantOrderObject.Notes = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) resturantOrderObject.AcceptDate = reader.GetDateTime( start + 18 );			
				if(!reader.IsDBNull(19)) resturantOrderObject.RejectedReason = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) resturantOrderObject.RejectedDate = reader.GetDateTime( start + 20 );			
				if(!reader.IsDBNull(21)) resturantOrderObject.PaymentMethod = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) resturantOrderObject.TaxAmount = reader.GetDouble( start + 22 );			
				if(!reader.IsDBNull(23)) resturantOrderObject.RestaurantLocation = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) resturantOrderObject.IsViewed = reader.GetBoolean( start + 24 );			
				if(!reader.IsDBNull(25)) resturantOrderObject.AcceptTime = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) resturantOrderObject.DeliveryNotes = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) resturantOrderObject.CardProfile = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) resturantOrderObject.CardNumber = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) resturantOrderObject.IsDeleted = reader.GetBoolean( start + 29 );			
				if(!reader.IsDBNull(30)) resturantOrderObject.DiscountValue = reader.GetDouble( start + 30 );			
				if(!reader.IsDBNull(31)) resturantOrderObject.DiscountCode = reader.GetString( start + 31 );			
			FillBaseObject(resturantOrderObject, reader, (start + 32));

			
			resturantOrderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ResturantOrder object
        /// </summary>
        /// <param name="resturantOrderObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ResturantOrderBase resturantOrderObject, SqlDataReader reader)
		{
			FillObject(resturantOrderObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ResturantOrder object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ResturantOrder object</returns>
		private ResturantOrder GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ResturantOrder resturantOrderObject= new ResturantOrder();
					FillObject(resturantOrderObject, reader);
					return resturantOrderObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ResturantOrder objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ResturantOrder objects</returns>
		private ResturantOrderList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ResturantOrder list
			ResturantOrderList list = new ResturantOrderList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ResturantOrder resturantOrderObject = new ResturantOrder();
					FillObject(resturantOrderObject, reader);

					list.Add(resturantOrderObject);
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
