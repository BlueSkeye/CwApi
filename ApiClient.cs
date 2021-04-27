using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;

using ApiAuth;

namespace CwApi
{
    public class ApiClient
    {
        private string CyberwatchPrefix = "Cyberwatch";
        private const string PingPath = "/api/v3/ping";
        private string _apiKey;
        private SecureString _apiSecret;
        private Uri _baseUri;

        public ApiClient(Uri baseUri, string keyId, SecureString secretKey)
        {
            try { _baseUri = ValidateBaseUri(baseUri); }
            catch (Exception e) {
                throw new ArgumentException(nameof(baseUri), e);
            }
            _apiKey = keyId ?? throw new ArgumentNullException(nameof(keyId));
            _apiSecret = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
        }

        private void Authenticate(HttpWebRequest request)
        {
            string now = DateTime.UtcNow.ToString("R");
            string signedMessage = string.Format("{0},{1},,{2},{3}",
                request.Method, request.ContentType, request.RequestUri, now);
            HMACSHA256 hasher = new HMACSHA256();
        }

        private void Decorate(HttpWebRequest request)
        {
            if (0 < request.ContentLength) {
                request.ContentType = "application/json";
            }
            Authenticate(request);
        }

        private string ExtractResponse(HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream()) {
                using (MemoryStream fullResponse = new MemoryStream()) {
                    byte[] buffer = new byte[4096];
                    while (true) {
                        int inputBytes = responseStream.Read(buffer, 0, buffer.Length);
                        if (0 == inputBytes) {
                            break;
                        }
                        fullResponse.Write(buffer, 0, inputBytes);
                    }
                    string responseText = Encoding.ASCII.GetString(fullResponse.GetBuffer());
                    Console.WriteLine(responseText);
                    return responseText;
                }
            }
        }
        
        private HttpClient GetClient()
        {
            HttpClient client = HttpClient.Create();
            client.ApiKeyId = _apiKey;
            client.ApiSecretKey = _apiSecret;
            client.AuthorizationPrefix = CyberwatchPrefix;
            return client;
        }

        private Uri GetUri(string path)
        {
            return new Uri(_baseUri, path ?? throw new ArgumentNullException(nameof(path)));
        }

        public string Ping()
        {
            Uri pingUri = GetUri(PingPath);
            using (HttpClient client = GetClient()) {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(pingUri);
                HttpWebResponse response = client.Send(request);
                return ExtractResponse(response);
            }
        }

        private static Uri ValidateBaseUri(Uri candidate)
        {
            if (null == candidate) {
                throw new ArgumentNullException(nameof(candidate));
            }
            //if ("HTTPS" != candidate.Scheme.ToUpper()) {
            //    throw new ArgumentException("HTTPS scheme is required.");
            //}
            // TODO : consider additional checks.
            return candidate;
        }
    }
}
