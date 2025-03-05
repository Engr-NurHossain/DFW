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
	public partial class GeeseRouteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTGEESEROUTE = "InsertGeeseRoute";
		private const string UPDATEGEESEROUTE = "UpdateGeeseRoute";
		private const string DELETEGEESEROUTE = "DeleteGeeseRoute";
		private const string GETGEESEROUTEBYID = "GetGeeseRouteById";
		private const string GETALLGEESEROUTE = "GetAllGeeseRoute";
		private const string GETPAGEDGEESEROUTE = "GetPagedGeeseRoute";
		private const string GETGEESEROUTEMAXIMUMID = "GetGeeseRouteMaximumId";
		private const string GETGEESEROUTEROWCOUNT = "GetGeeseRouteRowCount";	
		private const string GETGEESEROUTEBYQUERY = "GetGeeseRouteByQuery";
		#endregion
		
		#region Constructors
		public GeeseRouteDataAccess(ClientContext context) : base(context) { }
		public GeeseRouteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="geeseRouteObject"></param>
		private void AddCommonParams(SqlCommand cmd, GeeseRouteBase geeseRouteObject)
		{	
			AddParameter(cmd, pNVarChar(GeeseRouteBase.Property_Name, 50, geeseRouteObject.Name));
			AddParameter(cmd, pGuid(GeeseRouteBase.Property_RouteId, geeseRouteObject.RouteId));
			AddParameter(cmd, pGuid(GeeseRouteBase.Property_CreatedBy, geeseRouteObject.CreatedBy));
			AddParameter(cmd, pDateTime(GeeseRouteBase.Property_CreatedDate, geeseRouteObject.CreatedDate));
			AddParameter(cmd, pGuid(GeeseRouteBase.Property_UpdatedBy, geeseRouteObject.UpdatedBy));
			AddParameter(cmd, pDateTime(GeeseRouteBase.Property_UpdatedDate, geeseRouteObject.UpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts GeeseRoute
        /// </summary>
        /// <param name="geeseRouteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(GeeseRouteBase geeseRouteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTGEESEROUTE);
	
				AddParameter(cmd, pInt32Out(GeeseRouteBase.Property_Id));
				AddCommonParams(cmd, geeseRouteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					geeseRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					geeseRouteObject.Id = (Int32)GetOutParameter(cmd, GeeseRouteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(geeseRouteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates GeeseRoute
        /// </summary>
        /// <param name="geeseRouteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(GeeseRouteBase geeseRouteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEGEESEROUTE);
				
				AddParameter(cmd, pInt32(GeeseRouteBase.Property_Id, geeseRouteObject.Id));
				AddCommonParams(cmd, geeseRouteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					geeseRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(geeseRouteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes GeeseRoute
        /// </summary>
        /// <param name="Id">Id of the GeeseRoute object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEGEESEROUTE);	
				
				AddParameter(cmd, pInt32(GeeseRouteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(GeeseRoute), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves GeeseRoute object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the GeeseRoute object to retrieve</param>
        /// <returns>GeeseRoute object, null if not found</returns>
		public GeeseRoute Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETGEESEROUTEBYID))
			{
				AddParameter( cmd, pInt32(GeeseRouteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all GeeseRoute objects 
        /// </summary>
        /// <returns>A list of GeeseRoute objects</returns>
		public GeeseRouteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLGEESEROUTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all GeeseRoute objects by PageRequest
        /// </summary>
        /// <returns>A list of GeeseRoute objects</returns>
		public GeeseRouteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDGEESEROUTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				GeeseRouteList _GeeseRouteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _GeeseRouteList;
			}
		}
		
		/// <summary>
        /// Retrieves all GeeseRoute objects by query String
        /// </summary>
        /// <returns>A list of GeeseRoute objects</returns>
		public GeeseRouteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETGEESEROUTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get GeeseRoute Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of GeeseRoute
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETGEESEROUTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get GeeseRoute Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of GeeseRoute
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _GeeseRouteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETGEESEROUTEROWCOUNT))
			{
				SqlDataReader reader;
				_GeeseRouteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _GeeseRouteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills GeeseRoute object
        /// </summary>
        /// <param name="geeseRouteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(GeeseRouteBase geeseRouteObject, SqlDataReader reader, int start)
		{
			
				geeseRouteObject.Id = reader.GetInt32( start + 0 );			
				geeseRouteObject.Name = reader.GetString( start + 1 );			
				geeseRouteObject.RouteId = reader.GetGuid( start + 2 );			
				geeseRouteObject.CreatedBy = reader.GetGuid( start + 3 );			
				geeseRouteObject.CreatedDate = reader.GetDateTime( start + 4 );			
				geeseRouteObject.UpdatedBy = reader.GetGuid( start + 5 );			
				geeseRouteObject.UpdatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(geeseRouteObject, reader, (start + 7));

			
			geeseRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills GeeseRoute object
        /// </summary>
        /// <param name="geeseRouteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(GeeseRouteBase geeseRouteObject, SqlDataReader reader)
		{
			FillObject(geeseRouteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves GeeseRoute object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>GeeseRoute object</returns>
		private GeeseRoute GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					GeeseRoute geeseRouteObject= new GeeseRoute();
					FillObject(geeseRouteObject, reader);
					return geeseRouteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of GeeseRoute objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of GeeseRoute objects</returns>
		private GeeseRouteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//GeeseRoute list
			GeeseRouteList list = new GeeseRouteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					GeeseRoute geeseRouteObject = new GeeseRoute();
					FillObject(geeseRouteObject, reader);

					list.Add(geeseRouteObject);
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
