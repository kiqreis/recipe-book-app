using System.Collections;

namespace WebApi.Test.InlineData;

public class CultureInlineData : IEnumerable<object[]>
{
  public IEnumerator<object[]> GetEnumerator()
  {
    yield return ["es-ES"];
    yield return ["fr-FR"];
    yield return ["pt-BR"];
    yield return ["en"];
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
