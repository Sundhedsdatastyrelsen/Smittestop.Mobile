#!/bin/bash
#
# Src: https://github.com/microsoft/appcenter/blob/master/sample-build-scripts/xamarin/app-constants/appcenter-pre-build.sh
#   
#    MIT License
#
#    Copyright (c) Microsoft Corporation. All rights reserved.
#
#    Permission is hereby granted, free of charge, to any person obtaining a copy
#    of this software and associated documentation files (the "Software"), to deal
#    in the Software without restriction, including without limitation the rights
#    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
#    copies of the Software, and to permit persons to whom the Software is
#    furnished to do so, subject to the following conditions:
#
#    The above copyright notice and this permission notice shall be included in all
#    copies or substantial portions of the Software.
#
#    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
#    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
#    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
#    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
#    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
#    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
#    SOFTWARE
#
# For Xamarin, change some constants located in some class of the app.
# In this sample, suppose we have an AppConstant.cs class in shared folder with follow content:
#
# namespace Core
# {
#     public class AppConstant
#     {
#         public const string ApiUrl = "https://CMS_MyApp-Eur01.com/api";
#     }
# }
# 
# Suppose in our project exists two branches: master and develop. 
# We can release app for production API in master branch and app for test API in develop branch. 
# We just need configure this behaviour with environment variable in each branch :)
# 
# The same thing can be perform with any class of the app.
#
# AN IMPORTANT THING: FOR THIS SAMPLE YOU NEED DECLARE API_URL ENVIRONMENT VARIABLE IN APP CENTER BUILD CONFIGURATION.

if [ -z "BaseUrl" ]
then
    echo "You need define the BaseUrl variable in App Center"
    exit 1
fi

if [ -z "AuthHeader" ]
then
    echo "You need define the AuthHeader variable in App Center"
    exit 1
fi

if [ -z "FetchMinMinutes" ]
then
    echo "You need define the FetchMinMinutes variable in App Center"
    exit 1
fi

if [ -z "Oauth2ClientId" ]
then
    echo "You need define the Oauth2ClientId variable in App Center"
    exit 1
fi

if [ -z "Oauth2Scope" ]
then
    echo "You need define the Oauth2Scope variable in App Center"
    exit 1
fi

if [ -z "Oauth2RedirectUrl" ]
then
    echo "You need define the Oauth2RedirectUrl variable in App Center"
fi

if [ -z "Oauth2AuthoriseUrl" ]
then
    echo "You need define the Oauth2AuthoriseUrl variable in App Center"
fi

if [ -z "Oauth2AccessTokenUrl" ]
then
    echo "You need define the Oauth2AccessTokenUrl variable in App Center"
fi

if [ -z "Oauth2VerifyTokenPublicKey" ]
then
    echo "You need define the Oauth2VerifyTokenPublicKey variable in App Center"
fi

if [ -z "UseDevTools" ]
then
    echo "You need define the UseDevTools variable in App Center"
fi

CONF_FILE=$APPCENTER_SOURCE_DIRECTORY/NDB.Covid19/NDB.Covid19/config.json

if [ -e "$CONF_FILE" ]
then
    echo "Updating BaseUrl to $BaseUrl in config.json"
    sed -i '' 's#"BaseUrl": "[-A-Za-z0-9:_./]*"#"BaseUrl": "'$BaseUrl'"#' $CONF_FILE

    echo "Updating AuthHeader to $AuthHeader in config.json"
    sed -i '' 's#"AuthHeader": "[-A-Za-z0-9:_./]*"#"AuthHeader": "'$AuthHeader'"#' $CONF_FILE

    echo "Updating FetchMinMinutes to $FetchMinMinutes in config.json"
    sed -i '' 's#"FetchMinMinutes": [-A-Za-z0-9:_./]*#"FetchMinMinutes": '$FetchMinMinutes'#' $CONF_FILE

    echo "Updating Oauth2ClientId to $Oauth2ClientId in config.json"
    sed -i '' 's#"Oauth2ClientId": "[-A-Za-z0-9:_./]*"#"Oauth2ClientId": "'$Oauth2ClientId'"#' $CONF_FILE

    echo "Updating Oauth2Scope to $Oauth2Scope in config.json"
    sed -i '' 's#"Oauth2Scope": "[-A-Za-z0-9:_./]*"#"Oauth2Scope": "'$Oauth2Scope'"#' $CONF_FILE

    echo "Updating Oauth2RedirectUrl to $Oauth2RedirectUrl in config.json"
    sed -i '' 's#"Oauth2RedirectUrl": "[-A-Za-z0-9:_./]*"#"Oauth2RedirectUrl": "'$Oauth2RedirectUrl'"#' $CONF_FILE

    echo "Updating Oauth2AuthoriseUrl to $Oauth2AuthoriseUrl in config.json"
    sed -i '' 's#"Oauth2AuthoriseUrl": "[-A-Za-z0-9:_./]*"#"Oauth2AuthoriseUrl": "'$Oauth2AuthoriseUrl'"#' $CONF_FILE

    echo "Updating Oauth2AccessTokenUrl to $Oauth2AccessTokenUrl in config.json"
    sed -i '' 's#"Oauth2AccessTokenUrl": "[-A-Za-z0-9:_./]*"#"Oauth2AccessTokenUrl": "'$Oauth2AccessTokenUrl'"#' $CONF_FILE

    echo "Updating Oauth2VerifyTokenPublicKey to $Oauth2VerifyTokenPublicKey in config.json"
    sed -i '' 's#"Oauth2VerifyTokenPublicKey": "[-A-Za-z0-9:_./]*"#"Oauth2VerifyTokenPublicKey": "'$Oauth2VerifyTokenPublicKey'"#' $CONF_FILE

    echo "Updating UseDevTools to $UseDevTools in config.json"
    sed -i '' 's#"UseDevTools": [-A-Za-z0-9:_./]*#"UseDevTools": "'$UseDevTools'"#' $CONF_FILE
   
    echo "File content:"
    cat $CONF_FILE
    echo "Finished updating $CONF_FILE"
else
    echo "$CONF_FILE is missing"
    exit 1
fi