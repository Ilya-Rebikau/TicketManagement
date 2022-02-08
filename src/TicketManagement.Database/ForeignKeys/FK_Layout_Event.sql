ALTER TABLE dbo.[Events]
ADD CONSTRAINT FK_Layout_Event FOREIGN KEY (LayoutId)     
    REFERENCES dbo.Layouts (Id)