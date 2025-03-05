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
	public partial class ResturantOrderDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTURANTORDERDETAIL = "InsertResturantOrderDetail";
		private const string UPDATERESTURANTORDERDETAIL = "UpdateResturantOrderDetail";
		private const string DELETERESTURANTORDERDETAIL = "DeleteResturantOrderDetail";
		private const string GETRESTURANTORDERDETAILBYID = "GetResturantOrderDetailById";
		private const string GETALLRESTURANTORDERDETAIL = "GetAllResturantOrderDetail";
		private const string GETPAGEDRESTURANTORDERDETAIL = "GetPagedResturantOrderDetail";
		private const string GETRESTURANTORDERDETAILMAXIMUMID = "GetResturantOrderDetailMaximumId";
		private const string GETRESTURANTORDERDETAILROWCOUNT = "GetResturantOrderDetailRowCount";	
		private const string GETRESTURANTORDERDETAILBYQUERY = "GetResturantOrderDetailByQuery";
		#endregion
		
		#region Constructors
		public ResturantOrderDetailDataAccess(ClientContext context) : base(context) { }
		public ResturantOrderDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="resturantOrderDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, ResturantOrderDetailBase resturantOrderDetailObject)
		{	
			AddParameter(cmd, pGuid(ResturantOrderDetailBase.Property_OrderId, resturantOrderDetailObject.OrderId));
			AddParameter(cmd, pInt32(ResturantOrderDetailBase.Property_ItemId, resturantOrderDetailObject.ItemId));
			AddParameter(cmd, pNVarChar(ResturantOrderDetailBase.Property_ItemName, 250, resturantOrderDetailObject.ItemName));
			AddParameter(cmd, pDouble(ResturantOrderDetailBase.Property_ItemPrice, resturantOrderDetailObject.ItemPrice));
			AddParameter(cmd, pInt32(ResturantOrderDetailBase.Property_ItemQty, resturantOrderDetailObject.ItemQty));
			AddParameter(cmd, pDateTime(ResturantOrderDetailBase.Property_CreatedDate, resturantOrderDetailObject.CreatedDate));
			AddParameter(cmd, pNVarChar(ResturantOrderDetailBase.Property_CreatedBy, 150, resturantOrderDetailObject.CreatedBy));
			AddParameter(cmd, pDateTime(ResturantOrderDetailBase.Property_LastUpdatedDate, resturantOrderDetailObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(ResturantOrderDetailBase.Property_LastUpdatedBy, 150, resturantOrderDetailObject.LastUpdatedBy));
			AddParameter(cmd, pGuid(ResturantOrderDetailBase.Property_CompanyId, resturantOrderDetailObject.CompanyId));
			AddParameter(cmd, pBool(ResturantOrderDetailBase.Property_IsStock, resturantOrderDetailObject.IsStock));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ResturantOrderDetail
        /// </summary>
        /// <param name="resturantOrderDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ResturantOrderDetailBase resturantOrderDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTURANTORDERDETAIL);
	
				AddParameter(cmd, pInt32Out(ResturantOrderDetailBase.Property_Id));
				AddCommonParams(cmd, resturantOrderDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					resturantOrderDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					resturantOrderDetailObject.Id = (Int32)GetOutParameter(cmd, ResturantOrderDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(resturantOrderDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ResturantOrderDetail
        /// </summary>
        /// <param name="resturantOrderDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ResturantOrderDetailBase resturantOrderDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTURANTORDERDETAIL);
				
				AddParameter(cmd, pInt32(ResturantOrderDetailBase.Property_Id, resturantOrderDetailObject.Id));
				AddCommonParams(cmd, resturantOrderDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					resturantOrderDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(resturantOrderDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ResturantOrderDetail
        /// </summary>
        /// <param name="Id">Id of the ResturantOrderDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTURANTORDERDETAIL);	
				
				AddParameter(cmd, pInt32(ResturantOrderDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ResturantOrderDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ResturantOrderDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ResturantOrderDetail object to retrieve</param>
        /// <returns>ResturantOrderDetail object, null if not found</returns>
		public ResturantOrderDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERDETAILBYID))
			{
				AddParameter( cmd, pInt32(ResturantOrderDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ResturantOrderDetail objects 
        /// </summary>
        /// <returns>A list of ResturantOrderDetail objects</returns>
		public ResturantOrderDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTURANTORDERDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ResturantOrderDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of ResturantOrderDetail objects</returns>
		public ResturantOrderDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTURANTORDERDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ResturantOrderDetailList _ResturantOrderDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ResturantOrderDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all ResturantOrderDetail objects by query String
        /// </summary>
        /// <returns>A list of ResturantOrderDetail objects</returns>
		public ResturantOrderDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ResturantOrderDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ResturantOrderDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ResturantOrderDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ResturantOrderDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ResturantOrderDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_ResturantOrderDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ResturantOrderDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ResturantOrderDetail object
        /// </summary>
        /// <param name="resturantOrderDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ResturantOrderDetailBase resturantOrderDetailObject, SqlDataReader reader, int start)
		{
			
				resturantOrderDetailObject.Id = reader.GetInt32( start + 0 );			
				resturantOrderDetailObject.OrderId = reader.GetGuid( start + 1 );			
				resturantOrderDetailObject.ItemId = reader.GetInt32( start + 2 );			
				resturantOrderDetailObject.ItemName = reader.GetString( start + 3 );			
				resturantOrderDetailObject.ItemPrice = reader.GetDouble( start + 4 );			
				resturantOrderDetailObject.ItemQty = reader.GetInt32( start + 5 );			
				resturantOrderDetailObject.CreatedDate = reader.GetDateTime( start + 6 );			
				resturantOrderDetailObject.CreatedBy = reader.GetString( start + 7 );			
				resturantOrderDetailObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
				resturantOrderDetailObject.LastUpdatedBy = reader.GetString( start + 9 );			
				resturantOrderDetailObject.CompanyId = reader.GetGuid( start + 10 );			
				if(!reader.IsDBNull(11)) resturantOrderDetailObject.IsStock = reader.GetBoolean( start + 11 );			
			FillBaseObject(resturantOrderDetailObject, reader, (start + 12));

			
			resturantOrderDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ResturantOrderDetail object
        /// </summary>
        /// <param name="resturantOrderDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ResturantOrderDetailBase resturantOrderDetailObject, SqlDataReader reader)
		{
			FillObject(resturantOrderDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ResturantOrderDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ResturantOrderDetail object</returns>
		private ResturantOrderDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ResturantOrderDetail resturantOrderDetailObject= new ResturantOrderDetail();
					FillObject(resturantOrderDetailObject, reader);
					return resturantOrderDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ResturantOrderDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ResturantOrderDetail objects</returns>
		private ResturantOrderDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ResturantOrderDetail list
			ResturantOrderDetailList list = new ResturantOrderDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ResturantOrderDetail resturantOrderDetailObject = new ResturantOrderDetail();
					FillObject(resturantOrderDetailObject, reader);

					list.Add(resturantOrderDetailObject);
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
