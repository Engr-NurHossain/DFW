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
	public partial class ToppingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTOPPING = "InsertTopping";
		private const string UPDATETOPPING = "UpdateTopping";
		private const string DELETETOPPING = "DeleteTopping";
		private const string GETTOPPINGBYID = "GetToppingById";
		private const string GETALLTOPPING = "GetAllTopping";
		private const string GETPAGEDTOPPING = "GetPagedTopping";
		private const string GETTOPPINGMAXIMUMID = "GetToppingMaximumId";
		private const string GETTOPPINGROWCOUNT = "GetToppingRowCount";	
		private const string GETTOPPINGBYQUERY = "GetToppingByQuery";
		#endregion
		
		#region Constructors
		public ToppingDataAccess(ClientContext context) : base(context) { }
		public ToppingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="toppingObject"></param>
		private void AddCommonParams(SqlCommand cmd, ToppingBase toppingObject)
		{	
			AddParameter(cmd, pNVarChar(ToppingBase.Property_ToppingName, 100, toppingObject.ToppingName));
			AddParameter(cmd, pDouble(ToppingBase.Property_Price, toppingObject.Price));
			AddParameter(cmd, pBool(ToppingBase.Property_IsAvailable, toppingObject.IsAvailable));
			AddParameter(cmd, pDateTime(ToppingBase.Property_CreatedDate, toppingObject.CreatedDate));
			AddParameter(cmd, pGuid(ToppingBase.Property_CreatedBy, toppingObject.CreatedBy));
			AddParameter(cmd, pGuid(ToppingBase.Property_LastUpdatedBy, toppingObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(ToppingBase.Property_LastUpdatedDate, toppingObject.LastUpdatedDate));
			AddParameter(cmd, pInt32(ToppingBase.Property_ToppingCategoryId, toppingObject.ToppingCategoryId));
			AddParameter(cmd, pGuid(ToppingBase.Property_CompanyId, toppingObject.CompanyId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Topping
        /// </summary>
        /// <param name="toppingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ToppingBase toppingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTOPPING);
	
				AddParameter(cmd, pInt32Out(ToppingBase.Property_Id));
				AddCommonParams(cmd, toppingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					toppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					toppingObject.Id = (Int32)GetOutParameter(cmd, ToppingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(toppingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Topping
        /// </summary>
        /// <param name="toppingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ToppingBase toppingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETOPPING);
				
				AddParameter(cmd, pInt32(ToppingBase.Property_Id, toppingObject.Id));
				AddCommonParams(cmd, toppingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					toppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(toppingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Topping
        /// </summary>
        /// <param name="Id">Id of the Topping object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETOPPING);	
				
				AddParameter(cmd, pInt32(ToppingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Topping), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Topping object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Topping object to retrieve</param>
        /// <returns>Topping object, null if not found</returns>
		public Topping Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTOPPINGBYID))
			{
				AddParameter( cmd, pInt32(ToppingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Topping objects 
        /// </summary>
        /// <returns>A list of Topping objects</returns>
		public ToppingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTOPPING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Topping objects by PageRequest
        /// </summary>
        /// <returns>A list of Topping objects</returns>
		public ToppingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTOPPING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ToppingList _ToppingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ToppingList;
			}
		}
		
		/// <summary>
        /// Retrieves all Topping objects by query String
        /// </summary>
        /// <returns>A list of Topping objects</returns>
		public ToppingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTOPPINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Topping Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Topping
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTOPPINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Topping Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Topping
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ToppingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTOPPINGROWCOUNT))
			{
				SqlDataReader reader;
				_ToppingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ToppingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Topping object
        /// </summary>
        /// <param name="toppingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ToppingBase toppingObject, SqlDataReader reader, int start)
		{
			
				toppingObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) toppingObject.ToppingName = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) toppingObject.Price = reader.GetDouble( start + 2 );			
				if(!reader.IsDBNull(3)) toppingObject.IsAvailable = reader.GetBoolean( start + 3 );			
				toppingObject.CreatedDate = reader.GetDateTime( start + 4 );			
				toppingObject.CreatedBy = reader.GetGuid( start + 5 );			
				toppingObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
				toppingObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) toppingObject.ToppingCategoryId = reader.GetInt32( start + 8 );			
				toppingObject.CompanyId = reader.GetGuid( start + 9 );			
			FillBaseObject(toppingObject, reader, (start + 10));

			
			toppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Topping object
        /// </summary>
        /// <param name="toppingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ToppingBase toppingObject, SqlDataReader reader)
		{
			FillObject(toppingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Topping object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Topping object</returns>
		private Topping GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Topping toppingObject= new Topping();
					FillObject(toppingObject, reader);
					return toppingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Topping objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Topping objects</returns>
		private ToppingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Topping list
			ToppingList list = new ToppingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Topping toppingObject = new Topping();
					FillObject(toppingObject, reader);

					list.Add(toppingObject);
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
