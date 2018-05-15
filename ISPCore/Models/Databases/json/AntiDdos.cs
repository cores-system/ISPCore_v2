namespace ISPCore.Models.Databases.json
{
    public class AntiDdos
    {
        private string _CheckPorts, _Interface;
        private int _BlockingTime, _numberOfRequestsIn120Second, _maximumBurstSize;

        /// <summary>
        /// Включить/Выключить AntiDdos
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Интерфейс (eth0/ens33)
        /// </summary>
        public string Interface
        {
            get
            {
                if (_Interface != null)
                    return _Interface;

                return @"any";
            }
            set { _Interface = value; }

        }
        /// <summary>
        /// Список проверяемых портов
        /// </summary>
        public string CheckPorts
        {
            get
            {
                if (_CheckPorts != null)
                    return _CheckPorts;

                return @"80,443,53";
            }
            set { _CheckPorts = value; }
        }

        /// <summary>
        /// Вести журнал заблокированых IP
        /// </summary>
        public bool Jurnal { get; set; }

        /// <summary>
        /// Записывать информацию GeoIP об IP
        /// </summary>
        public bool GeoIP { get; set; }

        /// <summary>
        /// Проверять DNSLookup на белый список
        /// </summary>
        public bool DNSLookupEnabled { get; set; }

        /// <summary>
        /// Выполнять блокировку в IPtables или нет
        /// </summary>
        public bool BlockToIPtables { get; set; } = true;

        /// <summary>
        /// Время блокировки в минутах
        /// </summary>
        public int BlockingTime
        {
            get { return _BlockingTime == 0 ? 15 : _BlockingTime; }
            set { _BlockingTime = value; }
        }

        /// <summary>
        /// Режим активной блокировки
        /// </summary>
        public bool ActiveLockMode { get; set; } = true;

        /// <summary>
        /// Максимальное количиство запросов за 120 секунд
        /// </summary>
        public int NumberOfRequestsIn120Second
        {
            get { return _numberOfRequestsIn120Second == 0 ? 400 : _numberOfRequestsIn120Second; }
            set { _numberOfRequestsIn120Second = value; }
        }

        /// <summary>
        /// Максимально допустимый размер 'TCP/UDP' 
        /// </summary>
        public int MaximumBurstSize
        {
            get { return _maximumBurstSize == 0 ? 80 : _maximumBurstSize; }
            set { _maximumBurstSize = value; }
        }
    }
}
