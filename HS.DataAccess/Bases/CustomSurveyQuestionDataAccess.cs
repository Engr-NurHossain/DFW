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
	public partial class CustomSurveyQuestionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMSURVEYQUESTION = "InsertCustomSurveyQuestion";
		private const string UPDATECUSTOMSURVEYQUESTION = "UpdateCustomSurveyQuestion";
		private const string DELETECUSTOMSURVEYQUESTION = "DeleteCustomSurveyQuestion";
		private const string GETCUSTOMSURVEYQUESTIONBYID = "GetCustomSurveyQuestionById";
		private const string GETALLCUSTOMSURVEYQUESTION = "GetAllCustomSurveyQuestion";
		private const string GETPAGEDCUSTOMSURVEYQUESTION = "GetPagedCustomSurveyQuestion";
		private const string GETCUSTOMSURVEYQUESTIONMAXIMUMID = "GetCustomSurveyQuestionMaximumId";
		private const string GETCUSTOMSURVEYQUESTIONROWCOUNT = "GetCustomSurveyQuestionRowCount";	
		private const string GETCUSTOMSURVEYQUESTIONBYQUERY = "GetCustomSurveyQuestionByQuery";
		#endregion
		
		#region Constructors
		public CustomSurveyQuestionDataAccess(ClientContext context) : base(context) { }
		public CustomSurveyQuestionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customSurveyQuestionObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomSurveyQuestionBase customSurveyQuestionObject)
		{	
			AddParameter(cmd, pGuid(CustomSurveyQuestionBase.Property_SurveyId, customSurveyQuestionObject.SurveyId));
			AddParameter(cmd, pGuid(CustomSurveyQuestionBase.Property_QuestionId, customSurveyQuestionObject.QuestionId));
			AddParameter(cmd, pNVarChar(CustomSurveyQuestionBase.Property_Question, customSurveyQuestionObject.Question));
			AddParameter(cmd, pNVarChar(CustomSurveyQuestionBase.Property_QuestionType, 50, customSurveyQuestionObject.QuestionType));
			AddParameter(cmd, pGuid(CustomSurveyQuestionBase.Property_CreatedBy, customSurveyQuestionObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomSurveyQuestionBase.Property_CreatedDate, customSurveyQuestionObject.CreatedDate));
			AddParameter(cmd, pInt32(CustomSurveyQuestionBase.Property_OrderBy, customSurveyQuestionObject.OrderBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomSurveyQuestion
        /// </summary>
        /// <param name="customSurveyQuestionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomSurveyQuestionBase customSurveyQuestionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMSURVEYQUESTION);
	
				AddParameter(cmd, pInt32Out(CustomSurveyQuestionBase.Property_Id));
				AddCommonParams(cmd, customSurveyQuestionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customSurveyQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customSurveyQuestionObject.Id = (Int32)GetOutParameter(cmd, CustomSurveyQuestionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customSurveyQuestionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomSurveyQuestion
        /// </summary>
        /// <param name="customSurveyQuestionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomSurveyQuestionBase customSurveyQuestionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMSURVEYQUESTION);
				
				AddParameter(cmd, pInt32(CustomSurveyQuestionBase.Property_Id, customSurveyQuestionObject.Id));
				AddCommonParams(cmd, customSurveyQuestionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customSurveyQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customSurveyQuestionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomSurveyQuestion
        /// </summary>
        /// <param name="Id">Id of the CustomSurveyQuestion object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMSURVEYQUESTION);	
				
				AddParameter(cmd, pInt32(CustomSurveyQuestionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomSurveyQuestion), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomSurveyQuestion object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomSurveyQuestion object to retrieve</param>
        /// <returns>CustomSurveyQuestion object, null if not found</returns>
		public CustomSurveyQuestion Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYQUESTIONBYID))
			{
				AddParameter( cmd, pInt32(CustomSurveyQuestionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomSurveyQuestion objects 
        /// </summary>
        /// <returns>A list of CustomSurveyQuestion objects</returns>
		public CustomSurveyQuestionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMSURVEYQUESTION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomSurveyQuestion objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomSurveyQuestion objects</returns>
		public CustomSurveyQuestionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMSURVEYQUESTION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomSurveyQuestionList _CustomSurveyQuestionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomSurveyQuestionList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomSurveyQuestion objects by query String
        /// </summary>
        /// <returns>A list of CustomSurveyQuestion objects</returns>
		public CustomSurveyQuestionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYQUESTIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomSurveyQuestion Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomSurveyQuestion
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYQUESTIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomSurveyQuestion Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomSurveyQuestion
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomSurveyQuestionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYQUESTIONROWCOUNT))
			{
				SqlDataReader reader;
				_CustomSurveyQuestionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomSurveyQuestionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomSurveyQuestion object
        /// </summary>
        /// <param name="customSurveyQuestionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomSurveyQuestionBase customSurveyQuestionObject, SqlDataReader reader, int start)
		{
			
				customSurveyQuestionObject.Id = reader.GetInt32( start + 0 );			
				customSurveyQuestionObject.SurveyId = reader.GetGuid( start + 1 );			
				customSurveyQuestionObject.QuestionId = reader.GetGuid( start + 2 );			
				customSurveyQuestionObject.Question = reader.GetString( start + 3 );			
				customSurveyQuestionObject.QuestionType = reader.GetString( start + 4 );			
				customSurveyQuestionObject.CreatedBy = reader.GetGuid( start + 5 );			
				customSurveyQuestionObject.CreatedDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) customSurveyQuestionObject.OrderBy = reader.GetInt32( start + 7 );			
			FillBaseObject(customSurveyQuestionObject, reader, (start + 8));

			
			customSurveyQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomSurveyQuestion object
        /// </summary>
        /// <param name="customSurveyQuestionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomSurveyQuestionBase customSurveyQuestionObject, SqlDataReader reader)
		{
			FillObject(customSurveyQuestionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomSurveyQuestion object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomSurveyQuestion object</returns>
		private CustomSurveyQuestion GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomSurveyQuestion customSurveyQuestionObject= new CustomSurveyQuestion();
					FillObject(customSurveyQuestionObject, reader);
					return customSurveyQuestionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomSurveyQuestion objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomSurveyQuestion objects</returns>
		private CustomSurveyQuestionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomSurveyQuestion list
			CustomSurveyQuestionList list = new CustomSurveyQuestionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomSurveyQuestion customSurveyQuestionObject = new CustomSurveyQuestion();
					FillObject(customSurveyQuestionObject, reader);

					list.Add(customSurveyQuestionObject);
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
