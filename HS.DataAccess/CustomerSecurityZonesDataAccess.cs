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
    public partial class CustomerSecurityZonesDataAccess
    {
        public CustomerSecurityZonesDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllCustomerSecurityZoneByCustomerId(Guid CustomerId, string PlatformId)
        {
            string EventCodeDataKey = "BrinksEventCode";
            if (PlatformId == "'UCC'")
            {
                EventCodeDataKey = "UCCEventCode";
            }
            else if(PlatformId == "'NMC'")
            {
                EventCodeDataKey = "NMCEventCode";
            }
            string sqlQuery = @"select cz.*,lkevent.DisplayText as EventCodeVal,lkNmcEqpType.DisplayText as NmcEqpType,
                                lkNmcEqpLoc.DisplayText as NmcEqpLoc,zoneloc.EquipmentLocation as LocationVal,
                                eqpType.EquipmentType as EquipmentTypeVal from CustomerSecurityZones cz
                                left join Lookup lkevent on lkevent.DataValue = cz.EventCode and lkevent.DataKey = '{2}'
                                left join Lookup lkNmcEqpType on lkNmcEqpType.DataValue = cz.EquipmentType and lkNmcEqpType.DataKey = 'NMCEqpType'
                                left join Lookup lkNmcEqpLoc on lkNmcEqpLoc.DataValue = cz.Location and lkNmcEqpLoc.DataKey = 'NMCEqpLocation'
                                left join ZonesEquipmentLocation zoneloc on zoneloc.EquipmentLocationId = cz.Location 
                                left join ZonesEquipmentType eqpType on eqpType.EqpmentTypeId = cz.EquipmentType 
                                where cz.CustomerId='{0}' and cz.Platform ={1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, PlatformId, EventCodeDataKey);
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
