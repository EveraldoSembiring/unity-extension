using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnityExtension
{
    public class HttpRequestBuilder
    {
        HttpRequest constructedRequest;

        internal List<KeyValuePair<string, string>> Queries;

        internal HttpRequestBuilder(string httpMethod)
        {
            constructedRequest = new HttpRequest();
            constructedRequest.Method = httpMethod;
            constructedRequest.Headers = new Dictionary<string, string>();
            Queries = new List<KeyValuePair<string, string>>();
        }

        public HttpRequestBuilder To(string url)
        {
            constructedRequest.Uri = url;
            return this;
        }

        public HttpRequestBuilder WithAccept(string mediaType)
        {
            constructedRequest.Headers["Accept"] = mediaType;
            return this;
        }

        public HttpRequestBuilder WithContentType(string mediaType)
        {
            this.constructedRequest.Headers["Content-Type"] = mediaType;
            return this;
        }

        public HttpRequestBuilder WithQuery(string key, string value, bool checkEscapeKey = true, bool checkEscapeValue = true)
        {
            string queryKey = checkEscapeKey ? "?" + Uri.EscapeDataString(key) : key;
            string queryValue = checkEscapeValue ? "?" + Uri.EscapeDataString(value) : value;
            Queries.Add(new KeyValuePair<string, string>(queryKey, queryValue));
            return this;
        }

        public HttpRequestBuilder WithJsonContent<T>(T content,
            string mediaType = "application/json", bool logJsonParseResult = false)
        {
            byte[] bodyBytes = null;
            try
            {
                string json = JsonUtility.ToJson(content);
                if(logJsonParseResult)
                {
                    GameLogger.Verbose(json);
                }
                bodyBytes = Encoding.UTF8.GetBytes(json);
            }
            catch (Exception)
            {
            }

            if(bodyBytes != null)
            {
                this.constructedRequest.Headers["Content-Type"] = mediaType;
                this.constructedRequest.BodyBytes = bodyBytes;
            }

            return this;
        }

        public HttpRequestBuilder WithBasicToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return this;
            }

            this.constructedRequest.Headers["Authorization"] = "Basic " + token;
            return this;
        }

        public HttpRequestBuilder WithBearerToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return this;
            }

            this.constructedRequest.Headers["Authorization"] = "Bearer " + token;
            return this;
        }

        /// <summary>
        /// Adds a header to the request
        /// </summary>
        /// <param name="name">header name</param>
        /// <param name="value">value</param>
        /// <returns>HttpRequestBuilder</returns>
        public HttpRequestBuilder WithHeader(string name, string value)
        {
            this.constructedRequest.Headers[name] = value;
            return this;
        }


        public HttpRequest Build()
        {
            int queriesCount = Queries.Count;
            if (Queries != null && queriesCount > 0)
            {
                constructedRequest.Uri += "?";
                for (int i = 0; i < queriesCount; i++)
                {
                    if(i > 0)
                    {
                        constructedRequest.Uri += "&";
                    }

                    constructedRequest.Uri += $"{Queries[i].Key}={Queries[i].Value}";
                }
            }
            return constructedRequest;
        }
    }
}
