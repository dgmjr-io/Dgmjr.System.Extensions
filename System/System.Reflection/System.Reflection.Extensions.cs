/*
 * System.Reflection.Extensions.cs
 *
 *   Created: 2023-05-18-01:27:54
 *   Modified: 2023-05-18-01:27:55
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Reflection;

using System.IO;

public static class Extensions
{
    /// <summary>
    /// Returns <inheritdoc cref="ReadAssemblyResourceAllText" path="/returns" />
    /// </summary>
    /// <param name="assembly">the assembly from which to load the resource</param>
    /// <param name="resourceName">the name of the assembly manufest resource</param>
    /// <returns>the text contents of the assemnly with manifest name <paramref name="resourceName" /></returns>
    public static string ReadAssemblyResourceAllText(this Assembly assembly, string resourceName) =>
        assembly.GetManifestResourceStream(resourceName).ReadToEnd();

    /// <summary>
    /// Returns <inheritdoc cref="ReadAssemblyResourceAllText" path="/returns" />, read asynchronously from the assembly
    /// </summary>
    /// <param name="assembly">the assembly from which to load the resource</param>
    /// <param name="resourceName">the name of the assembly manufest resource</param>
    /// <returns>the text contents of the assemnly with manifest name <paramref name="resourceName" /></returns>
    public static Task<string> ReadAssemblyResourceAllTextAsync(
        this Assembly assembly,
        string resourceName
    ) => assembly.GetManifestResourceStream(resourceName).ReadToEndAsync();


    public static Expression<Func<T, U>> ToExpression<T, U>(this PropertyInfo propertyInfo)
    {
        // Validate the input
        if (propertyInfo == null)
        {
            throw new ArgumentNullException(nameof(propertyInfo));
        }

        if (typeof(T) != propertyInfo.DeclaringType && !typeof(T).IsSubclassOf(propertyInfo.DeclaringType))
        {
            throw new ArgumentException($"The property '{propertyInfo.Name}' does not belong to type {typeof(T).FullName}.", nameof(propertyInfo));
        }

        if (typeof(U) != propertyInfo.PropertyType)
        {
            throw new ArgumentException($"The property '{propertyInfo.Name}' is not of type {typeof(U).FullName}.", nameof(propertyInfo));
        }

        // Create the parameter expression (t => ...)
        var parameterExpression = Expression.Parameter(typeof(T), "t");

        // Create the property access expression (t => t.Property)
        var propertyAccessExpression = Expression.Property(parameterExpression, propertyInfo);

        // Create the lambda expression (t => t.Property)
        var lambdaExpression = Expression.Lambda<Func<T, U>>(propertyAccessExpression, parameterExpression);

        return lambdaExpression;
    }
}
