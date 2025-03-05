using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class WebsiteLocationDataAccess
	{
        public WebsiteLocationDataAccess(string ConnectionString) : base(ConnectionString)
        {

        }

        public WebsiteLocationDataAccess() { }
        public DataTable GetAllWebsiteLocationOperationByLocationIdAndCompanyId(Guid comid, int locid, string type)
        {
            string subquery = "";
            if(!string.IsNullOrWhiteSpace(type) && type == "store")
            {
                subquery = string.Format("and IsAdditional = 1");
            }
            string sqlQuery = @"select * into #lookup from [Lookup] where CompanyId = '{0}' and DataKey = 'Arrival'
                                
                                select distinct wlo.Id, wlo.CompanyId, wlo.SiteLocationId, wlo.HoursofOperation
                                ,wlo.OperationStartTime, wlo.OperationEndTime, slk.DisplayText as OperationStartTimeVal
                                ,elk.DisplayText as OperationEndTimeVal
                                ,wlo.StoreOperationStartTime, wlo.StoreOperationEndTime, wlo.IsAdditional
                                ,sslk.DisplayText as StoreOperationStartTimeVal, selk.DisplayText as StoreOperationEndTimeVal
                                from WebsiteLocationOperation wlo
                                left join #lookup slk on slk.DataValue = wlo.OperationStartTime and slk.DataKey = 'Arrival'
                                left join #lookup elk on elk.DataValue = wlo.OperationEndTime and elk.DataKey = 'Arrival'
                                left join #lookup sslk on sslk.DataValue = wlo.StoreOperationStartTime and sslk.DataKey = 'Arrival'
                                left join #lookup selk on selk.DataValue = wlo.StoreOperationEndTime and selk.DataKey = 'Arrival'
                                where wlo.SiteLocationId = {1}
                                order by wlo.OperationStartTime asc
                                drop table #lookup";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, locid, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllWebLocWithNoItems()
        {
            string sqlQuery = @"select wl.[Name], wl.CompanyId from WebsiteLocation wl
                                left join RestMenuItem _item on _item.CompanyId = wl.CompanyId
                                where wl.IsDefault = 1 and wl.OperationStartTime is not null and wl.OperationStartTime != '' and wl.OperationEndTime is not null and wl.OperationEndTime != ''
                                group by wl.[Name], wl.CompanyId
                                having COUNT(_item.Id) = 0";
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllNeighbarhoodByKey(string key)
        {
            string sqlQuery = @"select distinct NeighborhoodName, NeighborhoodURL
                                from ResturantNeighborhood";
            try
            {
                sqlQuery = string.Format(sqlQuery, key);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllTrackingNumbersByComapnyId(Guid comid)
        {
            string sqlQuery = @"select distinct tns.*, wl.[Name] as CompanyName from WebsiteLocation wl
                                left join TrackingNumberSetting tns on tns.CompanyId = wl.CompanyId
                                where (wl.CompanyId = '{0}' or wl.ReferCompanyId = '{0}')
                                and tns.CompanyId is not null";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteAllWebsiteLocationOperationByHoursOpt(string day, int siteid)
        {
            string sqlQuery = @"Delete from WebsiteLocationOperation where HoursofOperation = '{0}' and SiteLocationId = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, day, siteid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }	
}
