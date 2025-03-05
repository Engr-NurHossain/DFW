using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;

namespace HS.DataAccess
{
	public partial class ContactDataAccess
	{
        public ContactModel GetContacts(ContactFilter filter)
        {
            string searchTextQuery = "";
            string subquery = "";

            string ContactOwnerIdFilter = "";
         
            string CountFilter = "";
            string filterQuery = "";
            string tagquery = "";
          
          
            if (!string.IsNullOrEmpty(filter.Mobile))
            {
                filterQuery += string.Format(" c.Mobile = '{0}' and", filter.Mobile);

            }
            if (!string.IsNullOrEmpty(filter.Work) )
            {
                filterQuery += string.Format(" c.Work = '{0}' and", filter.Work);

            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                filterQuery += string.Format(" c.Email = '{0}' and", filter.Email);

            }
            if (filter.DateFrom != new DateTime() && filter.DateTo != new DateTime())
            {
                filterQuery += string.Format(" c.CreatedDate between '{0}' and '{1}' and", filter.DateFrom.SetZeroHour().ClientToUTCTime(), filter.DateTo.SetMaxHour().ClientToUTCTime());

            }
            if(filter.ContactsList  != null && filter.ContactsList.Count > 0)
            {
                filterQuery += string.Format(" c.ContactId IN('{0}') and", string.Join("','",filter.ContactsList));
            }
        
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = string.Format(" (c.FirstName+' '+c.LastName like '%{0}%' or c.LastName like '%{0}%' or c.Work like '%{0}%' or c.Mobile like '%{0}%' or c.email like '%{0}%'  ) AND",filter.SearchText);
            }
            if(filter.ContactOwnerId != null  && filter.ContactOwnerId != new Guid())
            {
           
                ContactOwnerIdFilter = string.Format("ContactOwner = '{0}' and", filter.ContactOwnerId);
                CountFilter = string.Format(" where ContactOwner = '{0}'", filter.ContactOwnerId);

            }
            if (!string.IsNullOrWhiteSpace(filter.Identifier))
            {
                tagquery = string.Format("and map.TagId = '{0}'", filter.Identifier);
            }
            List<Contact> ContactList = new List<Contact>();
            ContactCount TotalContact = new ContactCount();
            string rawQuery = @"declare @pagesize int
                                declare @pageno int 
                                set @pagesize = "+filter.UnitPerPage+@"
                                set @pageno = "+filter.PageNumber+ @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  
                                 select  * into #temptable from (select distinct c.Id,
                                 c.ContactId,
								 c.ContactOwner,
								 c.FirstName,
								 c.LastName,
								 c.Work,
								 c.Mobile,
                                 c.Email,
								 c.CreatedDate,
                                 c.Ext
                                ,c.FirstName+' '+c.LastName as Name
                                ,c.CreatedBy
                                ,emp.FirstName+' '+emp.LastName as CreatedByName  FROM Contact c
                                left join employee emp on emp.UserId = c.CreatedBy
                                left join RMRTagMap map on map.ContactId = c.ContactId
								where  {1}{2}{3} c.Id > 0
                                {4}
								) a

								select * into #ftemp from #temptable 

