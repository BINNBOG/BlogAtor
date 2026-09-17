#!/bin/bash

if [ `id -u` -ne '0' ]; then
    echo "root priveleges are required"
    exit 1
fi

# add MS repo
sudo add-apt-repository ppa:dotnet/backports
# install dotnet SDK
apt-get update && apt-get install -y dotnet-sdk-9.0
