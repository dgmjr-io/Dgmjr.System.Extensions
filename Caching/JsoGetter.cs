namespace Microsoft.Extensions.Caching.Distributed;

using System.Collections;
using System.Reflection;

internal static class JsoGetter
{
    delegate object GetPrimaryAndSecondaryDelegate(out object secondary);
    const string DependentHandleName = "DependentHandle";
    const string GetPrimaryAndSecondaryName = "GetPrimaryAndSecondary";
    const string entriesName = "entries";
    const string ContainerName = "Container";
    const string AllName = "All";
    const string TrackedOptionsInstancesName = "TrackedOptionsInstances";
    const string TargetAndDependentName = "TargetAndDependent";

    static readonly ConditionalWeakTable<Jso, object> All =
        (typeof(Jso)
            .GetNestedType(
                TrackedOptionsInstancesName,
#pragma warning disable S3011
                BindingFlags.Static | BindingFlags.NonPublic
#pragma warning restore S3011
            )
            .GetRuntimeProperty(AllName)
            .GetValue(null) as ConditionalWeakTable<Jso, object>)!;

    static object ToContainer(this ConditionalWeakTable<Jso, object> all)
        => typeof(ConditionalWeakTable<Jso,object>).GetRuntimeFields().First(f => f.FieldType.Name == ContainerName).GetValue(all);

    static IEnumerable ToEntries(this object container)
        => (container.GetType().GetRuntimeFields().First(f => f.Name.Contains(entriesName)).GetValue(container) as IEnumerable)!;

    static object ToDependentHandle(this object entry)
        => entry.GetType().GetRuntimeFields().First(f => f.FieldType.Name == DependentHandleName).GetValue(entry);

    static ITuple? ToTargetAndDependent(this object dependentHandle)
    {
        try { return dependentHandle.GetType().GetRuntimeProperty(TargetAndDependentName).GetValue(dependentHandle) as ITuple; }
        catch { return null; }
    }

    static Jso? ToJso(this ITuple targetAndDependent)
        => targetAndDependent[0] as Jso ?? targetAndDependent[1] as Jso;

    static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> ts)
        => ts.Where(t => t is not null)!;

    static readonly Jso DefaultJso = new()
        {
            AllowTrailingCommas = true,
            MaxDepth = 10,
            DefaultIgnoreCondition = JIgnore.Never,
            ReferenceHandler = ReferenceHandler.Preserve,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            NumberHandling =
                JNumbers.AllowNamedFloatingPointLiterals | JNumbers.AllowReadingFromString,
            DictionaryKeyPolicy = JNaming.CamelCase,
            UnknownTypeHandling = JUnknownTypes.JsonElement,
            WriteIndented = false,
            IgnoreReadOnlyFields = false,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JComments.Skip
        };

    /// <summary>We're going to assume that the JSO that we want was the first one added to this collection, at startup</summary>
    public static Jso GetJso()
    {
        var jsos = All.ToContainer().ToEntries().OfType<object>().Select(ToDependentHandle).Select(ToTargetAndDependent).WhereNotNull().Select(ToJso).WhereNotNull();
        jsos = jsos.OrderByDescending(jso => jso.Converters.Count);
        return jsos.FirstOrDefault() ?? DefaultJso;
    }
}
