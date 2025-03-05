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
	public partial class BrinksSignedInfoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBRINKSSIGNEDINFO = "InsertBrinksSignedInfo";
		private const string UPDATEBRINKSSIGNEDINFO = "UpdateBrinksSignedInfo";
		private const string DELETEBRINKSSIGNEDINFO = "DeleteBrinksSignedInfo";
		private const string GETBRINKSSIGNEDINFOBYID = "GetBrinksSignedInfoById";
		private const string GETALLBRINKSSIGNEDINFO = "GetAllBrinksSignedInfo";
		private const string GETPAGEDBRINKSSIGNEDINFO = "GetPagedBrinksSignedInfo";
		private const string GETBRINKSSIGNEDINFOMAXIMUMID = "GetBrinksSignedInfoMaximumId";
		private const string GETBRINKSSIGNEDINFOROWCOUNT = "GetBrinksSignedInfoRowCount";	
		private const string GETBRINKSSIGNEDINFOBYQUERY = "GetBrinksSignedInfoByQuery";
		#endregion
		
		#region Constructors
		public BrinksSignedInfoDataAccess(ClientContext context) : base(context) { }
		public BrinksSignedInfoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="brinksSignedInfoObject"></param>
		private void AddCommonParams(SqlCommand cmd, BrinksSignedInfoBase brinksSignedInfoObject)
		{	
			AddParameter(cmd, pGuid(BrinksSignedInfoBase.Property_CustomerId, brinksSignedInfoObject.CustomerId));
			AddParameter(cmd, pBool(BrinksSignedInfoBase.Property_IsSigned, brinksSignedInfoObject.IsSigned));
			AddParameter(cmd, pBool(BrinksSignedInfoBase.Property_HasBillingInfo, brinksSignedInfoObject.HasBillingInfo));
			AddParameter(cmd, pBool(BrinksSignedInfoBase.Property_HasBusinessPicture, brinksSignedInfoObject.HasBusinessPicture));
			AddParameter(cmd, pGuid(BrinksSignedInfoBase.Property_CreatedBy, brinksSignedInfoObject.CreatedBy));
			AddParameter(cmd, pDateTime(BrinksSignedInfoBase.Property_CreatedDate, brinksSignedInfoObject.CreatedDate));
			AddParameter(cmd, pBool(BrinksSignedInfoBase.Property_IsCreditCheck, brinksSignedInfoObject.IsCreditCheck));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BrinksSignedInfo
        /// </summary>
        /// <param name="brinksSignedInfoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BrinksSignedInfoBase brinksSignedInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBRINKSSIGNEDINFO);
	
				AddParameter(cmd, pInt32Out(BrinksSignedInfoBase.Property_Id));
				AddCommonParams(cmd, brinksSignedInfoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					brinksSignedInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					brinksSignedInfoObject.Id = (Int32)GetOutParameter(cmd, BrinksSignedInfoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(brinksSignedInfoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BrinksSignedInfo
        /// </summary>
        /// <param name="brinksSignedInfoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BrinksSignedInfoBase brinksSignedInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBRINKSSIGNEDINFO);
				
				AddParameter(cmd, pInt32(BrinksSignedInfoBase.Property_Id, brinksSignedInfoObject.Id));
				AddCommonParams(cmd, brinksSignedInfoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					brinksSignedInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(brinksSignedInfoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BrinksSignedInfo
        /// </summary>
        /// <param name="Id">Id of the BrinksSignedInfo object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBRINKSSIGNEDINFO);	
				
				AddParameter(cmd, pInt32(BrinksSignedInfoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BrinksSignedInfo), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BrinksSignedInfo object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BrinksSignedInfo object to retrieve</param>
        /// <returns>BrinksSignedInfo object, null if not found</returns>
		public BrinksSignedInfo Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBRINKSSIGNEDINFOBYID))
			{
				AddParameter( cmd, pInt32(BrinksSignedInfoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BrinksSignedInfo objects 
        /// </summary>
        /// <returns>A list of BrinksSignedInfo objects</returns>
		public BrinksSignedInfoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBRINKSSIGNEDINFO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BrinksSignedInfo objects by PageRequest
        /// </summary>
        /// <returns>A list of BrinksSignedInfo objects</returns>
		public BrinksSignedInfoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBRINKSSIGNEDINFO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BrinksSignedInfoList _BrinksSignedInfoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BrinksSignedInfoList;
			}
		}
		
		/// <summary>
        /// Retrieves all BrinksSignedInfo objects by query String
        /// </summary>
        /// <returns>A list of BrinksSignedInfo objects</returns>
		public BrinksSignedInfoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBRINKSSIGNEDINFOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BrinksSignedInfo Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BrinksSignedInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBRINKSSIGNEDINFOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BrinksSignedInfo Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BrinksSignedInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BrinksSignedInfoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBRINKSSIGNEDINFOROWCOUNT))
			{
				SqlDataReader reader;
				_BrinksSignedInfoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BrinksSignedInfoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BrinksSignedInfo object
        /// </summary>
        /// <param name="brinksSignedInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BrinksSignedInfoBase brinksSignedInfoObject, SqlDataReader reader, int start)
		{
			
				brinksSignedInfoObject.Id = reader.GetInt32( start + 0 );			
				brinksSignedInfoObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) brinksSignedInfoObject.IsSigned = reader.GetBoolean( start + 2 );			
				if(!reader.IsDBNull(3)) brinksSignedInfoObject.HasBillingInfo = reader.GetBoolean( start + 3 );			
				if(!reader.IsDBNull(4)) brinksSignedInfoObject.HasBusinessPicture = reader.GetBoolean( start + 4 );			
				brinksSignedInfoObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) brinksSignedInfoObject.CreatedDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) brinksSignedInfoObject.IsCreditCheck = reader.GetBoolean( start + 7 );			
			FillBaseObject(brinksSignedInfoObject, reader, (start + 8));

			
			brinksSignedInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BrinksSignedInfo object
        /// </summary>
        /// <param name="brinksSignedInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BrinksSignedInfoBase brinksSignedInfoObject, SqlDataReader reader)
		{
			FillObject(brinksSignedInfoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BrinksSignedInfo object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BrinksSignedInfo object</returns>
		private BrinksSignedInfo GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BrinksSignedInfo brinksSignedInfoObject= new BrinksSignedInfo();
					FillObject(brinksSignedInfoObject, reader);
					return brinksSignedInfoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BrinksSignedInfo objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BrinksSignedInfo objects</returns>
		private BrinksSignedInfoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BrinksSignedInfo list
			BrinksSignedInfoList list = new BrinksSignedInfoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BrinksSignedInfo brinksSignedInfoObject = new BrinksSignedInfo();
					FillObject(brinksSignedInfoObject, reader);

					list.Add(brinksSignedInfoObject);
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
