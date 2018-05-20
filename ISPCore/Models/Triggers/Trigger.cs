using System.Collections.Generic;

namespace ISPCore.Models.Triggers
{
    public class Trigger
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Позиция обьекта
        /// </summary>
        public Position position { get; set; } = new Position();

        /// <summary>
        /// Условие
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// System, System.IO
        /// </summary>
        public List<string> Namespaces { get; set; } = new List<string>();

        /// <summary>
        /// Newtonsoft.Json.dll
        /// </summary>
        public List<string> References { get; set; } = new List<string>();

        /// <summary>
        /// Что делать дальше
        /// </summary>
        public ReturnType returnType => string.IsNullOrWhiteSpace(NextSteps) ? ReturnType.exit : ReturnType.NextStep;

        /// <summary>
        /// Ссылки на следующие условия
        /// </summary>
        public string NextSteps { get; set; }
    }
}
