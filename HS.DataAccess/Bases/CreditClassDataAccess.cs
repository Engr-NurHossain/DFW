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
	public partial class CreditClassDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCREDITCLASS = "InsertCreditClass";
		private const string UPDATECREDITCLASS = "UpdateCreditClass";
		private const string DELETECREDITCLASS = "DeleteCreditClass";
		private const string GETCREDITCLASSBYID = "GetCreditClassById";
		private const string GETALLCREDITCLASS = "GetAllCreditClass";
		private const string GETPAGEDCREDITCLASS = "GetPagedCreditClass";
		private const string GETCREDITCLASSMAXIMUMID = "GetCreditClassMaximumId";
		private const string GETCREDITCLASSROWCOUNT = "GetCreditClassRowCount";	
		private const string GETCREDITCLASSBYQUERY = "GetCreditClassByQuery";
		#endregion
		
		#region Constructors
		public CreditClassDataAccess(ClientContext context) : base(context) { }
		public CreditClassDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="creditClassObject"></param>
		private void AddCommonParams(SqlCommand cmd, CreditClassBase creditClassObject)
		{	
			AddParameter(cmd, pNVarChar(CreditClassBase.Property_Name, 50, creditClassObject.Name));
			AddParameter(cmd, pInt32(CreditClassBase.Property_Min, creditClassObject.Min));
			AddParameter(cmd, pInt32(CreditClassBase.Property_Max, creditClassObject.Max));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CreditClass
        /// </summary>
        /// <param name="creditClassObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CreditClassBase creditClassObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCREDITCLASS);
	
				AddParameter(cmd, pInt32Out(CreditClassBase.Property_Id));
				AddCommonParams(cmd, creditClassObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					creditClassObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					creditClassObject.Id = (Int32)GetOutParameter(cmd, CreditClassBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(creditClassObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CreditClass
        /// </summary>
        /// <param name="creditClassObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CreditClassBase creditClassObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECREDITCLASS);
				
				AddParameter(cmd, pInt32(CreditClassBase.Property_Id, creditClassObject.Id));
				AddCommonParams(cmd, creditClassObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					creditClassObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(creditClassObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CreditClass
        /// </summary>
        /// <param name="Id">Id of the CreditClass object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECREDITCLASS);	
				
				AddParameter(cmd, pInt32(CreditClassBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CreditClass), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CreditClass object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CreditClass object to retrieve</param>
        /// <returns>CreditClass object, null if not found</returns>
		public CreditClass Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCREDITCLASSBYID))
			{
				AddParameter( cmd, pInt32(CreditClassBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CreditClass objects 
        /// </summary>
        /// <returns>A list of CreditClass objects</returns>
		public CreditClassList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCREDITCLASS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CreditClass objects by PageRequest
        /// </summary>
        /// <returns>A list of CreditClass objects</returns>
		public CreditClassList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCREDITCLASS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CreditClassList _CreditClassList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CreditClassList;
			}
		}
		
		/// <summary>
        /// Retrieves all CreditClass objects by query String
        /// </summary>
        /// <returns>A list of CreditClass objects</returns>
		public CreditClassList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCREDITCLASSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CreditClass Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CreditClass
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCREDITCLASSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CreditClass Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CreditClass
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CreditClassRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCREDITCLASSROWCOUNT))
			{
				SqlDataReader reader;
				_CreditClassRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CreditClassRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CreditClass object
        /// </summary>
        /// <param name="creditClassObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CreditClassBase creditClassObject, SqlDataReader reader, int start)
		{
			
				creditClassObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) creditClassObject.Name = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) creditClassObject.Min = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) creditClassObject.Max = reader.GetInt32( start + 3 );			
			FillBaseObject(creditClassObject, reader, (start + 4));

			
			creditClassObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CreditClass object
        /// </summary>
        /// <param name="creditClassObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CreditClassBase creditClassObject, SqlDataReader reader)
		{
			FillObject(creditClassObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CreditClass object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CreditClass object</returns>
		private CreditClass GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CreditClass creditClassObject= new CreditClass();
					FillObject(creditClassObject, reader);
					return creditClassObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CreditClass objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CreditClass objects</returns>
		private CreditClassList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CreditClass list
			CreditClassList list = new CreditClassList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CreditClass creditClassObject = new CreditClass();
					FillObject(creditClassObject, reader);

					list.Add(creditClassObject);
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
