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
	public partial class CreditScoreGradeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCREDITSCOREGRADE = "InsertCreditScoreGrade";
		private const string UPDATECREDITSCOREGRADE = "UpdateCreditScoreGrade";
		private const string DELETECREDITSCOREGRADE = "DeleteCreditScoreGrade";
		private const string GETCREDITSCOREGRADEBYID = "GetCreditScoreGradeByID";
		private const string GETALLCREDITSCOREGRADE = "GetAllCreditScoreGrade";
		private const string GETPAGEDCREDITSCOREGRADE = "GetPagedCreditScoreGrade";
		private const string GETCREDITSCOREGRADEMAXIMUMID = "GetCreditScoreGradeMaximumID";
		private const string GETCREDITSCOREGRADEROWCOUNT = "GetCreditScoreGradeRowCount";	
		private const string GETCREDITSCOREGRADEBYQUERY = "GetCreditScoreGradeByQuery";
		#endregion
		
		#region Constructors
		public CreditScoreGradeDataAccess(ClientContext context) : base(context) { }
		public CreditScoreGradeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="creditScoreGradeObject"></param>
		private void AddCommonParams(SqlCommand cmd, CreditScoreGradeBase creditScoreGradeObject)
		{	
			AddParameter(cmd, pGuid(CreditScoreGradeBase.Property_CreditGradeId, creditScoreGradeObject.CreditGradeId));
			AddParameter(cmd, pInt32(CreditScoreGradeBase.Property_MinScore, creditScoreGradeObject.MinScore));
			AddParameter(cmd, pInt32(CreditScoreGradeBase.Property_MaxScore, creditScoreGradeObject.MaxScore));
			AddParameter(cmd, pNVarChar(CreditScoreGradeBase.Property_Grade, 50, creditScoreGradeObject.Grade));
			AddParameter(cmd, pGuid(CreditScoreGradeBase.Property_CreatedBy, creditScoreGradeObject.CreatedBy));
			AddParameter(cmd, pDateTime(CreditScoreGradeBase.Property_CreatedDate, creditScoreGradeObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CreditScoreGrade
        /// </summary>
        /// <param name="creditScoreGradeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CreditScoreGradeBase creditScoreGradeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCREDITSCOREGRADE);
	
				AddParameter(cmd, pInt32Out(CreditScoreGradeBase.Property_ID));
				AddCommonParams(cmd, creditScoreGradeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					creditScoreGradeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					creditScoreGradeObject.ID = (Int32)GetOutParameter(cmd, CreditScoreGradeBase.Property_ID);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(creditScoreGradeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CreditScoreGrade
        /// </summary>
        /// <param name="creditScoreGradeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CreditScoreGradeBase creditScoreGradeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECREDITSCOREGRADE);
				
				AddParameter(cmd, pInt32(CreditScoreGradeBase.Property_ID, creditScoreGradeObject.ID));
				AddCommonParams(cmd, creditScoreGradeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					creditScoreGradeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(creditScoreGradeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CreditScoreGrade
        /// </summary>
        /// <param name="ID">ID of the CreditScoreGrade object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _ID)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECREDITSCOREGRADE);	
				
				AddParameter(cmd, pInt32(CreditScoreGradeBase.Property_ID, _ID));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CreditScoreGrade), _ID, x);
			}
			
		}
		#endregion
		
		#region Get By ID Method
		/// <summary>
        /// Retrieves CreditScoreGrade object using it's ID
        /// </summary>
        /// <param name="ID">The ID of the CreditScoreGrade object to retrieve</param>
        /// <returns>CreditScoreGrade object, null if not found</returns>
		public CreditScoreGrade Get(Int32 _ID)
		{
			using( SqlCommand cmd = GetSPCommand(GETCREDITSCOREGRADEBYID))
			{
				AddParameter( cmd, pInt32(CreditScoreGradeBase.Property_ID, _ID));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CreditScoreGrade objects 
        /// </summary>
        /// <returns>A list of CreditScoreGrade objects</returns>
		public CreditScoreGradeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCREDITSCOREGRADE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CreditScoreGrade objects by PageRequest
        /// </summary>
        /// <returns>A list of CreditScoreGrade objects</returns>
		public CreditScoreGradeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCREDITSCOREGRADE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CreditScoreGradeList _CreditScoreGradeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CreditScoreGradeList;
			}
		}
		
		/// <summary>
        /// Retrieves all CreditScoreGrade objects by query String
        /// </summary>
        /// <returns>A list of CreditScoreGrade objects</returns>
		public CreditScoreGradeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCREDITSCOREGRADEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CreditScoreGrade Maximum ID Method
		/// <summary>
        /// Retrieves Get Maximum ID of CreditScoreGrade
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxID()
		{
			Int32 _MaximumID = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCREDITSCOREGRADEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumID = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumID;
		}
		
		#endregion
		
		#region Get CreditScoreGrade Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CreditScoreGrade
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CreditScoreGradeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCREDITSCOREGRADEROWCOUNT))
			{
				SqlDataReader reader;
				_CreditScoreGradeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CreditScoreGradeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CreditScoreGrade object
        /// </summary>
        /// <param name="creditScoreGradeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CreditScoreGradeBase creditScoreGradeObject, SqlDataReader reader, int start)
		{
			
				creditScoreGradeObject.ID = reader.GetInt32( start + 0 );			
				creditScoreGradeObject.CreditGradeId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) creditScoreGradeObject.MinScore = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) creditScoreGradeObject.MaxScore = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) creditScoreGradeObject.Grade = reader.GetString( start + 4 );			
				creditScoreGradeObject.CreatedBy = reader.GetGuid( start + 5 );			
				creditScoreGradeObject.CreatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(creditScoreGradeObject, reader, (start + 7));

			
			creditScoreGradeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CreditScoreGrade object
        /// </summary>
        /// <param name="creditScoreGradeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CreditScoreGradeBase creditScoreGradeObject, SqlDataReader reader)
		{
			FillObject(creditScoreGradeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CreditScoreGrade object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CreditScoreGrade object</returns>
		private CreditScoreGrade GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CreditScoreGrade creditScoreGradeObject= new CreditScoreGrade();
					FillObject(creditScoreGradeObject, reader);
					return creditScoreGradeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CreditScoreGrade objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CreditScoreGrade objects</returns>
		private CreditScoreGradeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CreditScoreGrade list
			CreditScoreGradeList list = new CreditScoreGradeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CreditScoreGrade creditScoreGradeObject = new CreditScoreGrade();
					FillObject(creditScoreGradeObject, reader);

					list.Add(creditScoreGradeObject);
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
