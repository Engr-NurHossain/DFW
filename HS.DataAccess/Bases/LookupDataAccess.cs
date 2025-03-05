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
	public partial class LookupDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTLOOKUP = "InsertLookup";
		private const string UPDATELOOKUP = "UpdateLookup";
		private const string DELETELOOKUP = "DeleteLookup";
		private const string GETLOOKUPBYID = "GetLookupById";
		private const string GETALLLOOKUP = "GetAllLookup";
		private const string GETPAGEDLOOKUP = "GetPagedLookup";
		private const string GETLOOKUPMAXIMUMID = "GetLookupMaximumId";
		private const string GETLOOKUPROWCOUNT = "GetLookupRowCount";	
		private const string GETLOOKUPBYQUERY = "GetLookupByQuery";
		#endregion
		
		#region Constructors
		public LookupDataAccess(ClientContext context) : base(context) { }
		public LookupDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="lookupObject"></param>
		private void AddCommonParams(SqlCommand cmd, LookupBase lookupObject)
		{	
			AddParameter(cmd, pNVarChar(LookupBase.Property_ParentDataKey, 250, lookupObject.ParentDataKey));
			AddParameter(cmd, pNVarChar(LookupBase.Property_DataKey, 250, lookupObject.DataKey));
			AddParameter(cmd, pNVarChar(LookupBase.Property_DisplayText, 250, lookupObject.DisplayText));
			AddParameter(cmd, pNVarChar(LookupBase.Property_DataValue, 250, lookupObject.DataValue));
			AddParameter(cmd, pInt32(LookupBase.Property_DataOrder, lookupObject.DataOrder));
			AddParameter(cmd, pBool(LookupBase.Property_IsActive, lookupObject.IsActive));
			AddParameter(cmd, pNVarChar(LookupBase.Property_AlterDisplayText, 250, lookupObject.AlterDisplayText));
			AddParameter(cmd, pNVarChar(LookupBase.Property_AlterDisplayText1, 250, lookupObject.AlterDisplayText1));
			AddParameter(cmd, pBool(LookupBase.Property_IsDefaultItem, lookupObject.IsDefaultItem));
			AddParameter(cmd, pGuid(LookupBase.Property_CompanyId, lookupObject.CompanyId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Lookup
        /// </summary>
        /// <param name="lookupObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(LookupBase lookupObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTLOOKUP);
	
				AddParameter(cmd, pInt32Out(LookupBase.Property_Id));
				AddCommonParams(cmd, lookupObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					lookupObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					lookupObject.Id = (Int32)GetOutParameter(cmd, LookupBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(lookupObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Lookup
        /// </summary>
        /// <param name="lookupObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(LookupBase lookupObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATELOOKUP);
				
				AddParameter(cmd, pInt32(LookupBase.Property_Id, lookupObject.Id));
				AddCommonParams(cmd, lookupObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					lookupObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(lookupObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Lookup
        /// </summary>
        /// <param name="Id">Id of the Lookup object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETELOOKUP);	
				
				AddParameter(cmd, pInt32(LookupBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Lookup), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Lookup object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Lookup object to retrieve</param>
        /// <returns>Lookup object, null if not found</returns>
		public Lookup Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETLOOKUPBYID))
			{
				AddParameter( cmd, pInt32(LookupBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Lookup objects 
        /// </summary>
        /// <returns>A list of Lookup objects</returns>
		public LookupList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLLOOKUP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Lookup objects by PageRequest
        /// </summary>
        /// <returns>A list of Lookup objects</returns>
		public LookupList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDLOOKUP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				LookupList _LookupList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _LookupList;
			}
		}
		
		/// <summary>
        /// Retrieves all Lookup objects by query String
        /// </summary>
        /// <returns>A list of Lookup objects</returns>
		public LookupList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETLOOKUPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Lookup Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Lookup
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETLOOKUPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Lookup Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Lookup
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _LookupRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETLOOKUPROWCOUNT))
			{
				SqlDataReader reader;
				_LookupRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _LookupRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Lookup object
        /// </summary>
        /// <param name="lookupObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(LookupBase lookupObject, SqlDataReader reader, int start)
		{
			
				lookupObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) lookupObject.ParentDataKey = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) lookupObject.DataKey = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) lookupObject.DisplayText = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) lookupObject.DataValue = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) lookupObject.DataOrder = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) lookupObject.IsActive = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) lookupObject.AlterDisplayText = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) lookupObject.AlterDisplayText1 = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) lookupObject.IsDefaultItem = reader.GetBoolean( start + 9 );			
				lookupObject.CompanyId = reader.GetGuid( start + 10 );			
			FillBaseObject(lookupObject, reader, (start + 11));

			
			lookupObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Lookup object
        /// </summary>
        /// <param name="lookupObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(LookupBase lookupObject, SqlDataReader reader)
		{
			FillObject(lookupObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Lookup object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Lookup object</returns>
		private Lookup GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Lookup lookupObject= new Lookup();
					FillObject(lookupObject, reader);
					return lookupObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Lookup objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Lookup objects</returns>
		private LookupList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Lookup list
			LookupList list = new LookupList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Lookup lookupObject = new Lookup();
					FillObject(lookupObject, reader);

					list.Add(lookupObject);
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
