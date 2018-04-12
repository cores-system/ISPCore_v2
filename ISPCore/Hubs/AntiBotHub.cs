using ISPCore.Engine;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Hash;
using ISPCore.Models.Databases.json;
using Microsoft.AspNetCore.SignalR;
using System;
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
                await Clients.Client(Context.ConnectionId).InvokeAsync("OnError", "Что-то пошло не так, попробуйте обновить страницу");
                Context.Connection.Abort();
                return;
            }

            // Валидные куки
            string cookie = Engine.core.AntiBot.GetValidCookie(HourCacheToUser, IP);

            // Отдаем пользователю результат
            await Clients.Client(Context.ConnectionId).InvokeAsync("OnCookie", cookie, HourCacheToUser);
            Context.Connection.Abort();
        }
    }
}
