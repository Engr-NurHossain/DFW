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
	public partial class BillFileDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBILLFILE = "InsertBillFile";
		private const string UPDATEBILLFILE = "UpdateBillFile";
		private const string DELETEBILLFILE = "DeleteBillFile";
		private const string GETBILLFILEBYID = "GetBillFileById";
		private const string GETALLBILLFILE = "GetAllBillFile";
		private const string GETPAGEDBILLFILE = "GetPagedBillFile";
		private const string GETBILLFILEMAXIMUMID = "GetBillFileMaximumId";
		private const string GETBILLFILEROWCOUNT = "GetBillFileRowCount";	
		private const string GETBILLFILEBYQUERY = "GetBillFileByQuery";
		#endregion
		
		#region Constructors
		public BillFileDataAccess(ClientContext context) : base(context) { }
		public BillFileDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="billFileObject"></param>
		private void AddCommonParams(SqlCommand cmd, BillFileBase billFileObject)
		{	
			AddParameter(cmd, pNVarChar(BillFileBase.Property_FileDescription, billFileObject.FileDescription));
			AddParameter(cmd, pNVarChar(BillFileBase.Property_Filename, 500, billFileObject.Filename));
			AddParameter(cmd, pNVarChar(BillFileBase.Property_FileFullName, 500, billFileObject.FileFullName));
			AddParameter(cmd, pDateTime(BillFileBase.Property_Uploadeddate, billFileObject.Uploadeddate));
			AddParameter(cmd, pNVarChar(BillFileBase.Property_BillNo, 500, billFileObject.BillNo));
			AddParameter(cmd, pGuid(BillFileBase.Property_CompanyId, billFileObject.CompanyId));
			AddParameter(cmd, pBool(BillFileBase.Property_IsActive, billFileObject.IsActive));
			AddParameter(cmd, pDouble(BillFileBase.Property_FileSize, billFileObject.FileSize));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BillFile
        /// </summary>
        /// <param name="billFileObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BillFileBase billFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBILLFILE);
	
				AddParameter(cmd, pInt32Out(BillFileBase.Property_Id));
				AddCommonParams(cmd, billFileObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					billFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					billFileObject.Id = (Int32)GetOutParameter(cmd, BillFileBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(billFileObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BillFile
        /// </summary>
        /// <param name="billFileObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BillFileBase billFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBILLFILE);
				
				AddParameter(cmd, pInt32(BillFileBase.Property_Id, billFileObject.Id));
				AddCommonParams(cmd, billFileObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					billFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(billFileObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BillFile
        /// </summary>
        /// <param name="Id">Id of the BillFile object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBILLFILE);	
				
				AddParameter(cmd, pInt32(BillFileBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BillFile), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BillFile object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BillFile object to retrieve</param>
        /// <returns>BillFile object, null if not found</returns>
		public BillFile Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLFILEBYID))
			{
				AddParameter( cmd, pInt32(BillFileBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BillFile objects 
        /// </summary>
        /// <returns>A list of BillFile objects</returns>
		public BillFileList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBILLFILE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BillFile objects by PageRequest
        /// </summary>
        /// <returns>A list of BillFile objects</returns>
		public BillFileList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBILLFILE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BillFileList _BillFileList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BillFileList;
			}
		}
		
		/// <summary>
        /// Retrieves all BillFile objects by query String
        /// </summary>
        /// <returns>A list of BillFile objects</returns>
		public BillFileList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLFILEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BillFile Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BillFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLFILEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BillFile Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BillFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BillFileRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLFILEROWCOUNT))
			{
				SqlDataReader reader;
				_BillFileRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BillFileRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BillFile object
        /// </summary>
        /// <param name="billFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BillFileBase billFileObject, SqlDataReader reader, int start)
		{
			
				billFileObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) billFileObject.FileDescription = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) billFileObject.Filename = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) billFileObject.FileFullName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) billFileObject.Uploadeddate = reader.GetDateTime( start + 4 );			
				billFileObject.BillNo = reader.GetString( start + 5 );			
				billFileObject.CompanyId = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) billFileObject.IsActive = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) billFileObject.FileSize = reader.GetDouble( start + 8 );			
			FillBaseObject(billFileObject, reader, (start + 9));

			
			billFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BillFile object
        /// </summary>
        /// <param name="billFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BillFileBase billFileObject, SqlDataReader reader)
		{
			FillObject(billFileObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BillFile object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BillFile object</returns>
		private BillFile GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BillFile billFileObject= new BillFile();
					FillObject(billFileObject, reader);
					return billFileObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BillFile objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BillFile objects</returns>
		private BillFileList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BillFile list
			BillFileList list = new BillFileList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BillFile billFileObject = new BillFile();
					FillObject(billFileObject, reader);

					list.Add(billFileObject);
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
