namespace ISPCore.Models.Triggers
{
    public class Subscription
    {
        /// <summary>
        /// core.CheckLink
        /// </summary>
        public string TrigerPath { get; set; }

        /// <summary>
        /// RequestToMinute
        /// </summary>
        public string TrigerName { get; set; }

        /// <summary>
        /// Первый триггер с условием
        /// </summary>
        public string StepId { get; set; }
    }
}
