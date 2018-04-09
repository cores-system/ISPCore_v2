using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Models.RequestsFilter.Access;
using ISPCore.Engine.Base;
using System.IO;
using Newtonsoft.Json;

namespace ISPCore.Engine.RequestsFilter.Access
{
    public static class AccessIP
    {
        #region private
        private static Dictionary<string, List<AccessIPModel>> db = new Dictionary<string, List<AccessIPModel>>();

        static AccessIP()
        {
            if (File.Exists($"{Folders.Tmp}/AccessIP.json"))
            {
                db = JsonConvert.DeserializeObject<Dictionary<string, List<AccessIPModel>>>(File.ReadAllText($"{Folders.Tmp}/AccessIP.json"));
            }
        }

        private static void Save()
        {
            File.WriteAllText($"{Folders.Tmp}/AccessIP.json", JsonConvert.SerializeObject(db));
        }
        #endregion

        /// <summary>
        /// Добавить информацию и новом доступе
        /// </summary>
        /// <param name="IP">IP адрес пользователя</param>
        /// <param name="host">Домен для которого предоставлен доступ</param>
        /// <param name="expires">До какого времени предоставлен доступ</param>
        /// <param name="accessType">Режим доступа</param>
        public static void Add(string IP, string host, DateTime expires, AccessType accessType)
        {
            // Удаляем старые данные
            Remove(IP, host, accessType);

            // Модель
            var model = new AccessIPModel()
            {
                IP = IP,
                host = host,
                accessType = accessType,
                Expires = expires
            };

            // Обновляем даннные в базе
            if (db.TryGetValue($"{IP}-{host}-{accessType.ToString()}", out var mass)) {
                mass.Add(model);
            }
            else
            {
                db.Add($"{IP}-{host}-{accessType.ToString()}", new List<AccessIPModel>() { model });
            }

            // Сохраняем базу
            Save();
        }


        /// <summary>
        /// Удалить ключ из базы
        /// </summary>
        /// <param name="IP">IP адрес пользователя</param>
        /// <param name="host">Домен для которого предоставлен доступ</param>
        /// <param name="accessType">Режим предоставленного доступа</param>
        public static void Remove(string IP, string host, AccessType accessType)
        {
            string key = $"{IP}-{host}-{accessType.ToString()}";
            if (db.TryGetValue(key, out _))
            {
                db.Remove(key);
                Save();
            }
        }


        /// <summary>
        /// Список IP адресов с разрешенным доступом
        /// </summary>
        public static List<AccessIPModel> List()
        {
            var mass = new List<AccessIPModel>();
            foreach (var item in db)
                mass.AddRange(item.Value.FindAll(i => i.Expires > DateTime.Now));

            return mass;
        }


        /// <summary>
        /// Очистка базы
        /// </summary>
        public static void Clear()
        {
            var memoryCache = Service.Get<IMemoryCache>();
            if (!memoryCache.TryGetValue("Cron_AccessIP-Clear", out _))
            {
                memoryCache.Set("Cron_AccessIP-Clear", (byte)1, TimeSpan.FromHours(1));
                List<string> keyRemove = new List<string>();

                // Удаляем ненужные Value
                foreach (var item in db)  {
                    item.Value.RemoveAll(i => i.Expires < DateTime.Now);
                    if (item.Value.Count == 0)
                        keyRemove.Add(item.Key);
                }

                // Удаляем ключи с пустым Value
                foreach (var key in keyRemove) {
                    db.Remove(key);
                }

                // Сохраняем базу
                Save();
            }
        }
    }
}
