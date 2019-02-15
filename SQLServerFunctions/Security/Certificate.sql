use master

CREATE MASTER KEY ENCRYPTION BY PASSWORD = '23987hxJ#KL95234nl0zBe';  
GO  

-- the certificate of visual studio is a PFX 
CREATE CERTIFICATE ExcelFunctionCLR --AUTHORIZATION [$(DatabaseUser)]
	FROM FILE = '$(ProjectDir)$(CertPublicKeyPath)'   
	WITH PRIVATE KEY (FILE = '$(ProjectDir)$(PVKPrivateKeyPath)',
		DECRYPTION BY PASSWORD = '$(CertificatePassword)' );
GO    


:r .\Login.sql


--CREATE ASSEMBLY ExcelFunction
--AUTHORIZATION [dbo]
--FROM 'C:\data\calculPerf\lib\InterestRateOfReturn.dll'
----WITH PERMISSION_SET = SAFE;
--GO

--CREATE ASSEMBLY SQLExcelFunction
--AUTHORIZATION [dbo]
--FROM 'C:\data\calculPerf\lib\SQLServerFunctions.dll'
----WITH PERMISSION_SET = SAFE;
--GO

