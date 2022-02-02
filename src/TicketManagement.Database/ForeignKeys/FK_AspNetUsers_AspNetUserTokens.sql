ALTER TABLE dbo.AspNetUserTokens
ADD CONSTRAINT FK_AspNetUsers_AspNetUserTokens FOREIGN KEY ([UserId])     
    REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE
