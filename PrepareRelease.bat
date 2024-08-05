robocopy VOCALOID5\Plugins\AboutBetterloid release5\Plugins\AboutBetterloid /s /e
robocopy VOCALOID5\Plugins\BetterloidCore release5\Plugins\BetterloidCore /s /e
robocopy VOCALOID5\Plugins\Betterloid release5\Plugins\Betterloid /s /e
copy VOCALOID5\Betterloid.json release5\Betterloid.json /Y
copy hook\Newtonsoft.Json.V5.dll release5\Newtonsoft.Json.dll /Y

robocopy VOCALOID6\Plugins\AboutBetterloid release6\Plugins\AboutBetterloid /s /e
robocopy VOCALOID6\Plugins\BetterloidCore release6\Plugins\BetterloidCore /s /e
robocopy VOCALOID6\Plugins\Betterloid release6\Plugins\Betterloid /s /e
copy VOCALOID6\Betterloid.json release6\Betterloid.json /Y
copy hook\Newtonsoft.Json.V6.dll release6\Newtonsoft.Json.dll /Y

powershell Compress-Archive -Path "%cd%/release5" -DestinationPath "%cd%/Betterloid5.zip"
rmdir /S /Q release5

powershell Compress-Archive -Path "%cd%/release6" -DestinationPath "%cd%/Betterloid6.zip"
rmdir /S /Q release6