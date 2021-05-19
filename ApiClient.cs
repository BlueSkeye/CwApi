using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using ApiAuth;

namespace CwApi
{
    public class ApiClient
    {
        private string CyberwatchPrefix = "Cyberwatch";
        private const string DiscoveredAssetsPath = "/api/v3/hosts";
        private const string GroupPathPattern = "/api/v3/groups/{0}";
        private const string GroupsPath = "/api/v3/groups";
        private const string PingPath = "/api/v3/ping";
        private const string ServersPath = "/api/v3/servers";
        private string _apiKey;
        private SecureString _apiSecret;
        private Uri _baseUri;
        private JsonSerializerOptions jsonDeserializationOptions =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

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

        private T DeserializeResponse<T>(string from)
        {
            return JsonSerializer.Deserialize<T>(from, jsonDeserializationOptions);
        }

        private T ExtractResponse<T>(HttpWebResponse response)
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
                    string responseText = Encoding.ASCII.GetString(fullResponse.ToArray());
                    return DeserializeResponse<T>(responseText);
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

        private Uri GetUri(string path, PaginationRequest pagination = null)
        {
            if (null == pagination) {
                pagination = new PaginationRequest();
            }
            path = (path ?? throw new ArgumentNullException(nameof(path))) +
                string.Format("?per_page={0}&page={1}",
                    pagination.PageSize, pagination.PageNumber);
            return new Uri(_baseUri, path);
        }

        public List<HostResult> GetHosts(PaginationRequest pagination = null)
        {
            return GetPaginatedResults<HostResult>(DiscoveredAssetsPath, pagination);
        }

        public GroupResult GetGroup(int groupId)
        {
            Uri uri = GetUri(string.Format(GroupPathPattern, groupId));
            using (HttpClient client = GetClient()) {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                HttpWebResponse response = client.Send(request);
                return (null == response)
                    ? null
                    : ExtractResponse<GroupResult>(response);
            }
        }

        public List<GroupResult> GetGroups(PaginationRequest pagination = null)
        {
            return GetPaginatedResults<GroupResult>(GroupsPath, pagination);
        }

        private List<T> GetPaginatedResults<T>(string uriPath,
            PaginationRequest pagination = null)
        {
            List<T> compositeResult = null;
            if (null == pagination) {
                pagination = new PaginationRequest();
            }
            using (HttpClient client = GetClient()) {
                while (true) {
                    Uri uri = GetUri(uriPath, pagination);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    HttpWebResponse response = client.Send(request);
                    if (null == response) {
                        return null;
                    }
                    NameObjectCollectionBase.KeysCollection keys =
                        response.Headers.Keys;
                    string linkValue = response.Headers.Get("Link");
                    int? lastPageNumber = null;
                    int? nextPageNumber = null;
                    if (null != linkValue) {
                        ParseLink(linkValue, out lastPageNumber, out nextPageNumber);
                    }
                    if ((null != nextPageNumber) && (null == compositeResult)) {
                        compositeResult = new List<T>();
                    }
                    List <T> thisResult = ExtractResponse<List<T>>(response);
                    if (null != compositeResult) {
                        compositeResult.AddRange(thisResult);
                    }
                    if (null == nextPageNumber) {
                        return compositeResult ?? thisResult;
                    }
                    pagination.PageNumber += 1;
                }
            }
        }

        public List<ServerResult> GetServers(PaginationRequest pagination = null)
        {
            return GetPaginatedResults<ServerResult>(ServersPath, pagination);
        }

        private static void ParseLink(string linkValue, out int? lastPageNumber,
            out int? nextPageNumber)
        {
            string[] lines = linkValue.Split(",");
            lastPageNumber = null;
            nextPageNumber = null;
            foreach(string line in lines) {
                int? pageValue = ExtractPageIndex(line);
                if (null == pageValue) {
                    continue;
                }
                if (line.EndsWith("; rel=\"last\"")) {
                    lastPageNumber = pageValue;
                }
                else if (line.EndsWith("; rel=\"next\"")) {
                    nextPageNumber = pageValue;
                }
                else if (line.EndsWith("; rel=\"prev\"")
                    || line.EndsWith("; rel=\"first\"")) {
                    // Do nothing
                }
                else {
                    Console.WriteLine("WARNING : Unknown page relation kind.");
                    Console.WriteLine(line);
                }
            }
        }

        private static int? ExtractPageIndex(string from)
        {
            string searchedTag = "?page=";
            int pageIndex = from.IndexOf(searchedTag);
            if (0 > pageIndex) {
                return null;
            }
            int pageNumberStartIndex = pageIndex + searchedTag.Length;
            int pageNumberEndIndex;
            for(int index = pageNumberStartIndex; ; index++) {
                if ((index >= from.Length) || !char.IsDigit(from[index])) {
                    pageNumberEndIndex = index - 1;
                    break;
                }
            }
            if (pageNumberEndIndex < pageNumberStartIndex) {
                return null;
            }
            string indexRawValue = from.Substring(pageNumberStartIndex,
                pageNumberEndIndex - pageNumberStartIndex + 1);
            return int.Parse(indexRawValue);
        }

        public PingResult Ping()
        {
            Uri uri = GetUri(PingPath);
            using (HttpClient client = GetClient()) {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                HttpWebResponse response = client.Send(request);
                return (null == response)
                    ? null
                    : ExtractResponse<PingResult>(response);
            }
        }

        private static Uri ValidateBaseUri(Uri candidate)
        {
            if (null == candidate) {
                throw new ArgumentNullException(nameof(candidate));
            }
            if ("HTTPS" != candidate.Scheme.ToUpper()) {
                throw new ArgumentException("HTTPS scheme is required.");
            }
            // TODO : consider additional checks.
            return candidate;
        }
    }
}
