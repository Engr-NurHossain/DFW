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
	public partial class PayrollTermSheetDataAccess
	{
        public bool SetAllTermSheetIsBaseFalse()
        {
            string sqlQuery = @"update PayrollTermSheet set IsBase=0";
            try
            {
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
        public bool CloneBasePayrollTermSheet(Guid oldTermSheetId,Guid newTermSheetId,Guid createdbyuid,DateTime dateTime)
        {
            string sqlQuery = @"declare @oldTermSheetId uniqueidentifier 
                                set @oldTermSheetId = '{0}'

                                declare @newTermSheetId uniqueidentifier 
                                set @newTermSheetId = '{1}'

                                declare @createdbyuid uniqueidentifier
                                set @createdbyuid = '{2}'

                                declare @datetime datetime
                                set @datetime = '{3}'

                                --PayrollAgreementLength Clone
                                INSERT INTO PayrollAgreementLength ([PayrollAgreementLengthId],[CompanyId],[AgreementLength],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[AgreementLength],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollAgreementLength where TermSheetId = @oldTermSheetId

                                --PayrollCreditRating Clone
                                INSERT INTO PayrollCreditRating ([PayrollCreditRatingId],[CompanyId],[MinCredit],[MaxCredit],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[MinCredit],[MaxCredit],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollCreditRating where TermSheetId = @oldTermSheetId

                                --PayrollCustomerBillingMethod Clone
                                INSERT INTO PayrollCustomerBillingMethod ([PayrollCustomerBillingMethodId],[CompanyId],[BillingMethod],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[BillingMethod],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollCustomerBillingMethod where TermSheetId = @oldTermSheetId

                                --PayrollCustomerType Clone
                                INSERT INTO PayrollCustomerType ([PayrollCustomerTypeId],[CompanyId],[CustomerType],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[CustomerType],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollCustomerType where TermSheetId = @oldTermSheetId

                                --PayrollHoldBack Clone
                                INSERT INTO PayrollHoldBack ([PayrollHoldBackId],[CompanyId],[HoldBack],[Percentage],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[HoldBack],[Percentage],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollHoldBack where TermSheetId = @oldTermSheetId

                                --PayrollInstallationFee Clone
                                INSERT INTO PayrollInstallationFee ([PayrollInstallationFeeId],[CompanyId],[InstallationFeeMin],[InstallationFeeMax],[Amount],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[InstallationFeeMin],[InstallationFeeMax],[Amount],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollInstallationFee where TermSheetId = @oldTermSheetId

                                --PayrollMileage Clone
                                INSERT INTO PayrollMileage ([PayrollMileageId],[CompanyId],[MileageMin],[MileageMax],[Amount],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[MileageMin],[MileageMax],[Amount],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollMileage where TermSheetId = @oldTermSheetId

                                --PayrollMonthlyProductionBonus Clone
                                INSERT INTO PayrollMonthlyProductionBonus ([PayrollMonthlyProductionBonusId],[CompanyId],[MonthlyProductionBonusMin],[MonthlyProductionBonusMax],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[MonthlyProductionBonusMin],[MonthlyProductionBonusMax],[Point],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollMonthlyProductionBonus where TermSheetId = @oldTermSheetId

                                --PayrollPassThrus Clone
                                INSERT INTO PayrollPassThrus ([PayrollPassThrusId],[CompanyId],[PassThrus],[IsBase],[Amount],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[PassThrus],[IsBase],[Amount],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollPassThrus where TermSheetId = @oldTermSheetId

                                --PayrollSingleItemSettings Clone
                                INSERT INTO PayrollSingleItemSettings ([SingleItemSettingsId],[CompanyId],[SearchKey],[SearchValue],[IsActive],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[TermSheetId]) SELECT (select NEWID()),[CompanyId],[SearchKey],[SearchValue],[IsActive],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],@newTermSheetId From PayrollSingleItemSettings where TermSheetId = @oldTermSheetId
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, oldTermSheetId, newTermSheetId, createdbyuid, dateTime);
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
    }	
}
