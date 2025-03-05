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
	public partial class ThirdPartyEmergencyContactDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTHIRDPARTYEMERGENCYCONTACT = "InsertThirdPartyEmergencyContact";
		private const string UPDATETHIRDPARTYEMERGENCYCONTACT = "UpdateThirdPartyEmergencyContact";
		private const string DELETETHIRDPARTYEMERGENCYCONTACT = "DeleteThirdPartyEmergencyContact";
		private const string GETTHIRDPARTYEMERGENCYCONTACTBYID = "GetThirdPartyEmergencyContactById";
		private const string GETALLTHIRDPARTYEMERGENCYCONTACT = "GetAllThirdPartyEmergencyContact";
		private const string GETPAGEDTHIRDPARTYEMERGENCYCONTACT = "GetPagedThirdPartyEmergencyContact";
		private const string GETTHIRDPARTYEMERGENCYCONTACTMAXIMUMID = "GetThirdPartyEmergencyContactMaximumId";
		private const string GETTHIRDPARTYEMERGENCYCONTACTROWCOUNT = "GetThirdPartyEmergencyContactRowCount";	
		private const string GETTHIRDPARTYEMERGENCYCONTACTBYQUERY = "GetThirdPartyEmergencyContactByQuery";
		#endregion
		
		#region Constructors
		public ThirdPartyEmergencyContactDataAccess(ClientContext context) : base(context) { }
		public ThirdPartyEmergencyContactDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="thirdPartyEmergencyContactObject"></param>
		private void AddCommonParams(SqlCommand cmd, ThirdPartyEmergencyContactBase thirdPartyEmergencyContactObject)
		{	
			AddParameter(cmd, pGuid(ThirdPartyEmergencyContactBase.Property_CustomerId, thirdPartyEmergencyContactObject.CustomerId));
			AddParameter(cmd, pNVarChar(ThirdPartyEmergencyContactBase.Property_FirstName, 50, thirdPartyEmergencyContactObject.FirstName));
			AddParameter(cmd, pNVarChar(ThirdPartyEmergencyContactBase.Property_LastName, 50, thirdPartyEmergencyContactObject.LastName));
			AddParameter(cmd, pNVarChar(ThirdPartyEmergencyContactBase.Property_Relation, 50, thirdPartyEmergencyContactObject.Relation));
			AddParameter(cmd, pNVarChar(ThirdPartyEmergencyContactBase.Property_Phone, 50, thirdPartyEmergencyContactObject.Phone));
			AddParameter(cmd, pNVarChar(ThirdPartyEmergencyContactBase.Property_Email, 50, thirdPartyEmergencyContactObject.Email));
			AddParameter(cmd, pNVarChar(ThirdPartyEmergencyContactBase.Property_ContactNo, 50, thirdPartyEmergencyContactObject.ContactNo));
			AddParameter(cmd, pNVarChar(ThirdPartyEmergencyContactBase.Property_CtacLink, 50, thirdPartyEmergencyContactObject.CtacLink));
			AddParameter(cmd, pNVarChar(ThirdPartyEmergencyContactBase.Property_Platform, 50, thirdPartyEmergencyContactObject.Platform));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ThirdPartyEmergencyContact
        /// </summary>
        /// <param name="thirdPartyEmergencyContactObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ThirdPartyEmergencyContactBase thirdPartyEmergencyContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTHIRDPARTYEMERGENCYCONTACT);
	
				AddParameter(cmd, pInt32Out(ThirdPartyEmergencyContactBase.Property_Id));
				AddCommonParams(cmd, thirdPartyEmergencyContactObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					thirdPartyEmergencyContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					thirdPartyEmergencyContactObject.Id = (Int32)GetOutParameter(cmd, ThirdPartyEmergencyContactBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(thirdPartyEmergencyContactObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ThirdPartyEmergencyContact
        /// </summary>
        /// <param name="thirdPartyEmergencyContactObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ThirdPartyEmergencyContactBase thirdPartyEmergencyContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETHIRDPARTYEMERGENCYCONTACT);
				
				AddParameter(cmd, pInt32(ThirdPartyEmergencyContactBase.Property_Id, thirdPartyEmergencyContactObject.Id));
				AddCommonParams(cmd, thirdPartyEmergencyContactObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					thirdPartyEmergencyContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(thirdPartyEmergencyContactObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ThirdPartyEmergencyContact
        /// </summary>
        /// <param name="Id">Id of the ThirdPartyEmergencyContact object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETHIRDPARTYEMERGENCYCONTACT);	
				
				AddParameter(cmd, pInt32(ThirdPartyEmergencyContactBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ThirdPartyEmergencyContact), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ThirdPartyEmergencyContact object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ThirdPartyEmergencyContact object to retrieve</param>
        /// <returns>ThirdPartyEmergencyContact object, null if not found</returns>
		public ThirdPartyEmergencyContact Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYEMERGENCYCONTACTBYID))
			{
				AddParameter( cmd, pInt32(ThirdPartyEmergencyContactBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ThirdPartyEmergencyContact objects 
        /// </summary>
        /// <returns>A list of ThirdPartyEmergencyContact objects</returns>
		public ThirdPartyEmergencyContactList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTHIRDPARTYEMERGENCYCONTACT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ThirdPartyEmergencyContact objects by PageRequest
        /// </summary>
        /// <returns>A list of ThirdPartyEmergencyContact objects</returns>
		public ThirdPartyEmergencyContactList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTHIRDPARTYEMERGENCYCONTACT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ThirdPartyEmergencyContactList _ThirdPartyEmergencyContactList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ThirdPartyEmergencyContactList;
			}
		}
		
		/// <summary>
        /// Retrieves all ThirdPartyEmergencyContact objects by query String
        /// </summary>
        /// <returns>A list of ThirdPartyEmergencyContact objects</returns>
		public ThirdPartyEmergencyContactList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYEMERGENCYCONTACTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ThirdPartyEmergencyContact Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ThirdPartyEmergencyContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYEMERGENCYCONTACTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ThirdPartyEmergencyContact Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ThirdPartyEmergencyContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ThirdPartyEmergencyContactRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYEMERGENCYCONTACTROWCOUNT))
			{
				SqlDataReader reader;
				_ThirdPartyEmergencyContactRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ThirdPartyEmergencyContactRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ThirdPartyEmergencyContact object
        /// </summary>
        /// <param name="thirdPartyEmergencyContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ThirdPartyEmergencyContactBase thirdPartyEmergencyContactObject, SqlDataReader reader, int start)
		{
			
				thirdPartyEmergencyContactObject.Id = reader.GetInt32( start + 0 );			
				thirdPartyEmergencyContactObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) thirdPartyEmergencyContactObject.FirstName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) thirdPartyEmergencyContactObject.LastName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) thirdPartyEmergencyContactObject.Relation = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) thirdPartyEmergencyContactObject.Phone = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) thirdPartyEmergencyContactObject.Email = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) thirdPartyEmergencyContactObject.ContactNo = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) thirdPartyEmergencyContactObject.CtacLink = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) thirdPartyEmergencyContactObject.Platform = reader.GetString( start + 9 );			
			FillBaseObject(thirdPartyEmergencyContactObject, reader, (start + 10));

			
			thirdPartyEmergencyContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ThirdPartyEmergencyContact object
        /// </summary>
        /// <param name="thirdPartyEmergencyContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ThirdPartyEmergencyContactBase thirdPartyEmergencyContactObject, SqlDataReader reader)
		{
			FillObject(thirdPartyEmergencyContactObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ThirdPartyEmergencyContact object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ThirdPartyEmergencyContact object</returns>
		private ThirdPartyEmergencyContact GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ThirdPartyEmergencyContact thirdPartyEmergencyContactObject= new ThirdPartyEmergencyContact();
					FillObject(thirdPartyEmergencyContactObject, reader);
					return thirdPartyEmergencyContactObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ThirdPartyEmergencyContact objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ThirdPartyEmergencyContact objects</returns>
		private ThirdPartyEmergencyContactList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ThirdPartyEmergencyContact list
			ThirdPartyEmergencyContactList list = new ThirdPartyEmergencyContactList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ThirdPartyEmergencyContact thirdPartyEmergencyContactObject = new ThirdPartyEmergencyContact();
					FillObject(thirdPartyEmergencyContactObject, reader);

					list.Add(thirdPartyEmergencyContactObject);
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
