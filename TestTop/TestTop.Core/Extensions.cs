
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Core
{
    //public class JsonNetSerializer : ISerializer
    //{

    //    public static readonly ISerializer Default = new JsonNetSerializer();
    //    private Nancy.Responses.DefaultJsonSerializer _defaultSerializer = new Nancy.Responses.DefaultJsonSerializer();

    //    public IEnumerable<string> Extensions
    //    {
    //        get
    //        {
    //            yield break;
    //        }
    //    }

    //    public bool CanSerialize(string contentType)
    //    {
    //        return _defaultSerializer.CanSerialize(contentType);
    //    }

    //    public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
    //    {
    //        var serializer = JsonSerializer.Create(new JsonSerializerSettings());
    //        using (var writer = new JsonTextWriter(new StreamWriter(new UnclosableStreamWrapper(outputStream))))
    //        {
    //            serializer.Serialize(writer, model);

    //            writer.Flush();
    //        }
    //    }
    //}

    //public static class Ext
    //{
    //    public static T AsJson<T>(this Request request)
    //    {
    //        using (var r = new StreamReader(request.Body))
    //            return JsonConvert.DeserializeObject<T>(r.ReadToEnd());
    //    }

    //    public static JsonResponse<T> ToJson<T>(this T obj)
    //    {
    //        var resp = new JsonResponse<T>(obj, JsonNetSerializer.Default);
    //        resp.ContentType = "application/json";
    //        return resp;
    //    }
    //}
}
