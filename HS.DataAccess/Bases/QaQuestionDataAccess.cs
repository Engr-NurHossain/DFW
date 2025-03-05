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
	public partial class QaQuestionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTQAQUESTION = "InsertQaQuestion";
		private const string UPDATEQAQUESTION = "UpdateQaQuestion";
		private const string DELETEQAQUESTION = "DeleteQaQuestion";
		private const string GETQAQUESTIONBYID = "GetQaQuestionById";
		private const string GETALLQAQUESTION = "GetAllQaQuestion";
		private const string GETPAGEDQAQUESTION = "GetPagedQaQuestion";
		private const string GETQAQUESTIONMAXIMUMID = "GetQaQuestionMaximumId";
		private const string GETQAQUESTIONROWCOUNT = "GetQaQuestionRowCount";	
		private const string GETQAQUESTIONBYQUERY = "GetQaQuestionByQuery";
		#endregion
		
		#region Constructors
		public QaQuestionDataAccess(ClientContext context) : base(context) { }
		public QaQuestionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="qaQuestionObject"></param>
		private void AddCommonParams(SqlCommand cmd, QaQuestionBase qaQuestionObject)
		{	
			AddParameter(cmd, pGuid(QaQuestionBase.Property_CompanyId, qaQuestionObject.CompanyId));
			AddParameter(cmd, pNVarChar(QaQuestionBase.Property_Title, qaQuestionObject.Title));
			AddParameter(cmd, pBool(QaQuestionBase.Property_Qa1, qaQuestionObject.Qa1));
			AddParameter(cmd, pBool(QaQuestionBase.Property_Qa2, qaQuestionObject.Qa2));
			AddParameter(cmd, pNVarChar(QaQuestionBase.Property_Type, 50, qaQuestionObject.Type));
			AddParameter(cmd, pBool(QaQuestionBase.Property_IsActive, qaQuestionObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts QaQuestion
        /// </summary>
        /// <param name="qaQuestionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(QaQuestionBase qaQuestionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTQAQUESTION);
	
				AddParameter(cmd, pInt32Out(QaQuestionBase.Property_Id));
				AddCommonParams(cmd, qaQuestionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					qaQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					qaQuestionObject.Id = (Int32)GetOutParameter(cmd, QaQuestionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(qaQuestionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates QaQuestion
        /// </summary>
        /// <param name="qaQuestionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(QaQuestionBase qaQuestionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEQAQUESTION);
				
				AddParameter(cmd, pInt32(QaQuestionBase.Property_Id, qaQuestionObject.Id));
				AddCommonParams(cmd, qaQuestionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					qaQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(qaQuestionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes QaQuestion
        /// </summary>
        /// <param name="Id">Id of the QaQuestion object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEQAQUESTION);	
				
				AddParameter(cmd, pInt32(QaQuestionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(QaQuestion), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves QaQuestion object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the QaQuestion object to retrieve</param>
        /// <returns>QaQuestion object, null if not found</returns>
		public QaQuestion Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETQAQUESTIONBYID))
			{
				AddParameter( cmd, pInt32(QaQuestionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all QaQuestion objects 
        /// </summary>
        /// <returns>A list of QaQuestion objects</returns>
		public QaQuestionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLQAQUESTION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all QaQuestion objects by PageRequest
        /// </summary>
        /// <returns>A list of QaQuestion objects</returns>
		public QaQuestionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDQAQUESTION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				QaQuestionList _QaQuestionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _QaQuestionList;
			}
		}
		
		/// <summary>
        /// Retrieves all QaQuestion objects by query String
        /// </summary>
        /// <returns>A list of QaQuestion objects</returns>
		public QaQuestionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETQAQUESTIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get QaQuestion Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of QaQuestion
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQAQUESTIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get QaQuestion Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of QaQuestion
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _QaQuestionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQAQUESTIONROWCOUNT))
			{
				SqlDataReader reader;
				_QaQuestionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _QaQuestionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills QaQuestion object
        /// </summary>
        /// <param name="qaQuestionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(QaQuestionBase qaQuestionObject, SqlDataReader reader, int start)
		{
			
				qaQuestionObject.Id = reader.GetInt32( start + 0 );			
				qaQuestionObject.CompanyId = reader.GetGuid( start + 1 );			
				qaQuestionObject.Title = reader.GetString( start + 2 );			
				qaQuestionObject.Qa1 = reader.GetBoolean( start + 3 );			
				qaQuestionObject.Qa2 = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) qaQuestionObject.Type = reader.GetString( start + 5 );			
				qaQuestionObject.IsActive = reader.GetBoolean( start + 6 );			
			FillBaseObject(qaQuestionObject, reader, (start + 7));

			
			qaQuestionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills QaQuestion object
        /// </summary>
        /// <param name="qaQuestionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(QaQuestionBase qaQuestionObject, SqlDataReader reader)
		{
			FillObject(qaQuestionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves QaQuestion object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>QaQuestion object</returns>
		private QaQuestion GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					QaQuestion qaQuestionObject= new QaQuestion();
					FillObject(qaQuestionObject, reader);
					return qaQuestionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of QaQuestion objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of QaQuestion objects</returns>
		private QaQuestionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//QaQuestion list
			QaQuestionList list = new QaQuestionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					QaQuestion qaQuestionObject = new QaQuestion();
					FillObject(qaQuestionObject, reader);

					list.Add(qaQuestionObject);
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
