using Microsoft.Extensions.Options;
using PTS.TG.Bot.Client.Configure;
using PTS.TG.Bot.Client.Services;
using Telegram.Bot;

namespace PTS.TG.Bot.Client;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<TelegramBotOptions>(_configuration.GetSection(nameof(TelegramBotOptions)));

        services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x =>
        {
            var telegramBotOptions = x.GetRequiredService<IOptions<TelegramBotOptions>>();

            return new TelegramBotClient(telegramBotOptions.Value.Token);
        });

        services.AddHostedService<BotHandler>();
    }

    public void Configure()
    {

    }
}