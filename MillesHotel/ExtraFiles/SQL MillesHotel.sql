--SQL fil med n�gra SELECT, WHERE, ORDER BY, JOINS, GROUP BY, SUBQUERY

--1. Visar aktiva kunder som har genomf�rt bokningar
SELECT DISTINCT 
C.*
FROM Customer C
JOIN Booking B ON C.CustomerID = B.CustomerID
WHERE C.IsActive = 1;

--2. Visar rum som �r st�rre �n 50 kvadratmeter och har bokats minst en g�ng
SELECT DISTINCT 
R.*
FROM Room R
JOIN Booking B ON R.RoomID = B.RoomID
WHERE R.RoomSize > 50
ORDER BY R.RoomSize;


--3. Visar obetalda fakturor och dess kunder
SELECT 
C.*, 
I.*
FROM Customer C
JOIN Booking B ON C.CustomerID = B.CustomerID
JOIN Invoice I ON B.InvoiceID = I.InvoiceID
WHERE I.IsPaid = 0;


--4. Visar kund, deras bokningar och ruminformation f�r endast kunder som har minst en bokning
SELECT 
C.*, 
B.*, 
R.*
FROM Customer C
JOIN Booking B ON C.CustomerID = B.CustomerID
JOIN Room R ON B.RoomID = R.RoomID
WHERE C.IsActive = 1;


--5. Visar total oms�ttning per rumstyp och sortera efter h�gsta int�kt
SELECT 
RoomType AS [Single/DoubleRoom], 
SUM(RoomPrice) AS TotalRevenue
FROM Room
JOIN Booking ON Room.RoomID = Booking.RoomID
GROUP BY RoomType
ORDER BY TotalRevenue DESC;


--6. Visar kunder som inte har gjort n�gra bokningar
SELECT 
C.*
FROM Customer C
LEFT JOIN Booking B ON C.CustomerID = B.CustomerID
WHERE B.CustomerID IS NULL;


--7. Visar rum och all information, men bara f�r det dyraste rummet
SELECT 
*
FROM Room
WHERE RoomPrice = (SELECT MAX(RoomPrice) FROM Room);


--8. Visar kunder som har bokat mer �n ett rum och antalet bokade rum
SELECT 
C.CustomerID, 
COUNT(B.RoomID) AS BookedRoomsCount
FROM Customer C
JOIN Booking B ON C.CustomerID = B.CustomerID
GROUP BY C.CustomerID
HAVING COUNT(B.RoomID) > 1;


--9. Visar lediga rum som inte �r bokade
SELECT 
*
FROM Room
LEFT JOIN Booking ON Room.RoomID = Booking.RoomID
WHERE Booking.RoomID IS NULL;


--10. Visar kundinformation, deras land och total fakturerat belopp, �ven om de inte har bokningar
SELECT 
C.CustomerID, 
C.CustomerFirstName, 
C.CustomerLastName, 
C.CustomerAge,
C.CustomerEmail, 
C.CustomerPhone, 
Co.CountryName AS CustomerCountry,
C.IsActive, COALESCE(SUM(I.InvoiceAmount), 0) AS TotalInvoiceAmount
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

