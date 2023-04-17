using AuditTest.Debugging;

namespace AuditTest
{
    public class AuditTestConsts
    {
        public const string LocalizationSourceName = "AuditTest";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "53272a340762497994678e57d5210f71";
    }
}
