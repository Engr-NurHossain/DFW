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
	public partial class AgreementAnswerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTAGREEMENTANSWER = "InsertAgreementAnswer";
		private const string UPDATEAGREEMENTANSWER = "UpdateAgreementAnswer";
		private const string DELETEAGREEMENTANSWER = "DeleteAgreementAnswer";
		private const string GETAGREEMENTANSWERBYID = "GetAgreementAnswerById";
		private const string GETALLAGREEMENTANSWER = "GetAllAgreementAnswer";
		private const string GETPAGEDAGREEMENTANSWER = "GetPagedAgreementAnswer";
		private const string GETAGREEMENTANSWERMAXIMUMID = "GetAgreementAnswerMaximumId";
		private const string GETAGREEMENTANSWERROWCOUNT = "GetAgreementAnswerRowCount";	
		private const string GETAGREEMENTANSWERBYQUERY = "GetAgreementAnswerByQuery";
		#endregion
		
		#region Constructors
		public AgreementAnswerDataAccess(ClientContext context) : base(context) { }
		public AgreementAnswerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="agreementAnswerObject"></param>
		private void AddCommonParams(SqlCommand cmd, AgreementAnswerBase agreementAnswerObject)
		{	
			AddParameter(cmd, pInt32(AgreementAnswerBase.Property_QuestionId, agreementAnswerObject.QuestionId));
			AddParameter(cmd, pGuid(AgreementAnswerBase.Property_CustomerId, agreementAnswerObject.CustomerId));
			AddParameter(cmd, pNVarChar(AgreementAnswerBase.Property_Answer, 50, agreementAnswerObject.Answer));
			AddParameter(cmd, pDateTime(AgreementAnswerBase.Property_AnswerDate, agreementAnswerObject.AnswerDate));
			AddParameter(cmd, pNVarChar(AgreementAnswerBase.Property_Ip, 50, agreementAnswerObject.Ip));
			AddParameter(cmd, pNVarChar(AgreementAnswerBase.Property_UserAgent, agreementAnswerObject.UserAgent));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AgreementAnswer
        /// </summary>
        /// <param name="agreementAnswerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AgreementAnswerBase agreementAnswerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTAGREEMENTANSWER);
	
				AddParameter(cmd, pInt32Out(AgreementAnswerBase.Property_Id));
				AddCommonParams(cmd, agreementAnswerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					agreementAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					agreementAnswerObject.Id = (Int32)GetOutParameter(cmd, AgreementAnswerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(agreementAnswerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AgreementAnswer
        /// </summary>
        /// <param name="agreementAnswerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AgreementAnswerBase agreementAnswerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEAGREEMENTANSWER);
				
				AddParameter(cmd, pInt32(AgreementAnswerBase.Property_Id, agreementAnswerObject.Id));
				AddCommonParams(cmd, agreementAnswerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					agreementAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(agreementAnswerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AgreementAnswer
        /// </summary>
        /// <param name="Id">Id of the AgreementAnswer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEAGREEMENTANSWER);	
				
				AddParameter(cmd, pInt32(AgreementAnswerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AgreementAnswer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AgreementAnswer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AgreementAnswer object to retrieve</param>
        /// <returns>AgreementAnswer object, null if not found</returns>
		public AgreementAnswer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETAGREEMENTANSWERBYID))
			{
				AddParameter( cmd, pInt32(AgreementAnswerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AgreementAnswer objects 
        /// </summary>
        /// <returns>A list of AgreementAnswer objects</returns>
		public AgreementAnswerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLAGREEMENTANSWER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AgreementAnswer objects by PageRequest
        /// </summary>
        /// <returns>A list of AgreementAnswer objects</returns>
		public AgreementAnswerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDAGREEMENTANSWER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AgreementAnswerList _AgreementAnswerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AgreementAnswerList;
			}
		}
		
		/// <summary>
        /// Retrieves all AgreementAnswer objects by query String
        /// </summary>
        /// <returns>A list of AgreementAnswer objects</returns>
		public AgreementAnswerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETAGREEMENTANSWERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AgreementAnswer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AgreementAnswer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAGREEMENTANSWERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AgreementAnswer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AgreementAnswer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AgreementAnswerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAGREEMENTANSWERROWCOUNT))
			{
				SqlDataReader reader;
				_AgreementAnswerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AgreementAnswerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AgreementAnswer object
        /// </summary>
        /// <param name="agreementAnswerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AgreementAnswerBase agreementAnswerObject, SqlDataReader reader, int start)
		{
			
				agreementAnswerObject.Id = reader.GetInt32( start + 0 );			
				agreementAnswerObject.QuestionId = reader.GetInt32( start + 1 );			
				agreementAnswerObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) agreementAnswerObject.Answer = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) agreementAnswerObject.AnswerDate = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) agreementAnswerObject.Ip = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) agreementAnswerObject.UserAgent = reader.GetString( start + 6 );			
			FillBaseObject(agreementAnswerObject, reader, (start + 7));

			
			agreementAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AgreementAnswer object
        /// </summary>
        /// <param name="agreementAnswerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AgreementAnswerBase agreementAnswerObject, SqlDataReader reader)
		{
			FillObject(agreementAnswerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AgreementAnswer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AgreementAnswer object</returns>
		private AgreementAnswer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AgreementAnswer agreementAnswerObject= new AgreementAnswer();
					FillObject(agreementAnswerObject, reader);
					return agreementAnswerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AgreementAnswer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AgreementAnswer objects</returns>
		private AgreementAnswerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AgreementAnswer list
			AgreementAnswerList list = new AgreementAnswerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AgreementAnswer agreementAnswerObject = new AgreementAnswer();
					FillObject(agreementAnswerObject, reader);

					list.Add(agreementAnswerObject);
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
