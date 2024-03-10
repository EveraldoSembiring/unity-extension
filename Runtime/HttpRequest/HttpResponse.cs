using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityExtension
{
    public class HttpResponse
    {
        public string Uri;
        public long RetCode;
        public IDictionary<string, string> Headers;
        public byte[] Body;

        public static HttpResponse CreateResponse(UnityWebRequest webrequest)
        {
            HttpResponse retval = new HttpResponse()
            {
                Uri = webrequest.url,
                RetCode = webrequest.responseCode,
                Headers = webrequest.GetResponseHeaders(),
                Body = webrequest.downloadHandler.data
            };

            return retval;
        }

        public void LogHttpResponseContent()
        {
            string content = $"Http Response.\n{Uri}\nReturn Code: {RetCode}";
            if (Headers != null)
            {
                foreach (var headerPair in Headers)
                {
                    content += $"\n{headerPair.Key} : {headerPair.Value}";
                }
            }
            GameLogger.Verbose(content);
        }

        internal T Parse<T>() where T : class
        {
            T retval = null;
            try
            {
                if (Body != null && Body.Length > 0)
                {
                    string bodyJson = System.Text.Encoding.UTF8.GetString(Body);
                    retval = JsonUtility.FromJson<T>(bodyJson);
                }
            }
            catch (Exception)
            {

            }
            return retval;
        }

        internal string ParseJson()
        {
            string bodyJson = null;
            try
            {
                if (Body != null && Body.Length > 0)
                {
                    bodyJson = System.Text.Encoding.UTF8.GetString(Body);
                }
            }
            catch (Exception)
            {

            }
            return bodyJson;
        }
    }
}
