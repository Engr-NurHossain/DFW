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
	public partial class ActivationFeeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTACTIVATIONFEE = "InsertActivationFee";
		private const string UPDATEACTIVATIONFEE = "UpdateActivationFee";
		private const string DELETEACTIVATIONFEE = "DeleteActivationFee";
		private const string GETACTIVATIONFEEBYID = "GetActivationFeeById";
		private const string GETALLACTIVATIONFEE = "GetAllActivationFee";
		private const string GETPAGEDACTIVATIONFEE = "GetPagedActivationFee";
		private const string GETACTIVATIONFEEMAXIMUMID = "GetActivationFeeMaximumId";
		private const string GETACTIVATIONFEEROWCOUNT = "GetActivationFeeRowCount";	
		private const string GETACTIVATIONFEEBYQUERY = "GetActivationFeeByQuery";
		#endregion
		
		#region Constructors
		public ActivationFeeDataAccess(ClientContext context) : base(context) { }
		public ActivationFeeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="activationFeeObject"></param>
		private void AddCommonParams(SqlCommand cmd, ActivationFeeBase activationFeeObject)
		{	
			AddParameter(cmd, pGuid(ActivationFeeBase.Property_CompanyId, activationFeeObject.CompanyId));
			AddParameter(cmd, pNVarChar(ActivationFeeBase.Property_Name, 50, activationFeeObject.Name));
			AddParameter(cmd, pDouble(ActivationFeeBase.Property_Fee, activationFeeObject.Fee));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ActivationFee
        /// </summary>
        /// <param name="activationFeeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ActivationFeeBase activationFeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTACTIVATIONFEE);
	
				AddParameter(cmd, pInt32Out(ActivationFeeBase.Property_Id));
				AddCommonParams(cmd, activationFeeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					activationFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					activationFeeObject.Id = (Int32)GetOutParameter(cmd, ActivationFeeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(activationFeeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ActivationFee
        /// </summary>
        /// <param name="activationFeeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ActivationFeeBase activationFeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEACTIVATIONFEE);
				
				AddParameter(cmd, pInt32(ActivationFeeBase.Property_Id, activationFeeObject.Id));
				AddCommonParams(cmd, activationFeeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					activationFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(activationFeeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ActivationFee
        /// </summary>
        /// <param name="Id">Id of the ActivationFee object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEACTIVATIONFEE);	
				
				AddParameter(cmd, pInt32(ActivationFeeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ActivationFee), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ActivationFee object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ActivationFee object to retrieve</param>
        /// <returns>ActivationFee object, null if not found</returns>
		public ActivationFee Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETACTIVATIONFEEBYID))
			{
				AddParameter( cmd, pInt32(ActivationFeeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ActivationFee objects 
        /// </summary>
        /// <returns>A list of ActivationFee objects</returns>
		public ActivationFeeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLACTIVATIONFEE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ActivationFee objects by PageRequest
        /// </summary>
        /// <returns>A list of ActivationFee objects</returns>
		public ActivationFeeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDACTIVATIONFEE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ActivationFeeList _ActivationFeeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ActivationFeeList;
			}
		}
		
		/// <summary>
        /// Retrieves all ActivationFee objects by query String
        /// </summary>
        /// <returns>A list of ActivationFee objects</returns>
		public ActivationFeeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETACTIVATIONFEEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ActivationFee Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ActivationFee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETACTIVATIONFEEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ActivationFee Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ActivationFee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ActivationFeeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETACTIVATIONFEEROWCOUNT))
			{
				SqlDataReader reader;
				_ActivationFeeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ActivationFeeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ActivationFee object
        /// </summary>
        /// <param name="activationFeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ActivationFeeBase activationFeeObject, SqlDataReader reader, int start)
		{
			
				activationFeeObject.Id = reader.GetInt32( start + 0 );			
				activationFeeObject.CompanyId = reader.GetGuid( start + 1 );			
				activationFeeObject.Name = reader.GetString( start + 2 );			
				activationFeeObject.Fee = reader.GetDouble( start + 3 );			
			FillBaseObject(activationFeeObject, reader, (start + 4));

			
			activationFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ActivationFee object
        /// </summary>
        /// <param name="activationFeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ActivationFeeBase activationFeeObject, SqlDataReader reader)
		{
			FillObject(activationFeeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ActivationFee object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ActivationFee object</returns>
		private ActivationFee GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ActivationFee activationFeeObject= new ActivationFee();
					FillObject(activationFeeObject, reader);
					return activationFeeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ActivationFee objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ActivationFee objects</returns>
		private ActivationFeeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ActivationFee list
			ActivationFeeList list = new ActivationFeeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ActivationFee activationFeeObject = new ActivationFee();
					FillObject(activationFeeObject, reader);

					list.Add(activationFeeObject);
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
