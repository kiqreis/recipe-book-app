using System.Collections;

namespace WebApi.Test.InlineData;

public class CultureInlineData : IEnumerable<object[]>
{
  public IEnumerator<object[]> GetEnumerator()
  {
    yield return ["en"];
    yield return ["es-ES"];
    yield return ["fr-FR"];
    yield return ["pt-BR"];
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
