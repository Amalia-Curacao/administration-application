using System.Text.Json.Serialization;

namespace Creative.Api.Internal.Json;

internal sealed class NoReferenceHandler : ReferenceHandler
{
    public override ReferenceResolver CreateResolver() => new NoReferenceResolver();


}
