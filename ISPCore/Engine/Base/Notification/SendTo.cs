using ISPCore.Models.Databases.json;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Http;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ISPCore.Engine.Base.Notification
{
    public class SendTo
    {
        static TelegramBotClient Bot = null;
        JsonDB jsonDB = Service.Get<JsonDB>();

        #region Telegram
        /// <summary>
        /// Отправить текст в Telegram
        /// </summary>
        /// <param name="chatId">Id клиента</param>
        /// <param name="msg">Сообщение</param>
        public bool Telegram(int chatId, string msg)
        {
            try
            {
                if (Bot == null)
                {
                    Bot = new TelegramBotClient(jsonDB.ServiceBot.Telegram.Token);
                    Bot.SetWebhookAsync("").Wait();
                }

                Bot.SendTextMessageAsync(chatId, msg, ParseMode.Html).Wait();
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region Email
        /// <summary>
        /// Отправить Email
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="subject">Титл</param>
        /// <param name="message">Сообщение</param>
        public bool Email(string email, string subject, string message)
        {
            try
            {
                var conf = jsonDB.ServiceBot.Email;
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("ISPCore Bot", conf.Login));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $"<p>{message}</p>"
                };

                using (var client = new SmtpClient())
                {
                    client.ConnectAsync(conf.ConnectUrl, conf.ConnectPort, conf.useSsl).Wait();
                    client.AuthenticateAsync(conf.Login, conf.Passwd).Wait();
                    client.SendAsync(emailMessage).Wait();
                    client.DisconnectAsync(true).Wait();
                }
                
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region Sms
        /// <summary>
        /// Отправить смс
        /// </summary>
        /// <param name="phone">Телефон</param>
        /// <param name="text">Текст сообщения</param>
        public bool Sms(string phone, string text)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    bool success = false;
                    string apikey = jsonDB.ServiceBot.SMS.apikey;
                    string msg = client.GetStringAsync($"http://smspilot.ru/api.php?send={HttpUtility.UrlEncode(text)}&to={phone}&apikey={apikey}").Result;

                    if (msg != null && msg.ToUpper().Contains("SUCCESS"))
                        success = true;
                    
                    return success;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
