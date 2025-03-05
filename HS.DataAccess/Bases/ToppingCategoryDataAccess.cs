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
	public partial class ToppingCategoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTOPPINGCATEGORY = "InsertToppingCategory";
		private const string UPDATETOPPINGCATEGORY = "UpdateToppingCategory";
		private const string DELETETOPPINGCATEGORY = "DeleteToppingCategory";
		private const string GETTOPPINGCATEGORYBYID = "GetToppingCategoryById";
		private const string GETALLTOPPINGCATEGORY = "GetAllToppingCategory";
		private const string GETPAGEDTOPPINGCATEGORY = "GetPagedToppingCategory";
		private const string GETTOPPINGCATEGORYMAXIMUMID = "GetToppingCategoryMaximumId";
		private const string GETTOPPINGCATEGORYROWCOUNT = "GetToppingCategoryRowCount";	
		private const string GETTOPPINGCATEGORYBYQUERY = "GetToppingCategoryByQuery";
		#endregion
		
		#region Constructors
		public ToppingCategoryDataAccess(ClientContext context) : base(context) { }
		public ToppingCategoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="toppingCategoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, ToppingCategoryBase toppingCategoryObject)
		{	
			AddParameter(cmd, pNVarChar(ToppingCategoryBase.Property_ToppingCategory, 250, toppingCategoryObject.ToppingCategory));
			AddParameter(cmd, pGuid(ToppingCategoryBase.Property_CompanyId, toppingCategoryObject.CompanyId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ToppingCategory
        /// </summary>
        /// <param name="toppingCategoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ToppingCategoryBase toppingCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTOPPINGCATEGORY);
	
				AddParameter(cmd, pInt32Out(ToppingCategoryBase.Property_Id));
				AddCommonParams(cmd, toppingCategoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					toppingCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					toppingCategoryObject.Id = (Int32)GetOutParameter(cmd, ToppingCategoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(toppingCategoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ToppingCategory
        /// </summary>
        /// <param name="toppingCategoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ToppingCategoryBase toppingCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETOPPINGCATEGORY);
				
				AddParameter(cmd, pInt32(ToppingCategoryBase.Property_Id, toppingCategoryObject.Id));
				AddCommonParams(cmd, toppingCategoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					toppingCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(toppingCategoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ToppingCategory
        /// </summary>
        /// <param name="Id">Id of the ToppingCategory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETOPPINGCATEGORY);	
				
				AddParameter(cmd, pInt32(ToppingCategoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ToppingCategory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ToppingCategory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ToppingCategory object to retrieve</param>
        /// <returns>ToppingCategory object, null if not found</returns>
		public ToppingCategory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTOPPINGCATEGORYBYID))
			{
				AddParameter( cmd, pInt32(ToppingCategoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ToppingCategory objects 
        /// </summary>
        /// <returns>A list of ToppingCategory objects</returns>
		public ToppingCategoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTOPPINGCATEGORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ToppingCategory objects by PageRequest
        /// </summary>
        /// <returns>A list of ToppingCategory objects</returns>
		public ToppingCategoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTOPPINGCATEGORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ToppingCategoryList _ToppingCategoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ToppingCategoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all ToppingCategory objects by query String
        /// </summary>
        /// <returns>A list of ToppingCategory objects</returns>
		public ToppingCategoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTOPPINGCATEGORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ToppingCategory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ToppingCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTOPPINGCATEGORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ToppingCategory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ToppingCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ToppingCategoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTOPPINGCATEGORYROWCOUNT))
			{
				SqlDataReader reader;
				_ToppingCategoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ToppingCategoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ToppingCategory object
        /// </summary>
        /// <param name="toppingCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ToppingCategoryBase toppingCategoryObject, SqlDataReader reader, int start)
		{
			
				toppingCategoryObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) toppingCategoryObject.ToppingCategory = reader.GetString( start + 1 );			
				toppingCategoryObject.CompanyId = reader.GetGuid( start + 2 );			
			FillBaseObject(toppingCategoryObject, reader, (start + 3));

			
			toppingCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ToppingCategory object
        /// </summary>
        /// <param name="toppingCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ToppingCategoryBase toppingCategoryObject, SqlDataReader reader)
		{
			FillObject(toppingCategoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ToppingCategory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ToppingCategory object</returns>
		private ToppingCategory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ToppingCategory toppingCategoryObject= new ToppingCategory();
					FillObject(toppingCategoryObject, reader);
					return toppingCategoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ToppingCategory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ToppingCategory objects</returns>
		private ToppingCategoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ToppingCategory list
			ToppingCategoryList list = new ToppingCategoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ToppingCategory toppingCategoryObject = new ToppingCategory();
					FillObject(toppingCategoryObject, reader);

					list.Add(toppingCategoryObject);
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
