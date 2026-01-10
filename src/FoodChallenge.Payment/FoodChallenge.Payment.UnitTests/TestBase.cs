using Bogus;

namespace FoodChallenge.Payment.UnitTests;

public class TestBase
{
    private const string Locale = "pt_BR";

    protected Faker GetFaker()
    {
        return new Faker(Locale);
    }
}
