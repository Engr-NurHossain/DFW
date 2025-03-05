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
	public partial class RestToppingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTTOPPING = "InsertRestTopping";
		private const string UPDATERESTTOPPING = "UpdateRestTopping";
		private const string DELETERESTTOPPING = "DeleteRestTopping";
		private const string GETRESTTOPPINGBYID = "GetRestToppingById";
		private const string GETALLRESTTOPPING = "GetAllRestTopping";
		private const string GETPAGEDRESTTOPPING = "GetPagedRestTopping";
		private const string GETRESTTOPPINGMAXIMUMID = "GetRestToppingMaximumId";
		private const string GETRESTTOPPINGROWCOUNT = "GetRestToppingRowCount";	
		private const string GETRESTTOPPINGBYQUERY = "GetRestToppingByQuery";
		#endregion
		
		#region Constructors
		public RestToppingDataAccess(ClientContext context) : base(context) { }
		public RestToppingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restToppingObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestToppingBase restToppingObject)
		{	
			AddParameter(cmd, pGuid(RestToppingBase.Property_ToppingId, restToppingObject.ToppingId));
			AddParameter(cmd, pNVarChar(RestToppingBase.Property_ToppingName, 100, restToppingObject.ToppingName));
			AddParameter(cmd, pDouble(RestToppingBase.Property_Price, restToppingObject.Price));
			AddParameter(cmd, pBool(RestToppingBase.Property_IsAvailable, restToppingObject.IsAvailable));
			AddParameter(cmd, pDateTime(RestToppingBase.Property_CreatedDate, restToppingObject.CreatedDate));
			AddParameter(cmd, pGuid(RestToppingBase.Property_CreatedBy, restToppingObject.CreatedBy));
			AddParameter(cmd, pGuid(RestToppingBase.Property_LastUpdatedBy, restToppingObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RestToppingBase.Property_LastUpdatedDate, restToppingObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(RestToppingBase.Property_CompanyId, restToppingObject.CompanyId));
			AddParameter(cmd, pGuid(RestToppingBase.Property_ToppingCategoryId, restToppingObject.ToppingCategoryId));
			AddParameter(cmd, pBool(RestToppingBase.Property_IsDefault, restToppingObject.IsDefault));
			AddParameter(cmd, pNVarChar(RestToppingBase.Property_Description, restToppingObject.Description));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestTopping
        /// </summary>
        /// <param name="restToppingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestToppingBase restToppingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTTOPPING);
	
				AddParameter(cmd, pInt32Out(RestToppingBase.Property_Id));
				AddCommonParams(cmd, restToppingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restToppingObject.Id = (Int32)GetOutParameter(cmd, RestToppingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restToppingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestTopping
        /// </summary>
        /// <param name="restToppingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestToppingBase restToppingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTTOPPING);
				
				AddParameter(cmd, pInt32(RestToppingBase.Property_Id, restToppingObject.Id));
				AddCommonParams(cmd, restToppingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restToppingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestTopping
        /// </summary>
        /// <param name="Id">Id of the RestTopping object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTTOPPING);	
				
				AddParameter(cmd, pInt32(RestToppingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestTopping), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestTopping object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestTopping object to retrieve</param>
        /// <returns>RestTopping object, null if not found</returns>
		public RestTopping Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTTOPPINGBYID))
			{
				AddParameter( cmd, pInt32(RestToppingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestTopping objects 
        /// </summary>
        /// <returns>A list of RestTopping objects</returns>
		public RestToppingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTTOPPING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestTopping objects by PageRequest
        /// </summary>
        /// <returns>A list of RestTopping objects</returns>
		public RestToppingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTTOPPING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestToppingList _RestToppingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestToppingList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestTopping objects by query String
        /// </summary>
        /// <returns>A list of RestTopping objects</returns>
		public RestToppingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTTOPPINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestTopping Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestTopping
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTTOPPINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestTopping Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestTopping
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestToppingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTTOPPINGROWCOUNT))
			{
				SqlDataReader reader;
				_RestToppingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestToppingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestTopping object
        /// </summary>
        /// <param name="restToppingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestToppingBase restToppingObject, SqlDataReader reader, int start)
		{
			
				restToppingObject.Id = reader.GetInt32( start + 0 );			
				restToppingObject.ToppingId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) restToppingObject.ToppingName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) restToppingObject.Price = reader.GetDouble( start + 3 );			
				if(!reader.IsDBNull(4)) restToppingObject.IsAvailable = reader.GetBoolean( start + 4 );			
				restToppingObject.CreatedDate = reader.GetDateTime( start + 5 );			
				restToppingObject.CreatedBy = reader.GetGuid( start + 6 );			
				restToppingObject.LastUpdatedBy = reader.GetGuid( start + 7 );			
				restToppingObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
				restToppingObject.CompanyId = reader.GetGuid( start + 9 );			
				restToppingObject.ToppingCategoryId = reader.GetGuid( start + 10 );			
				if(!reader.IsDBNull(11)) restToppingObject.IsDefault = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) restToppingObject.Description = reader.GetString( start + 12 );			
			FillBaseObject(restToppingObject, reader, (start + 13));

			
			restToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestTopping object
        /// </summary>
        /// <param name="restToppingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestToppingBase restToppingObject, SqlDataReader reader)
		{
			FillObject(restToppingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestTopping object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestTopping object</returns>
		private RestTopping GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestTopping restToppingObject= new RestTopping();
					FillObject(restToppingObject, reader);
					return restToppingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestTopping objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestTopping objects</returns>
		private RestToppingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestTopping list
			RestToppingList list = new RestToppingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestTopping restToppingObject = new RestTopping();
					FillObject(restToppingObject, reader);

					list.Add(restToppingObject);
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
