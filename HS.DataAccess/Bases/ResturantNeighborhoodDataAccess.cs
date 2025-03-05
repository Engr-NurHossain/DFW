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
	public partial class ResturantNeighborhoodDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTURANTNEIGHBORHOOD = "InsertResturantNeighborhood";
		private const string UPDATERESTURANTNEIGHBORHOOD = "UpdateResturantNeighborhood";
		private const string DELETERESTURANTNEIGHBORHOOD = "DeleteResturantNeighborhood";
		private const string GETRESTURANTNEIGHBORHOODBYID = "GetResturantNeighborhoodById";
		private const string GETALLRESTURANTNEIGHBORHOOD = "GetAllResturantNeighborhood";
		private const string GETPAGEDRESTURANTNEIGHBORHOOD = "GetPagedResturantNeighborhood";
		private const string GETRESTURANTNEIGHBORHOODMAXIMUMID = "GetResturantNeighborhoodMaximumId";
		private const string GETRESTURANTNEIGHBORHOODROWCOUNT = "GetResturantNeighborhoodRowCount";	
		private const string GETRESTURANTNEIGHBORHOODBYQUERY = "GetResturantNeighborhoodByQuery";
		#endregion
		
		#region Constructors
		public ResturantNeighborhoodDataAccess(ClientContext context) : base(context) { }
		public ResturantNeighborhoodDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="resturantNeighborhoodObject"></param>
		private void AddCommonParams(SqlCommand cmd, ResturantNeighborhoodBase resturantNeighborhoodObject)
		{	
			AddParameter(cmd, pInt32(ResturantNeighborhoodBase.Property_SiteLocationId, resturantNeighborhoodObject.SiteLocationId));
			AddParameter(cmd, pNVarChar(ResturantNeighborhoodBase.Property_NeighborhoodName, 250, resturantNeighborhoodObject.NeighborhoodName));
			AddParameter(cmd, pNVarChar(ResturantNeighborhoodBase.Property_NeighborhoodURL, 250, resturantNeighborhoodObject.NeighborhoodURL));
			AddParameter(cmd, pGuid(ResturantNeighborhoodBase.Property_CreatedBy, resturantNeighborhoodObject.CreatedBy));
			AddParameter(cmd, pDateTime(ResturantNeighborhoodBase.Property_CreatedDate, resturantNeighborhoodObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ResturantNeighborhood
        /// </summary>
        /// <param name="resturantNeighborhoodObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ResturantNeighborhoodBase resturantNeighborhoodObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTURANTNEIGHBORHOOD);
	
				AddParameter(cmd, pInt32Out(ResturantNeighborhoodBase.Property_Id));
				AddCommonParams(cmd, resturantNeighborhoodObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					resturantNeighborhoodObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					resturantNeighborhoodObject.Id = (Int32)GetOutParameter(cmd, ResturantNeighborhoodBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(resturantNeighborhoodObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ResturantNeighborhood
        /// </summary>
        /// <param name="resturantNeighborhoodObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ResturantNeighborhoodBase resturantNeighborhoodObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTURANTNEIGHBORHOOD);
				
				AddParameter(cmd, pInt32(ResturantNeighborhoodBase.Property_Id, resturantNeighborhoodObject.Id));
				AddCommonParams(cmd, resturantNeighborhoodObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					resturantNeighborhoodObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(resturantNeighborhoodObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ResturantNeighborhood
        /// </summary>
        /// <param name="Id">Id of the ResturantNeighborhood object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTURANTNEIGHBORHOOD);	
				
				AddParameter(cmd, pInt32(ResturantNeighborhoodBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ResturantNeighborhood), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ResturantNeighborhood object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ResturantNeighborhood object to retrieve</param>
        /// <returns>ResturantNeighborhood object, null if not found</returns>
		public ResturantNeighborhood Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTNEIGHBORHOODBYID))
			{
				AddParameter( cmd, pInt32(ResturantNeighborhoodBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ResturantNeighborhood objects 
        /// </summary>
        /// <returns>A list of ResturantNeighborhood objects</returns>
		public ResturantNeighborhoodList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTURANTNEIGHBORHOOD))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ResturantNeighborhood objects by PageRequest
        /// </summary>
        /// <returns>A list of ResturantNeighborhood objects</returns>
		public ResturantNeighborhoodList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTURANTNEIGHBORHOOD))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ResturantNeighborhoodList _ResturantNeighborhoodList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ResturantNeighborhoodList;
			}
		}
		
		/// <summary>
        /// Retrieves all ResturantNeighborhood objects by query String
        /// </summary>
        /// <returns>A list of ResturantNeighborhood objects</returns>
		public ResturantNeighborhoodList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTNEIGHBORHOODBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ResturantNeighborhood Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ResturantNeighborhood
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTNEIGHBORHOODMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ResturantNeighborhood Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ResturantNeighborhood
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ResturantNeighborhoodRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTNEIGHBORHOODROWCOUNT))
			{
				SqlDataReader reader;
				_ResturantNeighborhoodRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ResturantNeighborhoodRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ResturantNeighborhood object
        /// </summary>
        /// <param name="resturantNeighborhoodObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ResturantNeighborhoodBase resturantNeighborhoodObject, SqlDataReader reader, int start)
		{
			
				resturantNeighborhoodObject.Id = reader.GetInt32( start + 0 );			
				resturantNeighborhoodObject.SiteLocationId = reader.GetInt32( start + 1 );			
				resturantNeighborhoodObject.NeighborhoodName = reader.GetString( start + 2 );			
				resturantNeighborhoodObject.NeighborhoodURL = reader.GetString( start + 3 );			
				resturantNeighborhoodObject.CreatedBy = reader.GetGuid( start + 4 );			
				resturantNeighborhoodObject.CreatedDate = reader.GetDateTime( start + 5 );			
			FillBaseObject(resturantNeighborhoodObject, reader, (start + 6));

			
			resturantNeighborhoodObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ResturantNeighborhood object
        /// </summary>
        /// <param name="resturantNeighborhoodObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ResturantNeighborhoodBase resturantNeighborhoodObject, SqlDataReader reader)
		{
			FillObject(resturantNeighborhoodObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ResturantNeighborhood object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ResturantNeighborhood object</returns>
		private ResturantNeighborhood GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ResturantNeighborhood resturantNeighborhoodObject= new ResturantNeighborhood();
					FillObject(resturantNeighborhoodObject, reader);
					return resturantNeighborhoodObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ResturantNeighborhood objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ResturantNeighborhood objects</returns>
		private ResturantNeighborhoodList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ResturantNeighborhood list
			ResturantNeighborhoodList list = new ResturantNeighborhoodList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ResturantNeighborhood resturantNeighborhoodObject = new ResturantNeighborhood();
					FillObject(resturantNeighborhoodObject, reader);

					list.Add(resturantNeighborhoodObject);
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
