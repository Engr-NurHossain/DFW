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
	public partial class BundleDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBUNDLE = "InsertBundle";
		private const string UPDATEBUNDLE = "UpdateBundle";
		private const string DELETEBUNDLE = "DeleteBundle";
		private const string GETBUNDLEBYID = "GetBundleById";
		private const string GETALLBUNDLE = "GetAllBundle";
		private const string GETPAGEDBUNDLE = "GetPagedBundle";
		private const string GETBUNDLEMAXIMUMID = "GetBundleMaximumId";
		private const string GETBUNDLEROWCOUNT = "GetBundleRowCount";	
		private const string GETBUNDLEBYQUERY = "GetBundleByQuery";
		#endregion
		
		#region Constructors
		public BundleDataAccess(ClientContext context) : base(context) { }
		public BundleDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="bundleObject"></param>
		private void AddCommonParams(SqlCommand cmd, BundleBase bundleObject)
		{	
			AddParameter(cmd, pGuid(BundleBase.Property_CompanyId, bundleObject.CompanyId));
			AddParameter(cmd, pNVarChar(BundleBase.Property_Name, 250, bundleObject.Name));
			AddParameter(cmd, pNVarChar(BundleBase.Property_SKU, 50, bundleObject.SKU));
			AddParameter(cmd, pNVarChar(BundleBase.Property_Info, bundleObject.Info));
			AddParameter(cmd, pBool(BundleBase.Property_IsDisplay, bundleObject.IsDisplay));
			AddParameter(cmd, pDateTime(BundleBase.Property_LastUpdatedDate, bundleObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(BundleBase.Property_LastUpdatedBy, 50, bundleObject.LastUpdatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Bundle
        /// </summary>
        /// <param name="bundleObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BundleBase bundleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBUNDLE);
	
				AddParameter(cmd, pInt32Out(BundleBase.Property_Id));
				AddCommonParams(cmd, bundleObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					bundleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					bundleObject.Id = (Int32)GetOutParameter(cmd, BundleBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(bundleObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Bundle
        /// </summary>
        /// <param name="bundleObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BundleBase bundleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBUNDLE);
				
				AddParameter(cmd, pInt32(BundleBase.Property_Id, bundleObject.Id));
				AddCommonParams(cmd, bundleObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					bundleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(bundleObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Bundle
        /// </summary>
        /// <param name="Id">Id of the Bundle object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBUNDLE);	
				
				AddParameter(cmd, pInt32(BundleBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Bundle), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Bundle object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Bundle object to retrieve</param>
        /// <returns>Bundle object, null if not found</returns>
		public Bundle Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBUNDLEBYID))
			{
				AddParameter( cmd, pInt32(BundleBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Bundle objects 
        /// </summary>
        /// <returns>A list of Bundle objects</returns>
		public BundleList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBUNDLE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Bundle objects by PageRequest
        /// </summary>
        /// <returns>A list of Bundle objects</returns>
		public BundleList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBUNDLE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BundleList _BundleList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BundleList;
			}
		}
		
		/// <summary>
        /// Retrieves all Bundle objects by query String
        /// </summary>
        /// <returns>A list of Bundle objects</returns>
		public BundleList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBUNDLEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Bundle Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Bundle
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBUNDLEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Bundle Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Bundle
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BundleRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBUNDLEROWCOUNT))
			{
				SqlDataReader reader;
				_BundleRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BundleRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Bundle object
        /// </summary>
        /// <param name="bundleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BundleBase bundleObject, SqlDataReader reader, int start)
		{
			
				bundleObject.Id = reader.GetInt32( start + 0 );			
				bundleObject.CompanyId = reader.GetGuid( start + 1 );			
				bundleObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) bundleObject.SKU = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) bundleObject.Info = reader.GetString( start + 4 );			
				bundleObject.IsDisplay = reader.GetBoolean( start + 5 );			
				bundleObject.LastUpdatedDate = reader.GetDateTime( start + 6 );			
				bundleObject.LastUpdatedBy = reader.GetString( start + 7 );			
			FillBaseObject(bundleObject, reader, (start + 8));

			
			bundleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Bundle object
        /// </summary>
        /// <param name="bundleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BundleBase bundleObject, SqlDataReader reader)
		{
			FillObject(bundleObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Bundle object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Bundle object</returns>
		private Bundle GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Bundle bundleObject= new Bundle();
					FillObject(bundleObject, reader);
					return bundleObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Bundle objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Bundle objects</returns>
		private BundleList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Bundle list
			BundleList list = new BundleList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Bundle bundleObject = new Bundle();
					FillObject(bundleObject, reader);

					list.Add(bundleObject);
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
