namespace UnityExtension
{
    public class HttpResult<T>
    {
        public long RetCode;
        public T Result;
        public bool IsSuccess;

        public HttpResult(long retCode, T result)
        {
            RetCode = retCode;
            Result = result;
            IsSuccess = RetCode >= 200 && RetCode < 300;
        }
    }
}
