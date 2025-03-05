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
	public partial class NewsletterSubscribeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTNEWSLETTERSUBSCRIBE = "InsertNewsletterSubscribe";
		private const string UPDATENEWSLETTERSUBSCRIBE = "UpdateNewsletterSubscribe";
		private const string DELETENEWSLETTERSUBSCRIBE = "DeleteNewsletterSubscribe";
		private const string GETNEWSLETTERSUBSCRIBEBYID = "GetNewsletterSubscribeById";
		private const string GETALLNEWSLETTERSUBSCRIBE = "GetAllNewsletterSubscribe";
		private const string GETPAGEDNEWSLETTERSUBSCRIBE = "GetPagedNewsletterSubscribe";
		private const string GETNEWSLETTERSUBSCRIBEMAXIMUMID = "GetNewsletterSubscribeMaximumId";
		private const string GETNEWSLETTERSUBSCRIBEROWCOUNT = "GetNewsletterSubscribeRowCount";	
		private const string GETNEWSLETTERSUBSCRIBEBYQUERY = "GetNewsletterSubscribeByQuery";
		#endregion
		
		#region Constructors
		public NewsletterSubscribeDataAccess(ClientContext context) : base(context) { }
		public NewsletterSubscribeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="newsletterSubscribeObject"></param>
		private void AddCommonParams(SqlCommand cmd, NewsletterSubscribeBase newsletterSubscribeObject)
		{	
			AddParameter(cmd, pGuid(NewsletterSubscribeBase.Property_CompanyId, newsletterSubscribeObject.CompanyId));
			AddParameter(cmd, pNVarChar(NewsletterSubscribeBase.Property_UserName, 50, newsletterSubscribeObject.UserName));
			AddParameter(cmd, pBool(NewsletterSubscribeBase.Property_IsSubscribe, newsletterSubscribeObject.IsSubscribe));
			AddParameter(cmd, pNVarChar(NewsletterSubscribeBase.Property_LastUpdatedBy, 50, newsletterSubscribeObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(NewsletterSubscribeBase.Property_LastUpdatedDate, newsletterSubscribeObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts NewsletterSubscribe
        /// </summary>
        /// <param name="newsletterSubscribeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(NewsletterSubscribeBase newsletterSubscribeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTNEWSLETTERSUBSCRIBE);
	
				AddParameter(cmd, pInt32Out(NewsletterSubscribeBase.Property_Id));
				AddCommonParams(cmd, newsletterSubscribeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					newsletterSubscribeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					newsletterSubscribeObject.Id = (Int32)GetOutParameter(cmd, NewsletterSubscribeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(newsletterSubscribeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates NewsletterSubscribe
        /// </summary>
        /// <param name="newsletterSubscribeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(NewsletterSubscribeBase newsletterSubscribeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATENEWSLETTERSUBSCRIBE);
				
				AddParameter(cmd, pInt32(NewsletterSubscribeBase.Property_Id, newsletterSubscribeObject.Id));
				AddCommonParams(cmd, newsletterSubscribeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					newsletterSubscribeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(newsletterSubscribeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes NewsletterSubscribe
        /// </summary>
        /// <param name="Id">Id of the NewsletterSubscribe object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETENEWSLETTERSUBSCRIBE);	
				
				AddParameter(cmd, pInt32(NewsletterSubscribeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(NewsletterSubscribe), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves NewsletterSubscribe object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the NewsletterSubscribe object to retrieve</param>
        /// <returns>NewsletterSubscribe object, null if not found</returns>
		public NewsletterSubscribe Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETNEWSLETTERSUBSCRIBEBYID))
			{
				AddParameter( cmd, pInt32(NewsletterSubscribeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all NewsletterSubscribe objects 
        /// </summary>
        /// <returns>A list of NewsletterSubscribe objects</returns>
		public NewsletterSubscribeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLNEWSLETTERSUBSCRIBE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all NewsletterSubscribe objects by PageRequest
        /// </summary>
        /// <returns>A list of NewsletterSubscribe objects</returns>
		public NewsletterSubscribeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDNEWSLETTERSUBSCRIBE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				NewsletterSubscribeList _NewsletterSubscribeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _NewsletterSubscribeList;
			}
		}
		
		/// <summary>
        /// Retrieves all NewsletterSubscribe objects by query String
        /// </summary>
        /// <returns>A list of NewsletterSubscribe objects</returns>
		public NewsletterSubscribeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETNEWSLETTERSUBSCRIBEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get NewsletterSubscribe Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of NewsletterSubscribe
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETNEWSLETTERSUBSCRIBEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get NewsletterSubscribe Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of NewsletterSubscribe
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _NewsletterSubscribeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETNEWSLETTERSUBSCRIBEROWCOUNT))
			{
				SqlDataReader reader;
				_NewsletterSubscribeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _NewsletterSubscribeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills NewsletterSubscribe object
        /// </summary>
        /// <param name="newsletterSubscribeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(NewsletterSubscribeBase newsletterSubscribeObject, SqlDataReader reader, int start)
		{
			
				newsletterSubscribeObject.Id = reader.GetInt32( start + 0 );			
				newsletterSubscribeObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) newsletterSubscribeObject.UserName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) newsletterSubscribeObject.IsSubscribe = reader.GetBoolean( start + 3 );			
				if(!reader.IsDBNull(4)) newsletterSubscribeObject.LastUpdatedBy = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) newsletterSubscribeObject.LastUpdatedDate = reader.GetDateTime( start + 5 );			
			FillBaseObject(newsletterSubscribeObject, reader, (start + 6));

			
			newsletterSubscribeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills NewsletterSubscribe object
        /// </summary>
        /// <param name="newsletterSubscribeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(NewsletterSubscribeBase newsletterSubscribeObject, SqlDataReader reader)
		{
			FillObject(newsletterSubscribeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves NewsletterSubscribe object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>NewsletterSubscribe object</returns>
		private NewsletterSubscribe GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					NewsletterSubscribe newsletterSubscribeObject= new NewsletterSubscribe();
					FillObject(newsletterSubscribeObject, reader);
					return newsletterSubscribeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of NewsletterSubscribe objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of NewsletterSubscribe objects</returns>
		private NewsletterSubscribeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//NewsletterSubscribe list
			NewsletterSubscribeList list = new NewsletterSubscribeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					NewsletterSubscribe newsletterSubscribeObject = new NewsletterSubscribe();
					FillObject(newsletterSubscribeObject, reader);

					list.Add(newsletterSubscribeObject);
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
