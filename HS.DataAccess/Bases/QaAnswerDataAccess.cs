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
	public partial class QaAnswerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTQAANSWER = "InsertQaAnswer";
		private const string UPDATEQAANSWER = "UpdateQaAnswer";
		private const string DELETEQAANSWER = "DeleteQaAnswer";
		private const string GETQAANSWERBYID = "GetQaAnswerById";
		private const string GETALLQAANSWER = "GetAllQaAnswer";
		private const string GETPAGEDQAANSWER = "GetPagedQaAnswer";
		private const string GETQAANSWERMAXIMUMID = "GetQaAnswerMaximumId";
		private const string GETQAANSWERROWCOUNT = "GetQaAnswerRowCount";	
		private const string GETQAANSWERBYQUERY = "GetQaAnswerByQuery";
		#endregion
		
		#region Constructors
		public QaAnswerDataAccess(ClientContext context) : base(context) { }
		public QaAnswerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="qaAnswerObject"></param>
		private void AddCommonParams(SqlCommand cmd, QaAnswerBase qaAnswerObject)
		{	
			AddParameter(cmd, pGuid(QaAnswerBase.Property_CompanyId, qaAnswerObject.CompanyId));
			AddParameter(cmd, pGuid(QaAnswerBase.Property_CustomerId, qaAnswerObject.CustomerId));
			AddParameter(cmd, pInt32(QaAnswerBase.Property_QuestionId, qaAnswerObject.QuestionId));
			AddParameter(cmd, pNVarChar(QaAnswerBase.Property_Answer, 50, qaAnswerObject.Answer));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts QaAnswer
        /// </summary>
        /// <param name="qaAnswerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(QaAnswerBase qaAnswerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTQAANSWER);
	
				AddParameter(cmd, pInt32Out(QaAnswerBase.Property_Id));
				AddCommonParams(cmd, qaAnswerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					qaAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					qaAnswerObject.Id = (Int32)GetOutParameter(cmd, QaAnswerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(qaAnswerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates QaAnswer
        /// </summary>
        /// <param name="qaAnswerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(QaAnswerBase qaAnswerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEQAANSWER);
				
				AddParameter(cmd, pInt32(QaAnswerBase.Property_Id, qaAnswerObject.Id));
				AddCommonParams(cmd, qaAnswerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					qaAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(qaAnswerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes QaAnswer
        /// </summary>
        /// <param name="Id">Id of the QaAnswer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEQAANSWER);	
				
				AddParameter(cmd, pInt32(QaAnswerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(QaAnswer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves QaAnswer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the QaAnswer object to retrieve</param>
        /// <returns>QaAnswer object, null if not found</returns>
		public QaAnswer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETQAANSWERBYID))
			{
				AddParameter( cmd, pInt32(QaAnswerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all QaAnswer objects 
        /// </summary>
        /// <returns>A list of QaAnswer objects</returns>
		public QaAnswerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLQAANSWER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all QaAnswer objects by PageRequest
        /// </summary>
        /// <returns>A list of QaAnswer objects</returns>
		public QaAnswerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDQAANSWER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				QaAnswerList _QaAnswerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _QaAnswerList;
			}
		}
		
		/// <summary>
        /// Retrieves all QaAnswer objects by query String
        /// </summary>
        /// <returns>A list of QaAnswer objects</returns>
		public QaAnswerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETQAANSWERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get QaAnswer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of QaAnswer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQAANSWERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get QaAnswer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of QaAnswer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _QaAnswerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQAANSWERROWCOUNT))
			{
				SqlDataReader reader;
				_QaAnswerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _QaAnswerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills QaAnswer object
        /// </summary>
        /// <param name="qaAnswerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(QaAnswerBase qaAnswerObject, SqlDataReader reader, int start)
		{
			
				qaAnswerObject.Id = reader.GetInt32( start + 0 );			
				qaAnswerObject.CompanyId = reader.GetGuid( start + 1 );			
				qaAnswerObject.CustomerId = reader.GetGuid( start + 2 );			
				qaAnswerObject.QuestionId = reader.GetInt32( start + 3 );			
				qaAnswerObject.Answer = reader.GetString( start + 4 );			
			FillBaseObject(qaAnswerObject, reader, (start + 5));

			
			qaAnswerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills QaAnswer object
        /// </summary>
        /// <param name="qaAnswerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(QaAnswerBase qaAnswerObject, SqlDataReader reader)
		{
			FillObject(qaAnswerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves QaAnswer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>QaAnswer object</returns>
		private QaAnswer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					QaAnswer qaAnswerObject= new QaAnswer();
					FillObject(qaAnswerObject, reader);
					return qaAnswerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of QaAnswer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of QaAnswer objects</returns>
		private QaAnswerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//QaAnswer list
			QaAnswerList list = new QaAnswerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					QaAnswer qaAnswerObject = new QaAnswer();
					FillObject(qaAnswerObject, reader);

					list.Add(qaAnswerObject);
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
