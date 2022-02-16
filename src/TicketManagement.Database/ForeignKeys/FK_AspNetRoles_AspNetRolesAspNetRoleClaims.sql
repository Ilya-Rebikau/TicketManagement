ALTER TABLE dbo.AspNetRoleClaims
ADD CONSTRAINT FK_AspNetRoles_AspNetRolesAspNetRoleClaims FOREIGN KEY ([RoleId])     
    REFERENCES dbo.AspNetRoles (Id) ON DELETE CASCADE
