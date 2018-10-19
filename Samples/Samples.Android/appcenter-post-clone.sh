#!/usr/bin/env bash

echo "Variables:"

# Updating manifest
sed -i '' "s/AC_ANDROID/$AC_ANDROID/g" $BUILD_REPOSITORY_LOCALPATH/Samples/Samples/App.xaml.cs

echo "Manifest updated!"