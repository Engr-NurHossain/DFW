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
    public partial class IndividualInstalledEquipmentDataAccess
    {
        public bool InsertInIndividualInstalledEquipment(Guid CompanyId)
        {
            string SqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%%' 
                                SET @pageno = 1 --default 1
                                SET @pagesize = 20 --default 10
                                SET @CompanyId = '{0}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '00000000-0000-0000-0000-000000000000'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                DECLARE @MyList Table(AppointmentEquipmentId int, Category nvarchar(50),Manufacturer nvarchar(50), [Description] nvarchar(50), TicketType nvarchar(50),
                                EmpUser varchar(50), TicketId int, RepliesCount int, AttachmentsCount int, CusIdInt int, CustomerName nvarchar(50), CompletionDate datetime,
                                SKU nvarchar(50),TotalPoint float, IsClosed bit, CompanyCost float, CustomerCost float, Quantity int, InstalledEquipment int, Qty int, [Status] nvarchar(50), InstalledByUid uniqueidentifier, EquipmentId uniqueidentifier)

                                DECLARE @IdCount int, @qty int, @i int, @j int, @getAppointmentEquipmentId int, @getCategory nvarchar(50), @getManufacturer nvarchar(50),@getDescription nvarchar(50),
                                 @getTicketType nvarchar(50), @getEmpUser nvarchar(50),  @getRepliesCount int, @getAttachmentsCount int, @getCusIdInt int,
                                 @getCustomerName nvarchar(50), @getCompletionDate datetime, @getSKU nvarchar(50), @getTotalPoint float, @getIsClosed bit, @getCompanyCost float,
                                 @getCustomerCost float, @getQuantity int, @getInstalledEquipment int, @getTicketId int, @getQty int, @getStatus nvarchar(50), @InstalledByUid uniqueidentifier, @EquipmentId uniqueidentifier

	                                set @i = 1;
	                                set @j = 1;

	                                --select @IdCount = count(_cae.Id) from  CustomerAppointmentEquipment _cae
										                                --left join ticket tk on _cae.AppointmentId = tk.TicketId
										                                --left join Equipment _eq on _eq.EquipmentId = _cae.EquipmentId
										                                --left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
										                                --left join Manufacturer manu on manu.Id = _eq.ManufacturerId
										                                --left join TicketUser tu on tu.TiketId = tk.TicketId
										                                --left join Employee emp on emp.UserId = _cae.InstalledByUid
                                                                        --left join Lookup lktype on  lktype.DataKey ='TicketType'
                                                                        --left join Customer cus on cus.CustomerId = tk.CustomerId  
                                                                        --and lktype.DataValue = tk.TicketType
										                                --where tk.CompanyId = '{0}'
                                                                        --and _eq.EquipmentClassId=1
										                                --and cus.id is not null
										                                --and _cae.QuantityLeftEquipment > 0
										
	                                Select 
                                      Row_Number() Over (Order By ca.Id DESC) As RowNum
                                     ,ca.Id,Quantity,AppointmentId,ca.EquipmentId,QuantityLeftEquipment,UnitPrice,InstalledByUid
                                    into #CustomerAppointmentEquipment From CustomerAppointmentEquipment ca
	                                left join ticket tk2 on ca.AppointmentId = tk2.TicketId
										                                left join Equipment _eq2 on _eq2.EquipmentId = ca.EquipmentId
										                                left join EquipmentType _et2 on _et2.Id = _eq2.EquipmentTypeId
										                                left join Manufacturer manu2 on manu2.Id = _eq2.ManufacturerId
										                                left join TicketUser tu2 on tu2.TiketId = tk2.TicketId
										                                left join Employee emp2 on emp2.UserId = ca.InstalledByUid
                                                                        left join Lookup lktype2 on  lktype2.DataKey ='TicketType'
                                                                        left join Customer cus2 on cus2.CustomerId = tk2.CustomerId  
                                                                        and lktype2.DataValue = tk2.TicketType
										                                where tk2.CompanyId = '{0}'
                                                                        and _eq2.EquipmentClassId=1
										                                and cus2.id is not null
										                                and ca.QuantityLeftEquipment > 0
										                                and tk2.Id not in (select TicketId from IndividualInstalledEquipment)
                                    select @IdCount = Count(*) from #CustomerAppointmentEquipment
	                                WHILE @i <= @IdCount
	                                BEGIN
		                                select @qty = _cae4.QuantityLeftEquipment from  #CustomerAppointmentEquipment _cae4
										                                left join ticket tk4 on _cae4.AppointmentId = tk4.TicketId
										                                left join Equipment _eq4 on _eq4.EquipmentId = _cae4.EquipmentId
										                                left join EquipmentType _et4 on _et4.Id = _eq4.EquipmentTypeId
										                                left join Manufacturer manu4 on manu4.Id = _eq4.ManufacturerId
										                                left join TicketUser tu4 on tu4.TiketId = tk4.TicketId
										                                left join Employee emp4 on emp4.UserId = _cae4.InstalledByUid
                                                                        left join Lookup lktype4 on  lktype4.DataKey ='TicketType'
                                                                        left join Customer cus4 on cus4.CustomerId = tk4.CustomerId  
                                                                        and lktype4.DataValue = tk4.TicketType
										                                where tk4.CompanyId = '{0}'
                                                                        and _eq4.EquipmentClassId=1
										                                and cus4.id is not null
										                                and _cae4.QuantityLeftEquipment > 0
										                                and _cae4.RowNum = @i
										
										
		                                WHILE @j <= @qty
		                                BEGIN
			                                select @getAppointmentEquipmentId = _cae3.Id,
				                                   @getCategory = _et3.Name,
				                                   @getManufacturer = manu3.Name,
				                                   @getDescription = _eq3.Name,
				                                   @getTicketType = lktype3.DisplayText,
				                                   @getEmpUser = emp3.FirstName + ' ' + emp3.LastName,
				                                   @getRepliesCount = (select count(id) from TicketFile where TicketId = tk3.TicketId)
		                                                                        + (select count(id) from TicketReply where TicketId = tk3.TicketId),
				                                   @getStatus = tk3.[Status],
				                                   @getTicketId = tk3.Id,
				                                   @getAttachmentsCount = (select count(id) from TicketFile where TicketId = tk3.TicketId),
				                                   @getCusIdInt = cus3.Id,
				                                   @getCustomerName = cus3.FirstName + ' ' + cus3.LastName,
				                                   @getCompletionDate = tk3.CompletionDate,
				                                   @getSKU = _eq3.SKU,
				                                   @getTotalPoint = Format(_eq3.Point*_cae3.QuantityLeftEquipment,'N2'),
				                                   @getIsClosed = tk3.IsClosed,
				                                   @getCompanyCost = (select Cost from EquipmentVendor where EquipmentId = _eq3.EquipmentId and IsPrimary = 1),
				                                   @getCustomerCost = _cae3.UnitPrice,
				                                   @getQuantity = _cae3.Quantity,
				                                   @getInstalledEquipment =_cae3.QuantityLeftEquipment,
				                                   @getQty = 1,
												   @InstalledByUid = _cae3.InstalledByUid, 
												   @EquipmentId = _cae3.EquipmentId
				                                   from  #CustomerAppointmentEquipment _cae3
				                                   left join ticket tk3 on _cae3.AppointmentId = tk3.TicketId
										                                left join Equipment _eq3 on _eq3.EquipmentId = _cae3.EquipmentId
										                                left join EquipmentType _et3 on _et3.Id = _eq3.EquipmentTypeId
										                                left join Manufacturer manu3 on manu3.Id = _eq3.ManufacturerId
										                                left join TicketUser tu3 on tu3.TiketId = tk3.TicketId
										                                left join Employee emp3 on emp3.UserId = _cae3.InstalledByUid
                                                                        left join Lookup lktype3 on  lktype3.DataKey ='TicketType'
                                                                        left join Customer cus3 on cus3.CustomerId = tk3.CustomerId  
                                                                        and lktype3.DataValue = tk3.TicketType
										                                where tk3.CompanyId = '{0}'
                                                                        and _eq3.EquipmentClassId=1
										                                and cus3.Id is not null
										                                and _cae3.QuantityLeftEquipment > 0
										                                and _cae3.RowNum = @i
										
			                                insert INTO IndividualInstalledEquipment values (@getAppointmentEquipmentId, @getCategory, @getManufacturer, @getDescription, @getTicketType, @getEmpUser, @getTicketId, @getRepliesCount,
			                                 @getAttachmentsCount, @getCusIdInt, @getCustomerName, @getCompletionDate, @getSKU, @getTotalPoint, @getIsClosed,
			                                @getCompanyCost, @getCustomerCost, @getQuantity, @getInstalledEquipment,@getQty, @getStatus,'00000000-0000-0000-0000-000000000000',CONVERT(time, GETDATE()), @InstalledByUid, @EquipmentId)

			                                SET @j += 1;
		                                END

		                                SET @i += 1;
		                                set @j = 1;
                                        END

                                        DROP TABLE #CustomerAppointmentEquipment";
            SqlQuery = string.Format(SqlQuery, CompanyId);
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
        public bool DeleteIndividualInstalledEquipmentByTicketIdAndEquipmentId(int TicketId, Guid EquipmentId)
        {
            string SqlQuery = @"delete from IndividualInstalledEquipment where TicketId = '{0}' and EquipmentId = '{1}'
                ";
            SqlQuery = string.Format(SqlQuery, TicketId, EquipmentId);
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
        public bool DeleteIndividualInstalledEquipmentByTicketIdAndEquipmentIdAndAppointmentEquipmentId(int TicketId, Guid EquipmentId, int AppointmentEquipmentId)
        {
            string SqlQuery = @"delete from IndividualInstalledEquipment where TicketId = '{0}' and EquipmentId = '{1}' and AppointmentEquipmentId ='{2}'
                ";
            SqlQuery = string.Format(SqlQuery, TicketId, EquipmentId, AppointmentEquipmentId);
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
        public DataTable GetAllIndividualInstalledEquipmentIdById(int Id)
        {
            string sqlQuery = @"select Id from IndividualInstalledEquipment Where AppointmentEquipmentId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, Id);
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
