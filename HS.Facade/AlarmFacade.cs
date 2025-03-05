using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class AlarmFacade : BaseFacade
    {
        public AlarmFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        SetupAlarmDataAccess _SetupAlarmDataAccess
        {
            get
            {
                return (SetupAlarmDataAccess)_ClientContext[typeof(SetupAlarmDataAccess)];
            }
        }

        AlarmAddOnnsDataAccess _AlarmAddOnnsDataAccess
        {
            get
            {
                return (AlarmAddOnnsDataAccess)_ClientContext[typeof(AlarmAddOnnsDataAccess)];
            }
        }
        public int InsertSetupAlarm(SetupAlarm SetupAlarm)
        {
            return (int)_SetupAlarmDataAccess.Insert(SetupAlarm);
        }

        public int InsertAlarmAddOnns(AlarmAddOnns addonns)
        {
            return (int)_AlarmAddOnnsDataAccess.Insert(addonns);
        }

        public List<AlarmAddOnns> GetAllAddOnns()
        {
            return _AlarmAddOnnsDataAccess.GetAll();
        }
        public AlarmAddOnns GeAddOnnsByName(string AddonName)
        {
            return _AlarmAddOnnsDataAccess.GetByQuery(string.Format("Name='{0}'", AddonName)).FirstOrDefault();
        }
        public bool UpdateSetupAlarm(SetupAlarm SetupAlarm)
        {
            return _SetupAlarmDataAccess.Update(SetupAlarm)>0;
        }

        public SetupAlarm GetSetupalarmByCustomerId(Guid customerId)
        {
            return _SetupAlarmDataAccess.GetByQuery(string.Format("CustomerId='{0}'",customerId)).FirstOrDefault();
        }

        public long DeleteSetupAlarm(int id)
        {
            return _SetupAlarmDataAccess.Delete(id);
        }
        //public List<BillDetail> GetBillDetialsListByBillId(int BillId)
        //{
        //    DataTable dt = _BillDetailDataAccess.GetBillDetialsListByBillId(BillId);
        //    List<BillDetail> NoteList = new List<BillDetail>();
        //    NoteList = (from DataRow dr in dt.Rows
        //                select new BillDetail()
        //                {
        //                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                    EquipmentId = (Guid)dr["EquipmentId"],
        //                }).ToList();
        //    return NoteList;
        //}
    }
}
