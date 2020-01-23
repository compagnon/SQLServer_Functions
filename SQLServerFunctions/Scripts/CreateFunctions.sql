--------------------------------------------------------------------------------
--     Ce code a été généré par un outil.
--
--     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
--     le code est régénéré.
--------------------------------------------------------------------------------

CREATE FUNCTION [dbo].[XIRR] (@cashflows [nvarchar](MAX), @cashflowsColumn [nvarchar](MAX), @dates [nvarchar](MAX), @datesColumn [nvarchar](MAX))
RETURNS [float]
AS EXTERNAL NAME [SQLServerFunctions].[UserDefinedFunctions].[XIRR];

GO

CREATE FUNCTION [dbo].[XIRR_detail] (@cashflows [nvarchar](MAX), @cashflowsColumn [nvarchar](MAX), @dates [nvarchar](MAX), @datesColumn [nvarchar](MAX), @estimate [float], @maxFloatingPoints [int], @maxNbDecimalDigits [int])
RETURNS [float]
AS EXTERNAL NAME [SQLServerFunctions].[UserDefinedFunctions].[XIRR_detail];

GO

CREATE FUNCTION [dbo].[FindInvalidEmails] (@modifiedSince [datetime])
RETURNS TABLE (CustomerId int, EmailAddress nvarchar(4000))
AS EXTERNAL NAME [SQLServerFunctions].[UserDefinedFunctions].[FindInvalidEmails];

GO

CREATE FUNCTION [dbo].[AddVAT] (@originalAmount [float])
RETURNS [float]
AS EXTERNAL NAME [SQLServerFunctions].[UserDefinedFunctions].[AddVAT];

GO

CREATE FUNCTION [dbo].[RemVAT] (@amount [float])
RETURNS [float]
AS EXTERNAL NAME [SQLServerFunctions].[UserDefinedFunctions].[RemVAT];

GO

CREATE FUNCTION [dbo].[GetVAT] ()
RETURNS [float]
AS EXTERNAL NAME [SQLServerFunctions].[UserDefinedFunctions].[GetVAT];

GO

CREATE FUNCTION [dbo].[ActiveContract] ()
RETURNS TABLE (InvsIT nvarchar(5), InvsIN nvarchar(20), InvsKey nvarchar(100),InvsId nvarchar(4000),InvsReference1 nvarchar(200), InvsReference2 nvarchar(200), Phase nvarchar(10))
AS EXTERNAL NAME [SQLServerFunctions].[UserDefinedFunctions].[ActiveContract];

GO