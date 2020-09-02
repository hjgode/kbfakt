net stop spooler
del %WINDIR%\System32\spool\drivers\w32x86\3\EPCOMP24.GPD
del %WINDIR%\System32\spool\drivers\w32x86\3\EPCOMP24.BUD
copy EPCOMP24.GPD %WINDIR%\System32\spool\drivers\w32x86\3\EPCOMP24.GPD
net start spooler
pause
