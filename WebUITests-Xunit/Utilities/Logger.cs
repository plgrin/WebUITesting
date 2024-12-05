using Serilog;

namespace WebUITests_Xunit.Utilities
{
    public static class Logger
    {
        public static ILogger Log { get; private set; }

        static Logger()
        {
            Log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("test_logs/logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
