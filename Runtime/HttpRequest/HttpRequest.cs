using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

namespace UnityExtension
{
    public class HttpRequest
    {
        public string Method;
        public string Uri;

        public Dictionary<string, string> Headers;
        public byte[] BodyBytes;

        /// <summary>
        /// Send http request and parse response body into object then returning it as callback
        /// </summary>
        /// <param name="timeoutMs">Request timeout</param>
        /// <param name="callback">Return T object</param>
        public async void Send<T>(int timeoutMs, Action<HttpResult<T>> callback) where T : class
        {
            Task<HttpResponse> responseTask = Send(timeoutMs);
            await responseTask;
            HttpResponse response = responseTask.Result;

            HttpResult<T> result = null;
            if(response != null)
            {
                T parsedBody = response.Parse<T>();
                result = new HttpResult<T>(response.RetCode, parsedBody);
            }
            callback?.Invoke(result);
        }

        /// <summary>
        /// Send http request and return json as callback
        /// </summary>
        /// <param name="timeoutMs">Request timeout</param>
        /// <param name="callback">Return json body</param>
        public async void Send(int timeoutMs, Action<string> callback)
        {
            Task<HttpResponse> responseTask = Send(timeoutMs);
            await responseTask;
            HttpResponse response = responseTask.Result;

            string result = null;
            if (response != null)
            {
                result = response.ParseJson();
            }
            callback?.Invoke(result);
        }

        #region private functions
        private async Task<HttpResponse> Send(int timeOutMs)
        {
            HttpResponse response = null;
            LogHttpRequestContent();
            using (UnityWebRequest task = CreateUnityWebRequest())
            {
                task.timeout = timeOutMs / 1000;
                await task.SendWebRequest();
                response = HttpResponse.CreateResponse(task);
            }

            if(response != null)
            {
                response.LogHttpResponseContent();
            }

            return response;
        }

        private UnityWebRequest CreateUnityWebRequest()
        {
            var unityWebRequest = new UnityWebRequest(Uri, Method);

            foreach(var headerPair in Headers)
            {
                unityWebRequest.SetRequestHeader(headerPair.Key, headerPair.Value);
            }

            if(BodyBytes != null)
            {
                unityWebRequest.uploadHandler = new UploadHandlerRaw(BodyBytes);
                unityWebRequest.disposeUploadHandlerOnDispose = true;
            }

            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            unityWebRequest.disposeDownloadHandlerOnDispose = true;

            return unityWebRequest;
        }

        public void LogHttpRequestContent()
        {
            string content = $"Http Request.\n{Method} {Uri}";
            if (Headers != null)
            {
                foreach (var headerPair in Headers)
                {
                    content += $"\n{headerPair.Key} : {headerPair.Value}";
                }
            }
            GameLogger.Verbose(content);
        }
        #endregion
    }
}
