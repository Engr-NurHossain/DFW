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
	public partial class PaymentInfoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYMENTINFO = "InsertPaymentInfo";
		private const string UPDATEPAYMENTINFO = "UpdatePaymentInfo";
		private const string DELETEPAYMENTINFO = "DeletePaymentInfo";
		private const string GETPAYMENTINFOBYID = "GetPaymentInfoById";
		private const string GETALLPAYMENTINFO = "GetAllPaymentInfo";
		private const string GETPAGEDPAYMENTINFO = "GetPagedPaymentInfo";
		private const string GETPAYMENTINFOMAXIMUMID = "GetPaymentInfoMaximumId";
		private const string GETPAYMENTINFOROWCOUNT = "GetPaymentInfoRowCount";	
		private const string GETPAYMENTINFOBYQUERY = "GetPaymentInfoByQuery";
		#endregion
		
		#region Constructors
		public PaymentInfoDataAccess(ClientContext context) : base(context) { }
		public PaymentInfoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="paymentInfoObject"></param>
		private void AddCommonParams(SqlCommand cmd, PaymentInfoBase paymentInfoObject)
		{	
			AddParameter(cmd, pGuid(PaymentInfoBase.Property_CompanyId, paymentInfoObject.CompanyId));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_AccountName, 500, paymentInfoObject.AccountName));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_BankAccountType, 50, paymentInfoObject.BankAccountType));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_RoutingNo, 50, paymentInfoObject.RoutingNo));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_AcountNo, 500, paymentInfoObject.AcountNo));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_CardType, 50, paymentInfoObject.CardType));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_CardNumber, 500, paymentInfoObject.CardNumber));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_CardExpireDate, 50, paymentInfoObject.CardExpireDate));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_CardSecurityCode, 150, paymentInfoObject.CardSecurityCode));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_CheckNo, 500, paymentInfoObject.CheckNo));
			AddParameter(cmd, pBool(PaymentInfoBase.Property_IsCash, paymentInfoObject.IsCash));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_EcheckType, 50, paymentInfoObject.EcheckType));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_FileName, 250, paymentInfoObject.FileName));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_BankName, 150, paymentInfoObject.BankName));
			AddParameter(cmd, pBool(PaymentInfoBase.Property_IsForBrinks, paymentInfoObject.IsForBrinks));
			AddParameter(cmd, pBool(PaymentInfoBase.Property_IsPartialPayment, paymentInfoObject.IsPartialPayment));
			AddParameter(cmd, pBool(PaymentInfoBase.Property_IsInitialPayment, paymentInfoObject.IsInitialPayment));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_City, 200, paymentInfoObject.City));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_State, 200, paymentInfoObject.State));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_ZipCode, 200, paymentInfoObject.ZipCode));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_Street, 500, paymentInfoObject.Street));
			AddParameter(cmd, pGuid(PaymentInfoBase.Property_CusotmerId, paymentInfoObject.CusotmerId));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_Token, 50, paymentInfoObject.Token));
			AddParameter(cmd, pNVarChar(PaymentInfoBase.Property_RMRToken, 50, paymentInfoObject.RMRToken));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PaymentInfo
        /// </summary>
        /// <param name="paymentInfoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PaymentInfoBase paymentInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYMENTINFO);
	
				AddParameter(cmd, pInt32Out(PaymentInfoBase.Property_Id));
				AddCommonParams(cmd, paymentInfoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					paymentInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					paymentInfoObject.Id = (Int32)GetOutParameter(cmd, PaymentInfoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(paymentInfoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PaymentInfo
        /// </summary>
        /// <param name="paymentInfoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PaymentInfoBase paymentInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYMENTINFO);
				
				AddParameter(cmd, pInt32(PaymentInfoBase.Property_Id, paymentInfoObject.Id));
				AddCommonParams(cmd, paymentInfoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					paymentInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(paymentInfoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PaymentInfo
        /// </summary>
        /// <param name="Id">Id of the PaymentInfo object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYMENTINFO);	
				
				AddParameter(cmd, pInt32(PaymentInfoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PaymentInfo), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PaymentInfo object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PaymentInfo object to retrieve</param>
        /// <returns>PaymentInfo object, null if not found</returns>
		public PaymentInfo Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOBYID))
			{
				AddParameter( cmd, pInt32(PaymentInfoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PaymentInfo objects 
        /// </summary>
        /// <returns>A list of PaymentInfo objects</returns>
		public PaymentInfoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYMENTINFO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PaymentInfo objects by PageRequest
        /// </summary>
        /// <returns>A list of PaymentInfo objects</returns>
		public PaymentInfoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYMENTINFO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PaymentInfoList _PaymentInfoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PaymentInfoList;
			}
		}
		
		/// <summary>
        /// Retrieves all PaymentInfo objects by query String
        /// </summary>
        /// <returns>A list of PaymentInfo objects</returns>
		public PaymentInfoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PaymentInfo Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PaymentInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PaymentInfo Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PaymentInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PaymentInfoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOROWCOUNT))
			{
				SqlDataReader reader;
				_PaymentInfoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PaymentInfoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PaymentInfo object
        /// </summary>
        /// <param name="paymentInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PaymentInfoBase paymentInfoObject, SqlDataReader reader, int start)
		{
			
				paymentInfoObject.Id = reader.GetInt32( start + 0 );			
				paymentInfoObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) paymentInfoObject.AccountName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) paymentInfoObject.BankAccountType = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) paymentInfoObject.RoutingNo = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) paymentInfoObject.AcountNo = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) paymentInfoObject.CardType = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) paymentInfoObject.CardNumber = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) paymentInfoObject.CardExpireDate = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) paymentInfoObject.CardSecurityCode = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) paymentInfoObject.CheckNo = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) paymentInfoObject.IsCash = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) paymentInfoObject.EcheckType = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) paymentInfoObject.FileName = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) paymentInfoObject.BankName = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) paymentInfoObject.IsForBrinks = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) paymentInfoObject.IsPartialPayment = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) paymentInfoObject.IsInitialPayment = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) paymentInfoObject.City = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) paymentInfoObject.State = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) paymentInfoObject.ZipCode = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) paymentInfoObject.Street = reader.GetString( start + 21 );			
				paymentInfoObject.CusotmerId = reader.GetGuid( start + 22 );			
				if(!reader.IsDBNull(23)) paymentInfoObject.Token = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) paymentInfoObject.RMRToken = reader.GetString( start + 24 );			
			FillBaseObject(paymentInfoObject, reader, (start + 25));

			
			paymentInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PaymentInfo object
        /// </summary>
        /// <param name="paymentInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PaymentInfoBase paymentInfoObject, SqlDataReader reader)
		{
			FillObject(paymentInfoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PaymentInfo object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PaymentInfo object</returns>
		private PaymentInfo GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PaymentInfo paymentInfoObject= new PaymentInfo();
					FillObject(paymentInfoObject, reader);
					return paymentInfoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PaymentInfo objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PaymentInfo objects</returns>
		private PaymentInfoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PaymentInfo list
			PaymentInfoList list = new PaymentInfoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PaymentInfo paymentInfoObject = new PaymentInfo();
					FillObject(paymentInfoObject, reader);

					list.Add(paymentInfoObject);
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
