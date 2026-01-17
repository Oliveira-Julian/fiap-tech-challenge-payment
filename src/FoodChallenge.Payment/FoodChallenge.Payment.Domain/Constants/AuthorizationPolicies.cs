namespace FoodChallenge.Payment.Domain.Constants;

public static class AuthorizationPolicies
{
    public const string PaymentsApi = "PaymentsApi";
}

public static class AuthorizationScopes
{
    public const string PaymentsRead = "Payments.read";
    public const string PaymentsWrite = "Payments.write";
}

public static class Audiences
{
    public const string PaymentsApi = "Payments-api";
}
