using Sqids;

namespace CommonTestsUtilities.IdEncrypt;

public class IdEncryptBuilder
{
  public static SqidsEncoder<long> Build()
  {
    return new SqidsEncoder<long>(new()
    {
      MinLength = 3,
      Alphabet = "achIugtW19s7vA4ldomHjULNFYbery0EpTMxkBiQ6qJ2SKXZG35Cz8RDfnPOVw"
    });
  }
}
