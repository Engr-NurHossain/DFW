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
	public partial class FormGeneratorDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTFORMGENERATOR = "InsertFormGenerator";
		private const string UPDATEFORMGENERATOR = "UpdateFormGenerator";
		private const string DELETEFORMGENERATOR = "DeleteFormGenerator";
		private const string GETFORMGENERATORBYID = "GetFormGeneratorById";
		private const string GETALLFORMGENERATOR = "GetAllFormGenerator";
		private const string GETPAGEDFORMGENERATOR = "GetPagedFormGenerator";
		private const string GETFORMGENERATORMAXIMUMID = "GetFormGeneratorMaximumId";
		private const string GETFORMGENERATORROWCOUNT = "GetFormGeneratorRowCount";	
		private const string GETFORMGENERATORBYQUERY = "GetFormGeneratorByQuery";
		#endregion
		
		#region Constructors
		public FormGeneratorDataAccess(ClientContext context) : base(context) { }
		public FormGeneratorDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="formGeneratorObject"></param>
		private void AddCommonParams(SqlCommand cmd, FormGeneratorBase formGeneratorObject)
		{	
			AddParameter(cmd, pGuid(FormGeneratorBase.Property_CompanyId, formGeneratorObject.CompanyId));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_FormName, 50, formGeneratorObject.FormName));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_FieldLabel, 50, formGeneratorObject.FieldLabel));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_FieldType, 50, formGeneratorObject.FieldType));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_FieldName, 50, formGeneratorObject.FieldName));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_DataType, 50, formGeneratorObject.DataType));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_DataKey, 50, formGeneratorObject.DataKey));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_Placeholder, 50, formGeneratorObject.Placeholder));
			AddParameter(cmd, pInt32(FormGeneratorBase.Property_OrderBy, formGeneratorObject.OrderBy));
			AddParameter(cmd, pBool(FormGeneratorBase.Property_IsActive, formGeneratorObject.IsActive));
			AddParameter(cmd, pBool(FormGeneratorBase.Property_IsRequired, formGeneratorObject.IsRequired));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_ErrorMessage, 250, formGeneratorObject.ErrorMessage));
			AddParameter(cmd, pNVarChar(FormGeneratorBase.Property_ErrorMessage2, 250, formGeneratorObject.ErrorMessage2));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts FormGenerator
        /// </summary>
        /// <param name="formGeneratorObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(FormGeneratorBase formGeneratorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTFORMGENERATOR);
	
				AddParameter(cmd, pInt32Out(FormGeneratorBase.Property_Id));
				AddCommonParams(cmd, formGeneratorObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					formGeneratorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					formGeneratorObject.Id = (Int32)GetOutParameter(cmd, FormGeneratorBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(formGeneratorObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates FormGenerator
        /// </summary>
        /// <param name="formGeneratorObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(FormGeneratorBase formGeneratorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEFORMGENERATOR);
				
				AddParameter(cmd, pInt32(FormGeneratorBase.Property_Id, formGeneratorObject.Id));
				AddCommonParams(cmd, formGeneratorObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					formGeneratorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(formGeneratorObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes FormGenerator
        /// </summary>
        /// <param name="Id">Id of the FormGenerator object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEFORMGENERATOR);	
				
				AddParameter(cmd, pInt32(FormGeneratorBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(FormGenerator), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves FormGenerator object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the FormGenerator object to retrieve</param>
        /// <returns>FormGenerator object, null if not found</returns>
		public FormGenerator Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETFORMGENERATORBYID))
			{
				AddParameter( cmd, pInt32(FormGeneratorBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all FormGenerator objects 
        /// </summary>
        /// <returns>A list of FormGenerator objects</returns>
		public FormGeneratorList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLFORMGENERATOR))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all FormGenerator objects by PageRequest
        /// </summary>
        /// <returns>A list of FormGenerator objects</returns>
		public FormGeneratorList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDFORMGENERATOR))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				FormGeneratorList _FormGeneratorList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _FormGeneratorList;
			}
		}
		
		/// <summary>
        /// Retrieves all FormGenerator objects by query String
        /// </summary>
        /// <returns>A list of FormGenerator objects</returns>
		public FormGeneratorList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETFORMGENERATORBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get FormGenerator Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of FormGenerator
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFORMGENERATORMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get FormGenerator Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of FormGenerator
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _FormGeneratorRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFORMGENERATORROWCOUNT))
			{
				SqlDataReader reader;
				_FormGeneratorRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _FormGeneratorRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills FormGenerator object
        /// </summary>
        /// <param name="formGeneratorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(FormGeneratorBase formGeneratorObject, SqlDataReader reader, int start)
		{
			
				formGeneratorObject.Id = reader.GetInt32( start + 0 );			
				formGeneratorObject.CompanyId = reader.GetGuid( start + 1 );			
				formGeneratorObject.FormName = reader.GetString( start + 2 );			
				formGeneratorObject.FieldLabel = reader.GetString( start + 3 );			
				formGeneratorObject.FieldType = reader.GetString( start + 4 );			
				formGeneratorObject.FieldName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) formGeneratorObject.DataType = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) formGeneratorObject.DataKey = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) formGeneratorObject.Placeholder = reader.GetString( start + 8 );			
				formGeneratorObject.OrderBy = reader.GetInt32( start + 9 );			
				formGeneratorObject.IsActive = reader.GetBoolean( start + 10 );			
				formGeneratorObject.IsRequired = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) formGeneratorObject.ErrorMessage = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) formGeneratorObject.ErrorMessage2 = reader.GetString( start + 13 );			
			FillBaseObject(formGeneratorObject, reader, (start + 14));

			
			formGeneratorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills FormGenerator object
        /// </summary>
        /// <param name="formGeneratorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(FormGeneratorBase formGeneratorObject, SqlDataReader reader)
		{
			FillObject(formGeneratorObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves FormGenerator object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>FormGenerator object</returns>
		private FormGenerator GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					FormGenerator formGeneratorObject= new FormGenerator();
					FillObject(formGeneratorObject, reader);
					return formGeneratorObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of FormGenerator objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of FormGenerator objects</returns>
		private FormGeneratorList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//FormGenerator list
			FormGeneratorList list = new FormGeneratorList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					FormGenerator formGeneratorObject = new FormGenerator();
					FillObject(formGeneratorObject, reader);

					list.Add(formGeneratorObject);
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
