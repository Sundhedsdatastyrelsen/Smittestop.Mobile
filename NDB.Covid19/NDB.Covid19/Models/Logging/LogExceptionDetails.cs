using System;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Models.Logging
{
    public class LogExceptionDetails
    {
        private readonly int _maxLengthOfStacktrace = 1000; //number of characters

        public LogExceptionDetails(Exception e)
        {
            if (e == null)
            {
                return;
            }

            ExceptionType = e.GetType().Name;
            ExceptionMessage = Anonymizer.RedactText(e.Message);
            ExceptionStackTrace = Anonymizer.RedactText(ShortenedText(e.StackTrace));

            if (e.InnerException != null)
            {
                InnerExceptionType = e.InnerException.GetType().Name;
                InnerExceptionMessage = Anonymizer.RedactText(e.InnerException.Message);
                InnerExceptionStackTrace = Anonymizer.RedactText(ShortenedText(e.InnerException.StackTrace));
            }
        }

        public string ExceptionType { get; }
        public string ExceptionMessage { get; }
        public string ExceptionStackTrace { get; }
        public string InnerExceptionType { get; }
        public string InnerExceptionMessage { get; }
        public string InnerExceptionStackTrace { get; }

        private string ShortenedText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= _maxLengthOfStacktrace)
            {
                return text;
            }

            return text.Substring(0, _maxLengthOfStacktrace);
        }
    }
}