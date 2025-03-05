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
	public partial class SalesComissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSALESCOMISSION = "InsertSalesComission";
		private const string UPDATESALESCOMISSION = "UpdateSalesComission";
		private const string DELETESALESCOMISSION = "DeleteSalesComission";
		private const string GETSALESCOMISSIONBYID = "GetSalesComissionById";
		private const string GETALLSALESCOMISSION = "GetAllSalesComission";
		private const string GETPAGEDSALESCOMISSION = "GetPagedSalesComission";
		private const string GETSALESCOMISSIONMAXIMUMID = "GetSalesComissionMaximumId";
		private const string GETSALESCOMISSIONROWCOUNT = "GetSalesComissionRowCount";	
		private const string GETSALESCOMISSIONBYQUERY = "GetSalesComissionByQuery";
		#endregion
		
		#region Constructors
		public SalesComissionDataAccess(ClientContext context) : base(context) { }
		public SalesComissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="salesComissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, SalesComissionBase salesComissionObject)
		{	
			AddParameter(cmd, pGuid(SalesComissionBase.Property_CompanyId, salesComissionObject.CompanyId));
			AddParameter(cmd, pGuid(SalesComissionBase.Property_PackageServiceId, salesComissionObject.PackageServiceId));
			AddParameter(cmd, pNVarChar(SalesComissionBase.Property_SalesLocation, 50, salesComissionObject.SalesLocation));
			AddParameter(cmd, pNVarChar(SalesComissionBase.Property_LeadType, 50, salesComissionObject.LeadType));
			AddParameter(cmd, pDouble(SalesComissionBase.Property_AmoutParcent, salesComissionObject.AmoutParcent));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SalesComission
        /// </summary>
        /// <param name="salesComissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SalesComissionBase salesComissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSALESCOMISSION);
	
				AddParameter(cmd, pInt32Out(SalesComissionBase.Property_Id));
				AddCommonParams(cmd, salesComissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					salesComissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					salesComissionObject.Id = (Int32)GetOutParameter(cmd, SalesComissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(salesComissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SalesComission
        /// </summary>
        /// <param name="salesComissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SalesComissionBase salesComissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESALESCOMISSION);
				
				AddParameter(cmd, pInt32(SalesComissionBase.Property_Id, salesComissionObject.Id));
				AddCommonParams(cmd, salesComissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					salesComissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(salesComissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SalesComission
        /// </summary>
        /// <param name="Id">Id of the SalesComission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESALESCOMISSION);	
				
				AddParameter(cmd, pInt32(SalesComissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SalesComission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SalesComission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SalesComission object to retrieve</param>
        /// <returns>SalesComission object, null if not found</returns>
		public SalesComission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSALESCOMISSIONBYID))
			{
				AddParameter( cmd, pInt32(SalesComissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SalesComission objects 
        /// </summary>
        /// <returns>A list of SalesComission objects</returns>
		public SalesComissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSALESCOMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SalesComission objects by PageRequest
        /// </summary>
        /// <returns>A list of SalesComission objects</returns>
		public SalesComissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSALESCOMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SalesComissionList _SalesComissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SalesComissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all SalesComission objects by query String
        /// </summary>
        /// <returns>A list of SalesComission objects</returns>
		public SalesComissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSALESCOMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SalesComission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SalesComission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSALESCOMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SalesComission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SalesComission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SalesComissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSALESCOMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_SalesComissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SalesComissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SalesComission object
        /// </summary>
        /// <param name="salesComissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SalesComissionBase salesComissionObject, SqlDataReader reader, int start)
		{
			
				salesComissionObject.Id = reader.GetInt32( start + 0 );			
				salesComissionObject.CompanyId = reader.GetGuid( start + 1 );			
				salesComissionObject.PackageServiceId = reader.GetGuid( start + 2 );			
				salesComissionObject.SalesLocation = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) salesComissionObject.LeadType = reader.GetString( start + 4 );			
				salesComissionObject.AmoutParcent = reader.GetDouble( start + 5 );			
			FillBaseObject(salesComissionObject, reader, (start + 6));

			
			salesComissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SalesComission object
        /// </summary>
        /// <param name="salesComissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SalesComissionBase salesComissionObject, SqlDataReader reader)
		{
			FillObject(salesComissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SalesComission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SalesComission object</returns>
		private SalesComission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SalesComission salesComissionObject= new SalesComission();
					FillObject(salesComissionObject, reader);
					return salesComissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SalesComission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SalesComission objects</returns>
		private SalesComissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SalesComission list
			SalesComissionList list = new SalesComissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SalesComission salesComissionObject = new SalesComission();
					FillObject(salesComissionObject, reader);

					list.Add(salesComissionObject);
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
