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
	public partial class CustomSurveyDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMSURVEY = "InsertCustomSurvey";
		private const string UPDATECUSTOMSURVEY = "UpdateCustomSurvey";
		private const string DELETECUSTOMSURVEY = "DeleteCustomSurvey";
		private const string GETCUSTOMSURVEYBYID = "GetCustomSurveyById";
		private const string GETALLCUSTOMSURVEY = "GetAllCustomSurvey";
		private const string GETPAGEDCUSTOMSURVEY = "GetPagedCustomSurvey";
		private const string GETCUSTOMSURVEYMAXIMUMID = "GetCustomSurveyMaximumId";
		private const string GETCUSTOMSURVEYROWCOUNT = "GetCustomSurveyRowCount";	
		private const string GETCUSTOMSURVEYBYQUERY = "GetCustomSurveyByQuery";
		#endregion
		
		#region Constructors
		public CustomSurveyDataAccess(ClientContext context) : base(context) { }
		public CustomSurveyDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customSurveyObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomSurveyBase customSurveyObject)
		{	
			AddParameter(cmd, pGuid(CustomSurveyBase.Property_SurveyId, customSurveyObject.SurveyId));
			AddParameter(cmd, pNVarChar(CustomSurveyBase.Property_SurveyName, customSurveyObject.SurveyName));
			AddParameter(cmd, pGuid(CustomSurveyBase.Property_CreatedBy, customSurveyObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomSurveyBase.Property_CreatedDate, customSurveyObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomSurvey
        /// </summary>
        /// <param name="customSurveyObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomSurveyBase customSurveyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMSURVEY);
	
				AddParameter(cmd, pInt32Out(CustomSurveyBase.Property_Id));
				AddCommonParams(cmd, customSurveyObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customSurveyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customSurveyObject.Id = (Int32)GetOutParameter(cmd, CustomSurveyBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customSurveyObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomSurvey
        /// </summary>
        /// <param name="customSurveyObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomSurveyBase customSurveyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMSURVEY);
				
				AddParameter(cmd, pInt32(CustomSurveyBase.Property_Id, customSurveyObject.Id));
				AddCommonParams(cmd, customSurveyObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customSurveyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customSurveyObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomSurvey
        /// </summary>
        /// <param name="Id">Id of the CustomSurvey object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMSURVEY);	
				
				AddParameter(cmd, pInt32(CustomSurveyBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomSurvey), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomSurvey object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomSurvey object to retrieve</param>
        /// <returns>CustomSurvey object, null if not found</returns>
		public CustomSurvey Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYBYID))
			{
				AddParameter( cmd, pInt32(CustomSurveyBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomSurvey objects 
        /// </summary>
        /// <returns>A list of CustomSurvey objects</returns>
		public CustomSurveyList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMSURVEY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomSurvey objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomSurvey objects</returns>
		public CustomSurveyList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMSURVEY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomSurveyList _CustomSurveyList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomSurveyList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomSurvey objects by query String
        /// </summary>
        /// <returns>A list of CustomSurvey objects</returns>
		public CustomSurveyList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomSurvey Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomSurvey
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomSurvey Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomSurvey
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomSurveyRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYROWCOUNT))
			{
				SqlDataReader reader;
				_CustomSurveyRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomSurveyRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomSurvey object
        /// </summary>
        /// <param name="customSurveyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomSurveyBase customSurveyObject, SqlDataReader reader, int start)
		{
			
				customSurveyObject.Id = reader.GetInt32( start + 0 );			
				customSurveyObject.SurveyId = reader.GetGuid( start + 1 );			
				customSurveyObject.SurveyName = reader.GetString( start + 2 );			
				customSurveyObject.CreatedBy = reader.GetGuid( start + 3 );			
				customSurveyObject.CreatedDate = reader.GetDateTime( start + 4 );			
			FillBaseObject(customSurveyObject, reader, (start + 5));

			
			customSurveyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomSurvey object
        /// </summary>
        /// <param name="customSurveyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomSurveyBase customSurveyObject, SqlDataReader reader)
		{
			FillObject(customSurveyObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomSurvey object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomSurvey object</returns>
		private CustomSurvey GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomSurvey customSurveyObject= new CustomSurvey();
					FillObject(customSurveyObject, reader);
					return customSurveyObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomSurvey objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomSurvey objects</returns>
		private CustomSurveyList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomSurvey list
			CustomSurveyList list = new CustomSurveyList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomSurvey customSurveyObject = new CustomSurvey();
					FillObject(customSurveyObject, reader);

					list.Add(customSurveyObject);
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
