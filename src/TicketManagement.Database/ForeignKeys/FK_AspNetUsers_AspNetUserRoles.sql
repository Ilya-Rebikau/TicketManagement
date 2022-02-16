ALTER TABLE dbo.AspNetUserRoles
ADD CONSTRAINT FK_AspNetUsers_AspNetUserRoles FOREIGN KEY ([UserId])     
    REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE
