
@echo off

REM !!! Generated by the fmp-cli 1.70.0.  DO NOT EDIT!

md Search\Assets\3rd\fmp-xtc-search

cd ..\vs2022
dotnet build -c Release

copy fmp-xtc-search-lib-mvcs\bin\Release\netstandard2.1\*.dll ..\unity2021\Search\Assets\3rd\fmp-xtc-search\
