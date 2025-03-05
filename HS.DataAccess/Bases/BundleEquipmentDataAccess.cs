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
	public partial class BundleEquipmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBUNDLEEQUIPMENT = "InsertBundleEquipment";
		private const string UPDATEBUNDLEEQUIPMENT = "UpdateBundleEquipment";
		private const string DELETEBUNDLEEQUIPMENT = "DeleteBundleEquipment";
		private const string GETBUNDLEEQUIPMENTBYID = "GetBundleEquipmentById";
		private const string GETALLBUNDLEEQUIPMENT = "GetAllBundleEquipment";
		private const string GETPAGEDBUNDLEEQUIPMENT = "GetPagedBundleEquipment";
		private const string GETBUNDLEEQUIPMENTMAXIMUMID = "GetBundleEquipmentMaximumId";
		private const string GETBUNDLEEQUIPMENTROWCOUNT = "GetBundleEquipmentRowCount";	
		private const string GETBUNDLEEQUIPMENTBYQUERY = "GetBundleEquipmentByQuery";
		#endregion
		
		#region Constructors
		public BundleEquipmentDataAccess(ClientContext context) : base(context) { }
		public BundleEquipmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="bundleEquipmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, BundleEquipmentBase bundleEquipmentObject)
		{	
			AddParameter(cmd, pGuid(BundleEquipmentBase.Property_CompanyId, bundleEquipmentObject.CompanyId));
			AddParameter(cmd, pInt32(BundleEquipmentBase.Property_BundleId, bundleEquipmentObject.BundleId));
			AddParameter(cmd, pGuid(BundleEquipmentBase.Property_EquipmentId, bundleEquipmentObject.EquipmentId));
			AddParameter(cmd, pBool(BundleEquipmentBase.Property_IsActive, bundleEquipmentObject.IsActive));
			AddParameter(cmd, pDateTime(BundleEquipmentBase.Property_LastUpdatedDate, bundleEquipmentObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(BundleEquipmentBase.Property_LastUpdatedBy, 50, bundleEquipmentObject.LastUpdatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BundleEquipment
        /// </summary>
        /// <param name="bundleEquipmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BundleEquipmentBase bundleEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBUNDLEEQUIPMENT);
	
				AddParameter(cmd, pInt32Out(BundleEquipmentBase.Property_Id));
				AddCommonParams(cmd, bundleEquipmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					bundleEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					bundleEquipmentObject.Id = (Int32)GetOutParameter(cmd, BundleEquipmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(bundleEquipmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BundleEquipment
        /// </summary>
        /// <param name="bundleEquipmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BundleEquipmentBase bundleEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBUNDLEEQUIPMENT);
				
				AddParameter(cmd, pInt32(BundleEquipmentBase.Property_Id, bundleEquipmentObject.Id));
				AddCommonParams(cmd, bundleEquipmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					bundleEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(bundleEquipmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BundleEquipment
        /// </summary>
        /// <param name="Id">Id of the BundleEquipment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBUNDLEEQUIPMENT);	
				
				AddParameter(cmd, pInt32(BundleEquipmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BundleEquipment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BundleEquipment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BundleEquipment object to retrieve</param>
        /// <returns>BundleEquipment object, null if not found</returns>
		public BundleEquipment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBUNDLEEQUIPMENTBYID))
			{
				AddParameter( cmd, pInt32(BundleEquipmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BundleEquipment objects 
        /// </summary>
        /// <returns>A list of BundleEquipment objects</returns>
		public BundleEquipmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBUNDLEEQUIPMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BundleEquipment objects by PageRequest
        /// </summary>
        /// <returns>A list of BundleEquipment objects</returns>
		public BundleEquipmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBUNDLEEQUIPMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BundleEquipmentList _BundleEquipmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BundleEquipmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all BundleEquipment objects by query String
        /// </summary>
        /// <returns>A list of BundleEquipment objects</returns>
		public BundleEquipmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBUNDLEEQUIPMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BundleEquipment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BundleEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBUNDLEEQUIPMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BundleEquipment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BundleEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BundleEquipmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBUNDLEEQUIPMENTROWCOUNT))
			{
				SqlDataReader reader;
				_BundleEquipmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BundleEquipmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BundleEquipment object
        /// </summary>
        /// <param name="bundleEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BundleEquipmentBase bundleEquipmentObject, SqlDataReader reader, int start)
		{
			
				bundleEquipmentObject.Id = reader.GetInt32( start + 0 );			
				bundleEquipmentObject.CompanyId = reader.GetGuid( start + 1 );			
				bundleEquipmentObject.BundleId = reader.GetInt32( start + 2 );			
				bundleEquipmentObject.EquipmentId = reader.GetGuid( start + 3 );			
				bundleEquipmentObject.IsActive = reader.GetBoolean( start + 4 );			
				bundleEquipmentObject.LastUpdatedDate = reader.GetDateTime( start + 5 );			
				bundleEquipmentObject.LastUpdatedBy = reader.GetString( start + 6 );			
			FillBaseObject(bundleEquipmentObject, reader, (start + 7));

			
			bundleEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BundleEquipment object
        /// </summary>
        /// <param name="bundleEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BundleEquipmentBase bundleEquipmentObject, SqlDataReader reader)
		{
			FillObject(bundleEquipmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BundleEquipment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BundleEquipment object</returns>
		private BundleEquipment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BundleEquipment bundleEquipmentObject= new BundleEquipment();
					FillObject(bundleEquipmentObject, reader);
					return bundleEquipmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BundleEquipment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BundleEquipment objects</returns>
		private BundleEquipmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BundleEquipment list
			BundleEquipmentList list = new BundleEquipmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BundleEquipment bundleEquipmentObject = new BundleEquipment();
					FillObject(bundleEquipmentObject, reader);

					list.Add(bundleEquipmentObject);
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
