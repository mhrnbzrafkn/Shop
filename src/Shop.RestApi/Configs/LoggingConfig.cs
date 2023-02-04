using Serilog;
using Serilog.Events;

namespace Shop.RestApi.Configs;

internal class LoggingConfig : Configuration
{
    private const string LevelConfigKey = "logging:level";
    private const string LogDbPathKey = "logging:filepath";

    public override void ConfigureLogging(
        WebHostBuilderContext context, 
        ILoggingBuilder logging)
    {
        logging.ClearProviders();
        if (context.HostingEnvironment.IsDevelopment())
            logging.SetMinimumLevel(LogLevel.Debug)
                .AddConsole()
                .AddDebug()
                .AddEventSourceLogger();
        else
            logging.SetMinimumLevel(
                context.Configuration
                .GetValue(LevelConfigKey, LogLevel.Warning)).AddSerilog();
    }

    public override void Initialized()
    {
        var level = AppSettings[LevelConfigKey];
        var filePath = AppSettings[LogDbPathKey];
        var minLevel = Enum.Parse<LogLevel>(level);
        var serilogLevel = ToSerilogLogLevel(minLevel);

        if (serilogLevel == null)
            throw new ArgumentOutOfRangeException("Invalid minimum logging level");

        var logsDbPath = Path.Combine(BaseDirectory, filePath);

        Log.Logger = new LoggerConfiguration()
            //.WriteTo
            //.SQLite(
            //logsDbPath, 
            //restrictedToMinimumLevel: serilogLevel.Value, 
            //rollOver: false)
            .CreateLogger();
    }

    private LogEventLevel? ToSerilogLogLevel(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Debug: return LogEventLevel.Debug;
            case LogLevel.Critical: return LogEventLevel.Fatal;
            case LogLevel.Error: return LogEventLevel.Error;
            case LogLevel.Information: return LogEventLevel.Information;
            case LogLevel.Trace: return LogEventLevel.Verbose;
            case LogLevel.Warning: return LogEventLevel.Warning;
        }

        return null;
    }
}