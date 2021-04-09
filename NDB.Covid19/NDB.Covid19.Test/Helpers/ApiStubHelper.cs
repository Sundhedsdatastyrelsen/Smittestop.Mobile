﻿using NDB.Covid19.Configuration;
using WireMock.Server;
using WireMock.Settings;

namespace NDB.Covid19.Test.Helpers
{
    public class ApiStubHelper
    {
        public static WireMockServer StubServer;
        public static string StubServerUrl { get; private set; }

        public static void StartServer()
        {
            StubServerUrl = Conf.BaseUrl.Remove(Conf.BaseUrl.Length - 1, 1);
            StubServer = WireMockServer.Start(new FluentMockServerSettings
            {
                Urls = new[] {StubServerUrl}
            });
        }

        public static void StopServer()
        {
            StubServer.Stop();
        }
    }
}