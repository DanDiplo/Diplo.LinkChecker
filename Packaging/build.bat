ECHO off

SET /P APPVEYOR_BUILD_NUMBER=Please enter a build number (e.g. 130):
SET /P PACKAGE_VERISON=Please enter your package version (e.g. 1.3.0):
SET /P UMBRACO_PACKAGE_PRERELEASE_SUFFIX=Please enter your package release suffix or leave empty (e.g. beta):

SET /P APPVEYOR_REPO_TAG=If you want to simulate a GitHub tag for a release (e.g. true):

if "%APPVEYOR_BUILD_NUMBER%" == "" (
  SET APPVEYOR_BUILD_NUMBER=
)
if "%PACKAGE_VERISON%" == "" (
  SET PACKAGE_VERISON=1.3.0
)


SET UMBRACO_PACKAGE_PRERELEASE_SUFFIX=

SET APPVEYOR_BUILD_VERSION=%PACKAGE_VERISON%.%APPVEYOR_BUILD_NUMBER%

build-appveyor.bat

@IF %ERRORLEVEL% NEQ 0 GOTO err
@EXIT /B 0
:err
@PAUSE
@EXIT /B 1