using ISPCore.Engine.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ISPCore.Models.Triggers
{
    public class TriggerConf
    {
        /// <summary>
        /// Уникальный Id
        /// </summary>
        public int Id { get; set; } = int.Parse(Generate.Passwd(6, IsNumberCode: true));

        /// <summary>
        /// Отображаемое имя/описание триггера
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Включен/Отключен
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Подписки
        /// </summary>
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        /// <summary>
        /// Условия триггера
        /// </summary>
        public Dictionary<string, Trigger> Trigger { get; set; } = new Dictionary<string, Trigger>();

        /// <summary>
        /// Файл с конфигурацией
        /// </summary>
        [JsonIgnore]
        public string TriggerFile { get; set; }

        /// <summary>
        /// Время модификации файла
        /// </summary>
        [JsonIgnore]
        public DateTime LastUpdateFile { get; set; } = DateTime.Now;

        /// <summary>
        /// Время последнего запуска без ошибок
        /// </summary>
        [JsonIgnore]
        public DateTime LastRunToSuccess { get; set; }
    }
}
