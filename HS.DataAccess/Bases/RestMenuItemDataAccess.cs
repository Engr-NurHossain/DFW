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
	public partial class RestMenuItemDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTMENUITEM = "InsertRestMenuItem";
		private const string UPDATERESTMENUITEM = "UpdateRestMenuItem";
		private const string DELETERESTMENUITEM = "DeleteRestMenuItem";
		private const string GETRESTMENUITEMBYID = "GetRestMenuItemById";
		private const string GETALLRESTMENUITEM = "GetAllRestMenuItem";
		private const string GETPAGEDRESTMENUITEM = "GetPagedRestMenuItem";
		private const string GETRESTMENUITEMMAXIMUMID = "GetRestMenuItemMaximumId";
		private const string GETRESTMENUITEMROWCOUNT = "GetRestMenuItemRowCount";	
		private const string GETRESTMENUITEMBYQUERY = "GetRestMenuItemByQuery";
		#endregion
		
		#region Constructors
		public RestMenuItemDataAccess(ClientContext context) : base(context) { }
		public RestMenuItemDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restMenuItemObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestMenuItemBase restMenuItemObject)
		{	
			AddParameter(cmd, pGuid(RestMenuItemBase.Property_ItemId, restMenuItemObject.ItemId));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_ItemName, 50, restMenuItemObject.ItemName));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_ItemNumber, 50, restMenuItemObject.ItemNumber));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_ItemLevel, 50, restMenuItemObject.ItemLevel));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_Description, restMenuItemObject.Description));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_Photo, restMenuItemObject.Photo));
			AddParameter(cmd, pInt32(RestMenuItemBase.Property_MaxQty, restMenuItemObject.MaxQty));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_DaysAvailable, restMenuItemObject.DaysAvailable));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_TimeAvailable, restMenuItemObject.TimeAvailable));
			AddParameter(cmd, pDouble(RestMenuItemBase.Property_Price, restMenuItemObject.Price));
			AddParameter(cmd, pBool(RestMenuItemBase.Property_Status, restMenuItemObject.Status));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_DaysAvailableOption, 50, restMenuItemObject.DaysAvailableOption));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_TimeAvailableOption, 50, restMenuItemObject.TimeAvailableOption));
			AddParameter(cmd, pGuid(RestMenuItemBase.Property_CompanyId, restMenuItemObject.CompanyId));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_UrlSlug, restMenuItemObject.UrlSlug));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_WebsiteURL, restMenuItemObject.WebsiteURL));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_MetaTitle, restMenuItemObject.MetaTitle));
			AddParameter(cmd, pNVarChar(RestMenuItemBase.Property_MetaDescription, restMenuItemObject.MetaDescription));
			AddParameter(cmd, pInt32(RestMenuItemBase.Property_DeliveryTime, restMenuItemObject.DeliveryTime));
			AddParameter(cmd, pBool(RestMenuItemBase.Property_IsTax, restMenuItemObject.IsTax));
			AddParameter(cmd, pDouble(RestMenuItemBase.Property_TaxPercentage, restMenuItemObject.TaxPercentage));
			AddParameter(cmd, pInt32(RestMenuItemBase.Property_OrderBy, restMenuItemObject.OrderBy));
			AddParameter(cmd, pBool(RestMenuItemBase.Property_IsInstruction, restMenuItemObject.IsInstruction));
			AddParameter(cmd, pGuid(RestMenuItemBase.Property_CreatedBy, restMenuItemObject.CreatedBy));
			AddParameter(cmd, pDateTime(RestMenuItemBase.Property_CreatedDate, restMenuItemObject.CreatedDate));
			AddParameter(cmd, pGuid(RestMenuItemBase.Property_LastUpdatedBy, restMenuItemObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RestMenuItemBase.Property_LastUpdatedDate, restMenuItemObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestMenuItem
        /// </summary>
        /// <param name="restMenuItemObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestMenuItemBase restMenuItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTMENUITEM);
	
				AddParameter(cmd, pInt32Out(RestMenuItemBase.Property_Id));
				AddCommonParams(cmd, restMenuItemObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restMenuItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restMenuItemObject.Id = (Int32)GetOutParameter(cmd, RestMenuItemBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restMenuItemObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestMenuItem
        /// </summary>
        /// <param name="restMenuItemObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestMenuItemBase restMenuItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTMENUITEM);
				
				AddParameter(cmd, pInt32(RestMenuItemBase.Property_Id, restMenuItemObject.Id));
				AddCommonParams(cmd, restMenuItemObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restMenuItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restMenuItemObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestMenuItem
        /// </summary>
        /// <param name="Id">Id of the RestMenuItem object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTMENUITEM);	
				
				AddParameter(cmd, pInt32(RestMenuItemBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestMenuItem), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestMenuItem object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestMenuItem object to retrieve</param>
        /// <returns>RestMenuItem object, null if not found</returns>
		public RestMenuItem Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMBYID))
			{
				AddParameter( cmd, pInt32(RestMenuItemBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestMenuItem objects 
        /// </summary>
        /// <returns>A list of RestMenuItem objects</returns>
		public RestMenuItemList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTMENUITEM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestMenuItem objects by PageRequest
        /// </summary>
        /// <returns>A list of RestMenuItem objects</returns>
		public RestMenuItemList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTMENUITEM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestMenuItemList _RestMenuItemList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestMenuItemList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestMenuItem objects by query String
        /// </summary>
        /// <returns>A list of RestMenuItem objects</returns>
		public RestMenuItemList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestMenuItem Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestMenuItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestMenuItem Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestMenuItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestMenuItemRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMROWCOUNT))
			{
				SqlDataReader reader;
				_RestMenuItemRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestMenuItemRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestMenuItem object
        /// </summary>
        /// <param name="restMenuItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestMenuItemBase restMenuItemObject, SqlDataReader reader, int start)
		{
			
				restMenuItemObject.Id = reader.GetInt32( start + 0 );			
				restMenuItemObject.ItemId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) restMenuItemObject.ItemName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) restMenuItemObject.ItemNumber = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) restMenuItemObject.ItemLevel = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) restMenuItemObject.Description = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) restMenuItemObject.Photo = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) restMenuItemObject.MaxQty = reader.GetInt32( start + 7 );			
				if(!reader.IsDBNull(8)) restMenuItemObject.DaysAvailable = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) restMenuItemObject.TimeAvailable = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) restMenuItemObject.Price = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) restMenuItemObject.Status = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) restMenuItemObject.DaysAvailableOption = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) restMenuItemObject.TimeAvailableOption = reader.GetString( start + 13 );			
				restMenuItemObject.CompanyId = reader.GetGuid( start + 14 );			
				if(!reader.IsDBNull(15)) restMenuItemObject.UrlSlug = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) restMenuItemObject.WebsiteURL = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) restMenuItemObject.MetaTitle = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) restMenuItemObject.MetaDescription = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) restMenuItemObject.DeliveryTime = reader.GetInt32( start + 19 );			
				if(!reader.IsDBNull(20)) restMenuItemObject.IsTax = reader.GetBoolean( start + 20 );			
				if(!reader.IsDBNull(21)) restMenuItemObject.TaxPercentage = reader.GetDouble( start + 21 );			
				if(!reader.IsDBNull(22)) restMenuItemObject.OrderBy = reader.GetInt32( start + 22 );			
				if(!reader.IsDBNull(23)) restMenuItemObject.IsInstruction = reader.GetBoolean( start + 23 );			
				restMenuItemObject.CreatedBy = reader.GetGuid( start + 24 );			
				restMenuItemObject.CreatedDate = reader.GetDateTime( start + 25 );			
				restMenuItemObject.LastUpdatedBy = reader.GetGuid( start + 26 );			
				restMenuItemObject.LastUpdatedDate = reader.GetDateTime( start + 27 );			
			FillBaseObject(restMenuItemObject, reader, (start + 28));

			
			restMenuItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestMenuItem object
        /// </summary>
        /// <param name="restMenuItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestMenuItemBase restMenuItemObject, SqlDataReader reader)
		{
			FillObject(restMenuItemObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestMenuItem object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestMenuItem object</returns>
		private RestMenuItem GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestMenuItem restMenuItemObject= new RestMenuItem();
					FillObject(restMenuItemObject, reader);
					return restMenuItemObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestMenuItem objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestMenuItem objects</returns>
		private RestMenuItemList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestMenuItem list
			RestMenuItemList list = new RestMenuItemList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestMenuItem restMenuItemObject = new RestMenuItem();
					FillObject(restMenuItemObject, reader);

					list.Add(restMenuItemObject);
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
