select * from Permission where Id BETWEEN 8000 AND 9000 order by Id Desc
insert into Permission values(8045, 'C7E72006-6EDF-4B5A-8589-BFD1B2DAE7BA',null, 'ShowTransferLog', 'Inventory -> Transfer Log',1)
insert into Permission values(8046, 'C7E72006-6EDF-4B5A-8589-BFD1B2DAE7BA',null, 'ShowWarehouseHistory', 'Inventory -> Warehouse History',1)

select * from PermissionGroup
select top 10  * from INFORMATION_SCHEMA.Tables where TABLE_NAME like '%Tag%'
select * from PermissionGroupMap where PermissionId=8046 order by PermissionGroupId
insert into PermissionGroupMap values(1,8044,1,'C7E72006-6EDF-4B5A-8589-BFD1B2DAE7BA')