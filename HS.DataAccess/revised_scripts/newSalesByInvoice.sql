declare @pagestart int
                                declare @pageend int
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select * into #SaleCom from (Select
								Distinct(cus.Id),
								CASE 
	WHEN (cus.DBA = '' or cus.DBA IS NULL) AND  (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	WHEN (cus.DBA = '' or cus.DBA IS NULL)  THEN cus.BusinessName
	ELSE  cus.DBA
END as DisplayName,
                                lkleadstatus.DisplayText as LeadStatus,
								cus.CustomerNo,
                                cus.SalesDate,
                                tkLookUp.DisplayText as TicketType,
								emp.FirstName+' '+emp.LastName as SalesPerson,
								lksource.DisplayText as LeadSource,
								lklocation.DisplayText as SalesLocation,
								cus.Type,
								ISNULL(pc.ActivationFee+pc.NonConformingFee,0) as ActNonFee,
								CAST(ISNULL(cus.MonthlyMonitoringFee,0) as float) as RMR,
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService!=1),0) as EquipmentFee,
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService=1 and eqp.IsARBEnabled=0 and cae.IsCopied=0 and tk.Status !='Incomplete'),0) as ServiceFee,
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService=1 and eqp.IsARBEnabled=1 and cae.IsCopied=0 and tk.Status !='Incomplete') *(pic.ForMonths-1),0) as AdvancedMonitoring,
								
								((
								ISNULL(pc.ActivationFee+pc.NonConformingFee,0)
								+
								CAST(ISNULL(cus.MonthlyMonitoringFee,0) as float)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService!=1),0)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService=1 and eqp.IsARBEnabled=0 and cae.IsCopied=0 and tk.Status !='Incomplete'),0)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService=1 and eqp.IsARBEnabled=1 and cae.IsCopied=0 and tk.Status !='Incomplete') *(pic.ForMonths-1),0)
								) * (select Value from globalsetting where SearchKey = 'Sales Tax'))/100 as TotalTax,

								ISNULL(cus.FinancedAmount,0) as FinancedAmount,

								((ISNULL(pc.ActivationFee+pc.NonConformingFee,0)
								+
								CAST(ISNULL(cus.MonthlyMonitoringFee,0) as float)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService!=1),0)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService=1 and eqp.IsARBEnabled=0 and cae.IsCopied=0 and tk.Status !='Incomplete'),0)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService=1 and eqp.IsARBEnabled=1 and cae.IsCopied=0 and tk.Status !='Incomplete') *(pic.ForMonths-1),0)
								)
								+
								((
								ISNULL(pc.ActivationFee+pc.NonConformingFee,0)
								+
								CAST(ISNULL(cus.MonthlyMonitoringFee,0) as float)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService!=1),0)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService=1 and eqp.IsARBEnabled=0 and cae.IsCopied=0 and tk.Status !='Incomplete'),0)
								+
								ISNULL((Select SUM(TotalPrice) from CustomerAppointmentEquipment cae 
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								Where tk.CustomerId=cus.CustomerId and cae.IsService=1 and eqp.IsARBEnabled=1 and cae.IsCopied=0 and tk.Status !='Incomplete') *(pic.ForMonths-1),0)
								) * (select Value from globalsetting where SearchKey = 'Sales Tax'))/100
								--+
								--ISNULL(cus.FinancedAmount,0)
								) as TotalSales
								from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
								LEFT JOIN CustomerCompany cc on cc.CustomerId=cus.CustomerId
								LEFT JOIN Employee emp on emp.UserId=cus.SoldBy1
                                left join Lookup lkleadstatus on lkleadstatus.DataKey ='LeadStatus' and lkleadstatus.DataValue = cus.Status and lkleadstatus.DataValue!='-1'
								left join Lookup lksource on lksource.DataKey ='LeadSource' and lksource.DataValue = cus.LeadSource and lksource.DataValue!='-1'
								left join Lookup lklocation on lklocation.DataKey = 'CommissionType' and lklocation.DataValue = cus.SalesLocation and lklocation.DataValue!='-1'
								LEFT join PackageCustomer pc on pc.CustomerId=cus.CustomerId
								LEFT JOIN PaymentInfoCustomer pic on pic.CustomerId=cus.CustomerId and pic.Payfor='Service'
                                LEFT JOIN Ticket tk on tk.CustomerId=cus.CustomerId and tk.IsAgreementTicket=1
								LEFT JOIN Lookup tkLookUp on tkLookUp.DataValue=tk.TicketType and tkLookUp.DataKey='TicketType'								
                                Where cc.IsLead=0 and cus.IsActive=1 and ce.IsTestAccount != 1 and cus.TransferCustomerId IS NULL and (cus.MoveCustomerId='00000000-0000-0000-0000-000000000000' or cus.MoveCustomerId IS NULL) and cus.SalesDate between '2023-01-17 00:00:00.000' and '2023-02-16 23:59:59.000'  
								) d

								Select *into #tempSaleCom from #SaleCom

								select top(@pagesize)
								* into #10SaleCom from #tempSaleCom
								where Id not in(select top(@pagestart) Id from #tempSaleCom #cd order by SalesDate desc)
                                order by SalesDate desc

								select * from #10SaleCom

								select count(*) CountTotal,count(Id) as CustomerCount,sum(ActNonFee) as SumActNonFeeTotal,sum(RMR) as SumRMRTotal,sum(EquipmentFee) as SumEquipmentFeeTotal,sum(ServiceFee) as SumServiceFeeTotal,sum(AdvancedMonitoring) as SumAdvancedMonitoringTotal,(sum(TotalSales)-sum(TotalTax)) as TotalWoTax,sum(TotalTax) as SumTaxTotal,Sum(TotalSales) as SumTotalSales,sum(FinancedAmount) as SumFinancedAmount from #SaleCom



								select 
								 sum(ActNonFee) as TotalActNonFee
								,sum(RMR) as TotalRMR
								,sum(EquipmentFee) as TotalEquipmentFee
								,sum(ServiceFee) as TotalServiceFee
								,sum(AdvancedMonitoring) as TotalAdvancedMonitoring
								,sum(TotalTax) as TotalTotalTax
								,sum(FinancedAmount) as FinancedAmount
								,sum(TotalSales) as TotalSales
								from #10SaleCom

								drop table #SaleCom
								drop table #tempSaleCom
                                drop table #10SaleCom


								