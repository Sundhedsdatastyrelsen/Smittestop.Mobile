﻿using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using NDB.Covid19.Enums;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using Uri = Android.Net.Uri;

namespace NDB.Covid19.Droid.OAuth2
{
    /// <summary>
    ///     This Activity is hit when redirecting from NemId in the browser
    /// </summary>
    [Activity(
        Label = "AuthUrlSchemeInterceptorActivity",
        LaunchMode = LaunchMode.SingleTop,
        NoHistory = true,
        Name = "md52ecc484fd43c6baf7f3301c3ba1d0d0c.AuthUrlSchemeInterceptorActivity")]
    [
        IntentFilter
        (
            new[] {Intent.ActionView},
            Categories = new[]
            {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            },
            DataSchemes = new[]
            {
                "com.netcompany.smittestop"
            },
            DataPath = "/oauth2redirect"
        )
    ]
    public class AuthUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Uri uri_android = Intent.Data;

                // Convert Android.Net.Url to C#/netxf/BCL System.Uri - common API
                System.Uri uri_netfx = new System.Uri(uri_android.ToString());

                // load redirect_url Page for parsing
                AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

                Finish();
            }
            catch (Exception e)
            {
                // Log if Intent is null or Intent.Data is null or Intent.Data
                string error = Intent == null ? "Intent was null" :
                    Intent.Data == null ? "Intent.Data was null" : "Intent.Data: " + Intent.Data.ToString();

                LogUtils.LogException(LogSeverity.WARNING, e,
                    nameof(AuthUrlSchemeInterceptorActivity) + " " + nameof(OnCreate) +
                    " error when redirectin to app after NemID validation", error);

                // Redirect and hit OnAuthError
                AuthenticationState.Authenticator.OnPageLoading(
                    new System.Uri("com.netcompany.smittestop:/oauth2redirect"));

                Finish();
            }
        }
    }
}