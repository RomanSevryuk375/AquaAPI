namespace Control.Application.Constants;

public static class CacheKeys
{
    public static string Rule(Guid userId, Guid ruleId) => $"rule:{userId}:{ruleId}";
    public static string Schedule(Guid userId, Guid scheduleId) => $"schedule:{userId}:{scheduleId}";
    public static string VacationMode(Guid userId, Guid vacationModeId) => $"vacation-mode:{userId}:{vacationModeId}";
    public static string Ecosystem(Guid userId, Guid ecosystemId) => $"ecosystem:{userId}:{ecosystemId}";
}
