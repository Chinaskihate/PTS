using PTS.TG.Bot.Client;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(x => x.UseStartup<Startup>());

builder.Build().Run();
