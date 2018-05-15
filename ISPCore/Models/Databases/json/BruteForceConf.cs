namespace ISPCore.Models.Databases.json
{
    public class BruteForceConf
    {
        private int _minuteLimit, _hourLimit, _dayLimit;

        /// <summary>
        /// Максимальное количиство запросов в минуту 
        /// </summary>
        public int MinuteLimit
        {
            get
            {
                if (_minuteLimit <= 0)
                    return 10;

                return _minuteLimit;
            }
            set { _minuteLimit = value; }
        }

        /// <summary>
        /// Максимальное количиство запросов за час 
        /// </summary>
        public int HourLimit
        {
            get
            {
                if (_hourLimit <= 0)
                    return 30;

                return _hourLimit;
            }
            set { _hourLimit = value; }
        }

        /// <summary>
        /// Максимальное количиство запросов за сутки 
        /// </summary>
        public int DayLimit
        {
            get
            {
                if (_dayLimit <= 0)
                    return 120;

                return _dayLimit;
            }
            set { _dayLimit = value; }
        }
    }
}
