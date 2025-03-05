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
	public partial class RoutingNumberDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTROUTINGNUMBER = "InsertRoutingNumber";
		private const string UPDATEROUTINGNUMBER = "UpdateRoutingNumber";
		private const string DELETEROUTINGNUMBER = "DeleteRoutingNumber";
		private const string GETROUTINGNUMBERBYID = "GetRoutingNumberById";
		private const string GETALLROUTINGNUMBER = "GetAllRoutingNumber";
		private const string GETPAGEDROUTINGNUMBER = "GetPagedRoutingNumber";
		private const string GETROUTINGNUMBERMAXIMUMID = "GetRoutingNumberMaximumId";
		private const string GETROUTINGNUMBERROWCOUNT = "GetRoutingNumberRowCount";	
		private const string GETROUTINGNUMBERBYQUERY = "GetRoutingNumberByQuery";
		#endregion
		
		#region Constructors
		public RoutingNumberDataAccess(ClientContext context) : base(context) { }
		public RoutingNumberDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="routingNumberObject"></param>
		private void AddCommonParams(SqlCommand cmd, RoutingNumberBase routingNumberObject)
		{	
			AddParameter(cmd, pNVarChar(RoutingNumberBase.Property_RoutingNumber, 50, routingNumberObject.RoutingNumber));
			AddParameter(cmd, pNVarChar(RoutingNumberBase.Property_BankName, 150, routingNumberObject.BankName));
			AddParameter(cmd, pNVarChar(RoutingNumberBase.Property_City, 150, routingNumberObject.City));
			AddParameter(cmd, pNVarChar(RoutingNumberBase.Property_State, 50, routingNumberObject.State));
			AddParameter(cmd, pNVarChar(RoutingNumberBase.Property_Address, 150, routingNumberObject.Address));
			AddParameter(cmd, pNVarChar(RoutingNumberBase.Property_ContactNo, 50, routingNumberObject.ContactNo));
			AddParameter(cmd, pNVarChar(RoutingNumberBase.Property_ZipCode, 50, routingNumberObject.ZipCode));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RoutingNumber
        /// </summary>
        /// <param name="routingNumberObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RoutingNumberBase routingNumberObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTROUTINGNUMBER);
	
				AddParameter(cmd, pInt32(RoutingNumberBase.Property_Id, routingNumberObject.Id));
				AddCommonParams(cmd, routingNumberObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					routingNumberObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(routingNumberObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RoutingNumber
        /// </summary>
        /// <param name="routingNumberObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RoutingNumberBase routingNumberObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEROUTINGNUMBER);
				
				AddParameter(cmd, pInt32(RoutingNumberBase.Property_Id, routingNumberObject.Id));
				AddCommonParams(cmd, routingNumberObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					routingNumberObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(routingNumberObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RoutingNumber
        /// </summary>
        /// <param name="Id">Id of the RoutingNumber object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEROUTINGNUMBER);	
				
				AddParameter(cmd, pInt32(RoutingNumberBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RoutingNumber), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RoutingNumber object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RoutingNumber object to retrieve</param>
        /// <returns>RoutingNumber object, null if not found</returns>
		public RoutingNumber Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETROUTINGNUMBERBYID))
			{
				AddParameter( cmd, pInt32(RoutingNumberBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RoutingNumber objects 
        /// </summary>
        /// <returns>A list of RoutingNumber objects</returns>
		public RoutingNumberList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLROUTINGNUMBER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RoutingNumber objects by PageRequest
        /// </summary>
        /// <returns>A list of RoutingNumber objects</returns>
		public RoutingNumberList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDROUTINGNUMBER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RoutingNumberList _RoutingNumberList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RoutingNumberList;
			}
		}
		
		/// <summary>
        /// Retrieves all RoutingNumber objects by query String
        /// </summary>
        /// <returns>A list of RoutingNumber objects</returns>
		public RoutingNumberList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETROUTINGNUMBERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RoutingNumber Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RoutingNumber
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETROUTINGNUMBERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RoutingNumber Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RoutingNumber
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RoutingNumberRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETROUTINGNUMBERROWCOUNT))
			{
				SqlDataReader reader;
				_RoutingNumberRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RoutingNumberRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RoutingNumber object
        /// </summary>
        /// <param name="routingNumberObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RoutingNumberBase routingNumberObject, SqlDataReader reader, int start)
		{
			
				routingNumberObject.Id = reader.GetInt32( start + 0 );			
				routingNumberObject.RoutingNumber = reader.GetString( start + 1 );			
				routingNumberObject.BankName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) routingNumberObject.City = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) routingNumberObject.State = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) routingNumberObject.Address = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) routingNumberObject.ContactNo = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) routingNumberObject.ZipCode = reader.GetString( start + 7 );			
			FillBaseObject(routingNumberObject, reader, (start + 8));

			
			routingNumberObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RoutingNumber object
        /// </summary>
        /// <param name="routingNumberObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RoutingNumberBase routingNumberObject, SqlDataReader reader)
		{
			FillObject(routingNumberObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RoutingNumber object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RoutingNumber object</returns>
		private RoutingNumber GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RoutingNumber routingNumberObject= new RoutingNumber();
					FillObject(routingNumberObject, reader);
					return routingNumberObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RoutingNumber objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RoutingNumber objects</returns>
		private RoutingNumberList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RoutingNumber list
			RoutingNumberList list = new RoutingNumberList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RoutingNumber routingNumberObject = new RoutingNumber();
					FillObject(routingNumberObject, reader);

					list.Add(routingNumberObject);
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
