--- Venue
insert into dbo.Venues
values ('First venue description', 'First venue address', '123 45 678 90 12', 'First venue name')

--- Layout
insert into dbo.Layouts
values (1, 'First layout description', 'First layout'),
(1, 'Second layout description', 'Second layout')

--- Area
insert into dbo.Areas
values (1, 'First area of first layout', 1, 1),
(1, 'Second area of first layout', 1, 2),
(2, 'First area of second layout', 1, 1)

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
SET @timeStart = CONVERT(smalldatetime, '2030-12-21 15:00:00', 20)
SET @timeEnd = CONVERT(smalldatetime, '2030-12-21 17:00:00', 20)
Exec sp_CreateEvent 'First event name', 'First event description', 1, @timeStart, @timeEnd, 'imageURL'

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