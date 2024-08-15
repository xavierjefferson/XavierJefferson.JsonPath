using System.Collections.Immutable;
using XavierJefferson.JsonPathParser.Interfaces;

namespace XavierJefferson.JsonPathParser;
/// <summary>
///     Configuration builder
/// </summary>
public class ConfigurationBuilder
{
    private ReadOnlySet<Option> _options = new ReadOnlySet<Option>(new HashSet<Option>());
    private List<EvaluationCallback> _evaluationCallbacks = new();
    public IMappingProvider? MappingProvider { get; private set; }

    public IJsonProvider? JsonProvider { get; private set; }

    public ConfigurationBuilder WithJsonProvider(IJsonProvider? provider)
    {
        JsonProvider = provider;
        return this;
    }

    public ConfigurationBuilder WithMappingProvider(IMappingProvider? provider)
    {
        MappingProvider = provider;
        return this;
    }

    public ConfigurationBuilder WithOptions(params Option[] flags)
    {
        this._options = new ReadOnlySet<Option>(flags);
        return this;
    }

    public ConfigurationBuilder WithOptions(IEnumerable<Option> options)
    {
        this._options = new ReadOnlySet<Option>(options);
        return this;
    }

    public ConfigurationBuilder WithEvaluationCallbacks(params EvaluationCallback[] callbacks)
    {
        _evaluationCallbacks = callbacks.ToList();
        return this;
    }

    public ConfigurationBuilder WithEvaluationCallbacks(ICollection<EvaluationCallback>? callbacks)
    {
        _evaluationCallbacks = callbacks == null
            ? new List<EvaluationCallback>()
            : new List<EvaluationCallback>(callbacks);
        return this;
    }

    public Configuration Build()
    {
        if (JsonProvider == null || MappingProvider == null)
        {
            var defaults = Configuration.GetEffectiveDefaults();
            if (JsonProvider == null) JsonProvider = defaults.JsonProvider;
            if (MappingProvider == null) MappingProvider = defaults.MappingProvider;
        }

        return new Configuration(JsonProvider, MappingProvider, _options, _evaluationCallbacks);
    }
}