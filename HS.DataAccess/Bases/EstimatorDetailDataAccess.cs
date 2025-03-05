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
	public partial class EstimatorDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTESTIMATORDETAIL = "InsertEstimatorDetail";
		private const string UPDATEESTIMATORDETAIL = "UpdateEstimatorDetail";
		private const string DELETEESTIMATORDETAIL = "DeleteEstimatorDetail";
		private const string GETESTIMATORDETAILBYID = "GetEstimatorDetailById";
		private const string GETALLESTIMATORDETAIL = "GetAllEstimatorDetail";
		private const string GETPAGEDESTIMATORDETAIL = "GetPagedEstimatorDetail";
		private const string GETESTIMATORDETAILMAXIMUMID = "GetEstimatorDetailMaximumId";
		private const string GETESTIMATORDETAILROWCOUNT = "GetEstimatorDetailRowCount";	
		private const string GETESTIMATORDETAILBYQUERY = "GetEstimatorDetailByQuery";
		#endregion
		
		#region Constructors
		public EstimatorDetailDataAccess(ClientContext context) : base(context) { }
		public EstimatorDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="estimatorDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, EstimatorDetailBase estimatorDetailObject)
		{	
			AddParameter(cmd, pNVarChar(EstimatorDetailBase.Property_EstimatorId, 50, estimatorDetailObject.EstimatorId));
			AddParameter(cmd, pNVarChar(EstimatorDetailBase.Property_PartDescription, 500, estimatorDetailObject.PartDescription));
			AddParameter(cmd, pNVarChar(EstimatorDetailBase.Property_PartNumber, 500, estimatorDetailObject.PartNumber));
			AddParameter(cmd, pInt32(EstimatorDetailBase.Property_CategoryId, estimatorDetailObject.CategoryId));
			AddParameter(cmd, pNVarChar(EstimatorDetailBase.Property_Unit, 50, estimatorDetailObject.Unit));
			AddParameter(cmd, pDouble(EstimatorDetailBase.Property_Qunatity, estimatorDetailObject.Qunatity));
			AddParameter(cmd, pDouble(EstimatorDetailBase.Property_Overhead, estimatorDetailObject.Overhead));
			AddParameter(cmd, pDouble(EstimatorDetailBase.Property_UnitCost, estimatorDetailObject.UnitCost));
			AddParameter(cmd, pDouble(EstimatorDetailBase.Property_TotalCost, estimatorDetailObject.TotalCost));
			AddParameter(cmd, pDouble(EstimatorDetailBase.Property_Profit, estimatorDetailObject.Profit));
			AddParameter(cmd, pDouble(EstimatorDetailBase.Property_TotalPrice, estimatorDetailObject.TotalPrice));
			AddParameter(cmd, pGuid(EstimatorDetailBase.Property_EquipmentId, estimatorDetailObject.EquipmentId));
			AddParameter(cmd, pGuid(EstimatorDetailBase.Property_SupplierId, estimatorDetailObject.SupplierId));
			AddParameter(cmd, pGuid(EstimatorDetailBase.Property_CreatedBy, estimatorDetailObject.CreatedBy));
			AddParameter(cmd, pDateTime(EstimatorDetailBase.Property_CreatedDate, estimatorDetailObject.CreatedDate));
			AddParameter(cmd, pBool(EstimatorDetailBase.Property_IsTaxable, estimatorDetailObject.IsTaxable));
			AddParameter(cmd, pDouble(EstimatorDetailBase.Property_OverheadRate, estimatorDetailObject.OverheadRate));
			AddParameter(cmd, pDouble(EstimatorDetailBase.Property_ProfitRate, estimatorDetailObject.ProfitRate));
			AddParameter(cmd, pGuid(EstimatorDetailBase.Property_ManufacturerId, estimatorDetailObject.ManufacturerId));
			AddParameter(cmd, pInt32(EstimatorDetailBase.Property_EquipmentManufacturerId, estimatorDetailObject.EquipmentManufacturerId));
			AddParameter(cmd, pNVarChar(EstimatorDetailBase.Property_Variation, 50, estimatorDetailObject.Variation));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EstimatorDetail
        /// </summary>
        /// <param name="estimatorDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EstimatorDetailBase estimatorDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTESTIMATORDETAIL);
	
				AddParameter(cmd, pInt32Out(EstimatorDetailBase.Property_Id));
				AddCommonParams(cmd, estimatorDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					estimatorDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					estimatorDetailObject.Id = (Int32)GetOutParameter(cmd, EstimatorDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(estimatorDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EstimatorDetail
        /// </summary>
        /// <param name="estimatorDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EstimatorDetailBase estimatorDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEESTIMATORDETAIL);
				
				AddParameter(cmd, pInt32(EstimatorDetailBase.Property_Id, estimatorDetailObject.Id));
				AddCommonParams(cmd, estimatorDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					estimatorDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(estimatorDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EstimatorDetail
        /// </summary>
        /// <param name="Id">Id of the EstimatorDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEESTIMATORDETAIL);	
				
				AddParameter(cmd, pInt32(EstimatorDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EstimatorDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EstimatorDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EstimatorDetail object to retrieve</param>
        /// <returns>EstimatorDetail object, null if not found</returns>
		public EstimatorDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORDETAILBYID))
			{
				AddParameter( cmd, pInt32(EstimatorDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EstimatorDetail objects 
        /// </summary>
        /// <returns>A list of EstimatorDetail objects</returns>
		public EstimatorDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLESTIMATORDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EstimatorDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of EstimatorDetail objects</returns>
		public EstimatorDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDESTIMATORDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EstimatorDetailList _EstimatorDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EstimatorDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all EstimatorDetail objects by query String
        /// </summary>
        /// <returns>A list of EstimatorDetail objects</returns>
		public EstimatorDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EstimatorDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EstimatorDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EstimatorDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EstimatorDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EstimatorDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_EstimatorDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EstimatorDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EstimatorDetail object
        /// </summary>
        /// <param name="estimatorDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EstimatorDetailBase estimatorDetailObject, SqlDataReader reader, int start)
		{
			
				estimatorDetailObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) estimatorDetailObject.EstimatorId = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) estimatorDetailObject.PartDescription = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) estimatorDetailObject.PartNumber = reader.GetString( start + 3 );			
				estimatorDetailObject.CategoryId = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) estimatorDetailObject.Unit = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) estimatorDetailObject.Qunatity = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) estimatorDetailObject.Overhead = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) estimatorDetailObject.UnitCost = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) estimatorDetailObject.TotalCost = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) estimatorDetailObject.Profit = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) estimatorDetailObject.TotalPrice = reader.GetDouble( start + 11 );			
				estimatorDetailObject.EquipmentId = reader.GetGuid( start + 12 );			
				estimatorDetailObject.SupplierId = reader.GetGuid( start + 13 );			
				estimatorDetailObject.CreatedBy = reader.GetGuid( start + 14 );			
				estimatorDetailObject.CreatedDate = reader.GetDateTime( start + 15 );			
				if(!reader.IsDBNull(16)) estimatorDetailObject.IsTaxable = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) estimatorDetailObject.OverheadRate = reader.GetDouble( start + 17 );			
				if(!reader.IsDBNull(18)) estimatorDetailObject.ProfitRate = reader.GetDouble( start + 18 );			
				estimatorDetailObject.ManufacturerId = reader.GetGuid( start + 19 );			
				if(!reader.IsDBNull(20)) estimatorDetailObject.EquipmentManufacturerId = reader.GetInt32( start + 20 );			
				if(!reader.IsDBNull(21)) estimatorDetailObject.Variation = reader.GetString( start + 21 );			
			FillBaseObject(estimatorDetailObject, reader, (start + 22));

			
			estimatorDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EstimatorDetail object
        /// </summary>
        /// <param name="estimatorDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EstimatorDetailBase estimatorDetailObject, SqlDataReader reader)
		{
			FillObject(estimatorDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EstimatorDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EstimatorDetail object</returns>
		private EstimatorDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EstimatorDetail estimatorDetailObject= new EstimatorDetail();
					FillObject(estimatorDetailObject, reader);
					return estimatorDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EstimatorDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EstimatorDetail objects</returns>
		private EstimatorDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EstimatorDetail list
			EstimatorDetailList list = new EstimatorDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EstimatorDetail estimatorDetailObject = new EstimatorDetail();
					FillObject(estimatorDetailObject, reader);

					list.Add(estimatorDetailObject);
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
