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
	public partial class CustomSurveyUserAnswersDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMSURVEYUSERANSWERS = "InsertCustomSurveyUserAnswers";
		private const string UPDATECUSTOMSURVEYUSERANSWERS = "UpdateCustomSurveyUserAnswers";
		private const string DELETECUSTOMSURVEYUSERANSWERS = "DeleteCustomSurveyUserAnswers";
		private const string GETCUSTOMSURVEYUSERANSWERSBYID = "GetCustomSurveyUserAnswersById";
		private const string GETALLCUSTOMSURVEYUSERANSWERS = "GetAllCustomSurveyUserAnswers";
		private const string GETPAGEDCUSTOMSURVEYUSERANSWERS = "GetPagedCustomSurveyUserAnswers";
		private const string GETCUSTOMSURVEYUSERANSWERSMAXIMUMID = "GetCustomSurveyUserAnswersMaximumId";
		private const string GETCUSTOMSURVEYUSERANSWERSROWCOUNT = "GetCustomSurveyUserAnswersRowCount";	
		private const string GETCUSTOMSURVEYUSERANSWERSBYQUERY = "GetCustomSurveyUserAnswersByQuery";
		#endregion
		
		#region Constructors
		public CustomSurveyUserAnswersDataAccess(ClientContext context) : base(context) { }
		public CustomSurveyUserAnswersDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customSurveyUserAnswersObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomSurveyUserAnswersBase customSurveyUserAnswersObject)
		{	
			AddParameter(cmd, pGuid(CustomSurveyUserAnswersBase.Property_SurveyId, customSurveyUserAnswersObject.SurveyId));
			AddParameter(cmd, pGuid(CustomSurveyUserAnswersBase.Property_QuestionId, customSurveyUserAnswersObject.QuestionId));
			AddParameter(cmd, pGuid(CustomSurveyUserAnswersBase.Property_AnswerId, customSurveyUserAnswersObject.AnswerId));
			AddParameter(cmd, pGuid(CustomSurveyUserAnswersBase.Property_UserId, customSurveyUserAnswersObject.UserId));
			AddParameter(cmd, pNVarChar(CustomSurveyUserAnswersBase.Property_Answer, customSurveyUserAnswersObject.Answer));
			AddParameter(cmd, pDateTime(CustomSurveyUserAnswersBase.Property_CreatedDate, customSurveyUserAnswersObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomSurveyUserAnswersBase.Property_SurveyUserId, customSurveyUserAnswersObject.SurveyUserId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomSurveyUserAnswers
        /// </summary>
        /// <param name="customSurveyUserAnswersObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomSurveyUserAnswersBase customSurveyUserAnswersObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMSURVEYUSERANSWERS);
	
				AddParameter(cmd, pInt32Out(CustomSurveyUserAnswersBase.Property_Id));
				AddCommonParams(cmd, customSurveyUserAnswersObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customSurveyUserAnswersObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customSurveyUserAnswersObject.Id = (Int32)GetOutParameter(cmd, CustomSurveyUserAnswersBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customSurveyUserAnswersObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomSurveyUserAnswers
        /// </summary>
        /// <param name="customSurveyUserAnswersObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomSurveyUserAnswersBase customSurveyUserAnswersObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMSURVEYUSERANSWERS);
				
				AddParameter(cmd, pInt32(CustomSurveyUserAnswersBase.Property_Id, customSurveyUserAnswersObject.Id));
				AddCommonParams(cmd, customSurveyUserAnswersObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customSurveyUserAnswersObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customSurveyUserAnswersObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomSurveyUserAnswers
        /// </summary>
        /// <param name="Id">Id of the CustomSurveyUserAnswers object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMSURVEYUSERANSWERS);	
				
				AddParameter(cmd, pInt32(CustomSurveyUserAnswersBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomSurveyUserAnswers), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomSurveyUserAnswers object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomSurveyUserAnswers object to retrieve</param>
        /// <returns>CustomSurveyUserAnswers object, null if not found</returns>
		public CustomSurveyUserAnswers Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYUSERANSWERSBYID))
			{
				AddParameter( cmd, pInt32(CustomSurveyUserAnswersBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomSurveyUserAnswers objects 
        /// </summary>
        /// <returns>A list of CustomSurveyUserAnswers objects</returns>
		public CustomSurveyUserAnswersList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMSURVEYUSERANSWERS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomSurveyUserAnswers objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomSurveyUserAnswers objects</returns>
		public CustomSurveyUserAnswersList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMSURVEYUSERANSWERS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomSurveyUserAnswersList _CustomSurveyUserAnswersList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomSurveyUserAnswersList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomSurveyUserAnswers objects by query String
        /// </summary>
        /// <returns>A list of CustomSurveyUserAnswers objects</returns>
		public CustomSurveyUserAnswersList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYUSERANSWERSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomSurveyUserAnswers Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomSurveyUserAnswers
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYUSERANSWERSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomSurveyUserAnswers Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomSurveyUserAnswers
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomSurveyUserAnswersRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYUSERANSWERSROWCOUNT))
			{
				SqlDataReader reader;
				_CustomSurveyUserAnswersRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomSurveyUserAnswersRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomSurveyUserAnswers object
        /// </summary>
        /// <param name="customSurveyUserAnswersObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomSurveyUserAnswersBase customSurveyUserAnswersObject, SqlDataReader reader, int start)
		{
			
				customSurveyUserAnswersObject.Id = reader.GetInt32( start + 0 );			
				customSurveyUserAnswersObject.SurveyId = reader.GetGuid( start + 1 );			
				customSurveyUserAnswersObject.QuestionId = reader.GetGuid( start + 2 );			
				customSurveyUserAnswersObject.AnswerId = reader.GetGuid( start + 3 );			
				customSurveyUserAnswersObject.UserId = reader.GetGuid( start + 4 );			
				customSurveyUserAnswersObject.Answer = reader.GetString( start + 5 );			
				customSurveyUserAnswersObject.CreatedDate = reader.GetDateTime( start + 6 );			
				customSurveyUserAnswersObject.SurveyUserId = reader.GetGuid( start + 7 );			
			FillBaseObject(customSurveyUserAnswersObject, reader, (start + 8));

			
			customSurveyUserAnswersObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomSurveyUserAnswers object
        /// </summary>
        /// <param name="customSurveyUserAnswersObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomSurveyUserAnswersBase customSurveyUserAnswersObject, SqlDataReader reader)
		{
			FillObject(customSurveyUserAnswersObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomSurveyUserAnswers object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomSurveyUserAnswers object</returns>
		private CustomSurveyUserAnswers GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomSurveyUserAnswers customSurveyUserAnswersObject= new CustomSurveyUserAnswers();
					FillObject(customSurveyUserAnswersObject, reader);
					return customSurveyUserAnswersObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomSurveyUserAnswers objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomSurveyUserAnswers objects</returns>
		private CustomSurveyUserAnswersList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomSurveyUserAnswers list
			CustomSurveyUserAnswersList list = new CustomSurveyUserAnswersList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomSurveyUserAnswers customSurveyUserAnswersObject = new CustomSurveyUserAnswers();
					FillObject(customSurveyUserAnswersObject, reader);

					list.Add(customSurveyUserAnswersObject);
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
