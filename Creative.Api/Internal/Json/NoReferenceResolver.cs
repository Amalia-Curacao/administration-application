using System.Text.Json.Serialization;

namespace Creative.Api.Internal.Json;

internal sealed class NoReferenceResolver : ReferenceResolver
{
    private IDictionary<string,object> _dic = new Dictionary<string, object>(); 
    public override void AddReference(string referenceId, object value) { }

    public override string GetReference(object value, out bool alreadyExists)
    {
        var keyValuePair = _dic.FirstOrDefault(obj => ReferenceEquals(obj.Value, value));
        if (!keyValuePair.Equals(default(KeyValuePair<string, object>)))
        {
            alreadyExists = true;
            return keyValuePair.Key;
        }
        else
        {
            alreadyExists = false;
            var referenceId = Guid.NewGuid().ToString();
            AddReference(referenceId, value);
            return referenceId;
        }
    }

    public override object ResolveReference(string referenceId) => _dic[referenceId];
}
