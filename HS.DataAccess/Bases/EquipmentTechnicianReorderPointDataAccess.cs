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
	public partial class EquipmentTechnicianReorderPointDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTTECHNICIANREORDERPOINT = "InsertEquipmentTechnicianReorderPoint";
		private const string UPDATEEQUIPMENTTECHNICIANREORDERPOINT = "UpdateEquipmentTechnicianReorderPoint";
		private const string DELETEEQUIPMENTTECHNICIANREORDERPOINT = "DeleteEquipmentTechnicianReorderPoint";
		private const string GETEQUIPMENTTECHNICIANREORDERPOINTBYID = "GetEquipmentTechnicianReorderPointById";
		private const string GETALLEQUIPMENTTECHNICIANREORDERPOINT = "GetAllEquipmentTechnicianReorderPoint";
		private const string GETPAGEDEQUIPMENTTECHNICIANREORDERPOINT = "GetPagedEquipmentTechnicianReorderPoint";
		private const string GETEQUIPMENTTECHNICIANREORDERPOINTMAXIMUMID = "GetEquipmentTechnicianReorderPointMaximumId";
		private const string GETEQUIPMENTTECHNICIANREORDERPOINTROWCOUNT = "GetEquipmentTechnicianReorderPointRowCount";	
		private const string GETEQUIPMENTTECHNICIANREORDERPOINTBYQUERY = "GetEquipmentTechnicianReorderPointByQuery";
		#endregion
		
		#region Constructors
		public EquipmentTechnicianReorderPointDataAccess(ClientContext context) : base(context) { }
		public EquipmentTechnicianReorderPointDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentTechnicianReorderPointObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentTechnicianReorderPointBase equipmentTechnicianReorderPointObject)
		{	
			AddParameter(cmd, pGuid(EquipmentTechnicianReorderPointBase.Property_CompanyId, equipmentTechnicianReorderPointObject.CompanyId));
			AddParameter(cmd, pGuid(EquipmentTechnicianReorderPointBase.Property_EquipmentId, equipmentTechnicianReorderPointObject.EquipmentId));
			AddParameter(cmd, pGuid(EquipmentTechnicianReorderPointBase.Property_TechnicianId, equipmentTechnicianReorderPointObject.TechnicianId));
			AddParameter(cmd, pInt32(EquipmentTechnicianReorderPointBase.Property_ReorderPoint, equipmentTechnicianReorderPointObject.ReorderPoint));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentTechnicianReorderPoint
        /// </summary>
        /// <param name="equipmentTechnicianReorderPointObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentTechnicianReorderPointBase equipmentTechnicianReorderPointObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTTECHNICIANREORDERPOINT);
	
				AddParameter(cmd, pInt32Out(EquipmentTechnicianReorderPointBase.Property_Id));
				AddCommonParams(cmd, equipmentTechnicianReorderPointObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentTechnicianReorderPointObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentTechnicianReorderPointObject.Id = (Int32)GetOutParameter(cmd, EquipmentTechnicianReorderPointBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentTechnicianReorderPointObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentTechnicianReorderPoint
        /// </summary>
        /// <param name="equipmentTechnicianReorderPointObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentTechnicianReorderPointBase equipmentTechnicianReorderPointObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTTECHNICIANREORDERPOINT);
				
				AddParameter(cmd, pInt32(EquipmentTechnicianReorderPointBase.Property_Id, equipmentTechnicianReorderPointObject.Id));
				AddCommonParams(cmd, equipmentTechnicianReorderPointObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentTechnicianReorderPointObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentTechnicianReorderPointObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentTechnicianReorderPoint
        /// </summary>
        /// <param name="Id">Id of the EquipmentTechnicianReorderPoint object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTTECHNICIANREORDERPOINT);	
				
				AddParameter(cmd, pInt32(EquipmentTechnicianReorderPointBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentTechnicianReorderPoint), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentTechnicianReorderPoint object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentTechnicianReorderPoint object to retrieve</param>
        /// <returns>EquipmentTechnicianReorderPoint object, null if not found</returns>
		public EquipmentTechnicianReorderPoint Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTTECHNICIANREORDERPOINTBYID))
			{
				AddParameter( cmd, pInt32(EquipmentTechnicianReorderPointBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentTechnicianReorderPoint objects 
        /// </summary>
        /// <returns>A list of EquipmentTechnicianReorderPoint objects</returns>
		public EquipmentTechnicianReorderPointList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTTECHNICIANREORDERPOINT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentTechnicianReorderPoint objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentTechnicianReorderPoint objects</returns>
		public EquipmentTechnicianReorderPointList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTTECHNICIANREORDERPOINT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentTechnicianReorderPointList _EquipmentTechnicianReorderPointList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentTechnicianReorderPointList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentTechnicianReorderPoint objects by query String
        /// </summary>
        /// <returns>A list of EquipmentTechnicianReorderPoint objects</returns>
		public EquipmentTechnicianReorderPointList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTTECHNICIANREORDERPOINTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentTechnicianReorderPoint Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentTechnicianReorderPoint
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTTECHNICIANREORDERPOINTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentTechnicianReorderPoint Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentTechnicianReorderPoint
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentTechnicianReorderPointRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTTECHNICIANREORDERPOINTROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentTechnicianReorderPointRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentTechnicianReorderPointRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentTechnicianReorderPoint object
        /// </summary>
        /// <param name="equipmentTechnicianReorderPointObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentTechnicianReorderPointBase equipmentTechnicianReorderPointObject, SqlDataReader reader, int start)
		{
			
				equipmentTechnicianReorderPointObject.Id = reader.GetInt32( start + 0 );			
				equipmentTechnicianReorderPointObject.CompanyId = reader.GetGuid( start + 1 );			
				equipmentTechnicianReorderPointObject.EquipmentId = reader.GetGuid( start + 2 );			
				equipmentTechnicianReorderPointObject.TechnicianId = reader.GetGuid( start + 3 );			
				equipmentTechnicianReorderPointObject.ReorderPoint = reader.GetInt32( start + 4 );			
			FillBaseObject(equipmentTechnicianReorderPointObject, reader, (start + 5));

			
			equipmentTechnicianReorderPointObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentTechnicianReorderPoint object
        /// </summary>
        /// <param name="equipmentTechnicianReorderPointObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentTechnicianReorderPointBase equipmentTechnicianReorderPointObject, SqlDataReader reader)
		{
			FillObject(equipmentTechnicianReorderPointObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentTechnicianReorderPoint object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentTechnicianReorderPoint object</returns>
		private EquipmentTechnicianReorderPoint GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentTechnicianReorderPoint equipmentTechnicianReorderPointObject= new EquipmentTechnicianReorderPoint();
					FillObject(equipmentTechnicianReorderPointObject, reader);
					return equipmentTechnicianReorderPointObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentTechnicianReorderPoint objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentTechnicianReorderPoint objects</returns>
		private EquipmentTechnicianReorderPointList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentTechnicianReorderPoint list
			EquipmentTechnicianReorderPointList list = new EquipmentTechnicianReorderPointList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentTechnicianReorderPoint equipmentTechnicianReorderPointObject = new EquipmentTechnicianReorderPoint();
					FillObject(equipmentTechnicianReorderPointObject, reader);

					list.Add(equipmentTechnicianReorderPointObject);
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
