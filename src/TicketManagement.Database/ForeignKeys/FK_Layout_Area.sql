ALTER TABLE dbo.Areas
ADD CONSTRAINT FK_Layout_Area FOREIGN KEY (LayoutId)     
    REFERENCES dbo.Layouts (Id)