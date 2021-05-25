﻿using System;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using UIKit;

namespace NDB.Covid19.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e, "iOS crashed (logged in Main.cs)");
                throw;
            }
        }
    }
}