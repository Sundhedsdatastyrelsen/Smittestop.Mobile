using System;
using System.Diagnostics;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.Models;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using Xamarin.Auth;

namespace NDB.Covid19.ViewModels
{
    public class InformationAndConsentViewModel
    {
        private AuthenticationManager _authManager;
        private EventHandler<AuthErrorType> _onError;

        private EventHandler _onSuccess;

        public InformationAndConsentViewModel(EventHandler onSuccess,
            EventHandler<AuthErrorType> onError)
        {
            _onSuccess = onSuccess;
            _onError = onError;
            _authManager = new AuthenticationManager();
        }

        public static string INFORMATION_CONSENT_HEADER_TEXT => "INFOCONSENT_HEADER".Translate();
        public static string INFORMATION_CONSENT_CONTENT_TEXT => "INFOCONSENT_DESCRIPTION".Translate();
        public static string INFORMATION_CONSENT_NEMID_BUTTON_TEXT => "INFOCONSENT_LOGIN".Translate();

        public static string CLOSE_BUTTON_ACCESSIBILITY_LABEL => "SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON".Translate();

        public static string INFOCONSENT_TITLE => "INFOCONSENT_TITLE".Translate();
        public static string INFOCONSENT_BODY_ONE => "INFOCONSENT_BODY_ONE".Translate();
        public static string INFOCONSENT_BODY_TWO => "INFOCONSENT_BODY_TWO".Translate();
        public static string INFOCONSENT_DESCRIPTION_ONE => "INFOCONSENT_DESCRIPTION_ONE".Translate();
        public event EventHandler<AuthErrorType> OnError;
        public event EventHandler OnSuccess;

        public void Init()
        {
            OnError += _onError;
            OnSuccess += _onSuccess;
            _authManager.Setup(OnAuthCompleted, OnAuthError);
        }

        public void Cleanup()
        {
            if (OnError != null) OnError -= _onError;
            if (OnSuccess != null) OnSuccess -= _onSuccess;
            _onError = null;
            _onSuccess = null;
            if (_authManager != null) _authManager.Cleanup();
        }

        private void Unsubscribe()
        {
            if (OnError != null) OnError -= _onError;
            if (OnSuccess != null) OnSuccess -= _onSuccess;
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            Debug.Print("Auth errored");
            Debug.Print(e.Message);
            Debug.Print(e.Exception?.ToString());
            OnError?.Invoke(this, AuthErrorType.Unknown);
        }

        private void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            string errorMsgPrefix = $"{nameof(InformationAndConsentViewModel)}.{nameof(OnAuthCompleted)}: ";
            Debug.Print("Authenticated: " + e.IsAuthenticated);
            if ((e?.IsAuthenticated ?? false) && e.Account?.Properties != null &&
                e.Account.Properties.ContainsKey("access_token"))
            {
                LogUtils.LogMessage(LogSeverity.INFO,
                    errorMsgPrefix + "User returned from NemID after authentication and access_token exists.");

                //Access_token
                string token = e.Account?.Properties["access_token"];
                PersonalDataModel payload = _authManager.GetPayloadValidateJWTToken(token);

                if (payload == null)
                {
                    OnError?.Invoke(this, AuthErrorType.Unknown);
                }
                else
                {
                    //Expiration time
                    if (e.Account.Properties.TryGetValue("expires_in", out string expires))
                    {
                        int.TryParse(expires, out int expiresSeconds);
                        if (expiresSeconds > 0)
                        {
                            payload.TokenExpiration = DateTime.Now.AddSeconds(expiresSeconds);
                            LogUtils.LogMessage(LogSeverity.INFO, errorMsgPrefix + "Access-token expires timestamp",
                                payload.TokenExpiration.ToString());
                        }
                    }
                    else
                    {
                        LogUtils.LogMessage(LogSeverity.ERROR, errorMsgPrefix + "'expires_in' value does not exist");
                    }

                    SaveCovidRelatedAttributes(payload);

                    if (AuthenticationState.PersonalData.IsBlocked)
                    {
                        OnError?.Invoke(this, AuthErrorType.MaxTriesExceeded);
                    }
                    else
                    {
                        if (AuthenticationState.PersonalData.IsNotInfected)
                        {
                            OnError?.Invoke(this, AuthErrorType.NotInfected);
                        }
                        else
                        {
                            if (!payload.Validate() || AuthenticationState.PersonalData.UnknownStatus)
                            {
                                if (AuthenticationState.PersonalData.UnknownStatus)
                                {
                                    LogUtils.LogMessage(LogSeverity.ERROR,
                                        errorMsgPrefix + "Value Covid19_status = ukendt");
                                }

                                OnError?.Invoke(this, AuthErrorType.Unknown);
                            }
                            else
                            {
                                OnSuccess?.Invoke(this, null);
                            }
                        }
                    }
                }
            }
            else
            {
                //The user clicked back
                Restart();
            }
        }

        private void Restart()
        {
            Unsubscribe();
            if (_authManager != null) _authManager.Cleanup();
            _authManager = new AuthenticationManager();
            Init();
        }

        private void SaveCovidRelatedAttributes(PersonalDataModel payload)
        {
            AuthenticationState.PersonalData = payload;
        }
    }
}