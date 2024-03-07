using System.Threading.Tasks;
using UnityEngine.Networking;

namespace UnityExtension
{
    public static class UnityWebrequestAsyncTask
    {
        public static System.Runtime.CompilerServices.TaskAwaiter<UnityWebRequest.Result> GetAwaiter(this UnityWebRequestAsyncOperation reqOp)
        {
            var taskCompletionSource = new TaskCompletionSource<UnityWebRequest.Result>();
            reqOp.completed += (asyncOp) =>
            {
                taskCompletionSource.TrySetResult(reqOp.webRequest.result);
            };

            if (reqOp.isDone)
            {
                taskCompletionSource.TrySetResult(reqOp.webRequest.result);
            }

            return taskCompletionSource.Task.GetAwaiter();
        }
    }
}
