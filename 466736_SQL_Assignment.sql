--Use master
--drop database assignment

CREATE DATABASE assignment
GO
USE assignment
GO

CREATE TABLE Customers(
	Customerid CHAR(5) NOT NULL,
	CompanyName VARCHAR(40) NOT NULL,
	ContactName CHAR(30) NULL,
	Address VARCHAR(60) NULL,
	City CHAR(15) NULL,
	Phone CHAR(24) NULL,
	Fax CHAR(24) NULL,
 )
 GO

CREATE TABLE Orders(
	OrderID INT NOT NULL,
	CustomerID CHAR(5) NOT NULL,
	OrderDate DATETIME NULL,
	ShippedDate DATETIME NULL,
	Freight MONEY NULL,
	ShipName VARCHAR(40) NULL,
	ShipAddress VARCHAR(60) NULL,
	Quantity INT NOT NULL,
 )
 GO

 --2
 ALTER TABLE Orders ADD shipregion INT NULL
 GO

 --3
 ALTER TABLE Orders ALTER COLUMN shipregion CHAR(8) NULL
 GO

 --4
 ALTER TABLE Orders DROP COLUMN shipregion
 GO

 --5
 ALTER TABLE Orders ADD CONSTRAINT DefaultOrderDate DEFAULT GETDATE() FOR OrderDate
 GO

 --6
 exec sp_rename 'Customers.City', 'Customers.Town', 'COLUMN'
 GO



CREATE TABLE Department(
	DepID CHAR(5) NOT NULL,
	DepName VARCHAR(50) NOT NULL,
	location VARCHAR(50) NOT NULL
)
GO

INSERT INTO Department VALUES ('d1', 'Res', 'Dallas')
INSERT INTO Department VALUES ('d2', 'Acc', 'Seattle')
INSERT INTO Department VALUES ('d3', 'Mar', 'Dallas')

CREATE TABLE Employee(
	emp_id CHAR(5) NOT NULL,
	emp_fname CHAR(50) NOT NULL,
	emp_lname CHAR(50) NOT NULL,
	dept_id CHAR(5) NOT NULL
)
GO

INSERT INTO Employee VALUES (25348, 'Matthew', 'Smith', 'd3')
INSERT INTO Employee VALUES (10102, 'Ann', 'Jones', 'd3')
INSERT INTO Employee VALUES (18316, 'John', 'Barrimor', 'd1')
INSERT INTO Employee VALUES (29346, 'James', 'James', 'd2')


CREATE TABLE Project(
	project_id CHAR(5) NOT NULL,
	project_name CHAR(50) NOT NULL,
	Budget INT NOT NULL
 )
 GO

INSERT INTO Project VALUES ('p1', 'Apollo', 12000)
INSERT INTO Project VALUES ('p2', 'Gemini', 95000)
INSERT INTO Project VALUES ('p3', 'Mercury', 18560)
 
CREATE TABLE Works_on(
	emp_no CHAR(5) NOT NULL,
	project_no CHAR(5) NOT NULL,
	Job CHAR(50) NULL,
	enter_date DATETIME NULL
 )
 GO

INSERT INTO Works_on VALUES (10102,'p1','Analyst','1997.10.1')
INSERT INTO Works_on VALUES (10102,'p3','manager','1999.1.1')
INSERT INTO Works_on VALUES (25348,'p2','Clerk','1998.2.15')
INSERT INTO Works_on VALUES (18316,'p2',NULL,'1998.6.1')
INSERT INTO Works_on VALUES (29346,'p2',NULL,'1997.12.15')
INSERT INTO Works_on VALUES (2581,'p3','Analyst','1998.10.15')
INSERT INTO Works_on VALUES (9031,'p1','Manager','1998.4.15')
INSERT INTO Works_on VALUES (28559,'p1',NULL,'1998.8.1')
INSERT INTO Works_on VALUES (28559,'p2','Clerk','1992.2.1')
INSERT INTO Works_on VALUES (9031,'p3','Clerk','1997.11.15')
INSERT INTO Works_on VALUES (29346,'p1','Clerk','1998.1.4')

--SIMPLE QUERIES
--1
SELECT (e.emp_fname + ' ' + e.emp_lname) AS Name FROM Works_on w
JOIN Employee e ON e.emp_id = w.emp_no
JOIN Project p ON p.project_id = w.project_no
WHERE w.project_no = 'p2' and e.emp_id < 10000

--2
SELECT emp_no FROM Works_on WHERE YEAR(Enter_Date) <> 1998

--3
SELECT emp_no FROM Works_on WHERE Job IN ('Analyst', 'manager')

--4
SELECT Enter_Date FROM Works_on WHERE project_no = 'p2' AND Job IS NULL

--5
SELECT e.emp_id, e.emp_lname FROM Employee e WHERE e.emp_fname like '%tt%'

--6
SELECT e.emp_id, e.emp_fname FROM Employee e
where (CHARINDEX('o', emp_lname) = 2 OR charindex('a', emp_lname) = 2) AND (emp_lname LIKE '%es')

--7
SELECT e.emp_id FROM Employee e 
JOIN Department d ON d.DepID = e.dept_id
WHERE d.location = 'Seattle'

