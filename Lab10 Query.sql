-- H�mta alla produkter med deras namn, pris och kategori namn. Sortera p� kategori namn och sen produkt namn
SELECT ProductName, UnitPrice, Categories.CategoryName FROM Products
JOIN Categories ON Products.CategoryID=Categories.CategoryID
ORDER BY CategoryName, ProductName;

--H�mta alla kunder och antal ordrar de gjort. Sortera fallande p� antal ordrar.
SELECT CompanyName, count(Orders.OrderID) as OrderCount FROM Customers
JOIN Orders ON Customers.CustomerID=Orders.CustomerID
GROUP BY CompanyName
ORDER BY OrderCount DESC;

--H�mta alla anst�llda tillsammans med territorie de har hand om (EmployeeTerritories och Territories tabellerna)
SELECT FirstName, LastName, Title, TerritoryDescription FROM Employees
JOIN EmployeeTerritories ON Employees.EmployeeID=EmployeeTerritories.EmployeeID
JOIN Territories ON EmployeeTerritories.TerritoryID=Territories.TerritoryID
ORDER BY LastName;
