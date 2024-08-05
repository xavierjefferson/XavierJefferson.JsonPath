using System.Text;
using Nito.AsyncEx.Synchronous;
using XavierJefferson.JsonPathParser.Helpers;
using XavierJefferson.JsonPathParser.Interfaces;

namespace XavierJefferson.JsonPathParser;

public class ParseContextImpl : IParseContext
{
    private readonly Configuration _configuration;

    public ParseContextImpl() : this(Configuration.DefaultConfiguration())
    {
    }

    public ParseContextImpl(Configuration configuration)
    {
        _configuration = configuration;
    }


    public IDocumentContext Parse(object? json)
    {
        if (json == null) throw new ArgumentNullException(nameof(json));
        if (json is JpDictionary || json is JpObjectList) return new JsonContext(json, _configuration);
        object? obj;
        if (json is string jsonString)
            obj = _configuration.JsonProvider.Parse(jsonString);
        else
            obj = _configuration.JsonProvider.Parse(_configuration.JsonProvider.ToJson(json));

        return new JsonContext(obj, _configuration);
    }


    public IDocumentContext Parse(string json)
    {
        Assertions.NotEmpty(json, "json string can not be null or empty");
        var obj = _configuration.JsonProvider.Parse(json);
        return new JsonContext(obj, _configuration);
    }


    public IDocumentContext ParseUtf8(byte[] json)
    {
        Assertions.NotEmpty(json, "json bytes can not be null or empty");
        var obj = _configuration.JsonProvider.Parse(json);
        return new JsonContext(obj, _configuration);
    }


    public IDocumentContext Parse(Stream stream)
    {
        return Parse(stream, Encoding.UTF8);
    }


    public IDocumentContext Parse(Stream stream, Encoding encoding)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        if (encoding == null) throw new ArgumentNullException(nameof(encoding));
        using (stream)
        {
            var obj = _configuration.JsonProvider.Parse(stream, encoding);
            return new JsonContext(obj, _configuration);
        }
    }


    public IDocumentContext Parse(FileInfo fileInfo)
    {
        if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));
        using (var fis = fileInfo.OpenRead())
        {
            return Parse(fis, Encoding.UTF8);
        }
    }


    [Obsolete]
    public IDocumentContext Parse(Uri url)
    {
        if (url == null) throw new ArgumentNullException(nameof(url));

        using (var fis = new HttpClient().GetStreamAsync(url).WaitAndUnwrapException())
        {
            return Parse(fis);
        }
    }
}