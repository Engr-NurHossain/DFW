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
	public partial class EmergencyContactDataAccess
	{
        public EmergencyContactDataAccess(string ConStr) : base(ConStr) { }

        public bool ReseedEmergencyContactTable()
        {
            string SqlQuery = @"Delete from EmergencyContact  
                                DBCC CHECKIDENT('EmergencyContact', RESEED, 0)
                                ";
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
        public bool UpdateEmergencyContact(int Id, string ColumnName, string NewValue)
        {
            string sqlQuery = @"Update EmergencyContact
                                Set {1} = '{2}' where Id = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, Id, ColumnName, NewValue);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public DataTable GetAllLeadEmergencyDetailByLeadIdandCompanyId(Guid companyId, int id)
        {
            string sqlQuery = @"
								select ec.FirstName+' '+ec.LastName as ContactName, ec.RelationShip as ContactRelation,
								ec.Phone as ContactPhone, ec.HasKey as ContactHaskey
								from EmergencyContact ec
								join customer cs
								on cs.CustomerId = ec.CustomerId
								join CustomerCompany cc
								on cc.CustomerId = cs.CustomerId
								where cc.CompanyId = '{0}'
                                and cc.IsLead = 1
                                and cs.Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, id);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetAllCustomerEmergencyDetailByLeadIdandCompanyId(Guid companyId, int id)
        {
            string sqlQuery = @"
								select ec.FirstName+' '+ec.LastName as ContactName, ec.RelationShip as ContactRelation,
								ec.Phone as ContactPhone, ec.HasKey as ContactHaskey
								from EmergencyContact ec
								join customer cs
								on cs.CustomerId = ec.CustomerId
								join CustomerCompany cc
								on cc.CustomerId = cs.CustomerId
								where cc.CompanyId = '{0}'
                                and cs.Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, id);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetAllEmergencyContactByCustomerId(Guid CustomerId)
        {
            string sqlQuery = @"
								select ec.*,lkrelation.DisplayText as RelationShipVal from EmergencyContact ec
                                left join Lookup lkrelation on lkrelation.DataValue = ec.RelationShip and lkrelation.DataKey = 'Relationship'
                                where CustomerId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetAllEmergencyContactByCustomerIdAndPlatform(Guid CustomerId,string Platform)
        {
            var RelationKey = "Relationship";
            if(!string.IsNullOrEmpty(Platform) && Platform == "'NMC'")
            {
                RelationKey = "NMCRelationship";
            }
            string sqlQuery = @"
								select ec.*,lkrelation.DisplayText as RelationShipVal from EmergencyContact ec
                                left join Lookup lkrelation on lkrelation.DataValue = ec.RelationShip and lkrelation.DataKey = '{1}'
                                where CustomerId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, RelationKey);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable GetAllEmergencyContactByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            string sqlQuery = @"select 
                                ec.Id,
                                ec.CompanyId,
                                ec.CustomerId,
                                ec.CrossSteet,
                                ec.FirstName,
                                ec.LastName,
                                lprelationship.DisplayText as RelationShip,
                                ec.Email,
                                ec.Phone,
                                ec.HasKey,
                                lpphonetype.DisplayText as PhoneType,
                                ec.OrderBy,
                                ec.ContactNo,
                                ec.Platform
                                from EmergencyContact ec
                                LEFT JOIN Lookup lprelationship on lprelationship.DataValue=ec.RelationShip and lprelationship.DataKey='RelationShip' and ec.RelationShip !='-1'
                                LEFT JOIN Lookup lpphonetype on lpphonetype.DataValue=ec.PhoneType and lpphonetype.DataKey='PhoneType' and ec.PhoneType !='-1'
                                where ec.CustomerId ='{0}' and ec.CompanyId='{1}' 	order by OrderBy";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }	
}
