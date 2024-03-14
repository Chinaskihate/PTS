namespace PTS.Contracts.Users;
public static class UserRoles
{
    public const string ThemeManager = "THEME_MANAGER";
    public const string TaskManager = "TASK_MANAGER";
    public const string TestManager = "TEST_MANAGER";
    public const string Admin = "ADMIN";
    public const string RootAdmin = "ROOT_ADMIN";
    public const string TelegramBot = "TELEGRAM_BOT";
    public const string AnyAdmin = Admin + "," + RootAdmin;
    public const string ThemeManagerRoles = ThemeManager + "," + Admin + "," + RootAdmin + "," + TelegramBot;
    public const string TaskManagerRoles = TaskManager + "," + Admin + "," + RootAdmin;
    public const string TestManagerRoles = TestManager + "," + Admin + "," + RootAdmin;

    public static string[] AllRoles => new[]
    {
        ThemeManager,
        TaskManager,
        TestManager,
        Admin,
        RootAdmin,
    };
}
