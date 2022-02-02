ALTER TABLE dbo.AspNetUserLogins
ADD CONSTRAINT FK_AspNetUsers_AspNetUserLogins FOREIGN KEY ([UserId])     
    REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE