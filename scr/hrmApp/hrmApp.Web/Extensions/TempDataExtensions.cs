using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace hrmApp.Web.Extensions
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            tempData.TryGetValue(key, out object tempDataObject);
            return tempDataObject == null ?
                default(T) : JsonSerializer.Deserialize<T>((string)tempDataObject);
        }

        public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object tempDataObject = tempData.Peek(key);
            return tempDataObject == null ?
                default(T) : JsonSerializer.Deserialize<T>((string)tempDataObject);
        }

        public static void Keep<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            tempData.Keep(key);
        }
    }
}