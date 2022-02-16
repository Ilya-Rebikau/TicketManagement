ALTER TABLE dbo.AspNetUserRoles
ADD CONSTRAINT FK_AspNetRoles_AspNetUserRoles FOREIGN KEY ([RoleId])     
    REFERENCES dbo.AspNetRoles (Id) ON DELETE CASCADE