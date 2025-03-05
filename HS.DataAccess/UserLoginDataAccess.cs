using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Linq;

namespace HS.DataAccess
{
	public partial class UserLoginDataAccess
	{
        public UserLoginDataAccess(string ConnectionStr):base(ConnectionStr) { }
        public UserLoginDataAccess() { }

        public UserLoginList GetByQuery(String query, bool resetdb)
        {
            using (SqlCommand cmd = GetSPCommand(GETUSERLOGINBYQUERY, resetdb))
            {
                AddParameter(cmd, pNVarChar("Query", 4000, query));
                return GetList(cmd, ALL_AVAILABLE_RECORDS); ;
            }
        }
        public UserLogin GetUserLogin(string email, string password, string MasterPassword, bool RememberMe, Guid comid)
        {
 
            String SQLQuery = @"Select * from UserLogin Where UserName =@UserName and ([Password]=@Password or @MasterPassword=@Password )  
                and IsActive = @IsActive 
                and IsDeleted = @IsDeleted 
                and CompanyId = @CompanyId";

            using (SqlCommand cmd = GetSQLCommand(SQLQuery))
            {
                AddParameter(cmd, pNVarChar("UserName", 250, email)); 
                AddParameter(cmd, pNVarChar("Password", 100, password));
                AddParameter(cmd, pNVarChar("MasterPassword", 100, MasterPassword)); 
                AddParameter(cmd, pBit("IsActive",  true));
                AddParameter(cmd, pBit("IsDeleted", false));
                AddParameter(cmd, pGuid("CompanyId", comid));

                return GetList(cmd, ALL_AVAILABLE_RECORDS).FirstOrDefault();
            }
        }
        public DataTable GetAllRecruitUserListByCompanyId(Guid ComId, string UserStatus)
        {
            string sqlQuery = @"select 
                                distinct ul.Id as Id,
                                emp.email as Email,
                                ul.IsActive ,
                                emp.Title +' '+emp.FirstName +' '+emp.LastName as ContactName,
                                emp.Recruited as IsRecruited,
                                emp.LastUpdatedDate as LastUpdate,
								emp.Phone as EmpPhone,
                                emp.Status as EmpStatus,
                                emp.CreatedDate as Datestamp,
								cb.Street as UserStreet,
								cb.City + ', ' + cb.State + ' ' + cb.ZipCode as UserLocation,
                                pg.Name as AccessRights,
                                pg.Tag as Tags

                                 from UserCompany uc
                                 left join UserLogin ul 
	                                on ul.UserId = uc.UserId
                                 left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                Left Join Employee emp
									on emp.UserId = ul.UserId
                                left join UserBranch ub on ub.UserId = uc.UserId
								left join CompanyBranch cb on cb.Id = ub.BranchId
                                where ul.Id is not null 
	                                and pg.Name is not null
                                    and emp.FirstName is not null 
									and emp.LastName is not null
                                    and emp.RecruitmentProcess =1  
                                    and uc.CompanyId ='{0}'
                                    ";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(UserStatus))
            {
                subquery = string.Format("and emp.Status = '{0}'", UserStatus);
                sqlQuery += subquery;
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, ComId, UserStatus);
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



        public bool InsertUserCredential(string UserName, Guid UserId, Guid CompanyId, string Email, string Password)
        {
            string sqlQuery = @"Declare @Username nvarchar(200)
                                Declare @UserId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                Declare @EmailAddress nvarchar(200)
                                Declare @Password nvarchar(50)
                                Declare @PermissionGroupId int

                                set @Username= '{0}'
                                set @UserId ='{1}'
                                set @CompanyId ='{2}'
                                set @EmailAddress ='{3}'
                                set @Password = '{4}'
                                --UserOrganization
                                --if not exists (select * from rmrmaster.dbo.UserOrganization where CompanyId =@CompanyId and UserName =@Username)
                                --begin
	                                --insert into UserOrganization ( CompanyId,UserName, IsActive) values 
	                                --(@CompanyId,@Username,1) 
                               -- end
                                --Userlogin
                                if not exists(select * from UserLogin where UserName =@Username)
                                begin
                                insert into UserLogin (EmailAddress,UserName,IsActive,IsDeleted,LastUpdatedBy,LastUpdatedDate,[Password],UserId) 
                                values (@EmailAddress,@Username,1,0,'lastUpdatedBy',GETDATE(),@Password,@UserId) 
                                end

                                --PermissionGroup
                                if not exists(select * from PermissionGroup where Name ='Customer' Or Tag ='Customer')
                                begin
                                insert into PermissionGroup (CompanyId,Name,Tag) values (@CompanyId,'Customer','Customer')
                                set @PermissionGroupId = SCOPE_IDENTITY()
                                end
                                else
                                begin
	                                set @PermissionGroupId = (select top(1) id   from PermissionGroup where Name='Customer' or Tag='Customer')
                                end

                                --UserPermission
                                if not exists(select * from UserPermission where UserId = @UserId)
                                begin
                                insert into UserPermission (CompanyId ,PermissionGroupId,UserId) values(@CompanyId,@PermissionGroupId,@UserId)
                                end ";
            
          
            try
            {
                sqlQuery = string.Format(sqlQuery, UserName, UserId, CompanyId, Email, Password);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public DataTable GetAllUserForExport(int? UserGroup, string searchText, string currentemp, Guid ComId)
        {
            string SearchBySql = "";
            string subquery = "";
            string UserGroupFilterQuery = "";
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                SearchBySql = @"and(emp.FirstName + ' ' + emp.LastName like '{0}%'
                or emp.LastName like '{0}%' or emp.Email like '%{0}%' or emp.Phone like '%{0}%')";

                SearchBySql = string.Format(SearchBySql, searchText);

            }
            if (!string.IsNullOrWhiteSpace(currentemp) && currentemp.ToLower() != "all")
            {
                if (currentemp == "1")
                {
                    subquery = string.Format("and emp.IsCurrentEmployee = 1");
                }
                else
                {
                    subquery = string.Format("and (emp.IsCurrentEmployee = 0 or emp.IsCurrentEmployee is null or pg.Name = 'Old Employees')");
                }
            }
            string DeleteEmployeQuery = " and ul.IsDeleted = 0";
            if (UserGroup.HasValue && UserGroup > 0)
            {

                if (UserGroup == 404)
                {
                    DeleteEmployeQuery = " and ul.IsDeleted = 1";
                }
                else
                {
                    UserGroupFilterQuery = string.Format("and pg.Id ={0}", UserGroup);
                    DeleteEmployeQuery = " and ul.IsDeleted = 0";
                }

            }
            string sqlQuery = @"select 
                                distinct ul.Id as Id,
                                emp.Title +' '+emp.FirstName +' '+emp.LastName as ContactName,
                                emp.Email as Email,
                                emp.Recruited as IsRecruited,
                                pg.Name as AccessRights,
                                pg.Tag as Tags,
								emp.IsCalendar,
						        case when emp.CalendarColor is null or emp.CalendarColor = '' then ''
								else
								'#'+emp.CalendarColor end as CalendarColor,
		                        CASE   
                                WHEN emp.IsActive = 1 THEN 'Active'  
								WHEN emp.IsDeleted = 1 THEN 'Deleted'  
                                else 'Invited'  
                                END  [Status],  
                               CASE   
                               WHEN emp.IsCurrentEmployee = 1 THEN 'Yes'  
                               else 'No'  
                                END  [Current Employee],
                                (select top(1) FirstName +' '+LastName from Employee where CONVERT(nvarchar(50),UserId) = emp.Supervisorid) as Supervisor
                                
                                from UserCompany uc
                                 left join UserLogin ul 
	                                on ul.UserId = uc.UserId
                                 left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                Left Join Employee emp
									on emp.UserId = ul.UserId
                                where ul.Id is not null
                                -- and ul.IsDeleted = 0
                                {3}
	                            and pg.Name is not null
                                and emp.FirstName is not null 
								and emp.LastName is not null
                                and emp.Recruited =1 
                                and uc.CompanyId = '{4}'
                                and emp.CompanyId = '{4}'
                                {0}
                                {2}
                                {1}
                                order by ContactName asc
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery, SearchBySql, subquery, UserGroupFilterQuery, DeleteEmployeQuery, ComId);
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

        public DataTable GetAllUserMgmtListByCompanyId(Guid ComId, int? UserGroup, string currentemp, string grpname, string searchText)
        {
            string UserGroupFilterQuery = "";
            string subquery = "";
            string SearchBySql = "";

            if (!string.IsNullOrWhiteSpace(currentemp) && currentemp.ToLower() != "all")
            {
                if (currentemp == "1")
                {
                    subquery = string.Format("and emp.IsCurrentEmployee = 1");
                }
                else
                {
                    subquery = string.Format("and (emp.IsCurrentEmployee = 0 or emp.IsCurrentEmployee is null or pg.Name = 'Old Employees')");
                }
            }
            string DeleteEmployeQuery = " and ul.IsDeleted = 0";
            if (UserGroup.HasValue && UserGroup > 0)
            {

                if (UserGroup == 404)
                {
                    DeleteEmployeQuery = " and ul.IsDeleted = 1";
                }
                else
                {
                    UserGroupFilterQuery = string.Format("and pg.Id ={0}", UserGroup);
                    DeleteEmployeQuery = " and ul.IsDeleted = 0";
                }

            }
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                SearchBySql = @"and(emp.FirstName + ' ' + emp.LastName like '{0}%'
                or emp.LastName like '{0}%' or emp.Email like '%{0}%' or emp.Phone like '%{0}%')";

                SearchBySql = string.Format(SearchBySql, searchText);

            }
            string sqlQuery = @"
                                select 
                                distinct ul.Id as Id,
                                ul.UserId,ul.IsDeleted,
                                emp.Email as Email,
                                ul.IsActive ,
                                emp.FirstName +' '+emp.LastName as ContactName,
                                emp.Recruited as IsRecruited,
                                pg.Name as AccessRights,
                                pg.Tag as Tags,
								emp.IsCalendar,
								emp.CalendarColor,
                                emp.IsCurrentEmployee,
                                CASE   
                                WHEN emp.IsActive = 1 THEN 'Active'  
								WHEN emp.IsDeleted = 1 THEN 'Deleted'  
                                else 'Invited'  
                                END  [Status],  
                               CASE   
                               WHEN emp.IsCurrentEmployee = 1 THEN 'Yes'  
                               else 'No'  
                                END  CurrentEmployee,
                                (select top(1) FirstName +' '+LastName from Employee where CONVERT(nvarchar(50),UserId) = emp.Supervisorid) as Supervisor

