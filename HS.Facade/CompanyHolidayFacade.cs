using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Facade
{
    public class CompanyHolidayFacade : BaseFacade
    {
        public CompanyHolidayFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CompanyHolidayDataAccess _CompanyHolidayDataAccess
        {
            get
            {
                return (CompanyHolidayDataAccess)_ClientContext[typeof(CompanyHolidayDataAccess)];
            }
        }

        public int InsertCompanyHoliday(CompanyHoliday model)
        {
            return (int)_CompanyHolidayDataAccess.Insert(model);
        }
        public bool UpdateCompanyHoliday(CompanyHoliday model)
        {
            return _CompanyHolidayDataAccess.Update(model) > 0;
        }
        public List<CompanyHoliday> GetAllCompanyHoliday()
        {
            return _CompanyHolidayDataAccess.GetAll();
        }
        public List<CompanyHoliday> GetAllCompanyHoliday(string year)
        {
            return _CompanyHolidayDataAccess.GetByQuery(string.Format("Year = '{0}' and IsActive = 1", year));
        }
        public List<CompanyHoliday> GetAllCompanyHoliday(Guid CompanyId, string year)
        {
            return _CompanyHolidayDataAccess.GetByQuery(string.Format(" CompanyId = '{0}' and Year = '{1}' and IsActive = 1", CompanyId, year));
        }
        public CompanyHoliday GetCompanyHoliday(int id)
        {
            return _CompanyHolidayDataAccess.Get(id);
        }
        public CompanyHoliday GetCompanyHoliday(DateTime date)
        {
            return _CompanyHolidayDataAccess.GetByQuery(string.Format("Holiday = '{0}' and IsActive = 1", date)).FirstOrDefault();
        }
        public List<CompanyHoliday> GetCompanyHolidayList(DateTime startdate, DateTime enddate)
        {
            return _CompanyHolidayDataAccess.GetByQuery(string.Format("Holiday between '{0}' and  '{1}'  and IsActive = 1", startdate.ToString("MM-dd-yyyy"), enddate.ToString("MM-dd-yyyy")));
        }
        public bool DeactiveCompanyHoliday(int id)
        {
            CompanyHoliday holiday = _CompanyHolidayDataAccess.Get(id);
            holiday.IsActive = false;
            return _CompanyHolidayDataAccess.Update(holiday) > 0;
        }
        public bool DeleteCompanyHoliday(int id)
        {
            return _CompanyHolidayDataAccess.Delete(id) > 0;
        }

    }
}
