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
	public partial class EstimatorDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTESTIMATOR = "InsertEstimator";
		private const string UPDATEESTIMATOR = "UpdateEstimator";
		private const string DELETEESTIMATOR = "DeleteEstimator";
		private const string GETESTIMATORBYID = "GetEstimatorById";
		private const string GETALLESTIMATOR = "GetAllEstimator";
		private const string GETPAGEDESTIMATOR = "GetPagedEstimator";
		private const string GETESTIMATORMAXIMUMID = "GetEstimatorMaximumId";
		private const string GETESTIMATORROWCOUNT = "GetEstimatorRowCount";	
		private const string GETESTIMATORBYQUERY = "GetEstimatorByQuery";
		#endregion
		
		#region Constructors
		public EstimatorDataAccess(ClientContext context) : base(context) { }
		public EstimatorDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="estimatorObject"></param>
		private void AddCommonParams(SqlCommand cmd, EstimatorBase estimatorObject)
		{	
			AddParameter(cmd, pGuid(EstimatorBase.Property_CompanyId, estimatorObject.CompanyId));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_EstimatorId, 50, estimatorObject.EstimatorId));
			AddParameter(cmd, pGuid(EstimatorBase.Property_CustomerId, estimatorObject.CustomerId));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_BillingAddress, estimatorObject.BillingAddress));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_ProjectAddress, estimatorObject.ProjectAddress));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_Status, 50, estimatorObject.Status));
			AddParameter(cmd, pDateTime(EstimatorBase.Property_StartDate, estimatorObject.StartDate));
			AddParameter(cmd, pDateTime(EstimatorBase.Property_CompletionDate, estimatorObject.CompletionDate));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_EmailAddress, 250, estimatorObject.EmailAddress));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_Description, estimatorObject.Description));
			AddParameter(cmd, pDouble(EstimatorBase.Property_TaxPercnetage, estimatorObject.TaxPercnetage));
			AddParameter(cmd, pDouble(EstimatorBase.Property_TaxAmount, estimatorObject.TaxAmount));
			AddParameter(cmd, pDouble(EstimatorBase.Property_TotalPrice, estimatorObject.TotalPrice));
			AddParameter(cmd, pDouble(EstimatorBase.Property_TotalCost, estimatorObject.TotalCost));
			AddParameter(cmd, pDouble(EstimatorBase.Property_PoriftPercentage, estimatorObject.PoriftPercentage));
			AddParameter(cmd, pDouble(EstimatorBase.Property_TotalProfitAmount, estimatorObject.TotalProfitAmount));
			AddParameter(cmd, pDouble(EstimatorBase.Property_OverheadCostPercentage, estimatorObject.OverheadCostPercentage));
			AddParameter(cmd, pDouble(EstimatorBase.Property_TotalOverheadCostAmount, estimatorObject.TotalOverheadCostAmount));
			AddParameter(cmd, pGuid(EstimatorBase.Property_CreatedBy, estimatorObject.CreatedBy));
			AddParameter(cmd, pDateTime(EstimatorBase.Property_CreatedDate, estimatorObject.CreatedDate));
			AddParameter(cmd, pGuid(EstimatorBase.Property_LastUpdatedBy, estimatorObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(EstimatorBase.Property_LastUpdatedDate, estimatorObject.LastUpdatedDate));
			AddParameter(cmd, pDouble(EstimatorBase.Property_DefaultOverheadRate, estimatorObject.DefaultOverheadRate));
			AddParameter(cmd, pDouble(EstimatorBase.Property_DefaultProfitRate, estimatorObject.DefaultProfitRate));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_CoverLetter, estimatorObject.CoverLetter));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_CoverLetterFile, estimatorObject.CoverLetterFile));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_PaymentTerm, 50, estimatorObject.PaymentTerm));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_ExpiresOn, 50, estimatorObject.ExpiresOn));
			AddParameter(cmd, pDateTime(EstimatorBase.Property_EstimateDate, estimatorObject.EstimateDate));
			AddParameter(cmd, pBool(EstimatorBase.Property_ShowServicePlan, estimatorObject.ShowServicePlan));
			AddParameter(cmd, pDouble(EstimatorBase.Property_ServicePlanRate, estimatorObject.ServicePlanRate));
			AddParameter(cmd, pBool(EstimatorBase.Property_ShowService, estimatorObject.ShowService));
			AddParameter(cmd, pDouble(EstimatorBase.Property_ServicePlanAmount, estimatorObject.ServicePlanAmount));
			AddParameter(cmd, pDouble(EstimatorBase.Property_ServiceTaxAmount, estimatorObject.ServiceTaxAmount));
			AddParameter(cmd, pDouble(EstimatorBase.Property_ServiceTotalAmount, estimatorObject.ServiceTotalAmount));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_ServicePlanType, 50, estimatorObject.ServicePlanType));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_ContractTerm, 50, estimatorObject.ContractTerm));
			AddParameter(cmd, pDouble(EstimatorBase.Property_DefaultMaterialMarkupRate, estimatorObject.DefaultMaterialMarkupRate));
			AddParameter(cmd, pDouble(EstimatorBase.Property_ActivationFee, estimatorObject.ActivationFee));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_EstimatorSignature, 250, estimatorObject.EstimatorSignature));
			AddParameter(cmd, pNVarChar(EstimatorBase.Property_ParentEstimatorRef, 100, estimatorObject.ParentEstimatorRef));
			AddParameter(cmd, pBool(EstimatorBase.Property_IsApproved, estimatorObject.IsApproved));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Estimator
        /// </summary>
        /// <param name="estimatorObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EstimatorBase estimatorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTESTIMATOR);
	
				AddParameter(cmd, pInt32Out(EstimatorBase.Property_Id));
				AddCommonParams(cmd, estimatorObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					estimatorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					estimatorObject.Id = (Int32)GetOutParameter(cmd, EstimatorBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(estimatorObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Estimator
        /// </summary>
        /// <param name="estimatorObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EstimatorBase estimatorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEESTIMATOR);
				
				AddParameter(cmd, pInt32(EstimatorBase.Property_Id, estimatorObject.Id));
				AddCommonParams(cmd, estimatorObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					estimatorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(estimatorObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Estimator
        /// </summary>
        /// <param name="Id">Id of the Estimator object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEESTIMATOR);	
				
				AddParameter(cmd, pInt32(EstimatorBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Estimator), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Estimator object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Estimator object to retrieve</param>
        /// <returns>Estimator object, null if not found</returns>
		public Estimator Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORBYID))
			{
				AddParameter( cmd, pInt32(EstimatorBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Estimator objects 
        /// </summary>
        /// <returns>A list of Estimator objects</returns>
		public EstimatorList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLESTIMATOR))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Estimator objects by PageRequest
        /// </summary>
        /// <returns>A list of Estimator objects</returns>
		public EstimatorList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDESTIMATOR))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EstimatorList _EstimatorList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EstimatorList;
			}
		}
		
		/// <summary>
        /// Retrieves all Estimator objects by query String
        /// </summary>
        /// <returns>A list of Estimator objects</returns>
		public EstimatorList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Estimator Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Estimator
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Estimator Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Estimator
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EstimatorRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORROWCOUNT))
			{
				SqlDataReader reader;
				_EstimatorRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EstimatorRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Estimator object
        /// </summary>
        /// <param name="estimatorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EstimatorBase estimatorObject, SqlDataReader reader, int start)
		{
			
				estimatorObject.Id = reader.GetInt32( start + 0 );			
				estimatorObject.CompanyId = reader.GetGuid( start + 1 );			
				estimatorObject.EstimatorId = reader.GetString( start + 2 );			
				estimatorObject.CustomerId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) estimatorObject.BillingAddress = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) estimatorObject.ProjectAddress = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) estimatorObject.Status = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) estimatorObject.StartDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) estimatorObject.CompletionDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) estimatorObject.EmailAddress = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) estimatorObject.Description = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) estimatorObject.TaxPercnetage = reader.GetDouble( start + 11 );			
				if(!reader.IsDBNull(12)) estimatorObject.TaxAmount = reader.GetDouble( start + 12 );			
				if(!reader.IsDBNull(13)) estimatorObject.TotalPrice = reader.GetDouble( start + 13 );			
				if(!reader.IsDBNull(14)) estimatorObject.TotalCost = reader.GetDouble( start + 14 );			
				if(!reader.IsDBNull(15)) estimatorObject.PoriftPercentage = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) estimatorObject.TotalProfitAmount = reader.GetDouble( start + 16 );			
				if(!reader.IsDBNull(17)) estimatorObject.OverheadCostPercentage = reader.GetDouble( start + 17 );			
				if(!reader.IsDBNull(18)) estimatorObject.TotalOverheadCostAmount = reader.GetDouble( start + 18 );			
				estimatorObject.CreatedBy = reader.GetGuid( start + 19 );			
				estimatorObject.CreatedDate = reader.GetDateTime( start + 20 );			
				estimatorObject.LastUpdatedBy = reader.GetGuid( start + 21 );			
				estimatorObject.LastUpdatedDate = reader.GetDateTime( start + 22 );			
				if(!reader.IsDBNull(23)) estimatorObject.DefaultOverheadRate = reader.GetDouble( start + 23 );			
				if(!reader.IsDBNull(24)) estimatorObject.DefaultProfitRate = reader.GetDouble( start + 24 );			
				if(!reader.IsDBNull(25)) estimatorObject.CoverLetter = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) estimatorObject.CoverLetterFile = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) estimatorObject.PaymentTerm = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) estimatorObject.ExpiresOn = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) estimatorObject.EstimateDate = reader.GetDateTime( start + 29 );			
				if(!reader.IsDBNull(30)) estimatorObject.ShowServicePlan = reader.GetBoolean( start + 30 );			
				if(!reader.IsDBNull(31)) estimatorObject.ServicePlanRate = reader.GetDouble( start + 31 );			
				if(!reader.IsDBNull(32)) estimatorObject.ShowService = reader.GetBoolean( start + 32 );			
				if(!reader.IsDBNull(33)) estimatorObject.ServicePlanAmount = reader.GetDouble( start + 33 );			
				if(!reader.IsDBNull(34)) estimatorObject.ServiceTaxAmount = reader.GetDouble( start + 34 );			
				if(!reader.IsDBNull(35)) estimatorObject.ServiceTotalAmount = reader.GetDouble( start + 35 );			
				if(!reader.IsDBNull(36)) estimatorObject.ServicePlanType = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) estimatorObject.ContractTerm = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) estimatorObject.DefaultMaterialMarkupRate = reader.GetDouble( start + 38 );			
				if(!reader.IsDBNull(39)) estimatorObject.ActivationFee = reader.GetDouble( start + 39 );			
				if(!reader.IsDBNull(40)) estimatorObject.EstimatorSignature = reader.GetString( start + 40 );			
				if(!reader.IsDBNull(41)) estimatorObject.ParentEstimatorRef = reader.GetString( start + 41 );			
				estimatorObject.IsApproved = reader.GetBoolean( start + 42 );			
			FillBaseObject(estimatorObject, reader, (start + 43));

			
			estimatorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Estimator object
        /// </summary>
        /// <param name="estimatorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EstimatorBase estimatorObject, SqlDataReader reader)
		{
			FillObject(estimatorObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Estimator object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Estimator object</returns>
		private Estimator GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Estimator estimatorObject= new Estimator();
					FillObject(estimatorObject, reader);
					return estimatorObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Estimator objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Estimator objects</returns>
		private EstimatorList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Estimator list
			EstimatorList list = new EstimatorList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Estimator estimatorObject = new Estimator();
					FillObject(estimatorObject, reader);

					list.Add(estimatorObject);
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
