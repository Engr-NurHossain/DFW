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
	public partial class ResturantReviewDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTURANTREVIEW = "InsertResturantReview";
		private const string UPDATERESTURANTREVIEW = "UpdateResturantReview";
		private const string DELETERESTURANTREVIEW = "DeleteResturantReview";
		private const string GETRESTURANTREVIEWBYID = "GetResturantReviewById";
		private const string GETALLRESTURANTREVIEW = "GetAllResturantReview";
		private const string GETPAGEDRESTURANTREVIEW = "GetPagedResturantReview";
		private const string GETRESTURANTREVIEWMAXIMUMID = "GetResturantReviewMaximumId";
		private const string GETRESTURANTREVIEWROWCOUNT = "GetResturantReviewRowCount";	
		private const string GETRESTURANTREVIEWBYQUERY = "GetResturantReviewByQuery";
		#endregion
		
		#region Constructors
		public ResturantReviewDataAccess(ClientContext context) : base(context) { }
		public ResturantReviewDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="resturantReviewObject"></param>
		private void AddCommonParams(SqlCommand cmd, ResturantReviewBase resturantReviewObject)
		{	
			AddParameter(cmd, pGuid(ResturantReviewBase.Property_CompanyId, resturantReviewObject.CompanyId));
			AddParameter(cmd, pNVarChar(ResturantReviewBase.Property_Name, 250, resturantReviewObject.Name));
			AddParameter(cmd, pNVarChar(ResturantReviewBase.Property_Email, 250, resturantReviewObject.Email));
			AddParameter(cmd, pNVarChar(ResturantReviewBase.Property_Comments, resturantReviewObject.Comments));
			AddParameter(cmd, pDouble(ResturantReviewBase.Property_Rating, resturantReviewObject.Rating));
			AddParameter(cmd, pGuid(ResturantReviewBase.Property_CreatedBy, resturantReviewObject.CreatedBy));
			AddParameter(cmd, pDateTime(ResturantReviewBase.Property_CreatedDate, resturantReviewObject.CreatedDate));
			AddParameter(cmd, pBool(ResturantReviewBase.Property_IsActive, resturantReviewObject.IsActive));
			AddParameter(cmd, pNVarChar(ResturantReviewBase.Property_Reply, resturantReviewObject.Reply));
			AddParameter(cmd, pNVarChar(ResturantReviewBase.Property_ReviewFor, 250, resturantReviewObject.ReviewFor));
			AddParameter(cmd, pGuid(ResturantReviewBase.Property_ReplyBy, resturantReviewObject.ReplyBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ResturantReview
        /// </summary>
        /// <param name="resturantReviewObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ResturantReviewBase resturantReviewObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTURANTREVIEW);
	
				AddParameter(cmd, pInt32Out(ResturantReviewBase.Property_Id));
				AddCommonParams(cmd, resturantReviewObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					resturantReviewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					resturantReviewObject.Id = (Int32)GetOutParameter(cmd, ResturantReviewBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(resturantReviewObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ResturantReview
        /// </summary>
        /// <param name="resturantReviewObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ResturantReviewBase resturantReviewObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTURANTREVIEW);
				
				AddParameter(cmd, pInt32(ResturantReviewBase.Property_Id, resturantReviewObject.Id));
				AddCommonParams(cmd, resturantReviewObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					resturantReviewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(resturantReviewObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ResturantReview
        /// </summary>
        /// <param name="Id">Id of the ResturantReview object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTURANTREVIEW);	
				
				AddParameter(cmd, pInt32(ResturantReviewBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ResturantReview), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ResturantReview object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ResturantReview object to retrieve</param>
        /// <returns>ResturantReview object, null if not found</returns>
		public ResturantReview Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTREVIEWBYID))
			{
				AddParameter( cmd, pInt32(ResturantReviewBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ResturantReview objects 
        /// </summary>
        /// <returns>A list of ResturantReview objects</returns>
		public ResturantReviewList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTURANTREVIEW))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ResturantReview objects by PageRequest
        /// </summary>
        /// <returns>A list of ResturantReview objects</returns>
		public ResturantReviewList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTURANTREVIEW))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ResturantReviewList _ResturantReviewList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ResturantReviewList;
			}
		}
		
		/// <summary>
        /// Retrieves all ResturantReview objects by query String
        /// </summary>
        /// <returns>A list of ResturantReview objects</returns>
		public ResturantReviewList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTREVIEWBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ResturantReview Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ResturantReview
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTREVIEWMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ResturantReview Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ResturantReview
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ResturantReviewRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTREVIEWROWCOUNT))
			{
				SqlDataReader reader;
				_ResturantReviewRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ResturantReviewRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ResturantReview object
        /// </summary>
        /// <param name="resturantReviewObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ResturantReviewBase resturantReviewObject, SqlDataReader reader, int start)
		{
			
				resturantReviewObject.Id = reader.GetInt32( start + 0 );			
				resturantReviewObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) resturantReviewObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) resturantReviewObject.Email = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) resturantReviewObject.Comments = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) resturantReviewObject.Rating = reader.GetDouble( start + 5 );			
				resturantReviewObject.CreatedBy = reader.GetGuid( start + 6 );			
				resturantReviewObject.CreatedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) resturantReviewObject.IsActive = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) resturantReviewObject.Reply = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) resturantReviewObject.ReviewFor = reader.GetString( start + 10 );			
				resturantReviewObject.ReplyBy = reader.GetGuid( start + 11 );			
			FillBaseObject(resturantReviewObject, reader, (start + 12));

			
			resturantReviewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ResturantReview object
        /// </summary>
        /// <param name="resturantReviewObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ResturantReviewBase resturantReviewObject, SqlDataReader reader)
		{
			FillObject(resturantReviewObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ResturantReview object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ResturantReview object</returns>
		private ResturantReview GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ResturantReview resturantReviewObject= new ResturantReview();
					FillObject(resturantReviewObject, reader);
					return resturantReviewObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ResturantReview objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ResturantReview objects</returns>
		private ResturantReviewList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ResturantReview list
			ResturantReviewList list = new ResturantReviewList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ResturantReview resturantReviewObject = new ResturantReview();
					FillObject(resturantReviewObject, reader);

					list.Add(resturantReviewObject);
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
