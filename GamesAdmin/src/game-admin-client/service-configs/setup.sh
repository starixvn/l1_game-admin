#!/bin/bash
SOURCE_PATH=/var/www/nex3/game-admin-client

helpFunction()
{
  echo "Usage: $0 [env]"
  echo "Supported Environments: test, uat, sit, production"
  exit 1
}

if [ "$1" = "test" ] || [ "$1" = "uat" ] || [ "$1" = "sit" ] || [ "$1" = "production" ]
then
  SERVICE_NAME="game-admin-client-$1.service"
  systemctl link "$SOURCE_PATH/service-configs/$SERVICE_NAME"
  systemctl enable "$SERVICE_NAME"
  systemctl start "$SERVICE_NAME"
else
  helpFunction
fi