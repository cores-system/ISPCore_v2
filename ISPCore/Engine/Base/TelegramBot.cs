using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.Databases.json;
using ISPCore.Models.Security;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISPCore.Engine.Base
{
    public static class TelegramBot
    {
        #region Настройки бота
        /// <summary>
        /// 
        /// </summary>
        static Telega tlg = null;

        /// <summary>
        /// 
        /// </summary>
        static IMemoryCache memoryCache = Service.Get<IMemoryCache>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Is2FA"></param>
        static string GetKey(string IP, bool Is2FA) => $"TelegramBot:IsAuth-{IP}:{Is2FA}";

        /// <summary>
        /// 
        /// </summary>
        static TelegramBotClient Bot = null;
        #endregion

        #region CreateToOnCallback
        static TelegramBot() => CreateToOnCallback();

        public static void CreateToOnCallback()
        {
            try
            {
                // Настройка
                tlg = Service.Get<JsonDB>().TelegramBot;
                Bot = new TelegramBotClient(tlg.Token);
                Bot.SetWebhookAsync("").Wait();

                // Callback
                Bot.OnCallbackQuery += async (object sc, CallbackQueryEventArgs ev) =>
                {
                    try
                    {
                        var message = ev.CallbackQuery.Message;
                        if (ev.CallbackQuery.Data.Contains("AuthCmd"))
                        {
                            // IP адрес
                            string IP = new Regex("([^\n\r\t ]+)([\n\r\t ]+)?$").Match(message.Text).Groups[0].Value;

                            #region Действие
                            switch (ev.CallbackQuery.Data)
                            {
                                case "AuthCmd-Access":
                                    {
                                        memoryCache.Set(GetKey(IP, message.Text.Contains("2FA")), (byte)0, TimeSpan.FromMinutes(20));
                                        await Bot.SendTextMessageAsync(message.Chat.Id, $"Доступ для '{IP}' разрешен");
                                        break;
                                    }

                                case "AuthCmd-NotAccess":
                                    {
                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Рекомендуем сменить пароли");
                                        break;
                                    }

                                case "AuthCmd-BlockedIP":
                                    {
                                        // Записываем IP в кеш IPtables
                                        memoryCache.Set(KeyToMemoryCache.IPtables(IP), new IPtables("TelegramBot", DateTime.Now.AddHours(1)), TimeSpan.FromHours(1));
                                        await Bot.SendTextMessageAsync(message.Chat.Id, $"IP '{IP}' заблокирован\nРекомендуем сменить пароли");
                                        break;
                                    }
                            }
                            #endregion

                            // Отсылаем пустое, чтобы убрать "часики" на кнопке
                            await Bot.AnswerCallbackQueryAsync(ev.CallbackQuery.Id);
                        }
                    }
                    catch { }
                };

                // Запускаем прием обновлений
                new System.Threading.Thread(() => Bot.StartReceiving()).Start();
            }
            catch { }
        }
        #endregion

        #region IsAuth
        /// <summary>
        /// Проверка авторизации
        /// </summary>
        /// <param name="IP">IP адрес пользователя</param>
        /// <param name="Is2FA">Страница 2FA или ISPCore Panel</param>
        public static bool IsAuth(string IP, bool Is2FA = false)
        {
            if (Bot == null || !tlg.EnabledToAuth)
                return true;

            // Проверяем авторизацию
            if (memoryCache.TryGetValue(GetKey(IP, Is2FA), out _))
            {
                // Обновляем время и возвращаем ответ
                memoryCache.Set(GetKey(IP, Is2FA), (byte)0, TimeSpan.FromMinutes(20));
                return true;
            }

            #region Авторизация через Telegram
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new [] {
                    new InlineKeyboardCallbackButton("Да","AuthCmd-Access"),
                    new InlineKeyboardCallbackButton("Нет","AuthCmd-NotAccess"),
                },
                new [] {
                    new InlineKeyboardCallbackButton("Заблокировать на 1 час","AuthCmd-BlockedIP"),
                },
            });

            // GeoIP
            var GeoIP = GeoIP2.City(IP);

            // Отправляем сообщение
            string type = Is2FA ? "для 2FA" : "в ISPCore Panel";
            Bot.SendTextMessageAsync(tlg.ClietnId, $"<b>Разрешить авторизацию {type} ?</b>\n{GeoIP.Country} / {GeoIP.Region} - {GeoIP.City}\n{IP}", replyMarkup: keyboard, parseMode: ParseMode.Html);

            // Ответ
            return false;
            #endregion
        }
        #endregion

        #region SendMsg
        /// <summary>
        /// Отправить текст
        /// </summary>
        /// <param name="msg">Сообщение html</param>
        async public static void SendMsg(string msg)
        {
            if (Bot == null || !tlg.IsNotification)
                return;

            await Bot.SendTextMessageAsync(tlg.ClietnId, msg, ParseMode.Html);
        }
        #endregion

        #region AuthToHtml
        /// <summary>
        /// Страница авторизации
        /// </summary>
        /// <param name="IP">IP адрес пользователя</param>
        public static string AuthToHtml(string IP) => @"<!DOCTYPE html>
<html lang='ru-RU'>
<head>
    <title>ISPCore</title>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1, user-scalable=no'>
    <link rel='stylesheet' href='/statics/style.css'>
</head>
<body>
    <div class='error'>
        <div class='error-block'>

            <div class='code'>2FA</div>
                <div class='title'>" + $"Telergam Bot</div><pre>Подтвердите авторизацию в Telergam<br />{IP}</pre>" +

            @"
            <div class='copyright'>
                <div>
                    &copy; 2018 <strong>ISPCore</strong>. All rights reserved.
                </div>
                <div>
                    <a href='/'>Главная сайта</a> / <a href='http://core-system.org/' target='_blank'>Core System</a>
                </div>
            </div>

        </div>
    </div>
</body>
</html>
";
        #endregion
    }
}
