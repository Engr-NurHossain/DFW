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
	public partial class EmergencyContactDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMERGENCYCONTACT = "InsertEmergencyContact";
		private const string UPDATEEMERGENCYCONTACT = "UpdateEmergencyContact";
		private const string DELETEEMERGENCYCONTACT = "DeleteEmergencyContact";
		private const string GETEMERGENCYCONTACTBYID = "GetEmergencyContactById";
		private const string GETALLEMERGENCYCONTACT = "GetAllEmergencyContact";
		private const string GETPAGEDEMERGENCYCONTACT = "GetPagedEmergencyContact";
		private const string GETEMERGENCYCONTACTMAXIMUMID = "GetEmergencyContactMaximumId";
		private const string GETEMERGENCYCONTACTROWCOUNT = "GetEmergencyContactRowCount";	
		private const string GETEMERGENCYCONTACTBYQUERY = "GetEmergencyContactByQuery";
		#endregion
		
		#region Constructors
		public EmergencyContactDataAccess(ClientContext context) : base(context) { }
		public EmergencyContactDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="emergencyContactObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmergencyContactBase emergencyContactObject)
		{	
			AddParameter(cmd, pGuid(EmergencyContactBase.Property_CompanyId, emergencyContactObject.CompanyId));
			AddParameter(cmd, pGuid(EmergencyContactBase.Property_CustomerId, emergencyContactObject.CustomerId));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_CrossSteet, emergencyContactObject.CrossSteet));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_FirstName, 50, emergencyContactObject.FirstName));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_LastName, 50, emergencyContactObject.LastName));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_RelationShip, 50, emergencyContactObject.RelationShip));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_Email, 50, emergencyContactObject.Email));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_Phone, 50, emergencyContactObject.Phone));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_HasKey, 50, emergencyContactObject.HasKey));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_PhoneType, 50, emergencyContactObject.PhoneType));
			AddParameter(cmd, pInt32(EmergencyContactBase.Property_OrderBy, emergencyContactObject.OrderBy));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_ContactNo, 100, emergencyContactObject.ContactNo));
			AddParameter(cmd, pNVarChar(EmergencyContactBase.Property_Platform, 100, emergencyContactObject.Platform));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmergencyContact
        /// </summary>
        /// <param name="emergencyContactObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmergencyContactBase emergencyContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMERGENCYCONTACT);
	
				AddParameter(cmd, pInt32Out(EmergencyContactBase.Property_Id));
				AddCommonParams(cmd, emergencyContactObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					emergencyContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					emergencyContactObject.Id = (Int32)GetOutParameter(cmd, EmergencyContactBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(emergencyContactObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmergencyContact
        /// </summary>
        /// <param name="emergencyContactObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmergencyContactBase emergencyContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMERGENCYCONTACT);
				
				AddParameter(cmd, pInt32(EmergencyContactBase.Property_Id, emergencyContactObject.Id));
				AddCommonParams(cmd, emergencyContactObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					emergencyContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(emergencyContactObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmergencyContact
        /// </summary>
        /// <param name="Id">Id of the EmergencyContact object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMERGENCYCONTACT);	
				
				AddParameter(cmd, pInt32(EmergencyContactBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmergencyContact), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmergencyContact object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmergencyContact object to retrieve</param>
        /// <returns>EmergencyContact object, null if not found</returns>
		public EmergencyContact Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMERGENCYCONTACTBYID))
			{
				AddParameter( cmd, pInt32(EmergencyContactBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmergencyContact objects 
        /// </summary>
        /// <returns>A list of EmergencyContact objects</returns>
		public EmergencyContactList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMERGENCYCONTACT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmergencyContact objects by PageRequest
        /// </summary>
        /// <returns>A list of EmergencyContact objects</returns>
		public EmergencyContactList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMERGENCYCONTACT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmergencyContactList _EmergencyContactList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmergencyContactList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmergencyContact objects by query String
        /// </summary>
        /// <returns>A list of EmergencyContact objects</returns>
		public EmergencyContactList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMERGENCYCONTACTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmergencyContact Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmergencyContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMERGENCYCONTACTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmergencyContact Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmergencyContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmergencyContactRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMERGENCYCONTACTROWCOUNT))
			{
				SqlDataReader reader;
				_EmergencyContactRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmergencyContactRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmergencyContact object
        /// </summary>
        /// <param name="emergencyContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmergencyContactBase emergencyContactObject, SqlDataReader reader, int start)
		{
			
				emergencyContactObject.Id = reader.GetInt32( start + 0 );			
				emergencyContactObject.CompanyId = reader.GetGuid( start + 1 );			
				emergencyContactObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) emergencyContactObject.CrossSteet = reader.GetString( start + 3 );			
				emergencyContactObject.FirstName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) emergencyContactObject.LastName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) emergencyContactObject.RelationShip = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) emergencyContactObject.Email = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) emergencyContactObject.Phone = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) emergencyContactObject.HasKey = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) emergencyContactObject.PhoneType = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) emergencyContactObject.OrderBy = reader.GetInt32( start + 11 );			
				if(!reader.IsDBNull(12)) emergencyContactObject.ContactNo = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) emergencyContactObject.Platform = reader.GetString( start + 13 );			
			FillBaseObject(emergencyContactObject, reader, (start + 14));

			
			emergencyContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmergencyContact object
        /// </summary>
        /// <param name="emergencyContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmergencyContactBase emergencyContactObject, SqlDataReader reader)
		{
			FillObject(emergencyContactObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmergencyContact object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmergencyContact object</returns>
		private EmergencyContact GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmergencyContact emergencyContactObject= new EmergencyContact();
					FillObject(emergencyContactObject, reader);
					return emergencyContactObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmergencyContact objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmergencyContact objects</returns>
		private EmergencyContactList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmergencyContact list
			EmergencyContactList list = new EmergencyContactList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmergencyContact emergencyContactObject = new EmergencyContact();
					FillObject(emergencyContactObject, reader);

					list.Add(emergencyContactObject);
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
