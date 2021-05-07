@echo off
pushd %~dp0
goto check_Permissions
:check_Permissions
    net session >nul 2>&1
    if %errorLevel% == 0 (
        goto make_link
    ) else (
        echo "Please restart the setup as an administrator"
        pause >nul
        exit /b 1
    )
:make_link
    rmdir VOCALOID5
    set /p location="Please Specify the location of your VOCALOID5 installation folder (MAKE SURE TO USE QUOTING MARKS):"
    if %location% == "" (
        echo "The input is invalid !"
        pause >nul
        exit /b 1
    )
    mklink /J VOCALOID5 %location%
    if %errorLevel% == 0 (
        echo "Succesfully created symlink, you can now start working on your plugin"
        pause >nul
        exit /b 0
    ) else (
        echo "An error occured when making the symlink, make sure there are no errors in your path and that the symlink doesn't already exist"
        pause >nul
        exit /b 1
    )