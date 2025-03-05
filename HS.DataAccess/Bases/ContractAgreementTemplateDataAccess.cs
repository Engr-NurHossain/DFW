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
	public partial class ContractAgreementTemplateDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCONTRACTAGREEMENTTEMPLATE = "InsertContractAgreementTemplate";
		private const string UPDATECONTRACTAGREEMENTTEMPLATE = "UpdateContractAgreementTemplate";
		private const string DELETECONTRACTAGREEMENTTEMPLATE = "DeleteContractAgreementTemplate";
		private const string GETCONTRACTAGREEMENTTEMPLATEBYID = "GetContractAgreementTemplateById";
		private const string GETALLCONTRACTAGREEMENTTEMPLATE = "GetAllContractAgreementTemplate";
		private const string GETPAGEDCONTRACTAGREEMENTTEMPLATE = "GetPagedContractAgreementTemplate";
		private const string GETCONTRACTAGREEMENTTEMPLATEMAXIMUMID = "GetContractAgreementTemplateMaximumId";
		private const string GETCONTRACTAGREEMENTTEMPLATEROWCOUNT = "GetContractAgreementTemplateRowCount";	
		private const string GETCONTRACTAGREEMENTTEMPLATEBYQUERY = "GetContractAgreementTemplateByQuery";
		#endregion
		
		#region Constructors
		public ContractAgreementTemplateDataAccess(ClientContext context) : base(context) { }
		public ContractAgreementTemplateDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="contractAgreementTemplateObject"></param>
		private void AddCommonParams(SqlCommand cmd, ContractAgreementTemplateBase contractAgreementTemplateObject)
		{	
			AddParameter(cmd, pGuid(ContractAgreementTemplateBase.Property_CompanyId, contractAgreementTemplateObject.CompanyId));
			AddParameter(cmd, pNVarChar(ContractAgreementTemplateBase.Property_Name, contractAgreementTemplateObject.Name));
			AddParameter(cmd, pNVarChar(ContractAgreementTemplateBase.Property_Description, 250, contractAgreementTemplateObject.Description));
			AddParameter(cmd, pNVarChar(ContractAgreementTemplateBase.Property_BodyContent, contractAgreementTemplateObject.BodyContent));
			AddParameter(cmd, pNVarChar(ContractAgreementTemplateBase.Property_BodyFile, contractAgreementTemplateObject.BodyFile));
			AddParameter(cmd, pBool(ContractAgreementTemplateBase.Property_IsActive, contractAgreementTemplateObject.IsActive));
			AddParameter(cmd, pGuid(ContractAgreementTemplateBase.Property_CreatedBy, contractAgreementTemplateObject.CreatedBy));
			AddParameter(cmd, pDateTime(ContractAgreementTemplateBase.Property_CreatedDate, contractAgreementTemplateObject.CreatedDate));
			AddParameter(cmd, pGuid(ContractAgreementTemplateBase.Property_LastUpdatedBy, contractAgreementTemplateObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(ContractAgreementTemplateBase.Property_LastUpdatedDate, contractAgreementTemplateObject.LastUpdatedDate));
			AddParameter(cmd, pBool(ContractAgreementTemplateBase.Property_IsDrawDiagram, contractAgreementTemplateObject.IsDrawDiagram));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ContractAgreementTemplate
        /// </summary>
        /// <param name="contractAgreementTemplateObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ContractAgreementTemplateBase contractAgreementTemplateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCONTRACTAGREEMENTTEMPLATE);
	
				AddParameter(cmd, pInt32Out(ContractAgreementTemplateBase.Property_Id));
				AddCommonParams(cmd, contractAgreementTemplateObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					contractAgreementTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					contractAgreementTemplateObject.Id = (Int32)GetOutParameter(cmd, ContractAgreementTemplateBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(contractAgreementTemplateObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ContractAgreementTemplate
        /// </summary>
        /// <param name="contractAgreementTemplateObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ContractAgreementTemplateBase contractAgreementTemplateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECONTRACTAGREEMENTTEMPLATE);
				
				AddParameter(cmd, pInt32(ContractAgreementTemplateBase.Property_Id, contractAgreementTemplateObject.Id));
				AddCommonParams(cmd, contractAgreementTemplateObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					contractAgreementTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(contractAgreementTemplateObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ContractAgreementTemplate
        /// </summary>
        /// <param name="Id">Id of the ContractAgreementTemplate object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECONTRACTAGREEMENTTEMPLATE);	
				
				AddParameter(cmd, pInt32(ContractAgreementTemplateBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ContractAgreementTemplate), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ContractAgreementTemplate object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ContractAgreementTemplate object to retrieve</param>
        /// <returns>ContractAgreementTemplate object, null if not found</returns>
		public ContractAgreementTemplate Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCONTRACTAGREEMENTTEMPLATEBYID))
			{
				AddParameter( cmd, pInt32(ContractAgreementTemplateBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ContractAgreementTemplate objects 
        /// </summary>
        /// <returns>A list of ContractAgreementTemplate objects</returns>
		public ContractAgreementTemplateList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCONTRACTAGREEMENTTEMPLATE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ContractAgreementTemplate objects by PageRequest
        /// </summary>
        /// <returns>A list of ContractAgreementTemplate objects</returns>
		public ContractAgreementTemplateList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCONTRACTAGREEMENTTEMPLATE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ContractAgreementTemplateList _ContractAgreementTemplateList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ContractAgreementTemplateList;
			}
		}
		
		/// <summary>
        /// Retrieves all ContractAgreementTemplate objects by query String
        /// </summary>
        /// <returns>A list of ContractAgreementTemplate objects</returns>
		public ContractAgreementTemplateList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCONTRACTAGREEMENTTEMPLATEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ContractAgreementTemplate Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ContractAgreementTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCONTRACTAGREEMENTTEMPLATEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ContractAgreementTemplate Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ContractAgreementTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ContractAgreementTemplateRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCONTRACTAGREEMENTTEMPLATEROWCOUNT))
			{
				SqlDataReader reader;
				_ContractAgreementTemplateRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ContractAgreementTemplateRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ContractAgreementTemplate object
        /// </summary>
        /// <param name="contractAgreementTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ContractAgreementTemplateBase contractAgreementTemplateObject, SqlDataReader reader, int start)
		{
			
				contractAgreementTemplateObject.Id = reader.GetInt32( start + 0 );			
				contractAgreementTemplateObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) contractAgreementTemplateObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) contractAgreementTemplateObject.Description = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) contractAgreementTemplateObject.BodyContent = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) contractAgreementTemplateObject.BodyFile = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) contractAgreementTemplateObject.IsActive = reader.GetBoolean( start + 6 );			
				contractAgreementTemplateObject.CreatedBy = reader.GetGuid( start + 7 );			
				contractAgreementTemplateObject.CreatedDate = reader.GetDateTime( start + 8 );			
				contractAgreementTemplateObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				contractAgreementTemplateObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
				contractAgreementTemplateObject.IsDrawDiagram = reader.GetBoolean( start + 11 );			
			FillBaseObject(contractAgreementTemplateObject, reader, (start + 12));

			
			contractAgreementTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ContractAgreementTemplate object
        /// </summary>
        /// <param name="contractAgreementTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ContractAgreementTemplateBase contractAgreementTemplateObject, SqlDataReader reader)
		{
			FillObject(contractAgreementTemplateObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ContractAgreementTemplate object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ContractAgreementTemplate object</returns>
		private ContractAgreementTemplate GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ContractAgreementTemplate contractAgreementTemplateObject= new ContractAgreementTemplate();
					FillObject(contractAgreementTemplateObject, reader);
					return contractAgreementTemplateObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ContractAgreementTemplate objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ContractAgreementTemplate objects</returns>
		private ContractAgreementTemplateList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ContractAgreementTemplate list
			ContractAgreementTemplateList list = new ContractAgreementTemplateList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ContractAgreementTemplate contractAgreementTemplateObject = new ContractAgreementTemplate();
					FillObject(contractAgreementTemplateObject, reader);

					list.Add(contractAgreementTemplateObject);
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
