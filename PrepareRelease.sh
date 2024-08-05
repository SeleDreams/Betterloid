#!/bin/bash

mkdir -p release5/Plugins/AboutBetterloid
cp -r VOCALOID5/Plugins/AboutBetterloid/* release5/Plugins/AboutBetterloid/
mkdir -p release5/Plugins/BetterloidCore
cp -r VOCALOID5/Plugins/BetterloidCore/* release5/Plugins/BetterloidCore/
mkdir -p release5/Plugins/Betterloid
cp -r VOCALOID5/Plugins/Betterloid/* release5/Plugins/Betterloid/
cp VOCALOID5/Betterloid.json release5/Betterloid.json
cp hook/Newtonsoft.Json.V5.dll release5/Newtonsoft.Json.dll

mkdir -p release6/Plugins/AboutBetterloid
cp -r VOCALOID6/Plugins/AboutBetterloid/* release6/Plugins/AboutBetterloid/
mkdir -p release6/Plugins/BetterloidCore
cp -r VOCALOID6/Plugins/BetterloidCore/* release6/Plugins/BetterloidCore/
mkdir -p release6/Plugins/Betterloid
cp -r VOCALOID6/Plugins/Betterloid/* release6/Plugins/Betterloid/
cp VOCALOID6/Betterloid.json release6/Betterloid.json
cp hook/Newtonsoft.Json.V6.dll release6/Newtonsoft.Json.dll

zip -r Betterloid5.zip release5
rm -rf release5

zip -r Betterloid6.zip release6
rm -rf release6
