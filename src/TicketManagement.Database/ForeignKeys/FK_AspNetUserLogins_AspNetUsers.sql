﻿ALTER TABLE dbo.AspNetUserLogins
ADD CONSTRAINT FK_AspNetUserLogins_AspNetUsers FOREIGN KEY ([UserId])     
    REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE