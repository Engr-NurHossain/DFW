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
	public partial class SmartPackageEquipmentServiceEquipmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT = "InsertSmartPackageEquipmentServiceEquipment";
		private const string UPDATESMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT = "UpdateSmartPackageEquipmentServiceEquipment";
		private const string DELETESMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT = "DeleteSmartPackageEquipmentServiceEquipment";
		private const string GETSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENTBYID = "GetSmartPackageEquipmentServiceEquipmentById";
		private const string GETALLSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT = "GetAllSmartPackageEquipmentServiceEquipment";
		private const string GETPAGEDSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT = "GetPagedSmartPackageEquipmentServiceEquipment";
		private const string GETSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENTMAXIMUMID = "GetSmartPackageEquipmentServiceEquipmentMaximumId";
		private const string GETSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENTROWCOUNT = "GetSmartPackageEquipmentServiceEquipmentRowCount";	
		private const string GETSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENTBYQUERY = "GetSmartPackageEquipmentServiceEquipmentByQuery";
		#endregion
		
		#region Constructors
		public SmartPackageEquipmentServiceEquipmentDataAccess(ClientContext context) : base(context) { }
		public SmartPackageEquipmentServiceEquipmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="smartPackageEquipmentServiceEquipmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, SmartPackageEquipmentServiceEquipmentBase smartPackageEquipmentServiceEquipmentObject)
		{	
			AddParameter(cmd, pGuid(SmartPackageEquipmentServiceEquipmentBase.Property_SmartPackageEquipmentServiceId, smartPackageEquipmentServiceEquipmentObject.SmartPackageEquipmentServiceId));
			AddParameter(cmd, pGuid(SmartPackageEquipmentServiceEquipmentBase.Property_EquipmentId, smartPackageEquipmentServiceEquipmentObject.EquipmentId));
			AddParameter(cmd, pInt32(SmartPackageEquipmentServiceEquipmentBase.Property_Quantity, smartPackageEquipmentServiceEquipmentObject.Quantity));
			AddParameter(cmd, pDouble(SmartPackageEquipmentServiceEquipmentBase.Property_EquipmentPrice, smartPackageEquipmentServiceEquipmentObject.EquipmentPrice));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SmartPackageEquipmentServiceEquipment
        /// </summary>
        /// <param name="smartPackageEquipmentServiceEquipmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SmartPackageEquipmentServiceEquipmentBase smartPackageEquipmentServiceEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT);
	
				AddParameter(cmd, pInt32Out(SmartPackageEquipmentServiceEquipmentBase.Property_Id));
				AddCommonParams(cmd, smartPackageEquipmentServiceEquipmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					smartPackageEquipmentServiceEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					smartPackageEquipmentServiceEquipmentObject.Id = (Int32)GetOutParameter(cmd, SmartPackageEquipmentServiceEquipmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(smartPackageEquipmentServiceEquipmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SmartPackageEquipmentServiceEquipment
        /// </summary>
        /// <param name="smartPackageEquipmentServiceEquipmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SmartPackageEquipmentServiceEquipmentBase smartPackageEquipmentServiceEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT);
				
				AddParameter(cmd, pInt32(SmartPackageEquipmentServiceEquipmentBase.Property_Id, smartPackageEquipmentServiceEquipmentObject.Id));
				AddCommonParams(cmd, smartPackageEquipmentServiceEquipmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					smartPackageEquipmentServiceEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(smartPackageEquipmentServiceEquipmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SmartPackageEquipmentServiceEquipment
        /// </summary>
        /// <param name="Id">Id of the SmartPackageEquipmentServiceEquipment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT);	
				
				AddParameter(cmd, pInt32(SmartPackageEquipmentServiceEquipmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SmartPackageEquipmentServiceEquipment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SmartPackageEquipmentServiceEquipment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SmartPackageEquipmentServiceEquipment object to retrieve</param>
        /// <returns>SmartPackageEquipmentServiceEquipment object, null if not found</returns>
		public SmartPackageEquipmentServiceEquipment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENTBYID))
			{
				AddParameter( cmd, pInt32(SmartPackageEquipmentServiceEquipmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SmartPackageEquipmentServiceEquipment objects 
        /// </summary>
        /// <returns>A list of SmartPackageEquipmentServiceEquipment objects</returns>
		public SmartPackageEquipmentServiceEquipmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SmartPackageEquipmentServiceEquipment objects by PageRequest
        /// </summary>
        /// <returns>A list of SmartPackageEquipmentServiceEquipment objects</returns>
		public SmartPackageEquipmentServiceEquipmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SmartPackageEquipmentServiceEquipmentList _SmartPackageEquipmentServiceEquipmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SmartPackageEquipmentServiceEquipmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all SmartPackageEquipmentServiceEquipment objects by query String
        /// </summary>
        /// <returns>A list of SmartPackageEquipmentServiceEquipment objects</returns>
		public SmartPackageEquipmentServiceEquipmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SmartPackageEquipmentServiceEquipment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SmartPackageEquipmentServiceEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SmartPackageEquipmentServiceEquipment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SmartPackageEquipmentServiceEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SmartPackageEquipmentServiceEquipmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEEQUIPMENTSERVICEEQUIPMENTROWCOUNT))
			{
				SqlDataReader reader;
				_SmartPackageEquipmentServiceEquipmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SmartPackageEquipmentServiceEquipmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SmartPackageEquipmentServiceEquipment object
        /// </summary>
        /// <param name="smartPackageEquipmentServiceEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SmartPackageEquipmentServiceEquipmentBase smartPackageEquipmentServiceEquipmentObject, SqlDataReader reader, int start)
		{
			
				smartPackageEquipmentServiceEquipmentObject.Id = reader.GetInt32( start + 0 );			
				smartPackageEquipmentServiceEquipmentObject.SmartPackageEquipmentServiceId = reader.GetGuid( start + 1 );			
				smartPackageEquipmentServiceEquipmentObject.EquipmentId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) smartPackageEquipmentServiceEquipmentObject.Quantity = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) smartPackageEquipmentServiceEquipmentObject.EquipmentPrice = reader.GetDouble( start + 4 );			
			FillBaseObject(smartPackageEquipmentServiceEquipmentObject, reader, (start + 5));

			
			smartPackageEquipmentServiceEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SmartPackageEquipmentServiceEquipment object
        /// </summary>
        /// <param name="smartPackageEquipmentServiceEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SmartPackageEquipmentServiceEquipmentBase smartPackageEquipmentServiceEquipmentObject, SqlDataReader reader)
		{
			FillObject(smartPackageEquipmentServiceEquipmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SmartPackageEquipmentServiceEquipment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SmartPackageEquipmentServiceEquipment object</returns>
		private SmartPackageEquipmentServiceEquipment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SmartPackageEquipmentServiceEquipment smartPackageEquipmentServiceEquipmentObject= new SmartPackageEquipmentServiceEquipment();
					FillObject(smartPackageEquipmentServiceEquipmentObject, reader);
					return smartPackageEquipmentServiceEquipmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SmartPackageEquipmentServiceEquipment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SmartPackageEquipmentServiceEquipment objects</returns>
		private SmartPackageEquipmentServiceEquipmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SmartPackageEquipmentServiceEquipment list
			SmartPackageEquipmentServiceEquipmentList list = new SmartPackageEquipmentServiceEquipmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SmartPackageEquipmentServiceEquipment smartPackageEquipmentServiceEquipmentObject = new SmartPackageEquipmentServiceEquipment();
					FillObject(smartPackageEquipmentServiceEquipmentObject, reader);

					list.Add(smartPackageEquipmentServiceEquipmentObject);
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
