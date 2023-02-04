#!/bin/bash

echo 'Checking for devices...'
numDevices=$(( $(./adb devices | wc -l) - 2 ))
sleep 1
if [[ $numDevices -lt 1 ]]; then
    echo 'No devices found. Please connect a device and start again'
    echo 'Press Enter to continue...'
    read
    exit 1
fi
wifiConnection=( $(./adb shell ip addr l wlan0 | sed -n '/inet /p') )
sleep 1
./adb tcpip 5555
sleep 1
ipAddress=$(echo ${wifiConnection[1]} | sed 's/\/.*$/:5555/')
echo 'Found IP address:' $ipAddress
echo 'Disconnect USB and press Enter to continue...'
read
./adb connect $ipAddress
echo 'Connected. Press Enter to finish.'
read