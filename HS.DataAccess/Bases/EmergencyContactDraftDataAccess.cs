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
	public partial class EmergencyContactDraftDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMERGENCYCONTACTDRAFT = "InsertEmergencyContactDraft";
		private const string UPDATEEMERGENCYCONTACTDRAFT = "UpdateEmergencyContactDraft";
		private const string DELETEEMERGENCYCONTACTDRAFT = "DeleteEmergencyContactDraft";
		private const string GETEMERGENCYCONTACTDRAFTBYID = "GetEmergencyContactDraftById";
		private const string GETALLEMERGENCYCONTACTDRAFT = "GetAllEmergencyContactDraft";
		private const string GETPAGEDEMERGENCYCONTACTDRAFT = "GetPagedEmergencyContactDraft";
		private const string GETEMERGENCYCONTACTDRAFTMAXIMUMID = "GetEmergencyContactDraftMaximumId";
		private const string GETEMERGENCYCONTACTDRAFTROWCOUNT = "GetEmergencyContactDraftRowCount";	
		private const string GETEMERGENCYCONTACTDRAFTBYQUERY = "GetEmergencyContactDraftByQuery";
		#endregion
		
		#region Constructors
		public EmergencyContactDraftDataAccess(ClientContext context) : base(context) { }
		public EmergencyContactDraftDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="emergencyContactDraftObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmergencyContactDraftBase emergencyContactDraftObject)
		{	
			AddParameter(cmd, pGuid(EmergencyContactDraftBase.Property_CompanyId, emergencyContactDraftObject.CompanyId));
			AddParameter(cmd, pGuid(EmergencyContactDraftBase.Property_CustomerId, emergencyContactDraftObject.CustomerId));
			AddParameter(cmd, pNVarChar(EmergencyContactDraftBase.Property_CrossSteet, emergencyContactDraftObject.CrossSteet));
			AddParameter(cmd, pNVarChar(EmergencyContactDraftBase.Property_FirstName, 50, emergencyContactDraftObject.FirstName));
			AddParameter(cmd, pNVarChar(EmergencyContactDraftBase.Property_LastName, 50, emergencyContactDraftObject.LastName));
			AddParameter(cmd, pNVarChar(EmergencyContactDraftBase.Property_RelationShip, 50, emergencyContactDraftObject.RelationShip));
			AddParameter(cmd, pNVarChar(EmergencyContactDraftBase.Property_Email, 50, emergencyContactDraftObject.Email));
			AddParameter(cmd, pNVarChar(EmergencyContactDraftBase.Property_Phone, 50, emergencyContactDraftObject.Phone));
			AddParameter(cmd, pNVarChar(EmergencyContactDraftBase.Property_HasKey, 50, emergencyContactDraftObject.HasKey));
			AddParameter(cmd, pNVarChar(EmergencyContactDraftBase.Property_PhoneType, 50, emergencyContactDraftObject.PhoneType));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmergencyContactDraft
        /// </summary>
        /// <param name="emergencyContactDraftObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmergencyContactDraftBase emergencyContactDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMERGENCYCONTACTDRAFT);
	
				AddParameter(cmd, pInt32Out(EmergencyContactDraftBase.Property_Id));
				AddCommonParams(cmd, emergencyContactDraftObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					emergencyContactDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					emergencyContactDraftObject.Id = (Int32)GetOutParameter(cmd, EmergencyContactDraftBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(emergencyContactDraftObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmergencyContactDraft
        /// </summary>
        /// <param name="emergencyContactDraftObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmergencyContactDraftBase emergencyContactDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMERGENCYCONTACTDRAFT);
				
				AddParameter(cmd, pInt32(EmergencyContactDraftBase.Property_Id, emergencyContactDraftObject.Id));
				AddCommonParams(cmd, emergencyContactDraftObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					emergencyContactDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(emergencyContactDraftObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmergencyContactDraft
        /// </summary>
        /// <param name="Id">Id of the EmergencyContactDraft object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMERGENCYCONTACTDRAFT);	
				
				AddParameter(cmd, pInt32(EmergencyContactDraftBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmergencyContactDraft), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmergencyContactDraft object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmergencyContactDraft object to retrieve</param>
        /// <returns>EmergencyContactDraft object, null if not found</returns>
		public EmergencyContactDraft Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMERGENCYCONTACTDRAFTBYID))
			{
				AddParameter( cmd, pInt32(EmergencyContactDraftBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmergencyContactDraft objects 
        /// </summary>
        /// <returns>A list of EmergencyContactDraft objects</returns>
		public EmergencyContactDraftList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMERGENCYCONTACTDRAFT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmergencyContactDraft objects by PageRequest
        /// </summary>
        /// <returns>A list of EmergencyContactDraft objects</returns>
		public EmergencyContactDraftList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMERGENCYCONTACTDRAFT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmergencyContactDraftList _EmergencyContactDraftList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmergencyContactDraftList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmergencyContactDraft objects by query String
        /// </summary>
        /// <returns>A list of EmergencyContactDraft objects</returns>
		public EmergencyContactDraftList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMERGENCYCONTACTDRAFTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmergencyContactDraft Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmergencyContactDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMERGENCYCONTACTDRAFTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmergencyContactDraft Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmergencyContactDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmergencyContactDraftRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMERGENCYCONTACTDRAFTROWCOUNT))
			{
				SqlDataReader reader;
				_EmergencyContactDraftRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmergencyContactDraftRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmergencyContactDraft object
        /// </summary>
        /// <param name="emergencyContactDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmergencyContactDraftBase emergencyContactDraftObject, SqlDataReader reader, int start)
		{
			
				emergencyContactDraftObject.Id = reader.GetInt32( start + 0 );			
				emergencyContactDraftObject.CompanyId = reader.GetGuid( start + 1 );			
				emergencyContactDraftObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) emergencyContactDraftObject.CrossSteet = reader.GetString( start + 3 );			
				emergencyContactDraftObject.FirstName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) emergencyContactDraftObject.LastName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) emergencyContactDraftObject.RelationShip = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) emergencyContactDraftObject.Email = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) emergencyContactDraftObject.Phone = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) emergencyContactDraftObject.HasKey = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) emergencyContactDraftObject.PhoneType = reader.GetString( start + 10 );			
			FillBaseObject(emergencyContactDraftObject, reader, (start + 11));

			
			emergencyContactDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmergencyContactDraft object
        /// </summary>
        /// <param name="emergencyContactDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmergencyContactDraftBase emergencyContactDraftObject, SqlDataReader reader)
		{
			FillObject(emergencyContactDraftObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmergencyContactDraft object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmergencyContactDraft object</returns>
		private EmergencyContactDraft GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmergencyContactDraft emergencyContactDraftObject= new EmergencyContactDraft();
					FillObject(emergencyContactDraftObject, reader);
					return emergencyContactDraftObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmergencyContactDraft objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmergencyContactDraft objects</returns>
		private EmergencyContactDraftList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmergencyContactDraft list
			EmergencyContactDraftList list = new EmergencyContactDraftList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmergencyContactDraft emergencyContactDraftObject = new EmergencyContactDraft();
					FillObject(emergencyContactDraftObject, reader);

					list.Add(emergencyContactDraftObject);
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
