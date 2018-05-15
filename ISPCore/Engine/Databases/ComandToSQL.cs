using System;

namespace ISPCore.Engine.Databases
{
    public class ComandToSQL
    {
        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="NameDB">Имя таблицы</param>
        /// <param name="Id">Id записи</param>
        public static string Delete(string NameDB, int Id) => $"DELETE FROM \"{NameDB}\" WHERE \"Id\" = {Id}";
    }
}
