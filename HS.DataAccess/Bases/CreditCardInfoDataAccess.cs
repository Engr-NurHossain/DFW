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
	public partial class CreditCardInfoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCREDITCARDINFO = "InsertCreditCardInfo";
		private const string UPDATECREDITCARDINFO = "UpdateCreditCardInfo";
		private const string DELETECREDITCARDINFO = "DeleteCreditCardInfo";
		private const string GETCREDITCARDINFOBYID = "GetCreditCardInfoById";
		private const string GETALLCREDITCARDINFO = "GetAllCreditCardInfo";
		private const string GETPAGEDCREDITCARDINFO = "GetPagedCreditCardInfo";
		private const string GETCREDITCARDINFOMAXIMUMID = "GetCreditCardInfoMaximumId";
		private const string GETCREDITCARDINFOROWCOUNT = "GetCreditCardInfoRowCount";	
		private const string GETCREDITCARDINFOBYQUERY = "GetCreditCardInfoByQuery";
		#endregion
		
		#region Constructors
		public CreditCardInfoDataAccess(ClientContext context) : base(context) { }
		public CreditCardInfoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="creditCardInfoObject"></param>
		private void AddCommonParams(SqlCommand cmd, CreditCardInfoBase creditCardInfoObject)
		{	
			AddParameter(cmd, pGuid(CreditCardInfoBase.Property_CompanyId, creditCardInfoObject.CompanyId));
			AddParameter(cmd, pGuid(CreditCardInfoBase.Property_CustomerId, creditCardInfoObject.CustomerId));
			AddParameter(cmd, pNVarChar(CreditCardInfoBase.Property_CardType, 50, creditCardInfoObject.CardType));
			AddParameter(cmd, pNVarChar(CreditCardInfoBase.Property_CardNumber, 16, creditCardInfoObject.CardNumber));
			AddParameter(cmd, pNVarChar(CreditCardInfoBase.Property_CardName, 50, creditCardInfoObject.CardName));
			AddParameter(cmd, pInt32(CreditCardInfoBase.Property_ExpireMonth, creditCardInfoObject.ExpireMonth));
			AddParameter(cmd, pInt32(CreditCardInfoBase.Property_ExpireYear, creditCardInfoObject.ExpireYear));
			AddParameter(cmd, pNVarChar(CreditCardInfoBase.Property_StreetAddress, 500, creditCardInfoObject.StreetAddress));
			AddParameter(cmd, pNVarChar(CreditCardInfoBase.Property_City, 50, creditCardInfoObject.City));
			AddParameter(cmd, pNVarChar(CreditCardInfoBase.Property_State, 50, creditCardInfoObject.State));
			AddParameter(cmd, pNVarChar(CreditCardInfoBase.Property_ZipCode, 50, creditCardInfoObject.ZipCode));
			AddParameter(cmd, pNVarChar(CreditCardInfoBase.Property_Country, 50, creditCardInfoObject.Country));
			AddParameter(cmd, pBool(CreditCardInfoBase.Property_IsDefault, creditCardInfoObject.IsDefault));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CreditCardInfo
        /// </summary>
        /// <param name="creditCardInfoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CreditCardInfoBase creditCardInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCREDITCARDINFO);
	
				AddParameter(cmd, pInt32Out(CreditCardInfoBase.Property_Id));
				AddCommonParams(cmd, creditCardInfoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					creditCardInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					creditCardInfoObject.Id = (Int32)GetOutParameter(cmd, CreditCardInfoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(creditCardInfoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CreditCardInfo
        /// </summary>
        /// <param name="creditCardInfoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CreditCardInfoBase creditCardInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECREDITCARDINFO);
				
				AddParameter(cmd, pInt32(CreditCardInfoBase.Property_Id, creditCardInfoObject.Id));
				AddCommonParams(cmd, creditCardInfoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					creditCardInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(creditCardInfoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CreditCardInfo
        /// </summary>
        /// <param name="Id">Id of the CreditCardInfo object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECREDITCARDINFO);	
				
				AddParameter(cmd, pInt32(CreditCardInfoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CreditCardInfo), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CreditCardInfo object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CreditCardInfo object to retrieve</param>
        /// <returns>CreditCardInfo object, null if not found</returns>
		public CreditCardInfo Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCREDITCARDINFOBYID))
			{
				AddParameter( cmd, pInt32(CreditCardInfoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CreditCardInfo objects 
        /// </summary>
        /// <returns>A list of CreditCardInfo objects</returns>
		public CreditCardInfoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCREDITCARDINFO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CreditCardInfo objects by PageRequest
        /// </summary>
        /// <returns>A list of CreditCardInfo objects</returns>
		public CreditCardInfoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCREDITCARDINFO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CreditCardInfoList _CreditCardInfoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CreditCardInfoList;
			}
		}
		
		/// <summary>
        /// Retrieves all CreditCardInfo objects by query String
        /// </summary>
        /// <returns>A list of CreditCardInfo objects</returns>
		public CreditCardInfoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCREDITCARDINFOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CreditCardInfo Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CreditCardInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCREDITCARDINFOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CreditCardInfo Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CreditCardInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CreditCardInfoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCREDITCARDINFOROWCOUNT))
			{
				SqlDataReader reader;
				_CreditCardInfoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CreditCardInfoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CreditCardInfo object
        /// </summary>
        /// <param name="creditCardInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CreditCardInfoBase creditCardInfoObject, SqlDataReader reader, int start)
		{
			
				creditCardInfoObject.Id = reader.GetInt32( start + 0 );			
				creditCardInfoObject.CompanyId = reader.GetGuid( start + 1 );			
				creditCardInfoObject.CustomerId = reader.GetGuid( start + 2 );			
				creditCardInfoObject.CardType = reader.GetString( start + 3 );			
				creditCardInfoObject.CardNumber = reader.GetString( start + 4 );			
				creditCardInfoObject.CardName = reader.GetString( start + 5 );			
				creditCardInfoObject.ExpireMonth = reader.GetInt32( start + 6 );			
				creditCardInfoObject.ExpireYear = reader.GetInt32( start + 7 );			
				creditCardInfoObject.StreetAddress = reader.GetString( start + 8 );			
				creditCardInfoObject.City = reader.GetString( start + 9 );			
				creditCardInfoObject.State = reader.GetString( start + 10 );			
				creditCardInfoObject.ZipCode = reader.GetString( start + 11 );			
				creditCardInfoObject.Country = reader.GetString( start + 12 );			
				creditCardInfoObject.IsDefault = reader.GetBoolean( start + 13 );			
			FillBaseObject(creditCardInfoObject, reader, (start + 14));

			
			creditCardInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CreditCardInfo object
        /// </summary>
        /// <param name="creditCardInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CreditCardInfoBase creditCardInfoObject, SqlDataReader reader)
		{
			FillObject(creditCardInfoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CreditCardInfo object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CreditCardInfo object</returns>
		private CreditCardInfo GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CreditCardInfo creditCardInfoObject= new CreditCardInfo();
					FillObject(creditCardInfoObject, reader);
					return creditCardInfoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CreditCardInfo objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CreditCardInfo objects</returns>
		private CreditCardInfoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CreditCardInfo list
			CreditCardInfoList list = new CreditCardInfoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CreditCardInfo creditCardInfoObject = new CreditCardInfo();
					FillObject(creditCardInfoObject, reader);

					list.Add(creditCardInfoObject);
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
