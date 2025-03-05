-- 16 DEC 2022 - Add column for storing the Source of Transfer Request
ALTER TABLE InventoryTech ADD TicketId uniqueidentifier;

-- Add column for storing the Source of Transfer Request
ALTER TABLE AssignedInventoryTechReceived ADD ReqSrc VARCHAR(20);
ALTER TABLE AssignedInventoryTechReceived ADD CONSTRAINT def_val_ReqSrc DEFAULT 'TT' FOR ReqSrc;

-- Changes for adding Permissions
