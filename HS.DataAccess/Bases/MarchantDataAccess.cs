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
	public partial class MarchantDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMARCHANT = "InsertMarchant";
		private const string UPDATEMARCHANT = "UpdateMarchant";
		private const string DELETEMARCHANT = "DeleteMarchant";
		private const string GETMARCHANTBYID = "GetMarchantById";
		private const string GETALLMARCHANT = "GetAllMarchant";
		private const string GETPAGEDMARCHANT = "GetPagedMarchant";
		private const string GETMARCHANTMAXIMUMID = "GetMarchantMaximumId";
		private const string GETMARCHANTROWCOUNT = "GetMarchantRowCount";	
		private const string GETMARCHANTBYQUERY = "GetMarchantByQuery";
		#endregion
		
		#region Constructors
		public MarchantDataAccess(ClientContext context) : base(context) { }
		public MarchantDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="marchantObject"></param>
		private void AddCommonParams(SqlCommand cmd, MarchantBase marchantObject)
		{	
			AddParameter(cmd, pGuid(MarchantBase.Property_CompanyId, marchantObject.CompanyId));
			AddParameter(cmd, pNVarChar(MarchantBase.Property_Name, 50, marchantObject.Name));
			AddParameter(cmd, pInt32(MarchantBase.Property_OrderBy, marchantObject.OrderBy));
			AddParameter(cmd, pBool(MarchantBase.Property_IsActive, marchantObject.IsActive));
			AddParameter(cmd, pBool(MarchantBase.Property_IsDefault, marchantObject.IsDefault));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Marchant
        /// </summary>
        /// <param name="marchantObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MarchantBase marchantObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMARCHANT);
	
				AddParameter(cmd, pInt32Out(MarchantBase.Property_Id));
				AddCommonParams(cmd, marchantObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					marchantObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					marchantObject.Id = (Int32)GetOutParameter(cmd, MarchantBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(marchantObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Marchant
        /// </summary>
        /// <param name="marchantObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MarchantBase marchantObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMARCHANT);
				
				AddParameter(cmd, pInt32(MarchantBase.Property_Id, marchantObject.Id));
				AddCommonParams(cmd, marchantObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					marchantObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(marchantObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Marchant
        /// </summary>
        /// <param name="Id">Id of the Marchant object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMARCHANT);	
				
				AddParameter(cmd, pInt32(MarchantBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Marchant), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Marchant object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Marchant object to retrieve</param>
        /// <returns>Marchant object, null if not found</returns>
		public Marchant Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTBYID))
			{
				AddParameter( cmd, pInt32(MarchantBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Marchant objects 
        /// </summary>
        /// <returns>A list of Marchant objects</returns>
		public MarchantList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMARCHANT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Marchant objects by PageRequest
        /// </summary>
        /// <returns>A list of Marchant objects</returns>
		public MarchantList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMARCHANT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MarchantList _MarchantList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MarchantList;
			}
		}
		
		/// <summary>
        /// Retrieves all Marchant objects by query String
        /// </summary>
        /// <returns>A list of Marchant objects</returns>
		public MarchantList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Marchant Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Marchant
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Marchant Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Marchant
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MarchantRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTROWCOUNT))
			{
				SqlDataReader reader;
				_MarchantRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MarchantRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Marchant object
        /// </summary>
        /// <param name="marchantObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MarchantBase marchantObject, SqlDataReader reader, int start)
		{
			
				marchantObject.Id = reader.GetInt32( start + 0 );			
				marchantObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) marchantObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) marchantObject.OrderBy = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) marchantObject.IsActive = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) marchantObject.IsDefault = reader.GetBoolean( start + 5 );			
			FillBaseObject(marchantObject, reader, (start + 6));

			
			marchantObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Marchant object
        /// </summary>
        /// <param name="marchantObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MarchantBase marchantObject, SqlDataReader reader)
		{
			FillObject(marchantObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Marchant object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Marchant object</returns>
		private Marchant GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Marchant marchantObject= new Marchant();
					FillObject(marchantObject, reader);
					return marchantObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Marchant objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Marchant objects</returns>
		private MarchantList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Marchant list
			MarchantList list = new MarchantList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Marchant marchantObject = new Marchant();
					FillObject(marchantObject, reader);

					list.Add(marchantObject);
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
