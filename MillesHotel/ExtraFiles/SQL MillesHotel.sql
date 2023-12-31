--SQL fil med några SELECT, WHERE, ORDER BY, JOINS, GROUP BY, SUBQUERY

--1
SELECT 
* 
FROM Customer 
WHERE IsActive = 1;

--2
SELECT 
* 
FROM Room 
WHERE RoomSize > 50 
ORDER BY RoomSize;

--3
SELECT 
InvoiceAmount, 
InvoiceDue 
FROM Invoice 
WHERE IsPaid = 0;

--4
SELECT 
C.*, 
B.*, 
R.*
FROM Customer C
JOIN Booking B ON C.CustomerID = B.CustomerID
JOIN Room R ON B.RoomID = R.RoomID;

--5
SELECT 
RoomType AS [Single/DoubleRoom], 
SUM(RoomPrice) AS TotalRevenue
FROM Room
JOIN Booking ON Room.RoomID = Booking.RoomID
GROUP BY RoomType;

--6 subquery
SELECT 
Customer.*
FROM Customer
LEFT JOIN Booking ON Customer.CustomerID = Booking.CustomerID
WHERE Booking.CustomerID IS NULL;

--7 visar enbart det dyraste rummet
SELECT 
* 
FROM Room 
ORDER BY RoomPrice DESC OFFSET 0 ROWS FETCH FIRST 1 ROW ONLY;

--8
SELECT 
CustomerID, 
COUNT(RoomID) AS BookedRoomsCount
FROM Booking
GROUP BY CustomerID
HAVING COUNT(RoomID) > 1;

--9
SELECT *
FROM Room
LEFT JOIN Booking ON Room.RoomID = Booking.RoomID
WHERE Booking.RoomID IS NULL;

--10
SELECT 
C.CustomerID,
C.CustomerFirstName,
C.CustomerLastName,
C.CustomerAge,
C.CustomerEmail,
C.CustomerPhone,
Co.CountryName AS CustomerCountry,
C.IsActive,
SUM(I.InvoiceAmount) AS TotalInvoiceAmount
FROM Customer C
LEFT JOIN Booking B ON C.CustomerID = B.CustomerID
LEFT JOIN Invoice I ON B.InvoiceID = I.InvoiceID
LEFT JOIN Country Co ON C.CountryID = Co.CountryID
GROUP BY 
C.CustomerID,
C.CustomerFirstName,
C.CustomerLastName,
C.CustomerAge,
C.CustomerEmail,
C.CustomerPhone,
Co.CountryName,
C.IsActive;
