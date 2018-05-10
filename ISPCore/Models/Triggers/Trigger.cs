namespace ISPCore.Models.Triggers
{
    public class Trigger
    {
        /// <summary>
        /// Условие
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Что делать дальше
        /// </summary>
        public ReturnType returnType { get; set; }

        /// <summary>
        /// Ссылки на следующие условия
        /// </summary>
        public string NextSteps { get; set; }
    }
}
