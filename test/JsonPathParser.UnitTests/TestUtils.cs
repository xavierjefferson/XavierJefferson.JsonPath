using XavierJefferson.JsonPathParser.Interfaces;
using XavierJefferson.JsonPathParser.Path;
using XavierJefferson.JsonPathParser.UnitTests.TestData;

namespace XavierJefferson.JsonPathParser.UnitTests;

public class TestUtils
{
    public static JpObjectList AsList(params object?[] items)
    {
        return new JpObjectList(items);
    }


    public static Stream GetResourceAsStream(string path)
    {
        var a = typeof(TestUtils).Assembly.GetManifestResourceNames()
            .FirstOrDefault(i => i.EndsWith(path, StringComparison.InvariantCultureIgnoreCase));
        return typeof(TestUtils).Assembly.GetManifestResourceStream(a);
    }

    public JpDictionary GetSingletonMap(string key, object? value)
    {
        return new JpDictionary { { key, value } };
    }

    public IPredicateContext CreatePredicateContext(object check, IProviderTypeTestCase testCase)
    {
        return new PredicateContextImpl(check, check, testCase.Configuration,
            new Dictionary<IPath, object?>());
    }
}