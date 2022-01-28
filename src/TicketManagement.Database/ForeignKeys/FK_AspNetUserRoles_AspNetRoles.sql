ALTER TABLE dbo.AspNetUserRoles
ADD CONSTRAINT FK_AspNetUserRoles_AspNetRoles FOREIGN KEY ([RoleId])     
    REFERENCES dbo.AspNetRoles (Id) ON DELETE CASCADE