
/** 
TO install once the certificate in the MS SQL Server instance, the same certificate is used for signing the DLL / Assembly
MUST BE DONE OUTSIDE VisualStudio , because "GRANT" is not allowed...
*/
-- use master
-- GO

CREATE MASTER KEY ENCRYPTION BY PASSWORD = '23987hxJ#KL95234nl0zBe';  
GO  

-- the certificate of visual studio is a PFX 
CREATE CERTIFICATE ExcelFunctionCLR --AUTHORIZATION [$(DatabaseUser)]
	FROM FILE = '$(ProjectDir)$(CertPublicKeyPath)'   
	WITH PRIVATE KEY (FILE = '$(ProjectDir)$(PVKPrivateKeyPath)',
		DECRYPTION BY PASSWORD = '$(CertificatePassword)' );
GO    


-- create the login in the database if needed
-- create a login  to add permission "UNSAFE ASSEMBLY" to the certificate
/*
CREATE LOGIN [ExcelFunctionCLR-Login] FROM CERTIFICATE ExcelFunctionCLR
GO

GRANT UNSAFE ASSEMBLY TO [ExcelFunctionCLR-Login]
GO
*/



