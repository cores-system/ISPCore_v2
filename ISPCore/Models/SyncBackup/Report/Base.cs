using ISPCore.Models.SyncBackup.Tasks;
using System;
using System.Collections.Generic;

namespace ISPCore.Models.SyncBackup.Report
{
    public class BaseItem
    {
        /// <summary>
        /// Имя метода
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Имя аргументов и значения в них
        /// </summary>
        public object ArgNameAndValue { get; set; }

        /// <summary>
        /// Время создания отчета по данной ошибке
        /// </summary>
        public DateTime time { get; private set; } = DateTime.Now;

        /// <summary>
        /// Ответ уделаного сервера или ошибка 'try catch'
        /// </summary>
        public object Response { get; set; }
    }


    public class Base
    {
        /// <summary>
        /// Финальный отчет
        /// </summary>
        /// <param name="_task">Данные задания</param>
        /// <param name="_Report">Отчет по ошибкам</param>
        public Base(Task _task, List<BaseItem> _Report)
        {
            task = _task;
            Report = _Report;
        }

        /// <summary>
        /// Данные задания
        /// </summary>
        public Task task { get; private set; }

        /// <summary>
        /// Отчет по ошибкам
        /// </summary>
        public List<BaseItem> Report { get; private set; }
    }
}
