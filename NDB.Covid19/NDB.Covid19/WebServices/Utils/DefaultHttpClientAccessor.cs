﻿using System;
using System.Net;
using System.Net.Http;

namespace NDB.Covid19.WebServices.Utils
{
    public interface IHttpClientAccessor
    {
        HttpClient HttpClient { get; }
        CookieContainer Cookies { get; }
    }

    public class DefaultHttpClientAccessor : IHttpClientAccessor
    {
        public DefaultHttpClientAccessor()
        {
            Cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = Cookies;
            HttpClient = new HttpClient(handler);
            HttpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public HttpClient HttpClient { get; }
        public CookieContainer Cookies { get; }
    }
}