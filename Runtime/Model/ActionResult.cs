using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtension
{
    public class ActionResult<T>
    {
        private Action<T> onSuccessDelegates;
        private Action<string> onFailedDelegates;
        private Action onCompleteDelegates;
        
        private T result;
        private string errorMessage;
        
        private bool isSuccess;
        private bool isFailed;

        public void Success(T actionResult)
        {
            this.result = actionResult;
            isSuccess = true;
            onSuccessDelegates?.Invoke(this.result);
            onCompleteDelegates?.Invoke();
        }

        public void Failed(string message)
        {
            errorMessage = message;
            isFailed = true;
            onFailedDelegates?.Invoke(errorMessage);
            onCompleteDelegates?.Invoke();
        }

        public void OnSuccess(Action<T> callback)
        {
            onSuccessDelegates += callback;
            if (isSuccess)
            {
                onSuccessDelegates?.Invoke(result);
                onCompleteDelegates?.Invoke();
            }
        }
        
        public void OnFailed(Action<string> callback)
        {
            onFailedDelegates += callback;
            if (isFailed)
            {
                onFailedDelegates?.Invoke(errorMessage);
                onCompleteDelegates?.Invoke();
            }
        }
        
        public void OnComplete(Action callback)
        {
            onCompleteDelegates += callback;
            if (isFailed)
            {
                onCompleteDelegates?.Invoke();
            }
        }
    }

    public class ActionResult : ActionResult<bool>
    {
        public void Success()
        {
            Success(actionResult: true);
        }
    }
}
