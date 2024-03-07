namespace UnityExtension
{
    public static class HttpRequestMethod
    {
        const string PostMethod = "POST";
        const string GetMethod = "GET";
        const string UpdateMethod = "UPDATE";
        const string DeleteMethod = "DELETE";

        public static HttpRequestBuilder Post()
        {
            return new HttpRequestBuilder(PostMethod);
        }

        public static HttpRequestBuilder Get()
        {
            return new HttpRequestBuilder(GetMethod);
        }

        public static HttpRequestBuilder Update()
        {
            return new HttpRequestBuilder(UpdateMethod);
        }

        public static HttpRequestBuilder Delete()
        {
            return new HttpRequestBuilder(DeleteMethod);
        }
    }
}
