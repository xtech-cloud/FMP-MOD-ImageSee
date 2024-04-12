
@echo off

REM !!! Generated by the fmp-cli 1.88.0.  DO NOT EDIT!

for /F %%i in ('git tag --contains') do ( set VERSION=%%i)
IF "%VERSION%" ==""  (
    echo [31m tag is required! [0m
    pause
    EXIT
)

set WORK_DIR=%cd%
IF NOT EXIST .UNITY_HOME.env (
    echo [31m .UNITY_HOME.env is required ! [0m
    pause
    EXIT
)

set /P UNITY_HOME=<.UNITY_HOME.env
IF NOT EXIST %UNITY_HOME% (
    echo [31m %UNITY_HOME% not found ! [0m
    pause
    EXIT
)

mkdir _build_
mkdir _dist_
DEL /Q/S _dist_\*.dll
copy Unity.csproj.keep _build_\Unity.csproj
xcopy /Q/S/Y ImageSee\Assets\Scripts\Module _build_\Module\
cd _build_
powershell -Command "(gc Unity.csproj) -replace '{{UNITY_HOME}}', '%UNITY_HOME%' | Out-File Unity.csproj"
powershell -Command "(gc Unity.csproj) -replace '{{WORK_DIR}}', '%WORK_DIR%' | Out-File Unity.csproj"
powershell -Command "(gc Unity.csproj) -replace '{{VERSION}}', '%VERSION%' | Out-File Unity.csproj"

dotnet build -c=Release
cd ..
DEL /Q/S _build_\bin\Release\netstandard2.1\Unity*
move _build_\bin\Release\netstandard2.1\*.Unity.dll .\_dist_\
RD /Q/S _build_
pause
