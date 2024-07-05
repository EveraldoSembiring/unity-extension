using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UnityExtension
{
    public static class TypeConverter
    {
        public static string ToJsonString<OriginT>(this OriginT source )
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            string retval = JsonConvert.SerializeObject(source, settings);
            return retval;
        }

        public static byte[] ToByte<OriginT>(this OriginT source)
        {
            string modelJson = source.ToJsonString();
            byte[] retval = modelJson.ToByte();
            return retval;
        }

        public static byte[] ToByte(this string source)
        {
            byte[] retval = System.Text.Encoding.UTF8.GetBytes(source);
            return retval;
        }

        public static string ToPlainString(this byte[] data)
        {
            string retval = System.Text.Encoding.UTF8.GetString(data);
            return retval;
        }

        public static OriginT ToObject<OriginT>(this string data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            OriginT retval = JsonConvert.DeserializeObject<OriginT>(data, settings);
            return retval;
        }

        public static OriginT ToObject<OriginT>(this byte[] data)
        {
            OriginT retval = data.ToPlainString().ToObject<OriginT>();
            return retval;
        }
    }
}
