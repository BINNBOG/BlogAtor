#!/bin/bash

if [ `id -u` -eq '0' ]; then
    echo "should not be run as root"
    exit 1
fi

# install dotnet entity framework tool
dotnet tool install --global dotnet-ef
