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
	public partial class AgreementQuestionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTAGREEMENTQUESTION = "InsertAgreementQuestion";
		private const string UPDATEAGREEMENTQUESTION = "UpdateAgreementQuestion";
		private const string DELETEAGREEMENTQUESTION = "DeleteAgreementQuestion";
		private const string GETAGREEMENTQUESTIONBYID = "GetAgreementQuestionById";
		private const string GETALLAGREEMENTQUESTION = "GetAllAgreementQuestion";
		private const string GETPAGEDAGREEMENTQUESTION = "GetPagedAgreementQuestion";
		private const string GETAGREEMENTQUESTIONMAXIMUMID = "GetAgreementQuestionMaximumId";
		private const string GETAGREEMENTQUESTIONROWCOUNT = "GetAgreementQuestionRowCount";	
		private const string GETAGREEMENTQUESTIONBYQUERY = "GetAgreementQuestionByQuery";
		#endregion
		
		#region Constructors
		public AgreementQuestionDataAccess(ClientContext context) : base(context) { }
		public AgreementQuestionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="agreementQuestionObject"></param>
		private void AddCommonParams(SqlCommand cmd, AgreementQuestionBase agreementQuestionObject)
		{	
			AddParameter(cmd, pNVarChar(AgreementQuestionBase.Property_Title, agreementQuestionObject.Title));
			AddParameter(cmd, pBool(AgreementQuestionBase.Property_IsActive, agreementQuestionObject.IsActive));
			AddParameter(cmd, pNVarChar(AgreementQuestionBase.Property_SiteType, 50, agreementQuestionObject.SiteType));
			AddParameter(cmd, pInt32(AgreementQuestionBase.Property_QuestionId, agreementQuestionObject.QuestionId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AgreementQuestion
        /// </summary>
        /// <param name="agreementQuestionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AgreementQuestionBase agreementQuestionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTAGREEMENTQUESTION);
	
				AddParameter(cmd, pInt32Out(AgreementQuestionBase.Property_Id));
				AddCommonParams(cmd, agreementQuestionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					agreementQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					agreementQuestionObject.Id = (Int32)GetOutParameter(cmd, AgreementQuestionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(agreementQuestionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AgreementQuestion
        /// </summary>
        /// <param name="agreementQuestionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AgreementQuestionBase agreementQuestionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEAGREEMENTQUESTION);
				
				AddParameter(cmd, pInt32(AgreementQuestionBase.Property_Id, agreementQuestionObject.Id));
				AddCommonParams(cmd, agreementQuestionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					agreementQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(agreementQuestionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AgreementQuestion
        /// </summary>
        /// <param name="Id">Id of the AgreementQuestion object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEAGREEMENTQUESTION);	
				
				AddParameter(cmd, pInt32(AgreementQuestionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AgreementQuestion), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AgreementQuestion object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AgreementQuestion object to retrieve</param>
        /// <returns>AgreementQuestion object, null if not found</returns>
		public AgreementQuestion Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETAGREEMENTQUESTIONBYID))
			{
				AddParameter( cmd, pInt32(AgreementQuestionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AgreementQuestion objects 
        /// </summary>
        /// <returns>A list of AgreementQuestion objects</returns>
		public AgreementQuestionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLAGREEMENTQUESTION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AgreementQuestion objects by PageRequest
        /// </summary>
        /// <returns>A list of AgreementQuestion objects</returns>
		public AgreementQuestionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDAGREEMENTQUESTION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AgreementQuestionList _AgreementQuestionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AgreementQuestionList;
			}
		}
		
		/// <summary>
        /// Retrieves all AgreementQuestion objects by query String
        /// </summary>
        /// <returns>A list of AgreementQuestion objects</returns>
		public AgreementQuestionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETAGREEMENTQUESTIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AgreementQuestion Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AgreementQuestion
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAGREEMENTQUESTIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AgreementQuestion Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AgreementQuestion
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AgreementQuestionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAGREEMENTQUESTIONROWCOUNT))
			{
				SqlDataReader reader;
				_AgreementQuestionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AgreementQuestionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AgreementQuestion object
        /// </summary>
        /// <param name="agreementQuestionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AgreementQuestionBase agreementQuestionObject, SqlDataReader reader, int start)
		{
			
				agreementQuestionObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) agreementQuestionObject.Title = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) agreementQuestionObject.IsActive = reader.GetBoolean( start + 2 );
            if (!reader.IsDBNull(3)) agreementQuestionObject.SiteType = reader.GetString(start + 3);
            if (!reader.IsDBNull(4)) agreementQuestionObject.QuestionId = reader.GetInt32(start + 4);
            FillBaseObject(agreementQuestionObject, reader, (start + 5));

			
			agreementQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AgreementQuestion object
        /// </summary>
        /// <param name="agreementQuestionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AgreementQuestionBase agreementQuestionObject, SqlDataReader reader)
		{
			FillObject(agreementQuestionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AgreementQuestion object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AgreementQuestion object</returns>
		private AgreementQuestion GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AgreementQuestion agreementQuestionObject= new AgreementQuestion();
					FillObject(agreementQuestionObject, reader);
					return agreementQuestionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AgreementQuestion objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AgreementQuestion objects</returns>
		private AgreementQuestionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AgreementQuestion list
			AgreementQuestionList list = new AgreementQuestionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AgreementQuestion agreementQuestionObject = new AgreementQuestion();
					FillObject(agreementQuestionObject, reader);

					list.Add(agreementQuestionObject);
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
