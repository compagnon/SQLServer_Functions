--CREATE CERTIFICATE [Certificate1]
--	ENCRYPTION BY PASSWORD = 'd1#rh,btqds|Bnv{nmraY>@wmsFT7_&#$!~<Nb!t=dpqa:{8'
--	WITH SUBJECT = 'certificate_subject'


CREATE CERTIFICATE ExcelFunctionCLR --AUTHORIZATION [$(DatabaseUser)]
	FROM FILE = 'SQLServerPK.pfx_1.cer'   
	WITH PRIVATE KEY (FILE = 'SQLServerPK.pfx_1.pvk',
		DECRYPTION BY PASSWORD = '[$(CertificatePassword)]' );
GO    
