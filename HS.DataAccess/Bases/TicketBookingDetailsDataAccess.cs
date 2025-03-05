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
	public partial class TicketBookingDetailsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETBOOKINGDETAILS = "InsertTicketBookingDetails";
		private const string UPDATETICKETBOOKINGDETAILS = "UpdateTicketBookingDetails";
		private const string DELETETICKETBOOKINGDETAILS = "DeleteTicketBookingDetails";
		private const string GETTICKETBOOKINGDETAILSBYID = "GetTicketBookingDetailsById";
		private const string GETALLTICKETBOOKINGDETAILS = "GetAllTicketBookingDetails";
		private const string GETPAGEDTICKETBOOKINGDETAILS = "GetPagedTicketBookingDetails";
		private const string GETTICKETBOOKINGDETAILSMAXIMUMID = "GetTicketBookingDetailsMaximumId";
		private const string GETTICKETBOOKINGDETAILSROWCOUNT = "GetTicketBookingDetailsRowCount";	
		private const string GETTICKETBOOKINGDETAILSBYQUERY = "GetTicketBookingDetailsByQuery";
		#endregion
		
		#region Constructors
		public TicketBookingDetailsDataAccess(ClientContext context) : base(context) { }
		public TicketBookingDetailsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketBookingDetailsObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketBookingDetailsBase ticketBookingDetailsObject)
		{	
			AddParameter(cmd, pGuid(TicketBookingDetailsBase.Property_CompanyId, ticketBookingDetailsObject.CompanyId));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_BookingId, 15, ticketBookingDetailsObject.BookingId));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_RugType, 30, ticketBookingDetailsObject.RugType));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_Length, ticketBookingDetailsObject.Length));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_LengthInch, ticketBookingDetailsObject.LengthInch));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_Width, ticketBookingDetailsObject.Width));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_WidthInch, ticketBookingDetailsObject.WidthInch));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_Radius, ticketBookingDetailsObject.Radius));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_RadiusInch, ticketBookingDetailsObject.RadiusInch));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_TotalSize, ticketBookingDetailsObject.TotalSize));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_Package, 100, ticketBookingDetailsObject.Package));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_Included, 100, ticketBookingDetailsObject.Included));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_Extras, 100, ticketBookingDetailsObject.Extras));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_UnitPrice, ticketBookingDetailsObject.UnitPrice));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_DiscountType, 50, ticketBookingDetailsObject.DiscountType));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_TaxType, 50, ticketBookingDetailsObject.TaxType));
			AddParameter(cmd, pInt32(TicketBookingDetailsBase.Property_Quantity, ticketBookingDetailsObject.Quantity));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_Discount, ticketBookingDetailsObject.Discount));
			AddParameter(cmd, pDouble(TicketBookingDetailsBase.Property_TotalPrice, ticketBookingDetailsObject.TotalPrice));
			AddParameter(cmd, pDateTime(TicketBookingDetailsBase.Property_AddedDate, ticketBookingDetailsObject.AddedDate));
			AddParameter(cmd, pGuid(TicketBookingDetailsBase.Property_AddedBy, ticketBookingDetailsObject.AddedBy));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_RugConditions, 1000, ticketBookingDetailsObject.RugConditions));
			AddParameter(cmd, pNVarChar(TicketBookingDetailsBase.Property_Comments, ticketBookingDetailsObject.Comments));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketBookingDetails
        /// </summary>
        /// <param name="ticketBookingDetailsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketBookingDetailsBase ticketBookingDetailsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETBOOKINGDETAILS);
	
				AddParameter(cmd, pInt32Out(TicketBookingDetailsBase.Property_Id));
				AddCommonParams(cmd, ticketBookingDetailsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketBookingDetailsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketBookingDetailsObject.Id = (Int32)GetOutParameter(cmd, TicketBookingDetailsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketBookingDetailsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketBookingDetails
        /// </summary>
        /// <param name="ticketBookingDetailsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketBookingDetailsBase ticketBookingDetailsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETBOOKINGDETAILS);
				
				AddParameter(cmd, pInt32(TicketBookingDetailsBase.Property_Id, ticketBookingDetailsObject.Id));
				AddCommonParams(cmd, ticketBookingDetailsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketBookingDetailsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketBookingDetailsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketBookingDetails
        /// </summary>
        /// <param name="Id">Id of the TicketBookingDetails object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETBOOKINGDETAILS);	
				
				AddParameter(cmd, pInt32(TicketBookingDetailsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketBookingDetails), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketBookingDetails object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketBookingDetails object to retrieve</param>
        /// <returns>TicketBookingDetails object, null if not found</returns>
		public TicketBookingDetails Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETBOOKINGDETAILSBYID))
			{
				AddParameter( cmd, pInt32(TicketBookingDetailsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketBookingDetails objects 
        /// </summary>
        /// <returns>A list of TicketBookingDetails objects</returns>
		public TicketBookingDetailsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETBOOKINGDETAILS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketBookingDetails objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketBookingDetails objects</returns>
		public TicketBookingDetailsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETBOOKINGDETAILS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketBookingDetailsList _TicketBookingDetailsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketBookingDetailsList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketBookingDetails objects by query String
        /// </summary>
        /// <returns>A list of TicketBookingDetails objects</returns>
		public TicketBookingDetailsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETBOOKINGDETAILSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketBookingDetails Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketBookingDetails
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETBOOKINGDETAILSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketBookingDetails Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketBookingDetails
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketBookingDetailsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETBOOKINGDETAILSROWCOUNT))
			{
				SqlDataReader reader;
				_TicketBookingDetailsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketBookingDetailsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketBookingDetails object
        /// </summary>
        /// <param name="ticketBookingDetailsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketBookingDetailsBase ticketBookingDetailsObject, SqlDataReader reader, int start)
		{
			
				ticketBookingDetailsObject.Id = reader.GetInt32( start + 0 );			
				ticketBookingDetailsObject.CompanyId = reader.GetGuid( start + 1 );			
				ticketBookingDetailsObject.BookingId = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) ticketBookingDetailsObject.RugType = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) ticketBookingDetailsObject.Length = reader.GetDouble( start + 4 );			
				if(!reader.IsDBNull(5)) ticketBookingDetailsObject.LengthInch = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) ticketBookingDetailsObject.Width = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) ticketBookingDetailsObject.WidthInch = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) ticketBookingDetailsObject.Radius = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) ticketBookingDetailsObject.RadiusInch = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) ticketBookingDetailsObject.TotalSize = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) ticketBookingDetailsObject.Package = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) ticketBookingDetailsObject.Included = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) ticketBookingDetailsObject.Extras = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) ticketBookingDetailsObject.UnitPrice = reader.GetDouble( start + 14 );			
				if(!reader.IsDBNull(15)) ticketBookingDetailsObject.DiscountType = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) ticketBookingDetailsObject.TaxType = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) ticketBookingDetailsObject.Quantity = reader.GetInt32( start + 17 );			
				if(!reader.IsDBNull(18)) ticketBookingDetailsObject.Discount = reader.GetDouble( start + 18 );			
				if(!reader.IsDBNull(19)) ticketBookingDetailsObject.TotalPrice = reader.GetDouble( start + 19 );			
				ticketBookingDetailsObject.AddedDate = reader.GetDateTime( start + 20 );			
				ticketBookingDetailsObject.AddedBy = reader.GetGuid( start + 21 );			
				if(!reader.IsDBNull(22)) ticketBookingDetailsObject.RugConditions = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) ticketBookingDetailsObject.Comments = reader.GetString( start + 23 );			
			FillBaseObject(ticketBookingDetailsObject, reader, (start + 24));

			
			ticketBookingDetailsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketBookingDetails object
        /// </summary>
        /// <param name="ticketBookingDetailsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketBookingDetailsBase ticketBookingDetailsObject, SqlDataReader reader)
		{
			FillObject(ticketBookingDetailsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketBookingDetails object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketBookingDetails object</returns>
		private TicketBookingDetails GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketBookingDetails ticketBookingDetailsObject= new TicketBookingDetails();
					FillObject(ticketBookingDetailsObject, reader);
					return ticketBookingDetailsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketBookingDetails objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketBookingDetails objects</returns>
		private TicketBookingDetailsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketBookingDetails list
			TicketBookingDetailsList list = new TicketBookingDetailsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketBookingDetails ticketBookingDetailsObject = new TicketBookingDetails();
					FillObject(ticketBookingDetailsObject, reader);

					list.Add(ticketBookingDetailsObject);
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
