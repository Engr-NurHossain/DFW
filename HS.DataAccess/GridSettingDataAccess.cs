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
	public partial class GridSettingDataAccess
	{
        public GridSettingDataAccess(string ConnectionString) : base(ConnectionString)
        {

        }

        public GridSettingDataAccess() { }
        public DataTable GetCustomerLeadGridSetting(Guid companyid)
        {
            string sqlQuery = @"select * into #CustomerGrid from GridSetting CustomerGrid
                                where CompanyId = '{0}'
                                and ListKeyName = 'CustomerGrid'

                                select * into #LeadGrid from GridSetting LeadGrid
                                where CompanyId = '{0}'
                                and ListKeyName = 'LeadGrid'

                                select #cg.Id as CustomerGridId, #lg.Id as LeadGridId, #cg.CompanyId as ComId,
                                #cg.SelectedColumn as SColumn, #cg.ColumnGroup as CustomerColumnGroup, #lg.ColumnGroup as LeadColumnGroup
                                ,#cg.GroupOrder as CustomerGroupOrder, #lg.GroupOrder as LeadGroupOrder,
                                #cg.OrderBy as ByOrder, #cg.IsActive as ActivateColumn, #cg.GridActive as CustomerGridActive,
                                #lg.GridActive as LeadGridActive, #cg.FormActive as CustomerFormActive,
                                #lg.FormActive as LeadFormActive, #cg.ListKeyName as CustomerKey, #lg.ListKeyName as LeadKey,
                                #cg.IsFilter as IsCustomerFilter,
								#lg.IsFilter as IsLeadFilter,
                                #cg.IsCustomerRequired as IsCustomerRequired,
								#lg.IsLeadRequired as IsLeadRequired,
                                #cg.IsCustomerLabel as IsCustomerLabel,
								#lg.IsLeadLabel as IsLeadLabel
                                from #CustomerGrid #cg
                                left join #LeadGrid #lg on #cg.SelectedColumn = #lg.SelectedColumn
                                order by #cg.OrderBy asc

                                drop table #CustomerGrid
                                drop table #LeadGrid";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
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

        public DataTable GetCustomerLeadDetailGridSetting(Guid companyid)
        {
            string sqlQuery = @"select * into #CustomerDetailTab from GridSetting where ListKeyName = 'CustomerDetailTab'
                                select * into #LeadDetailTab from GridSetting where ListKeyName = 'LeadDetailTab'

                                select #cdt.Id as CustomerDetailId, #ldt.Id as LeadDetailId, #cdt.ListKeyName as DetailKey
                                , #cdt.SelectedColumn as CustomerDetailColumn, #ldt.SelectedColumn as LeadDetailColumn, #cdt.OrderBy as CustomerDetailOrder
                                ,#ldt.OrderBy as LeadDetailOrder, #cdt.IsActive as DetailActive
                                ,#cdt.FormActive as CustomerDetailForm, #ldt.FormActive as LeadDetailForm from #CustomerDetailTab #cdt
                                left join #LeadDetailTab #ldt on #cdt.SelectedColumn = #ldt.SelectedColumn
                                where #cdt.CompanyId = '{0}'

                                drop table #CustomerDetailTab
                                drop table #LeadDetailTab";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
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
    }	
}
