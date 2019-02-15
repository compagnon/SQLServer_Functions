-- create a login  to add permission "UNSAFE ASSEMBLY" to the certificate

CREATE LOGIN [ExcelFunctionCLR-Login] FROM CERTIFICATE ExcelFunctionCLR
GO

GRANT UNSAFE ASSEMBLY TO [ExcelFunctionCLR-Login]
GO