--8
SELECT e.emp_lname, e.emp_fname FROM Employee e
JOIN Works_on w on e.emp_id = w.emp_no
WHERE w.enter_date = '1998.1.4'

--9
SELECT Location, Count(*) AS Count FROM Department GROUP BY Location

--10
SELECT MAX(emp_id) from Employee


--INDEXERS

--1
CREATE NONCLUSTERED INDEX enter_date_index ON Works_on(enter_date)
ALTER INDEX enter_date_index ON Works_on REBUILD WITH (FILLFACTOR = 60)
GO

--2
CREATE UNIQUE NONCLUSTERED INDEX names_index ON Employee(emp_lname, emp_fname) WITH(IGNORE_DUP_KEY = OFF)
GO

--VIEWS
--1
CREATE VIEW D1Employees AS
SELECT * FROM Employee WHERE dept_id = 'd1'
GO

--2
CREATE VIEW ProjectWithoutBudget AS
SELECT project_id, project_name FROM Project
GO

--3
CREATE VIEW GetEmployees AS
SELECT Emp_LName, Emp_FName FROM Employee P
JOIN Works_on W on P.emp_id = W.emp_no
WHERE YEAR(W.Enter_Date) = 1988 AND MONTH(W.Enter_Date) > 6
GO

--CONSTRAINTS
--1
DROP TABLE Customers
GO

CREATE TABLE Customers(
	Customerid CHAR(5) NOT NULL,
	CompanyName VARCHAR(40) NOT NULL,
	ContactName CHAR(30) NULL,
	Address VARCHAR(60) NULL,
	City CHAR(15) NULL,
	Phone CHAR(24) NULL,
	Fax CHAR(24) NULL,
	CONSTRAINT PK_Customer PRIMARY KEY(Customerid)
)
GO

DROP TABLE Orders
GO

CREATE TABLE Orders(
	OrderID INT NOT NULL PRIMARY KEY,
	CustomerID CHAR(5) NOT NULL,
	OrderDate DATETIME NULL,
	ShippedDate DATETIME NULL,
	Freight MONEY NULL,
	ShipName VARCHAR(40) NULL,
	ShipAddress VARCHAR(60) NULL,
	Quantity INT NOT NULL,
	CONSTRAINT fk_customer FOREIGN KEY(CustomerID) REFERENCES Customers(Customerid)
	)
GO

--2
ALTER TABLE Orders ADD CONSTRAINT order_qty CHECK (Quantity BETWEEN 1 AND 30)

--3
CREATE TYPE WesternCountries FROM VARCHAR(2) NOT NULL
GO
	
CREATE DEFAULT Default_WesternCountries AS 'CA'
GO

EXEC sp_bindefault 'Default_WesternCountries' ,'WesternCountries'
GO

CREATE RULE RULE_WesternCountries AS @Value IN ('CA','NM', 'OR','WA')
GO

EXEC sp_bindrule 'RULE_WesternCountries' ,'WesternCountries'
GO

CREATE TABLE Regions(
	City WesternCountries,
	Country VARCHAR(30) NOT NULL
)
GO

INSERT INTO [Regions] (Country) values ('USA')
INSERT INTO [Regions] (City,Country) values ('CA','USA')
INSERT INTO [Regions] (City,Country) values ('WA','USA')
INSERT INTO [Regions] (City,Country) values ('NM','USA')
INSERT INTO [Regions] (City,Country) values ('OR','USA')
GO

--4
SELECT a.name, b.name AS 'Default Constraint',c.name AS 'Check Constraints',d.name AS 'Primary Key',e.name 'Foreign Keys'
FROM sys.tables a 
LEFT JOIN sys.default_constraints b on a.object_id = b.parent_object_id
LEFT JOIN sys.check_constraints c on a.object_id = c.parent_object_id
LEFT JOIN sys.key_constraints d on a.object_id = d.parent_object_id
LEFT JOIN sys.foreign_keys e on a.object_id = e.parent_object_id
WHERE a.name = 'Orders'

--5
--ALTER TABLE Customers DROP CONSTRAINT PK_Customer 
-- Could not drop constraint. Because the constraint 'PK_Customer' is being referenced by table Orders of foreign key constraint
GO

--6
ALTER TABLE Orders DROP CONSTRAINT order_qty 
GO

--Functions & Procedures

--1
CREATE FUNCTION Func_Find_Age(@dob DATETIME)
RETURNS INT AS BEGIN
	RETURN DATEDIFF(YEAR, @dob, GETDATE())
END
GO
--select dbo.Func_Find_Age('2011-01-01')

--2
CREATE TABLE Students(
	SID INT NOT NULL PRIMARY KEY,
	StudentName	VARCHAR(50)	NOT NULL,
	DOB DATETIME NOT NULL
)
GO

CREATE PROCEDURE InsertStud(@Name VARCHAR(50), @dob DATETIME)
AS BEGIN
 SET NOCOUNT ON
	DECLARE @SID INT
	IF EXISTS (SELECT 1 FROM Students)
		BEGIN
			SELECT @SID = MAX(SID) + 1 FROM Students
		END
	ELSE
		BEGIN 
			SELECT @SID = 1
		END 
	INSERT INTO Students(SID, StudentName, DOB) VALUES (@SID, @Name, @dob)
END
GO
--EXEC InsertStud 'S1','1999-01-01'