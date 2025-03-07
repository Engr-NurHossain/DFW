using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.ComponentModel.Design;
using System.Reflection;

namespace HS.Facade
{
    public class EmployeeFacade : BaseFacade
    {
        EmployeeDataAccess _EmployeeDataAccess = null;
        EmployeeOperationsDataAccess _EmployeeOperationsDataAccess = null; 
        EmployeePtoAccrualRateDataAccess _EmployeePtoAccrualRateDataAccess = null; 
        EmployeePTOHourLogDataAccess _EmployeePTOHourLogDataAccess = null; 
        CompanyHolidayDataAccess _CompanyHolidayDataAccess = null; 
        public EmployeeFacade(ClientContext clientContext)
            : base(clientContext)
        {
            _EmployeeDataAccess = (EmployeeDataAccess)_ClientContext[typeof(EmployeeDataAccess)];
            _EmployeeOperationsDataAccess = (EmployeeOperationsDataAccess)_ClientContext[typeof(EmployeeOperationsDataAccess)];
            _EmployeePtoAccrualRateDataAccess = (EmployeePtoAccrualRateDataAccess)_ClientContext[typeof(EmployeePtoAccrualRateDataAccess)];
            _EmployeePTOHourLogDataAccess = (EmployeePTOHourLogDataAccess)_ClientContext[typeof(EmployeePTOHourLogDataAccess)];
            _CompanyHolidayDataAccess = (CompanyHolidayDataAccess)_ClientContext[typeof(CompanyHolidayDataAccess)];
        }
        public EmployeeFacade()
        {
            _EmployeeDataAccess = new EmployeeDataAccess();
            _EmployeeOperationsDataAccess = new EmployeeOperationsDataAccess();
            _EmployeePtoAccrualRateDataAccess = new EmployeePtoAccrualRateDataAccess();
            _EmployeePTOHourLogDataAccess = new EmployeePTOHourLogDataAccess();
            _CompanyHolidayDataAccess = new CompanyHolidayDataAccess();
        }
        public EmployeeFacade(string constr)
        {
            _EmployeeDataAccess = new EmployeeDataAccess(constr);
            _EmployeeOperationsDataAccess = new EmployeeOperationsDataAccess(constr);
            _EmployeePtoAccrualRateDataAccess = new EmployeePtoAccrualRateDataAccess(constr);
            _EmployeePTOHourLogDataAccess = new EmployeePTOHourLogDataAccess(constr);
            _CompanyHolidayDataAccess = new CompanyHolidayDataAccess(constr);
        }
        UserBranchDataAccess _UserBranchDataAccess
        {
            get
            {
                return (UserBranchDataAccess)_ClientContext[typeof(UserBranchDataAccess)];
            }
        }
        EmployeeLeadSourceDataAccess _EmployeeLeadSourceDataAccess
        {
            get
            {
                return (EmployeeLeadSourceDataAccess)_ClientContext[typeof(EmployeeLeadSourceDataAccess)];
            }
        }
        TimeClockDataAccess _TimeClockDataAccess
        {
            get
            {
                return (TimeClockDataAccess)_ClientContext[typeof(TimeClockDataAccess)];
            }
        }
        NoteAssignDataAccess _NoteAssignDataAccess
        {
            get
            {
                return (NoteAssignDataAccess)_ClientContext[typeof(NoteAssignDataAccess)];
            }
        }
        EmployeeNoteDataAccess _EmployeeNoteDataAccess
        {
            get
            {
                return (EmployeeNoteDataAccess)_ClientContext[typeof(EmployeeNoteDataAccess)];
            }
        }
        EmployeeCommissionDataAccess _EmployeeCommissionDataAccess
        {
            get
            {
                return (EmployeeCommissionDataAccess)_ClientContext[typeof(EmployeeCommissionDataAccess)];
            }
        }

        CustomerPackageServiceDataAccess _CustomerPackageServiceDataAccess
        {
            get
            {
                return (CustomerPackageServiceDataAccess)_ClientContext[typeof(CustomerPackageServiceDataAccess)];
            }
        }

        CustomerPackageEqpDataAccess _CustomerPackageEqpDataAccess
        {
            get
            {
                return (CustomerPackageEqpDataAccess)_ClientContext[typeof(CustomerPackageEqpDataAccess)];
            }
        }

        EmployeeTimeClockSupervisorDataAccess _EmployeeTimeClockSupervisorDataAccess
        {
            get
            {
                return (EmployeeTimeClockSupervisorDataAccess)_ClientContext[typeof(EmployeeTimeClockSupervisorDataAccess)];
            }
        }
        UserLoginDataAccess _UserLoginDataAccess
        {
            get
            {
                return (UserLoginDataAccess)_ClientContext[typeof(UserLoginDataAccess)];
            }
        }
        AgemniEmployeeMapperDataAccess _AgemniEmployeeMapperDataAccess
        {
            get
            {
                return (AgemniEmployeeMapperDataAccess)_ClientContext[typeof(AgemniEmployeeMapperDataAccess)];
            }
        }

        BrinksCustomerDataAccess _BrinksCustomerDataAccess
        {
            get
            {
                return (BrinksCustomerDataAccess)_ClientContext[typeof(BrinksCustomerDataAccess)];
            }
        }
        PayrollDetailDataAccess _PayrollDetailDataAccess
        {
            get
            {
                return (PayrollDetailDataAccess)_ClientContext[typeof(PayrollDetailDataAccess)];
            }
        }
        AAEmployeeDumpDataAccess _AAEmployeeDumpDataAccess
        {
            get
            {
                return (AAEmployeeDumpDataAccess)_ClientContext[typeof(AAEmployeeDumpDataAccess)];
            }
        }

        ErrorLogDataAccess _ErrorLogDataAccess
        {
            get
            {
                return (ErrorLogDataAccess)_ClientContext[typeof(ErrorLogDataAccess)];
            }
        }

        public long InsertEmployee(Employee emp)
        {
            return _EmployeeDataAccess.Insert(emp);
        }

