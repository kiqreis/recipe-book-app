using Bogus;

namespace CommonTestsUtilities.Requests;

public class StringGeneratorRequest
{
  public static string Paragraph(int minCharacters)
  {
    var faker = new Faker();
    var txt = faker.Lorem.Paragraphs(count: 7);

    while (txt.Length < minCharacters)
    {
      txt = $"{txt} {faker.Lorem.Paragraph()}";
    }

    return txt;
  }
}
