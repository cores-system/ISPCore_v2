using ISPCore.Engine.Auth;
using ISPCore.Engine.Hash;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ISPCore.Hubs
{
    public class AntiBotHub : Hub
    {
        async public Task GetValidCookie(string IP, int HourCacheToUser, string hash)
        {
            // Делаем проверку IP
            if (hash != md5.text($"{IP}:{HourCacheToUser}:{PasswdTo.salt}"))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("OnError", "Что-то пошло не так, попробуйте обновить страницу");
                Context.Abort();
                return;
            }

            // Валидные куки
            string cookie = Engine.core.AntiBot.GetValidCookie(HourCacheToUser, IP);

            // Отдаем пользователю результат
            await Clients.Client(Context.ConnectionId).SendAsync("OnCookie", cookie, HourCacheToUser);
            Context.Abort();
        }
    }
}
