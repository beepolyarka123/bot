using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
var botClient = new TelegramBotClient("5871230043:AAFzq99WSDOH_-XLA7mXZ1mx_erSh_tSYkM");
using CancellationTokenSource cts = new();
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};
botClient.StartReceiving(
updateHandler: HandleUpdateAsync,
pollingErrorHandler: HandlePollingErrorAsync,
receiverOptions: receiverOptions,
cancellationToken: cts.Token
);
var me = await botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
cts.Cancel();
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;
    var chatId = message.Chat.Id;
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    if (messageText == "Проверка")
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Проверка бота: работа корректна",
            cancellationToken: cancellationToken);
    }

    if (messageText == "Активация кода")
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Введите код:",
            cancellationToken: cancellationToken);
    }
    //Message sentMessage = await botClient.SendTextMessageAsync(
    //chatId: chatId,
    //text: "You said:\n" + messageText,
    //cancellationToken: cancellationToken);
}
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
    _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}