                                select TOP (@pagesize) *  from #ftemp f
                                where  f.Id NOT IN(Select TOP (@pagestart) Id from #ftemp  order by Id desc)
                                {0}
                                select Count(Id) As TotalCount from #ftemp  

								drop table #temptable
								drop table #ftemp";



            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/name")
                {
                    subquery = "order by FirstName asc";
              
                }
                else if (filter.Order == "descending/name")
                {
                    subquery = "order by FirstName desc";
                
                }
                else if (filter.Order == "ascending/lastname")
                {
                    subquery = "order by lastname asc";
            
                }
                else if (filter.Order == "descending/lastname")
                {
                    subquery = "order by lastname desc";
                   
                }
                else if (filter.Order == "ascending/work")
                {
                    subquery = "order by work asc";
                 
                }
                else if (filter.Order == "descending/work")
                {
                    subquery = "order by work desc";
               
                }
                else if (filter.Order == "ascending/mobile")
                {
                    subquery = "order by mobile asc";
                 
                }
                else if (filter.Order == "descending/mobile")
                {
                    subquery = "order by mobile desc";
                 
                }
                else if (filter.Order == "ascending/email")
                {
                    subquery = "order by email asc";
                    
                }
                else if (filter.Order == "descending/email")
                {
                    subquery = "order by email desc";
              
                }
                else if (filter.Order == "ascending/createdby")
                {
                    subquery = "order by emp.FirstName asc";

                }
                else if (filter.Order == "descending/createdby")
                {
                    subquery = "order by  emp.FirstName desc";

                }
                else if (filter.Order == "ascending/createddate")
                {
                    subquery = "order by CreateDate asc";

                }
                else if (filter.Order == "descending/createddate")
                {
                    subquery = "order by  CreateDate desc";

                }
                else if (filter.Order == "ascending/ext")
                {
                    subquery = "order by Ext asc";

                }
                else if (filter.Order == "descending/ext")
                {
                    subquery = "order by  Ext desc";

                }
            }
            else
            {
                subquery = "order by Id desc";
              
            }
            #endregion

