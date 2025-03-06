namespace UnityExtension
{
    [System.Serializable]
    public struct ExtendedVector2<T,V>
    {
        public T X;
        public V Y;

        public ExtendedVector2(T x, V y)
        {
            X = x;
            Y = y;
        }
    }
}