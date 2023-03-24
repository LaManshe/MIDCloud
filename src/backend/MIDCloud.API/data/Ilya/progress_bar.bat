@echo off
setlocal enabledelayedexpansion

set /a count=0
for /l %%x in (1,1,10) do (
    set /a count+=10
    ping -n 1 -w 1000 127.0.0.1 > nul
    echo !count!%% complete...
)

echo Done!
pause