SELECT * 
   FROM OPENROWSET('Microsoft.Jet.OLEDB.4.0',
      'D:\KFZEURO\kbfakt\kbfakt05\kbfakt05.mdb';
      'admin';'',RECH1)
GO
SELECT     RECH1.*
INTO            RECH1_
FROM         RECH1
go