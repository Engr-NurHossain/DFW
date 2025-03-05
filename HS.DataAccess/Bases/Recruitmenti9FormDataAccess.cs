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
	public partial class Recruitmenti9FormDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRECRUITMENTI9FORM = "InsertRecruitmenti9Form";
		private const string UPDATERECRUITMENTI9FORM = "UpdateRecruitmenti9Form";
		private const string DELETERECRUITMENTI9FORM = "DeleteRecruitmenti9Form";
		private const string GETRECRUITMENTI9FORMBYID = "GetRecruitmenti9FormById";
		private const string GETALLRECRUITMENTI9FORM = "GetAllRecruitmenti9Form";
		private const string GETPAGEDRECRUITMENTI9FORM = "GetPagedRecruitmenti9Form";
		private const string GETRECRUITMENTI9FORMMAXIMUMID = "GetRecruitmenti9FormMaximumId";
		private const string GETRECRUITMENTI9FORMROWCOUNT = "GetRecruitmenti9FormRowCount";	
		private const string GETRECRUITMENTI9FORMBYQUERY = "GetRecruitmenti9FormByQuery";
		#endregion
		
		#region Constructors
		public Recruitmenti9FormDataAccess(ClientContext context) : base(context) { }
		public Recruitmenti9FormDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="recruitmenti9FormObject"></param>
		private void AddCommonParams(SqlCommand cmd, Recruitmenti9FormBase recruitmenti9FormObject)
		{	
			AddParameter(cmd, pGuid(Recruitmenti9FormBase.Property_FormId, recruitmenti9FormObject.FormId));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_FirstName, 50, recruitmenti9FormObject.FirstName));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_LastName, 50, recruitmenti9FormObject.LastName));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_MiddleInitial, 50, recruitmenti9FormObject.MiddleInitial));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_MaidenName, 50, recruitmenti9FormObject.MaidenName));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_DOB, recruitmenti9FormObject.DOB));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_SSN, 50, recruitmenti9FormObject.SSN));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Address, 250, recruitmenti9FormObject.Address));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Apartment, 50, recruitmenti9FormObject.Apartment));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_City, 50, recruitmenti9FormObject.City));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_State, 50, recruitmenti9FormObject.State));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_ZipCode, 50, recruitmenti9FormObject.ZipCode));
			AddParameter(cmd, pBool(Recruitmenti9FormBase.Property_USCitizen, recruitmenti9FormObject.USCitizen));
			AddParameter(cmd, pBool(Recruitmenti9FormBase.Property_NoncitizenNational, recruitmenti9FormObject.NoncitizenNational));
			AddParameter(cmd, pBool(Recruitmenti9FormBase.Property_LawfulPermanentResident, recruitmenti9FormObject.LawfulPermanentResident));
			AddParameter(cmd, pBool(Recruitmenti9FormBase.Property_AlienAuthorizedToWork, recruitmenti9FormObject.AlienAuthorizedToWork));
			AddParameter(cmd, pBool(Recruitmenti9FormBase.Property_UntilExp, recruitmenti9FormObject.UntilExp));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Signature, 250, recruitmenti9FormObject.Signature));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_SignatureDate, recruitmenti9FormObject.SignatureDate));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_TransSignature, 250, recruitmenti9FormObject.TransSignature));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_TransPrintName, 50, recruitmenti9FormObject.TransPrintName));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_TransAddress, 250, recruitmenti9FormObject.TransAddress));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_TransSignaturedate, recruitmenti9FormObject.TransSignaturedate));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_DocTitleListA, 150, recruitmenti9FormObject.DocTitleListA));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_DoctTitleListB, 150, recruitmenti9FormObject.DoctTitleListB));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_DoctTitleListC, 150, recruitmenti9FormObject.DoctTitleListC));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_IssuingAuthorityListA, 150, recruitmenti9FormObject.IssuingAuthorityListA));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_IssuingAuthorityListB, 150, recruitmenti9FormObject.IssuingAuthorityListB));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_IssuingAuthorityListC, 150, recruitmenti9FormObject.IssuingAuthorityListC));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Doc1ListA, 150, recruitmenti9FormObject.Doc1ListA));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Doc1ListB, 150, recruitmenti9FormObject.Doc1ListB));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Doc1ListC, 150, recruitmenti9FormObject.Doc1ListC));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_Exp1ListA, recruitmenti9FormObject.Exp1ListA));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_Exp1ListB, recruitmenti9FormObject.Exp1ListB));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_Exp1ListC, recruitmenti9FormObject.Exp1ListC));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Doc2ListA, 150, recruitmenti9FormObject.Doc2ListA));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Doc2ListB, 150, recruitmenti9FormObject.Doc2ListB));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_Doc2ListC, 150, recruitmenti9FormObject.Doc2ListC));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_Exp2ListA, recruitmenti9FormObject.Exp2ListA));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_Exp2ListB, recruitmenti9FormObject.Exp2ListB));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_Exp2ListC, recruitmenti9FormObject.Exp2ListC));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_BeganEmploymentOn, recruitmenti9FormObject.BeganEmploymentOn));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_AuthRepresentativeSignature, 250, recruitmenti9FormObject.AuthRepresentativeSignature));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_AuthRepresentativeName, 150, recruitmenti9FormObject.AuthRepresentativeName));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_AuthRepresentativeTitle, 50, recruitmenti9FormObject.AuthRepresentativeTitle));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_AuthRepSignatureDate, recruitmenti9FormObject.AuthRepSignatureDate));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_OrgNameAndAddress, 250, recruitmenti9FormObject.OrgNameAndAddress));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_NewName, 150, recruitmenti9FormObject.NewName));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_DateOfRehire, recruitmenti9FormObject.DateOfRehire));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_PrevDocTitle, 50, recruitmenti9FormObject.PrevDocTitle));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_PrevDocNo, 50, recruitmenti9FormObject.PrevDocNo));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_PrevDocExp, recruitmenti9FormObject.PrevDocExp));
			AddParameter(cmd, pNVarChar(Recruitmenti9FormBase.Property_AuthRepSignature2, 250, recruitmenti9FormObject.AuthRepSignature2));
			AddParameter(cmd, pDateTime(Recruitmenti9FormBase.Property_AuthRepSignatureDate2, recruitmenti9FormObject.AuthRepSignatureDate2));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Recruitmenti9Form
        /// </summary>
        /// <param name="recruitmenti9FormObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(Recruitmenti9FormBase recruitmenti9FormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRECRUITMENTI9FORM);
	
				AddParameter(cmd, pInt32Out(Recruitmenti9FormBase.Property_Id));
				AddCommonParams(cmd, recruitmenti9FormObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					recruitmenti9FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					recruitmenti9FormObject.Id = (Int32)GetOutParameter(cmd, Recruitmenti9FormBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(recruitmenti9FormObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Recruitmenti9Form
        /// </summary>
        /// <param name="recruitmenti9FormObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(Recruitmenti9FormBase recruitmenti9FormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERECRUITMENTI9FORM);
				
				AddParameter(cmd, pInt32(Recruitmenti9FormBase.Property_Id, recruitmenti9FormObject.Id));
				AddCommonParams(cmd, recruitmenti9FormObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					recruitmenti9FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(recruitmenti9FormObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Recruitmenti9Form
        /// </summary>
        /// <param name="Id">Id of the Recruitmenti9Form object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERECRUITMENTI9FORM);	
				
				AddParameter(cmd, pInt32(Recruitmenti9FormBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Recruitmenti9Form), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Recruitmenti9Form object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Recruitmenti9Form object to retrieve</param>
        /// <returns>Recruitmenti9Form object, null if not found</returns>
		public Recruitmenti9Form Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTI9FORMBYID))
			{
				AddParameter( cmd, pInt32(Recruitmenti9FormBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Recruitmenti9Form objects 
        /// </summary>
        /// <returns>A list of Recruitmenti9Form objects</returns>
		public Recruitmenti9FormList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRECRUITMENTI9FORM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Recruitmenti9Form objects by PageRequest
        /// </summary>
        /// <returns>A list of Recruitmenti9Form objects</returns>
		public Recruitmenti9FormList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRECRUITMENTI9FORM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				Recruitmenti9FormList _Recruitmenti9FormList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _Recruitmenti9FormList;
			}
		}
		
		/// <summary>
        /// Retrieves all Recruitmenti9Form objects by query String
        /// </summary>
        /// <returns>A list of Recruitmenti9Form objects</returns>
		public Recruitmenti9FormList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTI9FORMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Recruitmenti9Form Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Recruitmenti9Form
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTI9FORMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Recruitmenti9Form Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Recruitmenti9Form
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _Recruitmenti9FormRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTI9FORMROWCOUNT))
			{
				SqlDataReader reader;
				_Recruitmenti9FormRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _Recruitmenti9FormRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Recruitmenti9Form object
        /// </summary>
        /// <param name="recruitmenti9FormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(Recruitmenti9FormBase recruitmenti9FormObject, SqlDataReader reader, int start)
		{
			
				recruitmenti9FormObject.Id = reader.GetInt32( start + 0 );			
				recruitmenti9FormObject.FormId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) recruitmenti9FormObject.FirstName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) recruitmenti9FormObject.LastName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) recruitmenti9FormObject.MiddleInitial = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) recruitmenti9FormObject.MaidenName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) recruitmenti9FormObject.DOB = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) recruitmenti9FormObject.SSN = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) recruitmenti9FormObject.Address = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) recruitmenti9FormObject.Apartment = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) recruitmenti9FormObject.City = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) recruitmenti9FormObject.State = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) recruitmenti9FormObject.ZipCode = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) recruitmenti9FormObject.USCitizen = reader.GetBoolean( start + 13 );			
				if(!reader.IsDBNull(14)) recruitmenti9FormObject.NoncitizenNational = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) recruitmenti9FormObject.LawfulPermanentResident = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) recruitmenti9FormObject.AlienAuthorizedToWork = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) recruitmenti9FormObject.UntilExp = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) recruitmenti9FormObject.Signature = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) recruitmenti9FormObject.SignatureDate = reader.GetDateTime( start + 19 );			
				if(!reader.IsDBNull(20)) recruitmenti9FormObject.TransSignature = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) recruitmenti9FormObject.TransPrintName = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) recruitmenti9FormObject.TransAddress = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) recruitmenti9FormObject.TransSignaturedate = reader.GetDateTime( start + 23 );			
				if(!reader.IsDBNull(24)) recruitmenti9FormObject.DocTitleListA = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) recruitmenti9FormObject.DoctTitleListB = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) recruitmenti9FormObject.DoctTitleListC = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) recruitmenti9FormObject.IssuingAuthorityListA = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) recruitmenti9FormObject.IssuingAuthorityListB = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) recruitmenti9FormObject.IssuingAuthorityListC = reader.GetString( start + 29 );			
				if(!reader.IsDBNull(30)) recruitmenti9FormObject.Doc1ListA = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) recruitmenti9FormObject.Doc1ListB = reader.GetString( start + 31 );			
				if(!reader.IsDBNull(32)) recruitmenti9FormObject.Doc1ListC = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) recruitmenti9FormObject.Exp1ListA = reader.GetDateTime( start + 33 );			
				if(!reader.IsDBNull(34)) recruitmenti9FormObject.Exp1ListB = reader.GetDateTime( start + 34 );			
				if(!reader.IsDBNull(35)) recruitmenti9FormObject.Exp1ListC = reader.GetDateTime( start + 35 );			
				if(!reader.IsDBNull(36)) recruitmenti9FormObject.Doc2ListA = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) recruitmenti9FormObject.Doc2ListB = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) recruitmenti9FormObject.Doc2ListC = reader.GetString( start + 38 );			
				if(!reader.IsDBNull(39)) recruitmenti9FormObject.Exp2ListA = reader.GetDateTime( start + 39 );			
				if(!reader.IsDBNull(40)) recruitmenti9FormObject.Exp2ListB = reader.GetDateTime( start + 40 );			
				if(!reader.IsDBNull(41)) recruitmenti9FormObject.Exp2ListC = reader.GetDateTime( start + 41 );			
				if(!reader.IsDBNull(42)) recruitmenti9FormObject.BeganEmploymentOn = reader.GetDateTime( start + 42 );			
				if(!reader.IsDBNull(43)) recruitmenti9FormObject.AuthRepresentativeSignature = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) recruitmenti9FormObject.AuthRepresentativeName = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) recruitmenti9FormObject.AuthRepresentativeTitle = reader.GetString( start + 45 );			
				if(!reader.IsDBNull(46)) recruitmenti9FormObject.AuthRepSignatureDate = reader.GetDateTime( start + 46 );			
				if(!reader.IsDBNull(47)) recruitmenti9FormObject.OrgNameAndAddress = reader.GetString( start + 47 );			
				if(!reader.IsDBNull(48)) recruitmenti9FormObject.NewName = reader.GetString( start + 48 );			
				if(!reader.IsDBNull(49)) recruitmenti9FormObject.DateOfRehire = reader.GetDateTime( start + 49 );			
				if(!reader.IsDBNull(50)) recruitmenti9FormObject.PrevDocTitle = reader.GetString( start + 50 );			
				if(!reader.IsDBNull(51)) recruitmenti9FormObject.PrevDocNo = reader.GetString( start + 51 );			
				if(!reader.IsDBNull(52)) recruitmenti9FormObject.PrevDocExp = reader.GetDateTime( start + 52 );			
				if(!reader.IsDBNull(53)) recruitmenti9FormObject.AuthRepSignature2 = reader.GetString( start + 53 );			
				if(!reader.IsDBNull(54)) recruitmenti9FormObject.AuthRepSignatureDate2 = reader.GetDateTime( start + 54 );			
			FillBaseObject(recruitmenti9FormObject, reader, (start + 55));

			
			recruitmenti9FormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Recruitmenti9Form object
        /// </summary>
        /// <param name="recruitmenti9FormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(Recruitmenti9FormBase recruitmenti9FormObject, SqlDataReader reader)
		{
			FillObject(recruitmenti9FormObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Recruitmenti9Form object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Recruitmenti9Form object</returns>
		private Recruitmenti9Form GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Recruitmenti9Form recruitmenti9FormObject= new Recruitmenti9Form();
					FillObject(recruitmenti9FormObject, reader);
					return recruitmenti9FormObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Recruitmenti9Form objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Recruitmenti9Form objects</returns>
		private Recruitmenti9FormList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Recruitmenti9Form list
			Recruitmenti9FormList list = new Recruitmenti9FormList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Recruitmenti9Form recruitmenti9FormObject = new Recruitmenti9Form();
					FillObject(recruitmenti9FormObject, reader);

					list.Add(recruitmenti9FormObject);
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
