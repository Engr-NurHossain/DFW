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
	public partial class ThirdPartyCustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTHIRDPARTYCUSTOMER = "InsertThirdPartyCustomer";
		private const string UPDATETHIRDPARTYCUSTOMER = "UpdateThirdPartyCustomer";
		private const string DELETETHIRDPARTYCUSTOMER = "DeleteThirdPartyCustomer";
		private const string GETTHIRDPARTYCUSTOMERBYID = "GetThirdPartyCustomerById";
		private const string GETALLTHIRDPARTYCUSTOMER = "GetAllThirdPartyCustomer";
		private const string GETPAGEDTHIRDPARTYCUSTOMER = "GetPagedThirdPartyCustomer";
		private const string GETTHIRDPARTYCUSTOMERMAXIMUMID = "GetThirdPartyCustomerMaximumId";
		private const string GETTHIRDPARTYCUSTOMERROWCOUNT = "GetThirdPartyCustomerRowCount";	
		private const string GETTHIRDPARTYCUSTOMERBYQUERY = "GetThirdPartyCustomerByQuery";
		#endregion
		
		#region Constructors
		public ThirdPartyCustomerDataAccess(ClientContext context) : base(context) { }
		public ThirdPartyCustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="thirdPartyCustomerObject"></param>
		private void AddCommonParams(SqlCommand cmd, ThirdPartyCustomerBase thirdPartyCustomerObject)
		{	
			AddParameter(cmd, pGuid(ThirdPartyCustomerBase.Property_CustomerId, thirdPartyCustomerObject.CustomerId));
			AddParameter(cmd, pInt32(ThirdPartyCustomerBase.Property_CustomerNumber, thirdPartyCustomerObject.CustomerNumber));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_SiteName, thirdPartyCustomerObject.SiteName));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_TransectionID, thirdPartyCustomerObject.TransectionID));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_DealerNumber, thirdPartyCustomerObject.DealerNumber));
			AddParameter(cmd, pDateTime(ThirdPartyCustomerBase.Property_AccountOnlineDate, thirdPartyCustomerObject.AccountOnlineDate));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_CodeWord, 100, thirdPartyCustomerObject.CodeWord));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_SiteAddress, thirdPartyCustomerObject.SiteAddress));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_ReceiverPhone, 50, thirdPartyCustomerObject.ReceiverPhone));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_PanelPhone, 50, thirdPartyCustomerObject.PanelPhone));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_PanelLocation, thirdPartyCustomerObject.PanelLocation));
			AddParameter(cmd, pDateTime(ThirdPartyCustomerBase.Property_InstallDate, thirdPartyCustomerObject.InstallDate));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_PanelCode, 50, thirdPartyCustomerObject.PanelCode));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_City, 100, thirdPartyCustomerObject.City));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_State, 100, thirdPartyCustomerObject.State));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_ZipCode, 100, thirdPartyCustomerObject.ZipCode));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_CountryName, 100, thirdPartyCustomerObject.CountryName));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_CrossStreet, 200, thirdPartyCustomerObject.CrossStreet));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_eContact, 50, thirdPartyCustomerObject.eContact));
			AddParameter(cmd, pBool(ThirdPartyCustomerBase.Property_IsSold, thirdPartyCustomerObject.IsSold));
			AddParameter(cmd, pGuid(ThirdPartyCustomerBase.Property_CreatedBy, thirdPartyCustomerObject.CreatedBy));
			AddParameter(cmd, pNVarChar(ThirdPartyCustomerBase.Property_Platform, 50, thirdPartyCustomerObject.Platform));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ThirdPartyCustomer
        /// </summary>
        /// <param name="thirdPartyCustomerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ThirdPartyCustomerBase thirdPartyCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTHIRDPARTYCUSTOMER);
	
				AddParameter(cmd, pInt32Out(ThirdPartyCustomerBase.Property_Id));
				AddCommonParams(cmd, thirdPartyCustomerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					thirdPartyCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					thirdPartyCustomerObject.Id = (Int32)GetOutParameter(cmd, ThirdPartyCustomerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(thirdPartyCustomerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ThirdPartyCustomer
        /// </summary>
        /// <param name="thirdPartyCustomerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ThirdPartyCustomerBase thirdPartyCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETHIRDPARTYCUSTOMER);
				
				AddParameter(cmd, pInt32(ThirdPartyCustomerBase.Property_Id, thirdPartyCustomerObject.Id));
				AddCommonParams(cmd, thirdPartyCustomerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					thirdPartyCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(thirdPartyCustomerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ThirdPartyCustomer
        /// </summary>
        /// <param name="Id">Id of the ThirdPartyCustomer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETHIRDPARTYCUSTOMER);	
				
				AddParameter(cmd, pInt32(ThirdPartyCustomerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ThirdPartyCustomer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ThirdPartyCustomer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ThirdPartyCustomer object to retrieve</param>
        /// <returns>ThirdPartyCustomer object, null if not found</returns>
		public ThirdPartyCustomer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYCUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(ThirdPartyCustomerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ThirdPartyCustomer objects 
        /// </summary>
        /// <returns>A list of ThirdPartyCustomer objects</returns>
		public ThirdPartyCustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTHIRDPARTYCUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ThirdPartyCustomer objects by PageRequest
        /// </summary>
        /// <returns>A list of ThirdPartyCustomer objects</returns>
		public ThirdPartyCustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTHIRDPARTYCUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ThirdPartyCustomerList _ThirdPartyCustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ThirdPartyCustomerList;
			}
		}
		
		/// <summary>
        /// Retrieves all ThirdPartyCustomer objects by query String
        /// </summary>
        /// <returns>A list of ThirdPartyCustomer objects</returns>
		public ThirdPartyCustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYCUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ThirdPartyCustomer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ThirdPartyCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYCUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ThirdPartyCustomer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ThirdPartyCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ThirdPartyCustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYCUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_ThirdPartyCustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ThirdPartyCustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ThirdPartyCustomer object
        /// </summary>
        /// <param name="thirdPartyCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ThirdPartyCustomerBase thirdPartyCustomerObject, SqlDataReader reader, int start)
		{
			
				thirdPartyCustomerObject.Id = reader.GetInt32( start + 0 );			
				thirdPartyCustomerObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) thirdPartyCustomerObject.CustomerNumber = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) thirdPartyCustomerObject.SiteName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) thirdPartyCustomerObject.TransectionID = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) thirdPartyCustomerObject.DealerNumber = reader.GetString( start + 5 );			
				thirdPartyCustomerObject.AccountOnlineDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) thirdPartyCustomerObject.CodeWord = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) thirdPartyCustomerObject.SiteAddress = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) thirdPartyCustomerObject.ReceiverPhone = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) thirdPartyCustomerObject.PanelPhone = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) thirdPartyCustomerObject.PanelLocation = reader.GetString( start + 11 );			
				thirdPartyCustomerObject.InstallDate = reader.GetDateTime( start + 12 );			
				if(!reader.IsDBNull(13)) thirdPartyCustomerObject.PanelCode = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) thirdPartyCustomerObject.City = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) thirdPartyCustomerObject.State = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) thirdPartyCustomerObject.ZipCode = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) thirdPartyCustomerObject.CountryName = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) thirdPartyCustomerObject.CrossStreet = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) thirdPartyCustomerObject.eContact = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) thirdPartyCustomerObject.IsSold = reader.GetBoolean( start + 20 );			
				thirdPartyCustomerObject.CreatedBy = reader.GetGuid( start + 21 );			
				if(!reader.IsDBNull(22)) thirdPartyCustomerObject.Platform = reader.GetString( start + 22 );			
			FillBaseObject(thirdPartyCustomerObject, reader, (start + 23));

			
			thirdPartyCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ThirdPartyCustomer object
        /// </summary>
        /// <param name="thirdPartyCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ThirdPartyCustomerBase thirdPartyCustomerObject, SqlDataReader reader)
		{
			FillObject(thirdPartyCustomerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ThirdPartyCustomer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ThirdPartyCustomer object</returns>
		private ThirdPartyCustomer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ThirdPartyCustomer thirdPartyCustomerObject= new ThirdPartyCustomer();
					FillObject(thirdPartyCustomerObject, reader);
					return thirdPartyCustomerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ThirdPartyCustomer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ThirdPartyCustomer objects</returns>
		private ThirdPartyCustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ThirdPartyCustomer list
			ThirdPartyCustomerList list = new ThirdPartyCustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ThirdPartyCustomer thirdPartyCustomerObject = new ThirdPartyCustomer();
					FillObject(thirdPartyCustomerObject, reader);

					list.Add(thirdPartyCustomerObject);
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
