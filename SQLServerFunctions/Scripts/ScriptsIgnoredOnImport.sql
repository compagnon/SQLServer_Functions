--https://sqlquantumleap.com/2017/08/09/sqlclr-vs-sql-server-2017-part-2-clr-strict-security-solution-1/
--https://sqlquantumleap.com/2017/08/16/sqlclr-vs-sql-server-2017-part-3-clr-strict-security-solution-2/

Use FSTDATA_TEST
GO

-- Enable show advanced options on the server
sp_configure 'show advanced options',1
RECONFIGURE
GO
-- Enable clr on the server
sp_configure 'clr enabled',1
RECONFIGURE
GO
--desactivate the security for being able to add SAFE CLR
sp_configure 'CLR strict security',1
RECONFIGURE
GO
-- https://docs.microsoft.com/en-us/sql/relational-databases/clr-integration/security/clr-integration-code-access-security?view=sql-server-2017



/* authorize a assembly
-- with the SHA2_512 hashcode 
exec sp_add_trusted_assembly 0x8893AD6D78D14EE43DF482E2EAD44123E3A0B684A8873C3F7...
*/


-- cette clé est crée par SQL Server
--CREATE CERTIFICATE ExcelFunction   
--   ENCRYPTION BY PASSWORD = 'SagisAM72!'  
--   WITH SUBJECT = 'Guillaume pour SAGIS PROD';
--   --EXPIRY_DATE = '20201031';  
--GO  

--BACKUP CERTIFICATE ExcelFunction TO FILE = 'C:\localGIT\compagnon\SQLServer_Functions\InterestRateOfReturn\ExcelFunction.cert'  
--    WITH PRIVATE KEY ( DECRYPTION BY PASSWORD = 'SagisAM72!' , FILE = 'C:\localGIT\compagnon\SQLServer_Functions\InterestRateOfReturn\ExcelFunction.key' ,   
--    ENCRYPTION BY PASSWORD = 'SagisAM72!' );  
--GO

-- Cette cle est créer par visual studio (pvx, puis avec l utiitaire PVKConverter , les clés CERT et PVK sont générées à partir de la clé Visual studio
CREATE MASTER KEY ENCRYPTION BY PASSWORD = '?@?@?@?@?@?@'  
DROP MASTER KEY

CREATE CERTIFICATE ExcelFunctionVSS   
    FROM FILE = 'C:\localGIT\compagnon\SQLServer_Functions\InterestRateOfReturn\Guillaume[AT]SAGIS.pfx_1.cer'   
    WITH PRIVATE KEY (FILE = 'C:\localGIT\compagnon\SQLServer_Functions\InterestRateOfReturn\Guillaume[AT]SAGIS.pfx_1.pvk',   
    DECRYPTION BY PASSWORD = '?@?@?@?@?@?@');
GO    


CREATE LOGIN [ExcelFunction-Login]
FROM CERTIFICATE ExcelFunctionVSS;

GRANT UNSAFE ASSEMBLY TO [ExcelFunction-Login];



 -- test avec une clé asymetrique
 CREATE ASYMMETRIC KEY ExcelFunctionSQL
     AUTHORIZATION [dbo]
     FROM EXECUTABLE FILE = 'C:\data\calculPerf\lib\InterestRateOfReturn.dll'

GRANT SecurityPermission
 ON CERTIFICATE ExcelFunctionVSS 


--Register the Library
CREATE ASSEMBLY ExcelFunction
AUTHORIZATION [dbo]
FROM 'C:\data\calculPerf\lib\InterestRateOfReturn.dll'
WITH PERMISSION_SET = SAFE;
GO

--unregister the assembly and the associated functions
DROP FUNCTION addVAT
GO

DROP ASSEMBLY ExcelFunction
GO


-- create the functions point
--CREATE FUNCTION addVAT (@inputOne float)
--RETURNS [float] WITH EXECUTE AS CALLER, RETURNS NULL ON NULL INPUT
--AS 
--EXTERNAL NAME IRRExcelFunctions.[Excel.Lib].[addVAT]
--GO

CREATE FUNCTION addVAT (@inputOne float)
RETURNS [float] WITH EXECUTE AS CALLER, RETURNS NULL ON NULL INPUT
AS 
EXTERNAL NAME ExcelFunction.[Excel.Lib.UserDefinedFunctions].[addVAT]
GO

select dbo.addVAT(2.3)


sp_configure 'clr enabled', 1
GO 

RECONFIGURE
GO