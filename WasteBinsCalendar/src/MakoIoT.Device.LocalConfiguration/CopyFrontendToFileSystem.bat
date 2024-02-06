@echo off
set p7z="C:\Program Files\7-Zip\7z.exe"
del FileSystem\index.html.gz
%p7z% a -tgzip "FileSystem\index.html.gz" "Frontend\dist\index.html"
del FileSystem\index.css.gz
%p7z% a -tgzip "FileSystem\index.css.gz" "Frontend\dist\index.css"
del FileSystem\index.js.gz
%p7z% a -tgzip "FileSystem\index.js.gz" "Frontend\dist\index.js"
del FileSystem\favicon.ico.gz
%p7z% a -tgzip "FileSystem\favicon.ico.gz" "Frontend\dist\favicon.ico"
del FileSystem\info-square.svg.gz
%p7z% a -tgzip "FileSystem\info-square.svg.gz" "Frontend\dist\info-square.svg"
