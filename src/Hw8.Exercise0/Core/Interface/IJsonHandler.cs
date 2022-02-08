namespace Hw8.Exercise0.Core.Interface;

public interface IJsonHandler
{
    public bool Serialize(Stream content, string fileName);
    public IEnumerable<object> Deserialize(string fileName);
}
