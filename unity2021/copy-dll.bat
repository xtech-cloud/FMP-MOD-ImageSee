
@echo off

REM !!! Generated by the fmp-cli 1.88.0.  DO NOT EDIT!

md ImageSee\Assets\3rd\fmp-xtc-imagesee

cd ..\vs2022
dotnet build -c Release

copy fmp-xtc-imagesee-lib-mvcs\bin\Release\netstandard2.1\*.dll ..\unity2021\ImageSee\Assets\3rd\fmp-xtc-imagesee\
