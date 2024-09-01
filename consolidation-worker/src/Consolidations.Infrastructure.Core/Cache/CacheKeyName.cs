namespace Consolidations.Infrastructure.Core.Cache;

public static class CacheKeyName
{
    public static string GetDailyBalanceKey(DateTime date) => $"consolidation:balance:daily:{date:yyyy-MM-dd}";
}