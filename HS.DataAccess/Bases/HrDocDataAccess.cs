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
	public partial class HrDocDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTHRDOC = "InsertHrDoc";
		private const string UPDATEHRDOC = "UpdateHrDoc";
		private const string DELETEHRDOC = "DeleteHrDoc";
		private const string GETHRDOCBYID = "GetHrDocById";
		private const string GETALLHRDOC = "GetAllHrDoc";
		private const string GETPAGEDHRDOC = "GetPagedHrDoc";
		private const string GETHRDOCMAXIMUMID = "GetHrDocMaximumId";
		private const string GETHRDOCROWCOUNT = "GetHrDocRowCount";	
		private const string GETHRDOCBYQUERY = "GetHrDocByQuery";
		#endregion
		
		#region Constructors
		public HrDocDataAccess(ClientContext context) : base(context) { }
		public HrDocDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="hrDocObject"></param>
		private void AddCommonParams(SqlCommand cmd, HrDocBase hrDocObject)
		{	
			AddParameter(cmd, pNVarChar(HrDocBase.Property_FileDescription, hrDocObject.FileDescription));
			AddParameter(cmd, pNVarChar(HrDocBase.Property_Filename, 500, hrDocObject.Filename));
			AddParameter(cmd, pDateTime(HrDocBase.Property_Uploadeddate, hrDocObject.Uploadeddate));
			AddParameter(cmd, pNVarChar(HrDocBase.Property_UserName, 50, hrDocObject.UserName));
			AddParameter(cmd, pGuid(HrDocBase.Property_CompanyId, hrDocObject.CompanyId));
			AddParameter(cmd, pGuid(HrDocBase.Property_CreatedBy, hrDocObject.CreatedBy));
			AddParameter(cmd, pDateTime(HrDocBase.Property_CreatedDate, hrDocObject.CreatedDate));
			AddParameter(cmd, pNVarChar(HrDocBase.Property_Category, 50, hrDocObject.Category));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts HrDoc
        /// </summary>
        /// <param name="hrDocObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(HrDocBase hrDocObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTHRDOC);
	
				AddParameter(cmd, pInt32Out(HrDocBase.Property_Id));
				AddCommonParams(cmd, hrDocObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					hrDocObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					hrDocObject.Id = (Int32)GetOutParameter(cmd, HrDocBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(hrDocObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates HrDoc
        /// </summary>
        /// <param name="hrDocObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(HrDocBase hrDocObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEHRDOC);
				
				AddParameter(cmd, pInt32(HrDocBase.Property_Id, hrDocObject.Id));
				AddCommonParams(cmd, hrDocObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					hrDocObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(hrDocObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes HrDoc
        /// </summary>
        /// <param name="Id">Id of the HrDoc object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEHRDOC);	
				
				AddParameter(cmd, pInt32(HrDocBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(HrDoc), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves HrDoc object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the HrDoc object to retrieve</param>
        /// <returns>HrDoc object, null if not found</returns>
		public HrDoc Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETHRDOCBYID))
			{
				AddParameter( cmd, pInt32(HrDocBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all HrDoc objects 
        /// </summary>
        /// <returns>A list of HrDoc objects</returns>
		public HrDocList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLHRDOC))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all HrDoc objects by PageRequest
        /// </summary>
        /// <returns>A list of HrDoc objects</returns>
		public HrDocList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDHRDOC))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				HrDocList _HrDocList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _HrDocList;
			}
		}
		
		/// <summary>
        /// Retrieves all HrDoc objects by query String
        /// </summary>
        /// <returns>A list of HrDoc objects</returns>
		public HrDocList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETHRDOCBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get HrDoc Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of HrDoc
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETHRDOCMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get HrDoc Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of HrDoc
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _HrDocRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETHRDOCROWCOUNT))
			{
				SqlDataReader reader;
				_HrDocRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _HrDocRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills HrDoc object
        /// </summary>
        /// <param name="hrDocObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(HrDocBase hrDocObject, SqlDataReader reader, int start)
		{
			
				hrDocObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) hrDocObject.FileDescription = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) hrDocObject.Filename = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) hrDocObject.Uploadeddate = reader.GetDateTime( start + 3 );			
				hrDocObject.UserName = reader.GetString( start + 4 );			
				hrDocObject.CompanyId = reader.GetGuid( start + 5 );			
				hrDocObject.CreatedBy = reader.GetGuid( start + 6 );			
				hrDocObject.CreatedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) hrDocObject.Category = reader.GetString( start + 8 );			
			FillBaseObject(hrDocObject, reader, (start + 9));

			
			hrDocObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills HrDoc object
        /// </summary>
        /// <param name="hrDocObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(HrDocBase hrDocObject, SqlDataReader reader)
		{
			FillObject(hrDocObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves HrDoc object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>HrDoc object</returns>
		private HrDoc GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					HrDoc hrDocObject= new HrDoc();
					FillObject(hrDocObject, reader);
					return hrDocObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of HrDoc objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of HrDoc objects</returns>
		private HrDocList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//HrDoc list
			HrDocList list = new HrDocList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					HrDoc hrDocObject = new HrDoc();
					FillObject(hrDocObject, reader);

					list.Add(hrDocObject);
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
