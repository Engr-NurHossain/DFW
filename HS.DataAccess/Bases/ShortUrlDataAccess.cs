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
	public partial class ShortUrlDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSHORTURL = "InsertShortUrl";
		private const string UPDATESHORTURL = "UpdateShortUrl";
		private const string DELETESHORTURL = "DeleteShortUrl";
		private const string GETSHORTURLBYID = "GetShortUrlById";
		private const string GETALLSHORTURL = "GetAllShortUrl";
		private const string GETPAGEDSHORTURL = "GetPagedShortUrl";
		private const string GETSHORTURLMAXIMUMID = "GetShortUrlMaximumId";
		private const string GETSHORTURLROWCOUNT = "GetShortUrlRowCount";	
		private const string GETSHORTURLBYQUERY = "GetShortUrlByQuery";
        #endregion

        #region Constructors
        public ShortUrlDataAccess(string ConnectionString) : base(ConnectionString) { }
        public ShortUrlDataAccess(ClientContext context) : base(context) { }
		public ShortUrlDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="shortUrlObject"></param>
		private void AddCommonParams(SqlCommand cmd, ShortUrlBase shortUrlObject)
		{	
			AddParameter(cmd, pGuid(ShortUrlBase.Property_CustomerId, shortUrlObject.CustomerId));
			AddParameter(cmd, pNVarChar(ShortUrlBase.Property_Code, 50, shortUrlObject.Code));
			AddParameter(cmd, pNVarChar(ShortUrlBase.Property_Url, shortUrlObject.Url));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ShortUrl
        /// </summary>
        /// <param name="shortUrlObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ShortUrlBase shortUrlObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSHORTURL);
	
				AddParameter(cmd, pInt32Out(ShortUrlBase.Property_Id));
				AddCommonParams(cmd, shortUrlObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					shortUrlObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					shortUrlObject.Id = (Int32)GetOutParameter(cmd, ShortUrlBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(shortUrlObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ShortUrl
        /// </summary>
        /// <param name="shortUrlObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ShortUrlBase shortUrlObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESHORTURL);
				
				AddParameter(cmd, pInt32(ShortUrlBase.Property_Id, shortUrlObject.Id));
				AddCommonParams(cmd, shortUrlObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					shortUrlObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(shortUrlObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ShortUrl
        /// </summary>
        /// <param name="Id">Id of the ShortUrl object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESHORTURL);	
				
				AddParameter(cmd, pInt32(ShortUrlBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ShortUrl), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ShortUrl object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ShortUrl object to retrieve</param>
        /// <returns>ShortUrl object, null if not found</returns>
		public ShortUrl Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSHORTURLBYID))
			{
				AddParameter( cmd, pInt32(ShortUrlBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ShortUrl objects 
        /// </summary>
        /// <returns>A list of ShortUrl objects</returns>
		public ShortUrlList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSHORTURL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ShortUrl objects by PageRequest
        /// </summary>
        /// <returns>A list of ShortUrl objects</returns>
		public ShortUrlList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSHORTURL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ShortUrlList _ShortUrlList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ShortUrlList;
			}
		}
		
		/// <summary>
        /// Retrieves all ShortUrl objects by query String
        /// </summary>
        /// <returns>A list of ShortUrl objects</returns>
		public ShortUrlList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSHORTURLBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ShortUrl Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ShortUrl
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSHORTURLMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ShortUrl Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ShortUrl
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ShortUrlRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSHORTURLROWCOUNT))
			{
				SqlDataReader reader;
				_ShortUrlRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ShortUrlRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ShortUrl object
        /// </summary>
        /// <param name="shortUrlObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ShortUrlBase shortUrlObject, SqlDataReader reader, int start)
		{
			
				shortUrlObject.Id = reader.GetInt32( start + 0 );			
				shortUrlObject.CustomerId = reader.GetGuid( start + 1 );			
				shortUrlObject.Code = reader.GetString( start + 2 );			
				shortUrlObject.Url = reader.GetString( start + 3 );			
			FillBaseObject(shortUrlObject, reader, (start + 4));

			
			shortUrlObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ShortUrl object
        /// </summary>
        /// <param name="shortUrlObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ShortUrlBase shortUrlObject, SqlDataReader reader)
		{
			FillObject(shortUrlObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ShortUrl object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ShortUrl object</returns>
		private ShortUrl GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ShortUrl shortUrlObject= new ShortUrl();
					FillObject(shortUrlObject, reader);
					return shortUrlObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ShortUrl objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ShortUrl objects</returns>
		private ShortUrlList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ShortUrl list
			ShortUrlList list = new ShortUrlList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ShortUrl shortUrlObject = new ShortUrl();
					FillObject(shortUrlObject, reader);

					list.Add(shortUrlObject);
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