            rawQuery = string.Format(rawQuery, subquery,searchTextQuery, ContactOwnerIdFilter, filterQuery, tagquery);
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
        

                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];
                DataTable dt1 = dsResult.Tables[1];
                try
                {
                    ContactList = (from DataRow dr in dt.Rows
                               select new Contact()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   ContactId = dr["ContactId"] !=DBNull.Value ? (Guid)dr["ContactId"] : Guid.Empty,
                                   CreatedBy = dr["CreatedBy"] != DBNull.Value ? (Guid)dr["CreatedBy"] : Guid.Empty,
                                   FirstName = dr["FirstName"].ToString(),
                                   LastName = dr["LastName"].ToString(),
                                   Work = dr["Work"].ToString(),
                                   Mobile = dr["Mobile"].ToString(),
                                   Email = dr["Email"].ToString(),
                                   Name = dr["Name"].ToString(),
                                   CreatedByName = dr["CreatedByName"].ToString(),
                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]): new DateTime(),
                                   Ext = dr["Ext"].ToString(),
                               }).ToList();

                   
                    TotalContact = (from DataRow dr in dt1.Rows
                                     select new ContactCount()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
                   
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            ContactModel contact = new ContactModel();
            contact.ContactList = ContactList;
            contact.TotalCount = TotalContact;
            return contact;
        }

        public List<Contact> GetFilteredContacts(ContactFilter filter)
        {
            string searchTextQuery = "";
       
            string filterQuery = "";


            if (!string.IsNullOrEmpty(filter.Mobile))
            {
                filterQuery += string.Format(" c.Mobile = '{0}' and", filter.Mobile);

            }
            if (!string.IsNullOrEmpty(filter.Work))
            {
                filterQuery += string.Format(" c.Work = '{0}' and", filter.Work);

            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                filterQuery += string.Format(" c.Email = '{0}' and", filter.Email);

            }
            if (filter.DateFrom != new DateTime() && filter.DateTo != new DateTime())
            {
                filterQuery += string.Format(" c.CreatedDate between '{0}' and '{1}' and", filter.DateFrom, filter.DateTo);

            }
            if (filter.ContactsList != null && filter.ContactsList.Count > 0)
            {
                filterQuery += string.Format(" c.ContactId IN('{0}') and", string.Join("','", filter.ContactsList));
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = " where (c.FirstName+' '+c.LastName like @SearchText or c.LastName like @SearchText or c.Work like @SearchText or c.Mobile like @SearchText or c.email like @SearchText ) AND";
            }
            if(searchTextQuery== "" && filterQuery != "")
            {
                filterQuery = "Where " + filterQuery;
            }
           
 
            string rawQuery = @"  select c.*
                                    ,c.FirstName+' '+c.LastName as Name
                                    ,emp.FirstName+' '+emp.LastName as CreatedByName  FROM Contact c
                                    left join employee emp on emp.UserId = c.CreatedBy
                                    {0}{1}";

            rawQuery = string.Format(rawQuery, searchTextQuery, filterQuery);
            if(filterQuery != "" || searchTextQuery != "")
            {
                rawQuery = rawQuery.Remove(rawQuery.Length - 4);
            }
            List<Contact> ContactList = new List<Contact>();
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));

                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];
      
                try
                {
                    ContactList = (from DataRow dr in dt.Rows
                                   select new Contact()
                                   {
                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                       FirstName = dr["FirstName"].ToString(),
                                       LastName = dr["LastName"].ToString(),
                                       Work = dr["Work"].ToString(),
                                       Mobile = dr["Mobile"].ToString(),
                                       Email = dr["Email"].ToString(),
                                       Name = dr["Name"].ToString(),
                                       CreatedByName = dr["CreatedByName"].ToString(),
                                       CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                   }).ToList();

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
           
            return ContactList;
        }

        public List<Contact> GetContactsBySearchKey(string key, string employeeTag, Guid empid, string type)
        {
            string searchTextQuery = ""; 
            if(!string.IsNullOrWhiteSpace(type) && type.ToLower() == "tag")
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    searchTextQuery = "left join RMRTagMap map on map.ContactId = c.ContactId left join RMRTag tag on tag.TagIdentifier = map.TagId where tag.TagName like @SearchText";
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    searchTextQuery = " where (c.FirstName+' '+c.LastName like @SearchText or c.LastName like @SearchText or c.Work like @SearchText or c.Mobile like @SearchText or c.email like @SearchText )";
                }
            }
            
            string rawQuery = @"  select c.*
                                    ,c.FirstName+' '+c.LastName as Name
                                    ,emp.FirstName+' '+emp.LastName as CreatedByName  FROM Contact c
                                    left join employee emp on emp.UserId = c.CreatedBy
                                    {0}";

            rawQuery = string.Format(rawQuery, searchTextQuery); 
            List<Contact> ContactList = new List<Contact>();
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", key)));

                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];

                try
                {
                    ContactList = (from DataRow dr in dt.Rows
                                   select new Contact()
                                   {
                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                       FirstName = dr["FirstName"].ToString(),
                                       LastName = dr["LastName"].ToString(),
                                       Work = dr["Work"].ToString(),
                                       Mobile = dr["Mobile"].ToString(),
                                       Email = dr["Email"].ToString(),
                                       Name = dr["Name"].ToString(),
                                       CreatedByName = dr["CreatedByName"].ToString(),
                                       CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                   }).ToList();

                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return ContactList;
        }

        public DataTable GetAllContactList(string query)
        {
            string sqlQuery = @"SELECT *
                                FROM Contact
                                 
									WHERE (FirstName +' '+ LastName) like '%{0}%' OR FirstName like '%{0}%' OR LastName like '%{0}%' 
                                       
                                 ";


            try
            {
                sqlQuery = string.Format(sqlQuery, query);
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

        public DataTable GetAllUserContactListByContactId(Guid ContactId)
        {
            string sqlQuery = @"select UC.*, 

                                ISNULL(emp.FirstName,cu.FirstName) as UserFirstName,
                                ISNULL(emp.LastName,cu.LastName) as UserLastName,
                                ISNULL(cu.BusinessName,cu.BusinessName) as UserBusinessName,
                                opp.OpportunityName ,
                                ISNULL(emp.Id,ISNULL(cu.Id ,opp.Id)) as UserIntId

                                from UserContact UC
                                left join Customer cu on UC.UserId = cu.CustomerId
                                left join Opportunity opp on opp.OpportunityId = UC.UserId
                                left join Employee emp on emp.UserId = UC.UserId

                                where uc.ContactId = '{0}'";


            try
            {
                sqlQuery = string.Format(sqlQuery, ContactId);
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

        public bool ExistEmailorCellNo(string Email,string Mobile,string work)
        {
            List<Contact> contactList = new List<Contact>();
            var filterMobile = "";
            var filterWork = "";
            string rawQuery = @"  
                                select  Id,FirstName,LastName FROM Contact  
                                where Email = '{0}' and (replace(Mobile,'-','') = '{1}' and  replace(Work,'-','') = '{2}' )
                                ";
            if (!string.IsNullOrEmpty(Mobile))
            {
                filterMobile = Mobile.Replace("-", "");
            }
            if (!string.IsNullOrEmpty(work))
            {
                filterWork = work.Replace("-", ""); ;
            }
            rawQuery = string.Format(rawQuery, Email, filterMobile, filterWork);
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];
              
                try
                {
                    contactList = (from DataRow dr in dt.Rows
                                   select new Contact()
                                   {
                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                       FirstName = dr["FirstName"].ToString(),
                                       LastName = dr["LastName"].ToString(),
                                       
                                   }).ToList();
                    if(contactList.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        
        }

        public bool DeleteUserContactByContactId(Guid ContactId)
        {
            string SqlQuery = @"
                            delete from UserContact
                            where ContactId = '{0}' ";
            SqlQuery = string.Format(SqlQuery, ContactId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
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

        public DataTable GetAllContactForExport()
        {
            string sqlQuery = @"select   c.FirstName,c.LastName,c.Suffix,c.Title,dbo.PhoneNumFormat(c.Work),c.Ext,
                                dbo.PhoneNumFormat(c.Mobile),c.Email,c.Role,c.Facebook,c.Twitter,c.Instagram,c.LinkedIN,c.Notes,
                                c.FirstName+' '+c.LastName as Name,emp.FirstName+' '+emp.LastName as CreatedBy,
                                empContact.FirstName+' '+empContact.LastName as ContactOwner  FROM Contact c
                                left join employee emp on emp.UserId = c.CreatedBy
                                left join Employee empContact on empContact.UserId = c.ContactOwner";
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

        public DataTable GetAllContactDatabaseForExport()
        {
            string sqlQuery = @"select * from Contact";
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
        public DataTable GetAllContactListByIds(string IdList)
        {
            string rawQuery = @" select  c.*,c.FirstName+' '+c.LastName as Name,emp.FirstName+' '+emp.LastName as CreatedByName  from Contact c
                                left join employee emp on emp.UserId = c.CreatedBy
                                 where c.Id in ({0}) 
                               ";
            try
            {
                rawQuery = string.Format(rawQuery, IdList);
                using (SqlCommand cmd = GetSQLCommand(rawQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    DataTable dt = dsResult.Tables[0];
                    return dt;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetContactInfoById(int Id)
        { 
            string rawQuery = @" select c.*                             
                                    ,emp.FirstName+' '+emp.LastName as ContactOwnerVal  FROM Contact c
                                    left join employee emp on emp.UserId = c.ContactOwner
                                     where c.Id = "+Id;
                               
            try
            {
               
                using (SqlCommand cmd = GetSQLCommand(rawQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    DataTable dt = dsResult.Tables[0];
                    return dt;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllContactListByContactIds(string IdList)
        {
            string rawQuery = @" select  c.*,c.FirstName+' '+c.LastName as Name,emp.FirstName+' '+emp.LastName as CreatedByName  from Contact c
                                left join employee emp on emp.UserId = c.CreatedBy
                                 where c.ContactId in ({0}) 
                               ";
            try
            {
                rawQuery = string.Format(rawQuery, IdList);
                using (SqlCommand cmd = GetSQLCommand(rawQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    DataTable dt = dsResult.Tables[0];
                    return dt;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}
