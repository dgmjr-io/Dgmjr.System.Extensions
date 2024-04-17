namespace System;

public static class StaticMethodExtensions
{
    public static MethodInfo GetStaticGenericMethod(
        this type ownerType,
        type t,
        [CallerMemberName] string? methodName = default,
        params type[] parameterTypes
    )
    {
        var methods = ownerType
            .GetRuntimeMethods()
            .Where(m => m.IsGenericMethodDefinition && m.Name == methodName);
        return methods
                .Select(m =>
                {
                    try
                    {
                        m = m.MakeGenericMethod(t);
                        var @params = m.GetParameters();
                        if (m.Name != methodName)
                        {
                            return default;
                        }

                        if (@params.Length != parameterTypes?.Length)
                        {
                            return default;
                        }

                        if (parameterTypes != null)
                        {
                            for (var i = 0; i < parameterTypes.Length; i++)
                            {
                                if (!@params[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                                {
                                    return default;
                                }
                            }
                        }

                        return m;
                    }
                    catch
                    {
                        return default;
                    }
                })
                .FirstOrDefault(m => m != null)
            ?? throw new EntryPointNotFoundException(
                $"Method {methodName} not found on {ownerType.Name}"
            );
        ;
    }

    public static MethodInfo GetGenericMethod(
        this type ownerType,
        type t,
        [CallerMemberName] string? methodName = default,
        params type[] parameterTypes
    )
    {
        var methods = ownerType
            .GetRuntimeMethods()
            .Where(m => m.IsGenericMethodDefinition && m.Name == methodName);
        return methods
                .Select(m =>
                {
                    try
                    {
                        m = m.MakeGenericMethod(t);
                        var @params = m.GetParameters();
                        if (m.Name != methodName)
                        {
                            return default;
                        }

                        if (@params.Length != parameterTypes?.Length)
                        {
                            return default;
                        }

                        if (parameterTypes != null)
                        {
                            for (var i = 0; i < parameterTypes.Length; i++)
                            {
                                if (!@params[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                                {
                                    return default;
                                }
                            }
                        }

                        return m;
                    }
                    catch
                    {
                        return default;
                    }
                })
                .FirstOrDefault(m => m != null)
            ?? throw new EntryPointNotFoundException(
                $"Method {methodName} not found on {ownerType.Name}"
            );
        ;
    }

    public static MethodInfo GetStaticGenericMethod<Target, T>(
        [CallerMemberName] string? methodName = default,
        params type[] parameterTypes
    )
    {
        return typeof(Target).GetStaticGenericMethod(typeof(T), methodName, parameterTypes);
    }

    public static MethodInfo GetStaticMethod(
        this type ownerType,
        [CallerMemberName] string? methodName = default,
        params type[] parameterTypes
    )
    {
        var methods = ownerType.GetRuntimeMethods().Where(m => m.Name == methodName);
        return methods
                .Select(m =>
                {
                    try
                    {
                        var @params = m.GetParameters();
                        if (m.Name != methodName)
                        {
                            return default;
                        }

                        if (@params.Length != parameterTypes?.Length)
                        {
                            return default;
                        }

                        if (parameterTypes is not null)
                        {
                            for (var i = 0; i < parameterTypes.Length; i++)
                            {
                                if (!@params[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                                {
                                    return default;
                                }
                            }
                        }

                        return m;
                    }
                    catch
                    {
                        return default;
                    }
                })
                .FirstOrDefault(m => m != null)
            ?? throw new EntryPointNotFoundException(
                $"Method {methodName} not found on {ownerType.Name}"
            );
    }

    public static MethodInfo GetStaticMethod<Target>(
        [CallerMemberName] string? methodName = default,
        params type[] parameterTypes
    )
    {
        return typeof(Target).GetStaticMethod(methodName, parameterTypes)
            ?? throw new EntryPointNotFoundException(
                $"Method {methodName} not found on {typeof(Target).Name}"
            );
    }

    public static TDelegate CreateDelegate<TDelegate>(this MethodInfo mi)
        where TDelegate : Delegate
    {
        return (TDelegate)mi.CreateDelegate(typeof(TDelegate))
            ?? throw new EntryPointNotFoundException(
                $"Method {mi.Name} is not compatible with delegate type {typeof(TDelegate).Name}"
            );
    }

    public static TDelegate CreateDelegate<TDelegate>(this MethodInfo mi, object target)
        where TDelegate : Delegate
    {
        return (TDelegate)mi.CreateDelegate(typeof(TDelegate), target)
            ?? throw new EntryPointNotFoundException(
                $"Method {mi.Name} is not compatible with delegate type {typeof(TDelegate).Name}"
            );
    }

    public static TResult InvokeGenericStaticMethod<TResult>(
        this type ownerType,
        type t,
        [CallerMemberName] string? methodName = default,
        params object[] parameters
    )
    {
        var method = ownerType.GetStaticGenericMethod(
            t,
            methodName,
            parameters.Select(p => p.GetType()).ToArray()
        );
        return method == null
            ? throw new EntryPointNotFoundException(
                $"Method {methodName} not found on {ownerType.Name}"
            )
            : (TResult)method.Invoke(null, parameters);
    }

    public static TResult InvokeGenericMethod<TOwner, TResult>(
        this TOwner owner,
        type t,
        [CallerMemberName] string? methodName = default,
        params object[] parameters
    )
    {
        var method =
            typeof(TOwner).GetGenericMethod(
                t,
                methodName,
                parameters.Select(p => p.GetType()).ToArray()
            )
            ?? throw new EntryPointNotFoundException(
                $"Method {methodName} not found on {typeof(TOwner).Name}"
            );
        return (TResult)method.Invoke(null, parameters);
    }

    public static object InvokeGenericStaticMethod(
        this type ownerType,
        type t,
        [CallerMemberName] string? methodName = default,
        params object[] parameters
    )
    {
        var method = ownerType.GetStaticGenericMethod(
            t,
            methodName,
            parameters.Select(p => p.GetType()).ToArray()
        );
        return method == null
            ? throw new EntryPointNotFoundException(
                $"Method {methodName} not found on {ownerType.Name}"
            )
            : method.Invoke(null, parameters);
    }

    public static object InvokeGenericMethod<TOwner>(
        this TOwner owner,
        type t,
        [CallerMemberName] string? methodName = default,
        params object[] parameters
    )
    {
        var method = typeof(TOwner).GetGenericMethod(
            t,
            methodName,
            parameters.Select(p => p.GetType()).ToArray()
        );
        return method == null
            ? throw new EntryPointNotFoundException(
                $"Method {methodName} not found on {typeof(TOwner).Name}"
            )
            : method.Invoke(null, parameters);
    }
}
