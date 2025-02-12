using System;
using GameFramework;

namespace GameMain.Runtime
{
    public class NewtonsoftJsonHelper : Utility.Json.IJsonHelper
    {
        public string ToJson(object obj)
        {
            return JsonEx.ToJson(obj);
        }

        public T ToObject<T>(string json)
        {
            return JsonEx.ToObject<T>(json);
        }

        public object ToObject(Type objectType, string json)
        {
            return JsonEx.ToObject(json, objectType);
        }
    }
}