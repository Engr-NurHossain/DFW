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
	public partial class RMRTagDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRMRTAG = "InsertRMRTag";
		private const string UPDATERMRTAG = "UpdateRMRTag";
		private const string DELETERMRTAG = "DeleteRMRTag";
		private const string GETRMRTAGBYID = "GetRMRTagById";
		private const string GETALLRMRTAG = "GetAllRMRTag";
		private const string GETPAGEDRMRTAG = "GetPagedRMRTag";
		private const string GETRMRTAGMAXIMUMID = "GetRMRTagMaximumId";
		private const string GETRMRTAGROWCOUNT = "GetRMRTagRowCount";	
		private const string GETRMRTAGBYQUERY = "GetRMRTagByQuery";
		#endregion
		
		#region Constructors
		public RMRTagDataAccess(ClientContext context) : base(context) { }
		public RMRTagDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="rMRTagObject"></param>
		private void AddCommonParams(SqlCommand cmd, RMRTagBase rMRTagObject)
		{	
			AddParameter(cmd, pGuid(RMRTagBase.Property_TagIdentifier, rMRTagObject.TagIdentifier));
			AddParameter(cmd, pNVarChar(RMRTagBase.Property_TagName, 150, rMRTagObject.TagName));
			AddParameter(cmd, pDateTime(RMRTagBase.Property_CreatedDate, rMRTagObject.CreatedDate));
			AddParameter(cmd, pGuid(RMRTagBase.Property_CreatedBy, rMRTagObject.CreatedBy));
			AddParameter(cmd, pDateTime(RMRTagBase.Property_LastUpdatedDate, rMRTagObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(RMRTagBase.Property_LastUpdatedBy, rMRTagObject.LastUpdatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RMRTag
        /// </summary>
        /// <param name="rMRTagObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RMRTagBase rMRTagObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRMRTAG);
	
				AddParameter(cmd, pInt32Out(RMRTagBase.Property_Id));
				AddCommonParams(cmd, rMRTagObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					rMRTagObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					rMRTagObject.Id = (Int32)GetOutParameter(cmd, RMRTagBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(rMRTagObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RMRTag
        /// </summary>
        /// <param name="rMRTagObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RMRTagBase rMRTagObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERMRTAG);
				
				AddParameter(cmd, pInt32(RMRTagBase.Property_Id, rMRTagObject.Id));
				AddCommonParams(cmd, rMRTagObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					rMRTagObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(rMRTagObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RMRTag
        /// </summary>
        /// <param name="Id">Id of the RMRTag object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERMRTAG);	
				
				AddParameter(cmd, pInt32(RMRTagBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RMRTag), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RMRTag object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RMRTag object to retrieve</param>
        /// <returns>RMRTag object, null if not found</returns>
		public RMRTag Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRMRTAGBYID))
			{
				AddParameter( cmd, pInt32(RMRTagBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RMRTag objects 
        /// </summary>
        /// <returns>A list of RMRTag objects</returns>
		public RMRTagList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRMRTAG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RMRTag objects by PageRequest
        /// </summary>
        /// <returns>A list of RMRTag objects</returns>
		public RMRTagList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRMRTAG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RMRTagList _RMRTagList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RMRTagList;
			}
		}
		
		/// <summary>
        /// Retrieves all RMRTag objects by query String
        /// </summary>
        /// <returns>A list of RMRTag objects</returns>
		public RMRTagList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRMRTAGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RMRTag Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RMRTag
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRMRTAGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RMRTag Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RMRTag
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RMRTagRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRMRTAGROWCOUNT))
			{
				SqlDataReader reader;
				_RMRTagRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RMRTagRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RMRTag object
        /// </summary>
        /// <param name="rMRTagObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RMRTagBase rMRTagObject, SqlDataReader reader, int start)
		{
			
				rMRTagObject.Id = reader.GetInt32( start + 0 );			
				rMRTagObject.TagIdentifier = reader.GetGuid( start + 1 );			
				rMRTagObject.TagName = reader.GetString( start + 2 );			
				rMRTagObject.CreatedDate = reader.GetDateTime( start + 3 );			
				rMRTagObject.CreatedBy = reader.GetGuid( start + 4 );			
				rMRTagObject.LastUpdatedDate = reader.GetDateTime( start + 5 );			
				rMRTagObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
			FillBaseObject(rMRTagObject, reader, (start + 7));

			
			rMRTagObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RMRTag object
        /// </summary>
        /// <param name="rMRTagObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RMRTagBase rMRTagObject, SqlDataReader reader)
		{
			FillObject(rMRTagObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RMRTag object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RMRTag object</returns>
		private RMRTag GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RMRTag rMRTagObject= new RMRTag();
					FillObject(rMRTagObject, reader);
					return rMRTagObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RMRTag objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RMRTag objects</returns>
		private RMRTagList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RMRTag list
			RMRTagList list = new RMRTagList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RMRTag rMRTagObject = new RMRTag();
					FillObject(rMRTagObject, reader);

					list.Add(rMRTagObject);
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
