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
	public partial class EmailTextTemplateDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMAILTEXTTEMPLATE = "InsertEmailTextTemplate";
		private const string UPDATEEMAILTEXTTEMPLATE = "UpdateEmailTextTemplate";
		private const string DELETEEMAILTEXTTEMPLATE = "DeleteEmailTextTemplate";
		private const string GETEMAILTEXTTEMPLATEBYID = "GetEmailTextTemplateById";
		private const string GETALLEMAILTEXTTEMPLATE = "GetAllEmailTextTemplate";
		private const string GETPAGEDEMAILTEXTTEMPLATE = "GetPagedEmailTextTemplate";
		private const string GETEMAILTEXTTEMPLATEMAXIMUMID = "GetEmailTextTemplateMaximumId";
		private const string GETEMAILTEXTTEMPLATEROWCOUNT = "GetEmailTextTemplateRowCount";	
		private const string GETEMAILTEXTTEMPLATEBYQUERY = "GetEmailTextTemplateByQuery";
		#endregion
		
		#region Constructors
		public EmailTextTemplateDataAccess(ClientContext context) : base(context) { }
		public EmailTextTemplateDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="emailTextTemplateObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmailTextTemplateBase emailTextTemplateObject)
		{	
			AddParameter(cmd, pGuid(EmailTextTemplateBase.Property_CompanyId, emailTextTemplateObject.CompanyId));
			AddParameter(cmd, pNVarChar(EmailTextTemplateBase.Property_Type, 50, emailTextTemplateObject.Type));
			AddParameter(cmd, pNVarChar(EmailTextTemplateBase.Property_TextContent, emailTextTemplateObject.TextContent));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmailTextTemplate
        /// </summary>
        /// <param name="emailTextTemplateObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmailTextTemplateBase emailTextTemplateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMAILTEXTTEMPLATE);
	
				AddParameter(cmd, pInt32Out(EmailTextTemplateBase.Property_Id));
				AddCommonParams(cmd, emailTextTemplateObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					emailTextTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					emailTextTemplateObject.Id = (Int32)GetOutParameter(cmd, EmailTextTemplateBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(emailTextTemplateObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmailTextTemplate
        /// </summary>
        /// <param name="emailTextTemplateObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmailTextTemplateBase emailTextTemplateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMAILTEXTTEMPLATE);
				
				AddParameter(cmd, pInt32(EmailTextTemplateBase.Property_Id, emailTextTemplateObject.Id));
				AddCommonParams(cmd, emailTextTemplateObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					emailTextTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(emailTextTemplateObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmailTextTemplate
        /// </summary>
        /// <param name="Id">Id of the EmailTextTemplate object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMAILTEXTTEMPLATE);	
				
				AddParameter(cmd, pInt32(EmailTextTemplateBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmailTextTemplate), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmailTextTemplate object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmailTextTemplate object to retrieve</param>
        /// <returns>EmailTextTemplate object, null if not found</returns>
		public EmailTextTemplate Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMAILTEXTTEMPLATEBYID))
			{
				AddParameter( cmd, pInt32(EmailTextTemplateBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmailTextTemplate objects 
        /// </summary>
        /// <returns>A list of EmailTextTemplate objects</returns>
		public EmailTextTemplateList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMAILTEXTTEMPLATE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmailTextTemplate objects by PageRequest
        /// </summary>
        /// <returns>A list of EmailTextTemplate objects</returns>
		public EmailTextTemplateList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMAILTEXTTEMPLATE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmailTextTemplateList _EmailTextTemplateList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmailTextTemplateList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmailTextTemplate objects by query String
        /// </summary>
        /// <returns>A list of EmailTextTemplate objects</returns>
		public EmailTextTemplateList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMAILTEXTTEMPLATEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmailTextTemplate Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmailTextTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMAILTEXTTEMPLATEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmailTextTemplate Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmailTextTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmailTextTemplateRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMAILTEXTTEMPLATEROWCOUNT))
			{
				SqlDataReader reader;
				_EmailTextTemplateRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmailTextTemplateRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmailTextTemplate object
        /// </summary>
        /// <param name="emailTextTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmailTextTemplateBase emailTextTemplateObject, SqlDataReader reader, int start)
		{
			
				emailTextTemplateObject.Id = reader.GetInt32( start + 0 );			
				emailTextTemplateObject.CompanyId = reader.GetGuid( start + 1 );			
				emailTextTemplateObject.Type = reader.GetString( start + 2 );			
				emailTextTemplateObject.TextContent = reader.GetString( start + 3 );			
			FillBaseObject(emailTextTemplateObject, reader, (start + 4));

			
			emailTextTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmailTextTemplate object
        /// </summary>
        /// <param name="emailTextTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmailTextTemplateBase emailTextTemplateObject, SqlDataReader reader)
		{
			FillObject(emailTextTemplateObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmailTextTemplate object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmailTextTemplate object</returns>
		private EmailTextTemplate GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmailTextTemplate emailTextTemplateObject= new EmailTextTemplate();
					FillObject(emailTextTemplateObject, reader);
					return emailTextTemplateObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmailTextTemplate objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmailTextTemplate objects</returns>
		private EmailTextTemplateList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmailTextTemplate list
			EmailTextTemplateList list = new EmailTextTemplateList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmailTextTemplate emailTextTemplateObject = new EmailTextTemplate();
					FillObject(emailTextTemplateObject, reader);

					list.Add(emailTextTemplateObject);
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
