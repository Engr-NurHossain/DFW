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
	public partial class EquipmentsFavouriteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTSFAVOURITE = "InsertEquipmentsFavourite";
		private const string UPDATEEQUIPMENTSFAVOURITE = "UpdateEquipmentsFavourite";
		private const string DELETEEQUIPMENTSFAVOURITE = "DeleteEquipmentsFavourite";
		private const string GETEQUIPMENTSFAVOURITEBYID = "GetEquipmentsFavouriteById";
		private const string GETALLEQUIPMENTSFAVOURITE = "GetAllEquipmentsFavourite";
		private const string GETPAGEDEQUIPMENTSFAVOURITE = "GetPagedEquipmentsFavourite";
		private const string GETEQUIPMENTSFAVOURITEMAXIMUMID = "GetEquipmentsFavouriteMaximumId";
		private const string GETEQUIPMENTSFAVOURITEROWCOUNT = "GetEquipmentsFavouriteRowCount";	
		private const string GETEQUIPMENTSFAVOURITEBYQUERY = "GetEquipmentsFavouriteByQuery";
		#endregion
		
		#region Constructors
		public EquipmentsFavouriteDataAccess(ClientContext context) : base(context) { }
		public EquipmentsFavouriteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentsFavouriteObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentsFavouriteBase equipmentsFavouriteObject)
		{	
			AddParameter(cmd, pGuid(EquipmentsFavouriteBase.Property_UserId, equipmentsFavouriteObject.UserId));
			AddParameter(cmd, pGuid(EquipmentsFavouriteBase.Property_CompanyId, equipmentsFavouriteObject.CompanyId));
			AddParameter(cmd, pGuid(EquipmentsFavouriteBase.Property_EquipmentId, equipmentsFavouriteObject.EquipmentId));
			AddParameter(cmd, pBool(EquipmentsFavouriteBase.Property_IsFavourite, equipmentsFavouriteObject.IsFavourite));
			AddParameter(cmd, pGuid(EquipmentsFavouriteBase.Property_CreatedBy, equipmentsFavouriteObject.CreatedBy));
			AddParameter(cmd, pDateTime(EquipmentsFavouriteBase.Property_CreatedDate, equipmentsFavouriteObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentsFavourite
        /// </summary>
        /// <param name="equipmentsFavouriteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentsFavouriteBase equipmentsFavouriteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTSFAVOURITE);
	
				AddParameter(cmd, pInt32Out(EquipmentsFavouriteBase.Property_Id));
				AddCommonParams(cmd, equipmentsFavouriteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentsFavouriteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentsFavouriteObject.Id = (Int32)GetOutParameter(cmd, EquipmentsFavouriteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentsFavouriteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentsFavourite
        /// </summary>
        /// <param name="equipmentsFavouriteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentsFavouriteBase equipmentsFavouriteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTSFAVOURITE);
				
				AddParameter(cmd, pInt32(EquipmentsFavouriteBase.Property_Id, equipmentsFavouriteObject.Id));
				AddCommonParams(cmd, equipmentsFavouriteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentsFavouriteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentsFavouriteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentsFavourite
        /// </summary>
        /// <param name="Id">Id of the EquipmentsFavourite object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTSFAVOURITE);	
				
				AddParameter(cmd, pInt32(EquipmentsFavouriteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentsFavourite), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentsFavourite object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentsFavourite object to retrieve</param>
        /// <returns>EquipmentsFavourite object, null if not found</returns>
		public EquipmentsFavourite Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTSFAVOURITEBYID))
			{
				AddParameter( cmd, pInt32(EquipmentsFavouriteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentsFavourite objects 
        /// </summary>
        /// <returns>A list of EquipmentsFavourite objects</returns>
		public EquipmentsFavouriteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTSFAVOURITE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentsFavourite objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentsFavourite objects</returns>
		public EquipmentsFavouriteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTSFAVOURITE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentsFavouriteList _EquipmentsFavouriteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentsFavouriteList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentsFavourite objects by query String
        /// </summary>
        /// <returns>A list of EquipmentsFavourite objects</returns>
		public EquipmentsFavouriteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTSFAVOURITEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentsFavourite Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentsFavourite
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTSFAVOURITEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentsFavourite Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentsFavourite
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentsFavouriteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTSFAVOURITEROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentsFavouriteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentsFavouriteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentsFavourite object
        /// </summary>
        /// <param name="equipmentsFavouriteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentsFavouriteBase equipmentsFavouriteObject, SqlDataReader reader, int start)
		{
			
				equipmentsFavouriteObject.Id = reader.GetInt32( start + 0 );			
				equipmentsFavouriteObject.UserId = reader.GetGuid( start + 1 );			
				equipmentsFavouriteObject.CompanyId = reader.GetGuid( start + 2 );			
				equipmentsFavouriteObject.EquipmentId = reader.GetGuid( start + 3 );			
				equipmentsFavouriteObject.IsFavourite = reader.GetBoolean( start + 4 );			
				equipmentsFavouriteObject.CreatedBy = reader.GetGuid( start + 5 );			
				equipmentsFavouriteObject.CreatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(equipmentsFavouriteObject, reader, (start + 7));

			
			equipmentsFavouriteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentsFavourite object
        /// </summary>
        /// <param name="equipmentsFavouriteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentsFavouriteBase equipmentsFavouriteObject, SqlDataReader reader)
		{
			FillObject(equipmentsFavouriteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentsFavourite object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentsFavourite object</returns>
		private EquipmentsFavourite GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentsFavourite equipmentsFavouriteObject= new EquipmentsFavourite();
					FillObject(equipmentsFavouriteObject, reader);
					return equipmentsFavouriteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentsFavourite objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentsFavourite objects</returns>
		private EquipmentsFavouriteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentsFavourite list
			EquipmentsFavouriteList list = new EquipmentsFavouriteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentsFavourite equipmentsFavouriteObject = new EquipmentsFavourite();
					FillObject(equipmentsFavouriteObject, reader);

					list.Add(equipmentsFavouriteObject);
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
