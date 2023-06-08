---
authors:
  - DGMJR-IO
title: DGMJR System.Extensions
lastmod: 2023-03-30T17:25:50.755Z
created: 2023-03-13-05:47:59
project: DGMJR System.Extensions
license: MIT
project: "System.Types"
keywords:
  - extensions
  - system
categories:
  - extensions
type: readme
slug: dgmjr-system-extensions
description: This is a collection of extensions for primary System.* namespaced classes in .NET.
---

# DGMJR System.Extensions

This is a collection of extensions for primary System.* namespaced classes in .NET.

## Namespaces

### System

* `HashCode` - Polyfill for .NET 2.0 and earlier
* `ICloneable<TSelf>` - A strongly-typed `ICloneable`
* `MathExtensions`
  Defines:
    * `const π = Math.PI`
    * `ToRadians(this double degrees)`
    * `ToDegrees(this double radians)`
    * `IsBetween(this [double, int, long, float, IComparable] value, [double, int, long, float, IComparable] min, [double, int, long, float, IComparable] max)`
    * `Randoms` - Read the docs
* `StringExtensions`
  Defines:
    * `Escape(string)` - Escapes some common special characters
    * `IsNullOrWhitespace()` - Extension method that does exactly the same thing as the `static` method on the `string`
      class
    * `IsNullOrEmpty()` - Extension method that does exactly the same thing as the `static` method on the `string` class
    * `byte[] FromBase64String(this string)` - Extension method that does like it says
    * `string ToBase64String(this byte[])` - Extension method that does like it says

### System.Collections.Generic

* `CaseInsensitiveKeyDictionary<TValue>`
* `DefaultableDictionary<TKey, TValue>` - A dictionary that won't throw an exception if you try to get a key that doesn'
  t exist; it'll just return the default value.
* `LambdaEqualityComparer<T>` - An equality comparer that takes a lambda expression as the comparer (I couldn't believe
  something like this wasn't pre-built into the stdlib!)
* `IEnumerableExtensions`
  Defines:
    * `WhereNotNull()`
    * `Join(string separator)`
* `ObservableCollection<T>` - A delegate-driven rather than event-driven observable collection
