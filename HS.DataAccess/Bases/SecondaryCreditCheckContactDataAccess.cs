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
	public partial class SecondaryCreditCheckContactDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSECONDARYCREDITCHECKCONTACT = "InsertSecondaryCreditCheckContact";
		private const string UPDATESECONDARYCREDITCHECKCONTACT = "UpdateSecondaryCreditCheckContact";
		private const string DELETESECONDARYCREDITCHECKCONTACT = "DeleteSecondaryCreditCheckContact";
		private const string GETSECONDARYCREDITCHECKCONTACTBYID = "GetSecondaryCreditCheckContactById";
		private const string GETALLSECONDARYCREDITCHECKCONTACT = "GetAllSecondaryCreditCheckContact";
		private const string GETPAGEDSECONDARYCREDITCHECKCONTACT = "GetPagedSecondaryCreditCheckContact";
		private const string GETSECONDARYCREDITCHECKCONTACTMAXIMUMID = "GetSecondaryCreditCheckContactMaximumId";
		private const string GETSECONDARYCREDITCHECKCONTACTROWCOUNT = "GetSecondaryCreditCheckContactRowCount";	
		private const string GETSECONDARYCREDITCHECKCONTACTBYQUERY = "GetSecondaryCreditCheckContactByQuery";
		#endregion
		
		#region Constructors
		public SecondaryCreditCheckContactDataAccess(ClientContext context) : base(context) { }
		public SecondaryCreditCheckContactDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="secondaryCreditCheckContactObject"></param>
		private void AddCommonParams(SqlCommand cmd, SecondaryCreditCheckContactBase secondaryCreditCheckContactObject)
		{	
			AddParameter(cmd, pGuid(SecondaryCreditCheckContactBase.Property_SecondaryContactId, secondaryCreditCheckContactObject.SecondaryContactId));
			AddParameter(cmd, pGuid(SecondaryCreditCheckContactBase.Property_CustomerId, secondaryCreditCheckContactObject.CustomerId));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_FirstName, 50, secondaryCreditCheckContactObject.FirstName));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_LastName, 50, secondaryCreditCheckContactObject.LastName));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_City, 50, secondaryCreditCheckContactObject.City));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_State, 50, secondaryCreditCheckContactObject.State));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_Zip, 50, secondaryCreditCheckContactObject.Zip));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_Street, 200, secondaryCreditCheckContactObject.Street));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_SSN, 50, secondaryCreditCheckContactObject.SSN));
			AddParameter(cmd, pDateTime(SecondaryCreditCheckContactBase.Property_DateOfBirth, secondaryCreditCheckContactObject.DateOfBirth));
			AddParameter(cmd, pBool(SecondaryCreditCheckContactBase.Property_IsUsed, secondaryCreditCheckContactObject.IsUsed));
			AddParameter(cmd, pDateTime(SecondaryCreditCheckContactBase.Property_CreatedDate, secondaryCreditCheckContactObject.CreatedDate));
			AddParameter(cmd, pGuid(SecondaryCreditCheckContactBase.Property_CreatedBy, secondaryCreditCheckContactObject.CreatedBy));
			AddParameter(cmd, pInt32(SecondaryCreditCheckContactBase.Property_CreditScore, secondaryCreditCheckContactObject.CreditScore));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_Email, 100, secondaryCreditCheckContactObject.Email));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_Phone, 100, secondaryCreditCheckContactObject.Phone));
			AddParameter(cmd, pBool(SecondaryCreditCheckContactBase.Property_IsForSecondarySign, secondaryCreditCheckContactObject.IsForSecondarySign));
			AddParameter(cmd, pNVarChar(SecondaryCreditCheckContactBase.Property_ReportPdfLink, 200, secondaryCreditCheckContactObject.ReportPdfLink));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SecondaryCreditCheckContact
        /// </summary>
        /// <param name="secondaryCreditCheckContactObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SecondaryCreditCheckContactBase secondaryCreditCheckContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSECONDARYCREDITCHECKCONTACT);
	
				AddParameter(cmd, pInt32Out(SecondaryCreditCheckContactBase.Property_Id));
				AddCommonParams(cmd, secondaryCreditCheckContactObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					secondaryCreditCheckContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					secondaryCreditCheckContactObject.Id = (Int32)GetOutParameter(cmd, SecondaryCreditCheckContactBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(secondaryCreditCheckContactObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SecondaryCreditCheckContact
        /// </summary>
        /// <param name="secondaryCreditCheckContactObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SecondaryCreditCheckContactBase secondaryCreditCheckContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESECONDARYCREDITCHECKCONTACT);
				
				AddParameter(cmd, pInt32(SecondaryCreditCheckContactBase.Property_Id, secondaryCreditCheckContactObject.Id));
				AddCommonParams(cmd, secondaryCreditCheckContactObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					secondaryCreditCheckContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(secondaryCreditCheckContactObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SecondaryCreditCheckContact
        /// </summary>
        /// <param name="Id">Id of the SecondaryCreditCheckContact object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESECONDARYCREDITCHECKCONTACT);	
				
				AddParameter(cmd, pInt32(SecondaryCreditCheckContactBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SecondaryCreditCheckContact), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SecondaryCreditCheckContact object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SecondaryCreditCheckContact object to retrieve</param>
        /// <returns>SecondaryCreditCheckContact object, null if not found</returns>
		public SecondaryCreditCheckContact Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSECONDARYCREDITCHECKCONTACTBYID))
			{
				AddParameter( cmd, pInt32(SecondaryCreditCheckContactBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SecondaryCreditCheckContact objects 
        /// </summary>
        /// <returns>A list of SecondaryCreditCheckContact objects</returns>
		public SecondaryCreditCheckContactList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSECONDARYCREDITCHECKCONTACT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SecondaryCreditCheckContact objects by PageRequest
        /// </summary>
        /// <returns>A list of SecondaryCreditCheckContact objects</returns>
		public SecondaryCreditCheckContactList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSECONDARYCREDITCHECKCONTACT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SecondaryCreditCheckContactList _SecondaryCreditCheckContactList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SecondaryCreditCheckContactList;
			}
		}
		
		/// <summary>
        /// Retrieves all SecondaryCreditCheckContact objects by query String
        /// </summary>
        /// <returns>A list of SecondaryCreditCheckContact objects</returns>
		public SecondaryCreditCheckContactList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSECONDARYCREDITCHECKCONTACTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SecondaryCreditCheckContact Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SecondaryCreditCheckContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSECONDARYCREDITCHECKCONTACTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SecondaryCreditCheckContact Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SecondaryCreditCheckContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SecondaryCreditCheckContactRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSECONDARYCREDITCHECKCONTACTROWCOUNT))
			{
				SqlDataReader reader;
				_SecondaryCreditCheckContactRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SecondaryCreditCheckContactRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SecondaryCreditCheckContact object
        /// </summary>
        /// <param name="secondaryCreditCheckContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SecondaryCreditCheckContactBase secondaryCreditCheckContactObject, SqlDataReader reader, int start)
		{
			
				secondaryCreditCheckContactObject.Id = reader.GetInt32( start + 0 );			
				secondaryCreditCheckContactObject.SecondaryContactId = reader.GetGuid( start + 1 );			
				secondaryCreditCheckContactObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) secondaryCreditCheckContactObject.FirstName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) secondaryCreditCheckContactObject.LastName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) secondaryCreditCheckContactObject.City = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) secondaryCreditCheckContactObject.State = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) secondaryCreditCheckContactObject.Zip = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) secondaryCreditCheckContactObject.Street = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) secondaryCreditCheckContactObject.SSN = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) secondaryCreditCheckContactObject.DateOfBirth = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) secondaryCreditCheckContactObject.IsUsed = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) secondaryCreditCheckContactObject.CreatedDate = reader.GetDateTime( start + 12 );			
				secondaryCreditCheckContactObject.CreatedBy = reader.GetGuid( start + 13 );			
				if(!reader.IsDBNull(14)) secondaryCreditCheckContactObject.CreditScore = reader.GetInt32( start + 14 );			
				if(!reader.IsDBNull(15)) secondaryCreditCheckContactObject.Email = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) secondaryCreditCheckContactObject.Phone = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) secondaryCreditCheckContactObject.IsForSecondarySign = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) secondaryCreditCheckContactObject.ReportPdfLink = reader.GetString( start + 18 );			
			FillBaseObject(secondaryCreditCheckContactObject, reader, (start + 19));

			
			secondaryCreditCheckContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SecondaryCreditCheckContact object
        /// </summary>
        /// <param name="secondaryCreditCheckContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SecondaryCreditCheckContactBase secondaryCreditCheckContactObject, SqlDataReader reader)
		{
			FillObject(secondaryCreditCheckContactObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SecondaryCreditCheckContact object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SecondaryCreditCheckContact object</returns>
		private SecondaryCreditCheckContact GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SecondaryCreditCheckContact secondaryCreditCheckContactObject= new SecondaryCreditCheckContact();
					FillObject(secondaryCreditCheckContactObject, reader);
					return secondaryCreditCheckContactObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SecondaryCreditCheckContact objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SecondaryCreditCheckContact objects</returns>
		private SecondaryCreditCheckContactList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SecondaryCreditCheckContact list
			SecondaryCreditCheckContactList list = new SecondaryCreditCheckContactList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SecondaryCreditCheckContact secondaryCreditCheckContactObject = new SecondaryCreditCheckContact();
					FillObject(secondaryCreditCheckContactObject, reader);

					list.Add(secondaryCreditCheckContactObject);
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
