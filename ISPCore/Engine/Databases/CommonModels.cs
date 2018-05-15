using System;
using ISPCore.Models.Databases.Interface;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using ISPCore.Models.Databases.Enums;

namespace ISPCore.Engine.Databases
{
    public class CommonModels
    {
        /// <summary>
        /// Обновить данные
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="oldItem">Исходные данные</param>
        /// <param name="newItem">Новые данные</param>
        /// <param name="HttpContext"></param>
        /// <param name="updateType">Метод обновления исходных данных</param>
        public static void Update<T>(T oldItem, T newItem, HttpContext HttpContext, UpdateType updateType = UpdateType.update) where T : class
        {
            Update(oldItem, newItem, $"^({string.Join('|', HttpContext.Request.Query.Keys)})$", updateType);
        }


        /// <summary>
        /// Обновить данные
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="oldItem">Исходные данные</param>
        /// <param name="newItem">Новые данные</param>
        /// <param name="pattern">Regex для игнорирования отдельный полей</param>
        /// <param name="updateType">Метод обновления исходных данных</param>
        public static void Update<T>(T oldItem, T newItem, string pattern = "^$", UpdateType updateType = UpdateType.Default) where T : class
        {
            // Получаем все поля "{get; set;}"
            foreach (var filed in typeof(T).GetProperties())
            {
                // Id оставляем прежним
                if (filed.Name.Contains("Id"))
                    continue;

                #region Пропускаем поля
                // Пропускаем поля указаные в pattern
                if (updateType == UpdateType.skip && Regex.IsMatch(filed.Name, pattern, RegexOptions.IgnoreCase))
                    continue;

                // Пропускаем все поля кроме тех что указаны в pattern
                if (updateType == UpdateType.update && !Regex.IsMatch(filed.Name, pattern, RegexOptions.IgnoreCase))
                    continue;
                #endregion

                dynamic oldValue = filed.GetValue(oldItem);            // Получаем значения oldItem
                dynamic newValue = filed.GetValue(newItem);            // Получаем значения newItem

                // Обрабатываем обычные поля и Enum
                if (filed.PropertyType.IsPrimitive || filed.PropertyType.IsEnum || (Type.GetTypeCode(filed.PropertyType) is TypeCode typeCode && (typeCode == TypeCode.DateTime || typeCode == TypeCode.String)))
                {
                    if (oldValue != newValue)                 // Сравниваем значения
                        filed.SetValue(oldItem, newValue);    // Заменяем старые значения на новые
                }

                // Обрабатываем классы которые потдерживают интерфейс IUpdate
                else if (updateType == UpdateType.Default && filed.PropertyType.GetInterface(nameof(IUpdate)) != null)
                {
                    if (oldValue is IUpdate up)
                    {
                        // Вызываем метод "Update" в классе
                        up.Update(newValue);
                    }
                    else
                    {
                        // Присваиваем новый класс
                        filed.SetValue(oldItem, newValue);
                    }
                }
            }
        }
    }
}
