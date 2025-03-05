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
	public partial class ContactDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCONTACT = "InsertContact";
		private const string UPDATECONTACT = "UpdateContact";
		private const string DELETECONTACT = "DeleteContact";
		private const string GETCONTACTBYID = "GetContactById";
		private const string GETALLCONTACT = "GetAllContact";
		private const string GETPAGEDCONTACT = "GetPagedContact";
		private const string GETCONTACTMAXIMUMID = "GetContactMaximumId";
		private const string GETCONTACTROWCOUNT = "GetContactRowCount";	
		private const string GETCONTACTBYQUERY = "GetContactByQuery";
		#endregion
		
		#region Constructors
		public ContactDataAccess(ClientContext context) : base(context) { }
		public ContactDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="contactObject"></param>
		private void AddCommonParams(SqlCommand cmd, ContactBase contactObject)
		{	
			AddParameter(cmd, pGuid(ContactBase.Property_ContactId, contactObject.ContactId));
			AddParameter(cmd, pNVarChar(ContactBase.Property_FirstName, 100, contactObject.FirstName));
			AddParameter(cmd, pNVarChar(ContactBase.Property_LastName, 100, contactObject.LastName));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Suffix, 50, contactObject.Suffix));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Title, 50, contactObject.Title));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Work, 50, contactObject.Work));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Ext, 50, contactObject.Ext));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Mobile, 50, contactObject.Mobile));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Email, 100, contactObject.Email));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Role, 50, contactObject.Role));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Facebook, 100, contactObject.Facebook));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Twitter, 100, contactObject.Twitter));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Instagram, 100, contactObject.Instagram));
			AddParameter(cmd, pNVarChar(ContactBase.Property_LinkedIN, 100, contactObject.LinkedIN));
			AddParameter(cmd, pNVarChar(ContactBase.Property_Notes, contactObject.Notes));
			AddParameter(cmd, pGuid(ContactBase.Property_ContactOwner, contactObject.ContactOwner));
			AddParameter(cmd, pGuid(ContactBase.Property_CreatedBy, contactObject.CreatedBy));
			AddParameter(cmd, pDateTime(ContactBase.Property_CreatedDate, contactObject.CreatedDate));
			AddParameter(cmd, pNVarChar(ContactBase.Property_ContactType, 50, contactObject.ContactType));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Contact
        /// </summary>
        /// <param name="contactObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ContactBase contactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCONTACT);
	
				AddParameter(cmd, pInt32Out(ContactBase.Property_Id));
				AddCommonParams(cmd, contactObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					contactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					contactObject.Id = (Int32)GetOutParameter(cmd, ContactBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(contactObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Contact
        /// </summary>
        /// <param name="contactObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ContactBase contactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECONTACT);
				
				AddParameter(cmd, pInt32(ContactBase.Property_Id, contactObject.Id));
				AddCommonParams(cmd, contactObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					contactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(contactObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Contact
        /// </summary>
        /// <param name="Id">Id of the Contact object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECONTACT);	
				
				AddParameter(cmd, pInt32(ContactBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Contact), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Contact object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Contact object to retrieve</param>
        /// <returns>Contact object, null if not found</returns>
		public Contact Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCONTACTBYID))
			{
				AddParameter( cmd, pInt32(ContactBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Contact objects 
        /// </summary>
        /// <returns>A list of Contact objects</returns>
		public ContactList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCONTACT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Contact objects by PageRequest
        /// </summary>
        /// <returns>A list of Contact objects</returns>
		public ContactList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCONTACT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ContactList _ContactList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ContactList;
			}
		}
		
		/// <summary>
        /// Retrieves all Contact objects by query String
        /// </summary>
        /// <returns>A list of Contact objects</returns>
		public ContactList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCONTACTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Contact Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Contact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCONTACTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Contact Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Contact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ContactRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCONTACTROWCOUNT))
			{
				SqlDataReader reader;
				_ContactRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ContactRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Contact object
        /// </summary>
        /// <param name="contactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ContactBase contactObject, SqlDataReader reader, int start)
		{
			
				contactObject.Id = reader.GetInt32( start + 0 );			
				contactObject.ContactId = reader.GetGuid( start + 1 );			
				contactObject.FirstName = reader.GetString( start + 2 );			
				contactObject.LastName = reader.GetString( start + 3 );			
				contactObject.Suffix = reader.GetString( start + 4 );			
				contactObject.Title = reader.GetString( start + 5 );			
				contactObject.Work = reader.GetString( start + 6 );			
				contactObject.Ext = reader.GetString( start + 7 );			
				contactObject.Mobile = reader.GetString( start + 8 );			
				contactObject.Email = reader.GetString( start + 9 );			
				contactObject.Role = reader.GetString( start + 10 );			
				contactObject.Facebook = reader.GetString( start + 11 );			
				contactObject.Twitter = reader.GetString( start + 12 );			
				contactObject.Instagram = reader.GetString( start + 13 );			
				contactObject.LinkedIN = reader.GetString( start + 14 );			
				contactObject.Notes = reader.GetString( start + 15 );			
				contactObject.ContactOwner = reader.GetGuid( start + 16 );			
				contactObject.CreatedBy = reader.GetGuid( start + 17 );			
				contactObject.CreatedDate = reader.GetDateTime( start + 18 );			
				if(!reader.IsDBNull(19)) contactObject.ContactType = reader.GetString( start + 19 );			
			FillBaseObject(contactObject, reader, (start + 20));

			
			contactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Contact object
        /// </summary>
        /// <param name="contactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ContactBase contactObject, SqlDataReader reader)
		{
			FillObject(contactObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Contact object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Contact object</returns>
		private Contact GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Contact contactObject= new Contact();
					FillObject(contactObject, reader);
					return contactObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Contact objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Contact objects</returns>
		private ContactList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Contact list
			ContactList list = new ContactList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Contact contactObject = new Contact();
					FillObject(contactObject, reader);

					list.Add(contactObject);
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
