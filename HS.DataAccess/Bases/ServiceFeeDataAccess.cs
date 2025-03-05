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
	public partial class ServiceFeeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSERVICEFEE = "InsertServiceFee";
		private const string UPDATESERVICEFEE = "UpdateServiceFee";
		private const string DELETESERVICEFEE = "DeleteServiceFee";
		private const string GETSERVICEFEEBYID = "GetServiceFeeById";
		private const string GETALLSERVICEFEE = "GetAllServiceFee";
		private const string GETPAGEDSERVICEFEE = "GetPagedServiceFee";
		private const string GETSERVICEFEEMAXIMUMID = "GetServiceFeeMaximumId";
		private const string GETSERVICEFEEROWCOUNT = "GetServiceFeeRowCount";	
		private const string GETSERVICEFEEBYQUERY = "GetServiceFeeByQuery";
		#endregion
		
		#region Constructors
		public ServiceFeeDataAccess(ClientContext context) : base(context) { }
		public ServiceFeeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="serviceFeeObject"></param>
		private void AddCommonParams(SqlCommand cmd, ServiceFeeBase serviceFeeObject)
		{	
			AddParameter(cmd, pGuid(ServiceFeeBase.Property_CompanyId, serviceFeeObject.CompanyId));
			AddParameter(cmd, pNVarChar(ServiceFeeBase.Property_Name, 50, serviceFeeObject.Name));
			AddParameter(cmd, pDouble(ServiceFeeBase.Property_Fee, serviceFeeObject.Fee));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ServiceFee
        /// </summary>
        /// <param name="serviceFeeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ServiceFeeBase serviceFeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSERVICEFEE);
	
				AddParameter(cmd, pInt32Out(ServiceFeeBase.Property_Id));
				AddCommonParams(cmd, serviceFeeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					serviceFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					serviceFeeObject.Id = (Int32)GetOutParameter(cmd, ServiceFeeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(serviceFeeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ServiceFee
        /// </summary>
        /// <param name="serviceFeeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ServiceFeeBase serviceFeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESERVICEFEE);
				
				AddParameter(cmd, pInt32(ServiceFeeBase.Property_Id, serviceFeeObject.Id));
				AddCommonParams(cmd, serviceFeeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					serviceFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(serviceFeeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ServiceFee
        /// </summary>
        /// <param name="Id">Id of the ServiceFee object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESERVICEFEE);	
				
				AddParameter(cmd, pInt32(ServiceFeeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ServiceFee), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ServiceFee object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ServiceFee object to retrieve</param>
        /// <returns>ServiceFee object, null if not found</returns>
		public ServiceFee Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEFEEBYID))
			{
				AddParameter( cmd, pInt32(ServiceFeeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ServiceFee objects 
        /// </summary>
        /// <returns>A list of ServiceFee objects</returns>
		public ServiceFeeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSERVICEFEE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ServiceFee objects by PageRequest
        /// </summary>
        /// <returns>A list of ServiceFee objects</returns>
		public ServiceFeeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSERVICEFEE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ServiceFeeList _ServiceFeeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ServiceFeeList;
			}
		}
		
		/// <summary>
        /// Retrieves all ServiceFee objects by query String
        /// </summary>
        /// <returns>A list of ServiceFee objects</returns>
		public ServiceFeeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEFEEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ServiceFee Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ServiceFee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEFEEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ServiceFee Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ServiceFee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ServiceFeeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEFEEROWCOUNT))
			{
				SqlDataReader reader;
				_ServiceFeeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ServiceFeeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ServiceFee object
        /// </summary>
        /// <param name="serviceFeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ServiceFeeBase serviceFeeObject, SqlDataReader reader, int start)
		{
			
				serviceFeeObject.Id = reader.GetInt32( start + 0 );			
				serviceFeeObject.CompanyId = reader.GetGuid( start + 1 );			
				serviceFeeObject.Name = reader.GetString( start + 2 );			
				serviceFeeObject.Fee = reader.GetDouble( start + 3 );			
			FillBaseObject(serviceFeeObject, reader, (start + 4));

			
			serviceFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ServiceFee object
        /// </summary>
        /// <param name="serviceFeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ServiceFeeBase serviceFeeObject, SqlDataReader reader)
		{
			FillObject(serviceFeeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ServiceFee object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ServiceFee object</returns>
		private ServiceFee GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ServiceFee serviceFeeObject= new ServiceFee();
					FillObject(serviceFeeObject, reader);
					return serviceFeeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ServiceFee objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ServiceFee objects</returns>
		private ServiceFeeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ServiceFee list
			ServiceFeeList list = new ServiceFeeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ServiceFee serviceFeeObject = new ServiceFee();
					FillObject(serviceFeeObject, reader);

					list.Add(serviceFeeObject);
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
