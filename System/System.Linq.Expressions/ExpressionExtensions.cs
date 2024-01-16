namespace System.Linq.Expressions;
using System.Reflection;

public static class ExpressionExtensions
{
    /// <summary>
    /// Returns the <see cref="PropertyInfo" /> that describes the property that
    /// is being returned in an expression in the form:
    /// <code>
    ///   x => x.SomeProperty
    /// </code>
    /// </summary>
    public static PropertyInfo AsProperty(this LambdaExpression propertyAccessor)
    {
        var property = TryGetMemberExpression<PropertyInfo>(propertyAccessor, out var propertyInfo) ? propertyInfo : null;
        if (property == null)
        {
            throw new ArgumentException("Expected a lambda expression in the form: x => x.SomeProperty", nameof(propertyAccessor));
        }

        return property;
    }

    /// <summary>
    /// Returns the <see cref="MethodInfo" /> that describes the method that
    /// is being called in an expression in the form:
    /// <code>
    ///   x => x.Method()
    /// </code>
    /// </summary>
    public static MethodInfo AsMethod(this LambdaExpression methodCaller)
    {
        var method = TryGetMemberExpression<MethodInfo>(methodCaller, out var methodInfo) ? methodInfo : null;
        if (method == null)
        {
            throw new ArgumentException("Expected a lambda expression in the form: x => x.Method()", nameof(methodCaller));
        }

        return method;
    }

    /// <summary>
    /// Returns the <see cref="PropertyInfo" /> that describes the property that
    /// is being returned in an expression in the form:
    /// <code>
    ///   x => x.SomeProperty
    /// </code>
    /// </summary>
    public static FieldInfo AsField(this LambdaExpression fieldAccessor)
    {
        var field = TryGetMemberExpression<FieldInfo>(fieldAccessor, out var fieldInfo) ? fieldInfo : null;
        if (field == null)
        {
            throw new ArgumentException("Expected a lambda expression in the form: x => x.SomeField", nameof(fieldAccessor));
        }

        return field;
    }

    private static bool TryGetMemberExpression<TMemberInfo>(LambdaExpression lambdaExpression, out TMemberInfo? memberInfo)
        where TMemberInfo : MemberInfo
    {
        if (lambdaExpression.Parameters.Count != 1)
        {
            memberInfo = null!;
            return false;
        }

        var body = lambdaExpression.Body;

        if (body is UnaryExpression castExpression)
        {
            if (castExpression.NodeType != ExpressionType.Convert)
            {
                memberInfo = null!;
                return false;
            }

            body = castExpression.Operand;
        }

        if (body is MemberExpression memberExpression)
        {
            if (memberExpression.Expression != lambdaExpression.Parameters[0])
            {
                memberInfo = null!;
                return false;
            }

            memberInfo = memberExpression.Member as TMemberInfo;
            return true;
        }
        memberInfo = null!;
        return false;
    }
}
