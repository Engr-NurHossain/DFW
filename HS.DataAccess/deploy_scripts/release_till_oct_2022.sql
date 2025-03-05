
CREATE TABLE Logs(
    Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
    Level varchar(10),
	CallSite varchar(max) not null,
    Message nvarchar(max),
	ErrorMessage nvarchar(max),
    StackTrace nvarchar(max),
    Exception nvarchar(max),
    Logger nvarchar(255),
    Url nvarchar(255),
	CreatedOn datetime NOT NULL
);

select TOP 10 * from PermissionGroupMap where PermissionId=8043 order by Id desc --where PermissionGroupId=1
--insert into PermissionGroupMap values(1,8043,1,'C7E72006-6EDF-4B5A-8589-BFD1B2DAE7BA')
select * from Permission where Id=8043
--insert into Permission values(8043, 'C7E72006-6EDF-4B5A-8589-BFD1B2DAE7BA',null, 'TechDropDownEditable', 'Inventory -> Transfer|Equipment Details',1)
select TOP 10 * from PermissionGroup


ALTER TABLE AssignedInventoryTechReceived ADD ReqSrc VARCHAR(20);
ALTER TABLE AssignedInventoryTechReceived ADD CONSTRAINT def_val_ReqSrc DEFAULT 'TT' FOR ReqSrc;

