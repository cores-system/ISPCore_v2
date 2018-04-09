using System;

namespace ISPCore.Engine.Databases
{
    public class ComandToSQL
    {
        public static string Delete(string NameDB, int Id) => $"DELETE FROM \"{NameDB}\" WHERE \"Id\" = {Id}";
    }
}
