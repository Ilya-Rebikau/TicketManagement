--- Venue
insert into dbo.Venue
values ('First venue description', 'First venue address', '123 45 678 90 12', 'First venue name')

--- Layout
insert into dbo.Layout
values (1, 'First layout description', 'First layout'),
(1, 'Second layout description', 'Second layout')

--- Area
insert into dbo.Area
values (1, 'First area of first layout', 1, 1),
(1, 'Second area of first layout', 1, 2),
(2, 'First area of second layout', 1, 1)

--- Seat
insert into dbo.Seat
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
