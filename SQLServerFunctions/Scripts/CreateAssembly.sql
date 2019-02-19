/**
in case of not working *automatic* scripts
*/

CREATE ASSEMBLY ExcelFunction
AUTHORIZATION [dbo]
FROM 'C:\data\calculPerf\lib\InterestRateOfReturn.dll'
--WITH PERMISSION_SET = SAFE;
GO

CREATE ASSEMBLY SQLServerFunctions
AUTHORIZATION [dbo]
FROM N'C:\data\calculPerf\lib\SQLServerFunctions.dll'
--WITH PERMISSION_SET = SAFE;
GO
