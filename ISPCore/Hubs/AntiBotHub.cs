﻿using ISPCore.Engine.Auth;
using ISPCore.Engine.Hash;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Trigger = ISPCore.Models.Triggers.Events.core.AntiBot;

namespace ISPCore.Hubs
{
    public class AntiBotHub : Hub
    {
        async public Task GetValidCookie(string IP, string host, int HourCacheToUser, string AntiBotHashKey, string hash)
        {
            // Делаем проверку IP
            if (hash != md5.text($"{IP}:{host}:{HourCacheToUser}:{AntiBotHashKey}:{PasswdTo.salt}"))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("OnError", "Что-то пошло не так, попробуйте обновить страницу");
                Context.Abort();
                return;
            }

            // Валидные куки
            string cookie = Engine.core.AntiBot.GetValidCookie(HourCacheToUser, IP, "SignalR", AntiBotHashKey);

            // Отдаем пользователю результат
            await Clients.Client(Context.ConnectionId).SendAsync("OnCookie", cookie, HourCacheToUser);
            Context.Abort();

            // Триггеры
            Trigger.OnSetValidCookie((IP, host, cookie, "SignalR", HourCacheToUser));
        }
    }
}
