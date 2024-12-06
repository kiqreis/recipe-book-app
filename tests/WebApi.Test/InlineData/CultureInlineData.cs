using System.Collections;

namespace WebApi.Test.InlineData;

public class CultureInlineData : IEnumerable<object[]>
{
  public IEnumerator<object[]> GetEnumerator()
  {
    yield return new object[] { "es" };
    yield return new object[] { "fr" };
    yield return new object[] { "pt" };
    yield return new object[] { "en" };
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
