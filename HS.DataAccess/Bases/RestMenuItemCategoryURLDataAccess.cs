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
	public partial class RestMenuItemCategoryURLDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTMENUITEMCATEGORYURL = "InsertRestMenuItemCategoryURL";
		private const string UPDATERESTMENUITEMCATEGORYURL = "UpdateRestMenuItemCategoryURL";
		private const string DELETERESTMENUITEMCATEGORYURL = "DeleteRestMenuItemCategoryURL";
		private const string GETRESTMENUITEMCATEGORYURLBYID = "GetRestMenuItemCategoryURLById";
		private const string GETALLRESTMENUITEMCATEGORYURL = "GetAllRestMenuItemCategoryURL";
		private const string GETPAGEDRESTMENUITEMCATEGORYURL = "GetPagedRestMenuItemCategoryURL";
		private const string GETRESTMENUITEMCATEGORYURLMAXIMUMID = "GetRestMenuItemCategoryURLMaximumId";
		private const string GETRESTMENUITEMCATEGORYURLROWCOUNT = "GetRestMenuItemCategoryURLRowCount";	
		private const string GETRESTMENUITEMCATEGORYURLBYQUERY = "GetRestMenuItemCategoryURLByQuery";
		#endregion
		
		#region Constructors
		public RestMenuItemCategoryURLDataAccess(ClientContext context) : base(context) { }
		public RestMenuItemCategoryURLDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restMenuItemCategoryURLObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestMenuItemCategoryURLBase restMenuItemCategoryURLObject)
		{	
			AddParameter(cmd, pInt32(RestMenuItemCategoryURLBase.Property_MenuId, restMenuItemCategoryURLObject.MenuId));
			AddParameter(cmd, pInt32(RestMenuItemCategoryURLBase.Property_MenuItemId, restMenuItemCategoryURLObject.MenuItemId));
			AddParameter(cmd, pNVarChar(RestMenuItemCategoryURLBase.Property_MenuCategoryURL, restMenuItemCategoryURLObject.MenuCategoryURL));
			AddParameter(cmd, pNVarChar(RestMenuItemCategoryURLBase.Property_ItemCategoryURL, restMenuItemCategoryURLObject.ItemCategoryURL));
			AddParameter(cmd, pDateTime(RestMenuItemCategoryURLBase.Property_CreatedDate, restMenuItemCategoryURLObject.CreatedDate));
			AddParameter(cmd, pGuid(RestMenuItemCategoryURLBase.Property_CreatedBy, restMenuItemCategoryURLObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestMenuItemCategoryURL
        /// </summary>
        /// <param name="restMenuItemCategoryURLObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestMenuItemCategoryURLBase restMenuItemCategoryURLObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTMENUITEMCATEGORYURL);
	
				AddParameter(cmd, pInt32Out(RestMenuItemCategoryURLBase.Property_Id));
				AddCommonParams(cmd, restMenuItemCategoryURLObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restMenuItemCategoryURLObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restMenuItemCategoryURLObject.Id = (Int32)GetOutParameter(cmd, RestMenuItemCategoryURLBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restMenuItemCategoryURLObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestMenuItemCategoryURL
        /// </summary>
        /// <param name="restMenuItemCategoryURLObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestMenuItemCategoryURLBase restMenuItemCategoryURLObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTMENUITEMCATEGORYURL);
				
				AddParameter(cmd, pInt32(RestMenuItemCategoryURLBase.Property_Id, restMenuItemCategoryURLObject.Id));
				AddCommonParams(cmd, restMenuItemCategoryURLObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restMenuItemCategoryURLObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restMenuItemCategoryURLObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestMenuItemCategoryURL
        /// </summary>
        /// <param name="Id">Id of the RestMenuItemCategoryURL object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTMENUITEMCATEGORYURL);	
				
				AddParameter(cmd, pInt32(RestMenuItemCategoryURLBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestMenuItemCategoryURL), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestMenuItemCategoryURL object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestMenuItemCategoryURL object to retrieve</param>
        /// <returns>RestMenuItemCategoryURL object, null if not found</returns>
		public RestMenuItemCategoryURL Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYURLBYID))
			{
				AddParameter( cmd, pInt32(RestMenuItemCategoryURLBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestMenuItemCategoryURL objects 
        /// </summary>
        /// <returns>A list of RestMenuItemCategoryURL objects</returns>
		public RestMenuItemCategoryURLList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTMENUITEMCATEGORYURL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestMenuItemCategoryURL objects by PageRequest
        /// </summary>
        /// <returns>A list of RestMenuItemCategoryURL objects</returns>
		public RestMenuItemCategoryURLList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTMENUITEMCATEGORYURL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestMenuItemCategoryURLList _RestMenuItemCategoryURLList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestMenuItemCategoryURLList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestMenuItemCategoryURL objects by query String
        /// </summary>
        /// <returns>A list of RestMenuItemCategoryURL objects</returns>
		public RestMenuItemCategoryURLList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYURLBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestMenuItemCategoryURL Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestMenuItemCategoryURL
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYURLMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestMenuItemCategoryURL Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestMenuItemCategoryURL
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestMenuItemCategoryURLRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYURLROWCOUNT))
			{
				SqlDataReader reader;
				_RestMenuItemCategoryURLRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestMenuItemCategoryURLRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestMenuItemCategoryURL object
        /// </summary>
        /// <param name="restMenuItemCategoryURLObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestMenuItemCategoryURLBase restMenuItemCategoryURLObject, SqlDataReader reader, int start)
		{
			
				restMenuItemCategoryURLObject.Id = reader.GetInt32( start + 0 );			
				restMenuItemCategoryURLObject.MenuId = reader.GetInt32( start + 1 );			
				restMenuItemCategoryURLObject.MenuItemId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) restMenuItemCategoryURLObject.MenuCategoryURL = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) restMenuItemCategoryURLObject.ItemCategoryURL = reader.GetString( start + 4 );			
				restMenuItemCategoryURLObject.CreatedDate = reader.GetDateTime( start + 5 );			
				restMenuItemCategoryURLObject.CreatedBy = reader.GetGuid( start + 6 );			
			FillBaseObject(restMenuItemCategoryURLObject, reader, (start + 7));

			
			restMenuItemCategoryURLObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestMenuItemCategoryURL object
        /// </summary>
        /// <param name="restMenuItemCategoryURLObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestMenuItemCategoryURLBase restMenuItemCategoryURLObject, SqlDataReader reader)
		{
			FillObject(restMenuItemCategoryURLObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestMenuItemCategoryURL object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestMenuItemCategoryURL object</returns>
		private RestMenuItemCategoryURL GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestMenuItemCategoryURL restMenuItemCategoryURLObject= new RestMenuItemCategoryURL();
					FillObject(restMenuItemCategoryURLObject, reader);
					return restMenuItemCategoryURLObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestMenuItemCategoryURL objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestMenuItemCategoryURL objects</returns>
		private RestMenuItemCategoryURLList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestMenuItemCategoryURL list
			RestMenuItemCategoryURLList list = new RestMenuItemCategoryURLList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestMenuItemCategoryURL restMenuItemCategoryURLObject = new RestMenuItemCategoryURL();
					FillObject(restMenuItemCategoryURLObject, reader);

					list.Add(restMenuItemCategoryURLObject);
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
