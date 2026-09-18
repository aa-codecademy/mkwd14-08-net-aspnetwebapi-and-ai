using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace NotesApp.Helpers;

public static class LoggingConfigurationHelper
{
    private const string OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// Creates the logger. Program.cs assigns it to the global Log.Logger
    /// and registers it with the host.
    /// More on how to configure Serilog: https://github.com/serilog/serilog/wiki/Configuration-Basics
    /// </summary>
    public static ILogger CreateSerilogLogger()
    {
        return new LoggerConfiguration()
            // ===> 1. LEVELS
            // Events below Information are discarded. Lower to Debug to see
            // NoteService's LogDebug() output.
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)

            // ===> 2. ENRICHERS
            // FromLogContext adds properties pushed onto the ambient log context,
            // e.g. RequestId for every event logged during a request.
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()

            // ===> 3. SINKS - every event is written to each sink.
            .WriteTo.Console(outputTemplate: OutputTemplate, restrictedToMinimumLevel: LogEventLevel.Debug)
            .WriteTo.File(
                path: "Logs/notesapp-.txt",
                rollingInterval: RollingInterval.Day, // Daily rolling file; the date is inserted before the extension (Logs/notesapp-20260916.txt)
                retainedFileCountLimit: 7, //  Keeps the last 7 files
                outputTemplate: OutputTemplate
            )
            .WriteTo.File(
                path: "Logs/notesapp-.json",
                rollingInterval: RollingInterval.Day,
                formatter: new CompactJsonFormatter()
            )
            //.WriteTo.MSSqlServer(
            //    connectionString: connectionString,
            //    sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
            //    {
            //        TableName = "Logs",
            //        AutoCreateSqlTable = true
            //    },
            //    restrictedToMinimumLevel: LogEventLevel.Information)

            .CreateLogger();
    }

    /* Logging good practices
      
        1. Log at Appropriate Levels (ERROR, INFORMATION, WARNING...)
        2. Add relevant context to log entries (ids, operation names...) to make troubleshooting easier
        3. Avoid Logging Sensitive Information (user credentials, passwords, tokens etc..)
        4. Log Exceptions with Stack Trace
        5. Use Structured Logging
        6. Avoid Excessive Logging
        7. Log at Start and End of Critical Operations

    */
}
