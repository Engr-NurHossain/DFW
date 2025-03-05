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
	public partial class CustomerAgreementTemplateDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERAGREEMENTTEMPLATE = "InsertCustomerAgreementTemplate";
		private const string UPDATECUSTOMERAGREEMENTTEMPLATE = "UpdateCustomerAgreementTemplate";
		private const string DELETECUSTOMERAGREEMENTTEMPLATE = "DeleteCustomerAgreementTemplate";
		private const string GETCUSTOMERAGREEMENTTEMPLATEBYID = "GetCustomerAgreementTemplateById";
		private const string GETALLCUSTOMERAGREEMENTTEMPLATE = "GetAllCustomerAgreementTemplate";
		private const string GETPAGEDCUSTOMERAGREEMENTTEMPLATE = "GetPagedCustomerAgreementTemplate";
		private const string GETCUSTOMERAGREEMENTTEMPLATEMAXIMUMID = "GetCustomerAgreementTemplateMaximumId";
		private const string GETCUSTOMERAGREEMENTTEMPLATEROWCOUNT = "GetCustomerAgreementTemplateRowCount";	
		private const string GETCUSTOMERAGREEMENTTEMPLATEBYQUERY = "GetCustomerAgreementTemplateByQuery";
		#endregion
		
		#region Constructors
		public CustomerAgreementTemplateDataAccess(ClientContext context) : base(context) { }
		public CustomerAgreementTemplateDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAgreementTemplateObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAgreementTemplateBase customerAgreementTemplateObject)
		{	
			AddParameter(cmd, pGuid(CustomerAgreementTemplateBase.Property_CompanyId, customerAgreementTemplateObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerAgreementTemplateBase.Property_CustomerId, customerAgreementTemplateObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerAgreementTemplateBase.Property_Name, customerAgreementTemplateObject.Name));
			AddParameter(cmd, pNVarChar(CustomerAgreementTemplateBase.Property_Description, 250, customerAgreementTemplateObject.Description));
			AddParameter(cmd, pNVarChar(CustomerAgreementTemplateBase.Property_BodyContent, customerAgreementTemplateObject.BodyContent));
			AddParameter(cmd, pBool(CustomerAgreementTemplateBase.Property_IsActive, customerAgreementTemplateObject.IsActive));
			AddParameter(cmd, pGuid(CustomerAgreementTemplateBase.Property_CreatedBy, customerAgreementTemplateObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomerAgreementTemplateBase.Property_CreatedDate, customerAgreementTemplateObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerAgreementTemplateBase.Property_LastUpdatedBy, customerAgreementTemplateObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(CustomerAgreementTemplateBase.Property_LastUpdatedDate, customerAgreementTemplateObject.LastUpdatedDate));
			AddParameter(cmd, pInt32(CustomerAgreementTemplateBase.Property_ReferenceTemplateId, customerAgreementTemplateObject.ReferenceTemplateId));
			AddParameter(cmd, pBool(CustomerAgreementTemplateBase.Property_IsFileTemplate, customerAgreementTemplateObject.IsFileTemplate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAgreementTemplate
        /// </summary>
        /// <param name="customerAgreementTemplateObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAgreementTemplateBase customerAgreementTemplateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERAGREEMENTTEMPLATE);
	
				AddParameter(cmd, pInt32Out(CustomerAgreementTemplateBase.Property_Id));
				AddCommonParams(cmd, customerAgreementTemplateObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAgreementTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAgreementTemplateObject.Id = (Int32)GetOutParameter(cmd, CustomerAgreementTemplateBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAgreementTemplateObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAgreementTemplate
        /// </summary>
        /// <param name="customerAgreementTemplateObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAgreementTemplateBase customerAgreementTemplateObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERAGREEMENTTEMPLATE);
				
				AddParameter(cmd, pInt32(CustomerAgreementTemplateBase.Property_Id, customerAgreementTemplateObject.Id));
				AddCommonParams(cmd, customerAgreementTemplateObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAgreementTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAgreementTemplateObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAgreementTemplate
        /// </summary>
        /// <param name="Id">Id of the CustomerAgreementTemplate object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERAGREEMENTTEMPLATE);	
				
				AddParameter(cmd, pInt32(CustomerAgreementTemplateBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAgreementTemplate), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAgreementTemplate object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAgreementTemplate object to retrieve</param>
        /// <returns>CustomerAgreementTemplate object, null if not found</returns>
		public CustomerAgreementTemplate Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAGREEMENTTEMPLATEBYID))
			{
				AddParameter( cmd, pInt32(CustomerAgreementTemplateBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAgreementTemplate objects 
        /// </summary>
        /// <returns>A list of CustomerAgreementTemplate objects</returns>
		public CustomerAgreementTemplateList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERAGREEMENTTEMPLATE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAgreementTemplate objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAgreementTemplate objects</returns>
		public CustomerAgreementTemplateList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERAGREEMENTTEMPLATE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAgreementTemplateList _CustomerAgreementTemplateList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAgreementTemplateList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAgreementTemplate objects by query String
        /// </summary>
        /// <returns>A list of CustomerAgreementTemplate objects</returns>
		public CustomerAgreementTemplateList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAGREEMENTTEMPLATEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAgreementTemplate Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAgreementTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAGREEMENTTEMPLATEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAgreementTemplate Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAgreementTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAgreementTemplateRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAGREEMENTTEMPLATEROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAgreementTemplateRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAgreementTemplateRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAgreementTemplate object
        /// </summary>
        /// <param name="customerAgreementTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAgreementTemplateBase customerAgreementTemplateObject, SqlDataReader reader, int start)
		{
			
				customerAgreementTemplateObject.Id = reader.GetInt32( start + 0 );			
				customerAgreementTemplateObject.CompanyId = reader.GetGuid( start + 1 );			
				customerAgreementTemplateObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerAgreementTemplateObject.Name = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerAgreementTemplateObject.Description = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerAgreementTemplateObject.BodyContent = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerAgreementTemplateObject.IsActive = reader.GetBoolean( start + 6 );			
				customerAgreementTemplateObject.CreatedBy = reader.GetGuid( start + 7 );			
				customerAgreementTemplateObject.CreatedDate = reader.GetDateTime( start + 8 );			
				customerAgreementTemplateObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				customerAgreementTemplateObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) customerAgreementTemplateObject.ReferenceTemplateId = reader.GetInt32( start + 11 );			
				if(!reader.IsDBNull(12)) customerAgreementTemplateObject.IsFileTemplate = reader.GetBoolean( start + 12 );			
			FillBaseObject(customerAgreementTemplateObject, reader, (start + 13));

			
			customerAgreementTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAgreementTemplate object
        /// </summary>
        /// <param name="customerAgreementTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAgreementTemplateBase customerAgreementTemplateObject, SqlDataReader reader)
		{
			FillObject(customerAgreementTemplateObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAgreementTemplate object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAgreementTemplate object</returns>
		private CustomerAgreementTemplate GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAgreementTemplate customerAgreementTemplateObject= new CustomerAgreementTemplate();
					FillObject(customerAgreementTemplateObject, reader);
					return customerAgreementTemplateObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAgreementTemplate objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAgreementTemplate objects</returns>
		private CustomerAgreementTemplateList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAgreementTemplate list
			CustomerAgreementTemplateList list = new CustomerAgreementTemplateList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAgreementTemplate customerAgreementTemplateObject = new CustomerAgreementTemplate();
					FillObject(customerAgreementTemplateObject, reader);

					list.Add(customerAgreementTemplateObject);
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
