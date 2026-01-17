namespace FoodChallenge.Payment.Domain.Constants;

public static class AuthorizationPolicies
{
    public const string PaymentsApi = "PaymentsApi";
}

public static class AuthorizationScopes
{
    public const string PaymentsRead = "payments.read";
    public const string PaymentsWrite = "payments.write";
}

public static class Audiences
{
    public const string PaymentsApi = "payments-api";
}
