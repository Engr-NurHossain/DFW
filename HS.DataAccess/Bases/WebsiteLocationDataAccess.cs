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
	public partial class WebsiteLocationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTWEBSITELOCATION = "InsertWebsiteLocation";
		private const string UPDATEWEBSITELOCATION = "UpdateWebsiteLocation";
		private const string DELETEWEBSITELOCATION = "DeleteWebsiteLocation";
		private const string GETWEBSITELOCATIONBYID = "GetWebsiteLocationById";
		private const string GETALLWEBSITELOCATION = "GetAllWebsiteLocation";
		private const string GETPAGEDWEBSITELOCATION = "GetPagedWebsiteLocation";
		private const string GETWEBSITELOCATIONMAXIMUMID = "GetWebsiteLocationMaximumId";
		private const string GETWEBSITELOCATIONROWCOUNT = "GetWebsiteLocationRowCount";	
		private const string GETWEBSITELOCATIONBYQUERY = "GetWebsiteLocationByQuery";
		#endregion
		
		#region Constructors
		public WebsiteLocationDataAccess(ClientContext context) : base(context) { }
		public WebsiteLocationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="websiteLocationObject"></param>
		private void AddCommonParams(SqlCommand cmd, WebsiteLocationBase websiteLocationObject)
		{	
			AddParameter(cmd, pGuid(WebsiteLocationBase.Property_CompanyId, websiteLocationObject.CompanyId));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_Name, 150, websiteLocationObject.Name));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_Address, 250, websiteLocationObject.Address));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_Address2, 250, websiteLocationObject.Address2));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_City, 50, websiteLocationObject.City));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_State, 50, websiteLocationObject.State));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_Zipcode, 50, websiteLocationObject.Zipcode));
			AddParameter(cmd, pDateTime(WebsiteLocationBase.Property_CreatedDate, websiteLocationObject.CreatedDate));
			AddParameter(cmd, pGuid(WebsiteLocationBase.Property_CreatedBy, websiteLocationObject.CreatedBy));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_PrimaryContact, 150, websiteLocationObject.PrimaryContact));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_DomainName, 150, websiteLocationObject.DomainName));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_StorePhone, 150, websiteLocationObject.StorePhone));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_TrackingPhonePhone, 150, websiteLocationObject.TrackingPhonePhone));
			AddParameter(cmd, pBool(WebsiteLocationBase.Property_OrdersEmail, websiteLocationObject.OrdersEmail));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_ThemeLoc, websiteLocationObject.ThemeLoc));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_HoursofOperation, 250, websiteLocationObject.HoursofOperation));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_DaysAvailable, 250, websiteLocationObject.DaysAvailable));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_DaysAvailableOption, 250, websiteLocationObject.DaysAvailableOption));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_HoursofOperationOption, 250, websiteLocationObject.HoursofOperationOption));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_OperationStartTime, 250, websiteLocationObject.OperationStartTime));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_OperationEndTime, 250, websiteLocationObject.OperationEndTime));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_UrlSlug, websiteLocationObject.UrlSlug));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_WebsiteURL, websiteLocationObject.WebsiteURL));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_MetaTitle, websiteLocationObject.MetaTitle));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_MetaDescription, websiteLocationObject.MetaDescription));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_CartOption, 150, websiteLocationObject.CartOption));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_ImageLoc, websiteLocationObject.ImageLoc));
			AddParameter(cmd, pBool(WebsiteLocationBase.Property_IsTax, websiteLocationObject.IsTax));
			AddParameter(cmd, pDouble(WebsiteLocationBase.Property_TaxPercentage, websiteLocationObject.TaxPercentage));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_CuisineType, 250, websiteLocationObject.CuisineType));
			AddParameter(cmd, pInt32(WebsiteLocationBase.Property_PreparationTime, websiteLocationObject.PreparationTime));
			AddParameter(cmd, pBool(WebsiteLocationBase.Property_IsInstruction, websiteLocationObject.IsInstruction));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_Notes, websiteLocationObject.Notes));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_InstructionNotes, websiteLocationObject.InstructionNotes));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_PaidOption, 250, websiteLocationObject.PaidOption));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_PaymentOption, websiteLocationObject.PaymentOption));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_FacebookFollowURL, websiteLocationObject.FacebookFollowURL));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_TwitterFollowURL, websiteLocationObject.TwitterFollowURL));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_InstagramFollowURL, websiteLocationObject.InstagramFollowURL));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_YoutubeFollowURL, websiteLocationObject.YoutubeFollowURL));
			AddParameter(cmd, pGuid(WebsiteLocationBase.Property_ReferCompanyId, websiteLocationObject.ReferCompanyId));
			AddParameter(cmd, pBool(WebsiteLocationBase.Property_IsDefault, websiteLocationObject.IsDefault));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_CoverImageLoc, websiteLocationObject.CoverImageLoc));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_ExpireTime, 150, websiteLocationObject.ExpireTime));
			AddParameter(cmd, pDouble(WebsiteLocationBase.Property_DeliveryRadius, websiteLocationObject.DeliveryRadius));
			AddParameter(cmd, pDateTime(WebsiteLocationBase.Property_CoverPhotoDate, websiteLocationObject.CoverPhotoDate));
			AddParameter(cmd, pDateTime(WebsiteLocationBase.Property_DirectoryPhotoDate, websiteLocationObject.DirectoryPhotoDate));
			AddParameter(cmd, pGuid(WebsiteLocationBase.Property_LastUpdatedBy, websiteLocationObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(WebsiteLocationBase.Property_LastUpdatedDate, websiteLocationObject.LastUpdatedDate));
			AddParameter(cmd, pDouble(WebsiteLocationBase.Property_DeliveryFee, websiteLocationObject.DeliveryFee));
			AddParameter(cmd, pBool(WebsiteLocationBase.Property_SearchEngineIndex, websiteLocationObject.SearchEngineIndex));
			AddParameter(cmd, pDouble(WebsiteLocationBase.Property_DiscountValue, websiteLocationObject.DiscountValue));
			AddParameter(cmd, pNVarChar(WebsiteLocationBase.Property_DiscountCode, 250, websiteLocationObject.DiscountCode));
			AddParameter(cmd, pInt32(WebsiteLocationBase.Property_MinimumDeliveryTime, websiteLocationObject.MinimumDeliveryTime));
			AddParameter(cmd, pDouble(WebsiteLocationBase.Property_MinimumOrderValue, websiteLocationObject.MinimumOrderValue));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts WebsiteLocation
        /// </summary>
        /// <param name="websiteLocationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(WebsiteLocationBase websiteLocationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTWEBSITELOCATION);
	
				AddParameter(cmd, pInt32Out(WebsiteLocationBase.Property_Id));
				AddCommonParams(cmd, websiteLocationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					websiteLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					websiteLocationObject.Id = (Int32)GetOutParameter(cmd, WebsiteLocationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(websiteLocationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates WebsiteLocation
        /// </summary>
        /// <param name="websiteLocationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(WebsiteLocationBase websiteLocationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEWEBSITELOCATION);
				
				AddParameter(cmd, pInt32(WebsiteLocationBase.Property_Id, websiteLocationObject.Id));
				AddCommonParams(cmd, websiteLocationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					websiteLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(websiteLocationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes WebsiteLocation
        /// </summary>
        /// <param name="Id">Id of the WebsiteLocation object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEWEBSITELOCATION);	
				
				AddParameter(cmd, pInt32(WebsiteLocationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(WebsiteLocation), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves WebsiteLocation object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the WebsiteLocation object to retrieve</param>
        /// <returns>WebsiteLocation object, null if not found</returns>
		public WebsiteLocation Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONBYID))
			{
				AddParameter( cmd, pInt32(WebsiteLocationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all WebsiteLocation objects 
        /// </summary>
        /// <returns>A list of WebsiteLocation objects</returns>
		public WebsiteLocationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLWEBSITELOCATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all WebsiteLocation objects by PageRequest
        /// </summary>
        /// <returns>A list of WebsiteLocation objects</returns>
		public WebsiteLocationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDWEBSITELOCATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				WebsiteLocationList _WebsiteLocationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _WebsiteLocationList;
			}
		}
		
		/// <summary>
        /// Retrieves all WebsiteLocation objects by query String
        /// </summary>
        /// <returns>A list of WebsiteLocation objects</returns>
		public WebsiteLocationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get WebsiteLocation Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of WebsiteLocation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get WebsiteLocation Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of WebsiteLocation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _WebsiteLocationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONROWCOUNT))
			{
				SqlDataReader reader;
				_WebsiteLocationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _WebsiteLocationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills WebsiteLocation object
        /// </summary>
        /// <param name="websiteLocationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(WebsiteLocationBase websiteLocationObject, SqlDataReader reader, int start)
		{
			
				websiteLocationObject.Id = reader.GetInt32( start + 0 );			
				websiteLocationObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) websiteLocationObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) websiteLocationObject.Address = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) websiteLocationObject.Address2 = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) websiteLocationObject.City = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) websiteLocationObject.State = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) websiteLocationObject.Zipcode = reader.GetString( start + 7 );			
				websiteLocationObject.CreatedDate = reader.GetDateTime( start + 8 );			
				websiteLocationObject.CreatedBy = reader.GetGuid( start + 9 );			
				if(!reader.IsDBNull(10)) websiteLocationObject.PrimaryContact = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) websiteLocationObject.DomainName = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) websiteLocationObject.StorePhone = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) websiteLocationObject.TrackingPhonePhone = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) websiteLocationObject.OrdersEmail = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) websiteLocationObject.ThemeLoc = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) websiteLocationObject.HoursofOperation = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) websiteLocationObject.DaysAvailable = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) websiteLocationObject.DaysAvailableOption = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) websiteLocationObject.HoursofOperationOption = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) websiteLocationObject.OperationStartTime = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) websiteLocationObject.OperationEndTime = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) websiteLocationObject.UrlSlug = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) websiteLocationObject.WebsiteURL = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) websiteLocationObject.MetaTitle = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) websiteLocationObject.MetaDescription = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) websiteLocationObject.CartOption = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) websiteLocationObject.ImageLoc = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) websiteLocationObject.IsTax = reader.GetBoolean( start + 28 );			
				if(!reader.IsDBNull(29)) websiteLocationObject.TaxPercentage = reader.GetDouble( start + 29 );			
				if(!reader.IsDBNull(30)) websiteLocationObject.CuisineType = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) websiteLocationObject.PreparationTime = reader.GetInt32( start + 31 );			
				if(!reader.IsDBNull(32)) websiteLocationObject.IsInstruction = reader.GetBoolean( start + 32 );			
				if(!reader.IsDBNull(33)) websiteLocationObject.Notes = reader.GetString( start + 33 );			
				if(!reader.IsDBNull(34)) websiteLocationObject.InstructionNotes = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) websiteLocationObject.PaidOption = reader.GetString( start + 35 );			
				if(!reader.IsDBNull(36)) websiteLocationObject.PaymentOption = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) websiteLocationObject.FacebookFollowURL = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) websiteLocationObject.TwitterFollowURL = reader.GetString( start + 38 );			
				if(!reader.IsDBNull(39)) websiteLocationObject.InstagramFollowURL = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) websiteLocationObject.YoutubeFollowURL = reader.GetString( start + 40 );			
				websiteLocationObject.ReferCompanyId = reader.GetGuid( start + 41 );			
				if(!reader.IsDBNull(42)) websiteLocationObject.IsDefault = reader.GetBoolean( start + 42 );			
				if(!reader.IsDBNull(43)) websiteLocationObject.CoverImageLoc = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) websiteLocationObject.ExpireTime = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) websiteLocationObject.DeliveryRadius = reader.GetDouble( start + 45 );			
				if(!reader.IsDBNull(46)) websiteLocationObject.CoverPhotoDate = reader.GetDateTime( start + 46 );			
				if(!reader.IsDBNull(47)) websiteLocationObject.DirectoryPhotoDate = reader.GetDateTime( start + 47 );			
				websiteLocationObject.LastUpdatedBy = reader.GetGuid( start + 48 );			
				websiteLocationObject.LastUpdatedDate = reader.GetDateTime( start + 49 );			
				if(!reader.IsDBNull(50)) websiteLocationObject.DeliveryFee = reader.GetDouble( start + 50 );			
				if(!reader.IsDBNull(51)) websiteLocationObject.SearchEngineIndex = reader.GetBoolean( start + 51 );			
				if(!reader.IsDBNull(52)) websiteLocationObject.DiscountValue = reader.GetDouble( start + 52 );			
				if(!reader.IsDBNull(53)) websiteLocationObject.DiscountCode = reader.GetString( start + 53 );			
				if(!reader.IsDBNull(54)) websiteLocationObject.MinimumDeliveryTime = reader.GetInt32( start + 54 );			
				if(!reader.IsDBNull(55)) websiteLocationObject.MinimumOrderValue = reader.GetDouble( start + 55 );			
			FillBaseObject(websiteLocationObject, reader, (start + 56));

			
			websiteLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills WebsiteLocation object
        /// </summary>
        /// <param name="websiteLocationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(WebsiteLocationBase websiteLocationObject, SqlDataReader reader)
		{
			FillObject(websiteLocationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves WebsiteLocation object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>WebsiteLocation object</returns>
		private WebsiteLocation GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					WebsiteLocation websiteLocationObject= new WebsiteLocation();
					FillObject(websiteLocationObject, reader);
					return websiteLocationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of WebsiteLocation objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of WebsiteLocation objects</returns>
		private WebsiteLocationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//WebsiteLocation list
			WebsiteLocationList list = new WebsiteLocationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					WebsiteLocation websiteLocationObject = new WebsiteLocation();
					FillObject(websiteLocationObject, reader);

					list.Add(websiteLocationObject);
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
