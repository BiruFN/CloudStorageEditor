using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using RestSharp;

namespace FortniteLauncher
{
    public class OAuth
    {
        public static string GetDeviceToken()
        {
            var client = new RestClient("https://account-public-service-prod03.ol.epicgames.com");
            var request = new RestRequest("/account/api/oauth/token", Method.Post);
            request.AddHeader("Authorization", "Basic OThmN2U0MmMyZTNhNGY4NmE3NGViNDNmYmI0MWVkMzk6MGEyNDQ5YTItMDAxYS00NTFlLWFmZWMtM2U4MTI5MDFjNGQ3");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");

            var response = client.Execute(request).Content;
            var token = JsonConvert.DeserializeObject<Token>(response);

            return token.access_token;
        }
        public static string GetDeviceCode(string deviceToken)
        {
            var client = new RestClient("https://account-public-service-prod03.ol.epicgames.com");
            var request = new RestRequest("/account/api/oauth/deviceAuthorization", Method.Post);
            request.AddHeader("Authorization", $"Bearer {deviceToken}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var response = client.Execute(request).Content;
            var deviceAuthorization = JsonConvert.DeserializeObject<DeviceAuthorization>(response);

            Process.Start(deviceAuthorization.verification_uri_complete);
            return deviceAuthorization.device_code;
        }
        public static string GetAccessToken(string deviceCode)
        {
            for (; ; )
            {
                var client = new RestClient("https://account-public-service-prod03.ol.epicgames.com");
                var request = new RestRequest("/account/api/oauth/token", Method.Post);
                request.AddHeader("Authorization", "Basic OThmN2U0MmMyZTNhNGY4NmE3NGViNDNmYmI0MWVkMzk6MGEyNDQ5YTItMDAxYS00NTFlLWFmZWMtM2U4MTI5MDFjNGQ3");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "device_code");
                request.AddParameter("device_code", deviceCode);

                var response = client.Execute(request).Content;
                var token = JsonConvert.DeserializeObject<Token>(response);
                var error = JsonConvert.DeserializeObject<Error>(response);

                if (token.access_token != null)
                {
                    return token.access_token;
                }
                if (error.errorCode == "errors.com.epicgames.account.oauth.authorization_pending")
                {
                    Thread.Sleep(1000);
                }
                if (error.errorCode == "errors.com.epicgames.not_found")
                {
                    Environment.Exit(0);
                }
            }
        }
        public static string GetExchangeCode(string accessToken)
        {
            var client = new RestClient("https://account-public-service-prod03.ol.epicgames.com");
            var request = new RestRequest("/account/api/oauth/exchange", Method.Get);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = client.Execute(request).Content;
            var exchange = JsonConvert.DeserializeObject<Exchange>(response);

            return exchange.code;
        }
    }
    public class Error
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }
    public class Token
    {
        public string access_token { get; set; }
        public string displayName { get; set; }
        public string account_id { get; set; }
    }
    public class DeviceAuthorization
    {
        public string device_code { get; set; }
        public string verification_uri_complete { get; set; }
    }
    public class Exchange
    {
        public string code { get; set; }
    }
}
