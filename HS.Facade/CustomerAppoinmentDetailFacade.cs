using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class CustomerAppoinmentDetailFacade : BaseFacade
    {
        public CustomerAppoinmentDetailFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerAppointmentDetailDataAccess _CustomerAppointmentDetailDataAccess
        {
            get
            {
                return (CustomerAppointmentDetailDataAccess)_ClientContext[typeof(CustomerAppointmentDetailDataAccess)];
            }
        }
        CustomerAppointmentDataAccess _CustomerAppointmentDataAccess
        {
            get
            {
                return (CustomerAppointmentDataAccess)_ClientContext[typeof(CustomerAppointmentDataAccess)];
            }
        }
        CustomerAppointmentEquipmentDataAccess _CustomerAppointmentEquipmentDataAccess
        {
            get
            {
                return (CustomerAppointmentEquipmentDataAccess)_ClientContext[typeof(CustomerAppointmentEquipmentDataAccess)];
            }
        }

        public List<CustomerAppointment> GetAllCustomerAppoinmentDetailByAppoinmentId(Guid appoinmentId)
        {
            return _CustomerAppointmentDataAccess.GetByQuery(string.Format( "AppointmentId = '{0}'", appoinmentId));
        }
        public CustomerAppointmentDetail GetAppointmentDetailById(int value)
        {
            return _CustomerAppointmentDetailDataAccess.Get(value);
        }
        public CustomerAppointmentDetail GetById(int id)
        {
            return _CustomerAppointmentDetailDataAccess.Get(id);
        }
        public bool UpdateCustomerAppoinmentDetail(CustomerAppointmentDetail ad)
        {
            return _CustomerAppointmentDetailDataAccess.Update(ad) > 0;
        }
        public long InsertCustomerAppoinmentDetail(CustomerAppointmentDetail ad)
        {
            return _CustomerAppointmentDetailDataAccess.Insert(ad);
        }
        public List<CustomerAppointmentDetail> IsAppointmentIdDetail(Guid AppointmentId)
        {
            return _CustomerAppointmentDetailDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId));
        }
        public CustomerAppointmentDetail GetCustomerAppointmentDetailByAppointmentId(Guid AppointmentId)
        {
            return _CustomerAppointmentDetailDataAccess.GetByQuery(string.Format(" AppointmentId = '{0}'", AppointmentId)).FirstOrDefault();
        }
        public CustomerAppointmentDetail GetAppoinmentidByAppoinmentId(Guid Appointmentid)
        {
            return _CustomerAppointmentDetailDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", Appointmentid)).FirstOrDefault();
        }
        public List<CustomerAppointmentDetail> IsAppointmentDetailExistCheck(Guid AppointmentId)
        {
            return _CustomerAppointmentDetailDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId));
        }
        public bool DeletePreviousCustomerDetailsByEquipmentId(List<CustomerAppointmentDetail> PreviousList)
        {
            bool result = false;

            foreach (var items in PreviousList)
            {
                result = _CustomerAppointmentDetailDataAccess.Delete(items.Id) > 0;
            }

            return result;
        }
        public bool InsertCustomerAppointmentDetail(CustomerAppointmentDetail CustomerAppointmentDetailObject)
        {
            return _CustomerAppointmentDetailDataAccess.Insert(CustomerAppointmentDetailObject) > 0;
        }
        //public CustomerAppointmentDetail GetInstallTypeNameByAppointmentID(Guid AppointmentID)
        //{
        //    return _CustomerAppointmentDetailDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentID)).FirstOrDefault();
        //}
        public CustomerAppointmentDetail GetAppointmentDetailInstallTypeById(int value)
        {
            return _CustomerAppointmentDetailDataAccess.Get(value);
        }
        public CustomerAppointmentDetail GetInstallTypeNameByAppointmentID(Guid AppointmentId)
        {
            return _CustomerAppointmentDetailDataAccess.GetByQuery(string.Format("AppointmentId= '{0}'", AppointmentId)).FirstOrDefault();
        }

        public long InsertCustomerAppointmentEquipment(CustomerAppointmentEquipment ad)
        {
            return _CustomerAppointmentEquipmentDataAccess.Insert(ad);
        }

    }
}
