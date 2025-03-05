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
	public partial class AgemniEmployeeMapperDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTAGEMNIEMPLOYEEMAPPER = "InsertAgemniEmployeeMapper";
		private const string UPDATEAGEMNIEMPLOYEEMAPPER = "UpdateAgemniEmployeeMapper";
		private const string DELETEAGEMNIEMPLOYEEMAPPER = "DeleteAgemniEmployeeMapper";
		private const string GETAGEMNIEMPLOYEEMAPPERBYID = "GetAgemniEmployeeMapperByID";
		private const string GETALLAGEMNIEMPLOYEEMAPPER = "GetAllAgemniEmployeeMapper";
		private const string GETPAGEDAGEMNIEMPLOYEEMAPPER = "GetPagedAgemniEmployeeMapper";
		private const string GETAGEMNIEMPLOYEEMAPPERMAXIMUMID = "GetAgemniEmployeeMapperMaximumID";
		private const string GETAGEMNIEMPLOYEEMAPPERROWCOUNT = "GetAgemniEmployeeMapperRowCount";	
		private const string GETAGEMNIEMPLOYEEMAPPERBYQUERY = "GetAgemniEmployeeMapperByQuery";
		#endregion
		
		#region Constructors
		public AgemniEmployeeMapperDataAccess(ClientContext context) : base(context) { }
		public AgemniEmployeeMapperDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="agemniEmployeeMapperObject"></param>
		private void AddCommonParams(SqlCommand cmd, AgemniEmployeeMapperBase agemniEmployeeMapperObject)
		{	
			AddParameter(cmd, pInt32(AgemniEmployeeMapperBase.Property_AgID, agemniEmployeeMapperObject.AgID));
			AddParameter(cmd, pGuid(AgemniEmployeeMapperBase.Property_RMRID, agemniEmployeeMapperObject.RMRID));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AgemniEmployeeMapper
        /// </summary>
        /// <param name="agemniEmployeeMapperObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AgemniEmployeeMapperBase agemniEmployeeMapperObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTAGEMNIEMPLOYEEMAPPER);
	
				AddParameter(cmd, pInt32Out(AgemniEmployeeMapperBase.Property_ID));
				AddCommonParams(cmd, agemniEmployeeMapperObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					agemniEmployeeMapperObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					agemniEmployeeMapperObject.ID = (Int32)GetOutParameter(cmd, AgemniEmployeeMapperBase.Property_ID);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(agemniEmployeeMapperObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AgemniEmployeeMapper
        /// </summary>
        /// <param name="agemniEmployeeMapperObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AgemniEmployeeMapperBase agemniEmployeeMapperObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEAGEMNIEMPLOYEEMAPPER);
				
				AddParameter(cmd, pInt32(AgemniEmployeeMapperBase.Property_ID, agemniEmployeeMapperObject.ID));
				AddCommonParams(cmd, agemniEmployeeMapperObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					agemniEmployeeMapperObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(agemniEmployeeMapperObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AgemniEmployeeMapper
        /// </summary>
        /// <param name="ID">ID of the AgemniEmployeeMapper object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _ID)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEAGEMNIEMPLOYEEMAPPER);	
				
				AddParameter(cmd, pInt32(AgemniEmployeeMapperBase.Property_ID, _ID));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AgemniEmployeeMapper), _ID, x);
			}
			
		}
		#endregion
		
		#region Get By ID Method
		/// <summary>
        /// Retrieves AgemniEmployeeMapper object using it's ID
        /// </summary>
        /// <param name="ID">The ID of the AgemniEmployeeMapper object to retrieve</param>
        /// <returns>AgemniEmployeeMapper object, null if not found</returns>
		public AgemniEmployeeMapper Get(Int32 _ID)
		{
			using( SqlCommand cmd = GetSPCommand(GETAGEMNIEMPLOYEEMAPPERBYID))
			{
				AddParameter( cmd, pInt32(AgemniEmployeeMapperBase.Property_ID, _ID));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AgemniEmployeeMapper objects 
        /// </summary>
        /// <returns>A list of AgemniEmployeeMapper objects</returns>
		public AgemniEmployeeMapperList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLAGEMNIEMPLOYEEMAPPER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AgemniEmployeeMapper objects by PageRequest
        /// </summary>
        /// <returns>A list of AgemniEmployeeMapper objects</returns>
		public AgemniEmployeeMapperList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDAGEMNIEMPLOYEEMAPPER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AgemniEmployeeMapperList _AgemniEmployeeMapperList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AgemniEmployeeMapperList;
			}
		}
		
		/// <summary>
        /// Retrieves all AgemniEmployeeMapper objects by query String
        /// </summary>
        /// <returns>A list of AgemniEmployeeMapper objects</returns>
		public AgemniEmployeeMapperList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETAGEMNIEMPLOYEEMAPPERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AgemniEmployeeMapper Maximum ID Method
		/// <summary>
        /// Retrieves Get Maximum ID of AgemniEmployeeMapper
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxID()
		{
			Int32 _MaximumID = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAGEMNIEMPLOYEEMAPPERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumID = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumID;
		}
		
		#endregion
		
		#region Get AgemniEmployeeMapper Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AgemniEmployeeMapper
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AgemniEmployeeMapperRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAGEMNIEMPLOYEEMAPPERROWCOUNT))
			{
				SqlDataReader reader;
				_AgemniEmployeeMapperRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AgemniEmployeeMapperRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AgemniEmployeeMapper object
        /// </summary>
        /// <param name="agemniEmployeeMapperObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AgemniEmployeeMapperBase agemniEmployeeMapperObject, SqlDataReader reader, int start)
		{
			
				agemniEmployeeMapperObject.ID = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) agemniEmployeeMapperObject.AgID = reader.GetInt32( start + 1 );			
				agemniEmployeeMapperObject.RMRID = reader.GetGuid( start + 2 );			
			FillBaseObject(agemniEmployeeMapperObject, reader, (start + 3));

			
			agemniEmployeeMapperObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AgemniEmployeeMapper object
        /// </summary>
        /// <param name="agemniEmployeeMapperObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AgemniEmployeeMapperBase agemniEmployeeMapperObject, SqlDataReader reader)
		{
			FillObject(agemniEmployeeMapperObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AgemniEmployeeMapper object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AgemniEmployeeMapper object</returns>
		private AgemniEmployeeMapper GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AgemniEmployeeMapper agemniEmployeeMapperObject= new AgemniEmployeeMapper();
					FillObject(agemniEmployeeMapperObject, reader);
					return agemniEmployeeMapperObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AgemniEmployeeMapper objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AgemniEmployeeMapper objects</returns>
		private AgemniEmployeeMapperList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AgemniEmployeeMapper list
			AgemniEmployeeMapperList list = new AgemniEmployeeMapperList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AgemniEmployeeMapper agemniEmployeeMapperObject = new AgemniEmployeeMapper();
					FillObject(agemniEmployeeMapperObject, reader);

					list.Add(agemniEmployeeMapperObject);
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
