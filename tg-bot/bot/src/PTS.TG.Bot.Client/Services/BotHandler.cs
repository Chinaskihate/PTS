using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PTS.TG.Bot.Client.Services;

public class BotHandler : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<BotHandler> _logger;

    public BotHandler(
        ITelegramBotClient botClient,
        ILogger<BotHandler> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("START RECEIVING...");

        _botClient.StartReceiving(
            updateHandler: OnMessage,
            pollingErrorHandler: OnError,
            cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task OnMessage(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        try
        {
            // ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            // {
            //     new KeyboardButton[] { KeyboardButton.WithWebApp("text", new WebAppInfo
            //     {
            //         // Url = "https://pts-web.netlify.app/"
            //         Url = "https://127.0.0.1:3000/"
            //     }),  },
            // })
            // {
            //     ResizeKeyboard = true
            // };
            //
            // if (update.Message != null)
            // {
            //     await _botClient.SendTextMessageAsync(
            //         chatId: update.Message.Chat.Id,
            //         text: "Choose a response",
            //         replyMarkup: replyKeyboardMarkup,
            //         cancellationToken: cancellationToken);
            // }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error: {Message}", exception.Message);
        }
    }

    private async Task OnError(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Error: {Message}", exception.Message);
    }
}