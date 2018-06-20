using System.Collections.Concurrent;
using ISPCore.Models.RequestsFilter.Monitoring;
using System;
using System.Text;
using IO = System.IO;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using ISPCore.Models.Base;
using ISPCore.Models.Databases;
using System.IO;
using System.Diagnostics;
using ISPCore.Models.Databases.json;
using System.Web;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.Base.SqlAndCache
{
    public static class WriteLogTo
    {
        #region FileStream
        /// <summary>
        /// Колекция открытых поток для записи в файл
        /// </summary>
        static ConcurrentDictionary<string, (DateTime LastWrite, IO.FileStream Stream)> dbFile = new ConcurrentDictionary<string, (DateTime LastWrite, IO.FileStream Stream)>();

        #region FileStream
        /// <summary>
        /// Записать данные журнала в файл
        /// </summary>
        /// <param name="jurn">Jurnal2FA</param>
        public static void FileStreamTo2faAuth(Jurnal2FA jurn)
        {
            WriteToFileStream(jurn.Host, "2FA", jurn.Time, jurn.IP, jurn.UserAgent, jurn.Method, jurn.Uri, jurn.FormData, jurn.Referer, jurn.Msg, jurn.Country, jurn.Region, jurn.City);
        }

        /// <summary>
        /// Записать данные журнала в файл
        /// </summary>
        /// <param name="jurn">Jurnal500</param>
        public static void FileStream(Jurnal500 jurn)
        {
            WriteToFileStream(jurn.Host, "500", jurn.Time, jurn.IP, jurn.UserAgent, jurn.Method, jurn.Uri, jurn.FormData, jurn.Referer, jurn.ErrorMsg);
        }

        /// <summary>
        /// Записать данные журнала в файл
        /// </summary>
        /// <param name="jurn">Jurnal403</param>
        public static void FileStream(Jurnal403 jurn)
        {
            WriteToFileStream(jurn.Host, "403", jurn.Time, jurn.IP, jurn.UserAgent, jurn.Method, jurn.Uri, jurn.FormData, jurn.Referer, null, jurn.Country, jurn.Region, jurn.City);
        }

        /// <summary>
        /// Записать данные журнала в файл
        /// </summary>
        /// <param name="jurn">Jurnal403</param>
        public static void FileStream(Jurnal401 jurn)
        {
            WriteToFileStream(jurn.Host, "401", jurn.Time, jurn.IP, jurn.UserAgent, null, null, null, null, jurn.Msg, jurn.Country, jurn.Region, jurn.City, jurn.Ptr);
        }

        /// <summary>
        /// Записать данные журнала в файл
        /// </summary>
        /// <param name="jurn">Jurnal303</param>
        public static void FileStream(Jurnal303 jurn)
        {
            WriteToFileStream(jurn.Host, "303", jurn.Time, jurn.IP, jurn.UserAgent, jurn.Method, jurn.Uri, jurn.FormData, jurn.Referer, null, jurn.Country, jurn.Region, jurn.City);
        }

        /// <summary>
        /// Записать данные журнала в файл
        /// </summary>
        /// <param name="jurn">JurnalAntiBot</param>
        public static void FileStream(Jurnal200 jurn)
        {
            string type = "unknown";
            switch (jurn.typeJurn)
            {
                case TypeJurn200.AntiBot:
                    type = "200 - AntiBot";
                    break;
                case TypeJurn200._2FA:
                    type = "200 - 2FA";
                    break;
                case TypeJurn200.IPtables:
                    type = "200 - IPtables";
                    break;
            }

            WriteToFileStream(jurn.Host, type, jurn.Time, jurn.IP, jurn.UserAgent, jurn.Method, jurn.Uri, jurn.FormData, jurn.Referer, null, jurn.Country, jurn.Region, jurn.City);
        }
        #endregion

        #region WriteToFileStream
        /// <summary>
        /// Конвертируем данные для записи в WriteToFileStream
        /// </summary>
        /// <param name="host">Домен</param>
        /// <param name="StatusCode">303/403/500/2FA</param>
        /// <param name="Time">Время записи</param>
        /// <param name="IP">IP пользователя</param>
        /// <param name="UserAgent">UserAgent пользователя</param>
        /// <param name="Method">Метод запроса</param>
        /// <param name="Uri">URL запроса</param>
        /// <param name="FormData">Даные POST запроса</param>
        /// <param name="Referer">Реффер</param>
        /// <param name="Msg">Сообщение для кода 500/2FA</param>
        /// <param name="Country">Страна</param>
        /// <param name="Region">Регион</param>
        /// <param name="City">Город</param>
        private static void WriteToFileStream(string host, string StatusCode, DateTime Time, string IP, string UserAgent, string Method, string Uri, string FormData, string Referer, 
                                             string Msg = null, string Country = null, string Region = null, string City = null, string Ptr = null)
        {
            // Запрет на запись логов
            if (Service.Get<JsonDB>().Base.DisableWriteLog)
                return;

            #region geoIP
            string geoIP = "";
            if (Country != null || Region != null || City != null)
                geoIP = $"Country: {Country} / Region: {Region} - City: {City}";
            #endregion

            if (!string.IsNullOrWhiteSpace(FormData))
                FormData = HttpUtility.UrlEncode(FormData);

            string msg = Msg == null ? "" : $"Msg: {Msg}";
            WriteToFileStream(host, Encoding.UTF8.GetBytes($"\"{Time.ToString()}\" - \"{IP}\" - \"{Ptr}\" - \"{geoIP}\" - \"{UserAgent}\" - \"{StatusCode}\" - \"{host}\" - \"{Method}: {Uri}\" - \"{FormData}\" - \"{Referer}\" - \"{msg}\"" + Environment.NewLine));
        }


        /// <summary>
        /// Запись да́нных в поток FileStream
        /// </summary>
        /// <param name="host">Домен</param>
        /// <param name="buffer">Данные в byte[]</param>
        private static void WriteToFileStream(string host, byte[] buffer)
        {
            #region Получем/Регистрируем поток
            (DateTime LastWrite, IO.FileStream Stream) data = (DateTime.Now, null);
            if (!dbFile.TryGetValue(host, out data))
            {
                data.Stream = new IO.FileStream($"{Folders.LogRequests}/{DateTime.Now.ToString("dd.MM.yyyy")}.log", IO.FileMode.Append, IO.FileAccess.Write);
                dbFile.TryAdd(host, data);
            }
            #endregion

            // Записываем данные в поток
            data.Stream.Write(buffer, 0, buffer.Length);
            data.Stream.Flush();
            data.LastWrite = DateTime.Now;
        }
        #endregion

        #region CloseFiles
        /// <summary>
        /// Автоматическое закрытие потоков на файлах которые не используются
        /// </summary>
        public static void CloseFiles() 
        {
            var memoryCache = Service.Get<IMemoryCache>();
            if (!memoryCache.TryGetValue("Cron_LogRequestsTo-AutoCloseFile", out _))
            {
                memoryCache.Set("Cron_LogRequestsTo-AutoCloseFile", (byte)1, TimeSpan.FromMinutes(10));

                try
                {
                    foreach (var item in dbFile)
                    {
                        if (DateTime.Now.AddMinutes(-60) > item.Value.LastWrite)
                        {
                            item.Value.Stream.Close();
                            dbFile.Remove(item.Key, out _);
                        }
                    }
                }
                catch { }
            }
        }
        #endregion

        #region ZipFiles
        /// <summary>
        /// Сжатие логов
        /// </summary>
        public static void ZipFiles()
        {
            var memoryCache = Service.Get<IMemoryCache>();
            if (!memoryCache.TryGetValue("Cron_LogRequestsTo-AutoZipFiles", out _))
            {
                memoryCache.Set("Cron_LogRequestsTo-AutoZipFiles", (byte)1, TimeSpan.FromHours(3));

                foreach (var inFile in Directory.GetFiles(Folders.LogRequests, "*.log"))
                {
                    // Текущие логи
                    if (inFile.Contains($"{DateTime.Now.ToString("dd.MM.yyyy")}.log"))
                        continue;

                    // Сжатие
                    GZip.Compress(inFile, Regex.Replace(inFile, @"\.log$", ".gz"));

                    // Удаляем оригинальный лог
                    File.Delete(inFile);
                }
            }
        }
        #endregion
        #endregion

        #region SQL
        /// <summary>
        /// BlockedsIP
        /// </summary>
        static MyConcurrentQueue<BlockedIP> BlockedsIP = new MyConcurrentQueue<BlockedIP>();
        public static void SQL(BlockedIP jurn) => BlockedsIP.Enqueue(jurn);

        /// <summary>
        /// Home_Jurnals
        /// </summary>
        static MyConcurrentQueue<Models.Home.Jurnal> JurnalsHome = new MyConcurrentQueue<Models.Home.Jurnal>();
        public static void SQL(Models.Home.Jurnal jurn) => JurnalsHome.Enqueue(jurn);

        /// <summary>
        /// AntiDdos_Jurnals
        /// </summary>
        static MyConcurrentQueue<Models.Security.AntiDdos.Jurnal> JurnalsAntiDdos = new MyConcurrentQueue<Models.Security.AntiDdos.Jurnal>();
        public static void SQL(Models.Security.AntiDdos.Jurnal jurn) => JurnalsAntiDdos.Enqueue(jurn);

        /// <summary>
        /// RequestsFilter_Jurnals2FA
        /// </summary>
        static MyConcurrentQueue<Jurnal2FA> Jurnals2FA = new MyConcurrentQueue<Jurnal2FA>();
        public static void SQL(Jurnal2FA jurn) => Jurnals2FA.Enqueue(jurn);

        /// <summary>
        /// RequestsFilter_Jurnals500
        /// </summary>
        static MyConcurrentQueue<Jurnal500> Jurnals500 = new MyConcurrentQueue<Jurnal500>();
        public static void SQL(Jurnal500 jurn) => Jurnals500.Enqueue(jurn);

        /// <summary>
        /// RequestsFilter_Jurnals403
        /// </summary>
        static MyConcurrentQueue<Jurnal403> Jurnals403 = new MyConcurrentQueue<Jurnal403>();
        public static void SQL(Jurnal403 jurn) => Jurnals403.Enqueue(jurn);


        /// <summary>
        /// RequestsFilter_Jurnals401
        /// </summary>
        static MyConcurrentQueue<Jurnal401> Jurnals401 = new MyConcurrentQueue<Jurnal401>();
        public static void SQL(Jurnal401 jurn) => Jurnals401.Enqueue(jurn);

        /// <summary>
        /// RequestsFilter_Jurnals303
        /// </summary>
        static MyConcurrentQueue<Jurnal303> Jurnals303 = new MyConcurrentQueue<Jurnal303>();
        public static void SQL(Jurnal303 jurn) => Jurnals303.Enqueue(jurn);

        /// <summary>
        /// RequestsFilter_Jurnals200
        /// </summary>
        static MyConcurrentQueue<Jurnal200> Jurnals200 = new MyConcurrentQueue<Jurnal200>();
        public static void SQL(Jurnal200 jurn) => Jurnals200.Enqueue(jurn);



        private static bool IsWriteLogToSqlRun = false;
        public static void WriteLogToSql(object ob)
        {
            if (IsWriteLogToSqlRun || SqlToMode.Mode == SqlMode.Read)
                return;
            IsWriteLogToSqlRun = true;

            try
            {
                #region BlockedsIP
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !BlockedsIP.IsEmpty && BlockedsIP.TryDequeue(out BlockedIP i))
                {
                    coreDB.BlockedsIP.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion

                #region Home_Jurnals
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !JurnalsHome.IsEmpty && JurnalsHome.TryDequeue(out Models.Home.Jurnal i))
                {
                    coreDB.Home_Jurnals.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion

                #region RequestsFilter_Jurnals2FA
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !Jurnals2FA.IsEmpty && Jurnals2FA.TryDequeue(out Jurnal2FA i))
                {
                    coreDB.RequestsFilter_Jurnals2FA.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion

                #region RequestsFilter_Jurnals500
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !Jurnals500.IsEmpty && Jurnals500.TryDequeue(out Jurnal500 i))
                {
                    coreDB.RequestsFilter_Jurnals500.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion

                #region RequestsFilter_Jurnals403
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !Jurnals403.IsEmpty && Jurnals403.TryDequeue(out Jurnal403 i))
                {
                    coreDB.RequestsFilter_Jurnals403.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion

                #region RequestsFilter_Jurnals401
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !Jurnals401.IsEmpty && Jurnals401.TryDequeue(out Jurnal401 i))
                {
                    coreDB.RequestsFilter_Jurnals401.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion

                #region AntiDdos_Jurnals
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !JurnalsAntiDdos.IsEmpty && JurnalsAntiDdos.TryDequeue(out Models.Security.AntiDdos.Jurnal i))
                {
                    coreDB.AntiDdos_Jurnals.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion

                #region RequestsFilter_Jurnals303
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !Jurnals303.IsEmpty && Jurnals303.TryDequeue(out Jurnal303 i))
                {
                    coreDB.RequestsFilter_Jurnals303.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion

                #region RequestsFilter_Jurnals200
                while (SqlToMode.Mode == SqlMode.ReadOrWrite && !Jurnals200.IsEmpty && Jurnals200.TryDequeue(out Jurnal200 i))
                {
                    coreDB.RequestsFilter_Jurnals200.Add(i);
                    coreDB.SaveChanges();
                }
                #endregion
            }
            catch (Exception ex)
            {
                try
                {
                    File.AppendAllText(Folders.File.SystemErrorLog, ex.ToString() + "\n\n=======================================================================\n\n");
                }
                catch { }
            }

            IsWriteLogToSqlRun = false;
        }
        #endregion

        #region CoreDB
        private static CoreDB _coreDB = null;
        private static CoreDB coreDB
        {
            get
            {
                if (_coreDB == null) {
                    _coreDB = Service.Get<CoreDB>();
                    _coreDB.ChangeTracker.AutoDetectChangesEnabled = false;
                }
                
                return _coreDB;
            }
        }
        #endregion

        #region MyConcurrentQueue
        private class MyConcurrentQueue<T> : ConcurrentQueue<T>
        {
            /// <summary>
            /// Добавить значение
            /// </summary>
            /// <param name="item">значение</param>
            new public void Enqueue(T item)
            {
                // Запрет на запись логов
                if (Service.Get<JsonDB>().Base.DisableWriteLog)
                    return;

                // Записываем данные в память
                base.Enqueue(item);
            }
        }
        #endregion
    }
}
