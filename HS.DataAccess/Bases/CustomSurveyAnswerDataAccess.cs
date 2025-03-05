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
	public partial class CustomSurveyAnswerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMSURVEYANSWER = "InsertCustomSurveyAnswer";
		private const string UPDATECUSTOMSURVEYANSWER = "UpdateCustomSurveyAnswer";
		private const string DELETECUSTOMSURVEYANSWER = "DeleteCustomSurveyAnswer";
		private const string GETCUSTOMSURVEYANSWERBYID = "GetCustomSurveyAnswerById";
		private const string GETALLCUSTOMSURVEYANSWER = "GetAllCustomSurveyAnswer";
		private const string GETPAGEDCUSTOMSURVEYANSWER = "GetPagedCustomSurveyAnswer";
		private const string GETCUSTOMSURVEYANSWERMAXIMUMID = "GetCustomSurveyAnswerMaximumId";
		private const string GETCUSTOMSURVEYANSWERROWCOUNT = "GetCustomSurveyAnswerRowCount";	
		private const string GETCUSTOMSURVEYANSWERBYQUERY = "GetCustomSurveyAnswerByQuery";
		#endregion
		
		#region Constructors
		public CustomSurveyAnswerDataAccess(ClientContext context) : base(context) { }
		public CustomSurveyAnswerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customSurveyAnswerObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomSurveyAnswerBase customSurveyAnswerObject)
		{	
			AddParameter(cmd, pGuid(CustomSurveyAnswerBase.Property_QuestionId, customSurveyAnswerObject.QuestionId));
			AddParameter(cmd, pGuid(CustomSurveyAnswerBase.Property_AnswerId, customSurveyAnswerObject.AnswerId));
			AddParameter(cmd, pNVarChar(CustomSurveyAnswerBase.Property_Answer, customSurveyAnswerObject.Answer));
			AddParameter(cmd, pGuid(CustomSurveyAnswerBase.Property_CreatedBy, customSurveyAnswerObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomSurveyAnswerBase.Property_CreatedDate, customSurveyAnswerObject.CreatedDate));
			AddParameter(cmd, pInt32(CustomSurveyAnswerBase.Property_OrderBy, customSurveyAnswerObject.OrderBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomSurveyAnswer
        /// </summary>
        /// <param name="customSurveyAnswerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomSurveyAnswerBase customSurveyAnswerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMSURVEYANSWER);
	
				AddParameter(cmd, pInt32Out(CustomSurveyAnswerBase.Property_Id));
				AddCommonParams(cmd, customSurveyAnswerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customSurveyAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customSurveyAnswerObject.Id = (Int32)GetOutParameter(cmd, CustomSurveyAnswerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customSurveyAnswerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomSurveyAnswer
        /// </summary>
        /// <param name="customSurveyAnswerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomSurveyAnswerBase customSurveyAnswerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMSURVEYANSWER);
				
				AddParameter(cmd, pInt32(CustomSurveyAnswerBase.Property_Id, customSurveyAnswerObject.Id));
				AddCommonParams(cmd, customSurveyAnswerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customSurveyAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customSurveyAnswerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomSurveyAnswer
        /// </summary>
        /// <param name="Id">Id of the CustomSurveyAnswer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMSURVEYANSWER);	
				
				AddParameter(cmd, pInt32(CustomSurveyAnswerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomSurveyAnswer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomSurveyAnswer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomSurveyAnswer object to retrieve</param>
        /// <returns>CustomSurveyAnswer object, null if not found</returns>
		public CustomSurveyAnswer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYANSWERBYID))
			{
				AddParameter( cmd, pInt32(CustomSurveyAnswerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomSurveyAnswer objects 
        /// </summary>
        /// <returns>A list of CustomSurveyAnswer objects</returns>
		public CustomSurveyAnswerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMSURVEYANSWER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomSurveyAnswer objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomSurveyAnswer objects</returns>
		public CustomSurveyAnswerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMSURVEYANSWER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomSurveyAnswerList _CustomSurveyAnswerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomSurveyAnswerList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomSurveyAnswer objects by query String
        /// </summary>
        /// <returns>A list of CustomSurveyAnswer objects</returns>
		public CustomSurveyAnswerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYANSWERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomSurveyAnswer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomSurveyAnswer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYANSWERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomSurveyAnswer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomSurveyAnswer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomSurveyAnswerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYANSWERROWCOUNT))
			{
				SqlDataReader reader;
				_CustomSurveyAnswerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomSurveyAnswerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomSurveyAnswer object
        /// </summary>
        /// <param name="customSurveyAnswerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomSurveyAnswerBase customSurveyAnswerObject, SqlDataReader reader, int start)
		{
			
				customSurveyAnswerObject.Id = reader.GetInt32( start + 0 );			
				customSurveyAnswerObject.QuestionId = reader.GetGuid( start + 1 );			
				customSurveyAnswerObject.AnswerId = reader.GetGuid( start + 2 );			
				customSurveyAnswerObject.Answer = reader.GetString( start + 3 );			
				customSurveyAnswerObject.CreatedBy = reader.GetGuid( start + 4 );			
				customSurveyAnswerObject.CreatedDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) customSurveyAnswerObject.OrderBy = reader.GetInt32( start + 6 );			
			FillBaseObject(customSurveyAnswerObject, reader, (start + 7));

			
			customSurveyAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomSurveyAnswer object
        /// </summary>
        /// <param name="customSurveyAnswerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomSurveyAnswerBase customSurveyAnswerObject, SqlDataReader reader)
		{
			FillObject(customSurveyAnswerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomSurveyAnswer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomSurveyAnswer object</returns>
		private CustomSurveyAnswer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomSurveyAnswer customSurveyAnswerObject= new CustomSurveyAnswer();
					FillObject(customSurveyAnswerObject, reader);
					return customSurveyAnswerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomSurveyAnswer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomSurveyAnswer objects</returns>
		private CustomSurveyAnswerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomSurveyAnswer list
			CustomSurveyAnswerList list = new CustomSurveyAnswerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomSurveyAnswer customSurveyAnswerObject = new CustomSurveyAnswer();
					FillObject(customSurveyAnswerObject, reader);

					list.Add(customSurveyAnswerObject);
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
