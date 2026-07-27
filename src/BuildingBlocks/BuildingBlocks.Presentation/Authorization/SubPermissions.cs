// Ignore Spelling: Realtime

namespace BuildingBlocks.Presentation.Authorization;

public static class SubPermissions
{
    public const string TankRead = "tank:read";
    public const string TankCreate = "tank:create";
    public const string TankUpdate = "tank:update";
    public const string TankDelete = "tank:delete";

    public const string TankLimit1 = "tank:limit:1";
    public const string TankLimit10 = "tank:limit:10";
    public const string TankLimitUnlimited = "tank:limit:unlim";

    public const string DeviceControl = "device:control";
    public const string DeviceEditManual = "device:manual";

    public const string AutoRuleCreate = "auto:rule:create";
    public const string AutoRuleLimit5 = "auto:rule:limit:5";
    public const string AutoRuleLimit10 = "auto:rule:limit:10";
    public const string AutoRuleUnlimited = "auto:rule:limit:unlim";

    public const string AutoScheduleCreate = "auto:schedule:create";
    public const string VacationMode = "auto:vacation";


    public const string TelemetryView = "data:view";
    public const string AnalyticsHistory = "data:history";
    public const string DiagnosticsFull = "data:diag";
    public const string DataRealtime = "data:rt";


    public const string MaintenanceLogRead = "notify:log:read";
    public const string MaintenanceLogWrite = "notify:log:write";
    public const string ReminderManage = "notify:reminder";

    public const string EmailAlerts = "notify:email";
    public const string TelegramAlerts = "notify:tg";

    public const string AccountView = "account:view";
    public const string AccountUpdate = "account:update";
}
