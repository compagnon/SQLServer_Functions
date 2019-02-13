/*
 Modèle de script de pré-déploiement							
--------------------------------------------------------------------------------------
 Ce fichier contient des instructions SQL qui seront exécutées avant le script de compilation.	
 Utilisez la syntaxe SQLCMD pour inclure un fichier dans le script de pré-déploiement.			
 Exemple :      :r .\monfichier.sql								
 Utilisez la syntaxe SQLCMD pour référencer une variable dans le script de pré-déploiement.		
 Exemple :      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
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
