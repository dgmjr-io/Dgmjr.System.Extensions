---

title: DGMJR Caching Extensions
lastmod: 2023-56-27T13:56:34.5772-05:00Z
date: 2023-56-27T13:56:34.5773-05:00Z
license: MIT
slug: Dgmjr.Extensions.Caching-readme
version:
authors:
- dgmjr
description: Contains extension methods and classes for working with caches
keywords:
- Dgmjr.Extensions.Caching
- dgmjr
- dgmjr-io
type: readme
------------

# DGMJR Caching Extensions

Tis package contains extension methods and classes for working with caches

## Contents

### [DistributedCacheExtensions](https://github.com/dgmjr-io/Dgmjr.System.Extensions/blob/main/Caching/DistributedCacheExtensions.cs)

Extends the functionality of [`IDistributedCache`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.distributed.idistributedcache?view=dotnet-plat-ext-8.0) to include the following methods:

- Task<TItem?> GetOrCreateAsync<TItem>(this IDistributedCache cache, object key, Func<Task<TItem>> factory, DistributedCacheEntryOptions? createOptions)
- Task<TItem?> GetOrCreateAsync<TItem>(this IDistributedCache cache, object key, Func<Task<TItem>> factory)
- TItem? GetOrCreate<TItem>(this IDistributedCache cache, object key, Func<DistributedCacheEntryOptions, TItem> factory)
- TItem? GetOrCreate<TItem>(this IDistributedCache cache, object key, Func<TItem> factory, DistributedCacheEntryOptions? createOptions)
- TItem? GetOrCreate<TItem>(this IDistributedCache cache, object key, Func<TItem> factory)

