namespace Dgmjr.System.Extensions.Tests.System.Text.Json;

using global::System.Collections;
using global::System.Text.Json;
using Xunit;
using CasingTuple = (string PascalCase, string CamelCase, string KebabCase, string SnakeCase);

public class JsonNamingPolicyTests
{
    [Theory]
    [MemberData(nameof(TestCases))]
    public void ToKebabCase_ShouldWork(
        string PascalCase,
        string CamelCase,
        string KebabCase,
        string SnakeCase
    )
    {
        PascalCase.ToKebabCase().Should().Be(KebabCase);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void FromKebabCase_ShouldWork(
        string PascalCase,
        string CamelCase,
        string KebabCase,
        string SnakeCase
    )
    {
        KebabCase.KebabCaseToPascalCase().Should().Be(PascalCase);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ToSnakeCase_ShouldWork(
        string PascalCase,
        string CamelCase,
        string KebabCase,
        string SnakeCase
    )
    {
        PascalCase.ToSnakeCase().Should().Be(SnakeCase);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void FromSnakeCase_ShouldWork(
        string PascalCase,
        string CamelCase,
        string KebabCase,
        string SnakeCase
    )
    {
        SnakeCase.SnakeCaseToPascalCase().Should().Be(PascalCase);
    }

    public static IEnumerable<object[]> TestCases =>
        new[]
        {
            new[] { "HelloWorld", "helloWorld", "hello-world", "hello_world" },
            new[]
            {
                "TwoRoadsDivergedInAYellowWood",
                "twoRoadsDivergedInAYellowWood",
                "two-roads-diverged-in-a-yellow-wood",
                "two_roads_diverged_in_a_yellow_wood"
            }
        };
}
