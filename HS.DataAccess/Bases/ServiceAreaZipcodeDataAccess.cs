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
	public partial class ServiceAreaZipcodeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSERVICEAREAZIPCODE = "InsertServiceAreaZipcode";
		private const string UPDATESERVICEAREAZIPCODE = "UpdateServiceAreaZipcode";
		private const string DELETESERVICEAREAZIPCODE = "DeleteServiceAreaZipcode";
		private const string GETSERVICEAREAZIPCODEBYID = "GetServiceAreaZipcodeById";
		private const string GETALLSERVICEAREAZIPCODE = "GetAllServiceAreaZipcode";
		private const string GETPAGEDSERVICEAREAZIPCODE = "GetPagedServiceAreaZipcode";
		private const string GETSERVICEAREAZIPCODEMAXIMUMID = "GetServiceAreaZipcodeMaximumId";
		private const string GETSERVICEAREAZIPCODEROWCOUNT = "GetServiceAreaZipcodeRowCount";	
		private const string GETSERVICEAREAZIPCODEBYQUERY = "GetServiceAreaZipcodeByQuery";
		#endregion
		
		#region Constructors
		public ServiceAreaZipcodeDataAccess(ClientContext context) : base(context) { }
		public ServiceAreaZipcodeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="serviceAreaZipcodeObject"></param>
		private void AddCommonParams(SqlCommand cmd, ServiceAreaZipcodeBase serviceAreaZipcodeObject)
		{	
			AddParameter(cmd, pNVarChar(ServiceAreaZipcodeBase.Property_Zipcode, 50, serviceAreaZipcodeObject.Zipcode));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ServiceAreaZipcode
        /// </summary>
        /// <param name="serviceAreaZipcodeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ServiceAreaZipcodeBase serviceAreaZipcodeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSERVICEAREAZIPCODE);
	
				AddParameter(cmd, pInt32Out(ServiceAreaZipcodeBase.Property_Id));
				AddCommonParams(cmd, serviceAreaZipcodeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					serviceAreaZipcodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					serviceAreaZipcodeObject.Id = (Int32)GetOutParameter(cmd, ServiceAreaZipcodeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(serviceAreaZipcodeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ServiceAreaZipcode
        /// </summary>
        /// <param name="serviceAreaZipcodeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ServiceAreaZipcodeBase serviceAreaZipcodeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESERVICEAREAZIPCODE);
				
				AddParameter(cmd, pInt32(ServiceAreaZipcodeBase.Property_Id, serviceAreaZipcodeObject.Id));
				AddCommonParams(cmd, serviceAreaZipcodeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					serviceAreaZipcodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(serviceAreaZipcodeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ServiceAreaZipcode
        /// </summary>
        /// <param name="Id">Id of the ServiceAreaZipcode object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESERVICEAREAZIPCODE);	
				
				AddParameter(cmd, pInt32(ServiceAreaZipcodeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ServiceAreaZipcode), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ServiceAreaZipcode object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ServiceAreaZipcode object to retrieve</param>
        /// <returns>ServiceAreaZipcode object, null if not found</returns>
		public ServiceAreaZipcode Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEAREAZIPCODEBYID))
			{
				AddParameter( cmd, pInt32(ServiceAreaZipcodeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ServiceAreaZipcode objects 
        /// </summary>
        /// <returns>A list of ServiceAreaZipcode objects</returns>
		public ServiceAreaZipcodeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSERVICEAREAZIPCODE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ServiceAreaZipcode objects by PageRequest
        /// </summary>
        /// <returns>A list of ServiceAreaZipcode objects</returns>
		public ServiceAreaZipcodeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSERVICEAREAZIPCODE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ServiceAreaZipcodeList _ServiceAreaZipcodeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ServiceAreaZipcodeList;
			}
		}
		
		/// <summary>
        /// Retrieves all ServiceAreaZipcode objects by query String
        /// </summary>
        /// <returns>A list of ServiceAreaZipcode objects</returns>
		public ServiceAreaZipcodeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEAREAZIPCODEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ServiceAreaZipcode Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ServiceAreaZipcode
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEAREAZIPCODEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ServiceAreaZipcode Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ServiceAreaZipcode
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ServiceAreaZipcodeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEAREAZIPCODEROWCOUNT))
			{
				SqlDataReader reader;
				_ServiceAreaZipcodeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ServiceAreaZipcodeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ServiceAreaZipcode object
        /// </summary>
        /// <param name="serviceAreaZipcodeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ServiceAreaZipcodeBase serviceAreaZipcodeObject, SqlDataReader reader, int start)
		{
			
				serviceAreaZipcodeObject.Id = reader.GetInt32( start + 0 );			
				serviceAreaZipcodeObject.Zipcode = reader.GetString( start + 1 );			
			FillBaseObject(serviceAreaZipcodeObject, reader, (start + 2));

			
			serviceAreaZipcodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ServiceAreaZipcode object
        /// </summary>
        /// <param name="serviceAreaZipcodeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ServiceAreaZipcodeBase serviceAreaZipcodeObject, SqlDataReader reader)
		{
			FillObject(serviceAreaZipcodeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ServiceAreaZipcode object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ServiceAreaZipcode object</returns>
		private ServiceAreaZipcode GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ServiceAreaZipcode serviceAreaZipcodeObject= new ServiceAreaZipcode();
					FillObject(serviceAreaZipcodeObject, reader);
					return serviceAreaZipcodeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ServiceAreaZipcode objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ServiceAreaZipcode objects</returns>
		private ServiceAreaZipcodeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ServiceAreaZipcode list
			ServiceAreaZipcodeList list = new ServiceAreaZipcodeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ServiceAreaZipcode serviceAreaZipcodeObject = new ServiceAreaZipcode();
					FillObject(serviceAreaZipcodeObject, reader);

					list.Add(serviceAreaZipcodeObject);
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
