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
	public partial class ZonesEquipmentTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTZONESEQUIPMENTTYPE = "InsertZonesEquipmentType";
		private const string UPDATEZONESEQUIPMENTTYPE = "UpdateZonesEquipmentType";
		private const string DELETEZONESEQUIPMENTTYPE = "DeleteZonesEquipmentType";
		private const string GETZONESEQUIPMENTTYPEBYID = "GetZonesEquipmentTypeByID";
		private const string GETALLZONESEQUIPMENTTYPE = "GetAllZonesEquipmentType";
		private const string GETPAGEDZONESEQUIPMENTTYPE = "GetPagedZonesEquipmentType";
		private const string GETZONESEQUIPMENTTYPEMAXIMUMID = "GetZonesEquipmentTypeMaximumID";
		private const string GETZONESEQUIPMENTTYPEROWCOUNT = "GetZonesEquipmentTypeRowCount";	
		private const string GETZONESEQUIPMENTTYPEBYQUERY = "GetZonesEquipmentTypeByQuery";
		#endregion
		
		#region Constructors
		public ZonesEquipmentTypeDataAccess(ClientContext context) : base(context) { }
		public ZonesEquipmentTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="zonesEquipmentTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, ZonesEquipmentTypeBase zonesEquipmentTypeObject)
		{	
			AddParameter(cmd, pNVarChar(ZonesEquipmentTypeBase.Property_EqpmentTypeId, 50, zonesEquipmentTypeObject.EqpmentTypeId));
			AddParameter(cmd, pNVarChar(ZonesEquipmentTypeBase.Property_EquipmentType, 50, zonesEquipmentTypeObject.EquipmentType));
			AddParameter(cmd, pNVarChar(ZonesEquipmentTypeBase.Property_Platform, 50, zonesEquipmentTypeObject.Platform));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ZonesEquipmentType
        /// </summary>
        /// <param name="zonesEquipmentTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ZonesEquipmentTypeBase zonesEquipmentTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTZONESEQUIPMENTTYPE);
	
				AddParameter(cmd, pInt32Out(ZonesEquipmentTypeBase.Property_ID));
				AddCommonParams(cmd, zonesEquipmentTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					zonesEquipmentTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					zonesEquipmentTypeObject.ID = (Int32)GetOutParameter(cmd, ZonesEquipmentTypeBase.Property_ID);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(zonesEquipmentTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ZonesEquipmentType
        /// </summary>
        /// <param name="zonesEquipmentTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ZonesEquipmentTypeBase zonesEquipmentTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEZONESEQUIPMENTTYPE);
				
				AddParameter(cmd, pInt32(ZonesEquipmentTypeBase.Property_ID, zonesEquipmentTypeObject.ID));
				AddCommonParams(cmd, zonesEquipmentTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					zonesEquipmentTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(zonesEquipmentTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ZonesEquipmentType
        /// </summary>
        /// <param name="ID">ID of the ZonesEquipmentType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _ID)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEZONESEQUIPMENTTYPE);	
				
				AddParameter(cmd, pInt32(ZonesEquipmentTypeBase.Property_ID, _ID));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ZonesEquipmentType), _ID, x);
			}
			
		}
		#endregion
		
		#region Get By ID Method
		/// <summary>
        /// Retrieves ZonesEquipmentType object using it's ID
        /// </summary>
        /// <param name="ID">The ID of the ZonesEquipmentType object to retrieve</param>
        /// <returns>ZonesEquipmentType object, null if not found</returns>
		public ZonesEquipmentType Get(Int32 _ID)
		{
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTTYPEBYID))
			{
				AddParameter( cmd, pInt32(ZonesEquipmentTypeBase.Property_ID, _ID));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ZonesEquipmentType objects 
        /// </summary>
        /// <returns>A list of ZonesEquipmentType objects</returns>
		public ZonesEquipmentTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLZONESEQUIPMENTTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ZonesEquipmentType objects by PageRequest
        /// </summary>
        /// <returns>A list of ZonesEquipmentType objects</returns>
		public ZonesEquipmentTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDZONESEQUIPMENTTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ZonesEquipmentTypeList _ZonesEquipmentTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ZonesEquipmentTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all ZonesEquipmentType objects by query String
        /// </summary>
        /// <returns>A list of ZonesEquipmentType objects</returns>
		public ZonesEquipmentTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ZonesEquipmentType Maximum ID Method
		/// <summary>
        /// Retrieves Get Maximum ID of ZonesEquipmentType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxID()
		{
			Int32 _MaximumID = 0; 
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumID = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumID;
		}
		
		#endregion
		
		#region Get ZonesEquipmentType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ZonesEquipmentType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ZonesEquipmentTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_ZonesEquipmentTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ZonesEquipmentTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ZonesEquipmentType object
        /// </summary>
        /// <param name="zonesEquipmentTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ZonesEquipmentTypeBase zonesEquipmentTypeObject, SqlDataReader reader, int start)
		{
			
				zonesEquipmentTypeObject.ID = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) zonesEquipmentTypeObject.EqpmentTypeId = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) zonesEquipmentTypeObject.EquipmentType = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) zonesEquipmentTypeObject.Platform = reader.GetString( start + 3 );			
			FillBaseObject(zonesEquipmentTypeObject, reader, (start + 4));

			
			zonesEquipmentTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ZonesEquipmentType object
        /// </summary>
        /// <param name="zonesEquipmentTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ZonesEquipmentTypeBase zonesEquipmentTypeObject, SqlDataReader reader)
		{
			FillObject(zonesEquipmentTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ZonesEquipmentType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ZonesEquipmentType object</returns>
		private ZonesEquipmentType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ZonesEquipmentType zonesEquipmentTypeObject= new ZonesEquipmentType();
					FillObject(zonesEquipmentTypeObject, reader);
					return zonesEquipmentTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ZonesEquipmentType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ZonesEquipmentType objects</returns>
		private ZonesEquipmentTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ZonesEquipmentType list
			ZonesEquipmentTypeList list = new ZonesEquipmentTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ZonesEquipmentType zonesEquipmentTypeObject = new ZonesEquipmentType();
					FillObject(zonesEquipmentTypeObject, reader);

					list.Add(zonesEquipmentTypeObject);
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
