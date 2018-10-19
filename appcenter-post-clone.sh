#!/usr/bin/env bash

echo "Variables:"

# Updating manifest
sed -i '' "s/AC_IOS/$AC_IOS/g" $BUILD_REPOSITORY_LOCALPATH/Samples/Samples/App.xaml.cs

sed -i '' "s/APP_SECRET/$APP_SECRET/g" $BUILD_REPOSITORY_LOCALPATH/Samples/Samples.iOS/Info.plist

echo "Manifest updated!"