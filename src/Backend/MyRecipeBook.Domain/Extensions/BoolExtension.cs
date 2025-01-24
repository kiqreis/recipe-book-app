namespace MyRecipeBook.Domain.Extensions;

public static class BoolExtension
{
  public static bool IsFalse(this bool value) => !value;
}
