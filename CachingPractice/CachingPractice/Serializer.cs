namespace CachingPractice
{
    ///// <summary>
    ///// HybridCache serializer for any type using System.Text.Json
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class JsonHybridCacheSerializer<T> : IHybridCacheSerializer<T>
    //{
    //    private static readonly JsonSerializerOptions _options = new()
    //    {
    //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //        WriteIndented = false,
    //        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    //    };

    //    public T Deserialize(ReadOnlySequence<byte> source)
    //    {
    //        if (source.IsSingleSegment)
    //        {
    //            return JsonSerializer.Deserialize<T>(source.FirstSpan, _options)!;
    //        }

    //        return JsonSerializer.Deserialize<T>(source.ToArray(), _options)!;
    //    }
    //    public void Serialize(T value, IBufferWriter<byte> target)
    //    {
    //        using var writer = new Utf8JsonWriter(target);
    //        JsonSerializer.Serialize(writer, value, _options);
    //        writer.Flush();
    //    }
    //}


    ///// <summary>
    ///// Factory for HybridCache serializers
    ///// </summary>
    //public class JsonHybridCacheSerializerFactory : IHybridCacheSerializerFactory
    //{
    //    public bool TryCreateSerializer<T>(out IHybridCacheSerializer<T>? serializer)
    //    {
    //        try
    //        {
    //            serializer = new JsonHybridCacheSerializer<T>();
    //            return true;
    //        }
    //        catch
    //        {
    //            serializer = null;
    //            return false;
    //        }
    //    }
    //}
}


