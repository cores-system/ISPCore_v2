namespace ISPCore.Models.Triggers
{
    public class Subscription
    {
        /// <summary>
        /// Позиция обьекта
        /// </summary>
        public Position position { get; set; } = new Position();

        /// <summary>
        /// core.CheckRequest
        /// </summary>
        public string TrigerPath { get; set; }

        /// <summary>
        /// RequestToMinute
        /// </summary> 
        public string TrigerName { get; set; }

        /// <summary>
        /// Первые триггеры с условием
        /// </summary>
        public string StepIds { get; set; } = string.Empty;
    }
}