                                from UserCompany uc
                                 left join UserLogin ul 
	                                on ul.UserId = uc.UserId and ul.CompanyId = uc.CompanyId
                                 left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                Left Join Employee emp
									on emp.UserId = ul.UserId
                                where ul.Id is not null
                                   {2}
	                                and pg.Name is not null
                                    and emp.FirstName is not null 
									and emp.LastName is not null
                                    and emp.Recruited =1 
                                    and uc.CompanyId ='{0}'
                                    and emp.CompanyId = '{0}'
                                    {1}
                                    {3}
                                    {4}
                                order by ContactName asc
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ComId, UserGroupFilterQuery, DeleteEmployeQuery, subquery, SearchBySql);
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
        public DataTable GetAllIsCurrentUserMgmtListByCompanyId(Guid ComId, int? UserGroup, string currentemp, string grpname, string searchText)
        {
            string UserGroupFilterQuery = "";
            string subquery = "";
            string SearchBySql = "";
            string GroupNameSql = "";


            string DeleteEmployeQuery = " and ul.IsDeleted = 0";
            if (UserGroup.HasValue && UserGroup > 0)
            {

                if (UserGroup == 404)
                {
                    DeleteEmployeQuery = " and ul.IsDeleted = 1";
                }
                else
                {
                    UserGroupFilterQuery = string.Format("and pg.Id ={0}", UserGroup);
                    DeleteEmployeQuery = " and ul.IsDeleted = 0";
                }

            }
            if (!string.IsNullOrWhiteSpace(grpname))
            {
                GroupNameSql = string.Format("and pg.Name = '{0}'", grpname);
            }
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                SearchBySql = @"and(emp.FirstName + ' ' + emp.LastName like '{0}%'
                or emp.LastName like '{0}%' or emp.Email like '%{0}%' or emp.Phone like '%{0}%')";