        public long InsertPayrollDetail(PayrollDetail PayrollDetail)
        {
            return _PayrollDetailDataAccess.Insert(PayrollDetail);
        }
        public bool UpdatePaidCommissionData(PayrollDetail PD)
        {
            return _PayrollDetailDataAccess.Update(PD) > 0;
        }
        public PayrollDetail GetPayrollDetailById(int id)
        {
            return _PayrollDetailDataAccess.Get(id);
        }
        public long InsertAgemniEmployeeMapper(AgemniEmployeeMapper emp)
        {
            return _AgemniEmployeeMapperDataAccess.Insert(emp);
        }
        public List<AgemniEmployeeMapper> GetAllAgemniEmpMapper()
        {
            return _AgemniEmployeeMapperDataAccess.GetAll();
        }
        public Employee GetEmployeeByUserId(Guid UserId)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserId = '{0}'", UserId)).FirstOrDefault();
        }
        public List<EmployeeOperations> GetAllEmployeeOperationById(Guid id, DateTime date)
        {
            return _EmployeeOperationsDataAccess.GetByQuery(string.Format(" EmployeeId = '{0}' and SelectedDate between '{1}' and '{2}'", id, new DateTime(date.Year, date.Month, 1).SetZeroHour(), date.SetMaxHour())).ToList();
        }
        public List<CompanyHoliday> GetCompanyHolidayList(DateTime startdate, DateTime enddate)
        {
            return _CompanyHolidayDataAccess.GetByQuery(string.Format("Holiday between '{0}' and  '{1}'  and IsActive = 1", startdate.ToString("MM-dd-yyyy"), enddate.ToString("MM-dd-yyyy")));
        }
        public Employee GetEmployeeByUserName(string UserName)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserName = '{0}'", UserName)).FirstOrDefault();
        }
        public Employee GetFirstNameByEmployeeId(string fname)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("FirstName = '{0}'", fname)).FirstOrDefault();
        }
        public bool UpdateEmployee(Employee tempEmp)
        {
            return _EmployeeDataAccess.Update(tempEmp) > 0;
        }

        public List<Employee> GetAllEmployeeByCompanyId(Guid comid, string userid)
        {
            //DataTable dt = _EmployeeDataAccess.GetAllEmployeeByCompanyId(comid);
            //List<Employee> EmployeeList = new List<Employee>();
            //EmployeeList = (from DataRow dr in dt.Rows
            //                select new Employee()
            //                {
            //                    UserId = (Guid)dr["UserId"],
            //                    EMPName = dr["EMPName"].ToString()
            //                }).ToList();
            //return EmployeeList;
            return _EmployeeDataAccess.GetAllEmployeeByCompanyId(comid, userid);
        }
        public List<AAEmployeeDump> AAEmployeeDumpList()
        {
            return _AAEmployeeDumpDataAccess.GetAll();
        }
        public List<Employee> GetAllCalendarEmployeeByCompanyId(Guid comid, string userid)
        {
            return _EmployeeDataAccess.GetAllCalendarEmployeeByCompanyId(comid, userid);
        }

        public List<Employee> GetAllEmployeeByCompanyIdAndKey(Guid CompanId, string Key)
        {
            return _EmployeeDataAccess.GetAllEmployeeByCompanyIdAndKey(CompanId, Key);
        }
        public List<EmployeeOperations> GetAllEmployeeOperationByDay(string day)
        {
            return _EmployeeOperationsDataAccess.GetByQuery(string.Format(" SelectedDate = '{0}'", day));
        }
        public List<Employee> GetAllQAEmployee(Guid CompanyId)
        {
            DataTable dt = _EmployeeDataAccess.GetAllQAEmployee(CompanyId);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {

                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                UserName = dr["UserName"].ToString()
                            }).ToList();
            return EmployeeList;
        }

        public List<Employee> GetAllEmployeeByEmpIdList(string EmpIdList)
        {
            DataTable dt = _EmployeeDataAccess.GetAllEmployeeByEmpIdList(EmpIdList);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {

                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                UserName = dr["UserName"].ToString(),
                                UserId = (Guid)dr["UserId"],
                                HourlyRate = dr["HourlyRate"] != DBNull.Value ? Convert.ToDouble(dr["HourlyRate"]) : 0,
                                PtoHour = dr["PtoHour"] != DBNull.Value ? Convert.ToDouble(dr["PtoHour"]) : 0,
                                PtoRate = dr["PtoRate"] != DBNull.Value ? Convert.ToDouble(dr["PtoRate"]) : 0,
                            }).ToList();
            return EmployeeList;
        }
        public Employee GetEmployeeByUserLoginId(int userId)
        {
            return _EmployeeDataAccess.GetEmployeeByUserLoginId(userId);
        }

        public Employee GetSalesPersonByEmployeeId(Guid EmployeeId)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserId = '{0}'", EmployeeId)).FirstOrDefault();
        }


        public List<Employee> GetEmployeeByCompanyIdAndTag(Guid CompanyId, string Tag, Guid userid, string tag2 = "")
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByCompanyIdAndTag(CompanyId, Tag, userid, tag2);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                City = dr["City"].ToString(),
                                Street = dr["Street"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                UserName = dr["UserName"].ToString(),
                                SalesCommissionStructure = dr["SalesCommissionStructure"].ToString(),
                                Address = dr["Address"].ToString(),
                                Phone = dr["Phone"].ToString()
                            }).ToList();
            return EmployeeList;
        }
        public List<Employee> GetEmployeeByCompanyIdAndTagTechnician(Guid CompanyId, string Tag, Guid userid, string tag2 = "")
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByCompanyIdAndTagTechnician(CompanyId, Tag, userid, tag2);
            List<Employee> EmployeeList = new List<Employee>();
            HashSet<Guid> userIds = new HashSet<Guid>(); // To track unique UserIds

            foreach (DataRow dr in dt.Rows)
            {
                Guid userId = dr.Table.Columns.Contains("UserId") ? new Guid(dr["UserId"].ToString()) : Guid.Empty;

                // Check if UserId is already added to the list, if yes, skip this row
                if (userIds.Contains(userId))
                    continue;

                userIds.Add(userId); // Add UserId to HashSet to mark it as encountered

                Employee employee = new Employee()
                {
                    UserId = userId,
                    City = dr.Table.Columns.Contains("City") ? dr["City"].ToString() : string.Empty,
                    Street = dr.Table.Columns.Contains("Street") ? dr["Street"].ToString() : string.Empty,
                    ZipCode = dr.Table.Columns.Contains("ZipCode") ? dr["ZipCode"].ToString() : string.Empty,
                    State = dr.Table.Columns.Contains("State") ? dr["State"].ToString() : string.Empty,
                    FirstName = dr.Table.Columns.Contains("FirstName") ? dr["FirstName"].ToString() : string.Empty,
                    LastName = dr.Table.Columns.Contains("LastName") ? dr["LastName"].ToString() : string.Empty,
                    Email = dr.Table.Columns.Contains("Email") ? dr["Email"].ToString() : string.Empty,
                    UserName = dr.Table.Columns.Contains("UserName") ? dr["UserName"].ToString() : string.Empty,
                    SalesCommissionStructure = dr.Table.Columns.Contains("SalesCommissionStructure") ? dr["SalesCommissionStructure"].ToString() : string.Empty,
                    Address = dr.Table.Columns.Contains("Address") ? dr["Address"].ToString() : string.Empty,
                    Phone = dr.Table.Columns.Contains("Phone") ? dr["Phone"].ToString() : string.Empty
                };

                if (dt.Columns.Contains("TechnicianCount"))
                {
                    int technicianCount = Convert.ToInt32(dr["TechnicianCount"]);
                    employee.TechnicianCount = technicianCount;
                }

                EmployeeList.Add(employee);
            }

            return EmployeeList;
        }

        public List<Employee> GetEmployeeByCompanyIdAndPerGrpId(Guid CompanyId, string Tag, Guid userid, string PerGrpId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByCompanyIdAndPerGrpId(CompanyId, Tag, userid, PerGrpId);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                City = dr["City"].ToString(),
                                Street = dr["Street"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                UserName = dr["UserName"].ToString(),
                                SalesCommissionStructure = dr["SalesCommissionStructure"].ToString(),
                                Address = dr["Address"].ToString(),
                                Phone = dr["Phone"].ToString()
                            }).ToList();
            return EmployeeList;
        }

        public List<Employee> GetAllEmployeeByCompanyId(Guid CompanyId)
        {
            DataTable dt = _EmployeeDataAccess.GetAllEmployeeByCompanyId(CompanyId);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                City = dr["City"].ToString(),
                                Street = dr["Street"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                UserName = dr["UserName"].ToString(),
                                SalesCommissionStructure = dr["SalesCommissionStructure"].ToString()
                            }).ToList();
            return EmployeeList;
        }
        public List<Employee> GetTechtransferEmployeeByCompanyIdAndTag(Guid CompanyId, string searchtext, string Tag, Guid userid, string tag2 = "")
        {
            DataTable dt = _EmployeeDataAccess.GetTechtransferEmployeeByCompanyIdAndTag(CompanyId, searchtext, Tag, userid, tag2);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                City = dr["City"].ToString(),
                                Street = dr["Street"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                UserName = dr["UserName"].ToString(),
                                SalesCommissionStructure = dr["SalesCommissionStructure"].ToString(),
                                Address = dr["Address"].ToString()
                            }).ToList();
            return EmployeeList;
        }
        public List<Employee> GetEmployeeByCompanyIdAndTagAndSearch(Guid CompanyId,string searchtext, string Tag, Guid userid, string tag2 = "")
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByCompanyIdAndTagAndSearch(CompanyId, searchtext, Tag, userid, tag2);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                City = dr["City"].ToString(),
                                Street = dr["Street"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                UserName = dr["UserName"].ToString(),
                                SalesCommissionStructure = dr["SalesCommissionStructure"].ToString(),
                                Address = dr["Address"].ToString()
                            }).ToList();
            return EmployeeList;
        }

        public EmployeeListModel GetTechnicianByCompanyIdAndTagAndSearch(Guid CompanyId, DateTime? start, DateTime? end, int pageno, int pagesize, string searchtext, string order, string Tag)
        {
            EmployeeListModel Model = new EmployeeListModel();
            DataSet ds = _EmployeeDataAccess.GetTechnicianByCompanyIdAndTagAndSearch(CompanyId, start, end, pageno, pagesize, searchtext, order, Tag);
            Model.Employees = (from DataRow dr in ds.Tables[0].Rows
                               select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                //City = dr["City"].ToString(),
                                //Street = dr["Street"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //State = dr["State"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                //Email = dr["Email"].ToString(),
                                UserName = dr["UserName"].ToString()
                                //SalesCommissionStructure = dr["SalesCommissionStructure"].ToString(),
                                //Address = dr["Address"].ToString()
                            }).ToList();
            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = pageno;
            Model.PageSize = pagesize;
            Model.Searchtext = searchtext;
            Model.StartDate = start.Value;
            Model.EndDate = end.Value;
            return Model;
        }

        public DataTable GetInventoryTechTruckReportForDownload(Guid comId,DateTime? start, DateTime? end, string searchtext, string tag)
        {
            DataTable dt = _EmployeeDataAccess.GetInventoryTechTruckReportForDownload(comId,start, end, searchtext,tag);
            return dt;
        }

        public DataTable GetInventoryTechUsedReportForDownload(Guid comId, DateTime? start, DateTime? end, string searchtext, string tag)
        {
            DataTable dt = _EmployeeDataAccess.GetInventoryTechUsedReportForDownload(comId, start, end, searchtext, tag);
            return dt;
        }

        public DataTable GetInventoryTechOrderReportForDownload(Guid comId, DateTime? start, DateTime? end, string searchtext, string tag)
        {
            DataTable dt = _EmployeeDataAccess.GetInventoryTechOrderReportForDownload(comId, start, end, searchtext, tag);
            return dt;
        }
        public List<Employee> GetEmployee()
        {
            DataTable dt = _EmployeeDataAccess.GetEmployee();
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                
                            }).ToList();
            return EmployeeList;
        }
        public List<Employee> Getfollowupsalesperson()
        {
            DataTable dt = _EmployeeDataAccess.Getfollowupsalesperson();
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),

                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),

                            }).ToList();
            return EmployeeList;
        }
        public List<Employee> GetSalesPerson()
        {
            DataTable dt = _EmployeeDataAccess.GetSalesPerSon();
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),

                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),

                            }).ToList();
            return EmployeeList;
        }
        public List<Customer> GetSalesPersonCustomer()
        {
            DataTable dt = _EmployeeDataAccess.GetSalesPerSonCustomer();
            List<Customer> EmployeeList = new List<Customer>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                 

                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),

                            }).ToList();
            return EmployeeList;
        }
        public List<Employee> GetEmployeeByCompanyIdAndTagAndTechnician(Guid CompanyId, string Tag, Guid userid)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByCompanyIdAndTagAndTechnician(CompanyId, Tag, userid);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                UserName = dr["UserName"].ToString()
                            }).ToList();
            return EmployeeList;
        }

        public Employee GetemployeeByFirstNameAndLastNameOrCSID(string userName,int CSId)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format(" FirstName + ' '+ LastName = '{0}' OR CSId = {1} ",userName, CSId)).FirstOrDefault();
        }

        public Employee GetEmployeeByEmailAddress(string userEmail)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format(" Email = '{0}' ", userEmail)).FirstOrDefault();
        }

        public List<Employee> GetSingleTechnicianList(Guid userId)
        {
            var query = "UserId='" + userId + "'";
            return _EmployeeDataAccess.GetByQuery(query);
        }
        public List<Employee> GetALLEmployeeByCompanyIdAndIsRecruted(Guid CompanyId)
        {
            List<Employee> EmployeeList = _EmployeeDataAccess.GetALLEmployeeByCompanyIdAndIsRecruted(CompanyId);
            return EmployeeList;
        }


        public List<Employee> GetAllWorkPersonEmployee(Guid companyid)
        {
            DataTable dt = _EmployeeDataAccess.GetAllWorkPersonEmployee(companyid);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                UserId = new Guid(dr["UserId"].ToString()),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                UserName = dr["UserName"].ToString()
                            }).ToList();
            return EmployeeList;
        }

        public List<Employee> GetEmployeeListBySupervisorId(Guid userId)
        {
            return _EmployeeDataAccess.GetEmployeeListBySupervisorId(userId);
        } 
        public List<Employee> GetTimeClockEmployeeListBySupervisorId(Guid userId)
        {
            return _EmployeeDataAccess.GetTimeClockEmployeeListBySupervisorId(userId);
        }

        public List<Employee> GetEmployeeListByUserTag(Guid companyid, Guid userid)
        {
            return _EmployeeDataAccess.GetEmployeeListByUserTag(companyid, userid);
        }
        //public Employee GetEmployeeByuserIdandCompanyId(Guid companyid, Guid userid)
        //{
        //    return _EmployeeDataAccess.GetEmployeeByuserIdandCompanyId(companyid, userid);
        //}
        public List<Employee> GetEmployeeByuserIdandCompanyId(Guid companyid, Guid userid)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByuserIdandCompanyId(companyid, userid);
            List<Employee> Emplist = new List<Employee>();
            Emplist = (from DataRow dr in dt.Rows
                       select new Employee()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           UserLoginId = dr["UserLoginId"] != DBNull.Value ? Convert.ToInt32(dr["UserLoginId"]) : 0,
           
                       }).ToList();
            return Emplist;
        }
        public Employee GetNewEmployeeByuserIdandCompanyId(Guid userid)
        {
            DataTable dt = _EmployeeDataAccess.GetNewEmployeeByuserIdandCompanyId(userid);
            Employee Emplist = new Employee();
            Emplist = (from DataRow dr in dt.Rows
                       select new Employee()
                       {
                           PasswordUpdateDays = dr["PasswordUpdateDays"] != DBNull.Value ? Convert.ToInt32(dr["PasswordUpdateDays"]) : 0,
                           Id  =dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           UserId=(Guid)dr["UserId"],
                           UserName =dr["UserName"].ToString(),
                           Title =         dr["Title"].ToString(),
                           FirstName =     dr["FirstName"].ToString(),
                           LastName =      dr["LastName"].ToString(),
                           Email =         dr["Email"].ToString(),
                           Street=         dr["Street"].ToString(),
                           City =          dr["City"].ToString(),
                           State =         dr["State"].ToString(),
                           ZipCode=        dr["ZipCode"].ToString(),
                           Country =       dr["Country"].ToString(),
                           Phone =         dr["Phone"].ToString(),
                           SSN  =          dr["SSN"].ToString(),
                           IsActive =      dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                           IsDeleted=      dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
                           HireDate =      dr["HireDate"] != DBNull.Value ? Convert.ToDateTime(dr["HireDate"]) : new DateTime(),
                           ProfilePicture= dr["ProfilePicture"].ToString(),
                           Session=        dr["Session"].ToString(),
                            JobTitle =     dr["JobTitle"].ToString(),
                            PlaceOfBirth = dr["PlaceOfBirth"].ToString(),
                           SalesCommissionStructure=          dr["SalesCommissionStructure"].ToString(),
                           TechCommissionStructure =         dr["TechCommissionStructure"].ToString(),
                           RecruitmentProcess =              dr["RecruitmentProcess"] != DBNull.Value ? Convert.ToBoolean(dr["RecruitmentProcess"]) : false,
                           Recruited  =                       dr["Recruited"] != DBNull.Value ? Convert.ToBoolean(dr["Recruited"]) : false,
                           IsCalendar  =                      dr["IsCalendar"] != DBNull.Value ? Convert.ToBoolean(dr["IsCalendar"]) : false,
                           CalendarColor =                    dr["CalendarColor"].ToString(),
                           CreatedDate =                      dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                           Status     =                       dr["Status"].ToString(),
                           LastUpdatedBy                 = dr["LastUpdatedBy"].ToString(),
                           LastUpdatedDate               = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                           IsSupervisor                  = dr["IsSupervisor"] != DBNull.Value ? Convert.ToBoolean(dr["IsSupervisor"]) : false,
                           SuperVisorId                  = dr["SuperVisorId"].ToString(),
                           HourlyRate                    = dr["HourlyRate"] != DBNull.Value ? Convert.ToDouble(dr["HourlyRate"]) : 0,
                           NoAutoClockOut               = dr["NoAutoClockOut"] != DBNull.Value ? Convert.ToBoolean(dr["NoAutoClockOut"]) : false,
                           FireLicenseExpirationDate    = dr["FireLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["FireLicenseExpirationDate"]) : new DateTime(),
                           SalesLicenseExpirationDate   = dr["SalesLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesLicenseExpirationDate"]) : new DateTime(),
                           InstallLicenseExpirationDate = dr["InstallLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallLicenseExpirationDate"]) : new DateTime(),
                           DriversLicenseExpirationDate=  dr["DriversLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["DriversLicenseExpirationDate"]) : new DateTime(),
                           ClockInIP            =         dr["ClockInIP"].ToString(),
                           DOB                  =         dr["DOB"] != DBNull.Value ? Convert.ToDateTime(dr["DOB"]) : new DateTime(),
                           BasePay              =         dr["BasePay"].ToString(),
                           EmpType              =         dr["EmpType"].ToString(),
                           Department           =         dr["Department"].ToString(),
                           PtoRate              =         dr["PtoRate"] != DBNull.Value ? Convert.ToDouble(dr["PtoRate"]) : 0,
                           PtoHour              =         dr["PtoHour"] != DBNull.Value ? Convert.ToDouble(dr["PtoHour"]) : 0,
                           PtoRemain            =         dr["PtoRemain"] != DBNull.Value ? Convert.ToDouble(dr["PtoRemain"]) : 0,
                           IsPayroll            =         dr["IsPayroll"] != DBNull.Value ? Convert.ToBoolean(dr["IsPayroll"]) : false,
                           LicenseNo            =         dr["LicenseNo"].ToString(),
                           AnniversaryDate      =         dr["AnniversaryDate"] != DBNull.Value ? Convert.ToDateTime(dr["AnniversaryDate"]) : new DateTime(),
                           BadgerUserId         =         dr["BadgerUserId"].ToString(),
                           AlarmId              =         dr["AlarmId"].ToString(),
                           UserXComission       =         dr["UserXComission"] != DBNull.Value ? Convert.ToInt32(dr["UserXComission"]) : 0,
                           IsCurrentEmployee    =         dr["IsCurrentEmployee"] != DBNull.Value ? Convert.ToBoolean(dr["IsCurrentEmployee"]) : false,
                           CSId                 =         dr["CSId"] != DBNull.Value ? Convert.ToInt32(dr["CSId"]) : 0,
                           Street2              =         dr["Street2"].ToString(),
                           City2                =         dr["City2"].ToString(),
                           State2               =         dr["State2"].ToString(),
                           ZipCode2             =         dr["ZipCode2"].ToString(),
                           StreetPrevious       =         dr["StreetPrevious"].ToString(),
                           IsSalesMatrixUserX   =      dr["IsSalesMatrixUserX"] != DBNull.Value ? Convert.ToBoolean(dr["IsSalesMatrixUserX"]) : false,
                           TerminationDate      =      dr["TerminationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TerminationDate"]) : new DateTime(),
                           CompanyId            =      new Guid(dr["CompanyId"].ToString()),
                           TermSheetId          =      new Guid(dr["TermSheetId"].ToString()),
                           BrinksDealerUser     =      dr["BrinksDealerUser"].ToString(),
                           BrinksDealerPassword =      dr["BrinksDealerPassword"].ToString(),
                           IsSalesMatrix        =      dr["IsSalesMatrix"] != DBNull.Value ? Convert.ToBoolean(dr["IsSalesMatrix"]) : false,
                           IsDefaultInCalendar  =      dr["IsDefaultInCalendar"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultInCalendar"]) : false,
                           IsLocation           =      dr["IsLocation"] != DBNull.Value ? Convert.ToBoolean(dr["IsLocation"]) : false,
                       }).FirstOrDefault();
            return Emplist;
        }
        public List<Employee> GetCurrentEmployeeListByCompanyId(Guid companyid)
        {
            return _EmployeeDataAccess.GetCurrentEmployeeListByCompanyId(companyid);
        }

        public List<Employee> GetAllServiceOrderEmployee()
        {
            DataTable dt = _EmployeeDataAccess.GetAllServiceOrderEmployee();
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                UserId = new Guid(dr["UserId"].ToString()),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                UserName = dr["UserName"].ToString()
                            }).ToList();
            return EmployeeList;
        }
        //public List<Employee> GetAllSalesPersonEmployeeByCompnayId(Guid CompanyId)
        //{
        //    DataTable dt = _EmployeeDataAccess.GetAllSalesPersonEmployeeByCompnayId(CompanyId);
        //    List<Employee> EmployeeList = new List<Employee>();
        //    EmployeeList = (from DataRow dr in dt.Rows
        //                    select new Employee()
        //                    {
        //                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                        UserId = (Guid)dr["UserId"],
        //                        UserName = dr["UserName"].ToString(),
        //                        Title = dr["Title"].ToString(),
        //                        FirstName = dr["FirstName"].ToString(),
        //                        LastName = dr["LastName"].ToString(),
        //                        Street = dr["Street"].ToString(),
        //                        City = dr["City"].ToString(),
        //                        State = dr["State"].ToString(),
        //                        ZipCode = dr["ZipCode"].ToString(),
        //                        Country = dr["Country"].ToString(),
        //                        IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
        //                        IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
        //                        //IsInstaller = dr["IsInstaller"] != DBNull.Value ? Convert.ToBoolean(dr["IsInstaller"]) : false,
        //                        //IsSalesPerson = dr["IsSalesPerson"] != DBNull.Value ? Convert.ToBoolean(dr["IsSalesPerson"]) : false,
        //                        //IsServiceCall = dr["IsServiceCall"] != DBNull.Value ? Convert.ToBoolean(dr["IsServiceCall"]) : false,
        //                        LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
        //                        LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
        //                    }).ToList();
        //    return EmployeeList;
        //}

        public List<Employee> GetAllInstallerEmployeeByCompnayId(Guid CompanyId)
        {
            DataTable dt = _EmployeeDataAccess.GetAllInstallerEmployeeByCompnayId(CompanyId);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                //Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = (Guid)dr["UserId"],
                                //UserName = dr["UserName"].ToString(),
                                //Title = dr["Title"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                //Street = dr["Street"].ToString(),
                                //City = dr["City"].ToString(),
                                //State = dr["State"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //Country = dr["Country"].ToString(),
                                //IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                //IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
                                //IsInstaller = dr["IsInstaller"] != DBNull.Value ? Convert.ToBoolean(dr["IsInstaller"]) : false,
                                //IsSalesPerson = dr["IsSalesPerson"] != DBNull.Value ? Convert.ToBoolean(dr["IsSalesPerson"]) : false,
                                //IsServiceCall = dr["IsServiceCall"] != DBNull.Value ? Convert.ToBoolean(dr["IsServiceCall"]) : false,
                                //LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                //LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                            }).ToList();
            return EmployeeList;
        }

        public Employee GetEmployeeByEmployeeId(Guid EmployeeId)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserId = '{0}'", EmployeeId)).FirstOrDefault();
        }
         
        public Employee GetEmployeeByEmployeeIdAndIsCalendar(Guid EmployeeId)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserId = '{0}' and IsCalendar = 1", EmployeeId)).FirstOrDefault();
        }
     
        public List<Employee> GetAllEmployee(Guid Companyid)
        {
            return _EmployeeDataAccess.GetAllEmployee(Companyid);
        }

        public List<Employee> GetTransferLocations(Guid Companyid)
        {
            return _EmployeeDataAccess.GetTransferLocations(Companyid);
        }
        public List<Employee> GetAllEmployeeForCalendar(Guid Companyid)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("IsCalendar is not null and IsCalendar = 1 and Recruited = 1 and IsCurrentEmployee = 1 and IsActive = 1 and IsDeleted = 0 and CompanyId = '{0}'", Companyid)).ToList();
        }
        public List<Employee> GetAllEmployeeDefaultForCalendar(Guid Companyid)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("IsCalendar is not null and IsCalendar = 1 and Recruited = 1 and IsCurrentEmployee = 1 and IsActive = 1 and IsDeleted = 0 and IsDefaultInCalendar is not null and IsDefaultInCalendar = 1 and CompanyId = '{0}'", Companyid)).ToList();
        }
        public List<Employee> GetAllEmployeeByCompanyIdForFinanced(Guid Companyid)
        {
            return _EmployeeDataAccess.GetAllEmployeeByCompanyIdForFinanced(Companyid);
        }
        public List<Employee> GetAllEmployeeByTicketUserIsprimary(Guid CompanyId)
        {
            return _EmployeeDataAccess.GetAllEmployeeByTicketUserIsprimary(CompanyId);
        }
        public List<Employee> GetAllEmployee()
        {
            return _EmployeeDataAccess.GetAll();
        }
        public List<Employee> GetAllEmployeeWithHireDate()
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("HireDate is not null Or HireDate = '' and IsActive = 1 ")).ToList();
        }
        public List<Employee> GetAllEmployeeByTicketAssigned(Guid Companyid, Guid userid)
        {
            return _EmployeeDataAccess.GetAllEmployeeByTicketAssigned(Companyid, userid);
        }

        public List<Employee> GetAllEmployeeByBirthDate(Guid Companyid,DateTime StartDate,DateTime EndDate)
        {
            DataTable dt = _EmployeeDataAccess.GetAllEmployeeByBirthDate(Companyid, StartDate, EndDate);
            List<Employee> Emplist = new List<Employee>();
            Emplist = (from DataRow dr in dt.Rows
                              select new Employee()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  UserLoginId = dr["UserLoginId"] != DBNull.Value ? Convert.ToInt32(dr["UserLoginId"]) : 0,
                                  FirstName = dr["FirstName"].ToString(),
                                  LastName = dr["LastName"].ToString(),
                                  DOB = dr["DOB"] != DBNull.Value ? Convert.ToDateTime(dr["DOB"]) : new DateTime(),
                              }).ToList();
            return Emplist;
        }
        
        public List<Employee> GetAllEmployeeByHireDate(DateTime FromDate,DateTime EndDate )
        {
           return _EmployeeDataAccess.GetAllEmployeeByHirehDate(FromDate, EndDate); 
        }
        public List<EmployeePTOHourLog> GetEmployeeTotalPtoHour(Guid UserId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeePtohour(UserId);
            List<EmployeePTOHourLog> Emplist = new List<EmployeePTOHourLog>();
            Emplist = (from DataRow dr in dt.Rows
                       select new EmployeePTOHourLog()
                       { 
                           TotalPTOHour = dr["TotalPTOHour"] != DBNull.Value ? Convert.ToDouble(dr["TotalPTOHour"]) : 0,
                       }).ToList();
            return Emplist;
        }
        public List<Pto> GetEmployeeAccrualPtoHourByUserId(Guid UserId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeAccrualPtoHourByUserId(UserId);
            List<Pto> Emplist = new List<Pto>();
            Emplist = (from DataRow dr in dt.Rows
                       select new Pto()
                       {
                           TotalMinute = dr["TotalMinute"] != DBNull.Value ? Convert.ToDouble(dr["TotalMinute"]) : 0,
                       }).ToList();
            return Emplist;
        }
        //public EmployeePTOHourLog GetTotalPTOHour(Guid employeeid)
        //{
        //    var query = string.Format(" Select Sum(PTOHour) As TotalPTOHour from EmployeePTOHourLog Where UserId = '{0}'", employeeid);
        //    return _EmployeePTOHourLogDataAccess.GetByQuery(query).FirstOrDefault();
        //}
        public List<Employee> GetAllEmployeeByAnniversaryDate(Guid Companyid, DateTime StartDate, DateTime EndDate)
        {
            DataTable dt = _EmployeeDataAccess.GetAllEmployeeByAnniversaryDate(Companyid, StartDate, EndDate);
            List<Employee> Emplist = new List<Employee>();
            Emplist = (from DataRow dr in dt.Rows
                       select new Employee()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           UserLoginId = dr["UserLoginId"] != DBNull.Value ? Convert.ToInt32(dr["UserLoginId"]) : 0,
                           FirstName = dr["FirstName"].ToString(),
                           LastName = dr["LastName"].ToString(),
                           AnniversaryYears = dr["AnniversaryYears"] != DBNull.Value ? Convert.ToInt32(dr["AnniversaryYears"]) : 0,
                           HireDate = dr["HireDate"] != DBNull.Value ? Convert.ToDateTime(dr["HireDate"]) : new DateTime(),
                       }).ToList();
            return Emplist;
        }
        public List<NoteAssign> GetAllEmployeeNameeByEmployeeId(Guid employeeid)
        {
            DataTable dt = _NoteAssignDataAccess.GetAllEmployeeNameeByEmployeeId(employeeid);
            List<NoteAssign> noteassignlist = new List<NoteAssign>();
            noteassignlist = (from DataRow dr in dt.Rows
                              select new NoteAssign()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  NoteId = dr["NoteId"] != DBNull.Value ? Convert.ToInt32(dr["NoteId"]) : 0,
                                  EmployeeId = (Guid)dr["EmployeeId"]
                              }).ToList();
            return noteassignlist;
            //return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyId));
        }
        public Employee GetEmployeeNameByEmployeeId(Guid employeeid)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format(" UserId = '{0}'", employeeid)).FirstOrDefault();
        }
        public string GetEmployeeNumByEmployeeId(Guid employeeid)
        {
            string EmapName = "";
            var result = _EmployeeDataAccess.GetByQuery(string.Format(" UserId = '{0}'", employeeid)).FirstOrDefault();
            if (result != null)
            {
                EmapName = result.FirstName + " " + result.LastName;
            }
            return EmapName;
        }
        public List<Lookup> GetLookupDisplaytext(string DataValue)
        {
            DataTable dt = _NoteAssignDataAccess.GetDisplayTextByDataValue(DataValue);
            List<Lookup> lookuplist = new List<Lookup>();
            lookuplist = (from DataRow dr in dt.Rows
                              select new Lookup()
                              {
                                  DisplayText = dr["DisplayText"].ToString(),
                              }).ToList();
            return lookuplist;
        }
        public List<Partner> GetEmployeeByPartnerId(Guid SupervisorId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByPartnerId(SupervisorId);
            List<Partner> PartnerList = new List<Partner>();
            PartnerList = (from DataRow dr in dt.Rows
                           select new Partner()
                           {
                               UserId = (Guid)dr["UserId"],
                               SupervisorId = new Guid(dr["SupervisorId"].ToString()),
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString()
                           }).ToList();
            return PartnerList;
        }

        public long InsertUserBranch(UserBranch ub)
        {
            return _UserBranchDataAccess.Insert(ub);
        }
        public string GetEmpolyeeNameByUserName(string UserName)
        {
            string EmapName = "";
            var result = _EmployeeDataAccess.GetByQuery(string.Format("UserName = '{0}'", UserName)).FirstOrDefault();
            if (result != null)
            {
                EmapName = result.FirstName + " " + result.LastName;
            }
            return EmapName;
        }

        public PermissionGroup GetEmployeeRoleByEmployeeIdAndCompanyId(Guid EmployeeId, Guid CompanyId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeRoleByEmployeeIdAndCompanyId(EmployeeId, CompanyId);
            PermissionGroup EmployeeList = new PermissionGroup();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new PermissionGroup()
                            {
                                Name = dr["Name"].ToString(),
                                Tag = dr["Tag"].ToString()
                            }).FirstOrDefault();
            return EmployeeList;
        }


        //public List<Employee> GetAllTechnicianByCompanyId(Guid CompanyId)
        //{
        //    DataTable dt = _EmployeeDataAccess.GetAllTechnicianByCompnayId(CompanyId);
        //    List<Employee> EmployeeList = new List<Employee>();
        //    EmployeeList = (from DataRow dr in dt.Rows
        //                    select new Employee()
        //                    {
        //                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                        UserId = (Guid)dr["UserId"],
        //                        UserName = dr["UserName"].ToString(),
        //                        Title = dr["Title"].ToString(),
        //                        FirstName = dr["FirstName"].ToString(),
        //                        LastName = dr["LastName"].ToString(),
        //                        Street = dr["Street"].ToString(),
        //                        City = dr["City"].ToString(),
        //                        State = dr["State"].ToString(),
        //                        ZipCode = dr["ZipCode"].ToString(),
        //                        Country = dr["Country"].ToString(),
        //                        IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
        //                        IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
        //                        //IsInstaller = dr["IsInstaller"] != DBNull.Value ? Convert.ToBoolean(dr["IsInstaller"]) : false,
        //                        //IsSalesPerson = dr["IsSalesPerson"] != DBNull.Value ? Convert.ToBoolean(dr["IsSalesPerson"]) : false,
        //                        //IsServiceCall = dr["IsServiceCall"] != DBNull.Value ? Convert.ToBoolean(dr["IsServiceCall"]) : false,
        //                        LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
        //                        LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
        //                    }).ToList();
        //    return EmployeeList;
        //    //return _EmployeeDataAccess.GetByQuery(string.Format(" EmployeeId = '{0}'", employeeId));
        //}

        public bool CheckTechnicianIsInCompany(Guid EmployeeId, Guid CompanyId)
        {
            bool result = false;
            DataTable dt = _EmployeeDataAccess.CheckTechnicianIsInCompany(EmployeeId, CompanyId);
            var dtresult = dt.Rows.Count;
            if (dtresult > 0)
            {
                result = true;
            }
            return result;
        }

        public Employee GetEmployeeById(int id)
        {
            return _EmployeeDataAccess.Get(id);
        }

        public bool DeleteEmployeeByUsername(string userName)
        {
            return _EmployeeDataAccess.DeleteEmployeeByUsername(userName);
        }
        public bool DeleteEmployeeByUserId(Guid UserId)
        {
            return _EmployeeDataAccess.DeleteEmployeeByUserId(UserId);
        }

        public UserBranch GetUserBranchByCompanyIdAndUserName(Guid comid, string user)
        {
            return _UserBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and UserName = '{1}'", comid, user)).FirstOrDefault();
        }
        public UserBranch GetUserBranchByCompanyIdAndUserId(Guid comid, Guid userId)
        {
            return _UserBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and UserId = '{1}'", comid, userId)).FirstOrDefault();
        }
        public bool UpdateUserBranch(UserBranch ub)
        {
            return _UserBranchDataAccess.Update(ub) > 0;
        }

        public EmployeeNote GetEmployeeNoteById(int value)
        {
            return _EmployeeNoteDataAccess.Get(value);
        }

        public bool UpdateEmployeeNote(EmployeeNote en)
        {
            return _EmployeeNoteDataAccess.Update(en) > 0;
        }

        public long InsertEmployeeNote(EmployeeNote en)
        {
            return _EmployeeNoteDataAccess.Insert(en);
        }

        public List<EmployeeNote> GetAllEmployeeNoteByCompanyIdAndEmployeeId(Guid comid, Guid empid)
        {
            return _EmployeeNoteDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and EmployeeId = '{1}' and IsShedule = 0 and IsFollowUp = 0 and IsActive = 1", comid, empid)).ToList();
        }

        public List<EmployeeNote> GetAllEmployeeRemainderByCompanyIdAndEmployeeId(Guid comid, Guid empid)
        {
            return _EmployeeNoteDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and EmployeeId = '{1}' and IsEmail = 0 and IsText = 0 and IsShedule = 0 and IsActive = 1", comid, empid)).ToList();
        }

        public List<EmployeeNote> GetAllEmployeeScheduleByCompanyIdAndEmployeeId(Guid comid, Guid empid)
        {
            return _EmployeeNoteDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and EmployeeId = '{1}' and IsEmail = 0 and IsText = 0 and IsFollowUp = 0 and IsActive = 1", comid, empid)).ToList();
        }

        public void DeleteEmployeeNote(int id)
        {
            _EmployeeNoteDataAccess.Delete(id);
        }

        public List<PayrollReport> GetAllPayrollReport(Guid CompanyId, string start,string end)
        {

            DateTime StartDate = start.ToDateTime();
            DateTime EndDate = end.ToDateTime();

            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                StartDate = StartDate.SetZeroHour().ClientToUTCTime();
                EndDate = EndDate.SetMaxHour().ClientToUTCTime();
            }

            DataTable dt = _EmployeeDataAccess.GetAllPayrollReport(CompanyId, StartDate, EndDate);
            List<PayrollReport> EmployeeList = new List<PayrollReport>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new PayrollReport()
                            {
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                UserId = (Guid)dr["UserId"],
                                HourlyRate = dr["HourlyRate"] != DBNull.Value ? Convert.ToDouble(dr["HourlyRate"]) : 0,
                                RegularHours = dr["RegularHours"] != DBNull.Value ? Convert.ToDouble(dr["RegularHours"]) : 0,
                                TotalCommission = dr["TotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCommission"]) : 0,
                            }).ToList();
            return EmployeeList;
        }
        public EmployeePayrollReport GetAllEmpPayrollReport(Guid SupervisorId, DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize)
        {
            DataSet ds = _EmployeeDataAccess.GetAllEmpPayrollReport(SupervisorId, FilterStartDate, FilterEndDate, order, pageno, pagesize);
            List<PayrollReport> buildList = new List<PayrollReport>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new PayrollReport()
                         {
                             FirstName = dr["FirstName"].ToString(),
                             LastName = dr["LastName"].ToString(),
                             UserId = (Guid)dr["UserId"],
                             HourlyRate = dr["HourlyRate"] != DBNull.Value ? Convert.ToDouble(dr["HourlyRate"]) : 0,
                             RegularHours = dr["RegularHours"] != DBNull.Value ? Convert.ToDouble(dr["RegularHours"]) : 0,
                             //TotalCommission = dr["TotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCommission"]) : 0,
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            EmployeePayrollReport EmpPayrollFilter = new EmployeePayrollReport();
            EmpPayrollFilter.PayrollReportList = buildList;
            EmpPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            return EmpPayrollFilter;
        }

        public EmpSaleCommisionReport GetAllSalesCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize,bool IsPaid,Guid UserId,string SearchText,string SalesPersonList)
        {
            DataSet ds = _EmployeeDataAccess.GetAllSalesReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid,UserId, SearchText, SalesPersonList);
            List<SalesCommission> buildList = new List<SalesCommission>();
            buildList = (from DataRow dr in ds.Tables[1].Rows
                         select new SalesCommission()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerName = dr["CustomerName"].ToString(),
                             TiketIdValue = dr["tkId"] != DBNull.Value ? Convert.ToInt32(dr["tkId"]) : 0,
                             TicketId = (Guid)dr["TicketId"],
                             SalesPerson = dr["SalesPerson"].ToString(),
                             CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                             RMRSold =  dr["RMRSold"] != DBNull.Value ? Convert.ToDouble(dr["RMRSold"]) : 0,
                             RMRCommission = dr["RMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["RMRCommission"]) : 0,
                             TotalCommission = dr["TotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCommission"]) : 0,
                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             EquipmentCommission = dr["EquipmentCommission"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCommission"]) : 0,
                             NoOfEquipment = dr["NoOfEquipment"] != DBNull.Value ? Convert.ToInt32(dr["NoOfEquipment"]) : 0,
                             CreatedBy = (Guid)dr["CreatedBy"],
                             UserId = (Guid)dr["UserId"],
                             IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                             Batch = dr["Batch"].ToString(),
                             CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,
                             Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                             TotalPoint = dr["TotalPoint"] != DBNull.Value ? Convert.ToDouble(dr["TotalPoint"]) : 0,
                             OriginalPoint = dr["OriginalPoint"] != DBNull.Value ? Convert.ToDouble(dr["OriginalPoint"]) : 0
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[0].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TotalPayrollSum TotalSalesCount = new TotalPayrollSum();
            TotalSalesCount = (from DataRow dr in ds.Tables[2].Rows
                               select new TotalPayrollSum()
                               {
                                   TotalAdjustment = dr["TotalAdjustment"] != DBNull.Value ? Convert.ToDouble(dr["TotalAdjustment"]) : 0.0,
                                   TotalCommission = dr["SumCommission"] != DBNull.Value ? Convert.ToDouble(dr["SumCommission"]) : 0.0,
                                   TotalCommissionablePoints = dr["SumCommissionablePoint"] != DBNull.Value ? Convert.ToDouble(dr["SumCommissionablePoint"]) : 0.0,
                                   TotalOverage = dr["SumOverage"] != DBNull.Value ? Convert.ToDouble(dr["SumOverage"]) : 0.0,
                                   TotalPoint = dr["SumPoint"] != DBNull.Value ? Convert.ToDouble(dr["SumPoint"]) : 0.0,
                                   TotalUnpaidBalance = dr["SumUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["SumUnpaid"]) : 0.0,
                               }).FirstOrDefault();
            EmpSaleCommisionReport EmpPayrollFilter = new EmpSaleCommisionReport();
            EmpPayrollFilter.PayrollReportList = buildList;
            EmpPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            EmpPayrollFilter.TotalSalesCount = TotalSalesCount;
            return EmpPayrollFilter;
        }
        public DataTable GetDownLoadAllSalesCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string SalesPersonList)
        {
            DataSet ds = _EmployeeDataAccess.GetDownLoadAllSalesReport(FilterStartDate, FilterEndDate, order, 1, 9999, IsPaid, UserId, SearchText, SalesPersonList);
            //List<SalesCommission> buildList = new List<SalesCommission>();
            DataTable buildList = ds.Tables[0];
                         

         
            return buildList;
        }

        public TicketListModel GetAllTicketReportByFilter(TicketFilter Filters, FilterReportModel filter)
       {
            TicketListModel Model = new TicketListModel();

            DataSet ds = _EmployeeDataAccess.GetTicketListByFilter(Filters, filter);
            Model.Tickets = (from DataRow dr in ds.Tables[1].Rows
                             select new Ticket()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 CompanyId = (Guid)dr["CompanyId"],
                                 CustomerName = dr["CustomerName"].ToString(),
                                 SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                 InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                 CustomerId = (Guid)dr["CustomerId"],
                                 TicketId = (Guid)dr["TicketId"],
                                 CreatedBy = (Guid)dr["TicketId"],
                                 Status = dr["Status"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 AssignedTo = dr["AssignedTo"].ToString().TrimEnd(' ', ','),
                                 //AdditionalMembers = dr["AdditionalMembers"].ToString().TrimEnd(' ', ','),
                                 Priority = dr["Priority"].ToString(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                 RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,
                                 //AppointmentStartTimeVal = dr["AppointmentStartTimeVal"].ToString(),
                                 //AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                 //AppointmentEndTime = dr["AppointmentStartTime"].ToString(),
                                 //AppointmentEndTimeVal = dr["AppointmentEndTimeVal"].ToString(),
                                 TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                 StatusVal = dr["StatusVal"].ToString(),
                                 PriorityVal = dr["PriorityVal"].ToString(),
                                 CreatedByVal = dr["CreatedByVal"].ToString(),
                                 //ExceedQuantity = dr["ExceedQuantity"] != DBNull.Value ? Convert.ToInt32(dr["ExceedQuantity"]) : 0,
                                 CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                 CusBusinessName = dr["CusBusinessName"].ToString(),
                                 CusSalesPerson = dr["CusSalesPerson"].ToString(),
                                 CusInstaller = dr["CusInstaller"].ToString(),
                                 RMRAmount = dr["RMRAmount"] != DBNull.Value ? Convert.ToDouble(dr["RMRAmount"]) : 0.0,
                                 PrevTicketType = dr["PrevTicketType"].ToString(),
                                 PrevAppointmentDate = dr["PrevAppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PrevAppointmentDate"]) : new DateTime(),
                                 PrevTechnician = dr["PrevTechnician"].ToString(),
                                 CusSalesLoc = dr["CusSalesLoc"].ToString(),
                                 LeadSource = dr["LeadSource"].ToString(),
                                 BookingId = dr["BookingId"].ToString(),
                                 BookingInvoiceAmount = dr["BookingInvoiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["BookingInvoiceAmount"]) : 0.0,
                             }).ToList();
            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public TicketListModel GetAllAppointmentDateReport(TicketFilter Filters, FilterReportModel filter)
        {
            TicketListModel Model = new TicketListModel();

            DataSet ds = _EmployeeDataAccess.GetAllAppointmentDateReport(Filters, filter);
            Model.Tickets = (from DataRow dr in ds.Tables[1].Rows
                             select new Ticket()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 CompanyId = (Guid)dr["CompanyId"],
                                 CustomerName = dr["CustomerName"].ToString(),
                                 CustomerNo = dr["CustomerNo"].ToString(),
                                 SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                 InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                 CustomerId = (Guid)dr["CustomerId"],
                                 TicketId = (Guid)dr["TicketId"],
                                 CreatedBy = (Guid)dr["TicketId"],
                                 Status = dr["Status"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 AssignedTo = dr["AssignedTo"].ToString().TrimEnd(' ', ','),
                                 Priority = dr["Priority"].ToString(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                 RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,
                                 ResignByVal = dr["ResignByVal"].ToString(),
                                 TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                 StatusVal = dr["StatusVal"].ToString(),
                                 PriorityVal = dr["PriorityVal"].ToString(),
                                 CreatedByVal = dr["CreatedByVal"].ToString(),
                                 CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                 CusBusinessName = dr["CusBusinessName"].ToString(),
                                 CusSalesPerson = dr["CusSalesPerson"].ToString(),
                                 CusInstaller = dr["CusInstaller"].ToString(),
                                 RMRAmount = dr["RMRAmount"] != DBNull.Value ? Convert.ToDouble(dr["RMRAmount"]) : 0.0,
                                 PrevTicketType = dr["PrevTicketType"].ToString(),
                                 PrevAppointmentDate = dr["PrevAppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PrevAppointmentDate"]) : new DateTime(),
                                 PrevTechnician = dr["PrevTechnician"].ToString(),
                                 CusSalesLoc = dr["CusSalesLoc"].ToString(),
                                 LeadSource = dr["LeadSource"].ToString(),
                                 BookingId = dr["BookingId"].ToString(),
                                 BookingInvoiceAmount = dr["BookingInvoiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["BookingInvoiceAmount"]) : 0.0,
                             }).ToList();
            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
        }
        public TicketListModel GetAllDateReferenceReport(TicketFilter Filters, FilterReportModel filter)
        {
            TicketListModel Model = new TicketListModel();

            DataSet ds = _EmployeeDataAccess.GetAllDateReferenceReport(Filters, filter);
            Model.Tickets = (from DataRow dr in ds.Tables[1].Rows
                             select new Ticket()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 CompanyId = (Guid)dr["CompanyId"],
                                 CustomerName = dr["CustomerName"].ToString(),
                                 SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                 InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                 CustomerId = (Guid)dr["CustomerId"],
                                 TicketId = (Guid)dr["TicketId"],
                                 CreatedBy = (Guid)dr["TicketId"],
                                 Status = dr["Status"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 AssignedTo = dr["AssignedTo"].ToString().TrimEnd(' ', ','),
                                 Priority = dr["Priority"].ToString(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                 RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,
                                 ResignByVal = dr["ResignByVal"].ToString(),
                                 TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                 StatusVal = dr["StatusVal"].ToString(),
                                 PriorityVal = dr["PriorityVal"].ToString(),
                                 CreatedByVal = dr["CreatedByVal"].ToString(),
                                 CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                 CusBusinessName = dr["CusBusinessName"].ToString(),
                                 CusSalesPerson = dr["CusSalesPerson"].ToString(),
                                 CusInstaller = dr["CusInstaller"].ToString(),
                                 RMRAmount = dr["RMRAmount"] != DBNull.Value ? Convert.ToDouble(dr["RMRAmount"]) : 0.0,
                                 PrevTicketType = dr["PrevTicketType"].ToString(),
                                 PrevAppointmentDate = dr["PrevAppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PrevAppointmentDate"]) : new DateTime(),
                                 PrevTechnician = dr["PrevTechnician"].ToString(),
                                 CusSalesLoc = dr["CusSalesLoc"].ToString(),
                                 LeadSource = dr["LeadSource"].ToString(),
                                 BookingId = dr["BookingId"].ToString(),
                                 BookingInvoiceAmount = dr["BookingInvoiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["BookingInvoiceAmount"]) : 0.0,
                             }).ToList();
            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
        }
        public TicketListModel GetAllGoBackReport(TicketFilter Filters, FilterReportModel filter)
        {
            TicketListModel Model = new TicketListModel();

            DataSet ds = _EmployeeDataAccess.GetAllGoBackReport(Filters, filter);
            if (ds != null)
            {
                Model.Tickets = (from DataRow dr in ds.Tables[1].Rows
                                 select new Ticket()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CompanyId = (Guid)dr["CompanyId"],
                                     CustomerName = dr["CustomerName"].ToString(),
                                     SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                     InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                     CustomerId = (Guid)dr["CustomerId"],
                                     TicketId = (Guid)dr["TicketId"],
                                     CreatedBy = (Guid)dr["TicketId"],
                                     Status = dr["Status"].ToString(),
                                     TicketType = dr["TicketType"].ToString(),
                                     AssignedTo = dr["AssignedTo"].ToString().TrimEnd(' ', ','),
                                     Priority = dr["Priority"].ToString(),
                                     CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                     AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                     RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,

                                     TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                     StatusVal = dr["StatusVal"].ToString(),
                                     PriorityVal = dr["PriorityVal"].ToString(),
                                     CreatedByVal = dr["CreatedByVal"].ToString(),
                                     CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                     CusBusinessName = dr["CusBusinessName"].ToString(),
                                     CusSalesPerson = dr["CusSalesPerson"].ToString(),
                                     CusInstaller = dr["CusInstaller"].ToString(),
                                     RMRAmount = dr["RMRAmount"] != DBNull.Value ? Convert.ToDouble(dr["RMRAmount"]) : 0.0,
                                     PrevTicketType = dr["PrevTicketType"].ToString(),
                                     PrevAppointmentDate = dr["PrevAppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PrevAppointmentDate"]) : new DateTime(),
                                     PrevTechnician = dr["PrevTechnician"].ToString(),
                                     CusSalesLoc = dr["CusSalesLoc"].ToString(),
                                     LeadSource = dr["LeadSource"].ToString(),
                                     BookingId = dr["BookingId"].ToString(),
                                     BookingInvoiceAmount = dr["BookingInvoiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["BookingInvoiceAmount"]) : 0.0,
                                     PrevTicketId = dr["PrevTicketId"] != DBNull.Value ? Convert.ToInt32(dr["PrevTicketId"]) : 0,

                                 }).ToList();
                Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
                Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
                Model.PageNo = Filters.PageNo.Value;
                Model.PageSize = Filters.PageSize;
                Model.Searchtext = Filters.SearchText;
                Model.TicketType = Filters.TicketType;
                Model.TicketStatus = Filters.TicketStatus;
                Model.Assigned = Filters.Assigned;
                Model.StartDate = Filters.StartDate;
                Model.EndDate = Filters.EndDate;
            }
            else
            {
                Model.Tickets = new List<Ticket>();
            }
            return Model;
        }
        public DataTable AppointmentDateReportListForDownload( TicketFilter Filters, FilterReportModel filter)
        {
            DataSet ds = _EmployeeDataAccess.GetAllAppointmentDateReport(Filters, filter);

            return ds.Tables[0];
        }
        public DataTable DateReferenceReportListForDownload(TicketFilter Filters, FilterReportModel filter)
        {
            DataSet ds = _EmployeeDataAccess.GetAllDateReferenceReport(Filters, filter);

            return ds.Tables[0];
        }
        public DataTable GoBackReportListForDownload(TicketFilter Filters, FilterReportModel filter)
        {
            DataSet ds = _EmployeeDataAccess.GetAllGoBackReport(Filters, filter);

            return ds.Tables[0];
        }
        public EmployeeListModel GetAllTechnicianReportByFilter(EmployeeFilter Filters, FilterReportModel filter)
        {
            EmployeeListModel Model = new EmployeeListModel();
            DataSet ds = _EmployeeDataAccess.GetTechnicianListByFilter(Filters, filter);
            Model.Employees = (from DataRow dr in ds.Tables[0].Rows
                             select new Employee()
                             {
                                // Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               //  CompanyId = (Guid)dr["CompanyId"],
                                  EMPName = dr["Name"].ToString(),
                                 City = dr["City"].ToString(),
                                 State = dr["State"].ToString(),
                                 InstallationsScheduled= dr["InstallationsScheduled"] != DBNull.Value ? Convert.ToInt32(dr["InstallationsScheduled"]) : 0,
                                 Installationscomplete = dr["Installationscomplete"] != DBNull.Value ? Convert.ToInt32(dr["Installationscomplete"]) : 0,
                                 servicesscheduled = dr["servicesscheduled"] != DBNull.Value ? Convert.ToInt32(dr["servicesscheduled"]) : 0,
                                 servicescomplete = dr["servicescomplete"] != DBNull.Value ? Convert.ToInt32(dr["servicescomplete"]) : 0,
                                 //Status = dr["Status"].ToString(),
                                
                                 //BookingInvoiceAmount = dr["BookingInvoiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["BookingInvoiceAmount"]) : 0.0,
                             }).ToList();

            Model.TotalCount = ds.Tables[2].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalCount"]) : 0;

            Model.TotalInstallationsScheduled = ds.Tables[1].Rows[0]["TotalInstallationsScheduled"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalInstallationsScheduled"]) : 0;
            Model.TotalInstallationscomplete = ds.Tables[1].Rows[0]["TotalInstallationscomplete"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalInstallationscomplete"]) : 0;
            Model.Totalservicesscheduled = ds.Tables[1].Rows[0]["Totalservicesscheduled"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Totalservicesscheduled"]) : 0;
            Model.Totalservicescomplete = ds.Tables[1].Rows[0]["Totalservicescomplete"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Totalservicescomplete"]) : 0;


            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public TicketListModel GetAllInstallationTicketReportByFilter(TicketFilter Filters, FilterReportModel filter)
        {
            TicketListModel Model = new TicketListModel();
            DataSet ds = _EmployeeDataAccess.GetInstallationTicketListByFilter(Filters, filter);
            Model.Tickets = (from DataRow dr in ds.Tables[0].Rows
                             select new Ticket()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 CompanyId = (Guid)dr["CompanyId"],
                                 CustomerName = dr["CustomerName"].ToString(),
                                 //SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                 InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                 CustomerId = (Guid)dr["CustomerId"],
                                 TicketId = (Guid)dr["TicketId"],
                                 CreatedBy = (Guid)dr["TicketId"],
                                 Status = dr["TicketStatus"].ToString(),
                                 OwnerShip = dr["OwnerShip"].ToString(),
                                 CustomerQA1 = dr["QA1"].ToString(),
                                 CustomerQA2 = dr["QA2"].ToString(),
                                 RegisteredVal = dr["Registered"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 AssignedTo = dr["AssignedTo"].ToString().TrimEnd(' ', ','),
                                 //AdditionalMembers = dr["AdditionalMembers"].ToString().TrimEnd(' ', ','),
                                 //Priority = dr["Priority"].ToString(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 CompletedDate = dr["CompletedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletedDate"]) : new DateTime(),
                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                 CustomerAgreementSignature= dr["CustomerSignatureDate"] != DBNull.Value ? Convert.ToDateTime(dr["CustomerSignatureDate"]) : new DateTime(),
                                 AccountOnlineDate = dr["AccountOnlineDate"] != DBNull.Value ? Convert.ToDateTime(dr["AccountOnlineDate"]) : new DateTime(),
                                 TechOnsiteDate=dr["TechOnsiteDate"] != DBNull.Value ? Convert.ToDateTime(dr["TechOnsiteDate"]) : new DateTime(),
                                 WhoPlacedOnline=dr["WhoPlaced"].ToString(),
                                 //AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                 //RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,
                                 //AppointmentStartTimeVal = dr["AppointmentStartTimeVal"].ToString(),
                                 //AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                 //AppointmentEndTime = dr["AppointmentStartTime"].ToString(),
                                 //AppointmentEndTimeVal = dr["AppointmentEndTimeVal"].ToString(),
                                 //TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                 //StatusVal = dr["StatusVal"].ToString(),
                                 //PriorityVal = dr["PriorityVal"].ToString(),
                                 CreatedByVal = dr["CreatedByVal"].ToString(),
                                 LeadSource = dr["LeadSourceVal"].ToString(),

                                 //ExceedQuantity = dr["ExceedQuantity"] != DBNull.Value ? Convert.ToInt32(dr["ExceedQuantity"]) : 0,
                                 CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                 CusBusinessName = dr["CusBusinessName"].ToString(),
                                 CusSalesPerson = dr["CusSalesPerson"].ToString(),
                                 CusInstaller = dr["CusInstaller"].ToString(),
                                 //RMRAmount = dr["RMRAmount"] != DBNull.Value ? Convert.ToDouble(dr["RMRAmount"]) : 0.0,
                                 //PrevTicketType = dr["PrevTicketType"].ToString(),
                                 //PrevAppointmentDate = dr["PrevAppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PrevAppointmentDate"]) : new DateTime(),
                                 //PrevTechnician = dr["PrevTechnician"].ToString(),
                                 //CusSalesLoc = dr["CusSalesLoc"].ToString(),
                                 //LeadSource = dr["LeadSource"].ToString(),
                                 //BookingId = dr["BookingId"].ToString(),
                                 //BookingInvoiceAmount = dr["BookingInvoiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["BookingInvoiceAmount"]) : 0.0,
                                 TotalCollectedAmount = dr["TotalCollectedAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalCollectedAmount"]) : 0.0,
                             }).ToList();
            Model.TotalAmountByPage = ds.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;

            Model.TotalCount = ds.Tables[2].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public CSRActivityModel GetAllCSRActivityReportByFilter(TicketFilter Filters, FilterReportModel filter)
        {
            CSRActivityModel Model = new CSRActivityModel();
            DataSet ds = _EmployeeDataAccess.GetCSRReportListByFilter(Filters, filter);
            Model.CSRActivityList = (from DataRow dr in ds.Tables[0].Rows
                             select new CSRActivity()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 Name = dr["Name"].ToString(),
                                 CancelledAccount = dr["CancelledAccount"] != DBNull.Value ? Convert.ToInt32(dr["CancelledAccount"]) : 0,
                                 CreatedAccount = dr["CreatedAccount"] != DBNull.Value ? Convert.ToInt32(dr["CreatedAccount"]) : 0,
                                 InstallScheduled = dr["InstallScheduled"] != DBNull.Value ? Convert.ToInt32(dr["InstallScheduled"]) : 0,
                                 ContractSent = dr["ContractSent"] != DBNull.Value ? Convert.ToInt32(dr["ContractSent"]) : 0,
                                 AccountPlaced = dr["AccountPlaced"] != DBNull.Value ? Convert.ToInt32(dr["AccountPlaced"]) : 0,
                                 ServiceScheduled = dr["ServicesScheduled"] != DBNull.Value ? Convert.ToInt32(dr["ServicesScheduled"]) : 0,
                             }).ToList();
            Model.TotalCancelledAccount = ds.Tables[1].Rows[0]["TotalCancelledAccount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCancelledAccount"]) : 0;
            Model.TotalCreatedAccount = ds.Tables[1].Rows[0]["TotalCreatedAccount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCreatedAccount"]) : 0;
            Model.TotalContractSent = ds.Tables[1].Rows[0]["TotalContractSent"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalContractSent"]) : 0;
            Model.TotalAccountPlaced = ds.Tables[1].Rows[0]["TotalAccountPlaced"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalAccountPlaced"]) : 0;
            Model.TotalInstallScheduled = ds.Tables[1].Rows[0]["TotalInstallScheduled"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalInstallScheduled"]) : 0;
            Model.TotalServicesScheduled = ds.Tables[1].Rows[0]["TotalServicesScheduled"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalServicesScheduled"]) : 0;


            Model.TotalCount = ds.Tables[2].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalCount"]) : 0;
      
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public List<SelectListItem> GetAllUserListForMultiSelect(Guid com)
        {
            List<Employee> ds = _EmployeeDataAccess.GetAllEmployee(com);
            List<SelectListItem> userList = new List<SelectListItem>();
            if (ds != null && ds.Count() > 0)
            {
                userList.AddRange(ds.OrderByDescending(x => x.IsCurrentEmployee).ThenBy(x => x.FirstName + " " + x.LastName).Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString()
                }).ToList());
            }

            return userList;
        }
        public AccountabilityReportModel GetAllEmployeeWithArticle(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            DataSet ds = _EmployeeDataAccess.GetAllEmployeeWithArticle(CompanyId, start, end, searchtext, pageno, pagesize, order);
            AccountabilityReportModel model = new AccountabilityReportModel();
            if (ds != null)
            {
                model.List = (from DataRow dr in ds.Tables[0].Rows
                              select new AccountabilityReport()
                              {
                                  Name = dr["Name"].ToString(),
                                  UserId = (Guid)dr["UserId"],
                                  Artical = dr["Artical"] != DBNull.Value ? Convert.ToInt32(dr["Artical"]) : 0,
                                  TotalUnread = dr["TotalUnread"] != DBNull.Value ? Convert.ToInt32(dr["TotalUnread"]) : 0,
                                  TotalCompleted = dr["TotalCompleted"] != DBNull.Value ? Convert.ToInt32(dr["TotalCompleted"]) : 0,
                              }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]) : 0;
            }
            return model;
        } 
        public DataTable GetAllEmployeeWithArticleForDownload(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            return _EmployeeDataAccess.GetAllEmployeeWithArticleForDownload(CompanyId, start, end, searchtext, pageno, pagesize, order);
        }
        public AccountabilityHistoryReportModel GetAccountabilityHistoryList(DateTime start, DateTime end, Guid UserId, string Order)
        {
            DataSet ds = _EmployeeDataAccess.GetAccountabilityHistoryList(start, end, UserId, Order);
            AccountabilityHistoryReportModel model = new AccountabilityHistoryReportModel();
            if (ds != null)
            {
                model.List = (from DataRow dr in ds.Tables[0].Rows
                              select new AccountabilityHistory()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  Name = dr["Title"].ToString(),
                                  AssignedDate = dr["AssignedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AssignedDate"]).UTCToClientTime() : new DateTime(),
                                  EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]).UTCToClientTime() : new DateTime(),
                                  AssignedBy = dr["AssignedBy"].ToString(),
                              }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]) : 0;
            }
            return model;
        }
        public AccountabilityHistoryReportModel GetAccountabilityHistoryUnreadList(DateTime start, DateTime end, Guid UserId, string Order)
        {
            DataSet ds = _EmployeeDataAccess.GetAccountabilityHistoryUnreadList(start, end, UserId, Order);
            AccountabilityHistoryReportModel model = new AccountabilityHistoryReportModel();
            if (ds != null)
            {
                model.List = (from DataRow dr in ds.Tables[0].Rows
                              select new AccountabilityHistory()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  Name = dr["Title"].ToString(),
                                  AssignedDate = dr["AssignedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AssignedDate"]).UTCToClientTime() : new DateTime(),
                                  EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]).UTCToClientTime() : new DateTime(),
                                  AssignedBy = dr["AssignedBy"].ToString(),
                              }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]) : 0;
            }
            return model;
        }
        public AccountabilityHistoryReportModel GetAccountabilityHistoryCompletedList(DateTime start, DateTime end, Guid UserId, string Order)
        {
            DataSet ds = _EmployeeDataAccess.GetAccountabilityHistoryCompletedList(start, end, UserId, Order);
            AccountabilityHistoryReportModel model = new AccountabilityHistoryReportModel();
            if (ds != null)
            {
                model.List = (from DataRow dr in ds.Tables[0].Rows
                              select new AccountabilityHistory()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  Name = dr["Title"].ToString(),
                                  AssignedDate = dr["AssignedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AssignedDate"]).UTCToClientTime() : new DateTime(),
                                  EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]).UTCToClientTime() : new DateTime(),
                                  AssignedBy = dr["AssignedBy"].ToString(),
                              }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]) : 0;
            }
            return model;
        }
        public ServiceTrackerModel GetAllServiceTrackerByFilter(TicketFilter Filters, FilterReportModel filter,string Start,string End)
        {
            ServiceTrackerModel Model = new ServiceTrackerModel();
            DataSet ds = _EmployeeDataAccess.GetServiceTrackerReportByFilter(Filters, filter,Start,End);
            if (ds != null)
            {
                Model.SeviceTrackerList = (from DataRow dr in ds.Tables[0].Rows
                                           select new SeviceTracker()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               CustomerId = (Guid)dr["CustomerId"],
                                               CustomerName = dr["CustomerName"].ToString(),
                                               TicketId = dr["TicketId"] != DBNull.Value ? Convert.ToInt32(dr["TicketId"]) : 0,
                                               InstallerTechnician = dr["InstallerTechnician"].ToString(),
                                               ServiceTechnician = dr["ServiceTechnician"].ToString(),
                                               ServiceType = dr["ServiceType"].ToString(),
                                               Reason = dr["Reason"].ToString(),
                                               ScheduledDate = dr["ScheduledServiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["ScheduledServiceDate"]) : new DateTime(),
                                               TechOnsiteDate = dr["TechOnsiteDate"] != DBNull.Value ? Convert.ToDateTime(dr["TechOnsiteDate"]) : new DateTime()

                                           }).ToList();
                Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            }
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public CSRActivityModel GetAllServiceReportByFilter(TicketFilter Filters, FilterReportModel filter)
        {
            CSRActivityModel Model = new CSRActivityModel();
            DataSet ds = _EmployeeDataAccess.GetCSRReportListByFilter(Filters, filter);
            Model.CSRActivityList = (from DataRow dr in ds.Tables[0].Rows
                                     select new CSRActivity()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         Name = dr["Name"].ToString(),
                                         CancelledAccount = dr["CancelledAccount"] != DBNull.Value ? Convert.ToInt32(dr["CancelledAccount"]) : 0,
                                         CreatedAccount = dr["CreatedAccount"] != DBNull.Value ? Convert.ToInt32(dr["CreatedAccount"]) : 0,
                                         InstallScheduled = dr["InstallScheduled"] != DBNull.Value ? Convert.ToInt32(dr["InstallScheduled"]) : 0,
                                         ContractSent = dr["ContractSent"] != DBNull.Value ? Convert.ToInt32(dr["ContractSent"]) : 0,
                                         AccountPlaced = dr["AccountPlaced"] != DBNull.Value ? Convert.ToInt32(dr["AccountPlaced"]) : 0,
                                         ServiceScheduled = dr["ServicesScheduled"] != DBNull.Value ? Convert.ToInt32(dr["ServicesScheduled"]) : 0,
                                     }).ToList();
            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;

            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public TicketListModel GetTicketListInstallByFilter(TicketFilter Filters)
        {
            TicketListModel Model = new TicketListModel();
            //DataSet ds = _EmployeeDataAccess.GetTicketListInstallByFilter(Filters);
            DataSet ds = _EmployeeDataAccess.GetIndividualInstalledEquipment(Filters);
            Model.Tickets = (from DataRow dr in ds.Tables[1].Rows
                             select new Ticket()
                             {
                                 AppointmentEquipmentId = dr["AppointmentEquipmentId"] != DBNull.Value ? Convert.ToInt32(dr["AppointmentEquipmentId"]) : 0,
                                 Category = dr["Category"].ToString(),
                                 Status=dr["Status"].ToString(),
                                 Manufacturer = dr["Manufacturer"].ToString(),
                                 Description = dr["Description"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 EmpUser = dr["EmpUser"].ToString(),
                                 Id = dr["TicketId"] != DBNull.Value ? Convert.ToInt32(dr["TicketId"]) : 0,
                                 CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                 CustomerName = dr["CustomerName"].ToString(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 SKU = dr["SKU"].ToString(),
                                 TotalPoint=dr["TotalPoint"]!=DBNull.Value?Convert.ToDouble(dr["TotalPoint"]):0.0,
                                 RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,
                                 AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                 IsClosed = dr["IsClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsClosed"]) : false,
                                 CompanyCost = dr["CompanyCost"] != DBNull.Value ? Convert.ToDouble(dr["CompanyCost"]) : 0,
                                 CustomerCost = dr["CustomerCost"] != DBNull.Value ? Convert.ToDouble(dr["CustomerCost"]) : 0,
                                 Quantity = dr["Qty"] != DBNull.Value ? Convert.ToInt32(dr["Qty"]) : 0,
                                 InstalledQuantity = dr["InstalledEquipment"] != DBNull.Value ? Convert.ToInt32(dr["InstalledEquipment"]) : 0,
                                 SoldBy = dr["Soldby"].ToString()
                             }).ToList();
            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalQuantity = ds.Tables[2].Rows[0]["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalQuantity"]) : 0;
            Model.TotalCustomerCost = ds.Tables[2].Rows[0]["TotalCustomerCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalCustomerCost"]) : 0.0;
            Model.TotalCompanyCost = ds.Tables[2].Rows[0]["TotalCompanyCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalCompanyCost"]) : 0.0;
            Model.PointSum = ds.Tables[2].Rows[0]["PointSum"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["PointSum"]) : 0.0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public TicketListModel GetTicketListAllByFilter(TicketFilter Filters)
        {
            TicketListModel Model = new TicketListModel();
            DataSet ds = _EmployeeDataAccess.GetTicketListAllByFilter(Filters);
            Model.Tickets = (from DataRow dr in ds.Tables[1].Rows
                             select new Ticket()
                             {
                                 AppointmentEquipmentId = dr["AppointmentEquipmentId"] != DBNull.Value ? Convert.ToInt32(dr["AppointmentEquipmentId"]) : 0,
                                 Category = dr["Category"].ToString(),
                                 Status = dr["Status"].ToString(),
                                 Manufacturer = dr["Manufacturer"].ToString(),
                                 Description = dr["Description"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 EmpUser = dr["EmpUser"].ToString(),
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                 CustomerName = dr["CustomerName"].ToString(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 SKU = dr["SKU"].ToString(),
                                 TotalPoint = dr["TotalPoint"] != DBNull.Value ? Convert.ToDouble(dr["TotalPoint"]) : 0.0,
                                 RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,
                                 AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                 IsClosed = dr["IsClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsClosed"]) : false,
                                 CompanyCost = dr["CompanyCost"] != DBNull.Value ? Convert.ToDouble(dr["CompanyCost"]) : 0,
                                 CustomerCost = dr["CustomerCost"] != DBNull.Value ? Convert.ToDouble(dr["CustomerCost"]) : 0,
                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                 InstalledQuantity = dr["InstalledEquipment"] != DBNull.Value ? Convert.ToInt32(dr["InstalledEquipment"]) : 0,
                             }).ToList();
            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalInstalledQuantity = ds.Tables[2].Rows[0]["TotalInstalledQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalInstalledQuantity"]) : 0;
            Model.TotalQuantity = ds.Tables[2].Rows[0]["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalQuantity"]) : 0;
            Model.TotalPoint = ds.Tables[2].Rows[0]["TotalPoint"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalPoint"]) : 0.0;
            Model.TotalCompanyCost = ds.Tables[2].Rows[0]["TotalCompanyCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalCompanyCost"]) : 0.0;
            Model.TotalCustomerCost = ds.Tables[2].Rows[0]["TotalCustomerCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalCustomerCost"]) : 0.0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        //public TicketListModel GetTechnicianlist(TicketFilter TicketFilters)
        //{
        //    TicketListModel Model = new TicketListModel();
        //    DataSet ds = _EmployeeDataAccess.GetTechnicianList(TicketFilters);
        //    Model.Tickets = (from DataRow dr in ds.Tables[0].Rows
        //                     select new Ticket()
        //                     {

        //                         EmpUser = dr["EmpUser"].ToString(),

        //                     }).ToList();

        //    return Model;
        //    // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        //}
        public List<Ticket> GetTechnicianlist(TicketFilter TicketFilters)
        {
            DataTable dt = _EmployeeDataAccess.GetTechnicianList(TicketFilters);

            //DataSet dsResult = _InventoryDataAccess.GetPurchaseOrderStatus();
            List<Ticket> POWList = new List<Ticket>();
            POWList = (from DataRow dr in dt.Rows
                       select new Ticket()
                       {
                           EmpUser = dr["EmpUser"].ToString()

                       }).ToList();
            return POWList;
        }
        public EmpTechCommisionReport GetAllTechCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize,bool IsPaid,Guid UserId,string SearchText,string TechPersonList)
        {
            DataSet ds = _EmployeeDataAccess.GetAllTechReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid,UserId, SearchText, TechPersonList);
            List<TechCommission> buildList = new List<TechCommission>();
            buildList = (from DataRow dr in ds.Tables[1].Rows
                         select new TechCommission()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerName = dr["CustomerName"].ToString(),
                             TicketIdValue = dr["ticketIdValue"] != DBNull.Value ? Convert.ToInt32(dr["ticketIdValue"]) : 0,
                             TicketId = (Guid)dr["TicketId"],
                             Technician = dr["Technician"].ToString(),
                             CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                             BaseRMR = dr["BaseRMR"] != DBNull.Value ? Convert.ToDouble(dr["BaseRMR"]) : 0,
                             BaseRMRCommission = dr["BaseRMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["BaseRMRCommission"]) : 0,
                             AddedRMR = dr["AddedRMR"] != DBNull.Value ? Convert.ToDouble(dr["AddedRMR"]) : 0,
                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             AddedRMRCommission = dr["AddedRMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["AddedRMRCommission"]) : 0,
                             TotalCommission = dr["TotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCommission"]) : 0,
                             IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                             CreatedBy = (Guid)dr["CreatedBy"],
                             UserId = (Guid)dr["UserId"],
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                             Batch = dr["Batch"].ToString(),
                             CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,
                             Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                             TotalPoint = dr["TotalPoint"] != DBNull.Value ? Convert.ToDouble(dr["TotalPoint"]) : 0,
                             OriginalPoint = dr["OriginalPoint"] != DBNull.Value ? Convert.ToDouble(dr["OriginalPoint"]) : 0
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[0].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TotalPayrollSum TotalTechCount = new TotalPayrollSum();
            TotalTechCount = (from DataRow dr in ds.Tables[2].Rows
                               select new TotalPayrollSum()
                               {
                                   TotalAdjustment = dr["TotalAdjustment"] != DBNull.Value ? Convert.ToDouble(dr["TotalAdjustment"]) : 0.0,
                                   TotalCommission = dr["SumCommission"] != DBNull.Value ? Convert.ToDouble(dr["SumCommission"]) : 0.0,
                                   TotalCommissionablePoints = dr["SumCommissionablePoint"] != DBNull.Value ? Convert.ToDouble(dr["SumCommissionablePoint"]) : 0.0,
                                   TotalPoint = dr["SumPoint"] != DBNull.Value ? Convert.ToDouble(dr["SumPoint"]) : 0.0,
                                   TotalUnpaidBalance = dr["SumUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["SumUnpaid"]) : 0.0,
                               }).FirstOrDefault();
            EmpTechCommisionReport EmpPayrollFilter = new EmpTechCommisionReport();
            EmpPayrollFilter.PayrollReportList = buildList;
            EmpPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            EmpPayrollFilter.TotalTechCount = TotalTechCount;
            return EmpPayrollFilter;
        }

        public DataTable GetDownLoadAllTechCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string TechPersonList)
        {
            DataSet ds = _EmployeeDataAccess.GetDownLoadAllTechReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, TechPersonList);
            //List<TechCommission> buildList = new List<TechCommission>();
            DataTable buildList = ds.Tables[0];
                        

           
            return buildList;
        }


        public DataTable GetExcelPayrollReport(Guid comid, DateTime StartDate, DateTime EndDate)
        {
            return _EmployeeDataAccess.GetAllPayrollReport(comid, StartDate, EndDate);
        }

        public Employee GetEmployeeByUsername(string oldUsername)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserName = '{0}'", oldUsername)).FirstOrDefault();
        }
        public Employee GetEmployeeByUsername(string oldUsername, bool ResetDb)
        {
            // 98%
            return _EmployeeDataAccess.GetByQuery(string.Format("UserName = '{0}'", oldUsername), ResetDb).FirstOrDefault();
        }

        public Employee GetEmployeeByUsernameAndCompanyId(string oldUsername, bool ResetDb, Guid comid)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserName = '{0}' and CompanyId = '{1}'", oldUsername, comid), ResetDb).FirstOrDefault();
        }

        public List<Employee> GetAllSupervisors()
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("IsSupervisor = 1  and IsDeleted = 'false'"));
        }

        #region EmployeeCommission
        public bool InsertEmployeeComission(EmployeeCommission ec)
        {
            return _EmployeeCommissionDataAccess.Insert(ec)>0;
        }
        public bool UpdateEmployeeComission(EmployeeCommission ec)
        {
            return _EmployeeCommissionDataAccess.Update(ec)>0;
        }
        public EmployeeCommission GetEmployeeComissionByCustomerId(Guid CustomerId)
        {
            string query = string.Format("CustomerId='{0}'", CustomerId);
            return _EmployeeCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public EmployeeCommission GetEmployeeComissionByCustomerIdUserId(Guid CustomerId,Guid UserId)
        {
            string query = string.Format("CustomerId='{0}' and UserId='{1}'", CustomerId, UserId);
            return _EmployeeCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        #endregion

        public SalesReportModel GetSalesReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus,string Order)
        {
            DataSet ds = _EmployeeDataAccess.GetSalesReportByCompanyId(companyid, pageno, pagesize, startdate, enddate, searchtext, invostatus, Order);
            List<Customer> buildList = new List<Customer>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new Customer()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerId = (Guid)dr["CustomerId"],
                             FirstName = dr["FirstName"].ToString(),
                             LastName = dr["LastName"].ToString(),
                             DisplayName=dr["DisplayName"].ToString(),
                           
                             TotalSales = dr["TotalSales"] != DBNull.Value ? Convert.ToDouble(dr["TotalSales"]) : 0.0,
                             TotalRMR = dr["TotalRMR"] != DBNull.Value ? Convert.ToDouble(dr["TotalRMR"]) : 0.0,
                             TotalTax = dr["TotalTax"] != DBNull.Value ? Convert.ToDouble(dr["TotalTax"]) : 0.0,

                             SalesAfterTax = dr["SalesAfterTax"] != DBNull.Value ? Convert.ToDouble(dr["SalesAfterTax"]) : 0.0,
                             TotalPaid = dr["TotalPaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalPaid"]) : 0.0,
                             TotalUnpaid = dr["TotalUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalUnpaid"]) : 0.0,
                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                 select new SalesReportCountModel()
                                 {
                                     TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();
            TotalSalesAmountModel TotalSalesAmountModel = new TotalSalesAmountModel();
            TotalSalesAmountModel = (from DataRow dr in ds.Tables[2].Rows
                                     select new TotalSalesAmountModel()
                                     {
                                         TotalSalesAmount = dr["TotalSalesAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalSalesAmount"]) : 0.0,
                                         AveSaleswoTax = dr["AveSaleswoTax"] != DBNull.Value ? Convert.ToDouble(dr["AveSaleswoTax"]) : 0.0,
                                         TotalTax = dr["SumTotalTax"] != DBNull.Value ? Convert.ToDouble(dr["SumTotalTax"]) : 0.0,
                                         SalesAfterTax = dr["SumSalesAfterTax"] != DBNull.Value ? Convert.ToDouble(dr["SumSalesAfterTax"]) : 0.0,
                                         AveSaleswTax = dr["AveSalesWTax"] != DBNull.Value ? Convert.ToDouble(dr["AveSalesWTax"]) : 0.0,
                                         TotalPaid = dr["SumTotalPaid"] != DBNull.Value ? Convert.ToDouble(dr["SumTotalPaid"]) : 0.0,
                                         TotalUnpaid = dr["SumTotalUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["SumTotalUnpaid"]) : 0.0,


                                     }).FirstOrDefault();
            SalesReportModel SalesReportModel = new SalesReportModel();
            SalesReportModel.ListCustomer = buildList;
            SalesReportModel.SalesReportCountModel = SalesReportCountModel;
            SalesReportModel.TotalSalesAmountModel = TotalSalesAmountModel;
            return SalesReportModel;
        }

        public InvoiceReportModel GetInvoiceReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus, string order, FilterReportModel filter)
        {
            DataSet ds = _EmployeeDataAccess.GetInvoiceReportByCompanyId(companyid, pageno, pagesize, startdate, enddate, searchtext, invostatus, order, filter);
            List<Invoice> buildList = new List<Invoice>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new Invoice()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                             CustomerId = (Guid)dr["CustomerId"],
                             CustomerName = dr["DisplayName"].ToString() ,
                             InvoiceId= dr["InvoiceId"].ToString(),
                             InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                             DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                             TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0.0,
                             AmountReceived = dr["PaidBalance"] != DBNull.Value ? Convert.ToDouble(dr["PaidBalance"]) : 0.0,
                             Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0,
                             Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0.0,
                             //SalesAfterTax = dr["SalesAfterTax"] != DBNull.Value ? Convert.ToDouble(dr["SalesAfterTax"]) : 0.0,
                             //TotalPaid = dr["TotalPaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalPaid"]) : 0.0,
                             //TotalUnpaid = dr["TotalUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalUnpaid"]) : 0.0,
                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
            TotalSalesAmountModel TotalSalesAmountModel = new TotalSalesAmountModel();
            TotalSalesAmountModel = (from DataRow dr in ds.Tables[2].Rows
                                     select new TotalSalesAmountModel()
                                     {
                                         TotalSalesAmount = dr["TotalSalesAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalSalesAmount"]) : 0.0,
                                         TotalDueAmount = dr["TotalDueAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalDueAmount"]) : 0.0,
                                         CustomerCount = dr["CustomerCount"] != DBNull.Value ? Convert.ToInt32(dr["CustomerCount"]) : 0,
                                         InvoiceCount = dr["InvoiceCount"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceCount"]) : 0,
                                         TotalAmount = dr["SumTotalAmt"] != DBNull.Value ? Convert.ToDouble(dr["SumTotalAmt"]) : 0.0,
                                         AveInvoiceAmount = dr["AveInvAmnt"] != DBNull.Value ? Convert.ToDouble(dr["AveInvAmnt"]) : 0.0,
                                         TotalTax = dr["SumTotalTax"] != DBNull.Value ? Convert.ToDouble(dr["SumTotalTax"]) : 0.0,

                                     }).FirstOrDefault();
            InvoiceReportModel SalesReportModel = new InvoiceReportModel();
            SalesReportModel.ListInvoice = buildList;
            SalesReportModel.InvoiceReportCountModel = SalesReportCountModel;
            SalesReportModel.TotalInvoiceAmountModel = TotalSalesAmountModel;
            return SalesReportModel;
        }

        public InvoiceReportModel GetEstimateReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus, string order, FilterReportModel filter)
        {
            DataSet ds = _EmployeeDataAccess.GetEstimateReportByCompanyId(companyid, pageno, pagesize, startdate, enddate, searchtext, invostatus, order, filter);
            List<Invoice> buildList = new List<Invoice>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new Invoice()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                             CustomerId = (Guid)dr["CustomerId"],
                             CustomerName = dr["DisplayName"].ToString(),
                             Status = dr["Status"].ToString(),
                             InvoiceId = dr["InvoiceId"].ToString(),
                             InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                             TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0.0,
                             AmountReceived = dr["PaidBalance"] != DBNull.Value ? Convert.ToDouble(dr["PaidBalance"]) : 0.0,
                             Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0,
                             Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0.0,
                             //SalesAfterTax = dr["SalesAfterTax"] != DBNull.Value ? Convert.ToDouble(dr["SalesAfterTax"]) : 0.0,
                             //TotalPaid = dr["TotalPaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalPaid"]) : 0.0,
                             //TotalUnpaid = dr["TotalUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalUnpaid"]) : 0.0,
                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
            TotalSalesAmountModel TotalSalesAmountModel = new TotalSalesAmountModel();
            TotalSalesAmountModel = (from DataRow dr in ds.Tables[2].Rows
                                     select new TotalSalesAmountModel()
                                     {
                                         TotalSalesAmount = dr["TotalSalesAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalSalesAmount"]) : 0.0,
                                         TotalDueAmount = dr["TotalDueAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalDueAmount"]) : 0.0
                                     }).FirstOrDefault();
            InvoiceReportModel SalesReportModel = new InvoiceReportModel();
            SalesReportModel.ListInvoice = buildList;
            SalesReportModel.InvoiceReportCountModel = SalesReportCountModel;
            SalesReportModel.TotalInvoiceAmountModel = TotalSalesAmountModel;
            return SalesReportModel;
        }



        public List<BookingReportModel> GetBookingSalesReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string source, string order)
        {
            
            DataSet ds = _EmployeeDataAccess.GetBookingSalesReportByCompanyId(companyid, pageno, pagesize, startdate, enddate, searchtext, source, order);
            List<BookingReportModel> buildList = new List<BookingReportModel>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new BookingReportModel()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                             CustomerId = (Guid)dr["CustomerId"],
                             FirstName = dr["FirstName"].ToString(),
                             //LastName = dr["LastName"].ToString(),
                             PrimaryPhone = dr["PrimaryPhone"].ToString(),
                             SecondaryPhone = dr["SecondaryPhone"].ToString(),
                             EmailAddress = dr["EmailAddress"].ToString(),
                             Address = dr["Address"].ToString(),
                             Street = dr["Street"].ToString(),
                             City = dr["City"].ToString(),
                             State = dr["State"].ToString(),
                             ZipCode = dr["ZipCode"].ToString(),
                             BookingIntId = dr["BookingIntId"] != DBNull.Value ? Convert.ToInt32(dr["BookingIntId"]) : 0,
                             BookingId = dr["BookingId"].ToString(),
                             BookingSource= dr["BookingSource"].ToString(),
                             TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,

                             Status = dr["Status"].ToString(),
                             Message = dr["Message"].ToString(),
                             RugType = dr["RugType"].ToString().Replace("@", "").ToString(),
                             //Length = dr["Length"] != DBNull.Value ? Convert.ToDouble(dr["Length"]) : 0.0,
                             //LengthInch = dr["LengthInch"] != DBNull.Value ? Convert.ToDouble(dr["LengthInch"]) : 0.0,
                             //Width = dr["Width"] != DBNull.Value ? Convert.ToDouble(dr["Width"]) : 0.0,
                             //WidthInch = dr["WidthInch"] != DBNull.Value ? Convert.ToDouble(dr["WidthInch"]) : 0.0,
                             //Radius = dr["Radius"] != DBNull.Value ? Convert.ToDouble(dr["Radius"]) : 0.0,
                             //RadiusInch = dr["RadiusInch"] != DBNull.Value ? Convert.ToDouble(dr["RadiusInch"]) : 0.0,

                             //TicketId = dr["TicketId"] != DBNull.Value ? Convert.ToInt32(dr["TicketId"]) : 0,

                             TicketType = dr["TicketType"].ToString().Replace("&lt;", "<").ToString().Replace("&gt;",">").ToString(),
                             PaymentMethod = dr["PaymentType"].ToString(),


                             InvoiceIntId = dr["InvoiceIntId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceIntId"]) : 0,
                             InvoiceId = dr["InvoiceId"].ToString(),

                             AmountPaid = dr["AmountPaid"] != DBNull.Value ? Convert.ToDouble(dr["AmountPaid"]) : 0.0,
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0.0,
                             InTotalAmount = dr["InTotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["InTotalAmount"]) : 0.0,
                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
            if(buildList.Count() > 0)
            {
                buildList[0].SalesReportCountModel = SalesReportCountModel;
            }

            
            return buildList;
        }

        public InvoiceReportModel GetCollectionReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, int salesCommission, string invostatus, string order, FilterReportModel filter)
        {

            DataSet ds = _EmployeeDataAccess.GetCollectionSalesReportByCompanyId(companyid, pageno, pagesize, startdate, enddate, searchtext, salesCommission, invostatus, order, filter);
            List<Invoice> buildList = new List<Invoice>();
            buildList = (from DataRow dr in ds.Tables[2].Rows
                         select new Invoice()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                             CustomerId = (Guid)dr["CustomerId"],
                             CustomerName = dr["FirstName"].ToString(),
                             SalesLocationName = dr["SalesLocationName"].ToString(),
                             InvoiceId = dr["InvoiceId"].ToString(),
                             InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                             DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                             TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0.0,
                             AmountReceived = dr["PaidBalance"] != DBNull.Value ? Convert.ToDouble(dr["PaidBalance"]) : 0.0,
                             PaymentMethod = dr["PaymentMethod"].ToString(),
                             Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0,
                             Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0.0,
                             TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime()
                             //SalesAfterTax = dr["SalesAfterTax"] != DBNull.Value ? Convert.ToDouble(dr["SalesAfterTax"]) : 0.0,
                             //TotalPaid = dr["TotalPaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalPaid"]) : 0.0,
                             //TotalUnpaid = dr["TotalUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalUnpaid"]) : 0.0,
                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[0].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
            TotalSalesAmountModel TotalSalesAmountModel = new TotalSalesAmountModel();
            TotalSalesAmountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new TotalSalesAmountModel()
                                     {
                                         TotalSalesAmount = dr["TotalSalesAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalSalesAmount"]) : 0.0,
                                         TotalDueAmount = dr["TotalDueAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalDueAmount"]) : 0.0,
                                         CustomerCount = dr["CountCustomer"] != DBNull.Value ? Convert.ToInt32(dr["CountCustomer"]) : 0,
                                         InvoiceCount = dr["CountInvoice"] != DBNull.Value ? Convert.ToInt32(dr["CountInvoice"]) : 0,
                                         TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                                         AveInvoiceAmount = dr["AveInvoiceAmt"] != DBNull.Value ? Convert.ToDouble(dr["AveInvoiceAmt"]) : 0.0,
                                         TotalTax = dr["TotalTax"] != DBNull.Value ? Convert.ToDouble(dr["TotalTax"]) : 0.0,
                                         TotalUnpaid = dr["TotalOpenBalance"] != DBNull.Value ? Convert.ToDouble(dr["TotalOpenBalance"]) : 0.0
                                     }).FirstOrDefault();
            TotalInvoiceAmount TotalInvoiceAmount = new TotalInvoiceAmount();
            TotalInvoiceAmount = (from DataRow dr in ds.Tables[3].Rows
                                     select new TotalInvoiceAmount()
                                     {
                                         TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                                         TotalCollected = dr["TotalCollected"] != DBNull.Value ? Convert.ToDouble(dr["TotalCollected"]) : 0.0,
                                         TotalInvoicesAmount = dr["TotalInvoicesAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalInvoicesAmount"]) : 0.0,
                                         TotalOpenBalance = dr["TotalOpenBalance"] != DBNull.Value ? Convert.ToDouble(dr["TotalOpenBalance"]) : 0.0,
                                         TotalTax = dr["TotalTax"] != DBNull.Value ? Convert.ToDouble(dr["TotalTax"]) : 0.0
                                     }).FirstOrDefault();

            InvoiceReportModel SalesReportModel = new InvoiceReportModel();
            SalesReportModel.ListInvoice = buildList;
            SalesReportModel.InvoiceReportCountModel = SalesReportCountModel;
            SalesReportModel.TotalInvoiceAmountModel = TotalSalesAmountModel;
            SalesReportModel.TotalInvoiceAmount = TotalInvoiceAmount;
            return SalesReportModel;
        }



        public List<EmployeeLeadSource> GetEmployeeLeadSourceByEmployeeId(Guid userId)
        {
            return _EmployeeLeadSourceDataAccess.GetByQuery(string.Format("EmployeeId = '{0}'",userId));
        }

        public List<Customer> GetPartnerReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string order,string eIdList)
        {

            DataSet ds = _EmployeeDataAccess.GetPartnerReportByCompanyId(companyid, pageno, pagesize, startdate, enddate, searchtext, order,eIdList);
            List<Customer> buildList = new List<Customer>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new Customer()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerId = (Guid)dr["CustomerId"],
                             FirstName = dr["FirstName"].ToString(),
                             PrimaryPhone = dr["PrimaryPhone"].ToString(),
                             SecondaryPhone = dr["SecondaryPhone"].ToString(),
                             EmailAddress = dr["EmailAddress"].ToString(),
                             Address = dr["Address"].ToString(),
                             Street = dr["Street"].ToString(),
                             City = dr["City"].ToString(),
                             State = dr["State"].ToString(),
                             ZipCode = dr["ZipCode"].ToString(),
                             Status = dr["Status"].ToString(),
                             IsLead = dr["IsLead"] != DBNull.Value ? Convert.ToBoolean(dr["IsLead"]) : false,
                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             EMPNUM = dr["CreatedByEmpName"].ToString(),
                             CustomerTotalRevenue = dr["CustomerTotalRevenue"] != DBNull.Value ? Convert.ToDouble(dr["CustomerTotalRevenue"]) : 0.0,

                         }).ToList();

            SalesReportCountModel ReportCountModel = new SalesReportCountModel();
            ReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
            if (buildList.Count() > 0)
            {
                buildList[0].TotalCount = ReportCountModel.TotalCount;
            }


            return buildList;
        }


        public PartnerReportBarModel GetPartnerReportBarByCompanyId(Guid companyid,  DateTime? startdate, DateTime? enddate, string eIdList)
        {

            DataSet ds = _EmployeeDataAccess.GetPartnerReportBarByCompanyId(companyid, startdate, enddate, eIdList);
            PartnerReportBarModel buildList = new PartnerReportBarModel();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new PartnerReportBarModel()
                         {
                             CustomerCount = dr["Customer"] != DBNull.Value ? Convert.ToInt32(dr["Customer"]) : 0,
                             LeadCount = dr["Lead"] != DBNull.Value ? Convert.ToInt32(dr["Lead"]) : 0,
                             TotalRevanew = dr["TotalRevanew"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevanew"]) : 0.0,
                             MonthlyMonitoringFee = dr["MonthlyMonitoringFee"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyMonitoringFee"]) : 0.0,
                             ScheduledCount = dr["Scheduled"] != DBNull.Value ? Convert.ToInt32(dr["Scheduled"]) : 0,
                             InstalledCount = dr["Installed"] != DBNull.Value ? Convert.ToInt32(dr["Installed"]) : 0,
                         }).FirstOrDefault();
            return buildList;
        }

        public List<Customer> GetLeadSourceReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string order, string eIdList)
        {

            DataSet ds = _EmployeeDataAccess.GetLeadSourceReportByCompanyId(companyid, pageno, pagesize, startdate, enddate, searchtext, order, eIdList);
            List<Customer> buildList = new List<Customer>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new Customer()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerId = (Guid)dr["CustomerId"],
                             FirstName = dr["FirstName"].ToString(),
                             PrimaryPhone = dr["PrimaryPhone"].ToString(),
                             SecondaryPhone = dr["SecondaryPhone"].ToString(),
                             EmailAddress = dr["EmailAddress"].ToString(),
                             Address = dr["Address"].ToString(),
                             Street = dr["Street"].ToString(),
                             City = dr["City"].ToString(),
                             State = dr["State"].ToString(),
                             ZipCode = dr["ZipCode"].ToString(),
                             Status = dr["Status"].ToString(),
                             IsLead = dr["IsLead"] != DBNull.Value ? Convert.ToBoolean(dr["IsLead"]) : false,
                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             EMPNUM = dr["CreatedByEmpName"].ToString(),
                             CustomerTotalRevenue = dr["CustomerTotalRevenue"] != DBNull.Value ? Convert.ToDouble(dr["CustomerTotalRevenue"]) : 0.0,

                         }).ToList();

            SalesReportCountModel ReportCountModel = new SalesReportCountModel();
            ReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                select new SalesReportCountModel()
                                {
                                    TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                }).FirstOrDefault();
            if (buildList.Count() > 0)
            {
                buildList[0].TotalCount = ReportCountModel.TotalCount;
            }


            return buildList;
        }

        public LeadSourceReportBarModel GetLeadSourceReportBarByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string eIdList)
        {

            DataSet ds = _EmployeeDataAccess.GetLeadSourceReportBarByCompanyId(companyid, startdate, enddate, eIdList);
            LeadSourceReportBarModel buildList = new LeadSourceReportBarModel();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new LeadSourceReportBarModel()
                         {
                             CustomerCount = dr["Customer"] != DBNull.Value ? Convert.ToInt32(dr["Customer"]) : 0,
                             LeadCount = dr["Lead"] != DBNull.Value ? Convert.ToInt32(dr["Lead"]) : 0,
                             TotalRevanew = dr["TotalRevanew"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevanew"]) : 0.0,
                             MonthlyMonitoringFee = dr["MonthlyMonitoringFee"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyMonitoringFee"]) : 0.0,
                             ScheduledCount = dr["Scheduled"] != DBNull.Value ? Convert.ToInt32(dr["Scheduled"]) : 0,
                             InstalledCount = dr["Installed"] != DBNull.Value ? Convert.ToInt32(dr["Installed"]) : 0,
                         }).FirstOrDefault();
            return buildList;
        }


        public DataTable GetSalesReportExportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string search, string invostatus)
        {
            return _EmployeeDataAccess.GetSalesReportExportByCompanyId(companyid, start, end, search, invostatus);
        }
        //public DataTable GetBrinksCustomer(Guid companyid, DateTime? start, DateTime? end, string search, string invostatus)
        //{
        //    return _BrinksCustomerDataAccess.GetSalesReportExportByCompanyId(companyid, start, end, search, invostatus);
        //}
        public DataTable GetInvoiceListReportExportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string search, string invostatus,string order, FilterReportModel filter)
        {
            return _EmployeeDataAccess.GetInvoiceListReportExportByCompanyId(companyid, start, end, search, invostatus,order, filter);
        }

        public DataTable GetEstimateListReportExportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string search, string invostatus, string order, FilterReportModel filter)
        {
            return _EmployeeDataAccess.GetEstimateListReportExportByCompanyId(companyid, start, end, search, invostatus, order, filter);
        }

        public DataTable GetTechEquipmentListReportExportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string search, Guid TechnicianId, int ActiveStatus, int EquipmentClass, int EquipmentCategory)
        {
            return _EmployeeDataAccess.GetTechEquipmentListReportExportByCompanyId(companyid, start, end, search, TechnicianId,ActiveStatus, EquipmentClass, EquipmentCategory);
        }


        public DataTable GetBookingSalesReportExportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string search, string source)
        {
            return _EmployeeDataAccess.GetBookingSalesReportExportByCompanyId(companyid, start, end, search, source);
        }

        public DataTable GetCollectionReportExportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string search,int salesCommission, string invostatus,string order, FilterReportModel filter)
        {
            return _EmployeeDataAccess.GetCollectionReportExportByCompanyId(companyid, start, end, search, salesCommission, invostatus,order, filter);
        }

        public DataTable GetPartnerReportExportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string eiDList)
        {
            return _EmployeeDataAccess.GetPartnerReportExportByCompanyId(companyid, start, end,  eiDList);
        }
        public DataTable GetLeadSourceReportExportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string eiDList)
        {
            return _EmployeeDataAccess.GetLeadSourceReportExportByCompanyId(companyid, start, end, eiDList);
        }


        public List<CustomerPackageService> GetCustomerPackageServiceByCustomerId(Guid customerid)
        {
            DataTable dt = _EmployeeDataAccess.GetCustomerPackageServiceByCustomerId(customerid);
            List<CustomerPackageService> EmployeeList = new List<CustomerPackageService>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new CustomerPackageService()
                            {
                                EquipmentServiceName = dr["EquipmentServiceName"].ToString(),
                                Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0,
                                MonthlyRate = dr["MonthlyRate"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyRate"]) : 0,
                                DiscountRate = dr["DiscountRate"] != DBNull.Value ? Convert.ToDouble(dr["DiscountRate"]) : 0
                            }).ToList();
            return EmployeeList;
        }

        public List<CustomerPackageEqp> GetCustomerPackageEqpByCustomerId(Guid customerid)
        {
            DataTable dt = _EmployeeDataAccess.GetCustomerPackageEqpByCustomerId(customerid);
            List<CustomerPackageEqp> EmployeeList = new List<CustomerPackageEqp>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new CustomerPackageEqp()
                            {
                                EquipmentServiceName = dr["EquipmentServiceName"].ToString(),
                                Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0,
                                UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                DiscountPckage = dr["DiscountPckage"] != DBNull.Value ? Convert.ToDouble(dr["DiscountPckage"]) : 0
                            }).ToList();
            return EmployeeList;
        }

        public List<Employee> GetAllTimeClockSupervisorListByUserId(Guid userid)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserId != '{0}' and IsActive = 1 and IsDeleted = 0 and Recruited = 1", userid)).ToList();
        }

        public long InsertTimeClockSupervisor(EmployeeTimeClockSupervisor etcs)
        {
            return _EmployeeTimeClockSupervisorDataAccess.Insert(etcs);
        }

        public List<EmployeeTimeClockSupervisor> GetTimeClockSupervisorListByUserId(Guid userid)
        {
            return _EmployeeTimeClockSupervisorDataAccess.GetByQuery(string.Format("UserId = '{0}'", userid)).ToList();
        }

        public bool DeleteAllTimeClockSupervisor(Guid userlist)
        {
            return _EmployeeDataAccess.DeleteAllTimeClockSupervisor(userlist);
        }

        public List<Employee> GetEmployeePayrollReportTimeClock()
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("IsPayroll = 1")).ToList();
        }

        public List<Employee> GetAllEmployeeUserAssign()
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("IsDeleted = 0 and Recruited = 1 and IsActive = 1")).ToList();
        }

        public EmployeeListMatrixWithCount GetFirsCallCloseMatrixWithCount(DateTime StartDate, DateTime EndDate, int pageno, int pagesize,string order)
        {
            _ErrorLogDataAccess.Insert(new ErrorLog() { ErrorId = Guid.NewGuid(), ErrorFor = "Facade|Employee|GetFirsCallCloseMatrixWithCount", Message = string.Format("{0} | {1} | {2} | {3}", StartDate, EndDate, pageno, order), TimeUtc=DateTime.Now });
            DataSet ds = _EmployeeDataAccess.GetFirsCallCloseMatrixWithCount(StartDate, EndDate, pageno, pagesize,order);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            EmployeeListMatrixWithCount model = new EmployeeListMatrixWithCount();

            model.EmployeeListMatrixList = (from DataRow dr in dt.Rows
                                                   select new EmployeeListMatrix()
                                                   {
                                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                       UserLoginId=dr["UserLoginId"]!=DBNull.Value? Convert.ToInt32(dr["UserLoginId"]):0,
                                                       EmployeeName = dr["EmployeeName"].ToString(),
                                                       TotalLeads = dr["TotalLeads"] != DBNull.Value ? Convert.ToInt32(dr["TotalLeads"]) : 0,
                                                       BadLeads = dr["BadLeads"] != DBNull.Value ? Convert.ToInt32(dr["BadLeads"]) : 0,
                                                       GoodLeads = dr["TotalLeads"] != DBNull.Value ? Convert.ToInt32(dr["GoodLeads"]) : 0,
                                                       Closing = dr["Closing"] != DBNull.Value ? Convert.ToInt32(dr["Closing"]) : 0,
                                                       Percentage = dr["Percentage"] != DBNull.Value ? Convert.ToDouble(dr["Percentage"]) : 0.0,
                                                       UserX = dr["UserX"] != DBNull.Value ? Convert.ToDouble(dr["UserX"]) : 0.0,
                                                       EmpId = (Guid)dr["EmpId"]
                                                   }).ToList();
            model.TotalEmployeeCount = (from DataRow dr in dt1.Rows
                                                    select new TotalEmployeeCount()
                                                    {
                                                        TotalCount = dr["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dr["TotalEmployee"]) : 0
                                                    }).FirstOrDefault();
            if (model.TotalEmployeeCount.TotalCount > 0)
            {
                model.TotalTotalLeads = model.EmployeeListMatrixList.Select(x => x.TotalLeads).Sum();
                model.TotalBadLeads = model.EmployeeListMatrixList.Select(x => x.BadLeads).Sum();
                model.TotalGoodLeads = model.EmployeeListMatrixList.Select(x => x.GoodLeads).Sum();
                model.TotalClosing = model.EmployeeListMatrixList.Select(x => x.Closing).Sum();
                model.AvgPercentage = model.EmployeeListMatrixList.Select(x => x.Percentage).Average();
                model.AvgUserX = model.EmployeeListMatrixList.Select(x => x.UserX).Average();
            }

            return model;
        }

        public EmployeeListMatrixWithCount GetFirsCallCloseMatrixWithCountT1(string Start, string End, int pageno, int pagesize, string order)
        {
            _ErrorLogDataAccess.Insert(new ErrorLog() { ErrorId = Guid.NewGuid(), ErrorFor = "Facade|Employee|GetFirsCallCloseMatrixWithCount", Message = string.Format("{0} | {1} | {2} | {3}", Start, End, pageno, order), TimeUtc = DateTime.Now });
            DataSet ds = _EmployeeDataAccess.GetFirsCallCloseMatrixWithCount(Start, End, pageno, pagesize, order);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            EmployeeListMatrixWithCount model = new EmployeeListMatrixWithCount();

            model.EmployeeListMatrixList = (from DataRow dr in dt.Rows
                                            select new EmployeeListMatrix()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                UserLoginId = dr["UserLoginId"] != DBNull.Value ? Convert.ToInt32(dr["UserLoginId"]) : 0,
                                                EmployeeName = dr["EmployeeName"].ToString(),
                                                TotalLeads = dr["TotalLeads"] != DBNull.Value ? Convert.ToInt32(dr["TotalLeads"]) : 0,
                                                BadLeads = dr["BadLeads"] != DBNull.Value ? Convert.ToInt32(dr["BadLeads"]) : 0,
                                                GoodLeads = dr["TotalLeads"] != DBNull.Value ? Convert.ToInt32(dr["GoodLeads"]) : 0,
                                                Closing = dr["Closing"] != DBNull.Value ? Convert.ToInt32(dr["Closing"]) : 0,
                                                Percentage = dr["Percentage"] != DBNull.Value ? Convert.ToDouble(dr["Percentage"]) : 0.0,
                                                UserX = dr["UserX"] != DBNull.Value ? Convert.ToDouble(dr["UserX"]) : 0.0,
                                                EmpId = (Guid)dr["EmpId"]
                                            }).ToList();
            model.TotalEmployeeCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dr["TotalEmployee"]) : 0
                                        }).FirstOrDefault();

            model.TotalTotalLeads = ds.Tables[2].Rows[0]["TotalTotalLeads"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalTotalLeads"]) : 0;
            model.TotalBadLeads = ds.Tables[2].Rows[0]["TotalBadLeads"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalBadLeads"]) : 0;
            model.TotalGoodLeads = ds.Tables[2].Rows[0]["TotalGoodLeads"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalGoodLeads"]) : 0;
            model.TotalClosing = ds.Tables[2].Rows[0]["TotalClosing"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalClosing"]) : 0;
            model.AvgPercentage = ds.Tables[2].Rows[0]["AvgPercentage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["AvgPercentage"]) : 0;
            model.AvgUserX = ds.Tables[2].Rows[0]["AvgUserX"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["AvgUserX"]) : 0;

            return model;
        }
        public DataTable GetDownLoadFirstCallCloseReport(DateTime? FilterStartDate, DateTime? FilterEndDate)
        {
            DataSet ds = _EmployeeDataAccess.GetFirsCallCloseDownload(FilterStartDate, FilterEndDate);
            //List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public DataTable GetDownLoadOverallCloseReport(DateTime? FilterStartDate, DateTime? FilterEndDate)
        {
            DataSet ds = _EmployeeDataAccess.GetDownloadOverAllClose(FilterStartDate, FilterEndDate);
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public DataTable GetDownLoadGetAllEmployeeForReport(DateTime? FilterStartDate, DateTime? FilterEndDate, string Search, string[] DeptFilter, string[] StatusFilter, string InsuranceFilter)
        {
            DataSet ds = _EmployeeDataAccess.GetAllEmployeeForReportDownload(FilterStartDate, FilterEndDate, Search, DeptFilter, StatusFilter, InsuranceFilter);
            //List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public DataTable GetAllEmployeeInsuranceForReportDownload(DateTime? FilterStartDate, DateTime? FilterEndDate, string Search, string[] DeptFilter, string[] StatusFilter, string InsuranceFilter)
        {
            DataSet ds = _EmployeeDataAccess.GetAllEmployeeInsuranceForReportDownload(FilterStartDate, FilterEndDate, Search, DeptFilter, StatusFilter, InsuranceFilter);
            //List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public EmployeeListWithCustomerModel GetAllEmployeeForReport(DateTime StartDate, DateTime EndDate,string Search, string[] DeptFilter,string[] StatusFilter,string InsuranceFilter, int pageno, int pagesize, string order)
        {
            DataSet ds = _EmployeeDataAccess.GetAllEmployeeForReport(StartDate, EndDate, Search,DeptFilter,StatusFilter,InsuranceFilter, pageno, pagesize, order);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            EmployeeListWithCustomerModel model = new EmployeeListWithCustomerModel();

            model.EmployeeList = (from DataRow dr in dt.Rows
                                            select new Employee()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                UserIntId = dr["UserIntId"] != DBNull.Value ? Convert.ToInt32(dr["UserIntId"]) : 0,
                                                FirstName = dr["FirstName"].ToString(),
                                                LastName = dr["LastName"].ToString(),
                                                Email = dr["Email"].ToString(),
                                                Street = dr["Street"].ToString(),
                                                Street2 = dr["Street2"].ToString(),
                                                City = dr["City"].ToString(),
                                                State = dr["State"].ToString(),
                                                ZipCode = dr["ZipCode"].ToString(),
                                                Department = dr["Department"].ToString(),
                                                DOB = dr["DOB"] != DBNull.Value ? Convert.ToDateTime(dr["DOB"]) : new DateTime(),
                                                HireDate = dr["HireDate"] != DBNull.Value ? Convert.ToDateTime(dr["HireDate"]) : new DateTime(),
                                                PtoRate = dr["PtoRate"] != DBNull.Value ? Convert.ToDouble(dr["PtoRate"]) : 0.0,
                                                Occurence = dr["Occurence"] != DBNull.Value ? Convert.ToDouble(dr["Occurence"]) : 0.0,
                                                Insurance = dr["Insurance"].ToString(),
                                                EligibleFrom = dr["EligibleFrom"] != DBNull.Value ? Convert.ToDateTime(dr["EligibleFrom"]) : new DateTime(),
                                                LastEvaluationDate = dr["LastEvaluationDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastEvaluationDate"]) : new DateTime(),
                                                InstallLicenseExpirationDate= dr["InstallLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallLicenseExpirationDate"]) : new DateTime(),
                                           
                                                FireLicenseExpirationDate= dr["FireLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["FireLicenseExpirationDate"]) : new DateTime(),
                                                DriversLicenseExpirationDate = dr["DriversLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["DriversLicenseExpirationDate"]) : new DateTime(),

                                                MedicalPlan = dr["MedicalPlan"].ToString(),
                                                DentalPlan = dr["DentalPlan"].ToString(),

                                                MedicalType = dr["MedicalType"].ToString(),
                                                DentalType = dr["DentalType"].ToString(),
                                                VisionType = dr["VisionType"].ToString(),

                                                MedicalAmount = dr["MedicalAmount"] != DBNull.Value ? Convert.ToDouble(dr["MedicalAmount"]) : 0.0,
                                                DentalAmount = dr["DentalAmount"] != DBNull.Value ? Convert.ToDouble(dr["DentalAmount"]) : 0.0,
                                                VisionAmount = dr["VisionAmount"] != DBNull.Value ? Convert.ToDouble(dr["VisionAmount"]) : 0.0,
                                                VoluntaryLifeAmount = dr["VoluntaryLifeAmount"] != DBNull.Value ? Convert.ToDouble(dr["VoluntaryLifeAmount"]) : 0.0,
                                                STDAmount = dr["STDAmount"] != DBNull.Value ? Convert.ToDouble(dr["STDAmount"]) : 0.0,
                                                LTDAmount = dr["LTDAmount"] != DBNull.Value ? Convert.ToDouble(dr["LTDAmount"]) : 0.0,

                                                NextEvaluationDate = dr["NextEvaluationDate"] != DBNull.Value ? Convert.ToDateTime(dr["NextEvaluationDate"]) : new DateTime(),

                                                IsMedical = dr["IsMedical"].ToString(),
                                                IsDental = dr["IsDental"].ToString(),
                                                IsVision = dr["IsVision"].ToString(),
                                                IsVoluntaryLife = dr["IsVoluntaryLife"].ToString(),
                                                IsSTD = dr["IsSTD"].ToString(),
                                                IsLTD = dr["IsLTD"].ToString(),

                                            }).ToList();
            model.TotalEmployeeCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dr["TotalEmployee"]) : 0
                                        }).FirstOrDefault();

         
            return model;
        }
        public DataTable GetDownLoadSoldtofundedReport(DateTime? FilterStartDate, DateTime? FilterEndDate)
        {
            DataSet ds = _EmployeeDataAccess.GetDownloadSoldToFunded(FilterStartDate, FilterEndDate);
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public DataTable GetDownLoadNumberofsalesReport(DateTime? FilterStartDate, DateTime? FilterEndDate)
        {
            DataSet ds = _EmployeeDataAccess.GetDownloadNumberOfSales(FilterStartDate, FilterEndDate);
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public DataTable GetDownLoadAppointmentSetReport(DateTime? FilterStartDate, DateTime? FilterEndDate)
        {
            DataSet ds = _EmployeeDataAccess.GetDownLoadAppointmentSet(FilterStartDate, FilterEndDate);
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public EmployeeListMatrixCustomerModel GetSoldFundedMatrixCustomerData(DateTime? StartDate,DateTime? EndDate,Guid EmployeeId, int pageno, int pagesize,string from)
        {
            DataSet dsResult = _EmployeeDataAccess.GetSoldFundedMatrixCustomerData(StartDate,EndDate,EmployeeId, pageno, pagesize,from);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            EmployeeListMatrixCustomerModel model = new EmployeeListMatrixCustomerModel();

            model.EmployeeListMatrixCustomerList = (from DataRow dr in dt.Rows
                                                    select new CustomerData()
                                                    {
                                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                        RMR = dr["MonthlyMonitoringFee"].ToString(),
                                                        CustomerName = dr["CustomerName"].ToString(),
                                                        CSNumber = dr["CustomerNo"].ToString(),
                                                        LeadSource = dr["LeadSourceVal"].ToString(),
                                                        LeadStatusVal = dr["LeadStatusVal"].ToString(),
                                                        SaleDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                                        AppoinmentSet = dr["AppoinmentSet"].ToString(),
                                                        CreatedDay = dr["CreatedDay"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDay"]) : new DateTime(),
                                                    }).ToList();
            model.TotalCustomerCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0
                                        }).FirstOrDefault();
            return model;
        }
        public DataTable GetSoldFundedMatrixCustomerDataDownload(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId, string from)
        {
            DataSet ds = _EmployeeDataAccess.GetSoldFundedMatrixCustomerDataDownload(StartDate, EndDate, EmployeeId, from);
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public EmployeeListMatrixCustomerModel GetFirsCallCustomerData(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId, int pageno, int pagesize)
        {
            DataSet dsResult = _EmployeeDataAccess.GetFirsCallCustomerData(StartDate, EndDate,EmployeeId, pageno, pagesize);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            EmployeeListMatrixCustomerModel model = new EmployeeListMatrixCustomerModel();

            model.EmployeeListMatrixCustomerList = (from DataRow dr in dt.Rows
                                            select new CustomerData()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                CustomerName = dr["CustomerName"].ToString(),

                                                RMR = dr["MonthlyMonitoringFee"].ToString(),
                                                CSNumber = dr["CustomerNo"].ToString(),
                                                LeadSource = dr["LeadSourceVal"].ToString(),
                                                SaleDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                                AppoinmentSet = dr["AppoinmentSet"].ToString(),
                                                CreatedDay = dr["CreatedDay"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDay"]) : new DateTime(),
                                            }).ToList();
            model.TotalCustomerCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0
                                        }).FirstOrDefault();
            return model;
        }

        public DataTable GetFirsCallCustomerDataDownload(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId)
        {
            DataSet ds = _EmployeeDataAccess.GetFirsCallCustomerDataDownload(StartDate, EndDate, EmployeeId);
            DataTable buildList = ds.Tables[0];

            return buildList;
        }

        public EmployeeListMatrixCustomerModel GetFirsCallCloseCustomerData(DateTime? StartDate, DateTime? EndDate,Guid EmployeeId, int pageno, int pagesize,string from)
        {
            DataSet dsResult = _EmployeeDataAccess.GetFirsCallCloseCustomerData(StartDate, EndDate,EmployeeId, pageno, pagesize,from);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            EmployeeListMatrixCustomerModel model = new EmployeeListMatrixCustomerModel();

            model.EmployeeListMatrixCustomerList = (from DataRow dr in dt.Rows
                                                    select new CustomerData()
                                                    {
                                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                        CustomerName = dr["CustomerName"].ToString(),
                                                        RMR = dr["MonthlyMonitoringFee"].ToString(),
                                                        CSNumber = dr["CustomerNo"].ToString(),
                                                        LeadSource = dr["LeadSourceVal"].ToString(),
                                                        LeadStatusVal = dr["LeadStatusVal"].ToString(),
                                                        SaleDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                                        AppoinmentSet = dr["AppoinmentSet"].ToString(),
                                                        CreatedDay = dr["CreatedDay"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDay"]) : new DateTime()
                                                    }).ToList();
            model.TotalCustomerCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0
                                        }).FirstOrDefault();
            return model;
        }

        public DataTable GetFirsCallCloseCustomerDataDownload(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId, string from)
        {
            DataSet ds = _EmployeeDataAccess.GetFirsCallCloseCustomerDownload(StartDate, EndDate, EmployeeId, from);
            DataTable buildList = ds.Tables[0];

            return buildList;
        }


        public EmployeeListMatrixWithCount GetOverAllCloseMatrixWithCount(DateTime StartDate, DateTime EndDate, int pageno, int pagesize,string order)
        {
            DataSet ds = _EmployeeDataAccess.GetOverAllCloseMatrixWithCount(StartDate, EndDate, pageno, pagesize,order);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            EmployeeListMatrixWithCount model = new EmployeeListMatrixWithCount();

            model.EmployeeListMatrixList = (from DataRow dr in dt.Rows
                                            select new EmployeeListMatrix()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                UserLoginId = dr["UserLoginId"] != DBNull.Value ? Convert.ToInt32(dr["UserLoginId"]) : 0,
                                                EmployeeName = dr["EmployeeName"].ToString(),
                                                TotalLeads = dr["TotalLeads"] != DBNull.Value ? Convert.ToInt32(dr["TotalLeads"]) : 0,
                                                BadLeads = dr["BadLeads"] != DBNull.Value ? Convert.ToInt32(dr["BadLeads"]) : 0,
                                                GoodLeads = dr["TotalLeads"] != DBNull.Value ? Convert.ToInt32(dr["GoodLeads"]) : 0,
                                                Closing = dr["Closing"] != DBNull.Value ? Convert.ToInt32(dr["Closing"]) : 0,
                                                Percentage = dr["Percentage"] != DBNull.Value ? Convert.ToDouble(dr["Percentage"]) : 0.0,
                                                UserX = dr["UserX"] != DBNull.Value ? Convert.ToDouble(dr["UserX"]) : 0.0,
                                                EmpId = (Guid)dr["EmpId"]
                                            }).ToList();
            model.TotalEmployeeCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dr["TotalEmployee"]) : 0
                                        }).FirstOrDefault();
            if (model.TotalEmployeeCount.TotalCount > 0)
            {
                model.TotalTotalLeads = model.EmployeeListMatrixList.Select(x => x.TotalLeads).Sum();
                model.TotalBadLeads = model.EmployeeListMatrixList.Select(x => x.BadLeads).Sum();
                model.TotalGoodLeads = model.EmployeeListMatrixList.Select(x => x.GoodLeads).Sum();
                model.TotalClosing = model.EmployeeListMatrixList.Select(x => x.Closing).Sum();
                model.AvgPercentage = model.EmployeeListMatrixList.Select(x => x.Percentage).Average();
                model.AvgUserX = model.EmployeeListMatrixList.Select(x => x.UserX).Average();
            }


            return model;
        }
        public EmployeeListMatrixWithCount GetSoldToFundedMatrixWithCount(DateTime StartDate, DateTime EndDate, int pageno, int pagesize,string order)
        {
            DataSet ds = _EmployeeDataAccess.GetSoldToFundedMatrixWithCount(StartDate, EndDate, pageno, pagesize,order);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            EmployeeListMatrixWithCount model = new EmployeeListMatrixWithCount();

            model.EmployeeListMatrixList = (from DataRow dr in dt.Rows
                                            select new EmployeeListMatrix()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                UserLoginId = dr["UserLoginId"] != DBNull.Value ? Convert.ToInt32(dr["UserLoginId"]) : 0,
                                                EmployeeName = dr["EmployeeName"].ToString(),
                                                NoOfLeads = dr["NoOfLeads"] != DBNull.Value ? Convert.ToInt32(dr["NoOfLeads"]) : 0,
                                                CustomerFunded = dr["CustomerFunded"] != DBNull.Value ? Convert.ToInt32(dr["CustomerFunded"]) : 0,
                                                Percentage = dr["Percentage"] != DBNull.Value ? Convert.ToDouble(dr["Percentage"]) : 0.0,
                                                UserX = dr["UserX"] != DBNull.Value ? Convert.ToDouble(dr["UserX"]) : 0.0,
                                                EmpId = (Guid)dr["EmpId"]
                                            }).ToList();
            model.TotalEmployeeCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dr["TotalEmployee"]) : 0
                                        }).FirstOrDefault();

            model.TotalTotalSales = ds.Tables[2].Rows[0]["TotalTotalSales"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalTotalSales"]) : 0;
            model.TotalCustomerFunded = ds.Tables[2].Rows[0]["TotalCustomerFunded"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalCustomerFunded"]) : 0;
            model.AvgPercentage = ds.Tables[2].Rows[0]["AvgPercentage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["AvgPercentage"]) : 0;
            model.AvgUserX = ds.Tables[2].Rows[0]["AvgUserX"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["AvgUserX"]) : 0;

            return model;
        }
        public EmployeeListMatrixWithCount GetNumberOfSalesMatrixWithCount(DateTime StartDate, DateTime EndDate, int pageno, int pagesize,string order)
        {
            DataSet ds = _EmployeeDataAccess.GetNumberOfSalesMatrixWithCount(StartDate, EndDate, pageno, pagesize,order);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            EmployeeListMatrixWithCount model = new EmployeeListMatrixWithCount();

            model.EmployeeListMatrixList = (from DataRow dr in dt.Rows
                                            select new EmployeeListMatrix()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                UserLoginId = dr["UserLoginId"] != DBNull.Value ? Convert.ToInt32(dr["UserLoginId"]) : 0,
                                                EmployeeName = dr["EmployeeName"].ToString(),
                                                Closing = dr["Closing"] != DBNull.Value ? Convert.ToInt32(dr["Closing"]) : 0,
                                                UserX = dr["UserX"] != DBNull.Value ? Convert.ToDouble(dr["UserX"]) : 0.0,
                                                EmpId = (Guid)dr["EmpId"]
                                            }).ToList();
            model.TotalEmployeeCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dr["TotalEmployee"]) : 0
                                        }).FirstOrDefault();

            model.TotalTotalSales = ds.Tables[2].Rows[0]["TotalTotalSales"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalTotalSales"]) : 0;
            model.AvgUserX = ds.Tables[2].Rows[0]["AvgUserX"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["AvgUserX"]) : 0;

            return model;
        }
        public EmployeeListMatrixWithCount GetAppointmentSetMatrixWithCount(DateTime StartDate, DateTime EndDate, int pageno, int pagesize,string order)
        {
            DataSet ds = _EmployeeDataAccess.GetAppointmentSetMatrixWithCount(StartDate, EndDate, pageno, pagesize,order);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            EmployeeListMatrixWithCount model = new EmployeeListMatrixWithCount();

            model.EmployeeListMatrixList = (from DataRow dr in dt.Rows
                                            select new EmployeeListMatrix()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                UserLoginId = dr["UserLoginId"] != DBNull.Value ? Convert.ToInt32(dr["UserLoginId"]) : 0,
                                                EmployeeName = dr["EmployeeName"].ToString(),
                                                NoOfLeads = dr["NoOfLeads"] != DBNull.Value ? Convert.ToInt32(dr["NoOfLeads"]) : 0,
                                                AppoinmentSet = dr["AppoinmentSet"] != DBNull.Value ? Convert.ToInt32(dr["AppoinmentSet"]) : 0,
                                                Percentage = dr["Percentage"] != DBNull.Value ? Convert.ToDouble(dr["Percentage"]) : 0.0,
                                                UserX = dr["UserX"] != DBNull.Value ? Convert.ToDouble(dr["UserX"]) : 0.0,
                                                EmpId = (Guid)dr["EmpId"]
                                            }).ToList();
            model.TotalEmployeeCount = (from DataRow dr in dt1.Rows
                                        select new TotalEmployeeCount()
                                        {
                                            TotalCount = dr["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dr["TotalEmployee"]) : 0
                                        }).FirstOrDefault();

            model.TotalTotalLeads = ds.Tables[2].Rows[0]["TotalTotalLeads"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalTotalLeads"]) : 0;
            model.TotalAppoinmentSet = ds.Tables[2].Rows[0]["TotalAppoinmentSet"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalAppoinmentSet"]) : 0;
            model.AvgPercentage = ds.Tables[2].Rows[0]["AvgPercentage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["AvgPercentage"]) : 0;
            model.AvgUserX = ds.Tables[2].Rows[0]["AvgUserX"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["AvgUserX"]) : 0;

            return model;
        }
        public int InsertEmployeeLeadSource(EmployeeLeadSource el)
        {
            return (int)_EmployeeLeadSourceDataAccess.Insert(el);
        }

        public bool DeleteEmployeeLeadSourceByUserId(Guid userId)
        {
            return _EmployeeLeadSourceDataAccess.DeleteEmployeeLeadSourceByUserId(userId);
        }
        public List<string> GetEmployeeList(List<string> emp)
        {
            return _EmployeeDataAccess.GetAssignedEmployeeList(emp);
        }
        public List<Employee> GetEmployeeListCalendarSchedule(Guid empid)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeListCalendarSchedule(empid);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                ResourceName = dr["ResourceName"].ToString(),
                                UserId = (Guid)dr["UserId"]
                            }).ToList();
            return EmployeeList;
        }
        public List<Employee> GetEmployeeListCustomCalendar(Guid empid)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeListCustomCalendar(empid);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                ResourceName = dr["ResourceName"].ToString(),
                                UserId = (Guid)dr["UserId"]
                            }).ToList();
            return EmployeeList;
        }
        public UpsellUserReportModel GetUpsellTechnicianList(Guid userid, string startdate, string enddate, string searchtxt)
        {
            DataSet dsResult = _EmployeeDataAccess.GetUpsellTechnicianList(userid, startdate, enddate, searchtxt);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            UpsellUserReportModel Model = new UpsellUserReportModel();
            Model.UpsellUserModelList = (from DataRow dr in dt1.Rows
                            select new UpsellUserModel()
                            {
                                EmpId = dr["EmpId"] != DBNull.Value ? Convert.ToInt32(dr["EmpId"]) : 0,
                                UserId = (Guid)dr["UserId"],
                                EmpDay = dr["EmpDay"].ToString(),
                                EmployeeName = dr["EmployeeName"].ToString(),
                                AddedRMR = dr["AddedRMR"] != DBNull.Value ? Convert.ToDouble(dr["AddedRMR"]) : 0,
                                ServiceName = dr["ServiceName"].ToString(),
                                PiecesSold = dr["PiecesSold"] != DBNull.Value ? Convert.ToInt32(dr["PiecesSold"]) : 0,
                                CollectedAmount = dr["CollectedAmount"] != DBNull.Value ? Convert.ToDouble(dr["CollectedAmount"]) : 0,
                            }).ToList();
            Model.TotalUpSell = (from DataRow dr in dt.Rows
                                 select new TotalUpSell()
                                 {
                                     Total = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();
            Model.SumOfUpsell = (from DataRow dr in dt2.Rows
                                         select new SumOfUpsell()
                                         {
                                             TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                                             TotalRMR = dr["TotalRMR"] != DBNull.Value ? Convert.ToDouble(dr["TotalRMR"]) : 0.0,
                                             TotalSold = dr["TotalSold"] != DBNull.Value ? Convert.ToDouble(dr["TotalSold"]) : 0.0
                                         }).FirstOrDefault();
            return Model;
        }
        public EmployeeAccrualPtoAndApprovePtohourModel GetEmployeeAccrualPtoAndApprovePtohour(Guid userid)
        {
            DataSet dsResult = _EmployeeDataAccess.GetEmployeeAccrualPtoAndApprovePtohour(userid);
            DataTable dt = dsResult.Tables[0]; 
            EmployeeAccrualPtoAndApprovePtohourModel Model = new EmployeeAccrualPtoAndApprovePtohourModel();
            Model.TotalPto = (from DataRow dr in dt.Rows
                                         select new AccrualPtoHourModel()
                                         {
                                           TotalPtoHour = dr["TotalRemaining"] != DBNull.Value ? Convert.ToDouble(dr["TotalRemaining"]) : 0.0,
                                         }).FirstOrDefault(); 
            return Model;
        }

        public EmployeeAccrualPtoAndApprovePtohourModel GetEmployeeAccrualPTOList(PayrollFilterModel filter)
        {
            DataSet dsResult = _EmployeeDataAccess.GetEmployeeAccrualPTOList(filter);
            DataTable dt = dsResult.Tables[0];
            //DataTable dt1 = dsResult.Tables[1];
            EmployeeAccrualPtoAndApprovePtohourModel Model = new EmployeeAccrualPtoAndApprovePtohourModel();
            Model.EmployeePTOHourLogList = (from DataRow dr in dt.Rows
                                         select new EmployeePTOHourLog()
                                         {
                                             EmployeeId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                             HireDate = dr["HireDate"] != DBNull.Value ? Convert.ToDateTime(dr["HireDate"]) : new DateTime(),
                                             PayType = dr["PayType"].ToString(),
                                             EmployeeName = dr["EmployeeName"].ToString(),
                                             UserId = (Guid)dr["UserId"], 
                                             TotalPTOEarned = dr["TotalPTOHour"] != DBNull.Value ? Convert.ToDouble(dr["TotalPTOHour"]) : 0.0, 
                                         }).ToList(); 
            return Model;
        }

        public DataTable GetUpsellTechnicianListExport(Guid userid, string startdate, string enddate, string searchtxt)
        {
            return _EmployeeDataAccess.GetUpsellTechnicianListExport(userid, startdate, enddate, searchtxt);
        }
        
        public Employee GetEmployeeByCompanyIdAndUserId(Guid comid, Guid userid)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and UserId = '{1}'", comid, userid)).FirstOrDefault();
        }
        public int GetTotalEmployeeCountByCompanyId(Guid companyID)
        {
            int count = 0;
            DataTable dt = _EmployeeDataAccess.GetTotalEmployeeCountByCompanyId(companyID);
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalEmployee"]) : 0;
            }
            return count;
        }

        #region Digiture


        public List<SelectListItem> GetEmployeeDropDownList(Guid CompanyId, string searchtext, string Tag, Guid userid, string tag2 = "")
        {
            //List<SelectListItem> DropDownList = new List<SelectListItem>();
            DataTable dtTechs = _EmployeeDataAccess.GetEmployeeByCompanyIdAndTagAndSearch(CompanyId, searchtext, Tag, userid, tag2);
            //List<SelectListItem> DropDownList = dtTechs.ToList<SelectListItem>();
            List<SelectListItem> DropDownList = (from DataRow dr in dtTechs.Rows
                                                select new SelectListItem()
                                                {
                                                    Text = dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                                                    Value = dr["UserId"].ToString()
                                               }).ToList();
                //dtTechs.Select(x =>
                //                               new SelectListItem()
                //                               {
                //                                   Text = x[""] + " " + x[""],
                //                                   Value = x[""],
                //                               });
                //}
            return DropDownList;
        }


        #endregion Digiture
        #region Holiday Calendar
        public double GetTotalPromiseHours(DateTime Start, DateTime End, float hours, string weeks, Guid user)
        {
            return _EmployeeOperationsDataAccess.GetTotalPromiseHours(Start, End, hours, weeks, user);
        } 
        public List<EmployeeOperations> GetAllEmployeeOperationByIdandFromDate(Guid id, DateTime date)
        {
            return _EmployeeOperationsDataAccess.GetByQuery(string.Format(" EmployeeId = '{0}' and SelectedDate >= '{1}'", id, date.ToString("yyyy/MM/dd"))).ToList();
        }
        #endregion Holiday Calendar

        #region Employee Operation
        public EmployeeOperations GetEmployeeOperationById(int id)
        {
            return _EmployeeOperationsDataAccess.Get(id);
        }
        public List<EmployeeOperations> GetAllEmployeeOperationById(Guid id, DateTime sdate, DateTime edate)
        {
            return _EmployeeOperationsDataAccess.GetByQuery(string.Format(" EmployeeId = '{0}' and SelectedDate between '{1}' and '{2}'", id, sdate.SetZeroHour(), edate.SetMaxHour())).ToList();
        }
        public EmployeeOperations GetOnlyEmployeeOperationById(Guid id, DateTime day)
        {
            return _EmployeeOperationsDataAccess.GetByQuery(string.Format(" EmployeeId = '{0}' and SelectedDate = '{1}'", id, day)).FirstOrDefault();
        }
        public int InsertEmployeeOperation(EmployeeOperations model)
        {
            return (int)_EmployeeOperationsDataAccess.Insert(model);
        }
        public int UpdateEmployeeOperation(EmployeeOperations model)
        {
            return (int)_EmployeeOperationsDataAccess.Update(model);
        }
        #endregion Employee Operation
        #region Employee Pto Accrual Rate
        public int UpdateEmployeePtoAccrualRate(EmployeePtoAccrualRate model)
        {
            return (int)_EmployeePtoAccrualRateDataAccess.Update(model);
        }
        public int InsertEmployeePtoAccrualRate(EmployeePtoAccrualRate model)
        {
            return (int)_EmployeePtoAccrualRateDataAccess.Insert(model);
        }
        public EmployeePtoAccrualRate GetEmployeePtoAccrualRateByCreatedBy(Guid CompanyId ,string CreatedBy)
        {
            string query =  string.Format("CompanyId = '{0}' and CreatedBy = '{1}'", CompanyId, CreatedBy);
            return _EmployeePtoAccrualRateDataAccess.GetByQuery(query).LastOrDefault();
        }
        public EmployeePtoAccrualRate GetEmployeePtoAccrualRateById(int id)
        {
            return _EmployeePtoAccrualRateDataAccess.Get(id);
        }
        public List<EmployeePtoAccrualRate> GetAllEmployeePtoAccrualRate()
        {
            return _EmployeePtoAccrualRateDataAccess.GetAll();
        }
        public EmployeePtoAccrualRate GetEmployeePtoAccrualRate(int Maxvalue, string Paytype)
        {
            string query = string.Format(" {0} BETWEEN MinimumDay AND MaximumDay and PayType = '{1}'", Maxvalue, Paytype);
            return _EmployeePtoAccrualRateDataAccess.GetByQuery(query).FirstOrDefault();
        }
        #endregion Employee Pto Accrual Rate 
        #region Employee Pto Hour Log
        public int InsertEmployeePTOHourLog(EmployeePTOHourLog model)
        {
            return (int)_EmployeePTOHourLogDataAccess.Insert(model);
        }
        public List<EmployeePTOHourLog> GetEmployeePTOHourLogByUserId(Guid UserId)
        {
            string query = string.Format(" UserId = '{0}'", UserId);
            return _EmployeePTOHourLogDataAccess.GetByQuery(query).ToList();
        }
        public List<EmployeePTOHourLog> GetAllEmployeePTOHourLogbyUserId(Guid UserId,string Paytype)
        {
            DataTable dt = _EmployeePTOHourLogDataAccess.GetAllEmployeePTOHourLogbyUserId(UserId,Paytype);
            List<EmployeePTOHourLog> EmployeeList = new List<EmployeePTOHourLog>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new EmployeePTOHourLog()
                            {
                                EmployeeName = dr["EmployeeName"].ToString(),
                                PayType = dr["PayType"].ToString(),
                                UserId = (Guid)dr["UserId"],
                                HireDate = dr["HireDate"] != DBNull.Value ? Convert.ToDateTime(dr["HireDate"]) : new DateTime(),
                                FromDate = dr["FromDate"] != DBNull.Value ? Convert.ToDateTime(dr["FromDate"]) : new DateTime(),
                                EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                PTOHour = dr["PTOHour"] != DBNull.Value ? Convert.ToDouble(dr["PTOHour"]) : 0.0,
                                WorkingHours = dr["WorkingHours"] != DBNull.Value ? Convert.ToDouble(dr["WorkingHours"]) : 0.0,
                            }).ToList();
            return EmployeeList;
        }
        #endregion Employee Pto Hour Log
    }
}
