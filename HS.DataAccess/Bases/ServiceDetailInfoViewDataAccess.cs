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
	public partial class ServiceDetailInfoViewDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSERVICEDETAILINFOVIEW = "InsertServiceDetailInfoView";
		private const string UPDATESERVICEDETAILINFOVIEW = "UpdateServiceDetailInfoView";
		private const string DELETESERVICEDETAILINFOVIEW = "DeleteServiceDetailInfoView";
		private const string GETSERVICEDETAILINFOVIEWBYID = "GetServiceDetailInfoViewById";
		private const string GETALLSERVICEDETAILINFOVIEW = "GetAllServiceDetailInfoView";
		private const string GETPAGEDSERVICEDETAILINFOVIEW = "GetPagedServiceDetailInfoView";
		private const string GETSERVICEDETAILINFOVIEWMAXIMUMID = "GetServiceDetailInfoViewMaximumId";
		private const string GETSERVICEDETAILINFOVIEWROWCOUNT = "GetServiceDetailInfoViewRowCount";	
		private const string GETSERVICEDETAILINFOVIEWBYQUERY = "GetServiceDetailInfoViewByQuery";
		#endregion
		
		#region Constructors
		public ServiceDetailInfoViewDataAccess(ClientContext context) : base(context) { }
		public ServiceDetailInfoViewDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="serviceDetailInfoViewObject"></param>
		private void AddCommonParams(SqlCommand cmd, ServiceDetailInfoViewBase serviceDetailInfoViewObject)
		{	
			AddParameter(cmd, pGuid(ServiceDetailInfoViewBase.Property_ServiceId, serviceDetailInfoViewObject.ServiceId));
			AddParameter(cmd, pBool(ServiceDetailInfoViewBase.Property_ShowManufacturer, serviceDetailInfoViewObject.ShowManufacturer));
			AddParameter(cmd, pBool(ServiceDetailInfoViewBase.Property_ShowLocation, serviceDetailInfoViewObject.ShowLocation));
			AddParameter(cmd, pBool(ServiceDetailInfoViewBase.Property_ShowType, serviceDetailInfoViewObject.ShowType));
			AddParameter(cmd, pBool(ServiceDetailInfoViewBase.Property_ShowModel, serviceDetailInfoViewObject.ShowModel));
			AddParameter(cmd, pBool(ServiceDetailInfoViewBase.Property_ShowFinish, serviceDetailInfoViewObject.ShowFinish));
			AddParameter(cmd, pBool(ServiceDetailInfoViewBase.Property_ShowCapacity, serviceDetailInfoViewObject.ShowCapacity));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ServiceDetailInfoView
        /// </summary>
        /// <param name="serviceDetailInfoViewObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ServiceDetailInfoViewBase serviceDetailInfoViewObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSERVICEDETAILINFOVIEW);
	
				AddParameter(cmd, pInt32Out(ServiceDetailInfoViewBase.Property_Id));
				AddCommonParams(cmd, serviceDetailInfoViewObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					serviceDetailInfoViewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					serviceDetailInfoViewObject.Id = (Int32)GetOutParameter(cmd, ServiceDetailInfoViewBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(serviceDetailInfoViewObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ServiceDetailInfoView
        /// </summary>
        /// <param name="serviceDetailInfoViewObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ServiceDetailInfoViewBase serviceDetailInfoViewObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESERVICEDETAILINFOVIEW);
				
				AddParameter(cmd, pInt32(ServiceDetailInfoViewBase.Property_Id, serviceDetailInfoViewObject.Id));
				AddCommonParams(cmd, serviceDetailInfoViewObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					serviceDetailInfoViewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(serviceDetailInfoViewObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ServiceDetailInfoView
        /// </summary>
        /// <param name="Id">Id of the ServiceDetailInfoView object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESERVICEDETAILINFOVIEW);	
				
				AddParameter(cmd, pInt32(ServiceDetailInfoViewBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ServiceDetailInfoView), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ServiceDetailInfoView object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ServiceDetailInfoView object to retrieve</param>
        /// <returns>ServiceDetailInfoView object, null if not found</returns>
		public ServiceDetailInfoView Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEDETAILINFOVIEWBYID))
			{
				AddParameter( cmd, pInt32(ServiceDetailInfoViewBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ServiceDetailInfoView objects 
        /// </summary>
        /// <returns>A list of ServiceDetailInfoView objects</returns>
		public ServiceDetailInfoViewList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSERVICEDETAILINFOVIEW))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ServiceDetailInfoView objects by PageRequest
        /// </summary>
        /// <returns>A list of ServiceDetailInfoView objects</returns>
		public ServiceDetailInfoViewList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSERVICEDETAILINFOVIEW))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ServiceDetailInfoViewList _ServiceDetailInfoViewList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ServiceDetailInfoViewList;
			}
		}
		
		/// <summary>
        /// Retrieves all ServiceDetailInfoView objects by query String
        /// </summary>
        /// <returns>A list of ServiceDetailInfoView objects</returns>
		public ServiceDetailInfoViewList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSERVICEDETAILINFOVIEWBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ServiceDetailInfoView Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ServiceDetailInfoView
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEDETAILINFOVIEWMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ServiceDetailInfoView Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ServiceDetailInfoView
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ServiceDetailInfoViewRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSERVICEDETAILINFOVIEWROWCOUNT))
			{
				SqlDataReader reader;
				_ServiceDetailInfoViewRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ServiceDetailInfoViewRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ServiceDetailInfoView object
        /// </summary>
        /// <param name="serviceDetailInfoViewObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ServiceDetailInfoViewBase serviceDetailInfoViewObject, SqlDataReader reader, int start)
		{
			
				serviceDetailInfoViewObject.Id = reader.GetInt32( start + 0 );			
				serviceDetailInfoViewObject.ServiceId = reader.GetGuid( start + 1 );			
				serviceDetailInfoViewObject.ShowManufacturer = reader.GetBoolean( start + 2 );			
				serviceDetailInfoViewObject.ShowLocation = reader.GetBoolean( start + 3 );			
				serviceDetailInfoViewObject.ShowType = reader.GetBoolean( start + 4 );			
				serviceDetailInfoViewObject.ShowModel = reader.GetBoolean( start + 5 );			
				serviceDetailInfoViewObject.ShowFinish = reader.GetBoolean( start + 6 );			
				serviceDetailInfoViewObject.ShowCapacity = reader.GetBoolean( start + 7 );			
			FillBaseObject(serviceDetailInfoViewObject, reader, (start + 8));

			
			serviceDetailInfoViewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ServiceDetailInfoView object
        /// </summary>
        /// <param name="serviceDetailInfoViewObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ServiceDetailInfoViewBase serviceDetailInfoViewObject, SqlDataReader reader)
		{
			FillObject(serviceDetailInfoViewObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ServiceDetailInfoView object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ServiceDetailInfoView object</returns>
		private ServiceDetailInfoView GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ServiceDetailInfoView serviceDetailInfoViewObject= new ServiceDetailInfoView();
					FillObject(serviceDetailInfoViewObject, reader);
					return serviceDetailInfoViewObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ServiceDetailInfoView objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ServiceDetailInfoView objects</returns>
		private ServiceDetailInfoViewList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ServiceDetailInfoView list
			ServiceDetailInfoViewList list = new ServiceDetailInfoViewList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ServiceDetailInfoView serviceDetailInfoViewObject = new ServiceDetailInfoView();
					FillObject(serviceDetailInfoViewObject, reader);

					list.Add(serviceDetailInfoViewObject);
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