                SearchBySql = string.Format(SearchBySql, searchText);

            }
            string sqlQuery = @"
                                select 
                                distinct ul.Id as Id,
                                ul.UserId,ul.IsDeleted,
                                emp.Email as Email,
                                ul.IsActive ,
                                emp.FirstName +' '+emp.LastName as ContactName,
                                emp.Recruited as IsRecruited,
                                pg.Name as AccessRights,
                                pg.Tag as Tags,
								emp.IsCalendar,
								emp.CalendarColor,
                                emp.IsCurrentEmployee,
                                CASE   
                                WHEN emp.IsActive = 1 THEN 'Active'  
								WHEN emp.IsDeleted = 1 THEN 'Deleted'  
                                else 'Invited'  
                                END  [Status],  
                               CASE   
                               WHEN emp.IsCurrentEmployee = 1 THEN 'Yes'  
                               else 'No'  
                                END  CurrentEmployee,
                                (select top(1) FirstName +' '+LastName from Employee where CONVERT(nvarchar(50),UserId) = emp.Supervisorid) as Supervisor

                                from UserCompany uc
                                 left join UserLogin ul 
	                                on ul.UserId = uc.UserId and ul.CompanyId = uc.CompanyId
                                 left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                Left Join Employee emp
									on emp.UserId = ul.UserId
                                where ul.Id is not null
                                   {2}
	                                and pg.Name is not null
                                    and emp.FirstName is not null 
									and emp.LastName is not null
                                    and emp.Recruited =1 
                                    and emp.IsCurrentEmployee = 1
                                    and uc.CompanyId ='{0}'
                                    and emp.CompanyId = '{0}'
                                    {1}
                                    {3}
                                    {4}
                                    {5}
                                order by ContactName asc
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ComId, UserGroupFilterQuery, DeleteEmployeQuery, subquery, SearchBySql, GroupNameSql);
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
        public DataSet GetAllUserMgmtListByCompanyId(Guid ComId, int? UserGroup, string currentemp, string grpname, int pageNo, int pageSize, string SearchText, Guid userid, string Order)
        {
            string UserGroupFilterQuery = "";
            string subquery = "";
            string SearchBySql = "";
            string UserQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchBySql = @"and(emp.FirstName + ' ' + emp.LastName like '{0}%'
                or supervisor.FirstName + ' ' + supervisor.LastName like '{0}%' or emp.LastName like '{0}%' or emp.Email like '%{0}%' or emp.Phone like '%{0}%')";

                SearchBySql = string.Format(SearchBySql, SearchText);

            }
            if (!string.IsNullOrWhiteSpace(currentemp) && currentemp.ToLower() != "all")
            {
                if (currentemp == "1")
                {
                    subquery = string.Format("and emp.IsCurrentEmployee = 1");
                }
                else
                {
                    subquery = string.Format("and (emp.IsCurrentEmployee = 0 or emp.IsCurrentEmployee is null or pg.Name = 'Old Employees')");
                }
            }
            string DeleteEmployeQuery = " and ul.IsDeleted = 0";
            if (UserGroup.HasValue && UserGroup > 0)
            {

                if (UserGroup == 404)
                {
                    DeleteEmployeQuery = " and ul.IsDeleted = 1";
                }
                else
                {
                    UserGroupFilterQuery = string.Format("and pg.Id ={0}", UserGroup);
                    DeleteEmployeQuery = " and ul.IsDeleted = 0";
                }

            }

            if (userid != new Guid())
            {
                UserQuery = string.Format("and ul.UserId = '{0}'", userid);
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(Order) && Order != "null")
            {
                if (Order == "ascending/ContactName")
                {
                    orderquery = "order by #ud.ContactName asc";
                    orderquery1 = "order by ContactName asc";
                }
                else if (Order == "descending/ContactName")
                {
                    orderquery = "order by #ud.ContactName desc";
                    orderquery1 = "order by ContactName desc";
                }
                else if (Order == "ascending/RouteList")
                {
                    orderquery = "order by #ud.RouteList asc";
                    orderquery1 = "order by RouteList asc";
                }
                else if (Order == "descending/RouteList")
                {
                    orderquery = "order by #ud.RouteList desc";
                    orderquery1 = "order by RouteList desc";
                }
                else if (Order == "ascending/Supervisor")
                {
                    orderquery = "order by #ud.Supervisor asc";
                    orderquery1 = "order by Supervisor asc";
                }
                else if (Order == "descending/Supervisor")
                {
                    orderquery = "order by #ud.Supervisor desc";
                    orderquery1 = "order by Supervisor desc";
                }
                else if (Order == "ascending/Email")
                {
                    orderquery = "order by #ud.Email asc";
                    orderquery1 = "order by Email asc";
                }
                else if (Order == "descending/Email")
                {
                    orderquery = "order by #ud.Email desc";
                    orderquery1 = "order by Email desc";
                }
                else if (Order == "ascending/UserRole")
                {
                    orderquery = "order by #ud.AccessRights asc";
                    orderquery1 = "order by AccessRights asc";
                }
                else if (Order == "descending/UserRole")
                {
                    orderquery = "order by #ud.AccessRights desc";
                    orderquery1 = "order by AccessRights desc";
                }
                else if (Order == "ascending/Status")
                {
                    orderquery = "order by #ud.IsActive asc";
                    orderquery1 = "order by IsActive asc";
                }
                else if (Order == "descending/Status")
                {
                    orderquery = "order by #ud.IsActive desc";
                    orderquery1 = "order by IsActive desc";
                }
                else if (Order == "ascending/CurrentEmployee")
                {
                    orderquery = "order by #ud.IsCurrentEmployee asc";
                    orderquery1 = "order by IsCurrentEmployee asc";
                }
                else if (Order == "descending/CurrentEmployee")
                {
                    orderquery = "order by #ud.IsCurrentEmployee desc";
                    orderquery1 = "order by IsCurrentEmployee desc";
                }
                else
                {
                    orderquery = "order by #ud.ContactName asc";
                    orderquery1 = "order by ContactName asc";
                }
            }
            else
            {
                orderquery = "order by #ud.ContactName asc";
                orderquery1 = "order by ContactName asc";
            }
            #endregion
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize

                                select 
                                distinct ul.Id as Id,
                                ul.UserId,ul.IsDeleted,
                                emp.Email as Email,
                                ul.IsActive ,
                                emp.FirstName +' '+emp.LastName as ContactName,
                                emp.Recruited as IsRecruited,
                                pg.Name as AccessRights,
                                pg.Tag as Tags,
								emp.IsCalendar,
                                ISNULL(lkemptype.DisplayText,'') as EmployeeType,
								emp.CalendarColor,
                                emp.IsCurrentEmployee
                                ,supervisor.FirstName +' '+supervisor.LastName as Supervisor
                                --,(select top(1) FirstName +' '+LastName from Employee where CONVERT(nvarchar(50),UserId) = emp.Supervisorid) as Supervisor
                                ,(STUFF((
										SELECT ', '  + GR.Name + ' '
										FROM EmployeeRoute  ER
										left join GeeseRoute GR on GR.RouteId = ER.RouteId
										where ER.UserId = emp.userid
										
										FOR XML PATH('')
										), 1, 2, '')
									) AS RouteList

                                into #UserData
                                from UserCompany uc
                                 left join UserLogin ul 
	                                on ul.UserId = uc.UserId and ul.CompanyId = uc.CompanyId
                                 left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                Left Join Employee emp
									on emp.UserId = ul.UserId
                                Left Join LookUp lkemptype
                                    on lkemptype.DataValue=emp.EmpType and lkemptype.DataKey='EmployeeTypeData'
                                Left Join Employee supervisor
									on CONVERT(nvarchar(50),supervisor.UserId) = emp.Supervisorid

                                where ul.Id is not null
                                   {2}
	                                and pg.Name is not null
                                    and emp.FirstName is not null 
									and emp.LastName is not null
                                    and emp.Recruited =1 
                                    and uc.CompanyId ='{0}'
                                    and emp.CompanyId = '{0}'
                                    {1}
                                    {3}
                                    {4}
                                    {5}
                                select * into #UserFilterdata from #UserData

								select top(@pagesize) * from #UserFilterdata
								where Id not in (Select TOP (@pagestart)  Id from #UserData #ud {6}) --order by #ud.ContactName asc
                                --order by ContactName asc
								{7}

                                select Count(Id) As TotalCount from #UserFilterdata 

								drop table #UserData
								drop table #UserFilterdata
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ComId, UserGroupFilterQuery, DeleteEmployeQuery, subquery, SearchBySql, UserQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageNo));
                    AddParameter(cmd, pInt32("pagesize", pageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteUserLoginByUsername(string username)
        {
            string sqlQuery = @" delete from userlogin where 
                                    UserName = '{0}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, username);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateAllUserNameByUserName(string oldUserName, string newUserName)
        {
            string sqlQuery = @"Declare @OldUserName nvarchar(50)
                                Declare @NewUserName nvarchar(50)

                                set @OldUserName = '{0}'
                                set @NewUserName = '{1}'

                                --CreatedBy
                                update CustomerAppointmentEquipment  set CreatedBy = @NewUserName where CreatedBy = @OldUserName 
                                update [Inventory]					 set CreatedBy = @NewUserName where CreatedBy = @OldUserName 
                                update Invoice						 set CreatedBy = @NewUserName where CreatedBy = @OldUserName 
                                update [InvoiceDetail]				 set CreatedBy = @NewUserName where CreatedBy = @OldUserName 
                                update [MarchantInvoice]			 set CreatedBy = @NewUserName where CreatedBy = @OldUserName 


                                --CreatedBy & LastUpdatedBy
                                update CustomerAppointment  set CreatedBy = @NewUserName where CreatedBy = @OldUserName
                                update CustomerAppointment  set LastUpdatedBy = @NewUserName where LastUpdatedBy = @OldUserName
                                --select CreatedBy , LastUpdatedBy from CustomerAppointment

                                --UpdatedBy
                                update CustomerBill set UpdatedBy  = @NewUserName where UpdatedBy = @OldUserName
                                update Fund			set UpdatedBy  = @NewUserName where UpdatedBy = @OldUserName
                                update SupplierBill	set UpdatedBy  = @NewUserName where UpdatedBy = @OldUserName

                                --UserName
                                update Employee				set UserName  = @NewUserName where UserName = @OldUserName 
                                update Company				set UserName  = @NewUserName where UserName = @OldUserName 
                                update CredentialSetting	set UserName  = @NewUserName where UserName = @OldUserName 
                                update HrDoc				set UserName  = @NewUserName where UserName = @OldUserName 
                                update UserBranch			set UserName  = @NewUserName where UserName = @OldUserName 
                                update UserCompany			set UserName  = @NewUserName where UserName = @OldUserName 
                                update UserPermission		set UserName  = @NewUserName where UserName = @OldUserName 
							 			  
                                --LastUpdatedBy	 
                                update  Bundle				 set LastUpdatedBy  = @NewUserName where LastUpdatedBy = @OldUserName 
                                update  BundleEquipment		 set LastUpdatedBy  = @NewUserName where LastUpdatedBy = @OldUserName 
                                update  Equipment			 set LastUpdatedBy  = @NewUserName where LastUpdatedBy = @OldUserName 
                                update  EquipmentType		 set LastUpdatedBy  = @NewUserName where LastUpdatedBy = @OldUserName 
                                update  NewsletterSubscribe	 set LastUpdatedBy  = @NewUserName where LastUpdatedBy = @OldUserName 
                                update  UserLogin			 set LastUpdatedBy  = @NewUserName where LastUpdatedBy = @OldUserName 

                                --LastVisitedBy
                                update CustomerView set LastVisitedBy = @NewUserName where LastVisitedBy = @OldUserName 
                                --AddedBy
                                update  InvoiceNote		set	AddedBy  = @NewUserName where AddedBy = @OldUserName 
                                update  PaymentRevenue	set	AddedBy  = @NewUserName where AddedBy = @OldUserName
                                update  [Transaction]	set	AddedBy  = @NewUserName where AddedBy = @OldUserName
					  ";
            try
            {
                sqlQuery = string.Format(sqlQuery, oldUserName, newUserName);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public UserLogin GetUserByUsername(string name, Guid comid)
        {
            string subquery = "";
            if(comid != new Guid())
            {
                subquery = string.Format("and uc.CompanyId = '{0}' and com.CompanyId = '{0}' and ul.CompanyId = '{0}'", comid);
            }
            string sqlQuery = @"select distinct ul.* 
                                --,emp.FirstName
                                ,case when pg.Tag = 'Customer'
	                                Then cu.FirstName
	                                Else emp.FirstName
                                end as FirstName
                                --,emp.LastName
                                ,case when pg.Tag = 'Customer'
	                                Then cu.LastName
	                                Else emp.LastName
                                end as LastName
                                --,emp.Email as EmailAddress
                                ,case when pg.Tag = 'Customer'
	                                Then cu.EmailAddress
	                                Else emp.Email
                                end as EmailAddress
                                --,emp.Phone as PhoneNumber
                                ,case when pg.Tag = 'Customer'
	                                Then cu.PrimaryPhone
	                                Else emp.Phone
                                end as PhoneNumber
                                --,uc.CompanyId as DefaultCompanyId
                                ,case when pg.Tag = 'Customer'
	                                Then cc.CompanyId
	                                Else uc.CompanyId
                                end as DefaultCompanyId
                                --,com.CompanyName as CompanyName
                                ,case when pg.Tag = 'Customer'
	                                Then CCom.CompanyName
	                                Else com.CompanyName
                                end as CompanyName

                                ,pg.Name as UserType
                                ,pg.Tag as UserTags
                                ,(select top(1) 
								CASE WHEN etc.ClockOutTime is not null
								THEN 'Clock Out'
								ELSE 'Clock In'
								end as Type
								 from EmployeeTimeClock etc where etc.UserId  = ul.UserId order by [ClockInTime] desc) as ClockedInOutStatus
								 ,(select top(1) 
								CASE WHEN etc.ClockOutTime is not null
								THEN etc.ClockOutTime
								ELSE etc.ClockInTime
								end as Time
								 from EmployeeTimeClock etc where etc.UserId  = ul.UserId order by [ClockInTime] desc) as ClockedInOutTime
                                ,ISNULL(emp.IsSupervisor,0) IsSupervisor

                                from UserLogin ul 
                                left join UserCompany uc on ul.UserId = uc.UserId
                                left join Company com on com.CompanyId = uc.CompanyId
                                left join Employee emp on emp.UserId = ul.UserId

                                --Added later starts
                                left join Customer cu on cu.CustomerId = ul.UserId
                                left join CustomerCompany cc on cc.CustomerId = cu.CustomerId
                                left join Company CCom on Ccom.CompanyId = cc.CompanyId
                                --Ends

                                left join UserPermission up 
                                on up.UserId = ul.userid and up.PermissionGroupId is not null

                                left join PermissionGroup Pg 
                                on Pg.Id = up.PermissionGroupId and pg.Id is not null
                                where ul.UserName = '{0}'
                                {2}";

            UserLogin UserLogin = null;
            try
            {
                sqlQuery = string.Format(sqlQuery, name, comid, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            UserLogin = new UserLogin();
                            FillObject(UserLogin, reader);
                            UserLogin.FirstName = reader["FirstName"].ToString();
                            UserLogin.LastName = reader["LastName"].ToString();
                            UserLogin.EmailAddress = reader["EmailAddress"].ToString();
                            UserLogin.PhoneNumber = reader["PhoneNumber"].ToString();
                            UserLogin.CompanyName = reader["CompanyName"].ToString();
                            UserLogin.DefaultCompanyId = (Guid) reader["DefaultCompanyId"];
                            UserLogin.UserTags = reader["UserTags"].ToString();
                            UserLogin.UserType = reader["UserType"].ToString();
                            UserLogin.ClockedInOutStatus = reader["ClockedInOutStatus"].ToString();
                            if (reader["IsSupervisor"].GetType() == typeof(DBNull))
                            {
                                UserLogin.IsSupervisor = false;
                            }
                            else
                            {
                                bool IsSupervisor = false; 
                                bool.TryParse(reader["IsSupervisor"].ToString(), out IsSupervisor);
                                UserLogin.IsSupervisor = IsSupervisor;

                            }
                            UserLogin.ClockedInOutTime = new DateTime();

                            if (reader["ClockedInOutTime"] == DBNull.Value)
                            {
                                UserLogin.ClockedInOutTime = new DateTime();
                            }
                            else
                            {
                                DateTime ClockedInOutTime = new DateTime();
                                DateTime.TryParse(reader["ClockedInOutTime"].ToString(), out ClockedInOutTime);
                                UserLogin.ClockedInOutTime = ClockedInOutTime;
                            }

                            if (string.IsNullOrWhiteSpace(UserLogin.UserType))
                            {
                                UserLogin.UserType = "none";
                            }

                            return UserLogin;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return UserLogin;
            }
        }
    }	
}
