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
	public partial class FundDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTFUND = "InsertFund";
		private const string UPDATEFUND = "UpdateFund";
		private const string DELETEFUND = "DeleteFund";
		private const string GETFUNDBYID = "GetFundById";
		private const string GETALLFUND = "GetAllFund";
		private const string GETPAGEDFUND = "GetPagedFund";
		private const string GETFUNDMAXIMUMID = "GetFundMaximumId";
		private const string GETFUNDROWCOUNT = "GetFundRowCount";	
		private const string GETFUNDBYQUERY = "GetFundByQuery";
		#endregion
		
		#region Constructors
		public FundDataAccess(ClientContext context) : base(context) { }
		public FundDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="fundObject"></param>
		private void AddCommonParams(SqlCommand cmd, FundBase fundObject)
		{	
			AddParameter(cmd, pGuid(FundBase.Property_CompanyId, fundObject.CompanyId));
			AddParameter(cmd, pGuid(FundBase.Property_CustomerId, fundObject.CustomerId));
			AddParameter(cmd, pNVarChar(FundBase.Property_Type, 50, fundObject.Type));
			AddParameter(cmd, pDouble(FundBase.Property_Amount, fundObject.Amount));
			AddParameter(cmd, pNVarChar(FundBase.Property_PaymentMethod, 50, fundObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(FundBase.Property_PaymentStatus, 50, fundObject.PaymentStatus));
			AddParameter(cmd, pDateTime(FundBase.Property_PaymentDate, fundObject.PaymentDate));
			AddParameter(cmd, pNVarChar(FundBase.Property_Notes, fundObject.Notes));
			AddParameter(cmd, pNVarChar(FundBase.Property_UpdatedBy, 50, fundObject.UpdatedBy));
			AddParameter(cmd, pDateTime(FundBase.Property_UpdatedDate, fundObject.UpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Fund
        /// </summary>
        /// <param name="fundObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(FundBase fundObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTFUND);
	
				AddParameter(cmd, pInt32Out(FundBase.Property_Id));
				AddCommonParams(cmd, fundObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					fundObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					fundObject.Id = (Int32)GetOutParameter(cmd, FundBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(fundObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Fund
        /// </summary>
        /// <param name="fundObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(FundBase fundObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEFUND);
				
				AddParameter(cmd, pInt32(FundBase.Property_Id, fundObject.Id));
				AddCommonParams(cmd, fundObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					fundObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(fundObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Fund
        /// </summary>
        /// <param name="Id">Id of the Fund object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEFUND);	
				
				AddParameter(cmd, pInt32(FundBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Fund), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Fund object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Fund object to retrieve</param>
        /// <returns>Fund object, null if not found</returns>
		public Fund Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETFUNDBYID))
			{
				AddParameter( cmd, pInt32(FundBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Fund objects 
        /// </summary>
        /// <returns>A list of Fund objects</returns>
		public FundList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLFUND))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Fund objects by PageRequest
        /// </summary>
        /// <returns>A list of Fund objects</returns>
		public FundList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDFUND))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				FundList _FundList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _FundList;
			}
		}
		
		/// <summary>
        /// Retrieves all Fund objects by query String
        /// </summary>
        /// <returns>A list of Fund objects</returns>
		public FundList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETFUNDBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Fund Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Fund
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFUNDMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Fund Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Fund
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _FundRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFUNDROWCOUNT))
			{
				SqlDataReader reader;
				_FundRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _FundRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Fund object
        /// </summary>
        /// <param name="fundObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(FundBase fundObject, SqlDataReader reader, int start)
		{
			
				fundObject.Id = reader.GetInt32( start + 0 );			
				fundObject.CompanyId = reader.GetGuid( start + 1 );			
				fundObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) fundObject.Type = reader.GetString( start + 3 );			
				fundObject.Amount = reader.GetDouble( start + 4 );			
				fundObject.PaymentMethod = reader.GetString( start + 5 );			
				fundObject.PaymentStatus = reader.GetString( start + 6 );			
				fundObject.PaymentDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) fundObject.Notes = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) fundObject.UpdatedBy = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) fundObject.UpdatedDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(fundObject, reader, (start + 11));

			
			fundObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Fund object
        /// </summary>
        /// <param name="fundObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(FundBase fundObject, SqlDataReader reader)
		{
			FillObject(fundObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Fund object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Fund object</returns>
		private Fund GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Fund fundObject= new Fund();
					FillObject(fundObject, reader);
					return fundObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Fund objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Fund objects</returns>
		private FundList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Fund list
			FundList list = new FundList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Fund fundObject = new Fund();
					FillObject(fundObject, reader);

					list.Add(fundObject);
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
