namespace Microsoft.Extensions.Caching.Distributed;

using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

using CacheHitCallback = System.Action<object, object>;
using CacheMissCallback = System.Action<object>;

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
    /// <param name="cacheHitCallback">The callback to be invoked if the key is found in the cache.</param>
    /// <param name="cacheMissCallback">The callback to be invoked if the key is not found in the cache.</param>
    /// <returns>The value associated with this key.</returns>
    public static TItem? GetOrCreate<TItem>(
        this IDistributedCache cache,
        object key,
        Func<TItem> factory,
        CacheHitCallback? cacheHitCallback = default,
        CacheMissCallback? cacheMissCallback = default
    )
    {
        return cache.GetOrCreate(key, factory, null, cacheHitCallback, cacheMissCallback);
    }

    /// <summary>
    /// Gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="createOptions">The options to be applied to the <see cref="DistributedCacheEntryOptions"/> if the key does not exist in the cache.</param>
    /// <param name="cacheHitCallback">The callback to be invoked if the key is found in the cache.</param>
    /// <param name="cacheMissCallback">The callback to be invoked if the key is not found in the cache.</param>
    /// <returns>The value associated with this key.</returns>
    public static TItem? GetOrCreate<TItem>(
        this IDistributedCache cache,
        object key,
        Func<TItem> factory,
        DistributedCacheEntryOptions? createOptions,
        CacheHitCallback? cacheHitCallback = default,
        CacheMissCallback? cacheMissCallback = default
    )
    {
        var byteValue = cache.Get(Serialize(key, Jso));
        TItem? result;
        if (byteValue == null)
        {
            cacheMissCallback?.Invoke(key);
            result = factory();
            cache.SetString(Serialize(key, Jso), Serialize(result, Jso), createOptions);
        }
        else
        {
            cacheHitCallback?.Invoke(key, byteValue);
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
    /// <param name="cacheHitCallback">The callback to be invoked if the key is found in the cache.</param>
    /// <param name="cacheMissCallback">The callback to be invoked if the key is not found in the cache.</param>
    /// <returns>The value associated with this key.</returns>
    public static TItem? GetOrCreate<TItem>(
        this IDistributedCache cache,
        object key,
        Func<DistributedCacheEntryOptions, TItem> factory,
        CacheHitCallback? cacheHitCallback = default,
        CacheMissCallback? cacheMissCallback = default
    )
    {
        var byteValue = cache.Get(Serialize(key, Jso));
        TItem? result;
        if (byteValue == null)
        {
            cacheMissCallback?.Invoke(key);
            var options = new DistributedCacheEntryOptions();
            result = factory(options);
            cache.SetString(Serialize(key, Jso), Serialize(result, Jso), options);
        }
        else
        {
            cacheHitCallback?.Invoke(key, byteValue);
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
    /// <param name="cacheHitCallback">The callback to be invoked if the key is found in the cache.</param>
    /// <param name="cacheMissCallback">The callback to be invoked if the key is not found in the cache.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<TItem?> GetOrCreateAsync<TItem>(
        this IDistributedCache cache,
        object key,
        Func<Task<TItem>> factory,
        CacheHitCallback? cacheHitCallback = default,
        CacheMissCallback? cacheMissCallback = default
    )
    {
        return GetOrCreateAsync(cache, key, factory, null, cacheHitCallback, cacheMissCallback);
    }

    /// <summary>
    /// Asynchronously gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory task that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="createOptions">The options to be applied to the <see cref="DistributedCacheEntryOptions"/> if the key does not exist in the cache.</param>
    /// <param name="cacheHitCallback">The callback to be invoked if the key is found in the cache.</param>
    /// <param name="cacheMissCallback">The callback to be invoked if the key is not found in the cache.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<TItem?> GetOrCreateAsync<TItem>(
        this IDistributedCache cache,
        object key,
        Func<Task<TItem>> factory,
        DistributedCacheEntryOptions? createOptions,
        CacheHitCallback? cacheHitCallback = default,
        CacheMissCallback? cacheMissCallback = default
    )
    {
        var byteValue = cache.Get(Serialize(key, Jso));
        TItem? result;
        if (byteValue == null)
        {
            cacheHitCallback?.Invoke(key, byteValue);
            result = await factory().ConfigureAwait(false);
            await cache.SetStringAsync(Serialize(key, Jso), Serialize(result, Jso), createOptions);
        }
        else
        {
            cacheMissCallback?.Invoke(key);
            result = Deserialize<TItem>(byteValue.AsSpan(), Jso);
        }

        return result;
    }

    public static Task SetAsync(
        this IDistributedCache cache,
        object key,
        object value,
        DateTimeOffset? absoluteExpiration = default,
        TimeSpan? absoluteExpirationRelativeToNow = default,
        TimeSpan? slidingExpiration = default,
        CancellationToken cancellationToken = default
    )
    {
        return cache.SetAsync(
            Serialize(key, Jso),
            value,
            absoluteExpiration,
            absoluteExpirationRelativeToNow,
            slidingExpiration,
            cancellationToken
        );
    }

    public static async Task SetAsync(
        this IDistributedCache cache,
        string key,
        object value,
        DateTimeOffset? absoluteExpiration = default,
        TimeSpan? absoluteExpirationRelativeToNow = default,
        TimeSpan? slidingExpiration = default,
        CancellationToken cancellationToken = default
    )
    {
        await cache.SetStringAsync(
            key,
            Serialize(value, Jso),
            absoluteExpiration,
            absoluteExpirationRelativeToNow,
            slidingExpiration,
            cancellationToken
        );
    }

    public static Task SetStringAsync(
        this IDistributedCache cache,
        object key,
        string value,
        DateTimeOffset? absoluteExpiration = default,
        TimeSpan? absoluteExpirationRelativeToNow = default,
        TimeSpan? slidingExpiration = default,
        CancellationToken cancellationToken = default
    )
    {
        return cache.SetStringAsync(
            Serialize(key, Jso),
            value,
            absoluteExpiration,
            absoluteExpirationRelativeToNow,
            slidingExpiration,
            cancellationToken
        );
    }

    public static Task SetStringAsync(
        this IDistributedCache cache,
        string key,
        string value,
        DateTimeOffset? absoluteExpiration = default,
        TimeSpan? absoluteExpirationRelativeToNow = default,
        TimeSpan? slidingExpiration = default,
        CancellationToken cancellationToken = default
    )
    {
        return cache.SetStringAsync(
            key,
            value,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = absoluteExpiration,
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow,
                SlidingExpiration = slidingExpiration
            },
            cancellationToken
        );
    }

    public static async Task<TValue?> GetAsync<TValue>(
        this IDistributedCache cache,
        object key,
        CancellationToken cancellationToken = default
    )
    {
        return await cache.GetAsync<TValue>(Serialize(key, Jso), cancellationToken);
    }

    public static async Task<TValue?> GetAsync<TValue>(
        this IDistributedCache cache,
        string key,
        CancellationToken cancellationToken = default
    )
    {
        return Deserialize<TValue>(await cache.GetAsync(key, cancellationToken), Jso);
    }
}
