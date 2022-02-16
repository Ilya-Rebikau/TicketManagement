--- Venue
insert into dbo.Venues
values ('First venue description', 'First venue address', '123 45 678 90 12', 'First venue name')

--- Layout
insert into dbo.Layouts
values (1, 'First layout description', 'First layout'),
(1, 'Second layout description', 'Second layout')

--- Area
insert into dbo.Areas
values (1, 'First area of first layout', 1, 1, 11),
(1, 'Second area of first layout', 1, 2, 12),
(2, 'First area of second layout', 1, 1, 13)

--- Seat
insert into dbo.Seats
values 
(1, 1, 1),
(1, 1, 2),
(1, 1, 3),
(1, 2, 1),
(1, 2, 2),
(1, 2, 3),
(2, 1, 1),
(2, 1, 2),
(2, 1, 3),
(2, 2, 1),
(2, 2, 2),
(2, 2, 3),
(3, 1, 1),
(3, 1, 2),
(3, 1, 3),
(3, 2, 1),
(3, 2, 2),
(3, 2, 3)

--- Event
DECLARE @timeStart smalldatetime
DECLARE @timeEnd smalldatetime
DECLARE @id INT
SET @timeStart = CONVERT(smalldatetime, '2030-12-21 15:00:00', 20)
SET @timeEnd = CONVERT(smalldatetime, '2030-12-21 17:00:00', 20)
Exec sp_CreateEvent 'First event name', 'First event description', 1, @timeStart, @timeEnd, 'https://w-dog.ru/wallpapers/5/16/428743654433638/kotyata-serye-zhivotnye-trava-gazon.jpg', @id OUTPUT

---EventArea

insert into dbo.EventAreas
values 
(1, 'First event area description', 1, 1, 11),
(1, 'Second event area description', 1, 2, 12)

---EventSeat
insert into dbo.EventSeats
values
(1, 1, 1, 0),
(1, 1, 2, 1),
(1, 2, 1, 1),
(2, 1, 1, 0),
(2, 1, 2, 1),
(2, 2, 1, 0)

---AspNetRoles
insert into dbo.AspNetRoles
values 
('420ecee9-cf46-4824-9dd5-84cabe7748aa', 'admin', 'ADMIN', '463fddb3-0509-498d-bec2-2762dd825cbb'),
('55b4665d-f989-47d9-ae4d-5bc66150603c', 'event manager', 'EVENT MANAGER', 'c3b4b8a8-db6f-4113-936f-e710885dc0e9'),
('91f86e9a-9464-4686-aac3-a7e3788298e1', 'user', 'USER', 'ba426d36-4cff-48a0-b726-b734128c418d'),
('e02b7cb1-8e72-4562-ae60-19228021d90a', 'venue manager', 'VENUE MANAGER', '98a240c3-4e3b-4205-b916-0d0c0938a7b2')

---AspNetUsers
insert into dbo.AspNetUsers
values 
('25ed5e6a-2563-45d8-b9eb-bd77f8c9f83e', 'eventmanager@mail.ru', 'EVENTMANAGER@MAIL.RU', NULL, NULL, 
'eventmanager@mail.ru', 'EVENTMANAGER@MAIL.RU', 0, 0, 
'AQAAAAEAACcQAAAAEC81ktyJThar7bZwM/bQAMgw0gGjFsgE54f5813sHq38JSpMrUZaJtf+c/ujTUWu3g==', 
'N3EXAEE2JBJ3IBNTHTJGZ5BB6WF4BWYJ', '3966505b-f92f-4dfb-bc35-65b14e4e2280', NULL, 0, 0, NULL, 1, 0, NULL),

('72357c52-99fd-43c1-898e-9ff3bb7aa6e9', 'admin@mail.ru', 'ADMIN@MAIL.RU', NULL, NULL, 
'admin@mail.ru', 'ADMIN@MAIL.RU', 0, 0, 
'AQAAAAEAACcQAAAAEFBeM9W0syVC7yg0Zasu09oh71rBfBpIgPWMdSk6EG4qhdaQagrDRr0gRCKqf8JvlA==', 
'YRHJBWNGAE6ZJTK4GMGZPBOBAIPY2JBV', '4a32730f-15d0-48a6-bc96-fec26c83f698', NULL, 0, 0, NULL, 1, 0, NULL),

('db4a4f7c-c895-4aba-8fba-c5a88d3c8298', 'venuemanager@mail.ru', 'VENUEMANAGER@MAIL.RU', NULL, NULL, 
'venuemanager@mail.ru', 'VENUEMANAGER@MAIL.RU', 0, 0, 
'AQAAAAEAACcQAAAAENg3aAC07cuLQhOhSWi9pPQcOavp+WQnbuCr75SX2Y/zWOHzYMioy3CzgXPRIBqKOg==', 
'NUFT6IZXAO3RE4IONSPNKRILRZLMFLUL', 'bb213e3e-a6d1-4b38-a66a-401f62110179', NULL, 0, 0, NULL, 1, 0, NULL),

('ffe41a98-cdbc-4799-b6a1-b7d3f9a924f0', 'user@mail.ru', 'USER@MAIL.RU', NULL, NULL, 
'user@mail.ru', 'USER@MAIL.RU', 0, 0, 
'AQAAAAEAACcQAAAAECf+hgn5fJFXwBpoiHI4m4EdUtRyoC1qCDuls1tUzZNqXtiukjBD7zFoPHGieu+g6w==', 
'3R46CL6WQUWSW3SA56L7OUV5HMEV2QKQ', '005a0e67-9393-4f69-84d4-05cc1ecc38d1', NULL, 0, 0, NULL, 1, 0, NULL)

---AspNetUserRoles
insert into dbo.AspNetUserRoles
values 
('25ed5e6a-2563-45d8-b9eb-bd77f8c9f83e', '420ecee9-cf46-4824-9dd5-84cabe7748aa'),
('72357c52-99fd-43c1-898e-9ff3bb7aa6e9', '420ecee9-cf46-4824-9dd5-84cabe7748aa'),
('db4a4f7c-c895-4aba-8fba-c5a88d3c8298', 'e02b7cb1-8e72-4562-ae60-19228021d90a'),
('ffe41a98-cdbc-4799-b6a1-b7d3f9a924f0', '91f86e9a-9464-4686-aac3-a7e3788298e1')
