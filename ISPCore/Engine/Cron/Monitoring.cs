using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using ISPCore.Models.Databases;
using Microsoft.EntityFrameworkCore;
using ISPCore.Engine.Databases;
using ISPCore.Models.RequestsFilter.Monitoring;
using ISPCore.Models.Base;
using ISPCore.Engine.Base.SqlAndCache;

namespace ISPCore.Engine.Cron
{
    public class Monitoring
    {
        static bool IsRun = false;
        public static void Run(CoreDB coreDB, IMemoryCache memoryCache)
        {
            if (IsRun)
                return;
            IsRun = true;

            #region IspNumberOfRequestToHour
            // Если есть кеш за прошлый час
            var TimeIspNumberOfRequestDay = DateTime.Now.AddHours(-1);
            if (memoryCache.TryGetValue(KeyToMemoryCache.IspNumberOfRequestToHour(TimeIspNumberOfRequestDay), out IDictionary<string, NumberOfRequestHour> DataNumberOfRequestToHour))
            {
                SqlToMode.SetMode(SqlMode.Read);

                // Записываем данные в базу
                foreach (var item in DataNumberOfRequestToHour)
                {
                    coreDB.RequestsFilter_NumberOfRequestDay.Add(new NumberOfRequestDay()
                    {
                        Host = item.Key,
                        Time = TimeIspNumberOfRequestDay,
                        Count200 = item.Value.Count200,
                        Count303 = item.Value.Count303,
                        Count401 = item.Value.Count401,
                        Count403 = item.Value.Count403,
                        Count500 = item.Value.Count500,
                        Count2FA = item.Value.Count2FA,
                        CountIPtables = item.Value.CountIPtables,
                    });
                }

                // Сохраняем базу
                coreDB.SaveChanges();

                // Разрешаем записывать данные в SQL
                SqlToMode.SetMode(SqlMode.ReadOrWrite);

                // Сносим кеш (статистика за час)
                memoryCache.Remove(KeyToMemoryCache.IspNumberOfRequestToHour(TimeIspNumberOfRequestDay));

                // Раз в час
                GC.Collect(GC.MaxGeneration);
            }
            #endregion

            #region Очистка баз + перенос NumberOfRequestDay в NumberOfRequestMonth
            if (memoryCache.TryGetValue("CronIspClearDB", out DateTime CronIspClearDB))
            {
                // Если дата отличается от текущей
                if (CronIspClearDB.Day != DateTime.Now.Day)
                {
                    SqlToMode.SetMode(SqlMode.Read);

                    // Обновляем кеш
                    memoryCache.Set("CronIspClearDB", DateTime.Now);

                    #region Очищаем NumberOfRequestMonth
                    foreach (var item in coreDB.RequestsFilter_NumberOfRequestMonth.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.RequestsFilter_NumberOfRequestMonth), item.Id));
                    }
                    #endregion

                    #region Очищаем Jurnals200
                    foreach (var item in coreDB.RequestsFilter_Jurnals200.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.RequestsFilter_Jurnals200), item.Id));
                    }
                    #endregion

                    #region Очищаем Jurnals303
                    foreach (var item in coreDB.RequestsFilter_Jurnals303.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.RequestsFilter_Jurnals303), item.Id));
                    }
                    #endregion

                    #region Очищаем Jurnals403
                    foreach (var item in coreDB.RequestsFilter_Jurnals403.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.RequestsFilter_Jurnals403), item.Id));
                    }
                    #endregion

                    #region Очищаем Jurnals401
                    foreach (var item in coreDB.RequestsFilter_Jurnals401.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.RequestsFilter_Jurnals401), item.Id));
                    }
                    #endregion

                    #region Очищаем Jurnals2FA
                    foreach (var item in coreDB.RequestsFilter_Jurnals2FA.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.RequestsFilter_Jurnals2FA), item.Id));
                    }
                    #endregion

                    #region Очищаем Jurnals500
                    foreach (var item in coreDB.RequestsFilter_Jurnals500.AsNoTracking())
                    {
                        // Если записи больше 90 дней
                        if ((DateTime.Now - item.Time).TotalDays > 90)
                            coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.RequestsFilter_Jurnals500), item.Id));
                    }
                    #endregion

                    #region Переносим NumberOfRequestDay в NumberOfRequestMonth
                    // Хранимм дату и значение
                    Dictionary<int, NumberOfRequestBase> NumberOfRequestMonth = new Dictionary<int, NumberOfRequestBase>();

                    // Собираем статистику за прошлые дни
                    foreach (var item in coreDB.RequestsFilter_NumberOfRequestDay.AsNoTracking())
                    {
                        // Пропускаем статистику за сегодня
                        if (item.Time.Day == DateTime.Now.Day && item.Time.Month == DateTime.Now.Month)
                            continue;

                        #region Переносим значения в NumberOfRequestMonth
                        if (NumberOfRequestMonth.TryGetValue(item.Time.Day, out NumberOfRequestBase it))
                        {
                            it.Count200 += item.Count200;
                            it.Count303 += item.Count303;
                            it.Count403 += item.Count403;
                            it.Count401 += item.Count401;
                            it.Count500 += item.Count500;
                            it.Count2FA += item.Count2FA;
                            it.CountIPtables += item.CountIPtables;
                        }
                        else
                        {
                            NumberOfRequestMonth.Add(item.Time.Day, item);
                        }
                        #endregion

                        // Удаляем значения из базы
                        coreDB.Database.ExecuteSqlCommand(ComandToSQL.Delete(nameof(coreDB.RequestsFilter_NumberOfRequestDay), item.Id));
                    }

                    // Переносим временные данные с NumberOfRequestMonth в базу
                    foreach (var item in NumberOfRequestMonth)
                    {
                        // Добовляем в базу
                        coreDB.RequestsFilter_NumberOfRequestMonth.Add(new NumberOfRequestMonth()
                        {
                            Time = item.Value.Time,
                            allRequests = item.Value.Count200 + item.Value.Count303 + item.Value.Count403 + item.Value.Count500 + item.Value.Count401 + item.Value.Count2FA + item.Value.CountIPtables,
                            Count200 = item.Value.Count200,
                            Count303 = item.Value.Count303,
                            Count401 = item.Value.Count401,
                            Count403 = item.Value.Count403,
                            Count500 = item.Value.Count500,
                            Count2FA = item.Value.Count2FA,
                            CountIPtables = item.Value.CountIPtables
                        });
                    }
                    #endregion

                    // Сохраняем базу
                    coreDB.SaveChanges();
                    SqlToMode.SetMode(SqlMode.ReadOrWrite);

                    // Раз в день
                    GC.Collect(GC.MaxGeneration);
                }
            }
            else
            {
                // Создаем кеш задним числом
                memoryCache.Set("CronIspClearDB", DateTime.Now.AddDays(-1));
            }
            #endregion

            IsRun = false;
        }
    }
}
