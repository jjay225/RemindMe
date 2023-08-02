using RemindMe.Contracts.Classes;
using RemindMe.Contracts.Services;
using RemindMe.Reminder;
using SendGrid.Extensions.DependencyInjection;
using Serilog;

var sendGridApiKey = "";
var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
Console.WriteLine(env);
var host = new HostBuilder()
     .ConfigureAppConfiguration((context, config) =>
     {
         config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .AddEnvironmentVariables();
         if (env == "Development")
         {
             config.AddUserSecrets<Program>();
         }

         var builtConfig = config.Build();

         sendGridApiKey = builtConfig.GetSection("ReminderDetails").GetValue<string>("SendGridApiKey");
         var delay = builtConfig.GetSection("ReminderDetails").GetValue<int>("WorkerDelayInMinutes");
     })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.Configure<ReminderDetails>(configuration.GetSection("ReminderDetails"));

        services.AddHostedService<EmailWorker>();
        services.AddSingleton<IRemindMeSendGridService, RemindMeSendGridService>();
        var logger = new LoggerConfiguration()
        .WriteTo.Console()
             .MinimumLevel.Error()
             .WriteTo.Console().MinimumLevel.Debug()
             .WriteTo.File("RemindMe.Reminders-.log", rollingInterval: RollingInterval.Day)
             .CreateLogger();

        services.AddLogging(logging => logging.AddSerilog(logger, true));
        services.AddSendGrid((config) =>
        {
            config.ApiKey = sendGridApiKey;
        });
    })
    .Build();

host.Run();
