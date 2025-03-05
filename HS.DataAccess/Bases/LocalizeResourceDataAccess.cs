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
	public partial class LocalizeResourceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTLOCALIZERESOURCE = "InsertLocalizeResource";
		private const string UPDATELOCALIZERESOURCE = "UpdateLocalizeResource";
		private const string DELETELOCALIZERESOURCE = "DeleteLocalizeResource";
		private const string GETLOCALIZERESOURCEBYID = "GetLocalizeResourceById";
		private const string GETALLLOCALIZERESOURCE = "GetAllLocalizeResource";
		private const string GETPAGEDLOCALIZERESOURCE = "GetPagedLocalizeResource";
		private const string GETLOCALIZERESOURCEMAXIMUMID = "GetLocalizeResourceMaximumId";
		private const string GETLOCALIZERESOURCEROWCOUNT = "GetLocalizeResourceRowCount";	
		private const string GETLOCALIZERESOURCEBYQUERY = "GetLocalizeResourceByQuery";
        #endregion

        #region Constructors
        public LocalizeResourceDataAccess() { }
		public LocalizeResourceDataAccess(ClientContext context) : base(context) { }
		public LocalizeResourceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="localizeResourceObject"></param>
		private void AddCommonParams(SqlCommand cmd, LocalizeResourceBase localizeResourceObject)
		{	
			AddParameter(cmd, pGuid(LocalizeResourceBase.Property_CompanyId, localizeResourceObject.CompanyId));
			AddParameter(cmd, pInt32(LocalizeResourceBase.Property_LanguageId, localizeResourceObject.LanguageId));
			AddParameter(cmd, pNVarChar(LocalizeResourceBase.Property_ResourceName, 200, localizeResourceObject.ResourceName));
			AddParameter(cmd, pNVarChar(LocalizeResourceBase.Property_ResourceValue, localizeResourceObject.ResourceValue));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts LocalizeResource
        /// </summary>
        /// <param name="localizeResourceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(LocalizeResourceBase localizeResourceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTLOCALIZERESOURCE);
	
				AddParameter(cmd, pInt32Out(LocalizeResourceBase.Property_Id));
				AddCommonParams(cmd, localizeResourceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					localizeResourceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					localizeResourceObject.Id = (Int32)GetOutParameter(cmd, LocalizeResourceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(localizeResourceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates LocalizeResource
        /// </summary>
        /// <param name="localizeResourceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(LocalizeResourceBase localizeResourceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATELOCALIZERESOURCE);
				
				AddParameter(cmd, pInt32(LocalizeResourceBase.Property_Id, localizeResourceObject.Id));
				AddCommonParams(cmd, localizeResourceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					localizeResourceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(localizeResourceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes LocalizeResource
        /// </summary>
        /// <param name="Id">Id of the LocalizeResource object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETELOCALIZERESOURCE);	
				
				AddParameter(cmd, pInt32(LocalizeResourceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(LocalizeResource), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves LocalizeResource object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the LocalizeResource object to retrieve</param>
        /// <returns>LocalizeResource object, null if not found</returns>
		public LocalizeResource Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETLOCALIZERESOURCEBYID))
			{
				AddParameter( cmd, pInt32(LocalizeResourceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all LocalizeResource objects 
        /// </summary>
        /// <returns>A list of LocalizeResource objects</returns>
		public LocalizeResourceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLLOCALIZERESOURCE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all LocalizeResource objects by PageRequest
        /// </summary>
        /// <returns>A list of LocalizeResource objects</returns>
		public LocalizeResourceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDLOCALIZERESOURCE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				LocalizeResourceList _LocalizeResourceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _LocalizeResourceList;
			}
		}
		
		/// <summary>
        /// Retrieves all LocalizeResource objects by query String
        /// </summary>
        /// <returns>A list of LocalizeResource objects</returns>
		public LocalizeResourceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETLOCALIZERESOURCEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get LocalizeResource Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of LocalizeResource
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETLOCALIZERESOURCEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get LocalizeResource Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of LocalizeResource
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _LocalizeResourceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETLOCALIZERESOURCEROWCOUNT))
			{
				SqlDataReader reader;
				_LocalizeResourceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _LocalizeResourceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills LocalizeResource object
        /// </summary>
        /// <param name="localizeResourceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(LocalizeResourceBase localizeResourceObject, SqlDataReader reader, int start)
		{
			
				localizeResourceObject.Id = reader.GetInt32( start + 0 );			
				localizeResourceObject.CompanyId = reader.GetGuid( start + 1 );			
				localizeResourceObject.LanguageId = reader.GetInt32( start + 2 );			
				localizeResourceObject.ResourceName = reader.GetString( start + 3 );			
				localizeResourceObject.ResourceValue = reader.GetString( start + 4 );			
			FillBaseObject(localizeResourceObject, reader, (start + 5));

			
			localizeResourceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills LocalizeResource object
        /// </summary>
        /// <param name="localizeResourceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(LocalizeResourceBase localizeResourceObject, SqlDataReader reader)
		{
			FillObject(localizeResourceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves LocalizeResource object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>LocalizeResource object</returns>
		private LocalizeResource GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					LocalizeResource localizeResourceObject= new LocalizeResource();
					FillObject(localizeResourceObject, reader);
					return localizeResourceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of LocalizeResource objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of LocalizeResource objects</returns>
		private LocalizeResourceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//LocalizeResource list
			LocalizeResourceList list = new LocalizeResourceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					LocalizeResource localizeResourceObject = new LocalizeResource();
					FillObject(localizeResourceObject, reader);

					list.Add(localizeResourceObject);
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
