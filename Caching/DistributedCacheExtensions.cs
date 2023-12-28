namespace Microsoft.Extensions.Caching.Distributed;

using System.Collections;
using System.Reflection;

public static class DistributedCacheExtensions
{
    public static readonly Jso Jso = JsoGetter.GetJso();

    /// <summary>
    /// Gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <returns>The value associated with this key.</returns>
    public static TItem? GetOrCreate<TItem>(
        this IDistributedCache cache,
        object key,
        Func<TItem> factory
    )
    {
        return cache.GetOrCreate(key, factory, null);
    }

    /// <summary>
    /// Gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="createOptions">The options to be applied to the <see cref="DistributedCacheEntryOptions"/> if the key does not exist in the cache.</param>
    /// <returns>The value associated with this key.</returns>
    public static TItem? GetOrCreate<TItem>(
        this IDistributedCache cache,
        object key,
        Func<TItem> factory,
        DistributedCacheEntryOptions? createOptions
    )
    {
        var byteValue = cache.Get(Serialize(key, Jso));
        TItem? result;
        if (byteValue == null)
        {
            result = factory();
            cache.SetString(Serialize(key, Jso), Serialize(result, Jso), createOptions);
        }
        else
        {
            result = Deserialize<TItem>(byteValue.AsSpan(), Jso);
        }

        return result;
    }

    /// <summary>
    /// Gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="createOptions">The options to be applied to the <see cref="DistributedCacheEntryOptions"/> if the key does not exist in the cache.</param>
    /// <returns>The value associated with this key.</returns>
    public static TItem? GetOrCreate<TItem>(
        this IDistributedCache cache,
        object key,
        Func<DistributedCacheEntryOptions, TItem> factory
    )
    {
        var byteValue = cache.Get(Serialize(key, Jso));
        TItem? result;
        if (byteValue == null)
        {
            var options = new DistributedCacheEntryOptions();
            result = factory(options);
            cache.SetString(Serialize(key, Jso), Serialize(result, Jso), options);
        }
        else
        {
            result = Deserialize<TItem>(byteValue.AsSpan(), Jso);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory task that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<TItem?> GetOrCreateAsync<TItem>(
        this IDistributedCache cache,
        object key,
        Func<Task<TItem>> factory
    )
    {
        return GetOrCreateAsync(cache, key, factory, null);
    }

    /// <summary>
    /// Asynchronously gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory task that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="createOptions">The options to be applied to the <see cref="DistributedCacheEntryOptions"/> if the key does not exist in the cache.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<TItem?> GetOrCreateAsync<TItem>(
        this IDistributedCache cache,
        object key,
        Func<Task<TItem>> factory,
        DistributedCacheEntryOptions? createOptions
    )
    {
        var byteValue = cache.Get(Serialize(key, Jso));
        TItem? result;
        if (byteValue == null)
        {
            result = await factory().ConfigureAwait(false);
            await cache.SetStringAsync(Serialize(key, Jso), Serialize(result, Jso), createOptions);
        }
        else
        {
            result = Deserialize<TItem>(byteValue.AsSpan(), Jso);
        }

        return result;
    }
}
