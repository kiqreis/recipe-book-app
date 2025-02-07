namespace MyRecipeBook.Domain.ValueObjects;

public abstract class MyRecipeBookRuleConstants
{
  public const int MAXIMUM_INGREDIENTS_RECIPE_GENERATE = 6;
  public const int MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES = 10;
  public const string CHAT_MODEL_VERSION = "gpt-4o";
  public const int REFRESH_TOKEN_EXPIRATION_MINUTES = 180;
}
