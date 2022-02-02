ALTER TABLE dbo.AspNetUserClaims
ADD CONSTRAINT FK_AspNetUsers_AspNetUserClaims FOREIGN KEY ([UserId])     
    REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE