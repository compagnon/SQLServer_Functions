/*
-- Interesting contributions
-- https://sqlquantumleap.com/2017/08/09/sqlclr-vs-sql-server-2017-part-2-clr-strict-security-solution-1/
-- https://sqlquantumleap.com/2017/08/16/sqlclr-vs-sql-server-2017-part-3-clr-strict-security-solution-2/
-- https://docs.microsoft.com/en-us/sql/relational-databases/clr-integration/security/clr-integration-code-access-security?view=sql-server-2017
*/

-- Enable show advanced options on the server
sp_configure 'show advanced options',1
RECONFIGURE
GO
-- Enable clr on the server
sp_configure 'clr enabled',1
RECONFIGURE
GO


DROP LOGIN [ExcelFunctionCLR-Login] 
DROP CERTIFICATE [ExcelFunctionCLR]

DROP MASTER KEY
GO

--activate  the security for being able to add SAFE CLR
sp_configure 'CLR strict security',1
RECONFIGURE
GO



